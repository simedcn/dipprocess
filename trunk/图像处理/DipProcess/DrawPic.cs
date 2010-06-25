using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace DipProcess
{
    public partial class DrawPic : UserControl
    {
        public DrawPic()
        {
            InitializeComponent();
        }

        private void SavePic_Click(object sender, EventArgs e)
        {
            string PicPath;

            SaveFileDialog PicOpen = new SaveFileDialog();
            PicOpen.Filter = "Bitmap文件(*.bmp)|*.bmp|Jpeg文件(*.Jpeg)|*.jpg|PNG文件|*.PNG|GIF文件|*.gif";
             //PicOpen.Filter = "Jpeg文件(*.jpg)|*.jpg";
            PicOpen.FilterIndex = 2;
            PicOpen.RestoreDirectory = true;
            if (PicOpen.ShowDialog() == DialogResult.OK)
            {
                //UnLoadAll();
                PicPath = PicOpen.FileName;
                //pictureBox1.Image.Save(PicOpen.FileName,ImageFormat.Jpeg);
                switch (PicOpen.FilterIndex)
                {
                    case 1:
                        {
                            pictureBox1.Image.Save(PicPath, ImageFormat.Bmp);
                            break;
                        }
                    case 2:
                        {
                            pictureBox1.Image.Save(PicOpen.FileName, ImageFormat.Jpeg);
                            break;
                        }
                    case 3:
                        {
                            pictureBox1.Image.Save(PicPath, ImageFormat.Png);
                            break;
                        }
                    case 4:
                        {
                            pictureBox1.Image.Save(PicPath, ImageFormat.Gif);
                            break;
                        }
                    default:
                        {
                            pictureBox1.Image.Save(PicPath, ImageFormat.Jpeg);
                            break;
                        }
                }
            }
            
        }

        #region 备用
        //public DrawPic(PICLib.GrayImg pic)
        //{
        //    InitializeComponent();

        //    PICLib.PicBase.ArrayToImg(out PicMap, pic);

        //    pictureBox1.Image = PicMap;
        //    if ((pic.Width < 320 && pic.Height < 240) || (pic.Width < 240 && pic.Height < 320))
        //    {
        //        pictureBox1.Width = pic.Width;
        //        pictureBox1.Height = pic.Height;
        //        Height = pic.Height + 12;
        //        Width = pic.Width;
        //    }
        //    else if (pic.Height < pic.Width)
        //    {
        //        Width = 320;
        //        Height = 252;

        //        pictureBox1.Width = 320;
        //        pictureBox1.Height = 240;
        //    }
        //    else
        //    {
        //        Width = 240;
        //        Height = 332;

        //        pictureBox1.Width = 240;
        //        pictureBox1.Height = 320;
        //    }
        //}

        //public DrawPic(Bitmap pic)
        //{
        //    InitializeComponent();
        //    pictureBox1.Image = pic;
        //    if ((pic.Width < 320 && pic.Height < 240) || (pic.Width < 240 && pic.Height < 320))
        //    {
        //        pictureBox1.Width = pic.Width;
        //        pictureBox1.Height = pic.Height;
        //        Height = pic.Height + 12;
        //        Width = pic.Width;
        //    }
        //    else if (pic.Height < pic.Width)
        //    {
        //        Width = 320;
        //        Height = 252;

        //        pictureBox1.Width = 320;
        //        pictureBox1.Height = 240;
        //    }
        //    else
        //    {
        //        Width = 240;
        //        Height = 332;

        //        pictureBox1.Width = 240;
        //        pictureBox1.Height = 320;
        //    }
        //}

        //public DrawPic(PICLib.GrayImg pic, string ShowTxt)
        //{
        //    InitializeComponent();
        //    PICLib.PicBase.ArrayToImg(out PicMap, pic);

        //    pictureBox1.Image = PicMap;
        //    TxtShow.Text = ShowTxt;

        //    if ((pic.Width < 320 && pic.Height < 240) || (pic.Width < 240 && pic.Height < 320))
        //    {
        //        pictureBox1.Width = pic.Width;
        //        pictureBox1.Height = pic.Height;
        //        Height = pic.Height + 12;
        //        Width = pic.Width;
        //    }
        //    else if (pic.Height < pic.Width)
        //    {
        //        Width = 320;
        //        Height = 252;

        //        pictureBox1.Width = 320;
        //        pictureBox1.Height = 240;
        //    }
        //    else
        //    {
        //        Width = 240;
        //        Height = 332;

        //        pictureBox1.Width = 240;
        //        pictureBox1.Height = 320;
        //    }
        //}

        //public DrawPic(Bitmap pic, string ShowTxt)
        //{
        //    InitializeComponent();
        //    pictureBox1.Image = pic;
        //    TxtShow.Text = ShowTxt;

        //    if ((pic.Width < 320 && pic.Height < 240) || (pic.Width < 240 && pic.Height < 320))
        //    {
        //        pictureBox1.Width = pic.Width;
        //        pictureBox1.Height = pic.Height;
        //        Height = pic.Height + 12;
        //        Width = pic.Width;
        //    }
        //    else if (pic.Height < pic.Width)
        //    {
        //        Width = 320;
        //        Height = 252;

        //        pictureBox1.Width = 320;
        //        pictureBox1.Height = 240;
        //    }
        //    else
        //    {
        //        Width = 240;
        //        Height = 332;

        //        pictureBox1.Width = 240;
        //        pictureBox1.Height = 320;
        //    }
        //}

        //public DrawPic(PICLib.GrayImg pic, int Size)
        //{
        //    InitializeComponent();

        //    PICLib.PicBase.ArrayToImg(out PicMap, pic);

        //    pictureBox1.Image = PicMap;

        //    if (pic.Height < pic.Width)
        //    {
        //        Width = 200;
        //        Height = 162;

        //        pictureBox1.Width = 200;
        //        pictureBox1.Height = 150;
        //    }
        //    else
        //    {
        //        Width = 150;
        //        Height = 212;

        //        pictureBox1.Width = 150;
        //        pictureBox1.Height = 200;
        //    }
        //    //pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        //}

        //public DrawPic(Bitmap pic, int Size)
        //{
        //    InitializeComponent();



        //    pictureBox1.Image = pic;

        //    if (pic.Height < pic.Width)
        //    {
        //        Width = 200;
        //        Height = 162;

        //        pictureBox1.Width = 200;
        //        pictureBox1.Height = 150;
        //    }
        //    else
        //    {
        //        Width = 150;
        //        Height = 212;

        //        pictureBox1.Width = 150;
        //        pictureBox1.Height = 200;
        //    }
        //    //pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        //}

        //public DrawPic(PICLib.GrayImg pic, string ShowTxt, int Size)
        //{
        //    InitializeComponent();

        //    //Height = 162;
        //    //Width = 200;

        //    PICLib.PicBase.ArrayToImg(out PicMap, pic);

        //    pictureBox1.Image = PicMap;
        //    TxtShow.Text = ShowTxt;

        //    if (pic.Height < pic.Width)
        //    {
        //        Height = 162;
        //        Width = 200;
        //        pictureBox1.Height = 150;
        //        pictureBox1.Width = 200;
        //    }
        //    else
        //    {
        //        Width = 162;
        //        Height = 200;
        //        pictureBox1.Height = 200;
        //        pictureBox1.Width = 150;
        //    }

        //    //pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        //}

        //public DrawPic(Bitmap pic, string ShowTxt, int Size)
        //{
        //    InitializeComponent();

        //    //Height = 162;
        //    //Width = 200;

        //    pictureBox1.Image = pic;
        //    TxtShow.Text = ShowTxt;

        //    if (pic.Height < pic.Width)
        //    {
        //        Height = 162;
        //        Width = 200;
        //        pictureBox1.Height = 150;
        //        pictureBox1.Width = 200;
        //    }
        //    else
        //    {
        //        Width = 162;
        //        Height = 200;
        //        pictureBox1.Height = 200;
        //        pictureBox1.Width = 150;
        //    }

        //    //pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        //}
        #endregion

    }
}
