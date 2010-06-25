using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DIP
{
    public partial class GraphicBar : UserControl
    {
        public Color ColorStart, ColorEnd;
        public Bitmap PicMap;
        private int[,] Hist;
        public int DrawColor;
        public Bitmap pic;
        public GraphicBar(Color s, Color e, int[,] t,int tune)
        {
            InitializeComponent();
            ColorStart = s;
            ColorEnd = e;
            Hist = t;                      //直方图矩阵
            DrawColor = tune;
            pictureBox1.Invalidate();
            pictureBox2.Invalidate();
        }
        public void ReGraphic(int[,] t,int tune)
        {
            Hist = t;                      //直方图矩阵
            DrawColor = tune;
            pictureBox1.Invalidate();
            pictureBox2.Invalidate();
        }

        public  GraphicBar(Bitmap PicIn)
        {
            InitializeComponent();
            pic = PicIn;                      //直方图矩阵
            DrawColor = 4;
            ColorStart = Color.Black;
            ColorEnd = Color.White;
            pictureBox1.Invalidate();
            pictureBox2.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics PaintPanel = e.Graphics;
            LinearGradientBrush LBrush = new LinearGradientBrush(pictureBox1.ClientRectangle, ColorStart, ColorEnd, LinearGradientMode.Horizontal);
            PaintPanel.FillRectangle(LBrush, pictureBox1.ClientRectangle);
        }
        /*开始绘制灰度直方图*/
        public void DrawHG()
        {
            pictureBox2.Invalidate();
        }
        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            int Rang = pictureBox2.Height;
            //int PixSum = PW * PH;
            Graphics PaintPanel = e.Graphics;
            SolidBrush BrushPen = new SolidBrush(Color.Black);
            Pen LinePen = new Pen(Color.Black, 0.1f);
            /*获取控件的宽度*/
            int PicBoxWidth = pictureBox2.Width;
            //计算标尺刻度
            float Scale = (float)pictureBox2.Width / 255.0f;

            if (DrawColor == 4)
            {
                pictureBox2.Image = pic;
                AdaptePictureSize(pictureBox2);
                Update();
            }
            else
            {
                for (int start = 0; start < 256; start++)
                {
                    if (DrawColor == 3)
                    {
                        float h1 = 0,h2= 0;
                        if (start != 0)
                            h1 = (float)pictureBox2.Bottom - Hist[0, start - 1];
                        else
                            h1 = 0;
                        h2 = (float)pictureBox2.Bottom - Hist[0, start];
                        PointF P1 = new PointF((float)(start - 1) * Scale + 0.1f, h1);
                        PointF P2 = new PointF((float)start * Scale + 0.1f, h2);
                        //读取当前点的个数
                        PaintPanel.DrawLine(new Pen(Color.Blue, 2.0f), P1, P2);
                        //PaintPanel.p


                        if (start != 0)
                            h1 = (float)pictureBox2.Bottom - Hist[1, start - 1];
                        else
                            h1 = 0;
                        h2 = (float)pictureBox2.Bottom - Hist[1, start];
                        P1 = new PointF((float)(start - 1) * Scale + 0.1f, h1);
                        P2 = new PointF((float)start * Scale + 0.1f, h2);
                        //读取当前点的个数
                        PaintPanel.DrawLine(new Pen(Color.Green, 2.0f), P1, P2);

                        if (start != 0)
                            h1 = (float)pictureBox2.Bottom - Hist[2, start - 1];
                        else
                            h1 = 0;
                        h2 = (float)pictureBox2.Bottom - Hist[2, start];
                        P1 = new PointF((float)(start - 1) * Scale + 0.1f, h1);
                        P2 = new PointF((float)start * Scale + 0.1f, h2);
                        //读取当前点的个数
                        PaintPanel.DrawLine(new Pen(Color.Red, 2.0f), P1, P2);
                    }
                    else if (DrawColor < 3 && DrawColor > -1)
                    {
                        float h = (float)pictureBox2.Bottom - Hist[DrawColor, start] ;
                        PointF P1 = new PointF((float)start * Scale + 0.1f, h);
                        PointF P2 = new PointF((float)start * Scale + 0.1f, (float)pictureBox2.Bottom);
                        //读取当前点的个数
                        PaintPanel.DrawLine(LinePen, P1, P2);
                    }

                }
            }
        }

        private void GraphicBar_Resize(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
            pictureBox2.Invalidate();
        }

        public void SetPic(Bitmap pic)
        {
            pictureBox2.Image = pic;
        }

        private void AdaptePictureSize(PictureBox Pb)
        {
            if (Pb.Image.Height > Pb.Height || Pb.Image.Width > Pb.Width)
                Pb.SizeMode = PictureBoxSizeMode.Zoom;
            else
                Pb.SizeMode = PictureBoxSizeMode.Normal;
        }
    }
}
