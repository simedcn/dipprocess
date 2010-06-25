using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace PICLib
{
    public class Pic
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


        /// <summary>
        /// 取对数
        /// </summary>
        /// <param name="times">对数系数</param>
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

        /// <summary>
        /// 白平衡
        /// </summary>
        public Bitmap WhiteBalance()
        {
            //throw new System.NotImplementedException();

            Bitmap b = (Bitmap)OldPic.Clone();
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, Width, Height),
                     ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = bmData.Stride;

            double[] Count = new double[Height * Width];
            int CountNow = 0;
            double MaxValue = 0;
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                //不是很明白用途
                int nOffset = stride - b.Width * 4;

                //列循环
                for (int y = 0; y < Height; ++y)
                {
                    //行循环
                    for (int x = 0; x < Width; ++x)
                    {
                        Count[CountNow] = p[0] * 0.114 + p[1] * 0.587 + p[2] * 0.299;
                        if (Count[CountNow] > MaxValue)
                            MaxValue = Count[CountNow];
                        p += 4;
                        CountNow++;
                    }
                    p += nOffset;
                }

                //白色点数目
                int CountWhite = 0;
                
                //存储白色的RGB平均值
                double[] CountRGB = {0,0,0};
                CountNow = 0;
                p = (byte*)(void*)Scan0;
                while (CountNow < Height * Width)
                {
                    if (Count[CountNow] > MaxValue * 0.95)
                    {
                        CountRGB[0] += p[CountNow * 4];
                        CountRGB[1] += p[CountNow * 4 + 1];
                        CountRGB[2] += p[CountNow * 4 + 2];
                        CountWhite++;
                    }
                    CountNow++;
                }

                CountRGB[0] = CountRGB[0] / CountWhite;
                CountRGB[1] = CountRGB[1] / CountWhite;
                CountRGB[2] = CountRGB[2] / CountWhite;

                double Y = (CountRGB[0] + CountRGB[1] + CountRGB[2]) / 3;

                //CountNow = 0;
                //CountRGB[0] = CountRGB[1] = CountRGB[2] = 0;
                //while (CountNow < Height * Width)
                //{
                //    //if (Count[CountNow] > MaxValue * 0.95)
                //    {
                //        CountRGB[0] += p[CountNow * 4];
                //        CountRGB[1] += p[CountNow * 4 + 1];
                //        CountRGB[2] += p[CountNow * 4 + 2];
                //        //CountWhite++;
                //    }
                //    CountNow++;
                //}

                //CountRGB[0] = CountRGB[0] / CountNow;
                //CountRGB[1] = CountRGB[1] / CountNow;
                //CountRGB[2] = CountRGB[2] / CountNow;

                double[] K = { Y / CountRGB[0], Y / CountRGB[1], Y / CountRGB[2] };

                //列循环
                for (int y = 0; y < Height; ++y)
                {
                    //行循环
                    for (int x = 0; x < Width; ++x)
                    {
                        //逐像素进行调整
                        
                        if (p[0] * K[0] > 255)
                            p[0] = 255;
                        else
                            p[0] = (byte)(p[0] * K[0]);

                        if (p[1] * K[1] > 255)
                            p[1] = 255;
                        else
                            p[1] = (byte)(p[1] * K[1]);

                        if (p[2] * K[2] > 255)
                            p[2] = 255;
                        else
                            p[2] = (byte)(p[2] * K[2]);
                        p += 4;
                        CountNow++;
                    }
                    p += nOffset;
                }

            }
            b.UnlockBits(bmData);
            return b;
        }

        /// <summary>
        /// 画直方图
        /// </summary>
        /// <param name="DrawColor">颜色通道</param>
        public Bitmap ZhiFangtu(int DrawColor)
        {
            //各通道颜色统计
            double[,] PixCount = new double[3,256];
            //
            double ResultHeight = 0;

            Bitmap b = (Bitmap)OldPic.Clone();
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, Width, Height),
                     ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            //b.UnlockBits(bmData);

            //byte temp = 0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - b.Width * 4;
                for (int y = 0; y < Height; ++y)
                {
                    for (int x = 0; x < Width; ++x)
                    {

                        //temp = *p;
                        //PixCount[0, p[0]]++;
                        //PixCount[1, p[1]]++;
                        for (int i = 0; i < 3; i++)
                        {
                            PixCount[i, p[i]]++;
                            if (PixCount[i, p[i]] > ResultHeight)
                                ResultHeight = (int)PixCount[i, p[i]];
                        }
                         p += 4;
                    }
                    p += nOffset;
                }
            }
            b.UnlockBits(bmData);

            for (int i = 0; i < 256; i++)
            {
                PixCount[0 , i] /= (Height * Width * 0.01);
                PixCount[1 , i] /= (Height * Width * 0.01);
                PixCount[2 , i] /= (Height * Width * 0.01);
            }
            ResultHeight = ResultHeight/(Height * Width * 0.01) * 60;

            Bitmap br = new Bitmap(256,(int) ResultHeight);
            bmData = br.LockBits(new Rectangle(0, 0, 256,(int) ResultHeight),
                     ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            stride = bmData.Stride;
            Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                for (int curr = 0; curr < 256; curr++)
                {
                    if (DrawColor > -1 && DrawColor < 3)
                    {

                        for (int i = (int)ResultHeight - 1; i >= (int)ResultHeight - (int)(PixCount[DrawColor, curr] * 60); i--)
                        {
                            
                            p[1024 * (i) + curr * 4 + DrawColor] = 255;
                            p[1024 * (i) + curr * 4 + 3] = 255;                            
                        
                        }
                    }
                    else
                    {
                        for (int RGB = 0; RGB < 3; RGB++)
                        {
                            for (int i = (int)ResultHeight - 1; i >= (int)ResultHeight - (int)(PixCount[RGB, curr] * 60); i--)
                            {

                                p[1024 * (i) + curr * 4 + RGB] = 255;
                                p[1024 * (i) + curr * 4 + 3] = 180;

                            }
                            
                        }
                    }

                    //g.DrawLine(System.Drawing.Pens.Blue, curr, ResultHeight, curr, ResultHeight - PixCount[DrawColor, curr]);
                }
                //for (int i = 0; i < ResultHeight; i++)
                //{
                //    p[1024 * i] = p[1024 * i + 1] = p[1024 * i + 2] = p[1024 * i + 1020] = p[1024 * i + 1021] = p[1024 * i + 1022] = 0;
                //}
            }
            br.UnlockBits(bmData);
            //br.Save(@"D:\a.bmp");
            return br;
            //throw new System.NotImplementedException();
        }

        public int[,] ZhiFangtu2()
        {
            //各通道颜色统计
            int[,] PixCount = new int[3, 256];
            //
            double ResultHeight = 0;

            Bitmap b = (Bitmap)OldPic.Clone();
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, Width, Height),
                     ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            //b.UnlockBits(bmData);

            //byte temp = 0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - b.Width * 4;
                for (int y = 0; y < Height; ++y)
                {
                    for (int x = 0; x < Width; ++x)
                    {

                        //temp = *p;
                        //PixCount[0, p[0]]++;
                        //PixCount[1, p[1]]++;
                        for (int i = 0; i < 3; i++)
                        {
                            PixCount[i, p[i]]++;
                            if (PixCount[i, p[i]] > ResultHeight)
                                ResultHeight = PixCount[i, p[i]];
                        }
                        p += 4;
                    }
                    p += nOffset;
                }
            }
            b.UnlockBits(bmData);

            for (int i = 0; i < 256; i++)
            {
                PixCount[0, i] = (int)(PixCount[0, i] / (Height * Width * 0.01) * 60);
                PixCount[1, i] = (int)(PixCount[1, i] / (Height * Width * 0.01) * 60);
                PixCount[2, i] = (int)(PixCount[2, i] / (Height * Width * 0.01) * 60);
            }
            ResultHeight = ResultHeight / (Height * Width * 0.01) * 60;

            
            //br.Save(@"D:\a.bmp");
            return PixCount;
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// 白平衡
        /// </summary>
        public Bitmap WhiteBalanceYCbCr()
        {
            //throw new System.NotImplementedException();

            Bitmap b = (Bitmap)OldPic.Clone();
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, Width, Height),
                     ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = bmData.Stride;

            double[] Count = new double[Height * Width];
            int CountNow = 0;
            double MaxValue = 0;
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                //行偏移量
                int nOffset = stride - b.Width * 4;

                //列循环
                for (int y = 0; y < Height; ++y)
                {
                    //行循环
                    for (int x = 0; x < Width; ++x)
                    {
                        Count[CountNow] = p[0] * 0.114 + p[1] * 0.587 + p[2] * 0.299;
                        if (Count[CountNow] > MaxValue)
                            MaxValue = Count[CountNow];
                        p += 4;
                        CountNow++;
                    }
                    p += nOffset;
                }

                //白色点数目
                int CountWhite = 0;

                //存储白色的RGB平均值
                double[] CountRGB = { 0, 0, 0 };
                CountNow = 0;
                p = (byte*)(void*)Scan0;
                while (CountNow < Height * Width)
                {
                    if (Count[CountNow] > MaxValue * 0.95)
                    {
                        CountRGB[0] += p[CountNow * 4];
                        CountRGB[1] += p[CountNow * 4 + 1];
                        CountRGB[2] += p[CountNow * 4 + 2];
                        CountWhite++;
                    }
                    CountNow++;
                }

                CountRGB[0] = CountRGB[0] / CountWhite;
                CountRGB[1] = CountRGB[1] / CountWhite;
                CountRGB[2] = CountRGB[2] / CountWhite;

                double Y = CountRGB[0] * 0.114 + CountRGB[1] * 0.587 + CountRGB[2] * 0.299;/*(CountRGB[0] + CountRGB[1] + CountRGB[2]) / 3*/ ;

                //CountNow = 0;
                //CountRGB[0] = CountRGB[1] = CountRGB[2] = 0;
                //while (CountNow < Height * Width)
                //{
                //    //if (Count[CountNow] > MaxValue * 0.95)
                //    {
                //        CountRGB[0] += p[CountNow * 4];
                //        CountRGB[1] += p[CountNow * 4 + 1];
                //        CountRGB[2] += p[CountNow * 4 + 2];
                //        //CountWhite++;
                //    }
                //    CountNow++;
                //}

                //CountRGB[0] = CountRGB[0] / CountNow;
                //CountRGB[1] = CountRGB[1] / CountNow;
                //CountRGB[2] = CountRGB[2] / CountNow;

                double[] K = { Y / CountRGB[0], Y / CountRGB[1], Y / CountRGB[2] };

                //列循环
                for (int y = 0; y < Height; ++y)
                {
                    //行循环
                    for (int x = 0; x < Width; ++x)
                    {
                        //逐像素进行调整

                        if (p[0] * K[0] > 255)
                            p[0] = 255;
                        else
                            p[0] = (byte)(p[0] * K[0]);

                        if (p[1] * K[1] > 255)
                            p[1] = 255;
                        else
                            p[1] = (byte)(p[1] * K[1]);

                        if (p[2] * K[2] > 255)
                            p[2] = 255;
                        else
                            p[2] = (byte)(p[2] * K[2]);
                        p += 4;
                        CountNow++;
                    }
                    p += nOffset;
                }

            }
            b.UnlockBits(bmData);
            return b;
        }

        /// <summary>
        /// 彩色补偿
        /// </summary>
        public Bitmap ColorCompensate()
        {
            //throw new System.NotImplementedException();

            Bitmap b = (Bitmap)OldPic.Clone();
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, Width, Height),
                     ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = bmData.Stride;

            double[,] Count = new double[Height * Width, 3];
            int[] eRGB = { 0, 0, 0 };
            double[] MaxValue = { 0, 0, 0 };
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                //行偏移量
                int nOffset = stride - b.Width * 4;
                int CountNow = 0;

                /*
                 * er=(r-b)+(r-g)
                 * eg=(g-b)+(g-r)
                 * eb=(b-r)+(b-g)
                 */

                //列循环
                for (int y = 0; y < Height; ++y)
                {
                    //行循环
                    for (int x = 0; x < Width; ++x)
                    {

                        Count[CountNow, 0] = 2 * p[0] - p[1] - p[2];
                        if (Count[CountNow, 0] > MaxValue[0])
                            MaxValue[0] = Count[CountNow, 0];

                        Count[CountNow, 1] = 2 * p[1] - p[0] - p[2];
                        if (Count[CountNow, 1] > MaxValue[1])
                            MaxValue[1] = Count[CountNow, 1];

                        Count[CountNow, 2] = 2 * p[2] - p[0] - p[1];
                        if (Count[CountNow, 2] > MaxValue[2])
                            MaxValue[2] = Count[CountNow, 2];

                        p += 4;
                        CountNow++;
                    }
                    p += nOffset;
                }


                p = (byte*)(void*)Scan0;
                double[,] AverageRGB = new double [ 3, 3 ];
                int[] Sum = { 0, 0, 0 };

                //遍历图像所有像素
                for (int curr = 0; curr < Height * Width; curr++)
                {
                    //分别计算RGB通道
                    for (int i = 0; i < 3; i++)
                    {
                        if (Count[curr,i] == MaxValue[i])
                        {
                            AverageRGB[i, 0] += p[curr * 4];
                            AverageRGB[i, 1] += p[curr * 4 + 1];
                            AverageRGB[i, 2] += p[curr * 4 + 2];
                            Sum[i]++;
                        }
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        AverageRGB[i, j] /= Sum[i];
                        AverageRGB[i, j] /= Sum[i];
                        AverageRGB[i, j] /= Sum[i];
                    }
                }

                //形成矩阵A1
                /*
                 * 
                */

                double[,] A1 ={
                                {AverageRGB[0,0],AverageRGB[1,0],AverageRGB[2,0]},
                                {AverageRGB[0,1],AverageRGB[1,1],AverageRGB[2,1]},
                                {AverageRGB[0,2],AverageRGB[1,2],AverageRGB[2,2]},
                             };

                //形成矩阵A2
                double[,] A2 = {
                                   {255 ,   0  ,   0    },
                                   {0   ,   255,   0    },
                                   {0   ,   0  ,   255  },
                               };

                //求矩阵A1的逆
                double[,] Areserve = ReverseMatrix(A1, 3);
                double[,] C = MatrixCompute(A2, Areserve);
                byte[] result = { 0, 0, 0 };
                for(int i=0;i<Height*Width;i++)
                {
                    //处理兰色通道
                    if (C[0, 0] * p[4 * i] + C[0, 1] * p[4 * i + 1] + C[0, 2] * p[4 * i + 2] > 255)
                        result[0] = 255;
                    else if (C[0, 0] * p[4 * i] + C[0, 1] * p[4 * i + 1] + C[0, 2] * p[4 * i + 2] < 0)
                        result[0] = 0;
                    else
                        result[0] = (byte)(C[0, 0] * p[4 * i] + C[0, 1] * p[4 * i + 1] + C[0, 2] * p[4 * i + 2]);

                    //处理绿色通道
                    if (C[1, 0] * p[4 * i] + C[1, 1] * p[4 * i + 1] + C[1, 2] * p[4 * i + 2] > 255)
                        result[1] = 255;
                    else if (C[1, 0] * p[4 * i] + C[1, 1] * p[4 * i + 1] + C[1, 2] * p[4 * i + 2] < 0)
                        result[1] = 0;
                    else
                        result[1] = (byte)(C[1, 0] * p[4 * i] + C[1, 1] * p[4 * i + 1] + C[1, 2] * p[4 * i + 2]);

                    //处理红色通道
                    if (C[2, 0] * p[4 * i] + C[2, 1] * p[4 * i + 1] + C[2, 2] * p[4 * i + 2] > 255)
                        result[2] = 255;
                    else if (C[2, 0] * p[4 * i] + C[2, 1] * p[4 * i + 1] + C[2, 2] * p[4 * i + 2] < 0)
                        result[2] = 0;
                    else
                        result[2] = (byte)(C[2, 0] * p[4 * i] + C[2, 1] * p[4 * i + 1] + C[2, 2] * p[4 * i + 2]);
                    p[4 * i    ] = result[0];
                    p[4 * i + 1] = result[1];
                    p[4 * i + 2] = result[2];
                }
            }

            b.UnlockBits(bmData);
            return b;
        }

        private double[,] ReverseMatrix(double[,] dMatrix, int Level)
        {

            double dMatrixValue = MatrixValue(dMatrix, Level);

            if (dMatrixValue == 0) return null;



            double[,] dReverseMatrix = new double[Level, 2 * Level];

            double x, c;

            // Init Reverse matrix 

            for (int i = 0; i < Level; i++)
            {

                for (int j = 0; j < 2 * Level; j++)
                {

                    if (j < Level)

                        dReverseMatrix[i, j] = dMatrix[i, j];

                    else

                        dReverseMatrix[i, j] = 0;

                }



                dReverseMatrix[i, Level + i] = 1;

            }



            for (int i = 0, j = 0; i < Level && j < Level; i++, j++)
            {

                if (dReverseMatrix[i, j] == 0)
                {

                    int m = i;

                    for (; dMatrix[m, j] == 0; m++) ;

                    if (m == Level)

                        return null;

                    else
                    {

                        // Add i-row with m-row

                        for (int n = j; n < 2 * Level; n++)

                            dReverseMatrix[i, n] += dReverseMatrix[m, n];

                    }

                }



                // Format the i-row with "1" start

                x = dReverseMatrix[i, j];

                if (x != 1)
                {

                    for (int n = j; n < 2 * Level; n++)

                        if (dReverseMatrix[i, n] != 0)

                            dReverseMatrix[i, n] /= x;

                }



                // Set 0 to the current column in the rows after current row

                for (int s = Level - 1; s > i; s--)
                {

                    x = dReverseMatrix[s, j];

                    for (int t = j; t < 2 * Level; t++)

                        dReverseMatrix[s, t] -= (dReverseMatrix[i, t] * x);

                }

            }



            // Format the first matrix into unit-matrix

            for (int i = Level - 2; i >= 0; i--)
            {

                for (int j = i + 1; j < Level; j++)

                    if (dReverseMatrix[i, j] != 0)
                    {

                        c = dReverseMatrix[i, j];

                        for (int n = j; n < 2 * Level; n++)

                            dReverseMatrix[i, n] -= (c * dReverseMatrix[j, n]);

                    }

            }



            double[,] dReturn = new double[Level, Level];

            for (int i = 0; i < Level; i++)

                for (int j = 0; j < Level; j++)

                    dReturn[i, j] = dReverseMatrix[i, j + Level];

            return dReturn;

        }

        private double MatrixValue(double[,] MatrixList, int Level)
        {

            double[,] dMatrix = new double[Level, Level];

            for (int i = 0; i < Level; i++)

                for (int j = 0; j < Level; j++)

                    dMatrix[i, j] = MatrixList[i, j];

            double c, x;

            int k = 1;

            for (int i = 0, j = 0; i < Level && j < Level; i++, j++)
            {

                if (dMatrix[i, j] == 0)
                {

                    int m = i;

                    for (; dMatrix[m, j] == 0; m++) ;

                    if (m == Level)

                        return 0;

                    else
                    {

                        // Row change between i-row and m-row

                        for (int n = j; n < Level; n++)
                        {

                            c = dMatrix[i, n];

                            dMatrix[i, n] = dMatrix[m, n];

                            dMatrix[m, n] = c;

                        }



                        // Change value pre-value

                        k *= (-1);

                    }

                }



                // Set 0 to the current column in the rows after current row

                for (int s = Level - 1; s > i; s--)
                {

                    x = dMatrix[s, j];

                    for (int t = j; t < Level; t++)

                        dMatrix[s, t] -= dMatrix[i, t] * (x / dMatrix[i, j]);

                }

            }



            double sn = 1;

            for (int i = 0; i < Level; i++)
            {

                if (dMatrix[i, i] != 0)

                    sn *= dMatrix[i, i];

                else

                    return 0;

            }

            return k * sn;

        }

        private double[,] MatrixCompute(double[,] matrixA, double[,] matrixB)
        {
            double[,] matrixC = new double[3, 3];
            // 计算矩阵A和矩阵B的乘积
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // 初始化矩阵C
                    matrixC[i, j] = 0;

                    // 计算矩阵A和矩阵B的乘积，并把值存放在矩阵C中
                    for (int k = 0; k < 3; k++)
                    {
                        matrixC[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }
            }

            return matrixC;

        }
    }
}
