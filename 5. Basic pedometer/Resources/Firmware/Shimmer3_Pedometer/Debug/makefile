################################################################################
# Automatically-generated file. Do not edit!
################################################################################

SHELL = cmd.exe

CG_TOOL_ROOT := c:/ti/ccsv6/tools/compiler/msp430_4.3.3

ORDERED_OBJS += \
$(GEN_CMDS__FLAG) \
"./main.obj" \
"./system_pre_init.obj" \
"./LSM303DLHC/lsm303dlhc.obj" \
"./Bluetooth/RN42.obj" \
"./5xx_HAL/hal_Board.obj" \
"./5xx_HAL/hal_Button.obj" \
"./5xx_HAL/hal_I2C.obj" \
"./5xx_HAL/hal_UCS.obj" \
"./5xx_HAL/hal_pmm.obj" \
"../lnk_msp430f5437a.cmd" \
-l"libc.a" \
-l"libmath.a" \

-include ../makefile.init

RM := DEL /F
RMDIR := RMDIR /S/Q

# All of the sources participating in the build are defined here
-include sources.mk
-include subdir_vars.mk
-include LSM303DLHC/subdir_vars.mk
-include Bluetooth/subdir_vars.mk
-include 5xx_HAL/subdir_vars.mk
-include subdir_rules.mk
-include LSM303DLHC/subdir_rules.mk
-include Bluetooth/subdir_rules.mk
-include 5xx_HAL/subdir_rules.mk
-include objects.mk

ifneq ($(MAKECMDGOALS),clean)
ifneq ($(strip $(S_DEPS)),)
-include $(S_DEPS)
endif
ifneq ($(strip $(S_UPPER_DEPS)),)
-include $(S_UPPER_DEPS)
endif
ifneq ($(strip $(S62_DEPS)),)
-include $(S62_DEPS)
endif
ifneq ($(strip $(C64_DEPS)),)
-include $(C64_DEPS)
endif
ifneq ($(strip $(ASM_DEPS)),)
-include $(ASM_DEPS)
endif
ifneq ($(strip $(CC_DEPS)),)
-include $(CC_DEPS)
endif
ifneq ($(strip $(S55_DEPS)),)
-include $(S55_DEPS)
endif
ifneq ($(strip $(C67_DEPS)),)
-include $(C67_DEPS)
endif
ifneq ($(strip $(C??_DEPS)),)
-include $(C??_DEPS)
endif
ifneq ($(strip $(CLA_DEPS)),)
-include $(CLA_DEPS)
endif
ifneq ($(strip $(CPP_DEPS)),)
-include $(CPP_DEPS)
endif
ifneq ($(strip $(S??_DEPS)),)
-include $(S??_DEPS)
endif
ifneq ($(strip $(C_DEPS)),)
-include $(C_DEPS)
endif
ifneq ($(strip $(C62_DEPS)),)
-include $(C62_DEPS)
endif
ifneq ($(strip $(CXX_DEPS)),)
-include $(CXX_DEPS)
endif
ifneq ($(strip $(C++_DEPS)),)
-include $(C++_DEPS)
endif
ifneq ($(strip $(ASM_UPPER_DEPS)),)
-include $(ASM_UPPER_DEPS)
endif
ifneq ($(strip $(K_DEPS)),)
-include $(K_DEPS)
endif
ifneq ($(strip $(C43_DEPS)),)
-include $(C43_DEPS)
endif
ifneq ($(strip $(S67_DEPS)),)
-include $(S67_DEPS)
endif
ifneq ($(strip $(SA_DEPS)),)
-include $(SA_DEPS)
endif
ifneq ($(strip $(S43_DEPS)),)
-include $(S43_DEPS)
endif
ifneq ($(strip $(OPT_DEPS)),)
-include $(OPT_DEPS)
endif
ifneq ($(strip $(S64_DEPS)),)
-include $(S64_DEPS)
endif
ifneq ($(strip $(C_UPPER_DEPS)),)
-include $(C_UPPER_DEPS)
endif
ifneq ($(strip $(C55_DEPS)),)
-include $(C55_DEPS)
endif
endif

