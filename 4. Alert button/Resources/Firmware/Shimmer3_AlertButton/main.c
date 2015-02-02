#include <stdint.h>
#include "msp430.h"
#include "hal_pmm.h"
#include "hal_UCS.h"
#include "hal_Board.h"
#include "hal_Button.h"
#include "RN42.h"
#include "shimmer.h"

//Method declarations
void Init(void);
uint8_t BtDataAvailable(uint8_t data);

// defines
#define DATA_PACKET_SIZE         4

// Variables
uint8_t btIsConnected;

void main(void) {

	Init();

	while(1) {
		__bis_SR_register(LPM3_bits);	//ACLK remains active
	}
}

void Init(void) {

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
	//BT_setFriendlyName("Shimmer3");
	//BT_setAuthentication(4);
	//BT_setPIN("1234");
	BT_configure();
	BT_receiveFunction(&BtDataAvailable);

	//Globally enable interrupts
	_enable_interrupts();

	//Bluetooth is initially disconnected
	btIsConnected = 0;
}

uint8_t BtDataAvailable(uint8_t data) {
	Board_ledToggle(LED_RED);
	return 1;
}


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

	    //BUTTON_SW1
		case  P1IV_P1IFG6:

			// Toggle red LED to indicate button press
        	Board_ledToggle(LED_RED);

        	// If Bluetooth connection is enabled transmit the buffer
        	if(btIsConnected)
        	{
        		BT_write("A", 1);
        	}

            break;

        // Default case
        default:
            break;
    }
}


// trap isr assignation - put all unused ISR vector here
#pragma vector = RTC_VECTOR, USCI_B1_VECTOR, \
	SYSNMI_VECTOR, TIMER0_B0_VECTOR, PORT2_VECTOR, \
	TIMER1_A1_VECTOR, UNMI_VECTOR, USCI_A0_VECTOR, \
	WDT_VECTOR, TIMER0_A1_VECTOR, \
	TIMER0_A0_VECTOR, TIMER0_B1_VECTOR, \
	DMA_VECTOR, ADC12_VECTOR, USCI_B0_VECTOR
__interrupt void TrapIsr(void)
{
  // this is a trap ISR - check for the interrupt cause here by
  // checking the interrupt flags, if necessary also clear the interrupt
  // flag
}
