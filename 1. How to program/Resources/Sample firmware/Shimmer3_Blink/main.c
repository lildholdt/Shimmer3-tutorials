#include <stdint.h>
#include "msp430.h"
#include "hal_UCS.h"
#include "hal_Board.h"

void BlinkTimerStart(void);
inline void BlinkTimerStop(void);

void main(void) {

	Board_init();

   // Start 32.768kHz XTAL as ACLK
   LFXT_Start(XT1DRIVE_0);

   SFRIFG1 = 0;                  // clear interrupt flag register
   SFRIE1 |= OFIE;               // enable oscillator fault interrupt enable

   //Globally enable interrupts
   _enable_interrupts();

   BlinkTimerStart();

   while(1) {
      __bis_SR_register(LPM3_bits + GIE); //ACLK remains active
   }
}

/**
*** Blink Timer
**/
// USING TB0 with CCR4
void BlinkTimerStart(void) {
   TB0CCR4 = 32768;   //1Hz
   Board_ledOff(LED_ALL);

   TB0CCTL4 = CCIE;
   TB0CTL = TBSSEL_1 + MC_2 + TBCLR;
}

inline void BlinkTimerStop() {
   TB0CTL = MC0;
   Board_ledOn(LED_ALL);
}

#pragma vector=TIMER0_B1_VECTOR
__interrupt void TIMER0_B1_ISR(void) {
   switch(__even_in_range(TB0IV,14)) {
   case  0: break;                        // No interrupt
   case  2: break;                        // TB0CCR1
   case  4: break;                        // TB0CCR2
   case  6: break;                        // TB0CCR3
   case  8:                               // TB0CCR4
      //Control LED
      TB0CCR4 += 32768;
      Board_ledToggle(LED_ALL);
      break;
   case 10: break;                       // reserved
   case 12: break;                       // reserved
   case 14: break;                       // TBIFG overflow handler
   }
}

/**
***
 */
// trap isr assignation - put all unused ISR vector here
#pragma vector = RTC_VECTOR, USCI_B1_VECTOR,\
   TIMER0_A0_VECTOR, TIMER0_A1_VECTOR, TIMER1_A1_VECTOR, \
   UNMI_VECTOR, WDT_VECTOR, SYSNMI_VECTOR, PORT1_VECTOR, \
   TIMER0_B0_VECTOR, DMA_VECTOR, ADC12_VECTOR, PORT2_VECTOR, \
   USCI_A1_VECTOR, TIMER1_A0_VECTOR, USCI_B0_VECTOR, USCI_A0_VECTOR
__interrupt void TrapIsr(void) {
  // this is a trap ISR - check for the interrupt cause here by
  // checking the interrupt flags, if necessary also clear the interrupt
  // flag
}
