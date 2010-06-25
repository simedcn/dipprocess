using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DIP
{
    public partial class DrawPic : UserControl
    {
        public Color ColorStart, ColorEnd;
        public Bitmap PicMap;
        int[] Hist;

        public DrawPic(int[] t)
        {
            InitializeComponent();
            //ColorStart = s;
            //ColorEnd = e;
            Hist = t;                      //直方图矩阵
            //pictureBox1.Invalidate();
        }
    }
}