-include ../makefile.defs

# Add inputs and outputs from these tool invocations to the build variables 
EXE_OUTPUTS += \
Shimmer3_Pedometer.out \

EXE_OUTPUTS__QUOTED += \
"Shimmer3_Pedometer.out" \

BIN_OUTPUTS += \
Shimmer3_Pedometer.hex \

BIN_OUTPUTS__QUOTED += \
"Shimmer3_Pedometer.hex" \


# All Target
all: Shimmer3_Pedometer.out

# Tool invocations
Shimmer3_Pedometer.out: $(OBJS) $(CMD_SRCS) $(GEN_CMDS)
	@echo 'Building target: $@'
	@echo 'Invoking: MSP430 Linker'
	"c:/ti/ccsv6/tools/compiler/msp430_4.3.3/bin/cl430" -vmspx --abi=eabi --data_model=restricted --advice:power="all" -g --define=__MSP430F5437A__ --diag_warning=225 --display_error_number --diag_wrap=off --silicon_errata=CPU21 --silicon_errata=CPU22 --silicon_errata=CPU23 --silicon_errata=CPU40 --printf_support=minimal -z -m"Shimmer3_Pedometer.map" --heap_size=160 --stack_size=160 --use_hw_mpy=F5 --cinit_hold_wdt=on -i"c:/ti/ccsv6/ccs_base/msp430/include" -i"c:/ti/ccsv6/tools/compiler/msp430_4.3.3/lib" -i"c:/ti/ccsv6/tools/compiler/msp430_4.3.3/include" -i"c:/ti/ccsv6/ccs_base/msp430/lib/5xx_6xx_FRxx" --reread_libs --warn_sections --display_error_number --diag_wrap=off --xml_link_info="Shimmer3_Pedometer_linkInfo.xml" --rom_model -o "Shimmer3_Pedometer.out" $(ORDERED_OBJS)
	@echo 'Finished building target: $@'
	@echo ' '
	@$(MAKE) --no-print-directory post-build

Shimmer3_Pedometer.hex: $(EXE_OUTPUTS)
	@echo 'Invoking: MSP430 Hex Utility'
	"c:/ti/ccsv6/tools/compiler/msp430_4.3.3/bin/hex430" --memwidth=8 --romwidth=8 -o "Shimmer3_Pedometer.hex" $(EXE_OUTPUTS__QUOTED)
	@echo 'Finished building: $@'
	@echo ' '

# Other Targets
clean:
	-$(RM) $(EXE_OUTPUTS__QUOTED)$(BIN_OUTPUTS__QUOTED)
	-$(RM) "main.pp" "system_pre_init.pp" "LSM303DLHC\lsm303dlhc.pp" "Bluetooth\RN42.pp" "5xx_HAL\hal_Board.pp" "5xx_HAL\hal_Button.pp" "5xx_HAL\hal_I2C.pp" "5xx_HAL\hal_UCS.pp" "5xx_HAL\hal_pmm.pp" 
	-$(RM) "main.obj" "system_pre_init.obj" "LSM303DLHC\lsm303dlhc.obj" "Bluetooth\RN42.obj" "5xx_HAL\hal_Board.obj" "5xx_HAL\hal_Button.obj" "5xx_HAL\hal_I2C.obj" "5xx_HAL\hal_UCS.obj" "5xx_HAL\hal_pmm.obj" 
	-@echo 'Finished clean'
	-@echo ' '

post-build:
	-"c:/ti/ccsv6/tools/compiler/msp430_4.3.3/bin/hex430" --ti_txt "Shimmer3_Pedometer.out" -o "Shimmer3_Pedometer.txt" -order MS -romwidth 16
	-@echo ' '

.PHONY: all clean dependents
.SECONDARY: post-build

-include ../makefile.targets

