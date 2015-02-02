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
using System.IO;
using System.Windows.Forms;

namespace ShimmerConnect
{
    public partial class MainForm : Form
    {
        public List<GraphForm> channelPlot = new List<GraphForm>();
        public StreamWriter csvFile;
        public ShimmerProfile profile;
        private int shimmerVersion = 0;
        private string versionNumber = "0.7";
        private Control controlForm;
        private Configure configureForm;
        public Boolean usingLinux =
        #if _PLATFORM_LINUX
         true
        #else
         false
        #endif
        ;

        private Point[] graphLocation = new Point[] {
#if _PLATFORM_LINUX
            new Point(364, 0),
            new Point(364, 245),
            new Point(364, 490),
            new Point(656, 0),
            new Point(656, 245),
            new Point(656, 490),
            new Point(948, 0),
            new Point(948, 245),
            new Point(948, 490),
            new Point(1240, 0),
            new Point(1240, 245),
            new Point(1240, 490),
            new Point(1532, 0),
            new Point(1532, 245),
            new Point(1532, 490),
            new Point(1824, 0),
            new Point(1824, 245),
            new Point(1824, 490),
            new Point(2116, 0),
            new Point(2116, 245),
            new Point(2116, 490),
            new Point(2408, 0),
            new Point(2408, 245),
            new Point(2408, 490),
            new Point(2700, 0),
            new Point(2700, 245),
            new Point(2700, 490)

#else
            // WINDOWS
            new Point(314, 0),
            new Point(314, 245),
            new Point(314, 490),
            new Point(597, 0),
            new Point(597, 245),
            new Point(597, 490),
            new Point(880, 0),
            new Point(880, 245),
            new Point(880, 490),
            new Point(1163, 0),
            new Point(1163, 245),
            new Point(1163, 490),
            new Point(1446, 0),
            new Point(1446, 245),
            new Point(1446, 490),
            new Point(1729, 0),
            new Point(1729, 245),
            new Point(1729, 490),
            new Point(2012, 0),
            new Point(2012, 245),
            new Point(2012, 490),
            new Point(2052, 0),
            new Point(2052, 245),
            new Point(2052, 490),
            new Point(2092, 0),
            new Point(2092, 245),
            new Point(2092, 490)
#endif
        };
        private Color[] graphColor = new Color[] {
            Color.Red,
            Color.LawnGreen,
            Color.Yellow,
            Color.RoyalBlue,
            Color.Magenta,
            Color.OrangeRed,
            Color.Thistle,
            Color.GhostWhite,
            Color.DarkSalmon,
            Color.Gold,
            Color.SteelBlue,
            Color.DarkKhaki,
            Color.AliceBlue,
            Color.AntiqueWhite,
            Color.Aqua,
            Color.Aquamarine,
            Color.Azure,
            Color.Beige,
            Color.BlueViolet,
            Color.Brown,
            Color.BurlyWood,
            Color.CadetBlue,
            Color.Chocolate
        };

        delegate void ShowGraphsCallback();

        public MainForm()
        {
            InitializeComponent();
            this.Text = Shimmer.ApplicationName + 'V' + versionNumber;
            tsStatusLabel.Text = "";
            profile = new ShimmerProfile();
            controlForm = new Control(profile, channelPlot, saveToCSVToolStripMenuItem.Checked,
                                      csvFile, showGraphsToolStripMenuItem.Checked,
                                      ShowGraphs, ChangeStatusLabel, this);
            controlForm.MdiParent = this;
            controlForm.Show();
            
            configureForm = new Configure(controlForm, profile);
        }

