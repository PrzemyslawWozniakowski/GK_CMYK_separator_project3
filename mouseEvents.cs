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
        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            if (checkBox1.Checked)
            {
                MyPoint[] curve = new MyPoint[1];
                if (radioButton1.Checked)
                    curve = cyanCurve;
                if (radioButton2.Checked)
                    curve = magentaCurve;
                if (radioButton3.Checked)
                    curve = yellowCurve;
                if (radioButton4.Checked)
                    curve = blackCurve;

                for (int i = 0; i < 4; i++)
                {
                    if (Math.Sqrt(Math.Pow(e.X - curve[i].X, 2) + Math.Pow(e.Y - curve[i].Y, 2)) <= 16)
                    {
                        moving = curve[i];
                        isMove = true;
                        movingIndex = i;
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
                        return;
                    }
                }
            }
        }


        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {

            if (isMove && checkBox1.Checked)
            {
                if (e.X < 0 || e.Y < 0) return;

                moving.X = e.X;
                moving.Y = e.Y;

                if (moving.X < offset) moving.X = offset;
                if (moving.Y < offset) moving.Y = offset;
                if (moving.X > 550 - offset) moving.X = 550 - offset;
                if (moving.Y > 550 - offset) moving.Y = 550 - offset;


                pictureBox3.Invalidate();
            }
        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            if (isMove && checkBox1.Checked)
            {
                isMove = false;
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox3.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cyanCurve = new MyPoint[] { new MyPoint(0 + offset, 550 - offset), new MyPoint(150 + offset, 350 + offset), new MyPoint(350 + offset, 150 + offset), new MyPoint(550 - offset, 0 + offset) };
            magentaCurve = new MyPoint[] { new MyPoint(0 + offset, 550 - offset), new MyPoint(150 + offset, 350 + offset), new MyPoint(350 + offset, 150 + offset), new MyPoint(550 - offset, 0 + offset) };
            yellowCurve = new MyPoint[] { new MyPoint(0 + offset, 550 - offset), new MyPoint(150 + offset, 350 + offset), new MyPoint(350 + offset, 150 + offset), new MyPoint(550 - offset, 0 + offset) };
            blackCurve = new MyPoint[] { new MyPoint(0 + offset, 550 - offset), new MyPoint(150 + offset, 550 - offset), new MyPoint(350 + offset, 550 - offset), new MyPoint(550 - offset, 550 - offset) };
            pictureBox3.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cyanCurve = new MyPoint[] { new MyPoint(0 + offset, 550 - offset), new MyPoint(150 + offset, 550 - offset), new MyPoint(350 + offset, 550 - offset), new MyPoint(550 - offset, 550 - offset) };
            magentaCurve = new MyPoint[] { new MyPoint(0 + offset, 550 - offset), new MyPoint(150 + offset, 550 - offset), new MyPoint(350 + offset, 550 - offset), new MyPoint(550 - offset, 550 - offset) };
            yellowCurve = new MyPoint[] { new MyPoint(0 + offset, 550 - offset), new MyPoint(150 + offset, 550 - offset), new MyPoint(350 + offset, 550 - offset), new MyPoint(550 - offset, 550 - offset) };
            blackCurve = new MyPoint[] { new MyPoint(0 + offset, 550 - offset), new MyPoint(150 + offset, 350 + offset), new MyPoint(350 + offset, 150 + offset), new MyPoint(550 - offset, 0 + offset) };
            pictureBox3.Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            calculateTables();
            childForm.RedrawPaintings(cyanValues, magentaValues, yellowValues, blackValues);
            UpdateTable();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            dialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Images";
            dialog.AutoUpgradeEnabled = true;
            dialog.Title = "Please select an image.";
            if (dialog.ShowDialog() == DialogResult.OK)
            {

                bitmap = new Bitmap(@dialog.FileName);
                clampBitmap(pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Image = bitmap;
                childForm.Close();
                childForm = new ChildForm(bitmap.Width, bitmap.Height, bitmap, cyanValues, magentaValues, yellowValues, blackValues);
                childForm.Show();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            cyanCurve = new MyPoint[] { new MyPoint(0 + offset, 550 - offset), new MyPoint(424, 115), new MyPoint(408, 129), new MyPoint(525, 126) };
            magentaCurve = new MyPoint[] { new MyPoint(0 + offset, 550 - offset), new MyPoint(443, 110), new MyPoint(363, 169), new MyPoint(525, 162) };
            yellowCurve = new MyPoint[] { new MyPoint(0 + offset, 550 - offset), new MyPoint(383, 175), new MyPoint(401, 126), new MyPoint(525, 140) };
            blackCurve = new MyPoint[] { new MyPoint(375 + offset, 550 - offset), new MyPoint(475, 525), new MyPoint(505, 360), new MyPoint(550 - offset, 0 + offset) };
            pictureBox3.Invalidate();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            cyanCurve = new MyPoint[] { new MyPoint(0 + offset, 550 - offset), new MyPoint(248, 299), new MyPoint(373, 181), new MyPoint(525, 174) };
            magentaCurve = new MyPoint[] { new MyPoint(0 + offset, 550 - offset), new MyPoint(225, 323), new MyPoint(352, 222), new MyPoint(525, 210) };
            yellowCurve = new MyPoint[] { new MyPoint(0 + offset, 550 - offset), new MyPoint(293, 262), new MyPoint(354, 231), new MyPoint(525, 226) };
            blackCurve = new MyPoint[] { new MyPoint(275, 525), new MyPoint(415, 474), new MyPoint(470, 380), new MyPoint(550 - offset, 0 + offset) };
            pictureBox3.Invalidate();
        }


        private void button7_Click(object sender, EventArgs e)
        {
            var curve = cyanCurve;
            string[] lines = { $"{curve[0].X};{curve[0].Y};{curve[1].X};{curve[1].Y};{curve[2].X};{curve[2].Y};{curve[3].X};{curve[3].Y};" };
            curve = magentaCurve;
            lines[0] += $"{curve[0].X};{curve[0].Y};{curve[1].X};{curve[1].Y};{curve[2].X};{curve[2].Y};{curve[3].X};{curve[3].Y};";
            curve = yellowCurve;
            lines[0] += $"{curve[0].X};{curve[0].Y};{curve[1].X};{curve[1].Y};{curve[2].X};{curve[2].Y};{curve[3].X};{curve[3].Y};";
            curve = blackCurve;
            lines[0] += $"{curve[0].X};{curve[0].Y};{curve[1].X};{curve[1].Y};{curve[2].X};{curve[2].Y};{curve[3].X};{curve[3].Y};";

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Curves";
            dialog.AutoUpgradeEnabled = true;
            dialog.Title = "Save a curve to a file.";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllLines(@dialog.FileName, lines);

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Curves";
            dialog.AutoUpgradeEnabled = true;
            dialog.Title = "Load a curve.";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string text = File.ReadAllText(dialog.FileName, Encoding.UTF8);
                string[] t = text.Split(';');
                cyanCurve = new MyPoint[] { new MyPoint(Convert.ToInt32(t[0]), Convert.ToInt32(t[1])), new MyPoint(Convert.ToInt32(t[2]), Convert.ToInt32(t[3])), new MyPoint(Convert.ToInt32(t[4]), Convert.ToInt32(t[5])), new MyPoint(Convert.ToInt32(t[6]), Convert.ToInt32(t[7])) };
                magentaCurve = new MyPoint[] { new MyPoint(Convert.ToInt32(t[8]), Convert.ToInt32(t[9])), new MyPoint(Convert.ToInt32(t[10]), Convert.ToInt32(t[11])), new MyPoint(Convert.ToInt32(t[12]), Convert.ToInt32(t[13])), new MyPoint(Convert.ToInt32(t[14]), Convert.ToInt32(t[15])) };
                yellowCurve = new MyPoint[] { new MyPoint(Convert.ToInt32(t[16]), Convert.ToInt32(t[17])), new MyPoint(Convert.ToInt32(t[18]), Convert.ToInt32(t[19])), new MyPoint(Convert.ToInt32(t[20]), Convert.ToInt32(t[21])), new MyPoint(Convert.ToInt32(t[22]), Convert.ToInt32(t[23])) };
                blackCurve = new MyPoint[] { new MyPoint(Convert.ToInt32(t[24]), Convert.ToInt32(t[25])), new MyPoint(Convert.ToInt32(t[26]), Convert.ToInt32(t[27])), new MyPoint(Convert.ToInt32(t[28]), Convert.ToInt32(t[29])), new MyPoint(Convert.ToInt32(t[30]), Convert.ToInt32(t[31])) };
                pictureBox3.Invalidate();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            childForm.saveFile();
        }
    }
}
