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


using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ShimmerConnect
{
    public partial class Configure : Form
    {
        private bool accelActive;
        private bool gyroActive;
        private bool magActive;
        private bool ecgActive;
        private bool emgActive;
        private bool gsrActive;
        private bool anex0Active;
        private bool anex7Active;
        private bool strainActive;
        private bool heartActive;
        private bool extAdcA7Active;
        private bool extAdcA6Active;
        private bool extAdcA15Active;
        private bool intAdcA1Active;
        private bool intAdcA12Active;
        private bool intAdcA13Active;
        private bool intAdcA14Active;
        private bool pressureActive;
        private bool old5VregSetting = false;
        private bool changeMagMode = false;
        private bool changeAccelMode = false;
        private bool changeGyroMode = false;

        public ShimmerProfile pProfile;
        public Control pControlForm;


        public Configure()
        {
            InitializeComponent();
            
            accelActive = false;
            gyroActive = false;
            magActive = false;
            ecgActive = false;
            emgActive = false;
            gsrActive = false;
            anex0Active = false;
            anex7Active = false;
            strainActive = false;
            heartActive = false;
            extAdcA7Active = false;
            extAdcA6Active = false;
            extAdcA15Active = false;
            intAdcA1Active = false;
            intAdcA12Active = false;
            intAdcA13Active = false;
            intAdcA14Active = false;
            pressureActive = false;

            foreach (string range in Shimmer2.AccelRangeString)
            {
                cmdAccelRange.Items.Add(range);
            }
            cmdAccelRange.Enabled = false;
            foreach (string range in Shimmer2.GsrRangeString)
            {
                cmdGsrRange.Items.Add(range);
            }
            cmdGsrRange.Enabled = false;
        }

        public Configure(Control controlForm, ShimmerProfile profile)
            : this()
        {
            pControlForm = controlForm;
            pProfile = profile;
        }

        private void Configure_Shown(object sender, EventArgs e)
        {
            SplitSensors(pProfile.GetSensors());
            cmdAccelRange.Items.Clear();
            cmdGyroRange.Items.Clear();
            cmdMagRange.Items.Clear();
            cmdPresRes.Items.Clear();
            if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
            {
                foreach (string range in Shimmer3.AccelRangeString)
                {
                    cmdAccelRange.Items.Add(range);
                }
                cmdAccelRange.Enabled = false;

                foreach (string range in Shimmer3.GyroRangeString)
                {
                    cmdGyroRange.Items.Add(range);
                }
                cmdGyroRange.Enabled = false;

                foreach (string range in Shimmer3.MagRangeString)
                {
                    cmdMagRange.Items.Add(range);
                }
                cmdMagRange.Enabled = false;

                foreach (string range in Shimmer3.PressureResolutionString)
                {
                    cmdPresRes.Items.Add(range);
                }
                cmdPresRes.Enabled = false;

            }
            else
            {
                foreach (string range in Shimmer2.AccelRangeString)
                {
                    cmdAccelRange.Items.Add(range);
                }
                cmdAccelRange.Enabled = false;

                foreach (string range in Shimmer2.MagRangeString)
                {
                    cmdMagRange.Items.Add(range);
                }
                cmdMagRange.Enabled = false;
            }


            if (pProfile != null)
            {
                cmbSamplingRate.Items.Clear();
                if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER2 || pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER2R)
                {
                    foreach (string rate in Shimmer2.SamplingRatesString)
                    {
                        cmbSamplingRate.Items.Add(rate);
                    }
                }
                else if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
                {
                    foreach (string rate in Shimmer3.SamplingRatesString)
                    {
                        cmbSamplingRate.Items.Add(rate);
                    }
                }
                if (pProfile.GetShimmerVersion() == 3)
                {
                    chBxECG.Enabled = false;
                    chBxEMG.Enabled = false;
                    chBxGSR.Enabled = false;
                    chBxAnEx0.Enabled = false;
                    chBxStrain.Enabled = false;
                    chBxHeartRate.Enabled = false;
                    chBxAnEx7.Text = "VSenseBatt";
                    chBx5VReg.Enabled = false;
                    chBxMagLowPower.Enabled = true;
                    chBxPwrMux.Enabled = false;
                    chBxExtADCA7.Enabled = true;
                    chBxExtADCA6.Enabled = true;
                    chBxExtADCA15.Enabled = true;
                    chBxIntADCA1.Enabled = true;
                    chBxIntADCA12.Enabled = true;
                    chBxIntADCA13.Enabled = true;
                    chBxIntADCA14.Enabled = true;
                    chBxMagLowPower.Enabled = true;
                    chBxAccelLowPower.Enabled = true;
                    chBxGyroLowPower.Enabled = true;
                    chBxPressure.Enabled = true;
                }
                else
                {
                    chBxAccelLowPower.Enabled = false;
                    chBxGyroLowPower.Enabled = false;
                    chBxECG.Enabled = true;
                    chBxEMG.Enabled = true;
                    chBxGSR.Enabled = true;
                    chBxAnEx0.Enabled = true;
                    chBxAnEx7.Enabled = true;
                    chBxStrain.Enabled = true;
                    chBxHeartRate.Enabled = true;
                    chBx5VReg.Enabled = true;
                    chBxMagLowPower.Enabled = true;
                    chBxAccelLowPower.Enabled = false;
                    chBxGyroLowPower.Enabled = false;
                    chBxPwrMux.Enabled = true;
                    chBxExtADCA7.Enabled = false;
                    chBxExtADCA6.Enabled = false;
                    chBxExtADCA15.Enabled = false;
                    chBxIntADCA1.Enabled = false;
                    chBxIntADCA12.Enabled = false;
                    chBxIntADCA13.Enabled = false;
                    chBxIntADCA14.Enabled = false;
                    chBxPressure.Enabled = false;
                }
            }


            cmbSamplingRate.SelectedIndex = SamplingRateToList(pProfile.GetAdcSamplingRate());
            chBxAccel.Checked = accelActive;
            chBxGyro.Checked = gyroActive;
            chBxMag.Checked = magActive;
            chBxECG.Checked = ecgActive;
            chBxEMG.Checked = emgActive;
            chBxGSR.Checked = gsrActive;
            chBxAnEx0.Checked = anex0Active;
            chBxAnEx7.Checked = anex7Active;
            chBxStrain.Checked = strainActive;
            chBxHeartRate.Checked = heartActive;
            chBxMagLowPower.Checked = pProfile.enableLowPowerMag;
            chBxAccelLowPower.Checked = pProfile.enableLowPowerAccel;
            chBxGyroLowPower.Checked = pProfile.enableLowPowerGyro;
            chBx5VReg.Checked = pProfile.GetVReg();
            chBxExtADCA7.Checked = extAdcA7Active;
            chBxExtADCA6.Checked = extAdcA6Active;
            chBxExtADCA15.Checked = extAdcA15Active;
            chBxIntADCA1.Checked = intAdcA1Active;
            chBxIntADCA12.Checked = intAdcA12Active;
            chBxIntADCA13.Checked = intAdcA13Active;
            chBxIntADCA14.Checked = intAdcA14Active;
            chBxPressure.Checked = pressureActive;
            changeMagMode = false;
            changeAccelMode = false;
            changeGyroMode = false;
            if (pProfile.GetLoggingDelimiter().Equals(","))
            {
                cbBxComma.Checked = true;
                chBxTab.Checked = false;
            }
            if (pProfile.GetLoggingDelimiter().Equals("\t"))
            {
                cbBxComma.Checked = false;
                chBxTab.Checked = true;
            }
            if ((pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3))
            {
                if (chBxStrain.Checked)
                    chBx5VReg.Enabled = false;
                else
                    chBx5VReg.Enabled = true;
            }
            chBxPwrMux.Checked = pProfile.GetPMux();
            if (chBxPwrMux.Checked)
            {
                chBxAnEx0.Text = "VSenseReg";
                chBxAnEx7.Text = "VSenseBatt";
            }
            
            cmdAccelRange.SelectedIndex = pProfile.GetAccelRange();
            if ((pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3))
            {
                cmdGyroRange.SelectedIndex = pProfile.GetGyroRange();
                cmdPresRes.SelectedIndex = pProfile.GetPresRes();
                cmdMagRange.SelectedIndex = pProfile.GetMagRange() - 1;
            }
            else
            {
                cmdMagRange.SelectedIndex = pProfile.GetMagRange();
            }
            if (accelActive)
            {
                cmdAccelRange.Enabled = true;
            }
            else
            {
                cmdAccelRange.Enabled = false;
            }

            if (gyroActive && pProfile.GetShimmerVersion()==(int)Shimmer.ShimmerVersion.SHIMMER3){
                cmdGyroRange.Enabled = true;
            }
            else 
            {
                cmdGyroRange.Enabled = false;
            }

            if (pressureActive && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
            {
                cmdPresRes.Enabled = true;
            }
            else
            {
                cmdPresRes.Enabled = false;
            }

            if (magActive && !pProfile.GetFirmwareVersionFullName().Equals("BoilerPlate 0.1.0"))
            {
                cmdMagRange.Enabled = true;
            }
            else
            {
                cmdMagRange.Enabled = false;
            }


            cmdGsrRange.SelectedIndex = pProfile.GetGsrRange();
            if (gsrActive)
                cmdGsrRange.Enabled = true;
            else
                cmdGsrRange.Enabled = false;
        }

        private int combineSensors()
        {
            int val =  0 ;
            if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
            {
                if (accelActive)
                    val |= (int)Shimmer2.SensorBitmap.SensorAccel;
                if (gyroActive)
                    val |= (int)Shimmer2.SensorBitmap.SensorGyro;
                if (magActive)
                    val |= (int)Shimmer2.SensorBitmap.SensorMag;
                if (ecgActive)
                    val |= (int)Shimmer2.SensorBitmap.SensorECG;
                if (emgActive)
                    val |= (int)Shimmer2.SensorBitmap.SensorEMG;
                if (gsrActive)
                    val |= (int)Shimmer2.SensorBitmap.SensorGSR;
                if (anex0Active)
                    val |= (int)Shimmer2.SensorBitmap.SensorAnExA0;
                if (anex7Active)
                    val |= (int)Shimmer2.SensorBitmap.SensorAnExA7;
                if (strainActive)
                    val |= (int)Shimmer2.SensorBitmap.SensorStrain;
                if (heartActive)
                    val |= (int)Shimmer2.SensorBitmap.SensorHeart;

                return val;
            }
            else
            {
                
                if (accelActive)
                    if (cmdAccelRange.SelectedIndex == 0)
                    {
                        val |= (int)Shimmer3.SensorBitmap.SensorAAccel;
                    }
                    else
                    {
                        val |= (int)Shimmer3.SensorBitmap.SensorDAccel;
                    }
                if (gyroActive)
                    val |= (int)Shimmer3.SensorBitmap.SensorGyro;
                if (anex7Active)
                    val |= (int)Shimmer3.SensorBitmap.SensorVBatt;
                //if (dAccelActive)
                 //   val |= (int)Shimmer3.Sensor0Bitmap.SensorDAccel;
                if (magActive)
                    val |= (int)Shimmer3.SensorBitmap.SensorMag;
                if (extAdcA7Active)
                    val |= (int)Shimmer3.SensorBitmap.SensorExtA7;
                if (extAdcA6Active)
                    val |= (int)Shimmer3.SensorBitmap.SensorExtA6;
                if (extAdcA15Active)
                    val |= (int)Shimmer3.SensorBitmap.SensorExtA15;
                if (intAdcA1Active)
                    val |= (int)Shimmer3.SensorBitmap.SensorIntA1;
                if (intAdcA12Active)
                    val |= (int)Shimmer3.SensorBitmap.SensorIntA12;
                if (intAdcA13Active)
                    val |= (int)Shimmer3.SensorBitmap.SensorIntA13;
                if (intAdcA14Active)
                    val |= (int)Shimmer3.SensorBitmap.SensorIntA14;
                if (pressureActive)
                    val |= (int)Shimmer3.SensorBitmap.SensorPressure;
                return val;
            }
         
        }

        private void SplitSensors(int val)
        {
            if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
            {
                if ((val & (int)Shimmer2.SensorBitmap.SensorAccel) != 0)
                    accelActive = true;
                else
                    accelActive = false;
                if ((val & (int)Shimmer2.SensorBitmap.SensorGyro) != 0)
                    gyroActive = true;
                else
                    gyroActive = false;
                if ((val & (int)Shimmer2.SensorBitmap.SensorMag) != 0)
                    magActive = true;
                else
                    magActive = false;
                if ((val & (int)Shimmer2.SensorBitmap.SensorECG) != 0)
                    ecgActive = true;
                else
                    ecgActive = false;
                if ((val & (int)Shimmer2.SensorBitmap.SensorEMG) != 0)
                    emgActive = true;
                else
                    emgActive = false;
                if ((val & (int)Shimmer2.SensorBitmap.SensorGSR) != 0)
                    gsrActive = true;
                else
                    gsrActive = false;
                if ((val & (int)Shimmer2.SensorBitmap.SensorAnExA0) != 0)
                    anex0Active = true;
                else
                    anex0Active = false;
                if ((val & (int)Shimmer2.SensorBitmap.SensorAnExA7) != 0)
                    anex7Active = true;
                else
                    anex7Active = false;
                if ((val & (int)Shimmer2.SensorBitmap.SensorStrain) != 0)
                    strainActive = true;
                else
                    strainActive = false;
                if ((val & (int)Shimmer2.SensorBitmap.SensorHeart) != 0)
                    heartActive = true;
                else
                    heartActive = false;
            }
            else
            {
                ecgActive = false;
                emgActive = false;
                gsrActive = false;
                strainActive = false;
                if (((val & (int)Shimmer3.SensorBitmap.SensorAAccel) != 0) || ((val & (int)Shimmer3.SensorBitmap.SensorDAccel) != 0))
                    accelActive = true;
                else
                    accelActive = false;
                if ((val & (int)Shimmer3.SensorBitmap.SensorGyro) != 0)
                    gyroActive = true;
                else
                    gyroActive = false;
                if ((val & (int)Shimmer3.SensorBitmap.SensorVBatt) != 0)
                    anex7Active = true;
                else
                    anex7Active = false;
                if ((val & (int)Shimmer3.SensorBitmap.SensorMag) != 0)
                    magActive = true;
                else
                    magActive = false;
                if ((val & (int)Shimmer3.SensorBitmap.SensorExtA7) != 0)
                    extAdcA7Active = true;
                else
                    extAdcA7Active = false;
                if ((val & (int)Shimmer3.SensorBitmap.SensorExtA6) != 0)
                    extAdcA6Active = true;
                else
                    extAdcA6Active = false;
                if ((val & (int)Shimmer3.SensorBitmap.SensorExtA15) != 0)
                    extAdcA15Active = true;
                else
                    extAdcA15Active = false;
                if ((val & (int)Shimmer3.SensorBitmap.SensorIntA1) != 0)
                    intAdcA1Active = true;
                else
                    intAdcA1Active = false;
                if ((val & (int)Shimmer3.SensorBitmap.SensorIntA12) != 0)
                    intAdcA12Active = true;
                else
                    intAdcA1Active = false;
                if ((val & (int)Shimmer3.SensorBitmap.SensorIntA13) != 0)
                    intAdcA13Active = true;
                else
                    intAdcA13Active = false;
                if ((val & (int)Shimmer3.SensorBitmap.SensorIntA14) != 0)
                    intAdcA14Active = true;
                else
                    intAdcA14Active = false;
                if ((val & (int)Shimmer3.SensorBitmap.SensorPressure) != 0)
                    pressureActive = true;
                else
                    pressureActive = false;

            }
        }

        private int SamplingRateToList(int val)
        {
            int ret=0;
            if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
            {
                switch (val)
                {
                    case (int)Shimmer2.SamplingRates.Sampling0HzOff:
                        ret = 0;
                        break;
                    case (int)Shimmer2.SamplingRates.Sampling10Hz:
                        ret = 1;
                        break;
                    case (int)Shimmer2.SamplingRates.Sampling50Hz:
                        ret = 2;
                        break;
                    case (int)Shimmer2.SamplingRates.Sampling100Hz:
                        ret = 3;
                        break;
                    case (int)Shimmer2.SamplingRates.Sampling125Hz:
                        ret = 4;
                        break;
                    case (int)Shimmer2.SamplingRates.Sampling166Hz:
                        ret = 5;
                        break;
                    case (int)Shimmer2.SamplingRates.Sampling200Hz:
                        ret = 6;
                        break;
                    case (int)Shimmer2.SamplingRates.Sampling250Hz:
                        ret = 7;
                        break;
                    case (int)Shimmer2.SamplingRates.Sampling500Hz:
                        ret = 8;
                        break;
                    case (int)Shimmer2.SamplingRates.Sampling1000Hz:
                        ret = 9;
                        break;
                    default:
                        ret = -1;
                        break;
                }

            }
            else
            {
                switch (val)
            {
                case (int)Shimmer3.SamplingRates.Sampling1Hz:
                    ret = 0;
                    break;
                case (int)Shimmer3.SamplingRates.Sampling10Hz:
                    ret = 1;
                    break;
                case (int)Shimmer3.SamplingRates.Sampling50Hz:
                    ret = 2;
                    break;
                case (int)Shimmer3.SamplingRates.Sampling100Hz:
                    ret = 3;
                    break;
                case (int)Shimmer3.SamplingRates.Sampling200Hz:
                    ret = 4;
                    break;
                case (int)Shimmer3.SamplingRates.Sampling250Hz:
                    ret = 5;
                    break;
                case (int)Shimmer3.SamplingRates.Sampling500Hz:
                    ret = 6;
                    break;
                case (int)Shimmer3.SamplingRates.Sampling1000Hz:
                    ret = 7;
                    break;
                default:
                    ret = -1;
                    break;
                }
            }
            return ret;
        }

        private int ListToSamplingRate(int val)
        {
            int ret;
            if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
            {
                switch (val)
                {
                    case 0:
                        ret = (int)Shimmer2.SamplingRates.Sampling0HzOff;
                        break;
                    case 1:
                        ret = (int)Shimmer2.SamplingRates.Sampling10Hz;
                        break;
                    case 2:
                        ret = (int)Shimmer2.SamplingRates.Sampling50Hz;
                        break;
                    case 3:
                        ret = (int)Shimmer2.SamplingRates.Sampling100Hz;
                        break;
                    case 4:
                        ret = (int)Shimmer2.SamplingRates.Sampling125Hz;
                        break;
                    case 5:
                        ret = (int)Shimmer2.SamplingRates.Sampling166Hz;
                        break;
                    case 6:
                        ret = (int)Shimmer2.SamplingRates.Sampling200Hz;
                        break;
                    case 7:
                        ret = (int)Shimmer2.SamplingRates.Sampling250Hz;
                        break;
                    case 8:
                        ret = (int)Shimmer2.SamplingRates.Sampling500Hz;
                        break;
                    case 9:
                        ret = (int)Shimmer2.SamplingRates.Sampling1000Hz;
                        break;
                    default:
                        ret = -1;
                        break;
                }
            }
            else
            {
                switch (val)
                {
                    case 0:
                        ret = (int)Shimmer3.SamplingRates.Sampling1Hz;
                        break;
                    case 1:
                        ret = (int)Shimmer3.SamplingRates.Sampling10Hz;
                        break;
                    case 2:
                        ret = (int)Shimmer3.SamplingRates.Sampling50Hz;
                        break;
                    case 3:
                        ret = (int)Shimmer3.SamplingRates.Sampling100Hz;
                        break;
                    case 4:
                        ret = (int)Shimmer3.SamplingRates.Sampling200Hz;
                        break;
                    case 5:
                        ret = (int)Shimmer3.SamplingRates.Sampling250Hz;
                        break;
                    case 6:
                        ret = (int)Shimmer3.SamplingRates.Sampling500Hz;
                        break;
                    case 7:
                        ret = (int)Shimmer3.SamplingRates.Sampling1000Hz;
                        break;
                    default:
                        ret = -1;
                        break;
                }
            }
            return ret;
        }

        private void chBxAccel_CheckedChanged(object sender, EventArgs e)
        {
            if (chBxAccel.Checked)
                cmdAccelRange.Enabled = true;
            else
            {
                cmdAccelRange.Enabled = false;
                pProfile.enable3DOrientation = false;
                chBx3D.Checked = false;
            }
        }

        private void chBxGyro_CheckedChanged(object sender, EventArgs e)
        {
            if (chBxGyro.Checked)
            {
                if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                {
                    chBxECG.Checked = false;
                    chBxEMG.Checked = false;
                    chBxGSR.Checked = false;
                    chBxStrain.Checked = false;
                }
                if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
                {
                    cmdGyroRange.Enabled = true;
                }
            }
            else
            {   
                cmdGyroRange.Enabled = false;
                pProfile.enable3DOrientation = false;
                chBx3D.Checked = false;
            }
        }

        private void chBxMag_CheckedChanged(object sender, EventArgs e)
        {
            if (chBxMag.Checked)
            {
                if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                {
                    chBxECG.Checked = false;
                    chBxEMG.Checked = false;
                    chBxGSR.Checked = false;
                    chBxStrain.Checked = false;
                }
                cmdMagRange.Enabled = true;
                
            }
            else
            {
                cmdMagRange.Enabled = false;
                pProfile.enable3DOrientation = false;
                chBx3D.Checked = false;
            }
        }

        private void chBxECG_CheckedChanged(object sender, EventArgs e)
        {
            if (chBxECG.Checked)
            {
                if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                {
                    chBxGyro.Checked = false;
                    chBxMag.Checked = false;
                    chBxEMG.Checked = false;
                    chBxGSR.Checked = false;
                    chBxStrain.Checked = false;
                }
            }
        }

        private void chBxEMG_CheckedChanged(object sender, EventArgs e)
        {
            if (chBxEMG.Checked)
            {
                if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                {
                    chBxGyro.Checked = false;
                    chBxMag.Checked = false;
                    chBxECG.Checked = false;
                    chBxGSR.Checked = false;
                    chBxStrain.Checked = false;
                }
            }
        }

        private void chBxGSR_CheckedChanged(object sender, EventArgs e)
        {
            if (chBxGSR.Checked)
            {
                if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                {
                    chBxGyro.Checked = false;
                    chBxMag.Checked = false;
                    chBxECG.Checked = false;
                    chBxEMG.Checked = false;
                    chBxStrain.Checked = false;
                    cmdGsrRange.Enabled = true;
                }
            }
            else
                cmdGsrRange.Enabled = false;
        }

        private void chBxAnEx0_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void chBxAnEx7_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void chBxStrain_CheckedChanged(object sender, EventArgs e)
        {
            if (chBxStrain.Checked)
            {
                chBxGyro.Checked = false;
                chBxMag.Checked = false;
                chBxECG.Checked = false;
                chBxEMG.Checked = false;
                chBxGSR.Checked = false;
                old5VregSetting = pProfile.GetVReg();
                chBx5VReg.Enabled = false;
            }
            else
            {
                chBx5VReg.Checked = old5VregSetting;
                chBx5VReg.Enabled = true;
            }
        }

        private void chBxHeartRate_CheckedChanged(object sender, EventArgs e)
        {
            // it is valid to be able to select HR and AnEx channels simultaneously
            // as the HR does not use either of the ADC channels and the voltage monitoring can still work
        }

        private void chBxPwrMux_CheckedChanged(object sender, EventArgs e)
        {
            if (chBxPwrMux.Checked)
            {
                chBxAnEx0.Text = "VSenseReg";
                chBxAnEx7.Text = "VSenseBatt";
            }
            else
            {
                chBxAnEx0.Text = "ExpBoard ADC0";
                chBxAnEx7.Text = "ExpBoard ADC7";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool changeSensors = false;

            // check sampling rate
            if (SamplingRateToList(pProfile.GetAdcSamplingRate()) != cmbSamplingRate.SelectedIndex || changeMagMode || changeAccelMode || changeGyroMode)
            {
                if (!pControlForm.isStreaming)
                {
                    if (SamplingRateToList(pProfile.GetAdcSamplingRate()) != cmbSamplingRate.SelectedIndex)
                    {
                        pProfile.SetAdcSamplingRate(ListToSamplingRate(cmbSamplingRate.SelectedIndex));
                        pProfile.changeSamplingRate = true;
                    }


                    if (changeMagMode)
                    {
                        pProfile.changeSamplingRate = true;
                        if (chBxMagLowPower.Checked)
                        {
                            pProfile.enableLowPowerMag = true;
                        }
                        else
                        {
                            pProfile.enableLowPowerMag = false;
                        }
                    }

                    if (changeAccelMode)
                    {
                        pProfile.changeSamplingRate = true;
                        if (chBxAccelLowPower.Checked)
                        {
                            pProfile.enableLowPowerAccel = true;
                        }
                        else
                        {
                            pProfile.enableLowPowerAccel = false;
                        }
                    }

                    if (changeGyroMode)
                    {
                        pProfile.changeSamplingRate = true;
                        if (chBxGyroLowPower.Checked)
                        {
                            pProfile.enableLowPowerGyro = true;
                        }
                        else
                        {
                            pProfile.enableLowPowerGyro = false;
                        }
                    }
                }
                else
                {
                    if (changeMagMode)
                    {
                        MessageBox.Show("Cannot change mag sampling rate when Shimmer is Streaming ", Shimmer.ApplicationName,
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Cannot change sampling rate when Shimmer is Streaming ", Shimmer.ApplicationName,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            // check sensors
            if (chBxAccel.Checked != accelActive)
            {
                accelActive = chBxAccel.Checked;
                changeSensors = true;
            }
            if (chBxGyro.Checked != gyroActive)
            {
                gyroActive = chBxGyro.Checked;
                changeSensors = true;
            }
            if (chBxMag.Checked != magActive)
            {
                magActive = chBxMag.Checked;
                changeSensors = true;
            }
            if (chBxECG.Checked != ecgActive)
            {
                ecgActive = chBxECG.Checked;
                changeSensors = true;
            }
            if (chBxEMG.Checked != emgActive)
            {
                emgActive = chBxEMG.Checked;
                changeSensors = true;
            }
            if (chBxGSR.Checked != gsrActive)
            {
                gsrActive = chBxGSR.Checked;
                changeSensors = true;
            }
            if (chBxAnEx0.Checked != anex0Active)
            {
                anex0Active = chBxAnEx0.Checked;
                changeSensors = true;
            }
            if (chBxAnEx7.Checked != anex7Active)
            {
                anex7Active = chBxAnEx7.Checked;
                changeSensors = true;
            }
            if (chBxStrain.Checked != strainActive)
            {
                strainActive = chBxStrain.Checked;
                changeSensors = true;
            }
            if (chBxHeartRate.Checked != heartActive)
            {
                heartActive = chBxHeartRate.Checked;
                changeSensors = true;
            }
            if (chBxExtADCA7.Checked != extAdcA7Active)
            {
                extAdcA7Active = chBxExtADCA7.Checked;
                changeSensors = true;
            }
            if (chBxExtADCA6.Checked != extAdcA6Active)
            {
                extAdcA6Active = chBxExtADCA6.Checked;
                changeSensors = true;
            }
            if (chBxExtADCA15.Checked != extAdcA15Active)
            {
                extAdcA15Active = chBxExtADCA15.Checked;
                
            }
            if (chBxIntADCA1.Checked != intAdcA1Active)
            {
                intAdcA1Active = chBxIntADCA1.Checked;
                changeSensors = true;
            }
            if (chBxIntADCA12.Checked != intAdcA12Active)
            {
                intAdcA12Active = chBxIntADCA12.Checked;
                changeSensors = true;
            }
            if (chBxIntADCA13.Checked != intAdcA13Active)
            {
                intAdcA13Active = chBxIntADCA13.Checked;
                changeSensors = true;
            }
            if (chBxIntADCA14.Checked != intAdcA14Active)
            {
                intAdcA14Active = chBxIntADCA14.Checked;
                changeSensors = true;
            }

            if (chBxPressure.Checked != pressureActive)
            {
                pressureActive = chBxPressure.Checked;
                changeSensors = true;
            }

            if (changeSensors)
            {
                if (pControlForm.isStreaming)
                {
                    MessageBox.Show("Cannot enable/disable sensors while streaming data ", Shimmer.ApplicationName,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    pProfile.setSensors(combineSensors());
                    pProfile.changeSensors = true;
                }
            }
            // check 5V reg
            if ((chBx5VReg.Enabled) && (chBx5VReg.Checked != pProfile.GetVReg()))
            {
                pProfile.SetVReg(chBx5VReg.Checked);
                pProfile.change5Vreg = true;
            }
            // check Power MUX
            if (chBxPwrMux.Checked != pProfile.GetPMux())
            {
                pProfile.SetPMux(chBxPwrMux.Checked);
                pProfile.changePwrMux = true;
            }
            // check accel range
            if (pProfile.GetAccelRange() != cmdAccelRange.SelectedIndex)
            {
                if (pControlForm.isStreaming)
                {
                    MessageBox.Show("Cannot change range while streaming data ", Shimmer.ApplicationName,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    pProfile.SetAccelRange(cmdAccelRange.SelectedIndex);
                    pProfile.changeAccelSens = true;
                    pProfile.setSensors(combineSensors());
                    pProfile.changeSensors = true;
                }
            }
            // check gyro range
            if (pProfile.GetGyroRange() != cmdGyroRange.SelectedIndex)
            {
                if (pControlForm.isStreaming)
                {
                    MessageBox.Show("Cannot change range while streaming data ", Shimmer.ApplicationName,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    pProfile.SetGyroRange(cmdGyroRange.SelectedIndex);
                    pProfile.changeGyroSens = true;
                    pProfile.setSensors(combineSensors());
                    pProfile.changeSensors = true;
                }
            }

            //check pressure resolution
            if (pProfile.GetPresRes() != cmdPresRes.SelectedIndex)
            {
                if (pControlForm.isStreaming)
                {
                    MessageBox.Show("Cannot change setting while streaming data ", Shimmer.ApplicationName,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    pProfile.SetPresRes(cmdGyroRange.SelectedIndex);
                    pProfile.changePresRes = true;
                    pProfile.setSensors(combineSensors());
                    pProfile.changeSensors = true;
                }
            }

            // check mag range
            if (pProfile.GetMagRange() != cmdMagRange.SelectedIndex + 1 && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
            {
                if (pControlForm.isStreaming)
                {
                    MessageBox.Show("Cannot change range while streaming data ", Shimmer.ApplicationName,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    pProfile.SetMagRange(cmdMagRange.SelectedIndex + 1);
                    pProfile.changeMagSens = true;
                    pProfile.setSensors(combineSensors());
                    pProfile.changeSensors = true;
                }
            }
            else //if Shimmer2
            {
                if (pProfile.GetMagRange() != cmdMagRange.SelectedIndex)
                {
                    if (pControlForm.isStreaming)
                    {
                        MessageBox.Show("Cannot change range while streaming data ", Shimmer.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        pProfile.SetMagRange(cmdMagRange.SelectedIndex);
                        pProfile.changeMagSens = true;
                        pProfile.setSensors(combineSensors());
                        pProfile.changeSensors = true;
                    }
                }
            }
            // check gsr range
            if (pProfile.GetGsrRange() != cmdGsrRange.SelectedIndex)
            {
                if (pControlForm.isStreaming)
                {
                    MessageBox.Show("Cannot enable/disable sensors while streaming data ", Shimmer.ApplicationName,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    pProfile.SetGsrRange(cmdGsrRange.SelectedIndex);
                    pProfile.changeGsrRange = true;
                }
            }
        }

        private void btnToggleLED_Click(object sender, EventArgs e)
        {
            pControlForm.ToggleLED();
        }

        private void Configure_Load(object sender, EventArgs e)
        {

        }

        private void cmdAccelRange_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }


    

        private void cbBxComma_CheckedChanged(object sender, EventArgs e)
        {
            if (cbBxComma.Checked)
            {
                chBxTab.Checked = false;
                pProfile.SetLoggingFormat(",");
            }
        }

        private void chBxTab_CheckedChanged(object sender, EventArgs e)
        {
            if (chBxTab.Checked)
            {
                cbBxComma.Checked = false;
                pProfile.SetLoggingFormat("\t");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chBx3D.Checked)
            {
                if (pControlForm.isStreaming)
                {
                    MessageBox.Show("Cannot enable 3D Orientation while streaming, please stop streaming first before enabling 3D Orientation. ", Shimmer.ApplicationName,
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    chBx3D.Checked = false;
                } else {
                    chBxAccel.Checked = true;
                    chBxGyro.Checked = true;
                    chBxMag.Checked = true;
                    pProfile.enable3DOrientation = true;
                }
            }
            else
            {
                pProfile.enable3DOrientation = false;
            }
        }

        private void chBx5VReg_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBoxGyroOnTheFly.Checked)
            {
                chBxGyro.Checked = true;
                pControlForm.enableGyroOnTheFlyCalibration = true;
            } else {
                pControlForm.enableGyroOnTheFlyCalibration = false;
            }
        }

        private void chBxMagLowPower_CheckedChanged(object sender, EventArgs e)
        {
            changeMagMode = true;
            
        }

        private void cmbSamplingRate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chBxAccelLowPower_CheckedChanged(object sender, EventArgs e)
        {
            changeAccelMode = true;
        }

        private void chBxGyroLowPower_CheckedChanged(object sender, EventArgs e)
        {
            changeGyroMode = true;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void cmdMagRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void cmdPresRes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
    }
}
