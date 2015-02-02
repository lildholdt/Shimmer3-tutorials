/*
 * Copyright (c) 2010, Shimmer Research, Ltd.
 * All rights reserved
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are
 * met:

 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above
 *       copyright notice, this list of conditions and the following
 *       disclaimer in the documentation and/or other materials provided
 *       with the distribution.
 *     * Neither the name of Shimmer Research, Ltd. nor the names of its
 *       contributors may be used to endorse or promote products derived
 *       from this software without specific prior written permission.

 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
 * OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * @author Mike Healy
 * @date   January, 2011
 */

/*
 * ChangeLog
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerConnect
{
    public class Shimmer
    {
        public enum ShimmerVersion
        {
            SHIMMER1 = 0,
            SHIMMER2 = 1,
            SHIMMER2R = 2,
            SHIMMER3 = 3
        }
        public static readonly string ApplicationName = "ShimmerConnect";
    }

    public class Shimmer2
    {
        

        public enum SamplingRates
        {
            Sampling1000Hz =    1,
            Sampling500Hz =     2,
            Sampling250Hz =     4,
            Sampling200Hz =     5,
            Sampling166Hz =     6,
            Sampling125Hz =     8,
            Sampling100Hz =     10,
            Sampling50Hz =      20,
            Sampling10Hz =      100,
            Sampling0HzOff =    255
        }

        public static double[,] SensitivityMatrixAccel1p5gShimmer2 = new double[3, 3] { { 101, 0, 0 }, { 0, 101, 0 }, { 0, 0, 101 } };
        public static double[,] SensitivityMatrixAccel2gShimmer2 = new double[3, 3] { { 76, 0, 0 }, { 0, 76, 0 }, { 0, 0, 76 } };
        public static double[,] SensitivityMatrixAccel4gShimmer2 = new double[3, 3] { { 38, 0, 0 }, { 0, 38, 0 }, { 0, 0, 38 } };
        public static double[,] SensitivityMatrixAccel6gShimmer2 = new double[3, 3] { { 25, 0, 0 }, { 0, 25, 0 }, { 0, 0, 25 } };
        public static double[,] SensitivityMatrixAccelShimmer2 = new double[3, 3] { { 38, 0, 0 }, { 0, 38, 0 }, { 0, 0, 38 } }; 	//Default Values for Accelerometer Calibration
        public static double[,] OffsetVectorAccelShimmer2 = new double[3, 1] { { 2048 }, { 2048 }, { 2048 } };				//Default Values for Accelerometer Calibration

        public static double[,] AlignmentMatrixGyroShimmer2 = new double[3, 3] { { 0, -1, 0 }, { -1, 0, 0 }, { 0, 0, -1 } }; 				//Default Values for Gyroscope Calibration
        public static double[,] SensitivityMatrixGyroShimmer2 = new double[3, 3] { { 2.73, 0, 0 }, { 0, 2.73, 0 }, { 0, 0, 2.73 } }; 		//Default Values for Gyroscope Calibration
        public static double[,] OffsetVectorGyroShimmer2 = new double[3, 1] { { 1843 }, { 1843 }, { 1843 } };						//Default Values for Gyroscope Calibration

        public static double[,] AlignmentMatrixMagShimmer2 = new double[3, 3] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, -1 } }; 				//Default Values for Magnetometer Calibration
        public static double[,] SensitivityMatrixMagShimmer2 = new double[3, 3] { { 580, 0, 0 }, { 0, 580, 0 }, { 0, 0, 580 } }; 			//Default Values for Magnetometer Calibration
        public static double[,] OffsetVectorMagShimmer2 = new double[3, 1] { { 0 }, { 0 }, { 0 } };									//Default Values for Magnetometer Calibration



        public static readonly string[] SamplingRatesString = new string[] {
            "       0Hz (Off)",
            "  10.2Hz",
            "  51.2Hz",
            "102.4Hz",
            "   128Hz",
            "170.7Hz",
            "204.8Hz",
            "   256Hz",
            "   512Hz",
            " 1024Hz"};

        
        public enum PacketType : byte
        {
            DATA_PACKET                 = 0x00,
            INQUIRY_COMMAND             = 0x01,
            INQUIRY_RESPONSE            = 0x02,
            GET_SAMPLING_RATE_COMMAND   = 0x03,
            SAMPLING_RATE_RESPONSE      = 0x04,
            SET_SAMPLING_RATE_COMMAND   = 0x05,
            TOGGLE_LED_COMMAND          = 0x06,
            START_STREAMING_COMMAND     = 0x07,
            SET_SENSORS_COMMAND         = 0x08,
            SET_ACCEL_RANGE_COMMAND     = 0x09,
            ACCEL_RANGE_RESPONSE        = 0x0A,
            GET_ACCEL_RANGE_COMMAND     = 0x0B,
            SET_5V_REGULATOR_COMMAND    = 0x0C,
            SET_POWER_MUX_COMMAND          = 0x0D,
            SetConfigSetupByte0Command  = 0x0E,
            ConfigSetupByte0Response    = 0x0F,
            GetConfigSetupByte0Command  = 0x10,
            SetAccelCalibrationCommand  = 0x11,
            AccelCalibrationResponse    = 0x12,
            GetAccelCalibrationCommand  = 0x13,
            SetGyroCalibrationCommand   = 0x14,
            GyroCalibrationResponse     = 0x15,
            GetGyroCalibrationCommand   = 0x16,
            SetMagCalibrationCommand    = 0x17,
            MagCalibrationResponse      = 0x18,
            GetMagCalibrationCommand    = 0x19,
            STOP_STREAMING_COMMAND        = 0x20,
            SetGsrRangeCommand          = 0x21,
            gsrRangeResponse            = 0x22,
            GetGsrRangeCommand          = 0x23,
            GET_SHIMMER_VERSION_COMMAND    = 0x24,
            GET_SHIMMER_VERSION_RESPONSE      = 0x25,
            SET_EMG_CALIBRATION_COMMAND      = 0x26,
	        EMG_CALIBRATION_RESPONSE         = 0x27,
            GET_EMG_CALIBRATION_COMMAND      = 0x28,
	        SET_ECG_CALIBRATION_COMMAND      = 0x29,
	        ECG_CALIBRATION_RESPONSE         = 0x2A,
	        GET_ECG_CALIBRATION_COMMAND      = 0x2B,
	        GET_ALL_CALIBRATION_COMMAND      = 0x2C,
	        ALL_CALIBRATION_RESPONSE         = 0x2D, 
	        GET_FW_VERSION_COMMAND           = 0x2E,
	        FW_VERSION_RESPONSE              = 0x2F,
	        SET_BLINK_LED                    = 0x30,
	        BLINK_LED_RESPONSE               = 0x31,
	        GET_BLINK_LED                    = 0x32,
	        SET_GYRO_TEMP_VREF_COMMAND       = 0x33,
            SET_BUFFER_SIZE_COMMAND          = 0x34,
            BUFFER_SIZE_RESPONSE             = 0x35,
	        GET_BUFFER_SIZE_COMMAND          = 0x36,
	        SET_MAG_GAIN_COMMAND             = 0x37,
	        MAG_GAIN_RESPONSE                = 0x38,
	        GET_MAG_GAIN_COMMAND             = 0x39,
	        SET_MAG_SAMPLING_RATE_COMMAND    = 0x3A,
	        MAG_SAMPLING_RATE_RESPONSE       = 0x3B,
	        GET_MAG_SAMPLING_RATE_COMMAND    = 0x3C,
            AckCommandProcessed         = 0xFF
        };

        public enum ChannelContents
        {
            XAccel      = 0x00,
            YAccel      = 0x01,
            ZAccel      = 0x02,
            XGyro       = 0x03,
            YGyro       = 0x04,
            ZGyro       = 0x05,
            XMag        = 0x06,
            YMag        = 0x07,
            ZMag        = 0x08,
            EcgRaLl     = 0x09,
            EcgLaLl     = 0x0A,
            GsrRaw      = 0x0B,
            GsrRes      = 0x0C,
            Emg         = 0x0D,
            AnExA0      = 0x0E,
            AnExA7      = 0x0F,
            StrainHigh  = 0x10,
            StrainLow   = 0x11,
            HeartRate   = 0x12
        }

        

        public static String[] ChannelUnits = new String[19] { "m/(sec^2)",
        "m/(sec^2)","m/(sec^2)","deg/sec","deg/sec","deg/sec","local","local","local","mVolts","mVolts","kOhms","kOhms","mVolts","mVolts","mVolts","mVolts","mVolts","BPM"};

        public static String[] ChannelProperties = new String[19] { "Accelerometer X",
        "Accelerometer Y","Accelerometer Z","Gyroscope X","Gyroscope Y","Gyroscope Z","Magnetometer X","Magnetometer Y","Magnetometer Z","ECG RA-LL","ECG LA-LL","GSR","GSR","EMG","ExpBoard A0","ExpBoard A7","Strain Gauge High","Strain Gauge Low","BPM"};


        public enum AdcChannels
        {
            XAccel      = 0x00,
            YAccel      = 0x01,
            ZAccel      = 0x02,
            XGyro       = 0x03,
            YGyro       = 0x04,
            ZGyro       = 0x05,
            EcgRaLl     = 0x09,
            EcgLaLl     = 0x0A,
            GsrRaw      = 0x0B,
            GsrRes      = 0x0C,
            Emg         = 0x0D,
            AnExA0      = 0x0E,
            AnExA7      = 0x0F,
            StrainHigh  = 0x10,
            StrainLow   = 0x11
        }

        public enum TwoByteDigiChannels
        {
            XMag = 0x06,
            YMag = 0x07,
            ZMag = 0x08,
            HeartRate = 0x12
        }

        public enum OneByteDigiChannels
        {
            HeartRate = 0x12
        }

        public enum SensorBitmap
        {
            SensorAccel     = 0x80,
            SensorGyro      = 0x40,
            SensorMag       = 0x20,
            SensorECG       = 0x10,
            SensorEMG       = 0x08,
            SensorGSR       = 0x04,
            SensorAnExA7    = 0x02,
            SensorAnExA0    = 0x01,
            SensorStrain    = 0x8000,
            SensorHeart     = 0x4000
        
        }

        public enum ConfigSetupByte0Bitmap
        {
            Config5VReg = 0x80,
            ConfigPMux  = 0x40, 
        }

        public enum AccelRange
        {
            RANGE_1_5G = 0,
            RANGE_2_0G = 1,
            RANGE_4_0G = 2,
            RANGE_6_0G = 3
        }

        public enum GsrRange
        {
            HW_RES_40K    = 0,
            HW_RES_287K   = 1,
            HW_RES_1M     = 2,
            HW_RES_3M3    = 3,
            AUTORANGE     = 4
        }

        public enum MaxNumChannels
        {
            MaxNum2ByteChannels = 11,   // (3xAccel) + (3xGyro) + (3xMag) + (2xAnEx)
            MaxNum1ByteChannels = 1,
            MaxNumChannels = MaxNum2ByteChannels + MaxNum1ByteChannels
        }

        public enum MaxPacketSizes
        {
            DataPacketSize      = 3 + ((int)MaxNumChannels.MaxNum2ByteChannels * 2) + (int)MaxNumChannels.MaxNum1ByteChannels,
            //ResponsePacketSize  = 6 + (int)MaxNumChannels.MaxNumChannels,
            ResponsePacketSize = 22,
            MaxCommandArgSize   = 21
        }

        public static int NumSensorBytes = 2;

        public static readonly string[] AccelRangeString = new string[] {
            "±1.5g",
            "±2g",
            "±4g",
            "±6g"};

        public static readonly string[] MagRangeString = new string[] {
            "±0.8Ga",
            "±1.3Ga",
            "±1.9Ga",
            "±2.5Ga",
            "±4.0Ga",
            "±4.7Ga",
            "±5.6Ga",
            "±8.1Ga"
        };

        public static readonly string[] GsrRangeString = new string[] {
            "  10kΩ– 56kΩ", 
            "  56kΩ-220kΩ",
            "220kΩ-680kΩ",
            "680kΩ–4.7MΩ",
            "  Auto-Range"};
    }



    public class Shimmer3
    {
        public static readonly string ApplicationName = "Shimmer3Connect";
        public static int NumSensorBytes = 2;
        public enum SamplingRates
        {
            Sampling1000Hz = 32,         //1024Hz
            Sampling500Hz = 64,         //512Hz
            Sampling250Hz = 128,        //256
            Sampling200Hz = 160,        //204.8Hz
            Sampling100Hz = 320,        //102.4Hz
            Sampling50Hz = 640,        //51.2Hz
            Sampling10Hz = 3200,       //10.24Hz
            Sampling1Hz = 32768       //1Hz
        }

        public static readonly string[] AccelRangeString = new string[] {
            "±2g",
            "±4g",
            "±8g",
            "±16g"};

        public static readonly string[] PressureResolutionString = new string[] {
            "Low",
            "Standard",
            "High",
            "Very High"};

        public enum AccelRange
        {
            RANGE_2_0G = 0,
            RANGE_4_0G = 1,
            RANGE_8_0G = 2,
            RANGE_16_0G = 3
        }

        public static readonly string[] GyroRangeString = new string[] {
            "250dps",
            "500dps",
            "1000dps",
            "2000dps"
        };

        public enum GyroRange
        {
            RANGE_250DPS = 0,
            RANGE_500DPS = 1,
            RANGE_1000DPS = 2,
            RANGE_2000DPS = 3
        }

        public static readonly string[] MagRangeString = new string[] {
            "±1.3Ga",
            "±1.9Ga",
            "±2.5Ga",
            "±4.0Ga",
            "±4.7Ga",
            "±5.6Ga",
            "±8.1Ga"
        };

        public enum MagRange
        {
            RANGE_1_3Ga = 0,
            RANGE_1_9Ga = 1,
            RANGE_2_5Ga = 2,
            RANGE_4_0Ga = 4,
            RANGE_4_7Ga = 5,
            RANGE_5_6Ga = 6,
            RANGE_8_1Ga = 7
        }

        public static double[,] SensitivityMatrixLowNoiseAccel2gShimmer3 = new double[3, 3] { { 83, 0, 0 }, { 0, 83, 0 }, { 0, 0, 83 } };
        public static double[,] AlignmentMatrixLowNoiseAccelShimmer3 = new double[3, 3] { { 0, -1, 0 }, { -1, 0, 0 }, { 0, 0, -1 } }; 	//Default Values for Accelerometer Calibration
        public static double[,] OffsetVectorAccelLowNoiseShimmer3 = new double[3, 1] { { 2047 }, { 2047 }, { 2047 } };				//Default Values for Accelerometer Calibration
        public static double[,] SensitivityMatrixWideRangeAccel2gShimmer3 = new double[3, 3] { { 102, 0, 0 }, { 0, 102, 0 }, { 0, 0, 102 } };
        public static double[,] SensitivityMatrixWideRangeAccel4gShimmer3 = new double[3, 3] { { 51, 0, 0 }, { 0, 51, 0 }, { 0, 0, 51 } };
        public static double[,] SensitivityMatrixWideRangeAccel8gShimmer3 = new double[3, 3] { { 25, 0, 0 }, { 0, 25, 0 }, { 0, 0, 25 } };
        public static double[,] SensitivityMatrixWideRangeAccel16gShimmer3 = new double[3, 3] { { 8, 0, 0 }, { 0, 8, 0 }, { 0, 0, 8 } };
        public static double[,] AlignmentMatrixWideRangeAccelShimmer3 = new double[3, 3] { { -1, 0, 0 }, { 0, -1, 0 }, { 0, 0, -1 } }; 	//Default Values for Accelerometer Calibration
        public static double[,] OffsetVectorAccelWideRangeShimmer3 = new double[3, 1] { { 0 }, { 0 }, { 0 } };				//Default Values for Accelerometer Calibration
        
        
        //public double[,] SensitivityMatrixAccel2gShimmer3 = new double[3, 3] { { 76, 0, 0 }, { 0, 76, 0 }, { 0, 0, 76 } };
        //public double[,] SensitivityMatrixAccel4gShimmer3 = new double[3, 3] { { 38, 0, 0 }, { 0, 38, 0 }, { 0, 0, 38 } };
        //public double[,] SensitivityMatrixAccel6gShimmer3 = new double[3, 3] { { 25, 0, 0 }, { 0, 25, 0 }, { 0, 0, 25 } };

        public static double[,] AlignmentMatrixGyroShimmer3 = new double[3, 3] { { 0, -1, 0 }, { -1, 0, 0 }, { 0, 0, -1 } }; 				//Default Values for Gyroscope Calibration
        public static double[,] SensitivityMatrixGyroShimmer3 = new double[3, 3] { { 131, 0, 0 }, { 0, 131, 0 }, { 0, 0, 131 } }; 		//Default Values for Gyroscope Calibration
        public static double[,] OffsetVectorGyroShimmer3 = new double[3, 1] { { 0 }, { 0 }, { 0 } };						//Default Values for Gyroscope Calibration

        public static double[,] AlignmentMatrixMagShimmer3 = new double[3, 3] { { 1, 0, 0 }, { 0, -1, 0 }, { 0, 0, 1 } }; 				//Default Values for Magnetometer Calibration
        public static double[,] SensitivityMatrixMagShimmer3 = new double[3, 3] { { 420, 0, 0 }, { 0, 450, 0 }, { 0, 0, 520 } }; 			//Default Values for Magnetometer Calibration
        public static double[,] OffsetVectorMagShimmer3 = new double[3, 1] { { 180 }, { 50 }, { 50 } };	

        public static readonly string[] SamplingRatesString = new string[] {
            "    1Hz",
            "  10.2Hz",
            "  51.2Hz",
            "102.4Hz",
            "204.8Hz",
            "   250Hz",
            "   512Hz",
            " 1024kHz"};

        public enum PacketType : byte
        {
            DataPacket = 0x00,
            InquiryCommand = 0x01,
            InquiryResponse = 0x02,
            GetSamplingRateCommand = 0x03,
            SamplingRateResponse = 0x04,
            SetSamplingRateCommand = 0x05,
            ToggleLedCommand = 0x06,
            StartStreamingCommand = 0x07,
            SetSensorsCommand = 0x08,
            SetAAccelCalibrationCommand = 0x09,
            AAccelCalibrationResponse = 0x0A,
            GetAAccelCalibrationCommand = 0x0B,
            SetGyroCalibrationCommand = 0x0C,
            GyroCalibrationResponse = 0x0D,
            GetGyroCalibrationCommand = 0x0E,
            SetMagCalibrationCommand = 0x0F,
            MagCalibrationResponse = 0x10,
            GetMagCalibrationCommand = 0x11,
            SetDAccelCalibrationCommand = 0x12,
            DAccelCalibrationResponse = 0x13,
            GetDAccelCalibrationCommand = 0x14,
            StopStreamingCommand = 0x20,
            GET_SHIMMER_VERSION_COMMAND = 0x3F,
            ShimmerVersionResponse = 0x25,
            GetFwVersionCommand = 0x2E,
            FwVersionResponse = 0x2F,
            SET_LSM303DLHC_MAG_GAIN_COMMAND = 0x37,
            SET_ACCEL_SAMPLING_RATE_COMMAND  = 0x40,
            SET_MPU9150_SAMPLING_RATE_COMMAND = 0x4C,
            SET_MPU9150_GYRO_RANGE_COMMAND = 0x49,
            AckCommandProcessed = 0xFF
        };

        public enum ChannelContents
        {
            XAAccel = 0x00,
            YAAccel = 0x01,
            ZAAccel = 0x02,
            VBatt = 0x03,
            XDAccel = 0x04,
            YDAccel = 0x05,
            ZDAccel = 0x06,
            XMag = 0x07,
            YMag = 0x08,
            ZMag = 0x09,
            XGyro = 0x0A,
            YGyro = 0x0B,
            ZGyro = 0x0C,
            ExternalAdc7 = 0x0D,
            ExternalAdc6 = 0x0E,
            ExternalAdc15 = 0x0F,
            InternalAdc1 = 0x10,
            InternalAdc12 = 0x11,
            InternalAdc13 = 0x12,
            InternalAdc14 = 0x13,
            Pressure = 0x20
        }
        public static String[] ChannelUnits = new String[20] { "m/(sec^2)", "m/(sec^2)", "m/(sec^2)", "mVolts", "m/(sec^2)", "m/(sec^2)", "m/(sec^2)", "local", "local", "local", "deg/sec", "deg/sec", "deg/sec", "mVolts", "mVolts", "mVolts", "mVolts", "mVolts", "mVolts", "mVolts" };

        public static String[] ChannelProperties = new String[20] { "Accelerometer X", "Accelerometer Y", "Accelerometer Z", "VSenseBatt", "Accelerometer X", "Accelerometer Y", "Accelerometer Z", "Magnetometer X", "Magnetometer Y", "Magnetometer Z", "Gyroscope X", "Gyroscope Y", "Gyroscope Z", "External ADC A7", "External ADC A6", "External ADC A15", "Internal ADC A1","Internal ADC A12","Internal ADC A13","Internal ADC A14"};

        public enum SensorBitmap
        {
            SensorAAccel            = 0x80,
            SensorGyro              = 0x040,
            SensorVBatt             = 0x2000,
            SensorMag               = 0x20,
            SensorDAccel            = 0x1000,
            SensorExtA7             = 0x02,
            SensorExtA6             = 0x01,
            SensorExtA15            = 0x0800,
            SensorIntA1             = 0x0400,
            SensorIntA12            = 0x0200,
            SensorIntA13            = 0x0100,
            SensorIntA14            = 0x800000,
            SensorPressure          = 0x40000
        }


        public enum MaxNumChannels
        {
            MaxNumChannels = 13
        }

        public enum MaxPacketSizes
        {
            DataPacketSize = 29,
            ResponsePacketSize = 85,
            MaxCommandArgSize = 21
        }
    }

    public class ShimmerDataPacket
    {

        private int timeStamp;
        private List<int> channels = new List<int>();
        private int numAdcChannels;
        private int num1ByteDigiChannels;
        private int num2ByteDigiChannels;
        private int numChannels;
        private bool isFilled = false;

        public ShimmerDataPacket(List<byte> packet, int numAdcChans, int num1ByteDigiChans, int num2ByteDigiChans, int shimmerVersion, int numberofChannels, ShimmerProfile pProfile)
        {
            if (shimmerVersion != (int)Shimmer.ShimmerVersion.SHIMMER3)
            {
                int i;

                numAdcChannels = numAdcChans;
                num1ByteDigiChannels = num1ByteDigiChans;
                num2ByteDigiChannels = num2ByteDigiChans;
                numChannels = numAdcChannels + num1ByteDigiChannels + num2ByteDigiChannels;
                if (packet.Count >= (2 + ((numAdcChannels + num2ByteDigiChans) * 2) + num1ByteDigiChans))    // timestamp + channel data
                {
                    timeStamp = (int)packet[0];
                    timeStamp += ((int)packet[1] << 8) & 0xFF00;
                    // 16 bit channels come first
                    for (i = 0; i < (numAdcChannels + num2ByteDigiChannels); i++)
                    {
                        channels.Add((int)packet[2 + (2 * i)]);
                        channels[i] += ((int)packet[2 + (2 * i) + 1] << 8) & 0xFF00;
                    }
                    // 8 bit channels
                    i *= 2;
                    for (int j = 0; j < num1ByteDigiChannels; j++, i++)
                    {
                        channels.Add((int)packet[2 + i]);
                    }
                    isFilled = true;
                }
            }
            else
            {
                int i;

                numChannels = numberofChannels;
                if (packet.Count >= (2 + (numChannels * 2)))    // timestamp + channel data
                {
                    timeStamp = (int)packet[0];
                    timeStamp += ((int)packet[1] << 8) & 0xFF00;
                    // 16 bit channels
                    for (i = 0; i < numChannels; i++)
                    {
                        channels.Add((int)packet[2 + (2 * i)]);
                        channels[i] += ((int)packet[2 + (2 * i) + 1] << 8) & 0xFF00;

                        if (pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XMag ||
                               pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YMag ||
                               pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZMag ||
                               pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XGyro ||
                               pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YGyro ||
                               pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZGyro)
                        {
                            // The magnetometer and gyro gives a signed, big endian, 16 bit integer per channel
                            channels[i] = ((Int16)(((channels[i] & 0xFF00) >> 8) | ((channels[i] & 0xFF) << 8)));
                        }

                        if (pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XDAccel ||
                                pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YDAccel ||
                                pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZDAccel)
                        {
                            // The digital accel gives a signed 16 bit integer per channel
                            channels[i] = ((Int16)channels[i]);
                        }

                    }
                    isFilled = true;
                }
            }
        }

        public int GetTimeStamp()
        {
            return timeStamp;
        }

        public int GetNumChannels()
        {
            return numChannels;
        }

        public int GetNumAdcChannels()
        {
            return numAdcChannels;
        }

        public int GetNum1ByteDigiChannels()
        {
            return num1ByteDigiChannels;
        }

        public int GetNum2ByteDigiChannels()
        {
            return num2ByteDigiChannels;
        }

        public int GetChannel(int channelNum)
        {
            // channelNum is indexed from 0
            if (channelNum >= numChannels)
                return -1;
            else
                return channels[channelNum];
        }

        public void SetChannel(int channelNum, int val)
        {
            // channelNum is indexed from 0
            if (channelNum < numChannels)
                channels[channelNum] = val;
        }

        public bool GetIsFilled()
        {
            return isFilled;
        }
    }

    public class ShimmerProfile
    {
        public bool changeSamplingRate;
        public bool changeSensors;
        public bool change5Vreg;
        public bool changePwrMux;
        public bool changeAccelSens;
        public bool changeGyroSens;
        public bool changePresRes;
        public bool changeMagSens;
        public bool changeGsrRange;
        public bool enableLowPowerMag = false;
        public bool enableLowPowerAccel = false;
        public bool enableLowPowerGyro = false;
        public bool showMagHeading;
        public bool showGsrResistance;
        private int magSamplingRate; //
        private int adcSamplingRate;
        private int numChannels;
        private int numAdcChannels;
        private int num1ByteDigiChannels;
        private int num2ByteDigiChannels;
        private int bufferSize;
        private double firmwareIdentifier;
        private double firmwareVersion;
        private int firmwareInternal;
        private String firmwareVersionFullName;
        private List<int> channels = new List<int>();
        public Boolean enable3DOrientation = false;
        private int sensors;
        private int accelRange;
        private int gyroRange;
        private int pressureResolution;
        private int magGain;
        private int accelSamplingRate;
        private int mpu9150SamplingRate;
        private int configSetupByte0;
        private int gsrRange;
        private int shimmerVersion=0;

        public double[,] AlignmentMatrixAccel = new double[3, 3] { { -1, 0, 0 }, { 0, -1, 0 }, { 0, 0, 1 } }; 		
        public double[,] SensitivityMatrixAccel = new double[3, 3] { { 38, 0, 0 }, { 0, 38, 0 }, { 0, 0, 38 } }; 	
        public double[,] OffsetVectorAccel = new double[3, 1] { { 2048 }, { 2048 }, { 2048 } };				
        public double[,] AlignmentMatrixGyro = new double[3, 3] { { 0, -1, 0 }, { -1, 0, 0 }, { 0, 0, -1 } };
        public double[,] SensitivityMatrixGyro = new double[3, 3] { { 2.73, 0, 0 }, { 0, 2.73, 0 }, { 0, 0, 2.73 } };
        public double[,] OffsetVectorGyro = new double[3, 1] { { 1843 }, { 1843 }, { 1843 } };					
        public double[,] AlignmentMatrixMag = new double[3, 3] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, -1 } }; 				
        public double[,] SensitivityMatrixMag = new double[3, 3] { { 580, 0, 0 }, { 0, 580, 0 }, { 0, 0, 580 } }; 			
        public double[,] OffsetVectorMag = new double[3, 1] { { 0 }, { 0 }, { 0 } };
        public double[,] AlignmentMatrixAccel2 = new double[3, 3] { { -1, 0, 0 }, { 0, -1, 0 }, { 0, 0, 1 } };
        public double[,] SensitivityMatrixAccel2 = new double[3, 3] { { 38, 0, 0 }, { 0, 38, 0 }, { 0, 0, 38 } };
        public double[,] OffsetVectorAccel2 = new double[3, 1] { { 2048 }, { 2048 }, { 2048 } };				


     								//Default Values for Magnetometer Calibration



        public double OffsetECGRALL = 2060;
        public double GainECGRALL = 175;
        public double OffsetECGLALL = 2060;
        public double GainECGLALL = 175;
        public double OffsetEMG = 2060;
        public double GainEMG = 750;
        public double OffsetSGHigh = 60;
        public double VRef = 3;
        public double GainSGHigh = 551 * 2.8;
        public double OffsetSGLow = 1950;
        public double GainSGLow = 183.7 * 2.8;
        public double mLastReceivedTimeStamp = 0;
        public double mCurrentTimeStampCycle = 0;
        public double mLastReceivedCalibratedTimeStamp = -1;
        public long mPacketLossCount = 0;
        public double mPacketReceptionRate = 100;
        public int LastKnownHeartRate = 0;
        public Boolean mFirstTimeCalTime = true;
        public bool DefaultAccelParams = true;
        public bool DefaultDAccelParams = true;
        public bool DefaultGyroParams = true;
        public bool DefaultMagParams = true;
        public bool DefaultECGParams = true;
        public bool DefaultEMGParams = true;
        public int CurrentLEDStatus = 0;
        public String loggingFormat = ",";
        public double mCalTimeStart;
        private bool isFilled;

        public ShimmerProfile()
        {
            adcSamplingRate = -1;
            numChannels = 0;
            numAdcChannels = 0;
            num1ByteDigiChannels = 0;
            num2ByteDigiChannels = 0;
            bufferSize = 0;
            isFilled = false;
            sensors = 0;
            accelRange = 0;
            configSetupByte0 = 0;
            gsrRange = 0;

            changeSamplingRate = false;
            changeSensors = false;
            change5Vreg = false;
            changePwrMux = false;
            changeAccelSens = false;
            changeGyroSens = false;
            changeMagSens = false;
            changeGsrRange = false;
            showMagHeading = false;
            showGsrResistance = false;
            enable3DOrientation = false;
        }

        public ShimmerProfile(List<byte> packet)
        {
            isFilled = false;
            accelRange = 0;
            configSetupByte0 = 0;
            gsrRange = 0;

            changeSamplingRate = false;
            changeSensors = false;
            change5Vreg = false;
            changePwrMux = false;
            changeAccelSens = false;
            changeGyroSens = false;
            changeMagSens = false;
            changeGsrRange = false;
            showMagHeading = false;
            showGsrResistance = false;

            numAdcChannels = 0;
            num1ByteDigiChannels = 0;
            num2ByteDigiChannels = 0;

            fillProfileShimmer2(packet);
        }

        public void updateCalibrationParameters()
        {
        }
        

        public void fillProfileShimmer2(List<byte> packet) // this is the inquiry
        {
            /*
             * 
                          Inquiry Response Packet Format:
            	          Packet Type | ADC Sampling rate | Accel Range | Config Byte 0 |Num Chans | Buf size | Chan1 | Chan2 | ... | ChanX
            	   Byte:       0      |         1         |      2      |       3       |    4     |     5    |   6   |   7   | ... |   x 
            	
             **/
            //check if this packet is sane, and not just random
            if ((packet.Count >= 5) // minimum size
                && (packet.Count < (int)Shimmer2.MaxPacketSizes.ResponsePacketSize)     // maximum size
                && ((int)packet[3] <= (int)Shimmer2.MaxNumChannels.MaxNumChannels)      // max number of channels currently allowable
                && (Enum.IsDefined(typeof(Shimmer2.AccelRange), (int)packet[1])))       // ensure accel range is an allowable value
            {
                adcSamplingRate = (int)packet[0];
                accelRange = (int)packet[1];
                configSetupByte0 = (int)packet[2];
                numChannels = (int)packet[3];
                bufferSize = (int)packet[4];

                channels.Clear();

                for (int i = 0; i < numChannels; i++)
                {
                    channels.Add((int)packet[5 + i]);
                }
                isFilled = true;

                UpdateSensorsFromChannelsShimmer2();
            }
        }

        public void fillProfileShimmer3(List<byte> packet)
        {
            //check if this packet is sane, and not just random
            //check if this packet is sane, and not just random
            if ((packet.Count >= 4) // minimum size
                && (packet.Count < (int)Shimmer3.MaxPacketSizes.ResponsePacketSize))      // max number of channels currently allowable
            {
                adcSamplingRate = (int)packet[0] + ((((int)packet[1]) << 8) & 0xFF00);
                configSetupByte0 = (int)packet[2] + (((int)packet[3]) << 8) + (((int)packet[4]) << 16) + (((int)packet[5]) << 24);
                accelRange = (configSetupByte0 >> 2) & 0x03;
                gyroRange = (configSetupByte0 >>16) & 0x03;
                magGain = (configSetupByte0 >> 21) & 0x07;
                accelSamplingRate = (configSetupByte0 >> 4) & 0xF;
                mpu9150SamplingRate = (configSetupByte0 >> 8) & 0xFF;
                magSamplingRate = (configSetupByte0 >> 18) & 0x07;
                pressureResolution = (configSetupByte0 >> 28) & 0x03;

                if ((magSamplingRate == 4 && adcSamplingRate < 3200) )
                {
                    enableLowPowerMag = true;
                }

                if ((accelSamplingRate == 2 && adcSamplingRate < 3200))
                {
                    enableLowPowerAccel = true;
                }

                if ((mpu9150SamplingRate == 0xFF && adcSamplingRate < 3200))
                {
                    enableLowPowerGyro = true;
                } 

                numChannels = (int)packet[6];
                bufferSize = (int)packet[7];
                channels.Clear();

                for (int i = 0; i < numChannels; i++)
                {
                    channels.Add((int)packet[8 + i]);
                }
                isFilled = true;

                UpdateSensorsFromChannelsShimmer3();
            }
        }

        public int GetAdcSamplingRate()
        {
            return adcSamplingRate;
        }

        public void SetAdcSamplingRate(int rate)
        {
            if (Enum.IsDefined(typeof(Shimmer2.SamplingRates), rate) && (GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER2 || GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER2R))
            {
                adcSamplingRate = rate;
            }
            if (Enum.IsDefined(typeof(Shimmer3.SamplingRates), rate) && GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
            {
                adcSamplingRate = rate;
            }

        }

        public int GetBufferSize()
        {
            return bufferSize;
        }

        public void SetBufferSize(int size)
        {
            bufferSize = size;
        }


        public int GetNumChannels()
        {
            return numChannels;
        }

        public void SetNumChannels(int num)
        {
            numChannels = num;
        }

        public int GetNumAdcChannels()
        {
            return numAdcChannels;
        }

        public int GetNum1ByteDigiChannels()
        {
            return num1ByteDigiChannels;
        }

        public int GetNum2ByteDigiChannels()
        {
            return num2ByteDigiChannels;
        }

        public int GetChannel(int channelNum)
        {
            if (channelNum >= numChannels)
                return -1;
            else
                return channels[channelNum];
        }

        public bool GetIsFilled()
        {
            return isFilled;
        }

        public void SetVReg(bool val)
        {
            //vReg = val;
            if (val)
            {
                configSetupByte0 |= (int)Shimmer2.ConfigSetupByte0Bitmap.Config5VReg;
            }
            else
            {
                configSetupByte0 &= ~(int)Shimmer2.ConfigSetupByte0Bitmap.Config5VReg;   
            }
        }

        public bool GetVReg()
        {
            //return vReg;
            if ((configSetupByte0 & (int)Shimmer2.ConfigSetupByte0Bitmap.Config5VReg) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void SetPMux(bool val)
        {
            if (val)
            {
                configSetupByte0 |= (int)Shimmer2.ConfigSetupByte0Bitmap.ConfigPMux;
            }
            else
            {
                configSetupByte0 &= ~(int)Shimmer2.ConfigSetupByte0Bitmap.ConfigPMux;
            }
        }

        public bool GetPMux()
        {
            //return pMux;
            if ((configSetupByte0 & (int)Shimmer2.ConfigSetupByte0Bitmap.ConfigPMux) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void setSensors(int val)
        {
            sensors = val;
        }

        public int GetSensors()
        {
            return sensors;
        }

        public void SetAccelRange(int val)
        {
            if (Enum.IsDefined(typeof(Shimmer2.AccelRange), val) || Enum.IsDefined(typeof(Shimmer3.AccelRange), val))
                accelRange = val;
        }

        public int GetAccelRange()
        {
            return accelRange;
        }

        public int GetGyroRange()
        {
            return gyroRange;
        }

        public int GetPresRes()
        {
            return pressureResolution;
        }

        public void SetPresRes(int res)
        {
            pressureResolution = res;
        }

        public void SetGyroRange(int range)
        {
            gyroRange = range;
        }

        public int GetMagRange()
        {
            return magGain;
        }

        public void SetMagRange(int range)
        {
            magGain = range;
        }
        public string GetLoggingDelimiter()
        {
            return loggingFormat;
        }

        public void SetLoggingFormat(String value)
        {
            loggingFormat = value;
        }

        public void SetFirmwareIdentifier(double val)
        {
            firmwareIdentifier = val;
        }

        public double GetFirmwareIdentifider()
        {
            return firmwareIdentifier;
        }

        public void SetFirmwareVersion(double val)
        {
            firmwareVersion = val;
        }
        
        public double GetFirmwareVersion()
        {
            return firmwareVersion;
        }

        public void SetFirmwareInternal(int val)
        {
            firmwareInternal = val;
        }

        public int GetFirmwareInternal()
        {
            return firmwareInternal;
        }

        public void SetFirmVersionFullName(string val)
        {
            firmwareVersionFullName = val;
        }

        public int GetShimmerVersion()
        {
            return shimmerVersion;
        }

       

        public void SetShimmerVersion(int val)
        {
            shimmerVersion = val;
        }

        public String GetFirmwareVersionFullName()
        {
            return firmwareVersionFullName;
        }
        
        
        public void SetConfigSetupByte0(int val)
        {
            configSetupByte0 = val;
        }

        public int GetConfigSetupByte0()
        {
            return configSetupByte0;
        }

        public void SetGsrRange(int val)
        {
            if(Enum.IsDefined(typeof(Shimmer2.GsrRange), val))
                gsrRange = val;
        }

        public int GetGsrRange()
        {
            return gsrRange;
        }

        public void UpdateSensorsFromChannelsShimmer3()
        {
            // set the sensors value
            // crude way of getting this value, but allows for more customised firmware
            // to still work with this application
            // e.g. if any axis of the accelerometer is being transmitted, then it will
            // recognise that the accelerometer is being sampled
            sensors = 0;
            numChannels = 0;
            foreach (int channel in channels)
            {
                if ((channel == (int)Shimmer3.ChannelContents.XAAccel) ||
                    (channel == (int)Shimmer3.ChannelContents.YAAccel) ||
                    (channel == (int)Shimmer3.ChannelContents.ZAAccel))
                {
                    sensors |= (int)Shimmer3.SensorBitmap.SensorAAccel;
                }
                else if (channel == (int)Shimmer3.ChannelContents.VBatt)
                {
                    sensors |= (int)Shimmer3.SensorBitmap.SensorVBatt;
                }
                else if ((channel == (int)Shimmer3.ChannelContents.XGyro) ||
                         (channel == (int)Shimmer3.ChannelContents.YGyro) ||
                         (channel == (int)Shimmer3.ChannelContents.ZGyro))
                {
                    sensors |= (int)Shimmer3.SensorBitmap.SensorGyro;
                }
                else if ((channel == (int)Shimmer3.ChannelContents.XDAccel) ||
                         (channel == (int)Shimmer3.ChannelContents.YDAccel) ||
                         (channel == (int)Shimmer3.ChannelContents.ZDAccel))
                {
                    sensors |= (int)Shimmer3.SensorBitmap.SensorDAccel;
                }
                else if ((channel == (int)Shimmer3.ChannelContents.XMag) ||
                         (channel == (int)Shimmer3.ChannelContents.YMag) ||
                         (channel == (int)Shimmer3.ChannelContents.ZMag))
                {
                    sensors |= (int)Shimmer3.SensorBitmap.SensorMag;
                }
                else if ((channel == (int)Shimmer3.ChannelContents.ExternalAdc7))
                {
                    sensors |= (int)Shimmer3.SensorBitmap.SensorExtA7;
                }
                else if ((channel == (int)Shimmer3.ChannelContents.ExternalAdc6))
                {
                    sensors |= (int)Shimmer3.SensorBitmap.SensorExtA6;
                }
                else if ((channel == (int)Shimmer3.ChannelContents.ExternalAdc15))
                {
                    sensors |= (int)Shimmer3.SensorBitmap.SensorExtA15;
                }
                else if ((channel == (int)Shimmer3.ChannelContents.InternalAdc1))
                {
                    sensors |= (int)Shimmer3.SensorBitmap.SensorIntA1;
                }
                else if ((channel == (int)Shimmer3.ChannelContents.InternalAdc12))
                {
                    sensors |= (int)Shimmer3.SensorBitmap.SensorIntA12;
                }
                else if ((channel == (int)Shimmer3.ChannelContents.InternalAdc13))
                {
                    sensors |= (int)Shimmer3.SensorBitmap.SensorIntA13;
                }
                else if ((channel == (int)Shimmer3.ChannelContents.InternalAdc14))
                {
                    sensors |= (int)Shimmer3.SensorBitmap.SensorIntA14;
                }
                

                numChannels++;
            }
        }

        public void UpdateSensorsFromChannelsShimmer2()
        {
            // set the sensors value
            // crude way of getting this value, but allows for more customised firmware
            // to still work with this application
            // e.g. if any axis of the accelerometer is being transmitted, then it will
            // recognise that the accelerometer is being sampled
            sensors = 0;
            sensors = 0;
            numAdcChannels = 0;
            num1ByteDigiChannels = 0;
            num2ByteDigiChannels = 0;
            showMagHeading = false;
            showGsrResistance = false;
            foreach (int channel in channels)
            {
                if ((channel == (int)Shimmer2.ChannelContents.XAccel) ||
                    (channel == (int)Shimmer2.ChannelContents.YAccel) ||
                    (channel == (int)Shimmer2.ChannelContents.ZAccel))
                {
                    sensors |= (int)Shimmer2.SensorBitmap.SensorAccel;
                }
                else if ((channel == (int)Shimmer2.ChannelContents.XGyro) ||
                         (channel == (int)Shimmer2.ChannelContents.YGyro) ||
                         (channel == (int)Shimmer2.ChannelContents.ZGyro))
                {
                    sensors |= (int)Shimmer2.SensorBitmap.SensorGyro;
                }
                else if ((channel == (int)Shimmer2.ChannelContents.XMag) ||
                         (channel == (int)Shimmer2.ChannelContents.YMag) ||
                         (channel == (int)Shimmer2.ChannelContents.ZMag))
                {
                    sensors |= (int)Shimmer2.SensorBitmap.SensorMag;
                    showMagHeading = true;
                }
                else if ((channel == (int)Shimmer2.ChannelContents.EcgLaLl) ||
                         (channel == (int)Shimmer2.ChannelContents.EcgRaLl))
                {
                    sensors |= (int)Shimmer2.SensorBitmap.SensorECG;
                }
                else if (channel == (int)Shimmer2.ChannelContents.Emg)
                {
                    sensors |= (int)Shimmer2.SensorBitmap.SensorEMG;
                }
                else if (channel == (int)Shimmer2.ChannelContents.AnExA0)
                {
                    sensors |= (int)Shimmer2.SensorBitmap.SensorAnExA0;
                }
                else if (channel == (int)Shimmer2.ChannelContents.AnExA7)
                {
                    sensors |= (int)Shimmer2.SensorBitmap.SensorAnExA7;
                }
                else if ((channel == (int)Shimmer2.ChannelContents.StrainHigh) ||
                         (channel == (int)Shimmer2.ChannelContents.StrainLow))
                {
                    sensors |= (int)Shimmer2.SensorBitmap.SensorStrain;
                }
                else if ((channel == (int)Shimmer2.ChannelContents.GsrRaw) ||
                         (channel == (int)Shimmer2.ChannelContents.GsrRes))
                {
                    sensors |= (int)Shimmer2.SensorBitmap.SensorGSR;
                    showGsrResistance = true;
                }
                else if (channel == (int)Shimmer2.ChannelContents.HeartRate)
                {
                    sensors |= (int)Shimmer2.SensorBitmap.SensorHeart;
                }
                if (Enum.IsDefined(typeof(Shimmer2.AdcChannels), channel))
                {
                    numAdcChannels++;
                }
                else if (Enum.IsDefined(typeof(Shimmer2.OneByteDigiChannels), channel))
                {
                    if ((channel == (int)Shimmer2.ChannelContents.HeartRate) && firmwareVersion > 0.1)
                    {
                        num2ByteDigiChannels++;
                    }
                    else
                    {
                        num1ByteDigiChannels++;
                    }
                }
                else if (Enum.IsDefined(typeof(Shimmer2.TwoByteDigiChannels), channel))
                {
                    num2ByteDigiChannels++;
                }
            }
        }


        public void setAccelOffsetMatrix()
        {

        }




        public void UpdateChannelsFromSensorsShimmer2()
        {  
            showMagHeading = false;
            showGsrResistance = false;
            channels.Clear();
            numAdcChannels = 0;
            num1ByteDigiChannels = 0;
            num2ByteDigiChannels = 0;
            if ((sensors & (int)Shimmer2.SensorBitmap.SensorAccel) != 0)
            {
                channels.Add((int)Shimmer2.ChannelContents.XAccel);
                channels.Add((int)Shimmer2.ChannelContents.YAccel);
                channels.Add((int)Shimmer2.ChannelContents.ZAccel);
                numAdcChannels += 3;
            }
            if ((sensors & (int)Shimmer2.SensorBitmap.SensorGyro) != 0)
            {
                channels.Add((int)Shimmer2.ChannelContents.XGyro);
                channels.Add((int)Shimmer2.ChannelContents.YGyro);
                channels.Add((int)Shimmer2.ChannelContents.ZGyro);
                numAdcChannels += 3;
            }
            if ((sensors & (int)Shimmer2.SensorBitmap.SensorECG) != 0)
            {
                channels.Add((int)Shimmer2.ChannelContents.EcgRaLl);
                channels.Add((int)Shimmer2.ChannelContents.EcgLaLl);
                numAdcChannels += 2;
            }
            if ((sensors & (int)Shimmer2.SensorBitmap.SensorEMG) != 0)
            {
                channels.Add((int)Shimmer2.ChannelContents.Emg);
                numAdcChannels++;
            }
            if ((sensors & (int)Shimmer2.SensorBitmap.SensorAnExA7) != 0)
            {
                channels.Add((int)Shimmer2.ChannelContents.AnExA7);
                numAdcChannels++;
            }
            if ((sensors & (int)Shimmer2.SensorBitmap.SensorAnExA0) != 0)
            {
                channels.Add((int)Shimmer2.ChannelContents.AnExA0);
                numAdcChannels++;
            }
            if ((sensors & (int)Shimmer2.SensorBitmap.SensorStrain) != 0)
            {
                channels.Add((int)Shimmer2.ChannelContents.StrainHigh);
                channels.Add((int)Shimmer2.ChannelContents.StrainLow);
                numAdcChannels += 2;
            }
            if ((sensors & (int)Shimmer2.SensorBitmap.SensorGSR) != 0)
            {
                channels.Add((int)Shimmer2.ChannelContents.GsrRaw);
                numAdcChannels++;
                showGsrResistance = true;
            }
            if ((sensors & (int)Shimmer2.SensorBitmap.SensorMag) != 0)
            {
                channels.Add((int)Shimmer2.ChannelContents.XMag);
                channels.Add((int)Shimmer2.ChannelContents.YMag);
                channels.Add((int)Shimmer2.ChannelContents.ZMag);
                showMagHeading = true;
                num2ByteDigiChannels += 3;
            }
            if ((sensors & (int)Shimmer2.SensorBitmap.SensorHeart) != 0)
            {
                channels.Add((int)Shimmer2.ChannelContents.HeartRate);
                if (firmwareVersion > 0.1)
                {
                    num2ByteDigiChannels+=1;
                }
                else
                {
                    num1ByteDigiChannels++;
                }
            }
            numChannels = channels.Count;
        }

        public void UpdateChannelsFromSensorsShimmer3()
        {
            channels.Clear();
            numChannels = 0;
            if ((sensors & (int)Shimmer3.SensorBitmap.SensorAAccel) != 0)
            {
                channels.Add((int)Shimmer3.ChannelContents.XAAccel);
                channels.Add((int)Shimmer3.ChannelContents.YAAccel);
                channels.Add((int)Shimmer3.ChannelContents.ZAAccel);
                numChannels += 3;
            }
            if ((sensors & (int)Shimmer3.SensorBitmap.SensorVBatt) != 0)
            {
                channels.Add((int)Shimmer3.ChannelContents.VBatt);
                numChannels++;
            }
            if ((sensors & (int)Shimmer3.SensorBitmap.SensorGyro) != 0)
            {
                channels.Add((int)Shimmer3.ChannelContents.XGyro);
                channels.Add((int)Shimmer3.ChannelContents.YGyro);
                channels.Add((int)Shimmer3.ChannelContents.ZGyro);
                numChannels += 3;
            }
            if ((sensors & (int)Shimmer3.SensorBitmap.SensorDAccel) != 0)
            {
                channels.Add((int)Shimmer3.ChannelContents.XDAccel);
                channels.Add((int)Shimmer3.ChannelContents.YDAccel);
                channels.Add((int)Shimmer3.ChannelContents.ZDAccel);
                numChannels += 3;
            }
            if ((sensors & (int)Shimmer3.SensorBitmap.SensorExtA7) != 0)
            {
                channels.Add((int)Shimmer3.ChannelContents.ExternalAdc7);
                numChannels += 1;
            }
            if ((sensors & (int)Shimmer3.SensorBitmap.SensorExtA6) != 0)
            {
                channels.Add((int)Shimmer3.ChannelContents.ExternalAdc6);
                numChannels += 1;
            }
            if ((sensors & (int)Shimmer3.SensorBitmap.SensorExtA15) != 0)
            {
                channels.Add((int)Shimmer3.ChannelContents.ExternalAdc15);
                numChannels += 1;
            }
            if ((sensors & (int)Shimmer3.SensorBitmap.SensorIntA1) != 0)
            {
                channels.Add((int)Shimmer3.ChannelContents.InternalAdc1);
                numChannels += 1;
            }
            if ((sensors & (int)Shimmer3.SensorBitmap.SensorIntA12) != 0)
            {
                channels.Add((int)Shimmer3.ChannelContents.InternalAdc12);
                numChannels += 1;
            }
            if ((sensors & (int)Shimmer3.SensorBitmap.SensorIntA13) != 0)
            {
                channels.Add((int)Shimmer3.ChannelContents.InternalAdc13);
                numChannels += 1;
            }
            if ((sensors & (int)Shimmer3.SensorBitmap.SensorIntA14) != 0)
            {
                channels.Add((int)Shimmer3.ChannelContents.InternalAdc14);
                numChannels += 1;
            }
        }



    }
}
