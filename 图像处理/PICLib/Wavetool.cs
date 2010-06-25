using System;
using System.Collections.Generic;
using System.Text;

namespace PICLib
{
    public class Wavelets
    {
        //private GrayImg PicMap;


        #region 启用单例模式定义图像处理类
        private static Wavelets objSelf = null;

        public Wavelets()
        {
        }
        public static Wavelets GetInstance()
        {
            if (objSelf == null)
            {
                objSelf = new Wavelets();
            }
            return objSelf;
        }
        #endregion

        /// <summary>
        /// 小波行变换
        /// </summary>
        public void TransRow(GrayImg PicMap)
        {
            int i, j, Wide, Height;
            Wide = PicMap.Width;
            Height = PicMap.Height;

            //分配临时空间
            byte[] temp1 = new byte[Wide * Height];

            int nWide = Wide / 2;
            for (j = 0; j < Height; j++)
            {
                for (i = 0; i < nWide; i++)
                {
                    int w = i * 2;
                    temp1[j * Wide + i] = PicMap.Img[j * Wide + w];
                    temp1[j * Wide + nWide + i] = PicMap.Img[j * Wide + w + 1];
                }
            }

            //通过图像差分，完成小波变换
            for (j = 0; j < Height; j++)
            {
                for (i = 0; i < nWide - 1; i++)
                {
                    temp1[j * Wide + nWide - 1 + i] = (byte)(temp1[j * Wide + (nWide - 1) + i] - temp1[j * Wide + i] + 128);
                }
            }

            //小波经过处理后，入入显示缓存中
            for (j = 0; j < Height; j++)
            {
                for (i = 0; i < Wide; i++)
                {
                    PicMap.Img[j * Wide + i] = temp1[j * Wide + i];
                }
            }
        }

        /// <summary>
        /// 小波列变换
        /// </summary>
        /// <param name="PicMap"></param>
        public void Transline(GrayImg PicMap)
        {
            int i, j, Wide, Height;
            Wide = PicMap.Width;
            Height = PicMap.Height;

            //分配临时空间
            byte[] temp = new byte[Wide * Height];

            int nHeight = Height / 2;
            for (i = 0; i < Wide; i++)
            {
                for (j = 0; j < nHeight - 1; j++)
                {
                    int h = j * 2;
                    temp[j * Wide + i] = PicMap.Img[h * Wide + i];
                    temp[(nHeight + j) * Wide + i] = PicMap.Img[(h + 1) * Wide + i];
                }
            }

            //通过图像差分，完成小波变换
            for (i = 0; i < Wide; i++)
            {
                for (j = 0; j < nHeight - 1; j++)
                {
                    temp[j * Wide + i] = (byte)(temp[(nHeight + j) * Wide + i] - temp[j * Wide + i] + 128);
                }
            }

            //小波经过处理后，入入显示缓存中
            for (j = 0; j < Height; j++)
            {
                for (i = 0; i < Wide; i++)
                {
                    PicMap.Img[j * Wide + i] = temp[j * Wide + i];
                }
            }
        }

