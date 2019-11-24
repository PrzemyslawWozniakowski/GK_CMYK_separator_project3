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
        public int MainPictureWidth = 800;
        public int MainPictureHeight = 400;

        int offset = 25;
        MyPoint[] cyanCurve = { new MyPoint(0 + 25, 550 - 25), new MyPoint(150 + 25, 550 - 25), new MyPoint(350 + 25, 550 - 25), new MyPoint(550 - 25, 550 - 25) };
        MyPoint[] magentaCurve = { new MyPoint(0 + 25, 550 - 25), new MyPoint(150 + 25, 550 - 25), new MyPoint(350 + 25, 550 - 25), new MyPoint(550 - 25, 550 - 25) };
        MyPoint[] yellowCurve = { new MyPoint(0 + 25, 550 - 25), new MyPoint(150 + 25, 550 - 25), new MyPoint(350 + 25, 550 - 25), new MyPoint(550 - 25, 550 - 25) };
        MyPoint[] blackCurve = { new MyPoint(0 + 25, 550 - 25), new MyPoint(150 + 25, 350 + 25), new MyPoint(350 + 25, 150 + 25), new MyPoint(550 - 25, 0 + 25) };

        MyPoint moving;
        int movingIndex;
        bool isMove = false;
        ChildForm childForm;
        Bitmap bitmap;

        int[] cyanValues = new int[101];
        int[] magentaValues = new int[101];
        int[] yellowValues = new int[101];
        int[] blackValues = new int[101];
        int[] cyanCount = new int[101];
        int[] magentaCount = new int[101];
        int[] yellowCount = new int[101];
        int[] blackCount = new int[101];


        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = Image.FromFile(@"D:\Desktop\kot2.jpg");


            bitmap = new Bitmap(@"D:\Desktop\kot2.jpg");
            calculateTables();
            childForm = new ChildForm(pictureBox1.Width, pictureBox1.Height, bitmap, cyanValues, magentaValues, yellowValues, blackValues);
            childForm.Show();
            SetUpTable();
            pictureBox3.Invalidate();
        }

        private void calculateTable(MyPoint[] curve, ref int[] values,ref int[] count)
        {
            values = new int[101];
            count = new int[101];
            for (int i = 0; i < 101; i++)
            {
                values[i] = 0;
            }
            for (int i = 0; i <= 1000; i++)
            {
                float u = (float)i / 1000;

                float y = (float)((float)curve[0].Y * Math.Pow(1 - u, 3) + 3 * (float)curve[1].Y * Math.Pow(1 - u, 2) * (float)u + 3 * (float)curve[2].Y * Math.Pow(u, 2) * (float)(1 - u) + (float)curve[3].Y * Math.Pow(u, 3));
                float x = (float)((float)curve[0].X * Math.Pow(1 - u, 3) + 3 * (float)curve[1].X * Math.Pow(1 - u, 2) * (float)u + 3 * (float)curve[2].X * Math.Pow(u, 2) * (float)(1 - u) + (float)curve[3].X * Math.Pow(u, 3));
                int yy = 550 - offset - (int)y;
                int xx = (int)x - offset;
                if (xx % 5 == 0)
                {
                    values[xx / 5] += yy / 5;
                    count[xx / 5] += 1;
                }
            }
            for (int i = 0; i < 101; i++)
            {
                if (count[i] != 0)
                    values[i] /= count[i];
            }
        }

        private void calculateTables()
        {
            calculateTable(cyanCurve,ref cyanValues,ref cyanCount);
            calculateTable(magentaCurve,ref magentaValues,ref magentaCount);
            calculateTable(yellowCurve,ref yellowValues,ref yellowCount);
            calculateTable(blackCurve,ref blackValues,ref blackCount);
        }

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
                if (movingIndex != 3)
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
                childForm = new ChildForm(pictureBox1.Width, pictureBox1.Height, bitmap, cyanValues, magentaValues, yellowValues, blackValues);
                childForm.Show();
            }
        }

        public void clampBitmap(int w, int h)
        {
            if (bitmap.Width > w && bitmap.Height > h)
            {
                bitmap = new Bitmap(bitmap, new Size(w, h));
            }
            if (bitmap.Width > w)
                bitmap = new Bitmap(bitmap, new Size(w, bitmap.Height));
            if (bitmap.Height > h)
                bitmap = new Bitmap(bitmap, new Size(bitmap.Width, h));
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
            dialog.Title = "Please select an image.";
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

        private void SetUpTable()
        {
            myLayoutPanel1.Controls.Add(new Label() { Dock=DockStyle.Fill, Text = "Value" , TextAlign = ContentAlignment.MiddleCenter, Font= new Font("Arial", 11, FontStyle.Bold) }, 0, 0);
            myLayoutPanel1.Controls.Add(new Label() { Dock = DockStyle.Fill, Text = "Cyan", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 11, FontStyle.Bold) }, 0, 1);
            myLayoutPanel1.Controls.Add(new Label() { Dock = DockStyle.Fill, Text = "Magenta", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 11, FontStyle.Bold) }, 0, 2);
            myLayoutPanel1.Controls.Add(new Label() { Dock = DockStyle.Fill, Text = "Yellow", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 11, FontStyle.Bold) }, 0, 3);
            myLayoutPanel1.Controls.Add(new Label() { Dock = DockStyle.Fill, Text = "Black", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 11, FontStyle.Bold) }, 0, 4);
            
            for(int i=1; i<12; i++)
            {
                myLayoutPanel1.Controls.Add(new Label() { Dock = DockStyle.Fill, Text = $"{(i-1)*10}", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 11, FontStyle.Bold) }, i, 0);
            }

            for (int i = 1; i < 12; i++)
            {
                myLayoutPanel1.Controls.Add(new Label() { Dock = DockStyle.Fill, Text = $"{cyanValues[(i-1)*10]}", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 10, FontStyle.Regular) }, i, 1);
            }
            for (int i = 1; i < 12; i++)
            {
                myLayoutPanel1.Controls.Add(new Label() { Dock = DockStyle.Fill, Text = $"{magentaValues[(i - 1) * 10]}", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 10, FontStyle.Regular) }, i, 2);
            }
            for (int i = 1; i < 12; i++)
            {
                myLayoutPanel1.Controls.Add(new Label() { Dock = DockStyle.Fill, Text = $"{yellowValues[(i - 1) * 10]}", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 10, FontStyle.Regular) }, i, 3);
            }
            for (int i = 1; i < 12; i++)
            {
                myLayoutPanel1.Controls.Add(new Label() { Dock = DockStyle.Fill, Text = $"{blackValues[(i - 1) * 10]}", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 10, FontStyle.Regular) }, i, 4);
            }
        }
        private void UpdateTable()
        {
            myLayoutPanel1.SuspendLayout();
            for (int i = 1; i < 12; i++)
            {
                myLayoutPanel1.GetControlFromPosition(i, 1).Text = $"{cyanValues[(i - 1) * 10]}";
            }
            for (int i = 1; i < 12; i++)
            {
                myLayoutPanel1.GetControlFromPosition(i, 2).Text = $"{magentaValues[(i - 1) * 10]}";
            }
            for (int i = 1; i < 12; i++)
            {
                myLayoutPanel1.GetControlFromPosition(i, 3).Text = $"{yellowValues[(i - 1) * 10]}";
            }
            for (int i = 1; i < 12; i++)
            {
                myLayoutPanel1.GetControlFromPosition(i, 4).Text = $"{blackValues[(i - 1) * 10]}";
            }
            myLayoutPanel1.ResumeLayout();
            
        }
    }
}