        public void ShowGraphs()
        {
            if(statusStrip1.InvokeRequired)
            {
                ShowGraphsCallback d = new ShowGraphsCallback(ShowGraphs);
                this.Invoke(d);
            }
            else
            {
                if (channelPlot.Count > profile.GetNumChannels())
                {
                    while (channelPlot.Count != profile.GetNumChannels())
                    {
                        channelPlot[channelPlot.Count - 1].Hide();
                        channelPlot.RemoveAt(channelPlot.Count - 1);
                    }
                }
                int extraGraphs = 0;
                if (profile.enable3DOrientation)
                {
                    extraGraphs = 4;
                }
                //Shimmer 3
                if (shimmerVersion == (int)Shimmer.ShimmerVersion.SHIMMER3)
                {
                    if (channelPlot.Count > profile.GetNumChannels() + extraGraphs)
                    {
                        while (channelPlot.Count != profile.GetNumChannels())
                        {
                            channelPlot[channelPlot.Count - 1].Hide();
                            channelPlot.RemoveAt(channelPlot.Count - 1);
                        }
                    }

                    for (int i = 0; i < profile.GetNumChannels() + extraGraphs; i++)
                    {
                        if (i == channelPlot.Count)
                        {
                            channelPlot.Add(new GraphForm(usingLinux));
                            channelPlot[i].MdiParent = this;
                            channelPlot[i].StartPosition = FormStartPosition.Manual;
                            channelPlot[i].Location = graphLocation[i];
                            channelPlot[i].lineColor = graphColor[i];
                            //channelPlot[i].Text = Enum.GetName(typeof(Shimmer3.ChannelContents), profile.GetChannel(i));
                            channelPlot[i].Show();
                        }
                        channelPlot[i].Text = Enum.GetName(typeof(Shimmer3.ChannelContents), profile.GetChannel(i));

                        if ((profile.GetChannel(i) == (int)Shimmer3.ChannelContents.XAAccel) || (profile.GetChannel(i) == (int)Shimmer3.ChannelContents.XDAccel))
                        {
                            channelPlot[i].Text = "XAccel";
                        }
                        if ((profile.GetChannel(i) == (int)Shimmer3.ChannelContents.YAAccel) || (profile.GetChannel(i) == (int)Shimmer3.ChannelContents.YDAccel))
                        {
                            channelPlot[i].Text = "YAccel";
                        }
                        if ((profile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZAAccel) || (profile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZDAccel))
                        {
                            channelPlot[i].Text = "ZAccel";
                        }

                        

                        if (
                            profile.GetChannel(i) == (int)Shimmer3.ChannelContents.XGyro ||
                            profile.GetChannel(i) == (int)Shimmer3.ChannelContents.YGyro ||
                            profile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZGyro || profile.GetChannel(i) == (int)Shimmer3.ChannelContents.XDAccel ||
                            profile.GetChannel(i) == (int)Shimmer3.ChannelContents.YDAccel ||
                            profile.GetChannel(i) == (int)Shimmer3.ChannelContents.ZDAccel || profile.GetChannel(i) == (int)Shimmer3.ChannelContents.Temperature)
                        {
                            channelPlot[i].yMax = 65535;
                        }
                        else if (profile.GetChannel(i) == (int)Shimmer3.ChannelContents.Pressure)
                        {
                            channelPlot[i].yMax = 16777215 >> (8 - profile.GetPresRes());
                        }
                        else
                        {
                            channelPlot[i].yMax = 4095;
                        }

                        if (i == profile.GetNumChannels())
                        {
                            channelPlot[i].Text = "Quartenion 1";
                        }
                        else if (i == profile.GetNumChannels() + 1)
                        {
                            channelPlot[i].Text = "Quartenion 2";
                        }
                        else if (i == profile.GetNumChannels() + 2)
                        {
                            channelPlot[i].Text = "Quartenion 3";
                        }
                        else if (i == profile.GetNumChannels() + 3)
                        {
                            channelPlot[i].Text = "Quartenion 4";
                        }
                    }
                }
                //Shimmer 2
                if (shimmerVersion == (int)Shimmer.ShimmerVersion.SHIMMER2 || shimmerVersion == (int)Shimmer.ShimmerVersion.SHIMMER2R)
                {
                    for (int i = 0; i < profile.GetNumChannels() + extraGraphs; i++)
                    {
                        if (i == channelPlot.Count)
                        {
                            channelPlot.Add(new GraphForm(usingLinux));
                            channelPlot[i].MdiParent = this;
                            channelPlot[i].StartPosition = FormStartPosition.Manual;
                            channelPlot[i].Location = graphLocation[i];
                            channelPlot[i].lineColor = graphColor[i];
                            channelPlot[i].Show();
                        }
                        if (profile.GetChannel(i) == (int)Shimmer2.ChannelContents.HeartRate)
                        {
                            channelPlot[i].yMax = 255;
                        }
                        else
                        {
                            channelPlot[i].yMax = 4095;
                        }
                        if ((profile.GetChannel(i) == (int)Shimmer2.ChannelContents.AnExA0) && profile.GetPMux())
                        {
                            channelPlot[i].Text = "VSenseReg";
                        }
                        else if ((profile.GetChannel(i) == (int)Shimmer2.ChannelContents.AnExA7) && profile.GetPMux())
                        {
                            channelPlot[i].Text = "VSenseBatt";
                        }
                        else if (i < profile.GetNumChannels())
                        {
                            channelPlot[i].Text = Enum.GetName(typeof(Shimmer2.ChannelContents), profile.GetChannel(i));
                        }
                        
                        
                        if (i == profile.GetNumChannels())
                        {
                            channelPlot[i].Text = "Quartenion 1";
                        }
                        else if (i == profile.GetNumChannels() + 1)
                        {
                            channelPlot[i].Text = "Quartenion 2";
                        }
                        else if (i == profile.GetNumChannels() + 2)
                        {
                            channelPlot[i].Text = "Quartenion 3";
                        }
                        else if (i == profile.GetNumChannels() + 3)
                        {
                            channelPlot[i].Text = "Quartenion 4";
                        }
                    }
                }
            }
        }

        public void RemoveGraphs()
        {
            for(int i=channelPlot.Count-1; i>=0; i--)
            {
                channelPlot[i].Hide();
                channelPlot.RemoveAt(i);
            }
        }

        public void ChangeStatusLabel(string text)
        {
            tsStatusLabel.Text = text;
        }

        private void configureShimmerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (configureForm.ShowDialog(this) == DialogResult.OK)
            {
                controlForm.ChangeConfiguration();   
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveToCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveToCSVToolStripMenuItem.Checked)
            {
                saveToCSVToolStripMenuItem.Checked = false;
                // close file if open
                csvFile.Close();
                controlForm.SaveToFile(saveToCSVToolStripMenuItem.Checked);
            }
            else
            {
                openDialog.CheckFileExists = false;
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        csvFile = new StreamWriter(openDialog.FileName, false);
                        saveToCSVToolStripMenuItem.Checked = true;
                        controlForm.SaveToFile(saveToCSVToolStripMenuItem.Checked, csvFile);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Shimmer.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                } 
            }
            
        }

        private void showGraphsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showGraphsToolStripMenuItem.Checked)
            {
                showGraphsToolStripMenuItem.Checked = false;
                controlForm.UpdateGraphs(false);
                RemoveGraphs();
            }
            else 
            {
                showGraphsToolStripMenuItem.Checked = true;
                if(controlForm.isStreaming)
                {
                    ShowGraphs();
                }
                controlForm.UpdateGraphs(true);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        public void enableConfigFormMenu(Boolean enable)
        {
            configureShimmerToolStripMenuItem.Enabled = enable;
        }

        public void setShimmerVersion(int version)
        {
            shimmerVersion = version;
        }

        public int getShimmerVersion()
        {
            return shimmerVersion;
        }

    }
}
