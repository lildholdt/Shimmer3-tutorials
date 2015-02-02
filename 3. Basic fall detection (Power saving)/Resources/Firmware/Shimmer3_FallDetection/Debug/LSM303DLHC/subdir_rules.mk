################################################################################
# Automatically-generated file. Do not edit!
################################################################################

# Each subdirectory must supply rules for building sources it contributes
LSM303DLHC/lsm303dlhc.obj: ../LSM303DLHC/lsm303dlhc.c $(GEN_OPTS) $(GEN_HDRS)
	@echo 'Building file: $<'
	@echo 'Invoking: MSP430 Compiler'
	"c:/ti/ccsv6/tools/compiler/msp430_4.3.3/bin/cl430" -vmspx --abi=eabi --data_model=restricted --include_path="c:/ti/ccsv6/ccs_base/msp430/include" --include_path="C:/Users/Steffan/workspace_v6_0/Shimmer3_FallDetection/LSM303DLHC" --include_path="C:/Users/Steffan/workspace_v6_0/Shimmer3_FallDetection/Bluetooth" --include_path="C:/Users/Steffan/workspace_v6_0/Shimmer3_FallDetection/5xx_HAL" --include_path="c:/ti/ccsv6/tools/compiler/msp430_4.3.3/include" --advice:power="all" -g --define=__MSP430F5437A__ --diag_warning=225 --display_error_number --diag_wrap=off --silicon_errata=CPU21 --silicon_errata=CPU22 --silicon_errata=CPU23 --silicon_errata=CPU40 --printf_support=minimal --preproc_with_compile --preproc_dependency="LSM303DLHC/lsm303dlhc.pp" --obj_directory="LSM303DLHC" $(GEN_OPTS__FLAG) "$<"
	@echo 'Finished building: $<'
	@echo ' '


