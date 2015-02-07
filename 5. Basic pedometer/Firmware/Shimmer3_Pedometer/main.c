/*-----------------------------------------------------------------------------

	File: 		main.c
	Version:   	1.0
	Created:    07/02/2015
	Author:		Steffan Lildholdt
	Email:     	steffan@lildholdt.dk
	Website:   	steffanlildholdt.dk

-----------------------------------------------------------------------------*/

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
#include "string.h"

//Method declarations
void Init(void);
uint8_t BtDataAvailable(uint8_t data);
void StartSensing(void);
void StopSensing(void);
void AcquireData(void);
void ProcessData(void);
void delay_ms(unsigned int ms);

// Defines
#define PACKET_SIZE        		3
#define NUMBER_OF_SAMPLES 		10
#define SAMPLE_FREQUENCY		10

// Arrays
uint8_t txBuff[PACKET_SIZE];			// Transmit buffer
uint8_t accel_8bit[6];					// Raw 8 bit values samples from the accelerometer
int16_t accel_16bit[3];					// Converted 16 bit values from accel_8bit[]
float sampleBuff[NUMBER_OF_SAMPLES];	// All normalized accelerations

// Variables
uint8_t 		btIsConnected,					// Flag to check if Bluetooth is connected
				sampleCounter,					// Number of current samples
				processing,						// Flag indicating that processing is taking place
				i;								// Counter used in for loops

float 			accel_normalized = 0,			// Normalized acceleration
				lastSample = 0,
				currentSample = 0;

uint8_t 		peakHighDetected = 0,
				peakLowDetected = 0,
				peakPositionDifference = 0;

uint16_t 		numberOfSteps;					// Number of steps taken

void main(void) {

	Init();

	while(1) {
		__bis_SR_register(LPM3_bits + GIE); //ACLK remains active
	}
}

void Init(void) {

	// Global variables
	btIsConnected = 0;		// Bluetooth is not connected initially
	sampleCounter = 0;		// No samples is acquired
	numberOfSteps = 0;		// No steps have been taken initially


	// Init MSP430
	Board_init();


	//Set Vcore to maximum
	SetVCore(3);

	// Start 32.768kHz XTAL as ACLK
	LFXT_Start(XT1DRIVE_0);

	// Start 24MHz XTAL as MCLK and SMCLK
	XT2_Start(XT2DRIVE_2);        // XT2DRIVE_2 or XTDRIVE_3 for 24MHz (userguide section 5.4.7)
	UCSCTL4 |= SELS_5 + SELM_5;   // SMCLK=MCLK=XT2


	// Enable switch1 interrupt
	Button_init();
	Button_interruptEnable();

	//Globally enable interrupts
	_enable_interrupts();


	// Enable Port2 interrupt (Docking)
	if(P2IN & BIT3) {
		//high (docked)
	    P2IES |= BIT3;    //look for falling edge
	} else {
		//low (undocked)
	    P2IES &= ~BIT3;   //look for rising edge
	}
	P2IFG &= ~BIT3;      //clear flag
	P2IE |= BIT3;        //enable interrupt


	// Setup of LSM303DLHC
	LSM303DLHC_init();
	LSM303DLHC_accelInit(LSM303DLHC_ACCEL_50HZ,ACCEL_8G,0,1);

	BT_receiveFunction(&BtDataAvailable);

	// Start sample timer
	StartSensing();
}

uint8_t BtDataAvailable(uint8_t data) {
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

	         // Generate data package
	         txBuff[0] = 'P';
	         txBuff[1] = ((numberOfSteps >> 0) & 0xff);
	         txBuff[2] = ((numberOfSteps >> 8) & 0xff);

	         // Ensure that the BT module has been initialized
	         delay_ms(500);

	         BT_write(txBuff, PACKET_SIZE);

	         numberOfSteps = 0;

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

	      //BUTTON_SW1
	      case  P1IV_P1IFG6:

	    	  break;

        // Default case
        default:
            break;
    }
}

