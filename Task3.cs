using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp3
{
    public partial class Form1 : Form
    {
        int carX = 250;
        bool moveRight = true;

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.KeyPreview = true;

            this.Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D)
            {
                carX += 10;
                moveRight = true;
            }

            if (e.KeyCode == Keys.A)
            {
                carX -= 10;
                moveRight = false;
            }

            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

           
            g.Clear(Color.SkyBlue);

            
            Point[] m1 =
            {
                new Point(0,250),
                new Point(150,130),
                new Point(300,250)
            };

            Point[] m2 =
            {
                new Point(180,250),
                new Point(400,70),
                new Point(620,250)
            };

            g.FillPolygon(Brushes.Green, m1);
            g.FillPolygon(Brushes.Green, m2);

            
            g.FillEllipse(Brushes.Yellow, 600, 40, 100, 100);

            
            g.FillRectangle(Brushes.Gray, 0, 250, 800, 120);

            g.DrawLine(
                new Pen(Color.White, 5),
                0, 310,
                800, 310);

            
            DrawCar(g);
        }

        private void DrawCar(Graphics g)
        {
            if (moveRight)
            {
                g.TranslateTransform(carX, 240);
            }
            else
            {
                g.TranslateTransform(carX + 150, 240);
                g.ScaleTransform(-1, 1);
            }

            
            g.FillRectangle(Brushes.Red, 0, 20, 150, 60);

          
            g.FillRectangle(Brushes.Red, 35, -15, 80, 35);

            
            g.FillEllipse(Brushes.White, 20, 65, 40, 40);
            g.FillEllipse(Brushes.White, 90, 65, 40, 40);

            g.DrawEllipse(Pens.Black, 20, 65, 40, 40);
            g.DrawEllipse(Pens.Black, 90, 65, 40, 40);

            g.FillEllipse(Brushes.Black, 35, 80, 10, 10);
            g.FillEllipse(Brushes.Black, 105, 80, 10, 10);

            g.ResetTransform();
        }
    }
}