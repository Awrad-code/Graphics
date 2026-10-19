using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGLWinForms
{
    public class MainForm : Form
    {
        GLControl glControl;
        bool loaded = false;

        TrackBar tbTx, tbTy, tbRot, tbScale, tbShX, tbShY;
        CheckBox chkRefX, chkRefY;
        Button btnReset;
        Panel panel;

        public MainForm()
        {
            Text = "OpenGL Transformations - Windows Forms";
            ClientSize = new Size(800, 560);

            
            panel = new Panel { Dock = DockStyle.Right, Width = 240, BackColor = Color.WhiteSmoke };
            Controls.Add(panel);

           
            glControl = new GLControl();
            glControl.Dock = DockStyle.Fill;
            glControl.BackColor = Color.White;
            glControl.Load += GlControl_Load;
            glControl.Paint += GlControl_Paint;
            glControl.Resize += (s, e) => { if (loaded) glControl.Invalidate(); };
            Controls.Add(glControl);
            glControl.BringToFront(); 

            
            int y = 10;
            tbTx = AddSlider("Translate X", -100, 100, 0, ref y);
            tbTy = AddSlider("Translate Y", -100, 100, 0, ref y);
            tbRot = AddSlider("Rotation (deg)", -180, 180, 0, ref y);
            tbScale = AddSlider("Scale (%)", 10, 300, 100, ref y);
            tbShX = AddSlider("Shear X", -100, 100, 0, ref y);
            tbShY = AddSlider("Shear Y", -100, 100, 0, ref y);

            
            chkRefX = new CheckBox { Text = "Reflect around Y axis (left/right)", Left = 10, Top = y, Width = 220 };
            chkRefY = new CheckBox { Text = "Reflect around X axis (up/down)", Left = 10, Top = y + 28, Width = 220 };
            chkRefX.CheckedChanged += (s, e) => glControl.Invalidate();
            chkRefY.CheckedChanged += (s, e) => glControl.Invalidate();
            panel.Controls.Add(chkRefX);
            panel.Controls.Add(chkRefY);
            y += 65;

            
            btnReset = new Button { Text = "Reset", Left = 10, Top = y, Width = 215, Height = 32 };
            btnReset.Click += (s, e) => ResetAll();
            panel.Controls.Add(btnReset);
        }

       
        TrackBar AddSlider(string name, int min, int max, int value, ref int y)
        {
            Label lbl = new Label { Text = name + ": " + value, Left = 10, Top = y, Width = 220 };
            TrackBar tb = new TrackBar
            {
                Minimum = min,
                Maximum = max,
                Value = value,
                TickFrequency = (max - min) / 10,
                Left = 5,
                Top = y + 20,
                Width = 225
            };
            tb.ValueChanged += (s, e) =>
            {
                lbl.Text = name + ": " + tb.Value;
                glControl.Invalidate(); 
            };
            panel.Controls.Add(lbl);
            panel.Controls.Add(tb);
            y += 70;
            return tb;
        }

        void ResetAll()
        {
            tbTx.Value = 0;
            tbTy.Value = 0;
            tbRot.Value = 0;
            tbScale.Value = 100;
            tbShX.Value = 0;
            tbShY.Value = 0;
            chkRefX.Checked = false;
            chkRefY.Checked = false;
        }

        void GlControl_Load(object sender, EventArgs e)
        {
            loaded = true;
            glControl.MakeCurrent();
            GL.ClearColor(1f, 1f, 1f, 1f);
        }

        void GlControl_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded) return;

            glControl.MakeCurrent();

            int w = glControl.Width, h = Math.Max(1, glControl.Height);
            GL.Viewport(0, 0, w, h);

            
            float aspect = (float)w / h;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-aspect, aspect, -1, 1, -1, 1);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            
            float tx = tbTx.Value / 100f;
            float ty = tbTy.Value / 100f;
            float angle = tbRot.Value;
            float scale = tbScale.Value / 100f;
            float shX = tbShX.Value / 100f;
            float shY = tbShY.Value / 100f;
            float rx = chkRefX.Checked ? -1f : 1f;
            float ry = chkRefY.Checked ? -1f : 1f;

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Translate(tx, ty, 0f);                
            GL.Rotate(angle, 0f, 0f, 1f);             
            GL.Scale(scale * rx, scale * ry, 1f);     

            float[] shear =                            
            {
                1f,  shY, 0f, 0f,
                shX, 1f,  0f, 0f,
                0f,  0f,  1f, 0f,
                0f,  0f,  0f, 1f
            };
            GL.MultMatrix(shear);

            GL.Color3(0.4f, 0.0f, 0.8f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(-0.5f, 0.3f);
            GL.Vertex2(0.5f, 0.3f);
            GL.Vertex2(0.5f, -0.3f);
            GL.Vertex2(-0.5f, -0.3f);
            GL.End();

            
            GL.LoadIdentity();
            GL.Color3(0.7f, 0.7f, 0.7f);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-aspect, 0f); GL.Vertex2(aspect, 0f);
            GL.Vertex2(0f, -1f); GL.Vertex2(0f, 1f);
            GL.End();

            glControl.SwapBuffers();
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }
    }
}
