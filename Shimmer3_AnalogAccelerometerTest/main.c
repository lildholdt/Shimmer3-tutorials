#include <stdint.h>
#include "msp430.h"
#include "hal_pmm.h"
#include "hal_UCS.h"
#include "hal_Board.h"
#include "hal_Button.h"
#include "hal_ADC.h"
#include "hal_DMA.h"
#include "RN42.h"
#include "lsm303dlhc.h"
#include "math.h"
#include "shimmer.h"

//Method declarations
void Init(void);
uint8_t BtDataAvailable(uint8_t data);
void StartSensing(void);
void StopSensing(void);
uint8_t Dma0ConversionDone(void);

// Defines
#define PACKET_SIZE        		3
#define NUMBER_OF_SAMPLES 		10
#define SAMPLE_FREQUENCY		1
#define ADC_SAMPLES_SIZE		3

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

uint16_t *adcStartPtr;
uint16_t test[ADC_SAMPLES_SIZE];

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

	// Enable Bluetooth
	BT_init();
	BT_setRadioMode(SLAVE_MODE);
	BT_setFriendlyName("Shimmer3");
	//BT_setAuthentication(4);
	BT_setPIN("1234");
	BT_configure();
	BT_receiveFunction(&BtDataAvailable);

	// Start sample timer
	StartSensing();

	adcStartPtr = ADC_init(MASK_A_ACCEL);
	DMA0_transferDoneFunction(&Dma0ConversionDone);
	if (adcStartPtr) {
		DMA0_init(adcStartPtr, test, ADC_SAMPLES_SIZE);
	}

	// Enable Analog ADC. It is necessary to enable the ADC, which is connected to P8.6
	// of the MSP430. After this, we can read the accelerometer.
	P8REN &= ~BIT6;      //disable pull down resistor
	P8DIR |= BIT6;       //set as output
	P8OUT |= BIT6;       //analog accel being used so take out of sleep mode
}

uint8_t BtDataAvailable(uint8_t data) {

	// Close BT connection
	BT_disable();
	P1IES &= ~BIT0; //look for rising edge
	BT_connectionInterrupt(0);
	btIsConnected = 0;
	Board_ledOff(LED_BLUE);
	return 1;
}

/**
*** DMA interrupt vector
**/
uint8_t Dma0ConversionDone(void) {

   DMA0_repeatTransfer(adcStartPtr, test, ADC_SAMPLES_SIZE); //Destination address for next transfer
   ADC_disable();    //can disable ADC until next time sampleTimer fires (to save power)?
   DMA0_disable();

   Board_ledToggle(LED_GREEN0);

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



      } else {
    	 // Shimmer is NOT docked
         P2IES &= ~BIT3;   //look for rising edge
         Board_ledOff(LED_YELLOW0);

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
	   	   // Read from the ADC
	   	   DMA0_enable();
	   	   ADC_startConversion();
	   break;

   case 10: break;                       // reserved
   case 12: break;                       // reserved
   case 14: break;                       // TBIFG overflow handler
   }
}


// Trap ISR assignation - put all unused ISR vector here
#pragma vector = RTC_VECTOR, USCI_B1_VECTOR, \
	SYSNMI_VECTOR, TIMER0_B0_VECTOR, \
	TIMER1_A1_VECTOR, UNMI_VECTOR, USCI_A0_VECTOR, \
	WDT_VECTOR, TIMER0_A1_VECTOR, \
	TIMER0_A0_VECTOR
__interrupt void TrapIsr(void)
{
  // this is a trap ISR - check for the interrupt cause here by
  // checking the interrupt flags, if necessary also clear the interrupt
  // flag
}
