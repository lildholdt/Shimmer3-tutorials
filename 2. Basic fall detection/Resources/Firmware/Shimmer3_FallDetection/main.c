#include <stdint.h>
#include "msp430.h"
#include "hal_pmm.h"
#include "hal_UCS.h"
#include "hal_Board.h"
#include "hal_Button.h"
#include "RN42.h"
#include "lsm303dlhc.h"
#include "math.h"
#include "shimmer.h"

//Method declarations
void Init(void);
uint8_t BtDataAvailable(uint8_t data);
void StartSensing(void);
void StopSensing(void);
void AcquireData(void);
void ProcessData(void);

// Defines
#define NUMBER_OF_SAMPLES 		50
#define FALL_THRESHOLD			4000
#define SAMPLE_FREQUENCY		50

// Arrays
uint8_t accel_8bit[6];					// Raw 8 bit values samples from the accelerometer
int16_t accel_16bit[3];					// Converted 16 bit values from accel_8bit[]
float sampleBuff[NUMBER_OF_SAMPLES];	// All normalized accelerations

// Variables
float accel_normalized;					// Normalized acceleration
uint8_t btIsConnected,					// Flag to check if Bluetooth is connected
		sampleCounter,					// Number of current samples
		i;								// Counter used in for loops

void main(void) {

	Init();

	while(1) {
		__bis_SR_register(LPM3_bits);	//ACLK remains active
	}
}

void Init(void) {

	// Global variables
	btIsConnected = 0;		// Bluetooth is not connected initially
	sampleCounter = 0;		// No samples acquired initially

	// Init MSP430
	Board_init();

	//Set Vcore to maximum
	SetVCore(3);

	// Start 32.768kHz XTAL as ACLK
	//LFXT_Start(XT1DRIVE_0);

	// Start 24MHz XTAL as MCLK and SMCLK
	XT2_Start(XT2DRIVE_2);        // XT2DRIVE_2 or XTDRIVE_3 for 24MHz (userguide section 5.4.7)
	UCSCTL4 |= SELS_5 + SELM_5;   // SMCLK=MCLK=XT2

	// Enable switch1 interrupt
	Button_init();
	Button_interruptEnable();

	// Enable Bluetooth
	BT_init();
	BT_disableRemoteConfig(1);
	BT_setRadioMode(SLAVE_MODE);
	BT_setFriendlyName("Shimmer3");
	//BT_setAuthentication(4);
	BT_setPIN("1234");
	BT_configure();
	BT_receiveFunction(&BtDataAvailable);

	//Globally enable interrupts
	_enable_interrupts();

	// Setup of LSM303DLHC
	LSM303DLHC_init();
	LSM303DLHC_accelInit(LSM303DLHC_ACCEL_100HZ,ACCEL_16G,0,1);

	// Start sample timer
	StartSensing();
}

uint8_t BtDataAvailable(uint8_t data) {
	//Board_ledToggle(LED_RED);
	return 1;
}

void StartSensing(void) {
	TB0CCR4 = (32768 / SAMPLE_FREQUENCY);
	TB0CCTL4 = CCIE;
	TB0CTL = TBSSEL_1 + MC_2 + TBCLR;
}

void StopSensing(void) {
	TB0CTL = MC0;
}

// Interrupt Service Routine
// Handles bluetooth connection
#pragma vector=PORT1_VECTOR
__interrupt void Port1_ISR(void)
{
    // Context save interrupt flag before calling interrupt vector.
    // Reading interrupt vector generator will automatically clear IFG flag
	switch (__even_in_range(P1IV, P1IV_P1IFG7))
    {
		//BT Connect/Disconnect
		case  P1IV_P1IFG0:   //BT Connect/Disconnect
	      if(P1IN & BIT0) {
	         //BT is connected
	         P1IES |= BIT0; //look for falling edge
	         BT_connectionInterrupt(1);
	         btIsConnected = 1;
	         Board_ledOn(LED_BLUE);
	      } else {
	         //BT is not connected
	         P1IES &= ~BIT0; //look for rising edge
	         BT_connectionInterrupt(0);
	         btIsConnected = 0;
	         Board_ledOff(LED_BLUE);
	      }
	      break;

	      //BT RTS
	      case  P1IV_P1IFG3:
	         if(P1IN & BIT3) {
	            P1IES |= BIT3;    //look for falling edge
	            BT_rtsInterrupt(1);
	         } else {
	            P1IES &= ~BIT3;   //look for rising edge
	            BT_rtsInterrupt(0);
	         }
	         break;

        // Default case
        default:
            break;
    }
}

