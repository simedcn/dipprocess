using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace PICLib
{
    class UseGDIPlus
    {
        public Bitmap OldPic;
        private int Width, Height;
        Color colorTemp;

        public UseGDIPlus()
        {
            OldPic = null;
            Width = Height = 0;
        }

        public UseGDIPlus(Bitmap PicIn)
        {
            OldPic = PicIn;
            Width = OldPic.Width;
            Height = OldPic.Height;
        }
        public bool SetPic(Bitmap PicIn)
        {
            OldPic = PicIn;
            Width = OldPic.Width;
            Height = OldPic.Height;
            return true;
        }

        public Bitmap SmoothPic()
        {
            Bitmap temp = (Bitmap)OldPic.Clone();
            
            BitmapData bmData = temp.LockBits(new Rectangle(0, 0, Width, Height),
              ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = bmData.Stride;

            //获取图像第一个像素地址
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - Width * 4;  //每行像素数据偏移量
                int nWidth = Width * 4;
                for (int y = 1; y < Height - 1; y++)
                {
                    for (int x = 1; x < Width - 1; x++)
                    {
                        //p[0] = (byte)(255 - p[0]);
                        //++p;
                        int rSum = 0;
                        int gSum = 0;
                        int bSum = 0;

                        bSum = p[nWidth * (y - 1) + 4 * x - 4] + p[nWidth * (y - 1) + 4 * x    ] + p[nWidth * (y - 1) + 4 * x + 4] +
                               p[nWidth * (y    ) + 4 * x - 4] + p[nWidth * (y    ) + 4 * x    ] + p[nWidth * (y    ) + 4 * x + 4] +
                               p[nWidth * (y + 1) + 4 * x - 4] + p[nWidth * (y + 1) + 4 * x    ] + p[nWidth * (y + 1) + 4 * x + 4];

                        gSum = p[nWidth * (y - 1) + 4 * x - 3] + p[nWidth * (y - 1) + 4 * x + 1] + p[nWidth * (y - 1) + 4 * x + 5] +
                               p[nWidth * (y    ) + 4 * x - 3] + p[nWidth * (y    ) + 4 * x + 1] + p[nWidth * (y    ) + 4 * x + 4] +
                               p[nWidth * (y + 1) + 4 * x - 3] + p[nWidth * (y + 1) + 4 * x + 1] + p[nWidth * (y + 1) + 4 * x + 5];

                        rSum = p[nWidth * (y - 1) + 4 * x - 2] + p[nWidth * (y - 1) + 4 * x + 2] + p[nWidth * (y - 1) + 4 * x + 6] +
                               p[nWidth * (y    ) + 4 * x - 2] + p[nWidth * (y    ) + 4 * x + 2] + p[nWidth * (y    ) + 4 * x + 6] +
                               p[nWidth * (y + 1) + 4 * x - 2] + p[nWidth * (y + 1) + 4 * x + 2] + p[nWidth * (y + 1) + 4 * x + 6];

                        p[nWidth * (y) + 4 * x    ] = (byte)(bSum / 9);
                        p[nWidth * (y) + 4 * x + 1] = (byte)(gSum / 9);
                        p[nWidth * (y) + 4 * x + 2] = (byte)(rSum / 9);

                        
                    }
                }
            }
            temp.UnlockBits(bmData);
            return temp;
        }


        public Bitmap SmoothPic_Tranditional()
        {
            Bitmap temp = (Bitmap)OldPic.Clone();
            Color[,] color = new Color[3, 3];

            for (int x = 1; x < Width - 1; x++)
            {
                for (int y = 1; y < Height - 1; y++)
                {
                    color[0, 0] = temp.GetPixel(x - 1, y - 1);
                    color[0, 1] = temp.GetPixel(x - 1, y    );
                    color[0, 2] = temp.GetPixel(x - 1, y + 1);

                    color[1, 0] = temp.GetPixel(x, y - 1);
                    color[1, 1] = temp.GetPixel(x, y    );
                    color[1, 2] = temp.GetPixel(x, y + 1);

                    color[2, 0] = temp.GetPixel(x + 1, y - 1);
                    color[2, 1] = temp.GetPixel(x + 1, y    );
                    color[2, 2] = temp.GetPixel(x + 1, y + 1);

                    int rSum = 0;
                    int gSum = 0;
                    int bSum = 0;

                    for (int n = 0; n < 3; n++)
                    {
                        for (int nn = 0; nn < 3; nn++)
                        {
                            rSum += color[n, nn].R;
                            gSum += color[n, nn].G;
                            bSum += color[n, nn].B;
                        }

                    }
                    colorTemp = Color.FromArgb(255, (int)(rSum / 9), (int)(gSum / 9), (int)(bSum / 9));
                    temp.SetPixel(x, y, colorTemp);
                }
            }

            //temp.UnlockBits(bmData);
            return temp;
        }

        /// <summary>
        /// 图像锐化
        /// </summary>
        /// <param name="dep">锐化深度</param>
        /// <returns>处理后BitMap</returns>
        public Bitmap PicSharp(double dep)
        {
            Bitmap temp = (Bitmap)OldPic.Clone();

            BitmapData bmData = temp.LockBits(new Rectangle(0, 0, Width, Height),
              ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = bmData.Stride;

            //获取图像第一个像素地址
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - Width * 4;  //每行像素数据偏移量
                int nWidth = Width * 4;
                for (int y = 1; y < Height - 1; y++)
                {
                    for (int x = 1; x < Width - 1; x++)
                    {
                        double r, g, b;
                        b = (p[nWidth * (y) + 4 * x    ] + (p[nWidth * (y) + 4 * x    ] * 4 - p[nWidth * (y) + 4 * x - 4] + p[nWidth * (y) + 4 * x + 4] - p[nWidth * (y - 1) + 4 * x    ] + p[nWidth * (y + 1) + 4 * x    ]) * dep) / (1 + 4 * dep);
                        g = (p[nWidth * (y) + 4 * x + 1] + (p[nWidth * (y) + 4 * x + 1] * 4 - p[nWidth * (y) + 4 * x - 3] + p[nWidth * (y) + 4 * x + 5] - p[nWidth * (y - 1) + 4 * x + 1] + p[nWidth * (y + 1) + 4 * x + 1]) * dep) / (1 + 4 * dep);
                        r = (p[nWidth * (y) + 4 * x + 2] + (p[nWidth * (y) + 4 * x + 2] * 4 - p[nWidth * (y) + 4 * x - 2] + p[nWidth * (y) + 4 * x + 6] - p[nWidth * (y - 1) + 4 * x + 2] + p[nWidth * (y + 1) + 4 * x + 2]) * dep) / (1 + 4 * dep);
                        

                        p[nWidth * (y) + 4 * x] = (byte)Math.Min(255,Math.Max(0,b));
                        p[nWidth * (y) + 4 * x + 1] = (byte)Math.Min(255, Math.Max(0, g));
                        p[nWidth * (y) + 4 * x + 2] = (byte)Math.Min(255, Math.Max(0, r));


                    }
                }
            }
            temp.UnlockBits(bmData);
            return temp;
        }

        public Bitmap RectangeChange(double[,] Retange)
        {
            double sum = 0;
            int length = Retange.Rank + 1;
            for (int i = 0; i < length; i++)
                for (int j = 0; j < length; j++)
                    sum += Retange[i, j];

            Bitmap temp = (Bitmap)OldPic.Clone();

            BitmapData bmData = temp.LockBits(new Rectangle(0, 0, Width, Height),
              ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = bmData.Stride;

            //获取图像第一个像素地址
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - Width * 4;  //每行像素数据偏移量
                int nWidth = Width * 4;
                if (Retange.Rank == 2)
                {
                    for (int y = 1; y < Height - 1; y++)
                    {
                        for (int x = 1; x < Width - 1; x++)
                        {
                            double rSum = 0;
                            double gSum = 0;
                            double bSum = 0;

                            bSum = p[nWidth * (y - 1) + 4 * x - 4] * Retange[0, 0] + p[nWidth * (y - 1) + 4 * x] * Retange[0, 1] + p[nWidth * (y - 1) + 4 * x + 4] * Retange[0, 2] +
                                   p[nWidth * (y) + 4 * x - 4] * Retange[1, 0] + p[nWidth * (y) + 4 * x] * Retange[1, 1] + p[nWidth * (y) + 4 * x + 4] * Retange[1, 2] +
                                   p[nWidth * (y + 1) + 4 * x - 4] * Retange[2, 0] + p[nWidth * (y + 1) + 4 * x] * Retange[2, 1] + p[nWidth * (y + 1) + 4 * x + 4] * Retange[2, 2];

                            gSum = p[nWidth * (y - 1) + 4 * x - 3] * Retange[0, 0] + p[nWidth * (y - 1) + 4 * x + 1] * Retange[0, 1] + p[nWidth * (y - 1) + 4 * x + 5] * Retange[0, 2] +
                                   p[nWidth * (y) + 4 * x - 3] * Retange[1, 0] + p[nWidth * (y) + 4 * x + 1] * Retange[1, 1] + p[nWidth * (y) + 4 * x + 5] * Retange[1, 2] +
                                   p[nWidth * (y + 1) + 4 * x - 3] * Retange[2, 0] + p[nWidth * (y + 1) + 4 * x + 1] * Retange[2, 1] + p[nWidth * (y + 1) + 4 * x + 5] * Retange[2, 2];

                            rSum = p[nWidth * (y - 1) + 4 * x - 2] * Retange[0, 0] + p[nWidth * (y - 1) + 4 * x + 2] * Retange[0, 1] + p[nWidth * (y - 1) + 4 * x + 6] * Retange[0, 2] +
                                   p[nWidth * (y) + 4 * x - 2] * Retange[1, 0] + p[nWidth * (y) + 4 * x + 2] * Retange[1, 1] + p[nWidth * (y) + 4 * x + 6] * Retange[1, 2] +
                                   p[nWidth * (y + 1) + 4 * x - 2] * Retange[2, 0] + p[nWidth * (y + 1) + 4 * x + 2] * Retange[2, 1] + p[nWidth * (y + 1) + 4 * x + 6] * Retange[2, 2];

                            p[nWidth * (y) + 4 * x] = (byte)Math.Min(255, Math.Max(0, bSum / sum));
                            p[nWidth * (y) + 4 * x + 1] = (byte)Math.Min(255, Math.Max(0, gSum / sum));
                            p[nWidth * (y) + 4 * x + 2] = (byte)Math.Min(255, Math.Max(0, rSum / sum));


                        }
                    }
                }
                else if (Retange.Rank == 4)
                {
                    for (int y = 2; y < Height - 2; y++)
                    {
                        for (int x = 2; x < Width - 2; x++)
                        {
                            double rSum = 0;
                            double gSum = 0;
                            double bSum = 0;

                            bSum = p[nWidth * (y - 2) + 4 * x - 8] * Retange[0, 0] + p[nWidth * (y - 2) + 4 * x - 4] * Retange[0, 1] + p[nWidth * (y - 2) + 4 * x] * Retange[0, 2] + p[nWidth * (y - 2) + 4 * x + 4] * Retange[0, 3] + p[nWidth * (y - 2) + 4 * x + 8] * Retange[0, 4] +
                                   p[nWidth * (y - 1) + 4 * x - 8] * Retange[1, 0] + p[nWidth * (y - 1) + 4 * x - 4] * Retange[1, 1] + p[nWidth * (y - 1) + 4 * x] * Retange[1, 2] + p[nWidth * (y - 1) + 4 * x + 4] * Retange[1, 3] + p[nWidth * (y - 1) + 4 * x + 8] * Retange[1, 4] +
                                   p[nWidth * (y    ) + 4 * x - 8] * Retange[2, 0] + p[nWidth * (y    ) + 4 * x - 4] * Retange[2, 1] + p[nWidth * (y    ) + 4 * x] * Retange[2, 2] + p[nWidth * (y    ) + 4 * x + 4] * Retange[2, 3] + p[nWidth * (y    ) + 4 * x + 8] * Retange[2, 4] +
                                   p[nWidth * (y + 1) + 4 * x - 8] * Retange[3, 0] + p[nWidth * (y + 1) + 4 * x - 4] * Retange[3, 1] + p[nWidth * (y + 1) + 4 * x] * Retange[3, 2] + p[nWidth * (y + 1) + 4 * x + 4] * Retange[3, 3] + p[nWidth * (y + 1) + 4 * x + 8] * Retange[3, 4] +
                                   p[nWidth * (y + 2) + 4 * x - 8] * Retange[4, 0] + p[nWidth * (y + 2) + 4 * x - 4] * Retange[4, 1] + p[nWidth * (y + 2) + 4 * x] * Retange[4, 2] + p[nWidth * (y + 2) + 4 * x + 4] * Retange[4, 3] + p[nWidth * (y + 2) + 4 * x + 8] * Retange[4, 4];

                            gSum = p[nWidth * (y - 2) + 4 * x - 7] * Retange[0, 0] + p[nWidth * (y - 2) + 4 * x - 3] * Retange[0, 1] + p[nWidth * (y - 2) + 4 * x + 1] * Retange[0, 2] + p[nWidth * (y - 2) + 4 * x + 5] * Retange[0, 3] + p[nWidth * (y - 2) + 4 * x + 9] * Retange[0, 4] +
                                   p[nWidth * (y - 1) + 4 * x - 7] * Retange[1, 0] + p[nWidth * (y - 1) + 4 * x - 3] * Retange[1, 1] + p[nWidth * (y - 1) + 4 * x + 1] * Retange[1, 2] + p[nWidth * (y - 1) + 4 * x + 5] * Retange[1, 3] + p[nWidth * (y - 1) + 4 * x + 9] * Retange[1, 4] +
                                   p[nWidth * (y    ) + 4 * x - 7] * Retange[2, 0] + p[nWidth * (y    ) + 4 * x - 3] * Retange[2, 1] + p[nWidth * (y    ) + 4 * x + 1] * Retange[2, 2] + p[nWidth * (y    ) + 4 * x + 5] * Retange[2, 3] + p[nWidth * (y    ) + 4 * x + 9] * Retange[2, 4] +
                                   p[nWidth * (y + 1) + 4 * x - 7] * Retange[3, 0] + p[nWidth * (y + 1) + 4 * x - 3] * Retange[3, 1] + p[nWidth * (y + 1) + 4 * x + 1] * Retange[3, 2] + p[nWidth * (y + 1) + 4 * x + 5] * Retange[3, 3] + p[nWidth * (y + 1) + 4 * x + 9] * Retange[3, 4] +
                                   p[nWidth * (y + 2) + 4 * x - 7] * Retange[4, 0] + p[nWidth * (y + 2) + 4 * x - 3] * Retange[4, 1] + p[nWidth * (y + 2) + 4 * x + 1] * Retange[4, 2] + p[nWidth * (y + 2) + 4 * x + 5] * Retange[4, 3] + p[nWidth * (y + 2) + 4 * x + 9] * Retange[4, 4];

                            rSum = p[nWidth * (y - 2) + 4 * x - 6] * Retange[0, 0] + p[nWidth * (y - 2) + 4 * x - 2] * Retange[0, 1] + p[nWidth * (y - 2) + 4 * x + 2] * Retange[0, 2] + p[nWidth * (y - 2) + 4 * x + 6] * Retange[0, 3] + p[nWidth * (y - 2) + 4 * x + 10] * Retange[0, 4] +
                                   p[nWidth * (y - 1) + 4 * x - 6] * Retange[1, 0] + p[nWidth * (y - 1) + 4 * x - 2] * Retange[1, 1] + p[nWidth * (y - 1) + 4 * x + 2] * Retange[1, 2] + p[nWidth * (y - 1) + 4 * x + 6] * Retange[1, 3] + p[nWidth * (y - 1) + 4 * x + 10] * Retange[1, 4] +
                                   p[nWidth * (y    ) + 4 * x - 6] * Retange[2, 0] + p[nWidth * (y    ) + 4 * x - 2] * Retange[2, 1] + p[nWidth * (y    ) + 4 * x + 2] * Retange[2, 2] + p[nWidth * (y    ) + 4 * x + 6] * Retange[2, 3] + p[nWidth * (y    ) + 4 * x + 10] * Retange[2, 4] +
                                   p[nWidth * (y + 1) + 4 * x - 6] * Retange[3, 0] + p[nWidth * (y + 1) + 4 * x - 2] * Retange[3, 1] + p[nWidth * (y + 1) + 4 * x + 2] * Retange[3, 2] + p[nWidth * (y + 1) + 4 * x + 6] * Retange[3, 3] + p[nWidth * (y + 1) + 4 * x + 10] * Retange[3, 4] +
                                   p[nWidth * (y + 2) + 4 * x - 6] * Retange[4, 0] + p[nWidth * (y + 2) + 4 * x - 2] * Retange[4, 1] + p[nWidth * (y + 2) + 4 * x + 2] * Retange[4, 2] + p[nWidth * (y + 2) + 4 * x + 6] * Retange[4, 3] + p[nWidth * (y + 2) + 4 * x + 10] * Retange[4, 4];

                            p[nWidth * (y) + 4 * x] = (byte)Math.Min(255, Math.Max(0, bSum / sum));
                            p[nWidth * (y) + 4 * x + 1] = (byte)Math.Min(255, Math.Max(0, gSum / sum));
                            p[nWidth * (y) + 4 * x + 2] = (byte)Math.Min(255, Math.Max(0, rSum / sum));


                        }
                    }
                }
            }
            temp.UnlockBits(bmData);
            return temp;
        }
    }
}
