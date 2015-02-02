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
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace ShimmerConnect
{
    public partial class Control : Form
    {
        private volatile bool stopReading = false;
        private volatile bool sensorsChangePending = false;
        private Thread readThread;
        private List<TextBox> channelTextBox = new List<TextBox>();
        private List<Label> channelLabel = new List<Label>();
        private TextBox extraInfoTextBox = new TextBox();
        private Label extraInfoLabel = new Label();
        private Boolean setOrientation = false;
        private double[,] setOriMatrix = new double[,] { { 1, 0, 0 }, { 0, -1, 0 }, { 0, 0, -1 } };

        private delegate void SetTextChannelsCallback(ShimmerDataPacket packet);
        private delegate void ShowChannelTextBoxesCallback();
        GradDes3DOrientation mOrientation;
        private ShimmerProfile pProfile;
        private List<GraphForm> pChannelPlot;
        private StreamWriter pCsvFile;
        private bool pUpdateGraphs;
        private bool pSaveToFile;
        public delegate void ShowGraphsDelegate();
        ShowGraphsDelegate PShowGraphs;
        public delegate void ChangeStatusLabelDelegate(string text);
        ChangeStatusLabelDelegate PChangeStatusLabel;
        public bool isStreaming;
        public string delimiter;
        public Boolean enableGyroOnTheFlyCalibration = false;
        private int listSizeGyroOnTheFly = 100;
        private double thresholdGyroOnTheFly = 1.2;
        private List<double> gyroXCalList = new List<double>();
        private List<double> gyroYCalList = new List<double>();
        private List<double> gyroZCalList = new List<double>();
        private List<double> gyroXRawList = new List<double>();
        private List<double> gyroYRawList = new List<double>();
        private List<double> gyroZRawList = new List<double>();
        private MainForm mainForm;
        Visual3DForm mForm3D;
        Quaternion quat;
        private Point[] textBoxLocation = new Point[] {
#if _PLATFORM_LINUX
            new Point(10, 106),
            new Point(125, 106),
            new Point(240, 106),
            new Point(10, 147),
            new Point(125, 147),
            new Point(240, 147),
            new Point(10, 188),
            new Point(125, 188),
            new Point(240, 188),
            new Point(10, 229),
            new Point(125, 229),
            new Point(240, 229),
            new Point(10, 270),
            new Point(125, 270),
            new Point(240, 270),
            new Point(10, 311),
            new Point(125, 311),
            new Point(240, 311),
            new Point(10, 352),
            new Point(125, 352),
            new Point(240, 352),
            new Point(10, 393),
            new Point(125, 393),
            new Point(240, 393),
            new Point(10, 434),
            new Point(125, 434),
            new Point(240, 434),
            new Point(10, 475),
            new Point(125, 475),
            new Point(240, 475)
#else
            // WINDOWS
            new Point(10, 97),
            new Point(107, 97),
            new Point(204, 97),
            new Point(10, 137),
            new Point(107, 137),
            new Point(204, 137),
            new Point(10, 177),
            new Point(107, 177),
            new Point(204, 177),
            new Point(10, 217),
            new Point(107, 217),
            new Point(204, 217),
            new Point(10, 257),
            new Point(107, 257),
            new Point(204, 257),
            new Point(10, 297),
            new Point(107, 297),
            new Point(204, 297),
            new Point(10, 337),
            new Point(107, 337),
            new Point(204, 337),
            new Point(10, 377),
            new Point(107, 377),
            new Point(204, 377)
#endif
        };
        private Point[] labelLocation = new Point[] {
#if _PLATFORM_LINUX
            new Point(12, 88),
            new Point(129, 88),
            new Point(242, 88),
            new Point(12, 129),
            new Point(129, 129),
            new Point(242, 129),
            new Point(12, 170),
            new Point(129, 170),
            new Point(242, 170),
            new Point(12, 211),
            new Point(129, 211),
            new Point(242, 211), 
            new Point(12, 252),
            new Point(129, 252),
            new Point(242, 252),
            new Point(12, 293),
            new Point(129, 293),
            new Point(242, 293),
            new Point(12, 334),
            new Point(129, 334),
            new Point(242, 334),
            new Point(12, 375),
            new Point(129, 375),
            new Point(242, 375),
            new Point(12, 416),
            new Point(129, 416),
            new Point(242, 416)
#else
            // WINDOWS
            new Point(12, 81),
            new Point(109, 81),
            new Point(206, 81),
            new Point(12, 121),
            new Point(109, 121),
            new Point(206, 121),
            new Point(12, 161),
            new Point(109, 161),
            new Point(206, 161),
            new Point(12, 201),
            new Point(109, 201),
            new Point(206, 201), 
            new Point(12, 241),
            new Point(109, 241),
            new Point(206, 241),
            new Point(12, 281),
            new Point(109, 281),
            new Point(206, 281),
            new Point(12, 321),
            new Point(109, 321),
            new Point(206, 321),
            new Point(12, 361),
            new Point(109, 361),
            new Point(206, 361)
#endif
        };

        private void FillSerialCmbBox()
        {
            cmbComPortSelect.Items.Clear();
            
#if _PLATFORM_LINUX
            cmbComPortSelect.Items.Add("/dev/rfcomm0");
#endif
            string[] serialPorts = SerialPort.GetPortNames();
            foreach (string port in serialPorts)
            {
                cmbComPortSelect.Items.Add(port);
            }
        }

        public Control()
        {
            InitializeComponent();
            isStreaming = false;
            btnDisconnect.Enabled = false;
            btnStart.Enabled = false;
            btnStop.Enabled = false;
            FillSerialCmbBox();
        }

        public Control(ShimmerProfile profile, List<GraphForm> channelPlot, bool saveToFile, StreamWriter csvFile, 
            bool updateGraphs, ShowGraphsDelegate ShowGraphs, ChangeStatusLabelDelegate ChangeStatusLabel, MainForm mainForm, Visual3DForm form3D)
            : this()
        { 
            pProfile = profile;
            pChannelPlot = channelPlot;
            pSaveToFile = saveToFile;
            pCsvFile = csvFile;
            pUpdateGraphs = updateGraphs;
            PShowGraphs = ShowGraphs;
            PChangeStatusLabel = ChangeStatusLabel;
            this.mainForm = mainForm;
            mForm3D = form3D;
            mForm3D.Text = "3D Orientation";
            mForm3D.setControl(this);
        }

 
        private void cmbComPortSelect_DropDown(object sender, EventArgs e)
        {
            FillSerialCmbBox();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (cmbComPortSelect.Text != "")               
                serialPort1.PortName = cmbComPortSelect.Text;
            try
            {
                serialPort1.Open();
                serialPort1.DiscardInBuffer();
                serialPort1.DiscardOutBuffer();
                PChangeStatusLabel("Connected to " + serialPort1.PortName);
                stopReading = false;
                readThread = new Thread(new ThreadStart(ReadData));
                readThread.Start();
                // change configuration if required
                // ChangeConfiguration();
                // give the shimmer time to make the changes before continuing (required?)
                System.Threading.Thread.Sleep(500);
                // Read Shimmer Profile
                if (serialPort1.IsOpen)
                {

                    Debug.Write("Get FW version");
                    // Set default firmware version values, if there is not response it means that this values remain, and the old firmware version has been detected
                    // The following are the three main identifiers used to identify the firmware version
                    pProfile.SetFirmwareIdentifier(1);
                    pProfile.SetFirmwareVersion(0.1);
                    pProfile.SetFirmwareInternal(0);
                    pProfile.SetFirmVersionFullName("BoilerPlate 0.1.0");
                    serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.GET_FW_VERSION_COMMAND}, 0, 1);
                    System.Threading.Thread.Sleep(200);

                    serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.GET_FW_VERSION_COMMAND }, 0, 1);
                    System.Threading.Thread.Sleep(200);

                    if (!pProfile.GetFirmwareVersionFullName().Equals("BoilerPlate 0.1.0"))
                    {
                        if (pProfile.GetFirmwareVersion() != 1.2)
                        {
                            serialPort1.Write(new byte[1] { (byte)Shimmer3.PacketType.GET_SHIMMER_VERSION_COMMAND }, 0, 1);
                        }
                        else
                        {
                            serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.GET_SHIMMER_VERSION_COMMAND }, 0, 1);

                        }
                        System.Threading.Thread.Sleep(400);
                    }
                    else
                    {
                        // if Boilerplate version is definitely 2r
                        pProfile.SetShimmerVersion((int)Shimmer.ShimmerVersion.SHIMMER2R);
                    }
                    if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER2R || pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER2)
                    {
                        initializeShimmer2();
                    }
                    else if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
                    {
                        initializeShimmer3();
                    }

                    this.mainForm.enableConfigFormMenu(true);
                    this.mainForm.setShimmerVersion(pProfile.GetShimmerVersion()); // this is to configure the graphs
                    
                }
                if (pSaveToFile)
                {
                    System.Threading.Thread.Sleep(100);
                    if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
                    {
                        WriteHeaderToFileShimmer3();
                    }
                    else
                    {
                        WriteHeaderToFileShimmer2();
                    }
                }
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                btnStart.Enabled = true;
                btnStop.Enabled = true;
                cmbComPortSelect.Enabled = false;
            }
            catch
            {
                cmbComPortSelect.SelectedText = "";
                PChangeStatusLabel("Cannot connect to specified serial port!");
                MessageBox.Show("Cannot open " + serialPort1.PortName, Shimmer.ApplicationName,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }            
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void Disconnect()
        {
            if (serialPort1.IsOpen)
            {
                stopReading = true;
                PChangeStatusLabel("Disconnected");
            }
            this.mainForm.enableConfigFormMenu(false);
            this.mainForm.RemoveGraphs();
            RemoveChannelTextBoxes();
            isStreaming = false;
            btnDisconnect.Enabled = false;
            btnStart.Enabled = false;
            btnStop.Enabled = false;
            btnConnect.Enabled = true;
            cmbComPortSelect.Enabled = true;
            
        }

        private void RemoveChannelTextBoxes()
        {
            while (channelTextBox.Count !=0)
            {
                this.Controls.Remove(channelTextBox[0]);
                channelTextBox.RemoveAt(0);

                this.Controls.Remove(channelLabel[0]);
                channelLabel.RemoveAt(0);
            }
            if (pProfile.showMagHeading || pProfile.showGsrResistance)
            {
                this.Controls.Remove(extraInfoTextBox);
                this.Controls.Remove(extraInfoLabel);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                if (pProfile.GetIsFilled())
                {

                   
                        System.Threading.Thread.Sleep(500);
                        if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
                        {
                        }
                        else
                        {

                            serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.GET_BLINK_LED }, 0, 1);
                            System.Threading.Thread.Sleep(500);

                        }
                        ShowChannelTextBoxes();
                        setOriMatrix = new double[,] { { 1, 0, 0 }, { 0, -1, 0 }, { 0, 0, -1 } };
                        if (pUpdateGraphs)
                        {
                            PShowGraphs();
                        }
                        serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.START_STREAMING_COMMAND }, 0, 1);
                        pProfile.mFirstTimeCalTime = true;
                        pProfile.mLastReceivedCalibratedTimeStamp = -1;
                        pProfile.mPacketReceptionRate = 100;
                        double samplingRate = 0;
                        if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                        {
                            samplingRate = 1024 / pProfile.GetAdcSamplingRate();
                        }
                        else
                        {
                            samplingRate = 32768 / pProfile.GetAdcSamplingRate();
                        }
                        mOrientation = new GradDes3DOrientation(0.1, (double)1 / samplingRate, 1, 0, 0, 0);
                        isStreaming = true;
                        if ((pProfile.GetSensors() & (int)Shimmer2.SensorBitmap.SensorStrain) != 0)
                        {
                            pProfile.SetVReg(true);
                        }
                    
                }

                else
                {
                    MessageBox.Show("Failed to read configuration information from shimmer. Please ensure correct shimmer is connected", Shimmer.ApplicationName,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
                MessageBox.Show("No serial port is open", Shimmer.ApplicationName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.STOP_STREAMING_COMMAND }, 0, 1);
                isStreaming = false;
                if ((pProfile.GetSensors() & (int)Shimmer2.SensorBitmap.SensorStrain) != 0)
                    pProfile.SetVReg(false);

                while (serialPort1.BytesToRead != 0)
                {
                    serialPort1.ReadByte();
                }
            }
            else
                MessageBox.Show("No serial port is open", Shimmer.ApplicationName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ReadData()
        {
            List<byte> buffer = new List<byte>();
            int i;
            byte[] bufferbyte;
            serialPort1.DiscardInBuffer();
            while (!stopReading)    //continues to read for approx 1s (until serial read timeout)
            {
                try
                {
                    switch (serialPort1.ReadByte())
                    {
                        case (byte)Shimmer2.PacketType.DATA_PACKET:
                            if (pProfile.GetIsFilled())
                            {
                                if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                                {
                                    for (i = 0; i < (2 + ((pProfile.GetNumAdcChannels() + pProfile.GetNum2ByteDigiChannels()) * 2) + pProfile.GetNum1ByteDigiChannels()); i++)
                                    {
                                        buffer.Add((byte)serialPort1.ReadByte());
                                    }
                                    ShimmerDataPacket packet = new ShimmerDataPacket(buffer, pProfile.GetNumAdcChannels(), pProfile.GetNum1ByteDigiChannels(), pProfile.GetNum2ByteDigiChannels(), pProfile.GetShimmerVersion(), pProfile.GetNumChannels(), pProfile);
                                    if (packet.GetIsFilled())
                                    {
                                        SetTextChannels(packet);
                                    }
                                }
                                else
                                {
                                    for (i = 0; i < (2 + (pProfile.GetNumChannels() * 2)); i++)
                                    {
                                        buffer.Add((byte)serialPort1.ReadByte());
                                    }
                                    ShimmerDataPacket packet = new ShimmerDataPacket(buffer, pProfile.GetNumAdcChannels(), pProfile.GetNum1ByteDigiChannels(), pProfile.GetNum2ByteDigiChannels(), pProfile.GetShimmerVersion(), pProfile.GetNumChannels(), pProfile);
                                    if (packet.GetIsFilled() && (pProfile.GetNumChannels() > 0))
                                    {
                                        SetTextChannels(packet);
                                    }
                                }
                                buffer.Clear();
                            }
                            break;

                        case (byte)Shimmer2.PacketType.INQUIRY_RESPONSE:
                            if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER2 || pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER2R)
                            {
                                for (i = 0; i < 5; i++)
                                {
                                    // get Sampling rate, accel range, config setup byte0, num chans and buffer size
                                    buffer.Add((byte)serialPort1.ReadByte());
                                }
                                for (i = 0; i < (int)buffer[3]; i++)
                                {
                                    // read each channel type for the num channels
                                    buffer.Add((byte)serialPort1.ReadByte());
                                }
                                pProfile.fillProfileShimmer2(buffer);

                                if (sensorsChangePending)
                                {
                                    
                                    if (pSaveToFile)
                                    {
                                        if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER2R || pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER2)
                                        {
                                            WriteHeaderToFileShimmer2();
                                        }
                                        else if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
                                        {
                                            WriteHeaderToFileShimmer3();
                                        }
                                    }
                                }
                                else
                                {
                                   
                                }
                                
                                sensorsChangePending = false;
                            }
                            else if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
                            {
                                for (i = 0; i < 8; i++)
                                {
                                    // get Sampling rate, accel range, config setup byte0, num chans and buffer size
                                    buffer.Add((byte)serialPort1.ReadByte());
                                }
                                for (i = 0; i < (int)buffer[6]; i++)
                                {
                                    // read each channel type for the num channels
                                    buffer.Add((byte)serialPort1.ReadByte());
                                }
                                pProfile.fillProfileShimmer3(buffer);
                                
                                if (sensorsChangePending)
                                {
                                    
                                   
                                    if (pSaveToFile)
                                    {
                                        if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER2R || pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER2)
                                        {
                                            WriteHeaderToFileShimmer2();
                                        }
                                        else if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
                                        {
                                            WriteHeaderToFileShimmer3();
                                        }
                                    }
                                }
                                else
                                {
                                   
                                }

                                sensorsChangePending = false;
                            }
                            buffer.Clear();
                            break;

                        case (byte)Shimmer2.PacketType.SAMPLING_RATE_RESPONSE:
                            if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
                            {
                                int value = 0;
                                value = (int)serialPort1.ReadByte();
                                value += (((int)serialPort1.ReadByte() << 8) & 0xFF00);
                                pProfile.SetAdcSamplingRate(value);

                            } else {
                                pProfile.SetAdcSamplingRate(serialPort1.ReadByte());
                            }
                            break;

                        case (byte)Shimmer2.PacketType.ACCEL_RANGE_RESPONSE:
                            pProfile.SetAccelRange(serialPort1.ReadByte());
                            break;
                        case (byte)Shimmer2.PacketType.MAG_GAIN_RESPONSE:
                            pProfile.SetMagRange(serialPort1.ReadByte());
                            break;
                        case (byte)Shimmer2.PacketType.ConfigSetupByte0Response:
                            pProfile.SetConfigSetupByte0(serialPort1.ReadByte());
                            break;
                        case (byte)Shimmer2.PacketType.gsrRangeResponse:
                            pProfile.SetGsrRange(serialPort1.ReadByte());
                            break;
                        case (byte)Shimmer2.PacketType.AckCommandProcessed:
                            if (sensorsChangePending)
                            {
                                if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                                {
                                    //pProfile.UpdateChannelsFromSensorsShimmer2();
                                }
                                else
                                {
                                 //   pProfile.UpdateChannelsFromSensorsShimmer3();
                                }
                               
                               // sensorsChangePending = false;
                            }
                            break;
                        case (byte)Shimmer2.PacketType.AccelCalibrationResponse:
                            // size is 21 bytes
                            bufferbyte = new byte[21];
                            for (int p = 0; p < 21; p++)
                            {
                               bufferbyte[p]=(byte)serialPort1.ReadByte();
                                
                            }
                            retrievecalibrationparametersfrompacket(bufferbyte, (byte)Shimmer2.PacketType.AccelCalibrationResponse);
                            break;
                        case (byte)Shimmer2.PacketType.GyroCalibrationResponse:
                            // size is 21 bytes
                            bufferbyte = new byte[21];
                            for (int p = 0; p < 21; p++)
                            {
                                bufferbyte[p] = (byte)serialPort1.ReadByte();

                            }
                            retrievecalibrationparametersfrompacket(bufferbyte, (byte)Shimmer2.PacketType.GyroCalibrationResponse);
                            break;
                        case (byte)Shimmer2.PacketType.MagCalibrationResponse:
                            // size is 21 bytes
                            bufferbyte = new byte[21];
                            for (int p = 0; p < 21; p++)
                            {
                                bufferbyte[p] = (byte)serialPort1.ReadByte();

                            }
                            retrievecalibrationparametersfrompacket(bufferbyte, (byte)Shimmer2.PacketType.MagCalibrationResponse);
                            break;
                        case (byte)Shimmer2.PacketType.ALL_CALIBRATION_RESPONSE:
                            //Retrieve Accel
                            bufferbyte = new byte[21];
                            for (int p = 0; p < 21; p++)
                            {
                               bufferbyte[p]=(byte)serialPort1.ReadByte();
                                
                            }
                            retrievecalibrationparametersfrompacket(bufferbyte, (byte)Shimmer2.PacketType.AccelCalibrationResponse);

                            //Retrieve Gyro
                            bufferbyte = new byte[21];
                            for (int p = 0; p < 21; p++)
                            {
                                bufferbyte[p] = (byte)serialPort1.ReadByte();

                            }
                            retrievecalibrationparametersfrompacket(bufferbyte, (byte)Shimmer2.PacketType.GyroCalibrationResponse);

                            //Retrieve Mag
                            bufferbyte = new byte[21];
                            for (int p = 0; p < 21; p++)
                            {
                                bufferbyte[p] = (byte)serialPort1.ReadByte();

                            }
                            retrievecalibrationparametersfrompacket(bufferbyte, (byte)Shimmer2.PacketType.MagCalibrationResponse);
                            if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                            {
                                //Retrieve EMG n ECG
                                bufferbyte = new byte[12];
                                for (int p = 0; p < 12; p++)
                                {
                                    bufferbyte[p] = (byte)serialPort1.ReadByte();

                                }
                                if (bufferbyte[0] == 255 && bufferbyte[1] == 255 && bufferbyte[2] == 255 && bufferbyte[3] == 255)
                                {
                                    pProfile.DefaultEMGParams = true;
                                }
                                else
                                {
                                    pProfile.OffsetEMG = (double)((bufferbyte[0] & 0xFF) << 8) + (bufferbyte[1] & 0xFF);
                                    pProfile.GainEMG = (double)((bufferbyte[2] & 0xFF) << 8) + (bufferbyte[3] & 0xFF);
                                    pProfile.DefaultEMGParams = false;
                                }
                                if (bufferbyte[4] == 255 && bufferbyte[5] == 255 && bufferbyte[6] == 255 && bufferbyte[7] == 255)
                                {
                                    pProfile.DefaultECGParams = true;
                                }
                                else
                                {
                                    pProfile.OffsetECGLALL = (double)((bufferbyte[4] & 0xFF) << 8) + (bufferbyte[5] & 0xFF);
                                    pProfile.GainECGLALL = (double)((bufferbyte[6] & 0xFF) << 8) + (bufferbyte[7] & 0xFF);
                                    pProfile.OffsetECGRALL = (double)((bufferbyte[8] & 0xFF) << 8) + (bufferbyte[9] & 0xFF);
                                    pProfile.GainECGRALL = (double)((bufferbyte[10] & 0xFF) << 8) + (bufferbyte[11] & 0xFF);
                                    pProfile.DefaultECGParams = false;
                                }
                            }
                            else
                            {
                                //Retrieve Digital Accel Cal Paramters if Shimmer 3
                                bufferbyte = new byte[21];
                                for (int p = 0; p < 21; p++)
                                {
                                    bufferbyte[p] = (byte)serialPort1.ReadByte();

                                }
                                retrievecalibrationparametersfrompacket(bufferbyte, (byte)Shimmer3.PacketType.DAccelCalibrationResponse);
                            }

                            break;
                        case (byte)Shimmer2.PacketType.BLINK_LED_RESPONSE:
                            bufferbyte = new byte[1];
                            bufferbyte[0] = (byte)serialPort1.ReadByte();
                            pProfile.CurrentLEDStatus = bufferbyte[0];
                            break;
                        case (byte)Shimmer2.PacketType.FW_VERSION_RESPONSE:
                            // size is 21 bytes
                            bufferbyte = new byte[6];
                            for (int p = 0; p < 6; p++)
                            {
                                bufferbyte[p] = (byte)serialPort1.ReadByte();

                            }
                            pProfile.SetFirmwareIdentifier((double)((bufferbyte[1] & 0xFF) << 8) + (double)(bufferbyte[0] & 0xFF));
                            pProfile.SetFirmwareVersion((double)((bufferbyte[3] & 0xFF) << 8) + (double)(bufferbyte[2] & 0xFF) + ((double)((bufferbyte[4] & 0xFF)) / 10));
                            pProfile.SetFirmwareInternal((int)(bufferbyte[5] & 0xFF));
                            string temp = "BtStream " + pProfile.GetFirmwareVersion().ToString("0.0") + "." + pProfile.GetFirmwareInternal().ToString();
                            pProfile.SetFirmVersionFullName(temp);
                            break;
                        case (byte)Shimmer2.PacketType.GET_SHIMMER_VERSION_RESPONSE:
                            bufferbyte = new byte[1];
                            bufferbyte[0] = (byte)serialPort1.ReadByte();
                            pProfile.SetShimmerVersion(bufferbyte[0]);
                            // set default calibration parameters

                            break;
                        default:
                            break;
                    }
                }

                catch (System.TimeoutException)
                {
                    // do nothing
                }
                catch (System.InvalidOperationException)
                {
                    // do nothing
                    // gets here if serial port is forcibly closed
                }
                catch (System.IO.IOException)
                {
                    // do nothing
                }
                /*
                catch
                {
                    // should really try to kill the thread more gracefully
                }
                */
            }
            // only stop reading when disconnecting, so disconnect serial port here too
            serialPort1.Close();
            
        }

        // This method demonstrates a pattern for making thread-safe
        // calls on a Windows Forms control. 
        //
        // If the calling thread is different from the thread that
        // created the TextBox control, this method creates a
        // SetTextChannelsCallback and calls itself asynchronously using the
        // Invoke method.
        //
        // If the calling thread is the same as the thread that created
        // the TextBox control, the Text property is set directly. 
        // For full details see: http://msdn.microsoft.com/en-us/library/ms171728%28VS.80%29.aspx
        private void SetTextChannels(ShimmerDataPacket packet)
        {
            try
            {
                if (this.channelTextBox[0].InvokeRequired) // all will be in the same thread
                {
                    SetTextChannelsCallback d = new SetTextChannelsCallback(SetTextChannels);
                    this.Invoke(d, new object[] { packet });
                }
                else
                {
                    double[] datatemp = new double[3];
                    double[] datatemp3d = new double[3];
                    double[] dataAccel = new double[3];
                    double[] dataGyroRaw = new double[3];
                    double[] dataGyroCal = new double[3];
                    double[] dataMagRaw = new double[3];
                    double[] dataMagCal = new double[3];

                    if (pSaveToFile)
                    {
                        pCsvFile.Write(packet.GetTimeStamp().ToString());
                    }
                    



                    for (int i = 0; i < packet.GetNumChannels(); i++)
                    { 
                        //check the battery if it has been enabled
                        if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.AnExA7) && pProfile.GetPMux())
                            {
                                double battery = (calibrateU12AdcValue(packet.GetChannel(i), 0, 3, 1) * 2);
                                if ((calibrateU12AdcValue(packet.GetChannel(i), 0, 3, 1) * 2) < 3400)
                                {
                                    //System.Threading.Thread.Sleep(500);
                                    if (pProfile.CurrentLEDStatus == 0)
                                    {
                                        if (serialPort1.IsOpen)
                                        {
                                            serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_BLINK_LED, (byte)1 }, 0, 2);
                                            pProfile.CurrentLEDStatus = 1;
                                        }
                                    }
                                }
                            }


                        if (pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.XMag ||
                            pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.YMag ||
                            pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.ZMag)
                        {
                            // The magnetometer gives a signed 16 bit integer per channel
                            packet.SetChannel(i, ((Int16)packet.GetChannel(i)));
                        }

                        this.channelTextBox[i].Text = packet.GetChannel(i).ToString();
                        if (pProfile.showMagHeading && pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.XMag)
                        {
                            // Current channel + next 2 are mag channels
                            this.extraInfoTextBox.Text = MagHeading((Int16)packet.GetChannel(i),
                                                                     (Int16)packet.GetChannel(i + 1),
                                                                     (Int16)packet.GetChannel(i + 2)).ToString();
                        }
                        
                        if (pUpdateGraphs) 
                        {
                            int enabledSensors = pProfile.GetSensors();
                            //if (((byte)enabledSensors[0] & (byte)Shimmer2.Sensor0Bitmap.SensorAccel) > 0 && ((byte)enabledSensors[0] & (byte)Shimmer2.Sensor0Bitmap.SensorGyro) > 0 && ((byte)enabledSensors[0] & (byte)Shimmer2.Sensor0Bitmap.SensorMag) > 0)
                            if (pProfile.enable3DOrientation)
                            {

                                //run through and get all the data required for orientation calculation

                                if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.XAccel && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3) || ((pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XAAccel || pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XDAccel) && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3))
                                {
                                    dataAccel[0] = (double)packet.GetChannel(i);
                                  
                                }
                                else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.YAccel && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3) || ((pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YAAccel || pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YDAccel) && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3))
                                {

                                    dataAccel[1] = (double)packet.GetChannel(i);
                                  
                                }
                                else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.ZAccel && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3) || ((pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZAAccel || pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZDAccel) && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3))
                                {
                                    dataAccel[2] = (double)packet.GetChannel(i);
                                   
                                }
                                else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.XGyro && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3) || (pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XGyro && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3))
                                {
                                    dataGyroRaw[0] = (double)packet.GetChannel(i);
                                }
                                else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.YGyro && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3) || (pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YGyro && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3))
                                {
                                    dataGyroRaw[1] = (double)packet.GetChannel(i);
                                }
                                else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.ZGyro && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3) || (pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZGyro && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3))
                                {
                                    dataGyroRaw[2] = (double)packet.GetChannel(i);
                                  
                                }
                                else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.XMag && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3) || (pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XMag && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3))
                                {
                                    dataMagRaw[0] = (double)packet.GetChannel(i);
                                }
                                else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.YMag && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3) || (pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YMag && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3))
                                {
                                    dataMagRaw[1] = (double)packet.GetChannel(i);
                                    
                                }
                                else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.ZMag && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3) || (pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZMag && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3))
                                {
                                    dataMagRaw[2] = (double)packet.GetChannel(i);
                                    
                                    
                                }

                                if (i == packet.GetNumChannels() - 1) // if it is the last sensor channel calculate orientation
                                {
                                    //calculate quartenion here
                                    if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                                    {
                                        dataAccel = calibrateInertialSensorData(dataAccel, pProfile.AlignmentMatrixAccel, pProfile.SensitivityMatrixAccel, pProfile.OffsetVectorAccel);
                                    }
                                    else
                                    {
                                        if (pProfile.GetAccelRange() == 0)
                                        {
                                            dataAccel = calibrateInertialSensorData(dataAccel, pProfile.AlignmentMatrixAccel, pProfile.SensitivityMatrixAccel, pProfile.OffsetVectorAccel);
                                        }
                                        else
                                        {
                                            dataAccel = calibrateInertialSensorData(dataAccel, pProfile.AlignmentMatrixAccel2, pProfile.SensitivityMatrixAccel2, pProfile.OffsetVectorAccel2);
                                        }
                                    }
                                    if (enableGyroOnTheFlyCalibration)
                                    {
                                        gyroXRawList.Add(dataGyroRaw[0]);
                                        gyroYRawList.Add(dataGyroRaw[1]);
                                        gyroZRawList.Add(dataGyroRaw[2]);
                                        if (gyroXRawList.Count > listSizeGyroOnTheFly)
                                        {
                                            gyroXRawList.RemoveAt(0);
                                            gyroYRawList.RemoveAt(0);
                                            gyroZRawList.RemoveAt(0);
                                        }
                                    }


                                    dataGyroCal = calibrateInertialSensorData(dataGyroRaw, pProfile.AlignmentMatrixGyro, pProfile.SensitivityMatrixGyro, pProfile.OffsetVectorGyro);

                                    if (enableGyroOnTheFlyCalibration)
                                    {
                                        gyroXCalList.Add(dataGyroCal[0]);
                                        gyroYCalList.Add(dataGyroCal[1]);
                                        gyroZCalList.Add(dataGyroCal[2]);
                                        if (gyroXCalList.Count > listSizeGyroOnTheFly)
                                        {
                                            gyroXCalList.RemoveAt(0);
                                            gyroYCalList.RemoveAt(0);
                                            gyroZCalList.RemoveAt(0);

                                            if (getStandardDeviation(gyroXCalList) < thresholdGyroOnTheFly && getStandardDeviation(gyroYCalList) < thresholdGyroOnTheFly && getStandardDeviation(gyroZCalList) < thresholdGyroOnTheFly)
                                            {
                                                pProfile.OffsetVectorGyro[0, 0] = gyroXRawList.Average();
                                                pProfile.OffsetVectorGyro[1, 0] = gyroYRawList.Average();
                                                pProfile.OffsetVectorGyro[2, 0] = gyroZRawList.Average();
                                            }

                                        }
                                    }




                                    dataMagCal = calibrateInertialSensorData(dataMagRaw, pProfile.AlignmentMatrixMag, pProfile.SensitivityMatrixMag, pProfile.OffsetVectorMag);
                                    quat = mOrientation.update(dataAccel[0], dataAccel[1], dataAccel[2], dataGyroRaw[0] * Math.PI / 180, dataGyroRaw[1] * Math.PI / 180, dataGyroRaw[2] * Math.PI / 180, dataMagCal[0], dataMagCal[1], dataMagCal[2]);
                                    double theta, Rx, Ry, Rz, rho;
                                    rho = Math.Acos(quat.q1);
                                    theta = rho * 2;
                                    Rx = quat.q2 / Math.Sin(rho);
                                    Ry = quat.q3 / Math.Sin(rho);
                                    Rz = quat.q4 / Math.Sin(rho);
                                    double[,] rotMatrix = new double[3, 3];
                                    //convert to a rotation matrix
                                    rotMatrix[0, 0] = 2 * quat.q1 * quat.q1 - 1 + 2 * quat.q2 * quat.q2;
                                    rotMatrix[0, 1] = 2 * (quat.q2 * quat.q3 - quat.q1 * quat.q4);
                                    rotMatrix[0, 2] = 2 * (quat.q2 * quat.q4 + quat.q1 * quat.q3);
                                    rotMatrix[1, 0] = 2 * (quat.q2 * quat.q3 + quat.q1 * quat.q4);
                                    rotMatrix[1, 1] = 2 * quat.q1 * quat.q1 - 1 + 2 * quat.q3 * quat.q3;
                                    rotMatrix[1, 2] = 2 * (quat.q3 * quat.q4 - quat.q1 * quat.q2);
                                    rotMatrix[2, 0] = 2 * (quat.q2 * quat.q4 - quat.q1 * quat.q3);
                                    rotMatrix[2, 1] = 2 * (quat.q3 * quat.q4 + quat.q1 * quat.q2);
                                    rotMatrix[2, 2] = 2 * quat.q1 * quat.q1 - 1 + 2 * quat.q4 * quat.q4;

                                    // set function
                                    if (setOrientation)
                                    {
                                        setOriMatrix = matrixinverse3x3(rotMatrix);
                                        setOrientation = false;
                                    }

                                    rotMatrix = matrixmultiplication(setOriMatrix, rotMatrix);
                                    AxisAngle aa = new AxisAngle(new Matrix3d(rotMatrix[0, 0], rotMatrix[0, 1], rotMatrix[0, 2], rotMatrix[1, 0], rotMatrix[1, 1], rotMatrix[1, 2], rotMatrix[2, 0], rotMatrix[2, 1], rotMatrix[2, 2]));
                                    mForm3D.setAxisAngle(aa.angle * 180 / Math.PI, aa.x, aa.y, aa.z);

                                    if (pProfile.enable3DOrientation)
                                    {
                                        pChannelPlot[packet.GetNumChannels()].psQueue.Enqueue(new Point(0, (int)(quat.q1 * 1000) + 1000)); //to scale
                                        this.channelTextBox[packet.GetNumChannels()].Text = (Math.Round(quat.q1, 4)).ToString();
                                        pChannelPlot[packet.GetNumChannels()].Invalidate();
                                        pChannelPlot[packet.GetNumChannels() + 1].psQueue.Enqueue(new Point(0, (int)(quat.q2 * 1000) + 1000));
                                        this.channelTextBox[packet.GetNumChannels() + 1].Text = (Math.Round(quat.q2, 4)).ToString();
                                        pChannelPlot[packet.GetNumChannels() + 1].Invalidate();
                                        pChannelPlot[packet.GetNumChannels() + 2].psQueue.Enqueue(new Point(0, (int)(quat.q3 * 1000) + 1000));
                                        this.channelTextBox[packet.GetNumChannels() + 2].Text = (Math.Round(quat.q3, 4)).ToString();
                                        pChannelPlot[packet.GetNumChannels() + 2].Invalidate();
                                        pChannelPlot[packet.GetNumChannels() + 3].psQueue.Enqueue(new Point(0, (int)(quat.q4 * 1000) + 1000));
                                        this.channelTextBox[packet.GetNumChannels() + 3].Text = (Math.Round(quat.q4, 4)).ToString();
                                        pChannelPlot[packet.GetNumChannels() + 3].Invalidate();

                                    }
                                }

                            }

                            //the start of passing data to the plots
                            if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER2R || pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER2)
                            {
                                if (pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.XMag ||
                                    pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.YMag ||
                                    pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.ZMag)
                                {
                                    // scale the mag data to be positive
                                    pChannelPlot[i].psQueue.Enqueue(new Point(0, (packet.GetChannel(i) + 2048)));
                                }
                                else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.GsrRaw) &&
                                        (pProfile.GetGsrRange() == (int)Shimmer2.GsrRange.AUTORANGE))
                                {
                                    // remove the two bits indicating range
                                    pChannelPlot[i].psQueue.Enqueue(new Point(0, (packet.GetChannel(i) & 0x3FFF)));
                                }
                                else
                                    pChannelPlot[i].psQueue.Enqueue(new Point(0, packet.GetChannel(i)));
                            }
                            else
                            {

                                if (pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XMag ||
                                    pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YMag ||
                                    pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZMag)
                                {
                                    // scale the mag data to be positive
                                    pChannelPlot[i].psQueue.Enqueue(new Point(0, (packet.GetChannel(i) + 2048)));
                                }
                                else if (
                                    pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XGyro ||
                                    pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YGyro ||
                                    pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZGyro)
                                {
                                    // scale the digital accel data to be positive
                                    pChannelPlot[i].psQueue.Enqueue(new Point(0, (packet.GetChannel(i) + 32768)));
                                }
                                else if (pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XDAccel ||
                                   pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YDAccel ||
                                   pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZDAccel)
                                {
                                    pChannelPlot[i].psQueue.Enqueue(new Point(0, (packet.GetChannel(i) + 32768)));
                                }
                                else
                                {
                                    pChannelPlot[i].psQueue.Enqueue(new Point(0, packet.GetChannel(i)));
                                }
                            }
                                pChannelPlot[i].Invalidate();
                        }

                        if (pSaveToFile)
                        {
                            pCsvFile.Write(delimiter + packet.GetChannel(i).ToString());
                        }
                    }
                    //WRITE CAL DATA
                    if (pSaveToFile)
                    {
                        pCsvFile.Write(delimiter + calibrateTimeStamp(packet.GetTimeStamp()).ToString());
                    }
               

                    for (int i = 0; i < packet.GetNumChannels(); i++)
                    {
                        if (pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.XMag ||
                            pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.YMag ||
                            pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.ZMag)
                        {
                            // The magnetometer gives a signed 16 bit integer per channel
                            packet.SetChannel(i, ((Int16)packet.GetChannel(i)));
                        }
                        
                        
                        if (pSaveToFile)
                        {
                            if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.XAccel && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3) || ((pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XAAccel || pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XDAccel) && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3))
                            {

                                datatemp[0] = (double)packet.GetChannel(i);
                                //                                pCsvFile.Write(delimiter + packet.GetChannel(i).ToString());
                            }
                            else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.YAccel && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3) || ((pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YAAccel || pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YDAccel) && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3))
                            {

                                datatemp[1] = (double)packet.GetChannel(i);
                                //                                pCsvFile.Write(delimiter + packet.GetChannel(i).ToString());
                            }
                            else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.ZAccel && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3) || ((pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZAAccel || pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZDAccel) && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3))
                            {
                                datatemp[2] = (double)packet.GetChannel(i);
                                //calculate quartenion here
                                if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                                {
                                    datatemp = calibrateInertialSensorData(datatemp, pProfile.AlignmentMatrixAccel, pProfile.SensitivityMatrixAccel, pProfile.OffsetVectorAccel);
                                }
                                else
                                {
                                    if (pProfile.GetAccelRange() == 0)
                                    {
                                        datatemp = calibrateInertialSensorData(datatemp, pProfile.AlignmentMatrixAccel, pProfile.SensitivityMatrixAccel, pProfile.OffsetVectorAccel);
                                    }
                                    else
                                    {
                                        datatemp = calibrateInertialSensorData(datatemp, pProfile.AlignmentMatrixAccel2, pProfile.SensitivityMatrixAccel2, pProfile.OffsetVectorAccel2);
                                    }
                                }
                                pCsvFile.Write(delimiter + datatemp[0].ToString());
                                pCsvFile.Write(delimiter + datatemp[1].ToString());
                                pCsvFile.Write(delimiter + datatemp[2].ToString());
                            }
                            else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.XGyro && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3) || (pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XGyro && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3))
                            {

                                datatemp[0] = (double)packet.GetChannel(i);
                                //                                pCsvFile.Write(delimiter + packet.GetChannel(i).ToString());
                            }
                            else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.YGyro && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3) || (pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YGyro && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3))
                            {

                                datatemp[1] = (double)packet.GetChannel(i);
                                //                                pCsvFile.Write(delimiter + packet.GetChannel(i).ToString());
                            }
                            else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.ZGyro && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3) || (pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZGyro && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3))
                            {
                                datatemp[2] = (double)packet.GetChannel(i);
                                datatemp = calibrateInertialSensorData(datatemp, pProfile.AlignmentMatrixGyro, pProfile.SensitivityMatrixGyro, pProfile.OffsetVectorGyro);
                                pCsvFile.Write(delimiter + datatemp[0].ToString());
                                pCsvFile.Write(delimiter + datatemp[1].ToString());
                                pCsvFile.Write(delimiter + datatemp[2].ToString());
                            }
                            else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.XMag && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3) || (pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XMag && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3))
                            {
                                datatemp[0] = (double)packet.GetChannel(i);
                            }
                            else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.YMag && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3) || (pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YMag && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3))
                            {
                                datatemp[1] = (double)packet.GetChannel(i);
                            }
                            else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.ZMag && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3) || (pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZMag && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3))
                            {
                                datatemp[2] = (double)packet.GetChannel(i);
                                datatemp = calibrateInertialSensorData(datatemp, pProfile.AlignmentMatrixMag, pProfile.SensitivityMatrixMag, pProfile.OffsetVectorMag);
                                pCsvFile.Write(delimiter + datatemp[0].ToString());
                                pCsvFile.Write(delimiter + datatemp[1].ToString());
                                pCsvFile.Write(delimiter + datatemp[2].ToString());
                            }
                            else if (pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.Emg && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                            {
                                pCsvFile.Write(delimiter + calibrateU12AdcValue((double)packet.GetChannel(i), pProfile.OffsetEMG, 3, pProfile.GainEMG).ToString());
                            }
                            else if (pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.EcgRaLl && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                            {
                                pCsvFile.Write(delimiter + calibrateU12AdcValue((double)packet.GetChannel(i), pProfile.OffsetECGRALL, 3, pProfile.GainECGRALL).ToString());
                            }
                            else if (pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.EcgLaLl && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                            {
                                pCsvFile.Write(delimiter + calibrateU12AdcValue((double)packet.GetChannel(i), pProfile.OffsetECGLALL, 3, pProfile.GainECGLALL).ToString());
                            }
                            else if (pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.GsrRaw && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                            {
                                int newGSRRange = -1; // initialized to -1 so it will only come into play if mGSRRange = 4  
                                datatemp[0] = packet.GetChannel(i);
                                double p1 = 0, p2 = 0;
                                if (pProfile.GetGsrRange() == 4)
                                {
                                    newGSRRange = (49152 & (int)datatemp[0]) >> 14;
                                }
                                if (pProfile.GetGsrRange() == 0 || newGSRRange == 0)
                                { //Note that from FW 1.0 onwards the MSB of the GSR data contains the range
                                    // the polynomial function used for calibration has been deprecated, it is replaced with a linear function
                                    p1 = 0.0373;
                                    p2 = -24.9915;
                                }
                                else if (pProfile.GetGsrRange() == 1 || newGSRRange == 1)
                                {
                                    p1 = 0.0054;
                                    p2 = -3.5194;
                                }
                                else if (pProfile.GetGsrRange() == 2 || newGSRRange == 2)
                                {
                                    p1 = 0.0015;
                                    p2 = -1.0163;
                                }
                                else if (pProfile.GetGsrRange() == 3 || newGSRRange == 3)
                                {
                                    p1 = 4.5580e-04;
                                    p2 = -0.3014;
                                }
                                pCsvFile.Write(delimiter + calibrateGsrData(datatemp[0],p1,p2).ToString());
                            }

                            else if (pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.StrainHigh && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                            {
                                pCsvFile.Write(delimiter + calibrateU12AdcValue((double)packet.GetChannel(i), pProfile.OffsetSGHigh, pProfile.VRef, pProfile.GainSGHigh).ToString());
                            }
                            else if (pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.StrainLow && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                            {
                                pCsvFile.Write(delimiter + calibrateU12AdcValue((double)packet.GetChannel(i), pProfile.OffsetSGLow, pProfile.VRef, pProfile.GainSGLow).ToString());
                            }
                            else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.AnExA7) && pProfile.GetPMux() && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                            {
                                pCsvFile.Write(delimiter + (calibrateU12AdcValue(packet.GetChannel(i), 0, 3, 1) * 2).ToString());
                                
                            }
                            else if (((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.AnExA0) && pProfile.GetPMux() && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3) || ((pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.VBatt) && (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)))
                            {
                                pCsvFile.Write(delimiter + (calibrateU12AdcValue(packet.GetChannel(i), 0, 3, 1) * 1.988).ToString());
                            }

                            else if (pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.HeartRate && pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                            {
                                double tempdata = (double)packet.GetChannel(i);
                                if (pProfile.GetFirmwareVersion() != 0.1)
                                {
                                    if (tempdata == 0)
                                    {
                                        pCsvFile.Write(delimiter + pProfile.LastKnownHeartRate.ToString());
                                    }
                                    else
                                    {
                                        pCsvFile.Write(delimiter + ((int)(1024 / tempdata * 60)).ToString());
                                        pProfile.LastKnownHeartRate = ((int)(1024 / tempdata * 60));
                                    }
                                }
                                else
                                {
                                    pCsvFile.Write(delimiter + packet.GetChannel(i).ToString());
                                }
                            }

                            else
                            {
                                
                                if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
                                {
                                    pCsvFile.Write(delimiter + (calibrateU12AdcValue(packet.GetChannel(i), 0, 3, 1) * 2).ToString());
                                }
                                else
                                {
                                    pCsvFile.Write(delimiter + packet.GetChannel(i).ToString());
                                }
                            }
                        }
                    }


                        if (pSaveToFile)
                    {
                        if (pProfile.enable3DOrientation == true)
                        {
                            pCsvFile.Write(delimiter + quat.q1);
                            pCsvFile.Write(delimiter + quat.q2);
                            pCsvFile.Write(delimiter + quat.q3);
                            pCsvFile.Write(delimiter + quat.q4);
                        }

                        pCsvFile.Write("\n");
                    }
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                Disconnect();
                serialPort1.Close();
                MessageBox.Show("Receiving incorrect or corrupt packets. Closing the connection...", Shimmer.ApplicationName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected double calibrateU12AdcValue(double uncalibratedData, double offset, double vRefP, double gain)
        {
            double calibratedData = (uncalibratedData - offset) * (((vRefP * 1000) / gain) / 4095);
            return calibratedData;
        }

        protected double calibrateGsrData(double gsrUncalibratedData, double p1, double p2)
        {
            gsrUncalibratedData = (double)((int)gsrUncalibratedData & 4095);
            //the following polynomial is deprecated and has been replaced with a more accurate linear one, see GSR user guide for further details
            //double gsrCalibratedData = (p1*Math.pow(gsrUncalibratedData,4)+p2*Math.pow(gsrUncalibratedData,3)+p3*Math.pow(gsrUncalibratedData,2)+p4*gsrUncalibratedData+p5)/1000;
            //the following is the new linear method see user GSR user guide for further details
            double gsrCalibratedData = (1 / (p1 * gsrUncalibratedData + p2)) * 1000; //kohms 
            return gsrCalibratedData;
        }


        protected double[] calibrateInertialSensorData(double[] data, double[,] AM, double[,] SM, double[,] OV) {
		/*  Based on the theory outlined by Ferraris F, Grimaldi U, and Parvis M.  
           in "Procedure for effortless in-field calibration of three-axis rate gyros and accelerometers" Sens. Mater. 1995; 7: 311-30.            
           C = [R^(-1)] .[K^(-1)] .([U]-[B])
			where.....
			[C] -> [3 x n] Calibrated Data Matrix 
			[U] -> [3 x n] Uncalibrated Data Matrix
			[B] ->  [3 x n] Replicated Sensor Offset Vector Matrix 
			[R^(-1)] -> [3 x 3] Inverse Alignment Matrix
			[K^(-1)] -> [3 x 3] Inverse Sensitivity Matrix
			n = Number of Samples
			*/
            double[] tempdata = data;
            double [,] data2d=new double [3,1];
            data2d[0,0]=data[0];
            data2d[1,0]=data[1];
            data2d[2,0]=data[2];
            data2d = matrixmultiplication(matrixmultiplication(matrixinverse3x3(AM), matrixinverse3x3(SM)), matrixminus(data2d, OV));
            tempdata[0]=data2d[0,0];
            tempdata[1]=data2d[1,0];
            tempdata[2]=data2d[2,0];
            return tempdata;
	    }


        private double[,] matrixinverse3x3(double[,] data) {
	    double a,b,c,d,e,f,g,h,i;
	    a=data[0,0];
	    b=data[0,1];
	    c=data[0,2];
	    d=data[1,0];
	    e=data[1,1];
	    f=data[1,2];
	    g=data[2,0];
	    h=data[2,1];
	    i=data[2,2];
	    //
	    double deter=a*e*i+b*f*g+c*d*h-c*e*g-b*d*i-a*f*h;
	    double[,] answer=new double[3,3];
	    answer[0,0]=(1/deter)*(e*i-f*h);
	    
	    answer[0,1]=(1/deter)*(c*h-b*i);
	    answer[0,2]=(1/deter)*(b*f-c*e);
	    answer[1,0]=(1/deter)*(f*g-d*i);
	    answer[1,1]=(1/deter)*(a*i-c*g);
	    answer[1,2]=(1/deter)*(c*d-a*f);
	    answer[2,0]=(1/deter)*(d*h-e*g);
	    answer[2,1]=(1/deter)*(g*b-a*h);
	    answer[2,2]=(1/deter)*(a*e-b*d);
	    return answer;
	    }

        private double[,] matrixminus(double[,] a, double[,] b) {
		          
            int aRows = a.GetLength(0),
            aColumns = a.GetLength(1),
            bRows = b.GetLength(0),
            bColumns = b.GetLength(1);
            double[,] resultant = new double[aRows,bColumns];
	    	for(int i = 0; i < aRows; i++) { // aRow
		        for(int k = 0; k < aColumns; k++) { // aColumn
		        	resultant[i,k]=a[i,k]-b[i,k];
		        }
		    }
		    return resultant;
	    }

        private double[,] matrixmultiplication(double[,] a, double[,] b) {
   		  
          int aRows = a.GetLength(0),
              aColumns = a.GetLength(1),
               bRows = b.GetLength(0),
               bColumns = b.GetLength(1);
          double[,] resultant = new double[aRows,bColumns];

          for (int i = 0; i < aRows; i++)
          { // aRow
              for (int j = 0; j < bColumns; j++)
              { // bColumn
                  for (int k = 0; k < aColumns; k++) { // aColumn
   		        resultant[i,j] += a[i,k] * b[k,j];
   		      }
   		    }
   		  }
   		 
   		  return resultant;
    }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(serialPort1.IsOpen)
                serialPort1.Close();
            if (readThread != null)
                readThread.Abort();
            if (pCsvFile != null)
                pCsvFile.Close();
        }

        public void ChangeConfiguration()
        {
            bool wait = false;          // give shimmer chance to process command before sending another
            btnStart.Enabled = false;
            if (serialPort1.IsOpen)
            {
                if (pProfile.enable3DOrientation)
                {
                    mForm3D.Show();
                }
                if (pProfile.changeSamplingRate)
                {

                    if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                    {
                        serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_SAMPLING_RATE_COMMAND, 
                        (byte)pProfile.GetAdcSamplingRate() }, 0, 2);
                    }
                    else
                    {
                        byte firstbyte = (byte)( pProfile.GetAdcSamplingRate() & 0xff);
                        byte secondbyte = (byte)(pProfile.GetAdcSamplingRate() >> 8 & 0xff);
                        serialPort1.Write(new byte[3] { (byte)Shimmer2.PacketType.SET_SAMPLING_RATE_COMMAND, 
                        firstbyte,secondbyte }, 0, 3);
                    }
                    wait = true;
                    
                    if (wait)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                    if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                    {
                        double samplingRate = (double)1024 / (double)pProfile.GetAdcSamplingRate();
                        if (!pProfile.enableLowPowerMag)
                        {
                            
                            if (samplingRate <= 1)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)1 }, 0, 2);
                            }
                            else if (samplingRate <= 10)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)4 }, 0, 2);
                            }
                            else if (samplingRate <= 20)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)5 }, 0, 2);
                            }
                            else
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)6 }, 0, 2);
                            }
                        }
                        else
                        {
                            if (samplingRate >= 10)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)4 }, 0, 2);
                            }
                            else
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)1 }, 0, 2);
                            }

                        }
                    }
                    else
                    {
                        double samplingRate = (double)32768 / (double)pProfile.GetAdcSamplingRate();
                        if (!pProfile.enableLowPowerMag)
                        {
                            if (samplingRate <= 1)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)1 }, 0, 2);
                            }
                            else if (samplingRate <= 15)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)4 }, 0, 2);
                            }
                            else if (samplingRate <= 30)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)5 }, 0, 2);
                            }
                            else if (samplingRate <= 75)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)6 }, 0, 2);
                            }
                            else
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)7 }, 0, 2);
                            }
                        }
                        else
                        {
                            if (samplingRate >= 10)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)4 }, 0, 2);
                            }
                            else
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)1 }, 0, 2);
                            }
                        }
                    }
                    wait = true;
                    if (wait)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                    if ((pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3))
                    {
                        
                    }
                    else
                    {
                        if (!pProfile.enableLowPowerAccel)
                        {
                            double samplingRate = (double)32768 / (double)pProfile.GetAdcSamplingRate();
                            if (samplingRate <= 1)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer3.PacketType.SET_ACCEL_SAMPLING_RATE_COMMAND, (byte)1 }, 0, 2);
                            } 
                            else if (samplingRate <= 10)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer3.PacketType.SET_ACCEL_SAMPLING_RATE_COMMAND, (byte)2 }, 0, 2);
                            }
                            else if (samplingRate <= 25)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer3.PacketType.SET_ACCEL_SAMPLING_RATE_COMMAND, (byte)3 }, 0, 2);
                            }
                            else if (samplingRate <= 50)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer3.PacketType.SET_ACCEL_SAMPLING_RATE_COMMAND, (byte)4 }, 0, 2);
                            }
                            else if (samplingRate <= 100)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer3.PacketType.SET_ACCEL_SAMPLING_RATE_COMMAND, (byte)5 }, 0, 2);
                            }
                            else if (samplingRate <= 200)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer3.PacketType.SET_ACCEL_SAMPLING_RATE_COMMAND, (byte)6 }, 0, 2);
                            }
                            else
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer3.PacketType.SET_ACCEL_SAMPLING_RATE_COMMAND, (byte)7 }, 0, 2);
                            }
                        }
                        else
                        {
                            serialPort1.Write(new byte[2] { (byte)Shimmer3.PacketType.SET_ACCEL_SAMPLING_RATE_COMMAND, (byte)2 }, 0, 2);
                        }
                    }

                    wait = true;
                    if (wait)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                    if ((pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3))
                    {
                        //shimmer 2 has no low gyro power mode
                    }
                    else
                    {
                        if (!pProfile.enableLowPowerGyro)
                        {
                            double samplingRate = (double)32768 / (double)pProfile.GetAdcSamplingRate();
                            if (samplingRate <= 51.28)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer3.PacketType.SET_MPU9150_SAMPLING_RATE_COMMAND, (byte)0x9B }, 0, 2);
                            }
                            else if (samplingRate <= 102.56)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer3.PacketType.SET_MPU9150_SAMPLING_RATE_COMMAND, (byte)0x4D }, 0, 2);
                            }
                            else if (samplingRate <= 129.03)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer3.PacketType.SET_MPU9150_SAMPLING_RATE_COMMAND, (byte)0x3D }, 0, 2);
                            }
                            else if (samplingRate <= 173.91)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer3.PacketType.SET_MPU9150_SAMPLING_RATE_COMMAND, (byte)0x2D }, 0, 2);
                            }
                            else if (samplingRate <= 205.13)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer3.PacketType.SET_MPU9150_SAMPLING_RATE_COMMAND, (byte)0x26 }, 0, 2);
                            }
                            else if (samplingRate <= 258.06)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer3.PacketType.SET_MPU9150_SAMPLING_RATE_COMMAND, (byte)0x1E }, 0, 2);
                            }
                            else if (samplingRate <= 533.3)
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer3.PacketType.SET_MPU9150_SAMPLING_RATE_COMMAND, (byte)0xE }, 0, 2);
                            }
                            else
                            {
                                serialPort1.Write(new byte[2] { (byte)Shimmer3.PacketType.SET_MPU9150_SAMPLING_RATE_COMMAND, (byte)6 }, 0, 2);
                            }
                        }
                        else
                        {
                            serialPort1.Write(new byte[2] { (byte)Shimmer3.PacketType.SET_MPU9150_SAMPLING_RATE_COMMAND, (byte)0xFF }, 0, 2);
                        }
                    }

                    pProfile.changeSamplingRate = false;
                    wait = true;
                }

                
                if (pProfile.change5Vreg &&  (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3))
                {
                    if (wait)
                        System.Threading.Thread.Sleep(500);
                    serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_5V_REGULATOR_COMMAND,
                        (pProfile.GetVReg()?(byte)1:(byte)0)}, 0, 2);
                    pProfile.change5Vreg = false;
                    wait = true;
                }
                if (pProfile.changePwrMux && (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3))
                {
                    if (wait)
                        System.Threading.Thread.Sleep(500);
                    serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_POWER_MUX_COMMAND,
                        (pProfile.GetPMux()?(byte)1:(byte)0)}, 0, 2);
                    pProfile.changePwrMux = false;
                    wait = true;
                    sensorsChangePending = true;
                }
                if (pProfile.changeAccelSens)
                {
                    if (wait)
                        System.Threading.Thread.Sleep(500);
                    serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_ACCEL_RANGE_COMMAND, (byte)pProfile.GetAccelRange()}, 0, 2);

                    pProfile.changeAccelSens = false;
                    wait = true;
                }
                if (pProfile.changeGyroSens && pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
                {
                    if (wait)
                        System.Threading.Thread.Sleep(500);
                    serialPort1.Write(new byte[2] { (byte)Shimmer3.PacketType.SET_MPU9150_GYRO_RANGE_COMMAND, (byte)pProfile.GetGyroRange() }, 0, 2);
                    pProfile.changeGyroSens = false;
                    wait = true;
                }
                if (pProfile.changeMagSens)
                {
                    if (wait)
                        System.Threading.Thread.Sleep(500);
                    serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_GAIN_COMMAND, (byte)(pProfile.GetMagRange()) }, 0, 2);
                    pProfile.changeMagSens = false;
                    wait = true;
                }
                if (pProfile.changeGsrRange && (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3))
                {
                    if (wait)
                        System.Threading.Thread.Sleep(500);
                    serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SetGsrRangeCommand, 
                        (byte)pProfile.GetGsrRange()}, 0, 2);
                    pProfile.changeGsrRange = false;
                }
                if (pProfile.changeSensors)
                {
                    this.mainForm.RemoveGraphs();
                    RemoveChannelTextBoxes();
                    if (wait)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                    if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                    {

                        serialPort1.Write(new byte[3] { (byte)Shimmer2.PacketType.SET_SENSORS_COMMAND,
                        (byte)(pProfile.GetSensors() & 0xff), (byte)(pProfile.GetSensors()>>8 & 0xff)}, 0, 3);

                        if ((pProfile.GetSensors() & (int)Shimmer2.SensorBitmap.SensorGyro) > 0)
                        {
                            System.Threading.Thread.Sleep(7000);
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(500);
                        }

                        serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.INQUIRY_COMMAND }, 0, 1);
                        // give the shimmer a chance to process the previous command (required?)
                        System.Threading.Thread.Sleep(500);
                    }
                    else
                    {
                        byte firstByte = (byte)(pProfile.GetSensors() & 0xff);
                        byte secondByte = (byte)(pProfile.GetSensors() >> 8 & 0xff);
                        byte thirdByte = (byte)(pProfile.GetSensors() >> 16 & 0xff);
                        serialPort1.Write(new byte[4] { (byte)Shimmer2.PacketType.SET_SENSORS_COMMAND, firstByte, secondByte, thirdByte }, 0, 4);
                        System.Threading.Thread.Sleep(1000);

                        serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.INQUIRY_COMMAND }, 0, 1);
                        // give the shimmer a chance to process the previous command (required?)
                        System.Threading.Thread.Sleep(500);

                    }
                    pProfile.changeSensors = false;
                    wait = true;
                    sensorsChangePending = true;

                    serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.GET_ALL_CALIBRATION_COMMAND }, 0, 1);
                    System.Threading.Thread.Sleep(200);


                }
            }
            btnStart.Enabled = true;
        }

        public void ToggleLED()
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.TOGGLE_LED_COMMAND }, 0, 1);
            }
            else
                MessageBox.Show("No serial port is open", Shimmer.ApplicationName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowChannelTextBoxes()
        {
            if(this.btnConnect.InvokeRequired)  // will be in the same thread as the controls to be added
            {
                ShowChannelTextBoxesCallback d = new ShowChannelTextBoxesCallback(ShowChannelTextBoxes);
                this.Invoke(d);
            }
            else
            {
                if (channelTextBox.Count > pProfile.GetNumChannels())
                {
                    while (channelTextBox.Count != pProfile.GetNumChannels())
                    {
                        this.Controls.Remove(channelTextBox[channelTextBox.Count - 1]);
                        channelTextBox.RemoveAt(channelTextBox.Count - 1);

                        this.Controls.Remove(channelLabel[channelLabel.Count - 1]);
                        channelLabel.RemoveAt(channelLabel.Count - 1);
                    }
                }
                int extraGraphs = 0;
                if (pProfile.enable3DOrientation)
                {
                    extraGraphs = 4;
                }

                for (int i = 0; i < pProfile.GetNumChannels()+extraGraphs; i++)
                {
                    if (i == channelTextBox.Count)
                    {
                        channelTextBox.Add(new TextBox());
                        channelTextBox[i].BackColor = System.Drawing.SystemColors.Window;
                        channelTextBox[i].Location = textBoxLocation[i];
                        channelTextBox[i].ReadOnly = true;
#if _PLATFORM_LINUX
                        channelTextBox[i].Size = new System.Drawing.Size(108, 20);
#else
                        // WINDOWS
                        channelTextBox[i].Size = new System.Drawing.Size(90, 20);
#endif
                        this.Controls.Add(channelTextBox[i]);

                        channelLabel.Add(new Label());
                        channelLabel[i].AutoSize = true;
                        channelLabel[i].Location = labelLocation[i];
                        this.Controls.Add(channelLabel[i]);
                    }
                    
                    if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                    {
                        if (i < pProfile.GetNumChannels())
                        {
                            channelLabel[i].Text = Enum.GetName(typeof(Shimmer2.ChannelContents), pProfile.GetChannel(i));
                        }

                        if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.AnExA0) && pProfile.GetPMux())
                        {
                            channelLabel[i].Text = "VSenseReg";
                        }
                        else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.AnExA7) && pProfile.GetPMux())
                        {
                            channelLabel[i].Text = "VSenseBatt";
                        }
                        
                    }
                    else
                    {
                        if (i < pProfile.GetNumChannels())
                        {
                            channelLabel[i].Text = Enum.GetName(typeof(Shimmer3.ChannelContents), pProfile.GetChannel(i));
                        }

                        if ((pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XAAccel) || (pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XDAccel))
                        {
                            channelLabel[i].Text = "XAccel";
                        }
                        if ((pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YAAccel) || (pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YDAccel))
                        {
                            channelLabel[i].Text = "YAccel";
                        }
                        if ((pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZAAccel) || (pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZDAccel))
                        {
                            channelLabel[i].Text = "ZAccel";
                        }
                    }
                    if (i == pProfile.GetNumChannels())
                    {
                        channelLabel[i].Text = "Quartenion 1";
                    }
                    else if (i == pProfile.GetNumChannels() + 1)
                    {
                        channelLabel[i].Text = "Quartenion 2";
                    }
                    else if (i == pProfile.GetNumChannels() + 2)
                    {
                        channelLabel[i].Text = "Quartenion 3";
                    }
                    else if (i == pProfile.GetNumChannels() + 3)
                    {
                        channelLabel[i].Text = "Quartenion 4";
                    }
                    
                }
                if (pProfile.showMagHeading || pProfile.showGsrResistance)
                {
                    extraInfoTextBox.BackColor = System.Drawing.SystemColors.Window;
                    extraInfoTextBox.Location = textBoxLocation[channelTextBox.Count];
                    extraInfoTextBox.ReadOnly = true;
#if _PLATFORM_LINUX
                    extraInfoTextBox.Size = new System.Drawing.Size(90, 20);
#else
                    // WINDOWS
                    extraInfoTextBox.Size = new System.Drawing.Size(90, 20);
#endif
                    this.Controls.Add(extraInfoTextBox);

                    extraInfoLabel.AutoSize = true;
                    extraInfoLabel.Location = labelLocation[channelLabel.Count];
                    if (pProfile.showMagHeading)
                    {
                        extraInfoLabel.Text = "MagHeading";
                    }
                    else if (pProfile.showGsrResistance)
                    {
                        extraInfoLabel.Text = "GsrResistance";
                    }
                    this.Controls.Add(extraInfoLabel);
                }
                else
                {
                    this.Controls.Remove(extraInfoTextBox);
                    this.Controls.Remove(extraInfoLabel);
                }
            }
        }

        public void SaveToFile(bool saveToFile, StreamWriter csvFile)
        {
            pCsvFile = csvFile;
            SaveToFile(saveToFile);
            
        }

        public void SaveToFile(bool saveToFile)
        {
            pSaveToFile = saveToFile;
            if (pSaveToFile && pProfile.GetIsFilled() && serialPort1.IsOpen)
                if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER2R || pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER2)
                {
                    WriteHeaderToFileShimmer2();
                }
                else if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
                {
                    WriteHeaderToFileShimmer3();
                }
                
        }

        public void WriteHeaderToFileShimmer2()
        {
            
            //Insert object name
            delimiter = pProfile.GetLoggingDelimiter();
            pCsvFile.Write("Shimmer 1");
            for (int i = 0; i < pProfile.GetNumChannels(); i++)
            {
                    pCsvFile.Write(delimiter + "Shimmer 1");
                
            }
            pCsvFile.Write(delimiter+"Shimmer 1");
            for (int i = 0; i < pProfile.GetNumChannels(); i++)
            {
                pCsvFile.Write(delimiter + "Shimmer 1");

            }
            //If 3d orientation enabled insert extra headers here
            if (pProfile.enable3DOrientation == true)
            {
                pCsvFile.Write(delimiter + "Shimmer 1");
                pCsvFile.Write(delimiter + "Shimmer 1");
                pCsvFile.Write(delimiter + "Shimmer 1");
                pCsvFile.Write(delimiter + "Shimmer 1");
            }


            pCsvFile.Write("\n");




            //Insert property name for RAW format
            pCsvFile.Write("Timestamp");
            for (int i = 0; i < pProfile.GetNumChannels(); i++)
            {
                if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.AnExA0) && pProfile.GetPMux())
                {
                    pCsvFile.Write(delimiter + "VSenseReg");
                }
                else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.AnExA7) && pProfile.GetPMux())
                {
                    pCsvFile.Write(delimiter + "VSenseBatt");
                }
                else
                {
                    if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
                    {
                        pCsvFile.Write(delimiter + Shimmer3.ChannelProperties[pProfile.GetChannel(i)]);
                    }
                    else
                    {
                        pCsvFile.Write(delimiter + Shimmer2.ChannelProperties[pProfile.GetChannel(i)]);
                    }
                }
            }

            //Insert property name for CAL format
            pCsvFile.Write(delimiter+"Timestamp");
            for (int i = 0; i < pProfile.GetNumChannels(); i++)
            {
                if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.AnExA0) && pProfile.GetPMux())
                {
                    pCsvFile.Write(delimiter + "VSenseReg");
                }
                else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.AnExA7) && pProfile.GetPMux())
                {
                    pCsvFile.Write(delimiter + "VSenseBatt");
                }
                else
                {
                    pCsvFile.Write(delimiter + Shimmer2.ChannelProperties[pProfile.GetChannel(i)]);
                }
            }
            //If 3d orientation enabled insert extra headers here
            if (pProfile.enable3DOrientation == true)
            {
                pCsvFile.Write(delimiter + "Quartenion 1");
                pCsvFile.Write(delimiter + "Quartenion 2");
                pCsvFile.Write(delimiter + "Quartenion 3");
                pCsvFile.Write(delimiter + "Quartenion 4");
            }

            pCsvFile.Write("\n");

            //Insert format name
            pCsvFile.Write("RAW");
            for (int i = 0; i < pProfile.GetNumChannels(); i++)
            {
                pCsvFile.Write(delimiter + "RAW");

            }
            
            pCsvFile.Write(delimiter+"CAL");
            for (int i = 0; i < pProfile.GetNumChannels(); i++)
            {
                pCsvFile.Write(delimiter + "CAL");

            }
            //If 3d orientation enabled insert extra headers here
            if (pProfile.enable3DOrientation == true)
            {
                pCsvFile.Write(delimiter + "CAL");
                pCsvFile.Write(delimiter + "CAL");
                pCsvFile.Write(delimiter + "CAL");
                pCsvFile.Write(delimiter + "CAL");
            }


            pCsvFile.Write("\n");

            //Insert unit name
            pCsvFile.Write("No unit");
            for (int i = 0; i < pProfile.GetNumChannels(); i++)
            {
                pCsvFile.Write(delimiter + "No unit");

            }
            //Insert unit name for CAL format
            pCsvFile.Write(delimiter + "mSecs");
            for (int i = 0; i < pProfile.GetNumChannels(); i++)
            {
                // need to check if default calibration parameters are being used. if so add the * sign to the unit
                if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.XAccel || pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.YAccel || pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.ZAccel) && pProfile.DefaultAccelParams == true)
                {
                    pCsvFile.Write(delimiter + "m/(sec^2)*");
                }
                else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.XGyro || pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.YGyro || pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.ZGyro) && pProfile.DefaultGyroParams == true)
                {
                    pCsvFile.Write(delimiter + "deg/sec*");
                }
                else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.XMag || pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.YMag || pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.ZMag) && pProfile.DefaultMagParams == true)
                {
                    pCsvFile.Write(delimiter + "local*");
                }
                else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.EcgLaLl || pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.EcgRaLl) && pProfile.DefaultECGParams == true)
                {
                    pCsvFile.Write(delimiter + "mVolts*");
                }
                else if ((pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.Emg || pProfile.GetChannel(i) == (int)Shimmer2.ChannelContents.Emg) && pProfile.DefaultEMGParams == true)
                {
                    pCsvFile.Write(delimiter + "mVolts*");
                }
                else
                {
                    pCsvFile.Write(delimiter + Shimmer2.ChannelUnits[pProfile.GetChannel(i)]);
                }
            }

            //If 3d orientation enabled insert extra headers here
            if (pProfile.enable3DOrientation == true)
            {
                pCsvFile.Write(delimiter + "No unit");
                pCsvFile.Write(delimiter + "No unit"); 
                pCsvFile.Write(delimiter + "No unit"); 
                pCsvFile.Write(delimiter + "No unit");
            }


            pCsvFile.Write("\n");

           
            
        }


        public void WriteHeaderToFileShimmer3()
        {

            //Insert object name
            delimiter = pProfile.GetLoggingDelimiter();
            pCsvFile.Write("Shimmer 1");
            for (int i = 0; i < pProfile.GetNumChannels(); i++)
            {
                pCsvFile.Write(delimiter + "Shimmer 1");

            }
            pCsvFile.Write(delimiter + "Shimmer 1");
            for (int i = 0; i < pProfile.GetNumChannels(); i++)
            {
                pCsvFile.Write(delimiter + "Shimmer 1");

            }
            //If 3d orientation enabled insert extra headers here
            if (pProfile.enable3DOrientation == true)
            {
                pCsvFile.Write(delimiter + "Shimmer 1");
                pCsvFile.Write(delimiter + "Shimmer 1");
                pCsvFile.Write(delimiter + "Shimmer 1");
                pCsvFile.Write(delimiter + "Shimmer 1");
            }


            pCsvFile.Write("\n");




            //Insert property name for RAW format
            pCsvFile.Write("Timestamp");
            for (int i = 0; i < pProfile.GetNumChannels(); i++)
            {
                  pCsvFile.Write(delimiter + Shimmer3.ChannelProperties[pProfile.GetChannel(i)]);
            }

            //Insert property name for CAL format
            pCsvFile.Write(delimiter + "Timestamp");
            for (int i = 0; i < pProfile.GetNumChannels(); i++)
            {
                  pCsvFile.Write(delimiter + Shimmer3.ChannelProperties[pProfile.GetChannel(i)]);
            }
            //If 3d orientation enabled insert extra headers here
            if (pProfile.enable3DOrientation == true)
            {
                pCsvFile.Write(delimiter + "Quartenion 1");
                pCsvFile.Write(delimiter + "Quartenion 2");
                pCsvFile.Write(delimiter + "Quartenion 3");
                pCsvFile.Write(delimiter + "Quartenion 4");
            }

            pCsvFile.Write("\n");

            //Insert format name
            pCsvFile.Write("RAW");
            for (int i = 0; i < pProfile.GetNumChannels(); i++)
            {
                pCsvFile.Write(delimiter + "RAW");

            }

            pCsvFile.Write(delimiter + "CAL");
            for (int i = 0; i < pProfile.GetNumChannels(); i++)
            {
                pCsvFile.Write(delimiter + "CAL");

            }
            //If 3d orientation enabled insert extra headers here
            if (pProfile.enable3DOrientation == true)
            {
                pCsvFile.Write(delimiter + "CAL");
                pCsvFile.Write(delimiter + "CAL");
                pCsvFile.Write(delimiter + "CAL");
                pCsvFile.Write(delimiter + "CAL");
            }


            pCsvFile.Write("\n");

            //Insert unit name
            pCsvFile.Write("No unit");
            for (int i = 0; i < pProfile.GetNumChannels(); i++)
            {
                pCsvFile.Write(delimiter + "No unit");

            }
            //Insert unit name for CAL format
            pCsvFile.Write(delimiter + "mSecs");
            for (int i = 0; i < pProfile.GetNumChannels(); i++)
            {
                // need to check if default calibration parameters are being used. if so add the * sign to the unit
                if ((pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XAAccel || pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YAAccel || pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZAAccel) && pProfile.DefaultAccelParams == true)
                {
                    pCsvFile.Write(delimiter + "m/(sec^2)*");
                }
                else if ((pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XDAccel || pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YDAccel || pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZDAccel) && pProfile.DefaultDAccelParams == true)
                {
                    pCsvFile.Write(delimiter + "m/(sec^2)*");
                }
                else if ((pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XGyro || pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YGyro || pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZGyro) && pProfile.DefaultGyroParams == true)
                {
                    pCsvFile.Write(delimiter + "deg/sec*");
                }
                else if ((pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.XMag || pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.YMag || pProfile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZMag) && pProfile.DefaultMagParams == true)
                {
                    pCsvFile.Write(delimiter + "local*");
                }
                else
                {
                    pCsvFile.Write(delimiter + Shimmer3.ChannelUnits[pProfile.GetChannel(i)]);
                }
            }

            //If 3d orientation enabled insert extra headers here
            if (pProfile.enable3DOrientation == true)
            {
                pCsvFile.Write(delimiter + "No unit");
                pCsvFile.Write(delimiter + "No unit");
                pCsvFile.Write(delimiter + "No unit");
                pCsvFile.Write(delimiter + "No unit");
            }


            pCsvFile.Write("\n");



        }






        public void UpdateGraphs(bool val)
        {
            pUpdateGraphs = val;
        }

        public int MagHeading(Int16 x, Int16 y, Int16 z)
        {
            int heading;

            if (x == 0)
            {
                if (y < 0)
                    heading = 270;
                else
                    heading = 90;
            }
            else if (z < 0)
                heading = (int)(180.0 - RadianToDegree(Math.Atan2((float)y, (float)-x)));
            else
                heading = (int)(180.0 - RadianToDegree(Math.Atan2((float)y, (float)x)));

            return heading;
        }

        private double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        public int GsrResistance(int gsrAdcVal, int gsrRange)
        {
            int resistance = 0;
            switch (gsrRange)
            {
                // curve fitting using a 4th order polynomial
                case (int)Shimmer2.GsrRange.HW_RES_40K:
                    resistance = (int)(
                        ((0.0000000065995) * Math.Pow(gsrAdcVal, 4)) +
                        ((-0.000068950)    * Math.Pow(gsrAdcVal, 3)) +
                        ((0.2699)          * Math.Pow(gsrAdcVal, 2)) +
                        ((-476.9835)       * Math.Pow(gsrAdcVal, 1)) + 340351.3341);
                    break;
                case (int)Shimmer2.GsrRange.HW_RES_287K:
                    resistance = (int)(
                        ((0.000000013569627) * Math.Pow(gsrAdcVal, 4)) +
                        ((-0.0001650399)     * Math.Pow(gsrAdcVal, 3)) +
                        ((0.7541990)         * Math.Pow(gsrAdcVal, 2)) +
                        ((-1572.6287856)     * Math.Pow(gsrAdcVal, 1)) + 1367507.9270);
                    break;
                case (int)Shimmer2.GsrRange.HW_RES_1M:
                    resistance = (int)(
                        ((0.00000002550036498) * Math.Pow(gsrAdcVal, 4)) +
                        ((-0.00033136)         * Math.Pow(gsrAdcVal, 3)) +
                        ((1.6509426597)        * Math.Pow(gsrAdcVal, 2)) +
                        ((-3833.348044)        * Math.Pow(gsrAdcVal, 1)) + 3806317.6947);
                    break;
                case (int)Shimmer2.GsrRange.HW_RES_3M3:
                    resistance = (int)(
                        ((0.00000037153627) * Math.Pow(gsrAdcVal, 4)) +
                        ((-0.004239437)     * Math.Pow(gsrAdcVal, 3)) +
                        ((17.905709)        * Math.Pow(gsrAdcVal, 2)) +
                        ((-33723.8657)      * Math.Pow(gsrAdcVal, 1)) + 25368044.6279);
                    break;
            }
            return resistance;
        }

        private void Control_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void retrievecalibrationparametersfrompacket(byte[] bufferCalibrationParameters, byte packetType)
        {
            String[] dataType = { "i16", "i16", "i16", "i16", "i16", "i16", "i8", "i8", "i8", "i8", "i8", "i8", "i8", "i8", "i8" };
            int[] formattedPacket = formatdatapacketreverse(bufferCalibrationParameters, dataType); // using the datatype the calibration parameters are converted
            double[] AM = new double[9];
            for (int i = 0; i < 9; i++)
            {
                AM[i] = ((double)formattedPacket[6 + i]) / 100;
            }

            double[,] AlignmentMatrix = new double[3,3] { { AM[0], AM[1], AM[2] }, { AM[3], AM[4], AM[5] }, { AM[6], AM[7], AM[8] } };
            double[,] SensitivityMatrix = new double[3,3]  { { formattedPacket[3], 0, 0 }, { 0, formattedPacket[4], 0 }, { 0, 0, formattedPacket[5] } };
            double[,] OffsetVector = { { formattedPacket[0] }, { formattedPacket[1] }, { formattedPacket[2] } };
            
            if (packetType == (byte)Shimmer2.PacketType.AccelCalibrationResponse && SensitivityMatrix[0, 0] != -1)
            {   //used to be 65535 but changed to -1 as we are now using i16
                //mDefaultCalibrationParametersAccel = false;
                Debug.Write("Shimmer", "Accel Offet Vector(0,0): " + " " + OffsetVector[0, 0] + " " + SensitivityMatrix[0, 0] + " " + AlignmentMatrix[0, 0]);
                pProfile.AlignmentMatrixAccel = AlignmentMatrix;
                pProfile.OffsetVectorAccel = OffsetVector;
                pProfile.SensitivityMatrixAccel = SensitivityMatrix;
                pProfile.DefaultAccelParams = false;
            }
            else if (packetType == (byte)Shimmer2.PacketType.AccelCalibrationResponse && SensitivityMatrix[0, 0] == -1)
            {
                pProfile.DefaultAccelParams = true;
                if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                {
                    if (pProfile.GetAccelRange() == 0)
                    {
                        pProfile.SensitivityMatrixAccel = Shimmer2.SensitivityMatrixAccel1p5gShimmer2;
                    }
                    else if (pProfile.GetAccelRange() == 1)
                    {
                        pProfile.SensitivityMatrixAccel = Shimmer2.SensitivityMatrixAccel2gShimmer2;
                    }
                    else if (pProfile.GetAccelRange() == 2)
                    {
                        pProfile.SensitivityMatrixAccel = Shimmer2.SensitivityMatrixAccel4gShimmer2;
                    }
                    else if (pProfile.GetAccelRange() == 3)
                    {
                        pProfile.SensitivityMatrixAccel = Shimmer2.SensitivityMatrixAccel6gShimmer2;
                    }
                } else {
                    if (pProfile.GetAccelRange() == 0)
                    {
                        pProfile.SensitivityMatrixAccel = Shimmer3.SensitivityMatrixLowNoiseAccel2gShimmer3;
                        pProfile.AlignmentMatrixAccel = Shimmer3.AlignmentMatrixLowNoiseAccelShimmer3;
                        pProfile.OffsetVectorAccel = Shimmer3.OffsetVectorAccelLowNoiseShimmer3;
                    }
                    else if (pProfile.GetAccelRange() == 1)
                    {
                        pProfile.SensitivityMatrixAccel = Shimmer3.SensitivityMatrixWideRangeAccel4gShimmer3;
                        pProfile.AlignmentMatrixAccel = Shimmer3.AlignmentMatrixWideRangeAccelShimmer3;
                        pProfile.OffsetVectorAccel = Shimmer3.OffsetVectorAccelWideRangeShimmer3;
                    }
                    else if (pProfile.GetAccelRange() == 2)
                    {
                        pProfile.SensitivityMatrixAccel = Shimmer3.SensitivityMatrixWideRangeAccel8gShimmer3;
                        pProfile.AlignmentMatrixAccel = Shimmer3.AlignmentMatrixWideRangeAccelShimmer3;
                        pProfile.OffsetVectorAccel = Shimmer3.OffsetVectorAccelWideRangeShimmer3;
                    }
                    else if (pProfile.GetAccelRange() == 3)
                    {
                        pProfile.SensitivityMatrixAccel = Shimmer3.SensitivityMatrixWideRangeAccel16gShimmer3;
                        pProfile.AlignmentMatrixAccel = Shimmer3.AlignmentMatrixWideRangeAccelShimmer3;
                        pProfile.OffsetVectorAccel = Shimmer3.OffsetVectorAccelWideRangeShimmer3;
                    }
                }
            }
            else if (packetType == (byte)Shimmer3.PacketType.DAccelCalibrationResponse && SensitivityMatrix[0, 0] != -1)
            {   //used to be 65535 but changed to -1 as we are now using i16
                //mDefaultCalibrationParametersAccel = false;
                Debug.Write("Shimmer", "Accel Offet Vector(0,0): " + " " + OffsetVector[0, 0] + " " + SensitivityMatrix[0, 0] + " " + AlignmentMatrix[0, 0]);
                pProfile.AlignmentMatrixAccel2 = AlignmentMatrix;
                pProfile.OffsetVectorAccel2 = OffsetVector;
                pProfile.SensitivityMatrixAccel2 = SensitivityMatrix;
                pProfile.DefaultDAccelParams = false;
            }
            else if (packetType == (byte)Shimmer3.PacketType.DAccelCalibrationResponse && SensitivityMatrix[0, 0] == -1)
            {
                pProfile.DefaultDAccelParams = true;
                
                if (pProfile.GetAccelRange() == 0)
                {
                    pProfile.SensitivityMatrixAccel2 = Shimmer3.SensitivityMatrixWideRangeAccel2gShimmer3;
                    pProfile.AlignmentMatrixAccel2 = Shimmer3.AlignmentMatrixLowNoiseAccelShimmer3;
                    pProfile.OffsetVectorAccel2 = Shimmer3.OffsetVectorAccelLowNoiseShimmer3;
                }
                else if (pProfile.GetAccelRange() == 1)
                {
                    pProfile.SensitivityMatrixAccel2 = Shimmer3.SensitivityMatrixWideRangeAccel4gShimmer3;
                    pProfile.AlignmentMatrixAccel2 = Shimmer3.AlignmentMatrixWideRangeAccelShimmer3;
                    pProfile.OffsetVectorAccel2 = Shimmer3.OffsetVectorAccelWideRangeShimmer3;
                }
                else if (pProfile.GetAccelRange() == 2)
                {
                    pProfile.SensitivityMatrixAccel2 = Shimmer3.SensitivityMatrixWideRangeAccel8gShimmer3;
                    pProfile.AlignmentMatrixAccel2 = Shimmer3.AlignmentMatrixWideRangeAccelShimmer3;
                    pProfile.OffsetVectorAccel2 = Shimmer3.OffsetVectorAccelWideRangeShimmer3;
                }
                else if (pProfile.GetAccelRange() == 3)
                {
                    pProfile.SensitivityMatrixAccel2 = Shimmer3.SensitivityMatrixWideRangeAccel16gShimmer3;
                    pProfile.AlignmentMatrixAccel2 = Shimmer3.AlignmentMatrixWideRangeAccelShimmer3;
                    pProfile.OffsetVectorAccel2 = Shimmer3.OffsetVectorAccelWideRangeShimmer3;
                }
                
            }
            else if (packetType == (byte)Shimmer2.PacketType.GyroCalibrationResponse && SensitivityMatrix[0, 0] != -1)
            {   //used to be 65535 but changed to -1 as we are now using i16
                //mDefaultCalibrationParametersAccel = false;
                pProfile.AlignmentMatrixGyro = AlignmentMatrix;
                pProfile.OffsetVectorGyro = OffsetVector;
                pProfile.SensitivityMatrixGyro = SensitivityMatrix;
                pProfile.SensitivityMatrixGyro[0, 0] = pProfile.SensitivityMatrixGyro[0,0] / 100;
                pProfile.SensitivityMatrixGyro[1, 1] = pProfile.SensitivityMatrixGyro[1,1] / 100;
                pProfile.SensitivityMatrixGyro[2, 2] = pProfile.SensitivityMatrixGyro[2,2] / 100;
                pProfile.DefaultGyroParams = false;
            }
            else if (packetType == (byte)Shimmer2.PacketType.GyroCalibrationResponse && SensitivityMatrix[0, 0] == -1)
            {
                pProfile.DefaultGyroParams = true;
                if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                {
                    pProfile.SensitivityMatrixGyro = Shimmer2.SensitivityMatrixGyroShimmer2;
                    pProfile.AlignmentMatrixGyro = Shimmer2.AlignmentMatrixGyroShimmer2;
                    pProfile.OffsetVectorGyro = Shimmer2.OffsetVectorGyroShimmer2;
                }
                else
                {
                    pProfile.SensitivityMatrixGyro = Shimmer3.SensitivityMatrixGyroShimmer3;
                    pProfile.AlignmentMatrixGyro = Shimmer3.AlignmentMatrixGyroShimmer3;
                    pProfile.OffsetVectorGyro = Shimmer3.OffsetVectorGyroShimmer3;
                }
            }
            else if (packetType == (byte)Shimmer2.PacketType.MagCalibrationResponse && SensitivityMatrix[0, 0] != -1)
            {
                pProfile.DefaultMagParams = false;
                pProfile.AlignmentMatrixMag = AlignmentMatrix;
                pProfile.OffsetVectorMag = OffsetVector;
                pProfile.SensitivityMatrixMag = SensitivityMatrix;
            }
            else if (packetType == (byte)Shimmer2.PacketType.MagCalibrationResponse && SensitivityMatrix[0, 0] == -1)
            {
                pProfile.DefaultMagParams = true;
                if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                {
                    pProfile.SensitivityMatrixGyro = Shimmer2.SensitivityMatrixMagShimmer2;
                    pProfile.AlignmentMatrixGyro = Shimmer2.AlignmentMatrixMagShimmer2;
                    pProfile.OffsetVectorGyro = Shimmer2.OffsetVectorMagShimmer2;
                }
                else
                {
                    pProfile.SensitivityMatrixMag = Shimmer3.SensitivityMatrixMagShimmer3;
                    pProfile.AlignmentMatrixMag = Shimmer3.AlignmentMatrixMagShimmer3;
                    pProfile.OffsetVectorMag = Shimmer3.OffsetVectorMagShimmer3;
                }
            }


        }

        private int[] formatdatapacketreverse(byte[] data, String[] dataType)
        {
            int iData = 0;
            int[] formattedData = new int[dataType.Length];

            for (int i = 0; i < dataType.Length; i++)
                if (dataType[i] == "u8")
                {
                    formattedData[i] = (int)data[iData];
                    iData = iData + 1;
                }
                else if (dataType[i] == "i8")
                {
                    formattedData[i] = calculatetwoscomplement((int)((int)0xFF & data[iData]), 8);
                    iData = iData + 1;
                }
                else if (dataType[i] == "u12")
                {

                    formattedData[i] = (int)((int)(data[iData + 1] & 0xFF) + ((int)(data[iData] & 0xFF) << 8));
                    iData = iData + 2;
                }
                else if (dataType[i] == "u16")
                {

                    formattedData[i] = (int)((int)(data[iData + 1] & 0xFF) + ((int)(data[iData] & 0xFF) << 8));
                    iData = iData + 2;
                }
                else if (dataType[i] == "i16")
                {

                    formattedData[i] = calculatetwoscomplement((int)((int)(data[iData + 1] & 0xFF) + ((int)(data[iData] & 0xFF) << 8)), 16);
                    iData = iData + 2;
                }
            return formattedData;
        }
        private int calculatetwoscomplement(int signedData, int bitLength)
        {
            int newData = signedData;
            if (signedData >= (1 << (bitLength - 1)))
            {
                newData = -((signedData ^ (int)(Math.Pow(2, bitLength) - 1)) + 1);
            }

            return newData;
        }

        protected void setGyroOnTheFlyCalibration(Boolean enable, int bufferSize, double threshold)
        {
            if (enable)
            {
                enableGyroOnTheFlyCalibration = true;
                listSizeGyroOnTheFly = bufferSize;
                thresholdGyroOnTheFly = threshold; 
            }
        }


        protected double calibrateTimeStamp(double timeStamp)
        {
            //first convert to continuous time stamp
            double calibratedTimeStamp = 0;
            if (pProfile.mLastReceivedTimeStamp > (timeStamp + (65536 * pProfile.mCurrentTimeStampCycle)))
            {
                pProfile.mCurrentTimeStampCycle = pProfile.mCurrentTimeStampCycle + 1;
            }

            pProfile.mLastReceivedTimeStamp = (timeStamp + (65536 * pProfile.mCurrentTimeStampCycle));
            calibratedTimeStamp = pProfile.mLastReceivedTimeStamp / 32768 * 1000;   // to convert into mS
            if (pProfile.mFirstTimeCalTime)
            {
                pProfile.mFirstTimeCalTime = false;
                pProfile.mCalTimeStart = calibratedTimeStamp;
            }
            if (pProfile.mLastReceivedCalibratedTimeStamp != -1)
            {
                double timeDifference = calibratedTimeStamp - pProfile.mLastReceivedCalibratedTimeStamp;
                double clockConstant = 1024;
                if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER2R || pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER2)
                {
                    clockConstant = 1024;
                }
                else if (pProfile.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
                {
                    clockConstant = 32768;
                }

                if (timeDifference > (1 / ((clockConstant / pProfile.GetAdcSamplingRate()) - 1)) * 1000)
                {
                    pProfile.mPacketLossCount = pProfile.mPacketLossCount + 1;
                    long mTotalNumberofPackets = (long)((calibratedTimeStamp - pProfile.mCalTimeStart) / (1 / (clockConstant/pProfile.GetAdcSamplingRate())  * 1000));
                    pProfile.mPacketReceptionRate = (double)((mTotalNumberofPackets - pProfile.mPacketLossCount) / (double)mTotalNumberofPackets) * 100;
                   
                }
            }
            pProfile.mLastReceivedCalibratedTimeStamp = calibratedTimeStamp;
            return calibratedTimeStamp;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void setTheOrientation()
        {
            setOrientation = true;
        }


        internal void resetTheOrientation()
        {
            setOriMatrix = new double[,] { { 1, 0, 0 }, { 0, -1, 0 }, { 0, 0, -1 } };
        }

        private double getStandardDeviation(List<double> doubleList)
        {
            double average = doubleList.Average();
            double sumOfDerivation = 0;
            foreach (double value in doubleList)
            {
                sumOfDerivation += (value) * (value);
            }
            double sumOfDerivationAverage = sumOfDerivation / doubleList.Count;
            return Math.Sqrt(sumOfDerivationAverage - (average * average));
        }

        private void initializeShimmer2()
        {
            if (pProfile.GetFirmwareVersion() == 0.1)
            {
                Debug.Write("TX get Accel Cal Para");
                serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.GetAccelCalibrationCommand }, 0, 1);
                System.Threading.Thread.Sleep(500);

                Debug.Write("TX get Gyro Cal Para");
                serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.GetGyroCalibrationCommand }, 0, 1);
                System.Threading.Thread.Sleep(500);

                Debug.Write("TX get Mag Cal Para");
                serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.GetMagCalibrationCommand }, 0, 1);
                System.Threading.Thread.Sleep(500);
            }
            else
            {
                // check Shimmer Version 
                serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_BUFFER_SIZE_COMMAND, (byte)1 }, 0, 2);
                System.Threading.Thread.Sleep(200);
                serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.GET_SAMPLING_RATE_COMMAND }, 0, 1);
                System.Threading.Thread.Sleep(200);
                serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.GET_MAG_GAIN_COMMAND }, 0, 1);
                System.Threading.Thread.Sleep(200);
                if (pProfile.GetShimmerVersion() != (int)Shimmer.ShimmerVersion.SHIMMER3)
                {

                    if (!pProfile.enableLowPowerMag)
                    {
                        double samplingRate = (double)1024 / (double)pProfile.GetAdcSamplingRate();

                        if (samplingRate > 50)
                        {
                            serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)6 }, 0, 2);
                        }
                        else if (samplingRate > 20)
                        {
                            serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)5 }, 0, 2);
                        }
                        else if (samplingRate > 10)
                        {
                            serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)4 }, 0, 2);
                        }
                        else
                        {
                            serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)3 }, 0, 2);
                        }
                    }
                    else
                    {
                        serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)4 }, 0, 2);
                    }
                }
                else
                {
                    double samplingRate = (double)1024 / (double)pProfile.GetAdcSamplingRate();
                    if (!pProfile.enableLowPowerMag)
                    {
                        if (samplingRate > 102.4)
                        {
                            serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)7 }, 0, 2);
                        }
                        else if (samplingRate > 51.2)
                        {
                            serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)6 }, 0, 2);
                        }
                        else if (samplingRate > 10.24)
                        {
                            serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)4 }, 0, 2);
                        }
                        else
                        {
                            serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)4 }, 0, 2);
                        }
                    }
                    else
                    {
                        if (samplingRate >= 1)
                        {
                            serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)4 }, 0, 2);
                        }
                        else
                        {
                            serialPort1.Write(new byte[2] { (byte)Shimmer2.PacketType.SET_MAG_SAMPLING_RATE_COMMAND, (byte)1 }, 0, 2);
                        }
                    }
                }
                System.Threading.Thread.Sleep(200);
                serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.GET_ALL_CALIBRATION_COMMAND }, 0, 1);
                System.Threading.Thread.Sleep(200);
            }



            // Not strictly necessary here unless the GSR sensor is selected, but easier to get this value set correctly to begin with
            serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.GetGsrRangeCommand }, 0, 1);
            System.Threading.Thread.Sleep(500);
            // next obtain Accel Calibration Parameters

            serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.INQUIRY_COMMAND }, 0, 1);
            // give the shimmer a chance to process the previous command (required?)
            System.Threading.Thread.Sleep(500);

        }

        private void initializeShimmer3()
        {
            serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.GET_SAMPLING_RATE_COMMAND }, 0, 1);
            System.Threading.Thread.Sleep(200);

            serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.INQUIRY_COMMAND }, 0, 1);
            // give the shimmer a chance to process the previous command (required?)
            System.Threading.Thread.Sleep(500);

            serialPort1.Write(new byte[1] { (byte)Shimmer2.PacketType.GET_ALL_CALIBRATION_COMMAND }, 0, 1);
            System.Threading.Thread.Sleep(200);

        }


    }

    

}