// Interrupt Service Routine
// Handles acquition of samples from the accelerometer and status blink of LED
#pragma vector=TIMER0_B1_VECTOR
__interrupt void TIMER0_B1_ISR(void) {
   switch(__even_in_range(TB0IV,14)) {
   case  0: break;                     	// No interrupt
   case  2: break;                     	// TB0CCR1
   case  4: break;                     	// TB0CCR2
   case  6:								// TB0CCR3
	   break;
   case  8:                            	// TB0CCR4
	   // Set CCR
	   TB0CCR4 += (32768 / SAMPLE_FREQUENCY);
	   AcquireData();
	   break;

   case 10: break;                       // reserved
   case 12: break;                       // reserved
   case 14: break;                       // TBIFG overflow handler
   }
}

void AcquireData(void) {

	if (sampleCounter < NUMBER_OF_SAMPLES) {

		// Read sample from LSM303DLHC accelerometer
		LSM303DLHC_getAccel(accel_8bit);

		// 6 uint8_t are read from the accelerometer
		// These are converter into 3 int16_t values
		// Structure of accelBuff: [X_LSB X_MSB Y_LSB Y_MSB Z_LSB Z_MSB]
		for (i = 0; i < 3; i++) {
			accel_16bit[i] = ((int16_t) accel_8bit[(2 * i) + 1] << 8) | (accel_8bit[2 * i] & 0xff);
		}

		// Calculating normalized acceleration
		// sqrt(X^2 + Y^2 + Z^2)
		accel_normalized = pow(accel_16bit[0], 2) + pow(accel_16bit[1], 2) + pow(accel_16bit[2], 2);
		accel_normalized = sqrt(accel_normalized);

		// Store normalized acceleration
		sampleBuff[sampleCounter] = accel_normalized;
		sampleCounter++;
	}
	else
	{
		// Required number of samples has been acquired. Data processing is performed
		ProcessData();
	}
}

void ProcessData(void) {
	float min = 0, max = 0;

	// Find minimum and maximum of the sample buffer
	min = sampleBuff[0];
	max = sampleBuff[0];
	for (i = 1; i < NUMBER_OF_SAMPLES; i++) {
		if (sampleBuff[i] < min)
			min = sampleBuff[i];
		if (sampleBuff[i] > max)
			max = sampleBuff[i];
	}

	// Check if a fall has occurred
	if ((max - min) > FALL_THRESHOLD) {
		if (btIsConnected) {
			BT_write("F", 1);
		}
	}

	// Reset sample counter
	sampleCounter = 0;
}

// Trap ISR assignation - put all unused ISR vector here
#pragma vector = RTC_VECTOR, USCI_B1_VECTOR, \
	SYSNMI_VECTOR, TIMER0_B0_VECTOR, PORT2_VECTOR, \
	TIMER1_A1_VECTOR, UNMI_VECTOR, USCI_A0_VECTOR, \
	WDT_VECTOR, TIMER0_A1_VECTOR, \
	TIMER0_A0_VECTOR, \
	DMA_VECTOR, ADC12_VECTOR
__interrupt void TrapIsr(void)
{
  // this is a trap ISR - check for the interrupt cause here by
  // checking the interrupt flags, if necessary also clear the interrupt
  // flag
}
