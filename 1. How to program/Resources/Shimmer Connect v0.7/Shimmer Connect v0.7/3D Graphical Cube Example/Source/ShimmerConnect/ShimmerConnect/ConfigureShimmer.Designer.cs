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


namespace ShimmerConnect
{
	partial class Configure
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.chBxAccel = new System.Windows.Forms.CheckBox();
            this.chBxGyro = new System.Windows.Forms.CheckBox();
            this.chBxMag = new System.Windows.Forms.CheckBox();
            this.chBxECG = new System.Windows.Forms.CheckBox();
            this.chBxEMG = new System.Windows.Forms.CheckBox();
            this.chBxAnEx7 = new System.Windows.Forms.CheckBox();
            this.chBxGSR = new System.Windows.Forms.CheckBox();
            this.chBxAnEx0 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbSamplingRate = new System.Windows.Forms.ComboBox();
            this.chBx5VReg = new System.Windows.Forms.CheckBox();
            this.btnToggleLED = new System.Windows.Forms.Button();
            this.cmdAccelRange = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chBxIntADCA14 = new System.Windows.Forms.CheckBox();
            this.chBxExtADCA15 = new System.Windows.Forms.CheckBox();
            this.chBxIntADCA13 = new System.Windows.Forms.CheckBox();
            this.chBxExtADCA6 = new System.Windows.Forms.CheckBox();
            this.chBxIntADCA12 = new System.Windows.Forms.CheckBox();
            this.chBxIntADCA1 = new System.Windows.Forms.CheckBox();
            this.chBxExtADCA7 = new System.Windows.Forms.CheckBox();
            this.chBxStrain = new System.Windows.Forms.CheckBox();
            this.chBxHeartRate = new System.Windows.Forms.CheckBox();
            this.chBxPwrMux = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdGsrRange = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chBxTab = new System.Windows.Forms.CheckBox();
            this.cbBxComma = new System.Windows.Forms.CheckBox();
            this.chBx3D = new System.Windows.Forms.CheckBox();
            this.checkBoxGyroOnTheFly = new System.Windows.Forms.CheckBox();
            this.chBxMagLowPower = new System.Windows.Forms.CheckBox();
            this.chBxAccelLowPower = new System.Windows.Forms.CheckBox();
            this.chBxGyroLowPower = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmdGyroRange = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmdMagRange = new System.Windows.Forms.ComboBox();
            this.chBxPressure = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmdPresRes = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoEllipsis = true;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 449);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Accel Range";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 669);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 24);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(130, 669);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(88, 24);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chBxAccel
            // 
            this.chBxAccel.AutoSize = true;
            this.chBxAccel.Location = new System.Drawing.Point(6, 19);
            this.chBxAccel.Name = "chBxAccel";
            this.chBxAccel.Size = new System.Drawing.Size(94, 17);
            this.chBxAccel.TabIndex = 2;
            this.chBxAccel.Text = "Accelerometer";
            this.chBxAccel.UseVisualStyleBackColor = true;
            this.chBxAccel.CheckedChanged += new System.EventHandler(this.chBxAccel_CheckedChanged);
            // 
            // chBxGyro
            // 
            this.chBxGyro.AutoSize = true;
            this.chBxGyro.Location = new System.Drawing.Point(6, 42);
            this.chBxGyro.Name = "chBxGyro";
            this.chBxGyro.Size = new System.Drawing.Size(77, 17);
            this.chBxGyro.TabIndex = 4;
            this.chBxGyro.Text = "Gyroscope";
            this.chBxGyro.UseVisualStyleBackColor = true;
            this.chBxGyro.CheckedChanged += new System.EventHandler(this.chBxGyro_CheckedChanged);
            // 
            // chBxMag
            // 
            this.chBxMag.AutoSize = true;
            this.chBxMag.Location = new System.Drawing.Point(6, 65);
            this.chBxMag.Name = "chBxMag";
            this.chBxMag.Size = new System.Drawing.Size(94, 17);
            this.chBxMag.TabIndex = 5;
            this.chBxMag.Text = "Magnetometer";
            this.chBxMag.UseVisualStyleBackColor = true;
            this.chBxMag.CheckedChanged += new System.EventHandler(this.chBxMag_CheckedChanged);
            // 
            // chBxECG
            // 
            this.chBxECG.AutoSize = true;
            this.chBxECG.Location = new System.Drawing.Point(6, 88);
            this.chBxECG.Name = "chBxECG";
            this.chBxECG.Size = new System.Drawing.Size(48, 17);
            this.chBxECG.TabIndex = 6;
            this.chBxECG.Text = "ECG";
            this.chBxECG.UseVisualStyleBackColor = true;
            this.chBxECG.CheckedChanged += new System.EventHandler(this.chBxECG_CheckedChanged);
            // 
            // chBxEMG
            // 
            this.chBxEMG.AutoSize = true;
            this.chBxEMG.Location = new System.Drawing.Point(118, 19);
            this.chBxEMG.Name = "chBxEMG";
            this.chBxEMG.Size = new System.Drawing.Size(50, 17);
            this.chBxEMG.TabIndex = 7;
            this.chBxEMG.Text = "EMG";
            this.chBxEMG.UseVisualStyleBackColor = true;
            this.chBxEMG.CheckedChanged += new System.EventHandler(this.chBxEMG_CheckedChanged);
            // 
            // chBxAnEx7
            // 
            this.chBxAnEx7.AutoSize = true;
            this.chBxAnEx7.Location = new System.Drawing.Point(118, 88);
            this.chBxAnEx7.Name = "chBxAnEx7";
            this.chBxAnEx7.Size = new System.Drawing.Size(103, 17);
            this.chBxAnEx7.TabIndex = 10;
            this.chBxAnEx7.Text = "ExpBoard ADC7";
            this.chBxAnEx7.UseVisualStyleBackColor = true;
            this.chBxAnEx7.CheckedChanged += new System.EventHandler(this.chBxAnEx7_CheckedChanged);
            // 
            // chBxGSR
            // 
            this.chBxGSR.AutoSize = true;
            this.chBxGSR.Location = new System.Drawing.Point(118, 42);
            this.chBxGSR.Name = "chBxGSR";
            this.chBxGSR.Size = new System.Drawing.Size(49, 17);
            this.chBxGSR.TabIndex = 8;
            this.chBxGSR.Text = "GSR";
            this.chBxGSR.UseVisualStyleBackColor = true;
            this.chBxGSR.CheckedChanged += new System.EventHandler(this.chBxGSR_CheckedChanged);
            // 
            // chBxAnEx0
            // 
            this.chBxAnEx0.AutoSize = true;
            this.chBxAnEx0.Location = new System.Drawing.Point(118, 65);
            this.chBxAnEx0.Name = "chBxAnEx0";
            this.chBxAnEx0.Size = new System.Drawing.Size(103, 17);
            this.chBxAnEx0.TabIndex = 9;
            this.chBxAnEx0.Text = "ExpBoard ADC0";
            this.chBxAnEx0.UseVisualStyleBackColor = true;
            this.chBxAnEx0.CheckedChanged += new System.EventHandler(this.chBxAnEx0_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Sampling Rate";
            // 
            // cmbSamplingRate
            // 
            this.cmbSamplingRate.FormattingEnabled = true;
            this.cmbSamplingRate.Location = new System.Drawing.Point(12, 19);
            this.cmbSamplingRate.Name = "cmbSamplingRate";
            this.cmbSamplingRate.Size = new System.Drawing.Size(94, 21);
            this.cmbSamplingRate.TabIndex = 12;
            this.cmbSamplingRate.SelectedIndexChanged += new System.EventHandler(this.cmbSamplingRate_SelectedIndexChanged);
            // 
            // chBx5VReg
            // 
            this.chBx5VReg.AutoSize = true;
            this.chBx5VReg.Location = new System.Drawing.Point(18, 278);
            this.chBx5VReg.Name = "chBx5VReg";
            this.chBx5VReg.Size = new System.Drawing.Size(119, 17);
            this.chBx5VReg.TabIndex = 13;
            this.chBx5VReg.Text = "Enable 5V regulator";
            this.chBx5VReg.UseVisualStyleBackColor = true;
            this.chBx5VReg.CheckedChanged += new System.EventHandler(this.chBx5VReg_CheckedChanged);
            // 
            // btnToggleLED
            // 
            this.btnToggleLED.Location = new System.Drawing.Point(130, 19);
            this.btnToggleLED.Name = "btnToggleLED";
            this.btnToggleLED.Size = new System.Drawing.Size(88, 23);
            this.btnToggleLED.TabIndex = 14;
            this.btnToggleLED.Text = "Toggle LED";
            this.btnToggleLED.UseVisualStyleBackColor = true;
            this.btnToggleLED.Click += new System.EventHandler(this.btnToggleLED_Click);
            // 
            // cmdAccelRange
            // 
            this.cmdAccelRange.FormattingEnabled = true;
            this.cmdAccelRange.Location = new System.Drawing.Point(12, 465);
            this.cmdAccelRange.Name = "cmdAccelRange";
            this.cmdAccelRange.Size = new System.Drawing.Size(94, 21);
            this.cmdAccelRange.TabIndex = 15;
            this.cmdAccelRange.SelectedIndexChanged += new System.EventHandler(this.cmdAccelRange_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chBxPressure);
            this.groupBox1.Controls.Add(this.chBxIntADCA14);
            this.groupBox1.Controls.Add(this.chBxExtADCA15);
            this.groupBox1.Controls.Add(this.chBxIntADCA13);
            this.groupBox1.Controls.Add(this.chBxExtADCA6);
            this.groupBox1.Controls.Add(this.chBxIntADCA12);
            this.groupBox1.Controls.Add(this.chBxIntADCA1);
            this.groupBox1.Controls.Add(this.chBxExtADCA7);
            this.groupBox1.Controls.Add(this.chBxStrain);
            this.groupBox1.Controls.Add(this.chBxHeartRate);
            this.groupBox1.Controls.Add(this.chBxAccel);
            this.groupBox1.Controls.Add(this.chBxGyro);
            this.groupBox1.Controls.Add(this.chBxMag);
            this.groupBox1.Controls.Add(this.chBxECG);
            this.groupBox1.Controls.Add(this.chBxEMG);
            this.groupBox1.Controls.Add(this.chBxGSR);
            this.groupBox1.Controls.Add(this.chBxAnEx0);
            this.groupBox1.Controls.Add(this.chBxAnEx7);
            this.groupBox1.Location = new System.Drawing.Point(12, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 224);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sensors to sample";
            // 
            // chBxIntADCA14
            // 
            this.chBxIntADCA14.AutoSize = true;
            this.chBxIntADCA14.Location = new System.Drawing.Point(118, 180);
            this.chBxIntADCA14.Name = "chBxIntADCA14";
            this.chBxIntADCA14.Size = new System.Drawing.Size(85, 17);
            this.chBxIntADCA14.TabIndex = 28;
            this.chBxIntADCA14.Text = "Int ADC A14";
            this.chBxIntADCA14.UseVisualStyleBackColor = true;
            // 
            // chBxExtADCA15
            // 
            this.chBxExtADCA15.AutoSize = true;
            this.chBxExtADCA15.Location = new System.Drawing.Point(118, 134);
            this.chBxExtADCA15.Name = "chBxExtADCA15";
            this.chBxExtADCA15.Size = new System.Drawing.Size(88, 17);
            this.chBxExtADCA15.TabIndex = 15;
            this.chBxExtADCA15.Text = "Ext ADC A15";
            this.chBxExtADCA15.UseVisualStyleBackColor = true;
            // 
            // chBxIntADCA13
            // 
            this.chBxIntADCA13.AutoSize = true;
            this.chBxIntADCA13.Location = new System.Drawing.Point(6, 180);
            this.chBxIntADCA13.Name = "chBxIntADCA13";
            this.chBxIntADCA13.Size = new System.Drawing.Size(85, 17);
            this.chBxIntADCA13.TabIndex = 27;
            this.chBxIntADCA13.Text = "Int ADC A13";
            this.chBxIntADCA13.UseVisualStyleBackColor = true;
            // 
            // chBxExtADCA6
            // 
            this.chBxExtADCA6.AutoSize = true;
            this.chBxExtADCA6.Location = new System.Drawing.Point(6, 134);
            this.chBxExtADCA6.Name = "chBxExtADCA6";
            this.chBxExtADCA6.Size = new System.Drawing.Size(82, 17);
            this.chBxExtADCA6.TabIndex = 14;
            this.chBxExtADCA6.Text = "Ext ADC A6";
            this.chBxExtADCA6.UseVisualStyleBackColor = true;
            // 
            // chBxIntADCA12
            // 
            this.chBxIntADCA12.AutoSize = true;
            this.chBxIntADCA12.Location = new System.Drawing.Point(118, 157);
            this.chBxIntADCA12.Name = "chBxIntADCA12";
            this.chBxIntADCA12.Size = new System.Drawing.Size(85, 17);
            this.chBxIntADCA12.TabIndex = 26;
            this.chBxIntADCA12.Text = "Int ADC A12";
            this.chBxIntADCA12.UseVisualStyleBackColor = true;
            // 
            // chBxIntADCA1
            // 
            this.chBxIntADCA1.AutoSize = true;
            this.chBxIntADCA1.Location = new System.Drawing.Point(6, 157);
            this.chBxIntADCA1.Name = "chBxIntADCA1";
            this.chBxIntADCA1.Size = new System.Drawing.Size(79, 17);
            this.chBxIntADCA1.TabIndex = 25;
            this.chBxIntADCA1.Text = "Int ADC A1";
            this.chBxIntADCA1.UseVisualStyleBackColor = true;
            // 
            // chBxExtADCA7
            // 
            this.chBxExtADCA7.AutoSize = true;
            this.chBxExtADCA7.Location = new System.Drawing.Point(118, 111);
            this.chBxExtADCA7.Name = "chBxExtADCA7";
            this.chBxExtADCA7.Size = new System.Drawing.Size(82, 17);
            this.chBxExtADCA7.TabIndex = 13;
            this.chBxExtADCA7.Text = "Ext ADC A7";
            this.chBxExtADCA7.UseVisualStyleBackColor = true;
            // 
            // chBxStrain
            // 
            this.chBxStrain.AutoSize = true;
            this.chBxStrain.Location = new System.Drawing.Point(6, 111);
            this.chBxStrain.Name = "chBxStrain";
            this.chBxStrain.Size = new System.Drawing.Size(88, 17);
            this.chBxStrain.TabIndex = 11;
            this.chBxStrain.Text = "Strain Gauge";
            this.chBxStrain.UseVisualStyleBackColor = true;
            this.chBxStrain.CheckedChanged += new System.EventHandler(this.chBxStrain_CheckedChanged);
            // 
            // chBxHeartRate
            // 
            this.chBxHeartRate.Location = new System.Drawing.Point(118, 111);
            this.chBxHeartRate.Name = "chBxHeartRate";
            this.chBxHeartRate.Size = new System.Drawing.Size(0, 0);
            this.chBxHeartRate.TabIndex = 12;
            this.chBxHeartRate.Text = "Heart Rate";
            this.chBxHeartRate.UseVisualStyleBackColor = true;
            this.chBxHeartRate.CheckedChanged += new System.EventHandler(this.chBxHeartRate_CheckedChanged);
            // 
            // chBxPwrMux
            // 
            this.chBxPwrMux.AutoSize = true;
            this.chBxPwrMux.Location = new System.Drawing.Point(18, 301);
            this.chBxPwrMux.Name = "chBxPwrMux";
            this.chBxPwrMux.Size = new System.Drawing.Size(149, 17);
            this.chBxPwrMux.TabIndex = 18;
            this.chBxPwrMux.Text = "Enable Voltage monitoring";
            this.chBxPwrMux.UseVisualStyleBackColor = true;
            this.chBxPwrMux.CheckedChanged += new System.EventHandler(this.chBxPwrMux_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(121, 449);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "GSR Range";
            // 
            // cmdGsrRange
            // 
            this.cmdGsrRange.FormattingEnabled = true;
            this.cmdGsrRange.Location = new System.Drawing.Point(124, 465);
            this.cmdGsrRange.Name = "cmdGsrRange";
            this.cmdGsrRange.Size = new System.Drawing.Size(94, 21);
            this.cmdGsrRange.TabIndex = 19;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chBxTab);
            this.groupBox2.Controls.Add(this.cbBxComma);
            this.groupBox2.Location = new System.Drawing.Point(12, 596);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(235, 65);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Logging Delimiter Format";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // chBxTab
            // 
            this.chBxTab.AutoSize = true;
            this.chBxTab.Location = new System.Drawing.Point(7, 43);
            this.chBxTab.Name = "chBxTab";
            this.chBxTab.Size = new System.Drawing.Size(45, 17);
            this.chBxTab.TabIndex = 1;
            this.chBxTab.Text = "Tab";
            this.chBxTab.UseVisualStyleBackColor = true;
            this.chBxTab.CheckedChanged += new System.EventHandler(this.chBxTab_CheckedChanged);
            // 
            // cbBxComma
            // 
            this.cbBxComma.AutoSize = true;
            this.cbBxComma.Location = new System.Drawing.Point(7, 20);
            this.cbBxComma.Name = "cbBxComma";
            this.cbBxComma.Size = new System.Drawing.Size(61, 17);
            this.cbBxComma.TabIndex = 0;
            this.cbBxComma.Text = "Comma";
            this.cbBxComma.UseVisualStyleBackColor = true;
            this.cbBxComma.CheckedChanged += new System.EventHandler(this.cbBxComma_CheckedChanged);
            // 
            // chBx3D
            // 
            this.chBx3D.AutoSize = true;
            this.chBx3D.Location = new System.Drawing.Point(18, 324);
            this.chBx3D.Name = "chBx3D";
            this.chBx3D.Size = new System.Drawing.Size(130, 17);
            this.chBx3D.TabIndex = 22;
            this.chBx3D.Text = "Enable 3D Orientation";
            this.chBx3D.UseVisualStyleBackColor = true;
            this.chBx3D.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBoxGyroOnTheFly
            // 
            this.checkBoxGyroOnTheFly.AutoSize = true;
            this.checkBoxGyroOnTheFly.Location = new System.Drawing.Point(18, 347);
            this.checkBoxGyroOnTheFly.Name = "checkBoxGyroOnTheFly";
            this.checkBoxGyroOnTheFly.Size = new System.Drawing.Size(191, 17);
            this.checkBoxGyroOnTheFly.TabIndex = 23;
            this.checkBoxGyroOnTheFly.Text = "Enable Gyro On-The-Fly Calibration";
            this.checkBoxGyroOnTheFly.UseVisualStyleBackColor = true;
            this.checkBoxGyroOnTheFly.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // chBxMagLowPower
            // 
            this.chBxMagLowPower.AutoSize = true;
            this.chBxMagLowPower.Location = new System.Drawing.Point(18, 370);
            this.chBxMagLowPower.Name = "chBxMagLowPower";
            this.chBxMagLowPower.Size = new System.Drawing.Size(186, 17);
            this.chBxMagLowPower.TabIndex = 24;
            this.chBxMagLowPower.Text = "Enable Low Power Magnetometer";
            this.chBxMagLowPower.UseVisualStyleBackColor = true;
            this.chBxMagLowPower.CheckedChanged += new System.EventHandler(this.chBxMagLowPower_CheckedChanged);
            // 
            // chBxAccelLowPower
            // 
            this.chBxAccelLowPower.AutoSize = true;
            this.chBxAccelLowPower.Location = new System.Drawing.Point(18, 393);
            this.chBxAccelLowPower.Name = "chBxAccelLowPower";
            this.chBxAccelLowPower.Size = new System.Drawing.Size(186, 17);
            this.chBxAccelLowPower.TabIndex = 25;
            this.chBxAccelLowPower.Text = "Enable Low Power Accelerometer";
            this.chBxAccelLowPower.UseVisualStyleBackColor = true;
            this.chBxAccelLowPower.CheckedChanged += new System.EventHandler(this.chBxAccelLowPower_CheckedChanged);
            // 
            // chBxGyroLowPower
            // 
            this.chBxGyroLowPower.AutoSize = true;
            this.chBxGyroLowPower.Location = new System.Drawing.Point(18, 416);
            this.chBxGyroLowPower.Name = "chBxGyroLowPower";
            this.chBxGyroLowPower.Size = new System.Drawing.Size(169, 17);
            this.chBxGyroLowPower.TabIndex = 26;
            this.chBxGyroLowPower.Text = "Enable Low Power Gyroscope";
            this.chBxGyroLowPower.UseVisualStyleBackColor = true;
            this.chBxGyroLowPower.CheckedChanged += new System.EventHandler(this.chBxGyroLowPower_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoEllipsis = true;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 491);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Gyro Range";
            // 
            // cmdGyroRange
            // 
            this.cmdGyroRange.FormattingEnabled = true;
            this.cmdGyroRange.Location = new System.Drawing.Point(12, 507);
            this.cmdGyroRange.Name = "cmdGyroRange";
            this.cmdGyroRange.Size = new System.Drawing.Size(94, 21);
            this.cmdGyroRange.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.AutoEllipsis = true;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(121, 491);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Mag Range";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // cmdMagRange
            // 
            this.cmdMagRange.FormattingEnabled = true;
            this.cmdMagRange.Location = new System.Drawing.Point(124, 507);
            this.cmdMagRange.Name = "cmdMagRange";
            this.cmdMagRange.Size = new System.Drawing.Size(94, 21);
            this.cmdMagRange.TabIndex = 29;
            this.cmdMagRange.SelectedIndexChanged += new System.EventHandler(this.cmdMagRange_SelectedIndexChanged);
            // 
            // chBxPressure
            // 
            this.chBxPressure.AutoSize = true;
            this.chBxPressure.Location = new System.Drawing.Point(6, 203);
            this.chBxPressure.Name = "chBxPressure";
            this.chBxPressure.Size = new System.Drawing.Size(67, 17);
            this.chBxPressure.TabIndex = 29;
            this.chBxPressure.Text = "Pressure";
            this.chBxPressure.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoEllipsis = true;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 539);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 13);
            this.label6.TabIndex = 34;
            this.label6.Text = "Pressure Resolution";
            // 
            // cmdPresRes
            // 
            this.cmdPresRes.FormattingEnabled = true;
            this.cmdPresRes.Location = new System.Drawing.Point(12, 555);
            this.cmdPresRes.Name = "cmdPresRes";
            this.cmdPresRes.Size = new System.Drawing.Size(94, 21);
            this.cmdPresRes.TabIndex = 33;
            this.cmdPresRes.SelectedIndexChanged += new System.EventHandler(this.cmdPresRes_SelectedIndexChanged);
            // 
            // Configure
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(284, 782);
            this.ControlBox = false;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmdPresRes);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmdMagRange);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmdGyroRange);
            this.Controls.Add(this.chBxGyroLowPower);
            this.Controls.Add(this.chBxAccelLowPower);
            this.Controls.Add(this.chBxMagLowPower);
            this.Controls.Add(this.checkBoxGyroOnTheFly);
            this.Controls.Add(this.chBx3D);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdGsrRange);
            this.Controls.Add(this.chBxPwrMux);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmdAccelRange);
            this.Controls.Add(this.btnToggleLED);
            this.Controls.Add(this.chBx5VReg);
            this.Controls.Add(this.cmbSamplingRate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.MaximumSize = new System.Drawing.Size(300, 820);
            this.MinimumSize = new System.Drawing.Size(300, 720);
            this.Name = "Configure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure";
            this.Load += new System.EventHandler(this.Configure_Load);
            this.Shown += new System.EventHandler(this.Configure_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.CheckBox chBxAccel;
		private System.Windows.Forms.CheckBox chBxGyro;
		private System.Windows.Forms.CheckBox chBxMag;
		private System.Windows.Forms.CheckBox chBxECG;
		private System.Windows.Forms.CheckBox chBxEMG;
		private System.Windows.Forms.CheckBox chBxAnEx7;
		private System.Windows.Forms.CheckBox chBxGSR;
		private System.Windows.Forms.CheckBox chBxAnEx0;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbSamplingRate;
		private System.Windows.Forms.CheckBox chBx5VReg;
		private System.Windows.Forms.Button btnToggleLED;
		private System.Windows.Forms.ComboBox cmdAccelRange;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox chBxPwrMux;
		private System.Windows.Forms.CheckBox chBxStrain;
		private System.Windows.Forms.CheckBox chBxHeartRate;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.ComboBox cmdGsrRange;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.CheckBox cbBxComma;
      private System.Windows.Forms.CheckBox chBxTab;
      private System.Windows.Forms.CheckBox chBx3D;
      private System.Windows.Forms.CheckBox checkBoxGyroOnTheFly;
      private System.Windows.Forms.CheckBox chBxMagLowPower;
      private System.Windows.Forms.CheckBox chBxIntADCA14;
      private System.Windows.Forms.CheckBox chBxExtADCA15;
      private System.Windows.Forms.CheckBox chBxIntADCA13;
      private System.Windows.Forms.CheckBox chBxExtADCA6;
      private System.Windows.Forms.CheckBox chBxIntADCA12;
      private System.Windows.Forms.CheckBox chBxIntADCA1;
      private System.Windows.Forms.CheckBox chBxExtADCA7;
      private System.Windows.Forms.CheckBox chBxAccelLowPower;
      private System.Windows.Forms.CheckBox chBxGyroLowPower;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.ComboBox cmdGyroRange;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.ComboBox cmdMagRange;
      private System.Windows.Forms.CheckBox chBxPressure;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.ComboBox cmdPresRes;
	}
}