// Interrupt Service Routine
// Handles docking of the Shimmer
#pragma vector=PORT2_VECTOR
__interrupt void Port2_ISR(void) {

   switch (__even_in_range(P2IV, P2IV_P2IFG7)) {
   //DOCK
   case  P2IV_P2IFG3:

	   if(P2IN & BIT3) {

         // Shimmer is docked
    	 P2IES |= BIT3;    //look for falling edge
         Board_ledOn(LED_YELLOW0);

         // Enable Bluetooth
         BT_init();
         BT_setRadioMode(SLAVE_MODE);
         BT_setFriendlyName("Shimmer3");
         //BT_setAuthentication(4);
         //BT_setPIN("1234");
         BT_configure();

      } else {

    	 // Shimmer is NOT docked
         P2IES &= ~BIT3;   //look for rising edge
         Board_ledOff(LED_YELLOW0);

         // Notify PC that the BT connection will be closed
         BT_write("D",1);

         // Ensure that the package has been sent
         delay_ms(500);

         // Close BT connection
         BT_disable();
         BT_connectionInterrupt(0);
         btIsConnected = 0;
         Board_ledOff(LED_BLUE);

      }
      break;

      // Default case
      default: break;
   }
}

// Interrupt Service Routine
// Handles acquisition of samples from the accelerometer
#pragma vector=TIMER0_B1_VECTOR
__interrupt void TIMER0_B1_ISR(void) {
   switch(__even_in_range(TB0IV,14)) {
   case  0: break;                        // No interrupt
   case  2: break;                        // TB0CCR1
   case  4: break;                        // TB0CCR2
   case  6: break;                        // TB0CCR3
   case  8:                               // TB0CCR4

	   // Set CCR
	   TB0CCR4 += (32768 / SAMPLE_FREQUENCY);
	   AcquireData();
	   break;

   case 10: break;                       // reserved
   case 12: break;                       // reserved
   case 14: break;                       // TBIFG overflow handler
   }
}

// Acquires one sample (x,y,z) from the LSM303DLHC accelerometer and converts this
// into three 16 bit values which is placed in the sample array
void AcquireData(void) {

	if(sampleCounter < NUMBER_OF_SAMPLES)
	{
		// Read sample from LSM303DLHC accelerometer
		LSM303DLHC_getAccel(accel_8bit);

		// 6 uint8_t are read from the accelerometer
		// These are converted into 3 int16_t values
		// Structure of accelBuff: [X_LSB X_MSB Y_LSB Y_MSB Z_LSB Z_MSB]
		for (i = 0; i < 3; i++) {
			accel_16bit[i] = ((int16_t) accel_8bit[(2 * i) + 1] << 8) | (accel_8bit[2 * i] & 0xff);
		}

		// Calculating normalized acceleration
		// sqrt(X^2 + Y^2 + Z^2)
		accel_normalized = pow(accel_16bit[0], 2) + pow(accel_16bit[1], 2) + pow(accel_16bit[2], 2);
		accel_normalized = sqrt(accel_normalized);

		currentSample = accel_normalized;

		// Find high and low peak values
		if(lastSample != 0)
		{
			if((currentSample - lastSample) > 1000)
			{
				peakHighDetected = 1;
				peakPositionDifference = 0;
			}

			if ((currentSample - lastSample) < -1000)
			{
				peakLowDetected = 1;
			}

			if(peakHighDetected && !peakLowDetected)
			{
				peakPositionDifference++;
			}
		}

		// Check if a step has been taken
		if(peakLowDetected && peakHighDetected && peakPositionDifference < 4)
		{
			numberOfSteps++;
			peakHighDetected = 0;
			peakLowDetected = 0;
			peakPositionDifference = 0;
			Board_ledToggle(LED_RED);
		}

		lastSample = currentSample;
		sampleCounter++;
	}
	else
	{
		//Board_ledToggle(LED_YELLOW0);
		sampleCounter = 0;
	}
}

void delay_ms(unsigned int ms)
{
	while (ms)
    {
		__delay_cycles(24000);
        ms--;
    }
}

// Trap ISR assignation - put all unused ISR vector here
#pragma vector = RTC_VECTOR, USCI_B1_VECTOR, \
	SYSNMI_VECTOR, TIMER0_B0_VECTOR, \
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
