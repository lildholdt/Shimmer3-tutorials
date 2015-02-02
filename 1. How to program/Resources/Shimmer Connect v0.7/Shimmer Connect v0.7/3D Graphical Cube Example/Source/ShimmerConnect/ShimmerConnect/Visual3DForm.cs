using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tao.OpenGl;


namespace ShimmerConnect
{
    public partial class Visual3DForm : Form
    {
     double angle,x,y,z;
     int width;
     int height;
     Control control;

        public Visual3DForm()
        { 
            InitializeComponent();
            simpleOpenGlControl1.InitializeContexts();
            width = simpleOpenGlControl1.Width;
            height = simpleOpenGlControl1.Height;
            Gl.glViewport(0, 0, width, height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(45.0f, (double)width / (double)height, 0.01f, 5000.0f);
            Gl.glEnable(Gl.GL_CULL_FACE);
            Gl.glCullFace(Gl.GL_BACK);

        }

        public void setControl(Control control){
            this.control = control;
        }

        private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            Gl.glTranslated(0, 0, -5);
            Gl.glRotated(angle, x, y, z);
            
            Gl.glPointSize(3);
            Gl.glPolygonMode(Gl.GL_FRONT, Gl.GL_LINES);
            Gl.glPolygonMode(Gl.GL_BACK, Gl.GL_LINES);
            Gl.glBegin(Gl.GL_QUADS);

            Gl.glColor3ub(255, 0, 255);
            Gl.glVertex3d(1, 1, -1);
            Gl.glVertex3d(1, -1, -1);
            Gl.glVertex3d(-1, -1, -1);
            Gl.glVertex3d(-1, 1, -1);

            Gl.glColor3ub(0, 255, 255);
            Gl.glVertex3d(-1, -1, -1);
            Gl.glVertex3d(1, -1, -1);
            Gl.glVertex3d(1, -1, 1);
            Gl.glVertex3d(-1, -1, 1);

            Gl.glColor3ub(255, 255, 0);
            Gl.glVertex3d(-1, 1, -1);
            Gl.glVertex3d(-1, -1, -1);
            Gl.glVertex3d(-1, -1, 1);
            Gl.glVertex3d(-1, 1, 1);

            Gl.glColor3ub(0, 0, 255);
            Gl.glVertex3d(1, 1, 1);
            Gl.glVertex3d(1, -1, 1);
            Gl.glVertex3d(1, -1, -1);
            Gl.glVertex3d(1, 1, -1);

            Gl.glColor3ub(0, 255, 0);
            Gl.glVertex3d(-1, 1, -1);
            Gl.glVertex3d(-1, 1, 1);
            Gl.glVertex3d(1, 1, 1);
            Gl.glVertex3d(1, 1, -1);

            Gl.glColor3ub(255, 0, 0);
            Gl.glVertex3d(-1, 1, 1);
            Gl.glVertex3d(-1, -1, 1);
            Gl.glVertex3d(1, -1, 1);
            Gl.glVertex3d(1, 1, 1);

            Gl.glEnd();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.simpleOpenGlControl1.Invalidate();
        }

        private void simpleOpenGlControl1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide(); // hide the form instead of closing
            e.Cancel = true; // this cancels the close event.
        }
        public void setAxisAngle(double a, double x, double y, double z)
        {
            this.angle=a;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            control.resetTheOrientation();
        }

        private void buttonSet_Click(object sender, EventArgs e)
        {
            control.setTheOrientation();
        }
 



    }
}
