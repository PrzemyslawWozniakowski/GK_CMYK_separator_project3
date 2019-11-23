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

namespace Grafika_projekt_3
{
    public partial class ChildForm : Form
    {
        //PictureBox canvasC;
        //PictureBox canvasM;
        //PictureBox canvasY;
        //PictureBox canvasK;


        Bitmap bitmap;
        Color[,] colorMap;
        private PictureBox canvasC;
        private PictureBox canvasM;
        private PictureBox canvasY;
        private PictureBox canvasK;
        Color[,] blackMap;
        public ChildForm(int _width, int _height, Bitmap _bitmap, int[] cyanTable, int[] magentaTable, int[] yellowTable, int[] blackTable)
        {


            Width = _width;
            Height = _height;
            bitmap = _bitmap;
            clampBitmap(800, 400);
            InitializeComponent(_width, _height);



            colorMap = new Color[bitmap.Width, bitmap.Height];
            blackMap = new Color[bitmap.Width, bitmap.Height];
            for (int i = 0; i < bitmap.Width; i++)
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var color = bitmap.GetPixel(i % bitmap.Width, j % bitmap.Height);
                    int cyan = (255 - color.R) * 100 / 255;
                    int magenta = (255 - color.G) * 100 / 255;
                    int yellow = (255 - color.B) * 100 / 255;


                    int min = minVal(cyan, magenta, yellow);
                    cyan -= min; magenta -= min; yellow -= min;
                    cyan += cyanTable[min];
                    magenta += magentaTable[min];
                    yellow += yellowTable[min];
                    int black = (100 - blackTable[min]) * 255 / 100;
                    int red = (100 - cyan) * 255 / 100;
                    int green = (100 - magenta) * 255 / 100;
                    int blue = (100 - yellow) * 255 / 100;

                    colorMap[i, j] = Color.FromArgb(red, green, blue);
                    blackMap[i, j] = Color.FromArgb(black, black, black);
                }

            canvasC_Paint();
            canvasM_Paint();
            canvasY_Paint();
            canvasK_Paint();
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
        public int minVal(int m1, int m2, int m3)
        {
            if (m1 <= m2 && m1 <= m3) return m1;
            if (m2 <= m1 && m2 <= m3) return m2;
            if (m3 <= m1 && m3 <= m2) return m3;
            return 0;
        }

        public void RedrawPaintings(int[] cyanTable, int[] magentaTable, int[] yellowTable, int[] blackTable)
        {
            colorMap = new Color[bitmap.Width, bitmap.Height];
            blackMap = new Color[bitmap.Width, bitmap.Height];
            for (int i = 0; i < bitmap.Width; i++)
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var color = bitmap.GetPixel(i % bitmap.Width, j % bitmap.Height);
                    int cyan = (255 - color.R) * 100 / 255;
                    int magenta = (255 - color.G) * 100 / 255;
                    int yellow = (255 - color.B) * 100 / 255;


                    int min = minVal(cyan, magenta, yellow);
                    cyan -= min; magenta -= min; yellow -= min;
                    cyan += cyanTable[min];
                    magenta += magentaTable[min];
                    yellow += yellowTable[min];
                    int black = (100 - blackTable[min]) * 255 / 100;
                    int red = (100 - cyan) * 255 / 100;
                    int green = (100 - magenta) * 255 / 100;
                    int blue = (100 - yellow) * 255 / 100;

                    colorMap[i, j] = Color.FromArgb(red, green, blue);
                    blackMap[i, j] = Color.FromArgb(black, black, black);
                }

            canvasC_Paint();
            canvasM_Paint();
            canvasY_Paint();
            canvasK_Paint();
        }

