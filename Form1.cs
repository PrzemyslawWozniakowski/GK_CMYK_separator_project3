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
            childForm = new ChildForm(bitmap.Width, bitmap.Height, bitmap, cyanValues, magentaValues, yellowValues, blackValues);
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

       

        private void SetUpTable()
        {
            myLayoutPanel1.Controls.Add(new Label() { Dock=DockStyle.Fill, Text = "Gray scale (%)" , TextAlign = ContentAlignment.MiddleCenter, Font= new Font("Arial", 11, FontStyle.Bold) }, 0, 0);
            myLayoutPanel1.Controls.Add(new Label() { Dock = DockStyle.Fill, Text = "Cyan", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 11, FontStyle.Bold) }, 0, 1);
            myLayoutPanel1.Controls.Add(new Label() { Dock = DockStyle.Fill, Text = "Magenta", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 11, FontStyle.Bold) }, 0, 2);
            myLayoutPanel1.Controls.Add(new Label() { Dock = DockStyle.Fill, Text = "Yellow", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 11, FontStyle.Bold) }, 0, 3);
            myLayoutPanel1.Controls.Add(new Label() { Dock = DockStyle.Fill, Text = "Black", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 11, FontStyle.Bold) }, 0, 4);
            
            for(int i=1; i<12; i++)
            {
                myLayoutPanel1.Controls.Add(new Label() { Dock = DockStyle.Fill, Text = $"{(i-1)*10}%", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 11, FontStyle.Bold) }, i, 0);
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