        /// <summary>
        /// n层小波变换
        /// </summary>
        /// <param name="PicMap">传入图像</param>
        /// <param name="n">小波变换层数</param>
        public void WaveOnce(GrayImg PicMap, int n)
        {
            int i, j, Wide, Height, nWide, nHeight;
            for (int k = 1; k <= n; k++)
            {

                //获取传入图像大小
                Wide = PicMap.Width;
                Height = PicMap.Height;

                //临时数据空间
                byte[] temp1 = new byte[Wide * Height];
                byte[] temp2 = new byte[Wide * Height];

                nWide = (int)(Wide / Math.Pow(2, k));
                nHeight = (int)(Height / Math.Pow(2, k));

                //完成行变换
                for (j = (int)(Height * (1 - 1 / Math.Pow(2, (k - 1)))); j < Height; j++)
                {
                    for (i = 0; i < nWide; i++)
                    {
                        int w = i * 2;
                        temp1[j * Wide + i] = PicMap.Img[j * Wide + w]; //偶
                        temp1[j * Wide + nWide + i] = PicMap.Img[j * Wide + w + 1];  //奇
                    }
                }

                //通过图像的差分，完成小波变换
                for (j = (int)(Height * (1 - 1 / Math.Pow(2, (k - 1)))); j < Height; j++)
                {
                    for (i = 0; i < nWide - 1; i++)
                    {
                        temp1[j * Wide + nWide - 1 + i] = (byte)(temp1[j * Wide + nWide - 1 + i] - temp1[j * Wide + i] + 128);
                    }
                }

                //完成列变换
                for (i = 0; i < Wide / Math.Pow(2, k - 1); i++)
                {
                    for (j = (int)(Height * (1 - 1 / Math.Pow(2, (k - 1)))); j < Height * (1 - 1 / Math.Pow(2, (k - 1))) + nHeight; j++)
                    {
                        int m, h;
                        m = (int)(Height * (1 - 1 / Math.Pow(2, (k - 1))));
                        h = (j - m) * 2;
                        temp2[j * Wide + i] = temp1[(m + h) * Wide + i];  //奇数行
                        temp2[(nHeight + j) * Wide + i] = temp1[(m + h + 1) * Wide + i]; //偶数行
                    }
                }

                //通过图像养分，完成小波变换
                for (i = 0; i < Wide / Math.Pow(2, k - 1); i++)
                {
                    for (j = (int)(Height * (1 - 1 / Math.Pow(2, (k - 1)))); j < Height * (1 - 1 / Math.Pow(2, (k - 1))) + nHeight; j++)
                    {
                        temp2[j * Wide + i] = (byte)(temp2[j * Wide + i] - temp2[(nHeight + j) * Wide + i] + 128);
                    }
                }


                //小波经过处理后，放入显示缓存中
                for (j = (int)(Height * (1 - 1 / Math.Pow(2, (k - 1)))); j < Height; j++)
                {
                    for (i = 0; i < Wide / Math.Pow(2, k - 1); i++)
                    {
                        PicMap.Img[j * Wide + i] = temp2[j * Wide + i];
                    }
                }
            }
        }

        /// <summary>
        /// n层小波逆变换
        /// </summary>
        /// <param name="PicMap">传入图像</param>
        /// <param name="n">变换次数</param>
        public void IDWT(GrayImg PicMap, int n)
        {
            int i, j, Width, Height, nWide, nHeight;
            for (int k = n; k >= 1; k--)
            {

                //获取图像大小
                Width = PicMap.Width;
                Height = PicMap.Height;

                //分配临时存储空间
                byte[] temp1 = new byte[Width * Height];
                byte[] temp2 = new byte[Width * Height];
                byte[] temp3 = new byte[Width * Height];

                nWide = (int)(Width / Math.Pow(2, k));
                nHeight = (int)(Height / Math.Pow(2, k));

                //内存复制
                PicMap.Img.CopyTo(temp1, 0);
                //bool result = temp1.Equals(PicMap.Img);

                for (i = 0; i < Width / Math.Pow(2, k - 1); i++)
                {
                    for (j = (int)(Height * (1 - 1 / Math.Pow(2, (k - 1)))); j < Height * (1 - 1 / Math.Pow(2, (k - 1))) + nHeight; j++)
                    {
                        temp1[j * Width + i] = (byte)(temp1[(nHeight + j) * Width + i] + temp1[j * Width + i] - 128);
                    }
                }

                for (i = 0; i < Width / Math.Pow(2, k - 1); i++)
                {
                    for (j = (int)(Height * (1 - 1 / Math.Pow(2, (k - 1)))); j < Height * (1 - 1 / Math.Pow(2, (k - 1))) + nHeight; j++)
                    {
                        int m, h;
                        m = (int)(Height * (1 - 1 / Math.Pow(2, (k - 1))));
                        h = (j - m) * 2;
                        temp2[(m + h) * Width + i] = temp1[j * Width + i];
                        temp2[(m + h + 1) * Width + i] = temp1[(nHeight + j) * Width + i];
                    }
                }

                for (j = (int)(Height * (1 - 1 / Math.Pow(2, (k - 1)))); j < Height; j++)
                {
                    for (i = 0; i < nWide - 1; i++)
                    {
                        temp2[j * Width + nWide + i] += (byte)(temp2[j * Width + i] - 128);
                    }
                }

                for (j = (int)(Height * (1 - 1 / Math.Pow(2, (k - 1)))); j < Height; j++)
                {
                    for (i = 0; i < nWide; i++)
                    {
                        int w = i * 2;
                        temp3[j * Width + w] = temp2[j * Width + i];
                        temp3[j * Width + w + 1] = temp2[j * Width + nWide + i];
                    }
                }

                //逆变换后，放入显示缓存中
                for (j = (int)(Height * (1 - 1 / Math.Pow(2, (k - 1)))); j < Height; j++)
                {
                    for (i = 0; i < Width / Math.Pow(2, k - 1); i++)
                    {
                        PicMap.Img[j * Width + i] = (byte)(temp3[j * Width + i]);
                    }
                }

            }
        }