        public void FillCanvas(Color[,] colorToPaint, PictureBox canvas)
        {
            Bitmap processedBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            unsafe
            {
                BitmapData bitmapData = processedBitmap.LockBits(new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.ReadWrite, processedBitmap.PixelFormat);

                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        currentLine[x] = colorToPaint[x / 4, y].B;
                        currentLine[x + 1] = colorToPaint[x / 4, y].G;
                        currentLine[x + 2] = colorToPaint[x / 4, y].R;
                        currentLine[x + 3] = colorToPaint[x / 4, y].A;
                    }
                });
                processedBitmap.UnlockBits(bitmapData);
            }

            canvas.Image = processedBitmap;
        }

        private void canvasC_Paint()
        {

            Color[,] colorToPaint = new Color[bitmap.Width, bitmap.Height];

            for (int i = 0; i < bitmap.Width; i++)
                for (int j = 0; j < bitmap.Height; j++)
                {
                    colorToPaint[i, j] = Color.FromArgb(colorMap[i, j].R, 255, 255);
                }
            FillCanvas(colorToPaint, canvasC);
        }

        private void canvasM_Paint()
        {
            Color[,] colorToPaint = new Color[bitmap.Width, bitmap.Height];

            for (int i = 0; i < bitmap.Width; i++)
                for (int j = 0; j < bitmap.Height; j++)
                {
                    colorToPaint[i, j] = Color.FromArgb(255, colorMap[i, j].G, 255);
                }
            FillCanvas(colorToPaint, canvasM);
        }

        private void canvasY_Paint()
        {
            Color[,] colorToPaint = new Color[bitmap.Width, bitmap.Height];

            for (int i = 0; i < bitmap.Width; i++)
                for (int j = 0; j < bitmap.Height; j++)
                {
                    colorToPaint[i, j] = Color.FromArgb(255, 255, colorMap[i, j].B);
                }
            FillCanvas(colorToPaint, canvasY);
        }
        private void canvasK_Paint()
        {

            FillCanvas(blackMap, canvasK);
        }




        private void InitializeComponent(int _width, int _height)
        {
            this.canvasC = new System.Windows.Forms.PictureBox();
            this.canvasM = new System.Windows.Forms.PictureBox();
            this.canvasY = new System.Windows.Forms.PictureBox();
            this.canvasK = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.canvasC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvasM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvasY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvasK)).BeginInit();
            this.SuspendLayout();
            // 
            // canvasC
            // 
            this.canvasC.Location = new System.Drawing.Point(0, 0);
            this.canvasC.Name = "canvasC";
            this.canvasC.Size = new System.Drawing.Size(Width, Height);
            this.canvasC.TabIndex = 0;
            this.canvasC.TabStop = false;
            // 
            // canvasM
            // 
            this.canvasM.Location = new System.Drawing.Point(Width, 0);
            this.canvasM.Name = "canvasM";
            this.canvasM.Size = new System.Drawing.Size(Width, Height);
            this.canvasM.TabIndex = 1;
            this.canvasM.TabStop = false;
            // 
            // canvasY
            // 
            this.canvasY.Location = new System.Drawing.Point(0, Height);
            this.canvasY.Name = "canvasY";
            this.canvasY.Size = new System.Drawing.Size(Width, Height);
            this.canvasY.TabIndex = 2;
            this.canvasY.TabStop = false;
            // 
            // canvasK
            // 
            this.canvasK.Location = new System.Drawing.Point(Width, Height);
            this.canvasK.Name = "canvasK";
            this.canvasK.Size = new System.Drawing.Size(Width, Height);
            this.canvasK.TabIndex = 3;
            this.canvasK.TabStop = false;
            // 
            // ChildForm
            // 
            this.ClientSize = new System.Drawing.Size(2 * _width, 2 * _height);
            this.Controls.Add(this.canvasK);
            this.Controls.Add(this.canvasY);
            this.Controls.Add(this.canvasM);
            this.Controls.Add(this.canvasC);
            this.Name = "ChildForm";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            ((System.ComponentModel.ISupportInitialize)(this.canvasC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvasM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvasY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvasK)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
