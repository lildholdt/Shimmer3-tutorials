################################################################################
# Automatically-generated file. Do not edit!
################################################################################

# Each subdirectory must supply rules for building sources it contributes
5xx_HAL/hal_Board.obj: ../5xx_HAL/hal_Board.c $(GEN_OPTS) $(GEN_HDRS)
	@echo 'Building file: $<'
	@echo 'Invoking: MSP430 Compiler'
	"c:/ti/ccsv6/tools/compiler/msp430_4.3.3/bin/cl430" -vmspx --abi=eabi -g --include_path="c:/ti/ccsv6/ccs_base/msp430/include" --include_path="C:/Users/Steffan/Dropbox/CareStore/Shimmer3/Sample Programs/apps/Shimmer3_Blink/5xx_HAL/" --include_path="c:/ti/ccsv6/tools/compiler/msp430_4.3.3/include" --advice:power=all --define=__MSP430F5437A__ --diag_warning=225 --display_error_number --silicon_errata=CPU21 --silicon_errata=CPU22 --silicon_errata=CPU23 --silicon_errata=CPU40 --printf_support=minimal --preproc_with_compile --preproc_dependency="5xx_HAL/hal_Board.pp" --obj_directory="5xx_HAL" $(GEN_OPTS__FLAG) "$<"
	@echo 'Finished building: $<'
	@echo ' '

5xx_HAL/hal_UCS.obj: ../5xx_HAL/hal_UCS.c $(GEN_OPTS) $(GEN_HDRS)
	@echo 'Building file: $<'
	@echo 'Invoking: MSP430 Compiler'
	"c:/ti/ccsv6/tools/compiler/msp430_4.3.3/bin/cl430" -vmspx --abi=eabi -g --include_path="c:/ti/ccsv6/ccs_base/msp430/include" --include_path="C:/Users/Steffan/Dropbox/CareStore/Shimmer3/Sample Programs/apps/Shimmer3_Blink/5xx_HAL/" --include_path="c:/ti/ccsv6/tools/compiler/msp430_4.3.3/include" --advice:power=all --define=__MSP430F5437A__ --diag_warning=225 --display_error_number --silicon_errata=CPU21 --silicon_errata=CPU22 --silicon_errata=CPU23 --silicon_errata=CPU40 --printf_support=minimal --preproc_with_compile --preproc_dependency="5xx_HAL/hal_UCS.pp" --obj_directory="5xx_HAL" $(GEN_OPTS__FLAG) "$<"
	@echo 'Finished building: $<'
	@echo ' '