        /// <summary>
        /// 低通滤波
        /// </summary>
        /// <param name="PicMap">传入图像</param>
        /// <param name="n">滤波次数</param>
        public void LowFilter(GrayImg PicMap, int n)
        {
            int i, j, Width, Height, nWidth, nHeight;
            Width = PicMap.Width;
            Height = PicMap.Height;
            nWidth = Width / 2;
            nHeight = Height / 2;
            WaveOnce(PicMap, n);
            for (j = 0; j < Height; j++)
            {
                for (i = 0; i < Width; i++)
                {
                    if ((j >= Height * (1 - 1 / Math.Pow(2, (n - 1)))) && (i < Width / Math.Pow(2, n)))
                        continue;
                    else
                        PicMap.Img[j * Width + i] = (byte)128;
                }
            }
            IDWT(PicMap, n);
        }


        /// <summary>
        /// 高通滤波
        /// </summary>
        /// <param name="PicMap"></param>
        /// <param name="n">滤波次数</param>
        public void HighFilter(GrayImg PicMap, int n)
        {
            int i, j, Width, Height, nWidth, nHeight;
            Width = PicMap.Width;
            Height = PicMap.Height;
            nWidth = Width / 2;
            nHeight = Height / 2;
            int lLineBytes = (Width * 3 + 3) / 4 * 4;
            WaveOnce(PicMap, n);
            for (j = (int)(Height * (1 - 1 / Math.Pow(2, (n - 1)))); j < Height; j++)
            {
                for (i = 0; i < Width / Math.Pow(2, n); i++)
                {
                    PicMap.Img[j * Width + i] = (byte)128;
                }
            }
            IDWT(PicMap, n);
        }

        /// <summary>
        ///  拉普拉斯行
        /// </summary>
        public void LapLasLine(GrayImg PicMap)
        {
            int i, j, Wide, Height;
            Wide = PicMap.Width;
            Height = PicMap.Height;

            //分配临时空间
            byte[] temp1 = new byte[Wide * Height];
            for (j = 0; j < Height; j++)
            {
                for (i = 1; i < Wide - 1; i++)
                {
                    //int w = i * 2;
                    //temp1[j * Wide + i] = PicMap.Img[j * Wide + w];
                    temp1[j * Wide + i] = (byte)(Math.Abs(PicMap.Img[j * Wide + i] * 2 - PicMap.Img[j * Wide + i - 1] - PicMap.Img[j * Wide + i + 1]));
                }
            }
            for (i = 0; i < Wide * Height; i++)
                if (temp1[i] > 10)
                    PicMap.Img[i] = 255;
                else
                    PicMap.Img[i] = 0;
                //PicMap.Img[i] = temp1[i];
        }

        /// <summary>
        ///  拉普拉斯列
        /// </summary>
        public void LapLasRow(GrayImg PicMap)
        {
            int i, j, Wide, Height;
            Wide = PicMap.Width;
            Height = PicMap.Height;

            //分配临时空间
            byte[] temp1 = new byte[Wide * Height];
            for (j = 1; j < Height - 1; j++)
            {
                for (i = 1; i < Wide; i++)
                {
                    //int w = i * 2;
                    //temp1[j * Wide + i] = PicMap.Img[j * Wide + w];
                    temp1[j * Wide + i] = (byte)(Math.Abs(PicMap.Img[j * Wide + i] * 2 - PicMap.Img[(j - 1) * Wide + i] - PicMap.Img[(j + 1) * Wide + i]));
                }
            }
            for (i = 0; i < Wide * Height; i++)
                //PicMap.Img[i] = temp1[i];
                if (temp1[i] > 10)
                    PicMap.Img[i] = 255;
                else
                    PicMap.Img[i] = 0;
        }
    }
}
