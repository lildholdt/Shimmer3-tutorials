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
    public partial class GraphForm : Form
    {
        private Point[] psArray;
        private const int CP_NOCLOSE_BUTTON = 0x200;
        private Boolean usingLinux = false;
        public float yMax;
        public Color lineColor = new Color();
        public Color bgColor = new Color();
        public Queue<Point> psQueue = new Queue<Point>();

        public GraphForm(Boolean linux)
        {
            this.usingLinux = linux;
            InitializeComponent();
            //this.ControlBox = false;
            this.MaximizeBox = false;
            //this.MinimizeBox = false;
            yMax = 4095f;                   //default Y maximum
            lineColor = Color.Red;          //default line color
            bgColor = Color.Black;          //default background color
        }

        private void OnResizeForm(object o, System.EventArgs e)
        {
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                FillBackground(e.Graphics);
                PaintPlot(e.Graphics);
            }
            catch (Exception ex)
            {
                Console.Write("Exception : " + ex.Message);
            }

            base.OnPaint(e);
        }


        private void FillBackground(Graphics window)
        {
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

            using (SolidBrush sb1 = new SolidBrush(bgColor))
            {
                window.FillRectangle(sb1, rect);
            }
        }

        private void PaintPlot(Graphics window)
        {
            while (psQueue.Count > this.Width)
                psQueue.Dequeue();
            psArray = psQueue.ToArray();
            for (int i = 0; i < psArray.Length; i++)
            {
                psArray[i].X = i;
                float dHeight = this.Height - 28;
                int scaledY = (int)((float)dHeight - (((float)psArray[i].Y / yMax) * dHeight));
                psArray[i].Y = (int)scaledY;
            }
            using (Pen p = new Pen(lineColor))
            {
                p.Width = 2.0F;
                window.DrawLines(p, psArray);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // do nothing. Prevents flickering
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        private void GraphForm_Load(object sender, EventArgs e)
        {
            if (usingLinux)
            {
                this.Width = this.Width - 32;
                this.Height = this.Height - 15;
            }
        }
    }
}
