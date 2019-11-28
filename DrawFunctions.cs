using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Grafika_projekt_3
{
    public partial class Form1 : Form
    {
        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {

            //pictureBox3.Image = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            Pen pen = new Pen(Color.Cyan, 1);
            Pen bpen = new Pen(Color.Black, 1);
            Pen rpen = new Pen(Color.Red, 1);

            for (int i = offset; i < pictureBox3.Width - offset; i++)
            {
                e.Graphics.DrawRectangle(bpen, new Rectangle(i, pictureBox3.Width - offset, 1, 1));
                e.Graphics.DrawRectangle(bpen, new Rectangle(offset, i, 1, 1));
            }
            e.Graphics.DrawLine(bpen, offset, 125 + offset, offset + 500, 125 + offset);
            e.Graphics.DrawLine(bpen, offset, 250 + offset, offset + 500, 250 + offset);
            e.Graphics.DrawLine(bpen, offset, 375 + offset, offset + 500, 375 + offset);
            e.Graphics.DrawLine(bpen, offset, offset, offset + 500, offset);

            e.Graphics.DrawLine(bpen, offset + 125, offset, offset + 125, 500 + offset);
            e.Graphics.DrawLine(bpen, offset + 250, offset, offset + 250, 500 + offset);
            e.Graphics.DrawLine(bpen, offset + 375, offset, offset + 375, 500 + offset);
            e.Graphics.DrawLine(bpen, offset + 500, offset, offset + 500, 500 + offset);

            if (checkBox2.Checked)
            {
                DrawBezier(e, cyanCurve, Color.Cyan);
                DrawBezier(e, magentaCurve, Color.Magenta);
                DrawBezier(e, yellowCurve, Color.Yellow);
                DrawBezier(e, blackCurve, Color.Black);
            }
            else
            {
                if (radioButton1.Checked)
                    DrawBezier(e, cyanCurve, Color.Cyan);
                if (radioButton2.Checked)
                    DrawBezier(e, magentaCurve, Color.Magenta);
                if (radioButton3.Checked)
                    DrawBezier(e, yellowCurve, Color.Yellow);
                if (radioButton4.Checked)
                    DrawBezier(e, blackCurve, Color.Black);
            }


            if (checkBox1.Checked)
            {
                if (radioButton1.Checked)
                    foreach (var v in cyanCurve)
                        e.Graphics.DrawEllipse(bpen, new Rectangle(v.X - 4, v.Y - 4, 8, 8));

                if (radioButton2.Checked)
                    foreach (var v in magentaCurve)
                        e.Graphics.DrawEllipse(bpen, new Rectangle(v.X - 4, v.Y - 4, 8, 8));

                if (radioButton3.Checked)
                    foreach (var v in yellowCurve)
                        e.Graphics.DrawEllipse(bpen, new Rectangle(v.X - 4, v.Y - 4, 8, 8));

                if (radioButton4.Checked)
                    foreach (var v in blackCurve)
                        e.Graphics.DrawEllipse(bpen, new Rectangle(v.X - 4, v.Y - 4, 8, 8));
            }
        }


        private void DrawBezier(PaintEventArgs e, MyPoint[] curve, Color col)
        {
            Pen pen = new Pen(col, 1);
            for (int i = 0; i < 1000; i++)
            {
                float u = (float)i / 1000;

                float y = (float)((float)curve[0].Y * Math.Pow(1 - u, 3) + 3 * (float)curve[1].Y * Math.Pow(1 - u, 2) * (float)u + 3 * (float)curve[2].Y * Math.Pow(u, 2) * (float)(1 - u) + (float)curve[3].Y * Math.Pow(u, 3));
                float x = (float)((float)curve[0].X * Math.Pow(1 - u, 3) + 3 * (float)curve[1].X * Math.Pow(1 - u, 2) * (float)u + 3 * (float)curve[2].X * Math.Pow(u, 2) * (float)(1 - u) + (float)curve[3].X * Math.Pow(u, 3));
                e.Graphics.DrawRectangle(pen, new Rectangle((int)x, (int)y, 1, 1));
                // bm.SetPixel((int)x, (int)y, col);
            }

        }

    }
}