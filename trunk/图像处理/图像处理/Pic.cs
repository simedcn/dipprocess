using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace PICLib
{
    class Pic
    {
        public static  Bitmap OldPic;
        private int Width, Height;
        public Pic()
        {
            OldPic = null;
            Width = Height = 0;
        }

        public Pic(Bitmap PicIn)
        {
            OldPic = PicIn;
            Width = PicIn.Width;
            Height = PicIn.Height;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="PicIn"></param>
        public void SetPic(Bitmap PicIn)
        {
            OldPic = PicIn;
            Width = PicIn.Width;
            Height = PicIn.Height;
            return;
        }

        /// <summary>
        /// 灰度化
        /// </summary>
        /// <returns>灰度化后位图</returns>
        public  Bitmap Gray()
        {
            Bitmap b = (Bitmap) OldPic.Clone();
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, Width, Height),
                      ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - b.Width * 3;
                byte red, green, blue;
                for (int y = 0; y < Height; ++y)
                {
                    for (int x = 0; x < Width; ++x)
                    {
                        blue = p[0];
                        green = p[1];
                        red = p[2];
                        p[0] = p[1] = p[2] = (byte)(.299 * red + .587 * green + .114 * blue);
                        p += 3;
                    }
                    p += nOffset;
                }
            }
            b.UnlockBits(bmData);
            return b;
        }


        /// <summary>
        /// 取反色
        /// </summary>
        /// <returns></returns>
        public  Bitmap Invert()
        {
            Bitmap b = (Bitmap)OldPic.Clone();
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, Width, Height),
               ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - Width * 3;
                int nWidth = Width * 3;
                for (int y = 0; y < Height; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        p[0] = (byte)(255 - p[0]);
                        ++p;
                    }
                    p += nOffset;
                }
            }
            b.UnlockBits(bmData);
            return b;
        }

        ///// <summary>
        ///// 分离RGB通道
        ///// </summary>
        //public Bitmap[] SpliteRGB()
        //{
        //    if (OldPic != null)
        //    {
        //        Bitmap[] temp;
        //        temp[0] = splite(0);
        //        temp[1] = splite(1);
        //        temp[2] = splite(2);
                //if (splite(temp[0], 0))
                //    pictureBox4.Image = temp1;
                ////AdaptePictureSize(pictureBox4);
                ////Update();

                ////temp = (Bitmap)pictureBox1.Image.Clone();
                //if (splite(temp2, 1))
                //    pictureBox5.Image = temp2;
                ////AdaptePictureSize(pictureBox5);
                ////Update();

                ////temp = (Bitmap)pictureBox1.Image.Clone();
                //if (splite(temp3, 2))
                //    pictureBox6.Image = temp3;



        //        //AdaptePictureSize(pictureBox6);
        //    }
        //    //else
        //    //    MessageBox.Show("原始图像不存,请加载原图后再进行此项操作!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    //label3.Text = "分离RGB通道";
        //}


        /// <summary>
        /// 处理32位色位图一个通道
        /// </summary>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public Bitmap splite( int n)
        {
            Bitmap b = (Bitmap)OldPic.Clone();
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, Width, Height),
                     ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - b.Width * 4;
                for (int y = 0; y < Height; ++y)
                {
                    for (int x = 0; x < Width; ++x)
                    {
                        //blue = p[0];
                        //green = p[1];
                        //red = p[2];
                        //p[0] = p[1] = p[2] = (byte)(.299 * red + .587 * green + .114 * blue);
                        if (n == 0)
                            p[1] = p[2] = 0;
                        else if (n == 1)
                            p[0] = p[2] = 0;
                        else if (n == 2)
                            p[0] = p[1] = 0;
                        //else if (n == 3)
                        //    //p[0] = p[1] = p[2] = 0;
                        //    p[3] = 20;
                        p += 4;
                    }
                    p += nOffset;
                }
            }
            b.UnlockBits(bmData);
            return b;
        }

        /// <summary>
        /// 调整图像亮度
        /// </summary>
        /// <param name="b"></param>
        /// <param name="nBrightness"></param>
        /// <returns></returns>
        public  Bitmap Brightness(int nBrightness)
        {
            Bitmap b = (Bitmap)OldPic.Clone();
            if (nBrightness < -255 || nBrightness > 255)
                return null;
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, Width,
                                        Height), ImageLockMode.ReadWrite,
                                        PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            int nVal = 0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - Width * 3;
                int nWidth = b.Width * 3;
                for (int y = 0; y < Height; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        nVal = (int)(p[0] + nBrightness);
                        if (nVal < 0) nVal = 0;
                        if (nVal > 255) nVal = 255;
                        p[0] = (byte)nVal;
                        ++p;
                    }
                    p += nOffset;
                }
            }
            b.UnlockBits(bmData);
            return b;
        }


        /// <summary>
        /// 修改a通道
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public Bitmap ChangeA(byte Value)
        {
            Bitmap b = (Bitmap)OldPic.Clone();
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, Width, Height),
                     ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - b.Width * 4;
                for (int y = 0; y < Height; ++y)
                {
                    for (int x = 0; x < Width; ++x)
                    {
                    //    //blue = p[0];
                    //    //green = p[1];
                    //    //red = p[2];
                    //    //p[0] = p[1] = p[2] = (byte)(.299 * red + .587 * green + .114 * blue);
                    //    if (n == 0)
                    //        p[1] = p[2] = 0;
                    //    else if (n == 1)
                    //        p[0] = p[2] = 0;
                    //    else if (n == 2)
                    //        p[0] = p[1] = 0;
                        //else if (n == 3)
                        //    //p[0] = p[1] = p[2] = 0;
                        p[3] = Value;
                        p += 4;
                    }
                    p += nOffset;
                }
            }
            b.UnlockBits(bmData);
            return b;
        }


        public Bitmap UseLog(int times)
        {
            Bitmap b = (Bitmap)OldPic.Clone();
            //Bitmap b = Brightness(50);
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, Width, Height),
                     ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - b.Width * 4;
                for (int y = 0; y < Height; ++y)
                {
                    for (int x = 0; x < Width; ++x)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            byte  temp = (byte)(Math.Log(p[0] + 1) * 35);
                            if (temp < 255)
                                p[0] = temp;
                            else
                                p[0] = 255;
                            p++;
                        }
                        //p[0] = (byte)Math.Log10(p[0] + 1);
                        p += 1;
                    }
                    p += nOffset;
                }
            }
            b.UnlockBits(bmData);
            return b;
        }
    }
}
