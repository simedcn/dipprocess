using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace PICLib
{
    public class Process
    {
        
        #region 启用单例模式定义图像处理类
        private static Process objSelf = null;
        public static Process GetInstance()
        {
            if (objSelf == null)
            {
                objSelf = new Process();
            }
            return objSelf;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public Process()
        {
        }
        #endregion
        
        #region 灰度处理
        /// <summary>
        /// 灰度拉伸
        /// </summary>
        /// <param name="PicMap"></param>
        /// <param name="DestPic"></param>
        /// <param name="Min"></param>
        /// <param name="Max"></param>
        static public void ExtendPic(GrayImg PicMap, out GrayImg DestPic, byte Min, byte Max)
        {
            int Length = PicMap.Img.Length;

            //目标图像初始化
            DestPic.Img = new byte[Length];
            DestPic.Height = PicMap.Height;
            DestPic.Width = PicMap.Width;
            PicMap.Img.CopyTo(DestPic.Img, 0);

            //byte High, Low;
            //for (int i = 0; i < Length; i++)
            //{
            //    if (PicMap.Img[i] > High)
            //        High = PicMap.Img[i];
            //    if (PicMap.Img[i] < Low)
            //        Low = PicMap.Img[i];
            //}

            for (int i = 0; i < Length; i++)
            {
                if (PicMap.Img[i] > Max)
                    DestPic.Img[i] = 255;
                else if (PicMap.Img[i] < Min)
                    DestPic.Img[i] = 0;
                else
                    DestPic.Img[i] = (byte)((PicMap.Img[i] - Min) / ((Max - Min) / 255d));
            }

        }
        
        /// <summary>
        /// 获取灰度度范围
        /// </summary>
        /// <param name="fmax"></param>
        /// <param name="fmin"></param>
        /// <param name="inBase"></param>
        static public void Bright_Range(out byte fmax, out byte fmin, GrayImg inBase)
        {
            byte nf;
            fmax = 0;
            fmin = 255;
            int allPixel = inBase.Width * inBase.Height;
            for (int i = 0; i < allPixel; i++)
            {
                nf = inBase.Img[i];
                if (nf > fmax) fmax = nf;
                if (nf < fmin) fmin = nf;
            }
        }

        /// <summary>
        /// 灰度图均衡化
        /// </summary>
        /// <param name="pDestImg">输出均衡化后的灰度图</param>
        /// <param name="pSrcImg">输入待均衡的灰度图</param>
        static public int InteEqualize(out GrayImg pDestImg, GrayImg pSrcImg)
        {
            pDestImg = new GrayImg();
            int res = 0;
            int[] HistoGram = new int[256];
            int[] bMap = new int[256];

            int Temp, i, j;
            GrayImg TempImg;

            res = PicBase.ImgMalloc(out TempImg, pSrcImg.Width, pSrcImg.Height);
            if (res == 0)
                return 0;

            res = CalHistoGram(out HistoGram, pSrcImg);
            if (res == 0)
                return 0;

            for (i = 0; i < 256; i++)
            {
                Temp = 0;
                for (j = 0; j <= i; j++)
                    Temp = Temp + HistoGram[j];

                bMap[i] = (int)(Temp * 255.0f / (float)(pSrcImg.Height * pSrcImg.Width) + 0.5f);
            }

            for (i = 0; i < pSrcImg.Height; i++)
                for (j = 0; j < pSrcImg.Width; j++)
                {
                    //*(TempImg.pImg + i * TempImg.Width + j) = (BYTE)bMap[*(pSrcImg->pImg + i * pSrcImg->Width + j)];
                    TempImg.Img[i * TempImg.Width + j] = (byte)bMap[pSrcImg.Img[i * pSrcImg.Width + j]];
                }

            pDestImg.Width = TempImg.Width;
            pDestImg.Height = TempImg.Height;

            pDestImg.Img = TempImg.Img;

            return res;
        }

        /// <summary>
        /// 灰度图反色处理
        /// </summary>
        /// <param name="pDestImg">输出灰度图</param>
        static public int Invert(out GrayImg pDestImg, GrayImg pSrcImg)
        {
            int i, j;
            int res = 0;
            GrayImg TempImg;
            pDestImg = new GrayImg();

            res = PicBase.ImgMalloc(out TempImg, pSrcImg.Width, pSrcImg.Height);
            if (0 == res)
                return 0;

            for (i = 0; i < pSrcImg.Height; i++)
            {
                for (j = 0; j < pSrcImg.Width; j++)
                {
                    //*(TempImg.pImg + i * TempImg.Width + j) = 255 - *(pSrcImg->pImg + i * pSrcImg->Width + j);
                    TempImg.Img[i * TempImg.Width + j] = (byte)(255 - pSrcImg.Img[i * pSrcImg.Width + j]);
                }
            }
            pDestImg.Width = TempImg.Width;
            pDestImg.Height = TempImg.Height;
            pDestImg.Img = TempImg.Img;
            return 1;
        }
        #endregion

        #region 二值化

        /// <summary>
        /// 图像二值化——普通
        /// </summary>
        /// <param name="inBase"></param>
        /// <param name="outBase"></param>
        /// <param name="threshold"></param>
        static public void BinaryImg(PICLib.GrayImg inBase, out PICLib.GrayImg outBase, int threshold)
        {
            byte Max = 255, Min = 0;
            int Allpixel = inBase.Width * inBase.Height;
            PICLib.PicBase.ImgMalloc(out outBase, inBase.Width, inBase.Height);
            for (int i = 0; i < Allpixel; i++)
                outBase.Img[i] = inBase.Img[i] > threshold ? Max : Min;
        }
        
        /// <summary>
        /// 二值化最——大类间方差法
        /// </summary>
        /// <returns>返回计算的阈值</returns>
        static public int BinaryImg(PICLib.GrayImg inBase, out PICLib.GrayImg outBase)
        {
            int w = inBase.Width;
            int h = inBase.Height;

            int[] range = new int[256];
            for (int i = 0; i < 256; i++)
                range[i] = 0;

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                    if (j > 5 && j < h - 5 && i > 5 && i < w - 5)
                        range[inBase.Img[i + j * w]]++;

            }

            int a = w * h;
            int lowerQuart = 0;
            for (int i = 0; (double)i < (double)a * 0.01D; )
            {
                i += range[lowerQuart];
                lowerQuart++;
            }

            int thresh = (byte)(lowerQuart + 10);
            BinaryImg(inBase, out outBase, (byte)thresh);
            return thresh;
        }

        /// <summary>
        /// 图像二值化——大津法
        /// </summary>
        /// <param name="thresh">传出计算出的阈值</param>
        static public int BinaryImg(GrayImg inBase)
        {
            int thresh = 128;
            double mean1, mean2;

            int noudo1 = 0, noudo2 = 0;
            int th, level = 0;
            //求最高灰度级
            byte fmax = 255, fmin = 0;
            Bright_Range(out fmax, out fmin, inBase);

            int level1 = (int)fmin;
            level = (int)fmax;
            /*th循环开始，小于阈值th的像素归为第一类，大于th的像素归为第二类th=0,1,...,level*/
            int allPiexl = inBase.Width * inBase.Height;

            double bunsan = 0;
            thresh = 0;

            for (th = level1; th <= level; th++)
            {
                noudo1 = noudo2 = 0;
                int gasosu1 = 0, gasosu2 = 0;
                for (int i = 0; i < allPiexl; i++)
                {
                    if (inBase.Img[i] < th)
                    {
                        gasosu1++;
                        noudo1 += inBase.Img[i];
                    }
                    if (inBase.Img[i] >= th)
                    {
                        gasosu2++;
                        noudo2 += inBase.Img[i];
                    }
                }
                if (gasosu1 > 0)
                    mean1 = (double)((double)noudo1 / gasosu1);
                else
                    mean1 = 0;

                if (gasosu2 > 0)
                    mean2 = (double)((double)noudo2 / gasosu2);
                else
                    mean2 = 0;
                double bunsantmp = (double)gasosu1 * gasosu2 * (mean1 - mean2) * (mean1 - mean2);
                if (bunsantmp > bunsan)
                {
                    bunsan = bunsantmp;
                    thresh = th;
                }
            }
            return thresh;
        }

       

        #endregion
      
        #region 图像增强
        /// <summary>
        /// 图像双向增强
        /// </summary>
        /// <param name="PicMap"></param>
        /// <param name="Result"></param>
        static public void ImageEnhance(ColorImg PicMap, out ColorImg Result)
        {
            int Width, Height; //原图像相关数据

            //传入图像信息
            Width = PicMap.Width;
            Height = PicMap.Height;

            //返回图像大小
            Result.Width = Width;
            Result.Height = Height;

            //开辟返回图像数据区
            Result.B = new byte[Width * Height];
            Result.G = new byte[Width * Height];
            Result.R = new byte[Width * Height];


            //3x3模板
            double[,] h ={
                            {   -1d,    -1d,    -1d},
                            {   -1d,     8d,-   -1d},
                            {   -1d,    -1d,    -1d},
                          };

            for (int j = 1; j < Height - 1; j++)
            {
                for (int i = 1; i < Width - 1; i++)
                {
                    double[] pby_pt = { 0, 0, 0 };

                    pby_pt[0] = h[0, 0] * PicMap.R[(j - 1) * Width + i - 1] + h[0, 1] * PicMap.R[(j - 1) * Width + i] + h[0, 2] * PicMap.R[(j - 1) * Width + i + 1]
                              + h[1, 0] * PicMap.R[j * Width + i - 1] + h[1, 1] * PicMap.R[j * Width + i] + h[1, 2] * PicMap.R[j * Width + i + 1]
                              + h[2, 0] * PicMap.R[(j + 1) * Width + i - 1] + h[2, 1] * PicMap.R[(j + 1) * Width + i] + h[2, 2] * PicMap.R[(j + 1) * Width + i + 1];

                    pby_pt[1] = h[0, 0] * PicMap.G[(j - 1) * Width + i - 1] + h[0, 1] * PicMap.G[(j - 1) * Width + i] + h[0, 2] * PicMap.G[(j - 1) * Width + i + 1]
                              + h[1, 0] * PicMap.G[j * Width + i - 1] + h[1, 1] * PicMap.G[j * Width + i] + h[1, 2] * PicMap.G[j * Width + i + 1]
                              + h[2, 0] * PicMap.G[(j + 1) * Width + i - 1] + h[2, 1] * PicMap.G[(j + 1) * Width + i] + h[2, 2] * PicMap.G[(j + 1) * Width + i + 1];

                    pby_pt[2] = h[0, 0] * PicMap.B[(j - 1) * Width + i - 1] + h[0, 1] * PicMap.B[(j - 1) * Width + i] + h[0, 2] * PicMap.B[(j - 1) * Width + i + 1]
                              + h[1, 0] * PicMap.B[j * Width + i - 1] + h[1, 1] * PicMap.B[j * Width + i] + h[1, 2] * PicMap.B[j * Width + i + 1]
                              + h[2, 0] * PicMap.B[(j + 1) * Width + i - 1] + h[2, 1] * PicMap.B[(j + 1) * Width + i] + h[2, 2] * PicMap.B[(j + 1) * Width + i + 1];

                    pby_pt[0] /= 9.0d;
                    pby_pt[1] /= 9.0d;
                    pby_pt[2] /= 9.0d;

                    if (pby_pt[0] > 20)
                        Result.R[j * Width + i] = (byte)(Math.Abs(pby_pt[0]) + 100);
                    else
                        Result.R[j * Width + i] = (byte)(Math.Abs(pby_pt[0]));


                    if (pby_pt[1] > 20)
                        Result.G[j * Width + i] = (byte)(Math.Abs(pby_pt[1]) + 100);
                    else
                        Result.G[j * Width + i] = (byte)(Math.Abs(pby_pt[1]));


                    if (pby_pt[2] > 20)
                        Result.B[j * Width + i] = (byte)(Math.Abs(pby_pt[2]) + 100);
                    else
                        Result.B[j * Width + i] = (byte)(Math.Abs(pby_pt[2]));
                }
            }
            //p_temp.CopyTo(PicMap.Img, 0);
        }
        
        /// <summary>
        /// 图双向增强
        /// </summary>
        /// <param name="PicMap"></param>
        /// <param name="Result"></param>
        static public void ImageEnhance(GrayImg PicMap, out GrayImg Result)
        {
            int Width, Height; //原图像相关数据

            //传入图像信息
            Width = PicMap.Width;
            Height = PicMap.Height;

            //返回图像大小
            Result.Width = Width;
            Result.Height = Height;

            //开辟返回图像数据区
            Result.Img = new byte[Width * Height];

            ////3x3模板
            //double[,] h ={
            //                {   -1d,    -1d,    -1d},
            //                {   -1d,     8d,-   -1d},
            //                {   -1d,    -1d,    -1d},
            //              };

            //Sobel水平算子
            double[,] h ={
                            {   -1d,    0d,    1d},
                            {   -2d,    0d,    2d},
                            {   -1d,    0d,    1d},
                          };

            for (int j = 1; j < Height - 1; j++)
            {
                for (int i = 1; i < Width - 1; i++)
                {
                    double pby_pt;

                    pby_pt = h[0, 0] * PicMap.Img[(j - 1) * Width + i - 1] + h[0, 1] * PicMap.Img[(j - 1) * Width + i] + h[0, 2] * PicMap.Img[(j - 1) * Width + i + 1]
                           + h[1, 0] * PicMap.Img[j * Width + i - 1] + h[1, 1] * PicMap.Img[j * Width + i] + h[1, 2] * PicMap.Img[j * Width + i + 1]
                           + h[2, 0] * PicMap.Img[(j + 1) * Width + i - 1] + h[2, 1] * PicMap.Img[(j + 1) * Width + i] + h[2, 2] * PicMap.Img[(j + 1) * Width + i + 1];

                    pby_pt /= 9.0d;

                    if (pby_pt > 20)
                        Result.Img[j * Width + i] = (byte)(Math.Abs(pby_pt) + 100);
                    else
                        Result.Img[j * Width + i] = (byte)(Math.Abs(pby_pt));
                }
            }
        }

        /// <summary>
        /// 线性拉伸图像灰度
        /// </summary>
        /// <param name="pDestImg"></param>
        /// <param name="pSrcImg"></param>
        /// <param name="Min"></param>
        /// <param name="Max"></param>
        static public void LinerStretch(out GrayImg pDestImg, GrayImg pSrcImg, byte Min, byte Max)
        {
            PicBase.ImgMalloc(out pDestImg, pSrcImg.Width, pSrcImg.Height);
            int allPiexl = pSrcImg.Width * pSrcImg.Height;
            for (int i = 0; i < allPiexl; i++)
            {
                float Value = (float)255 * (float)(pSrcImg.Img[i] - Min) / (float)(Max - Min);
                if (Value > 255)
                    Value = 255;
                if (Value < 0)
                    Value = 0;
                pDestImg.Img[i] = (byte)Value;
            }
        }
       
        #endregion
         
        #region 图像缩放
        /// <summary>
        /// 缩放灰度图
        /// </summary>
        /// <param name="pDestImg"></param>
        /// <param name="pSrcImg"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <returns></returns>
        /// 
        static public int Zoomup(out GrayImg pDestImg, GrayImg pSrcImg, int newWidth, int newHeight)
        {
            PicBase.ImgMalloc(out pDestImg, newWidth, newHeight);
            if (pSrcImg.Img == null)
                return 0;

            pDestImg = new GrayImg();
            PicBase.ImgClone(out pDestImg, pSrcImg);

            if (pSrcImg.Width == newWidth && pSrcImg.Height == newHeight)
                return 1;
            double xscale = (double)newWidth / (double)pSrcImg.Width;
            double yscale = (double)newHeight / (double)pSrcImg.Height;

            pDestImg.Width = newWidth;
            pDestImg.Height = newHeight;
            int contLen = newWidth * newHeight;
            pDestImg.Img = new byte[contLen];
            /*插值存在黑色线条，插值用除法来做*/
            for (int y = 0; y < newHeight; y++)
            {
                int Fy = (int)((double)y / yscale);
                for (int x = 0; x < newWidth; x++)
                {
                    int Fx = (int)((double)x / xscale);
                    int Loc1 = Fy * pSrcImg.Width + Fx;
                    int Loc2 = y * newWidth + x;
                    pDestImg.Img[Loc2] = pSrcImg.Img[Loc1];
                }
            }
            return 1;
        }
       
        /// <summary>
        /// 缩放灰度图像(改进型）
        /// </summary>
        /// <param name="pDestImg"></param>
        /// <param name="pSrcImg"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <returns></returns>
        public int Zoomup1(out GrayImg pDestImg, GrayImg pSrcImg, int newWidth, int newHeight)
        {
            pDestImg = new GrayImg();
            PicBase.ImgClone(out pDestImg, pSrcImg);

            if (pSrcImg.Width == newWidth && pSrcImg.Height == newHeight)
                return 1;
            float xscale = (float)newWidth / (float)pSrcImg.Width;
            float yscale = (float)newHeight / (float)pSrcImg.Height;
            float xscale2 = 1 - xscale;
            float yscale2 = 1 - yscale;

            pDestImg.Width = newWidth;
            pDestImg.Height = newHeight;
            int contLen = newWidth * newHeight;
            pDestImg.Img = new byte[contLen];
            //pDestImg.G = new byte[contLen];
            //pDestImg.B = new byte[contLen];
            /*插值存在黑色线条，插值用除法来做*/
            for (int y = 0; y < newHeight; y++)
            {
                int Fy = (int)(y / yscale);
                for (int x = 0; x < newWidth; x++)
                {
                    int Fx = (int)(x / xscale);

                    //像素在原图像中位置
                    int Loc1 = Fy * pSrcImg.Width + Fx;
                    int Loc12 = (Fy + 1) * pSrcImg.Width + Fx;

                    //像素在目标图像中位置
                    int Loc2 = y * newWidth + x;
                    //pDestImg.R[Loc2] = (byte)((pSrcImg.R[Loc1] * xscale + pSrcImg.R[Loc1 + 1] * (1 - xscale2)) * yscale + (pSrcImg.R[Loc12] * xscale + pSrcImg.R[Loc12 + 1] * (1 - xscale2)) * yscale2);
                    //pDestImg.G[Loc2] = (byte)((pSrcImg.G[Loc1] * xscale + pSrcImg.G[Loc1 + 1] * (1 - xscale2)) * yscale + (pSrcImg.G[Loc12] * xscale + pSrcImg.G[Loc12 + 1] * (1 - xscale2)) * yscale2);
                    pDestImg.Img[Loc2] = (byte)((pSrcImg.Img[Loc1] * xscale + pSrcImg.Img[Loc1 + 1] * (1 - xscale2)) * yscale + (pSrcImg.Img[Loc12] * xscale + pSrcImg.Img[Loc12 + 1] * (1 - xscale2)) * yscale2);
                }
            }
            return 1;
        }
        
        /// <summary>
        /// 缩放彩色图像
        /// </summary>
        /// <param name="pDestImg"></param>
        /// <param name="pSrcImg"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <returns></returns>
        static public int Zoomup1(out ColorImg pDestImg, ColorImg pSrcImg, int newWidth, int newHeight)
        {
            pDestImg = new ColorImg();
            PicBase.ImgClone(out pDestImg, pSrcImg);

            if (pSrcImg.Width == newWidth && pSrcImg.Height == newHeight)
                return 1;
            float xscale = (float)newWidth / (float)pSrcImg.Width;
            float yscale = (float)newHeight / (float)pSrcImg.Height;

            pDestImg.Width = newWidth;
            pDestImg.Height = newHeight;
            int contLen = newWidth * newHeight;
            pDestImg.R = new byte[contLen];
            pDestImg.G = new byte[contLen];
            pDestImg.B = new byte[contLen];
            /*插值存在黑色线条，插值用除法来做*/
            for (int y = 0; y < newHeight; y++)
            {
                int Fy = (int)(y / yscale);
                for (int x = 0; x < newWidth; x++)
                {
                    int Fx = (int)(x / xscale);
                    int Loc1 = Fy * pSrcImg.Width + Fx;
                    int Loc2 = y * newWidth + x;
                    pDestImg.R[Loc2] = pSrcImg.R[Loc1];
                    pDestImg.G[Loc2] = pSrcImg.G[Loc1];
                    pDestImg.B[Loc2] = pSrcImg.B[Loc1];
                }
            }
            return 1;
        }

        /// <summary>
        /// 缩放彩色图像(改进型）
        /// </summary>
        /// <param name="pDestImg"></param>
        /// <param name="pSrcImg"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <returns></returns>
        static public int Zoomup(out ColorImg pDestImg, ColorImg pSrcImg, int newWidth, int newHeight)
        {
            pDestImg = new ColorImg();
            PicBase.ImgClone(out pDestImg, pSrcImg);

            if (pSrcImg.Width == newWidth && pSrcImg.Height == newHeight)
                return 1;
            float xscale = (float)newWidth / (float)pSrcImg.Width;
            float yscale = (float)newHeight / (float)pSrcImg.Height;
            float xscale2 = 1 - xscale;
            float yscale2 = 1 - yscale;

            pDestImg.Width = newWidth;
            pDestImg.Height = newHeight;
            int contLen = newWidth * newHeight;
            pDestImg.R = new byte[contLen];
            pDestImg.G = new byte[contLen];
            pDestImg.B = new byte[contLen];
            /*插值存在黑色线条，插值用除法来做*/
            for (int y = 0; y < newHeight; y++)
            {
                int Fy = (int)(y / yscale);
                for (int x = 0; x < newWidth; x++)
                {
                    int Fx = (int)(x / xscale);

                    //像素在原图像中位置
                    int Loc1 = Fy * pSrcImg.Width + Fx;
                    int Loc12 = (Fy + 1) * pSrcImg.Width + Fx;

                    //像素在目标图像中位置
                    int Loc2 = y * newWidth + x;
                    pDestImg.R[Loc2] = (byte)((pSrcImg.R[Loc1] * xscale + pSrcImg.R[Loc1 + 1] * (1 - xscale2)) * yscale + (pSrcImg.R[Loc12] * xscale + pSrcImg.R[Loc12 + 1] * (1 - xscale2)) * yscale2);
                    pDestImg.G[Loc2] = (byte)((pSrcImg.G[Loc1] * xscale + pSrcImg.G[Loc1 + 1] * (1 - xscale2)) * yscale + (pSrcImg.G[Loc12] * xscale + pSrcImg.G[Loc12 + 1] * (1 - xscale2)) * yscale2);
                    pDestImg.B[Loc2] = (byte)((pSrcImg.B[Loc1] * xscale + pSrcImg.B[Loc1 + 1] * (1 - xscale2)) * yscale + (pSrcImg.B[Loc12] * xscale + pSrcImg.B[Loc12 + 1] * (1 - xscale2)) * yscale2);
                }
            }
            return 1;
        }
        #endregion

        #region 图像裁切相关
        /// <summary>
        /// 裁切图像函数
        /// </summary>
        /// <param name="pDestImg"></param>
        /// <param name="pSrcImg"></param>
        /// <param name="startX"></param>
        /// <param name="endX"></param>
        /// <param name="startY"></param>
        /// <param name="endY"></param>
        /// <returns></returns>
        static public void CutImg(out GrayImg pDestImg, GrayImg pSrcImg, int startX, int endX, int startY, int endY)
        {
            pDestImg = new GrayImg();
            if (startX > endX)
            {
                int t = startX;
                startX = endX;
                endX = t;
            }
            if (startY > endY)
            {
                int t = startY;
                startY = endY;
                endY = t;
            }

            int _Xs = startX;                 //起始x位置
            int _Xe = endX;                   //终止x位置
            int _Width = _Xe - _Xs + 1;                //图像的实际长度

            int _Ys = startY;
            int _Ye = endY;
            int _Height = _Ye - _Ys + 1;                //图像的高度

            pDestImg.Img = new byte[_Width * _Height];

            int i = 0;
            for (int row = _Ys; row <= _Ye; row++)
            {
                for (int col = _Xs; col <= _Xe; col++)
                {
                    pDestImg.Img[i] = pSrcImg.Img[row * pSrcImg.Width + col];
                    i++;
                }
            }
            pDestImg.Height = _Height;
            pDestImg.Width = _Width;
        }

        /// <summary>
        /// 搜索裁切图像
        /// </summary>
        static public void CutScan(ref GrayImg pDestImg)
        {
            int startY = 0;
            /*从头到底进行扫描*/
            for (int y = 0; y < pDestImg.Height; y++)
            {
                for (int x = 0; x < pDestImg.Width; x++)
                {
                    if (pDestImg.Img[y * pDestImg.Width + x] == 0)
                    {
                        startY = y;
                        break;
                    }
                }
            }
            int startX = 0;
            /*从左到右扫描*/
            for (int x = 0; x < pDestImg.Width; x++)
            {
                for (int y = 0; y < pDestImg.Height; y++)
                {
                    if (pDestImg.Img[y * pDestImg.Width + x] == 0)
                    {
                        startX = x;
                        break;
                    }
                }
            }
            int endY = pDestImg.Height - 1;
            /*从底到头进行扫描*/
            for (int y = pDestImg.Height - 1; y > 0; y--)
            {
                for (int x = 0; x < pDestImg.Width; x++)
                {
                    if (pDestImg.Img[y * pDestImg.Width + x] == 0)
                    {
                        endY = y;
                        break;
                    }
                }
            }
            int endX = pDestImg.Width - 1;
            /*从右到左进行扫描*/
            for (int x = pDestImg.Width - 1; x > 0; x--)
            {
                for (int y = 0; y < pDestImg.Height; y++)
                {
                    if (pDestImg.Img[y * pDestImg.Width + x] == 0)
                    {
                        endX = x;
                        break;
                    }
                }
            }
            GrayImg pSrcImg;
            PicBase.ImgClone(out pSrcImg, pDestImg);
            CutImg(out pDestImg, pSrcImg, startX, endX, startY, endY);
        }
        #endregion

        #region 高斯平滑函数
        
        /// <summary>
        /// 高斯平滑
        /// </summary>
        /// <param name="inBase"></param>
        /// <param name="outBase"></param>
        /// <param name="size"></param>
        /// <param name="variance"></param>
        static public void GaussianSmooth(PICLib.GrayImg inBase, out PICLib.GrayImg outBase, int size, double variance)
        {
            float[] mask = new float[size * size];
            int k = 0;
            float sum = 0.0F;
            int so2 = (size + 1) / 2;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    k = i + j * size;
                    float xsqr = (i - so2) * (i - so2);
                    float ysqr = (j - so2) * (j - so2);
                    float vsqr = (float)(variance * variance);
                    mask[k] = (float)Math.Exp(-(xsqr + ysqr) / (2.0F * vsqr));
                    sum += mask[k];
                }
            }

            for (int i = 0; i < size * size; i++)
                mask[i] = mask[i] / sum;

            int width = inBase.Width;
            int height = inBase.Height;
            PICLib.PicBase.ImgMalloc(out outBase, width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    sum = 0.0F;
                    if (i - size / 2 < 0 || j - size / 2 < 0 || i + size / 2 >= width || j + size / 2 >= height)
                    {
                        outBase.Img[i + j * width] = 0;
                    }
                    else
                    {
                        int m = 0;
                        for (k = 0 - size / 2; k <= size / 2; k++)
                        {
                            for (int l = 0 - size / 2; l <= size / 2; l++)
                                sum += (float)inBase.Img[i + k + (j + l) * width] * mask[m++];
                        }

                        outBase.Img[i + j * width] = (byte)sum;
                    }
                }
            }
        }

        /// <summary>
        /// 采用高斯滤波对图象进行滤波，滤波先对x方向进行，然后对y方向进行
        /// </summary>
        /// <param name="PicMap">传入的待处理图像</param>
        /// <param name="sigma">高斯函数的标差</param>
        /// <param name="Result">处理后图像</param>
        /// 
        static public void GaussianSmooth(GrayImg PicMap, double sigma, out GrayImg Result)
        {
            // 循环控制变量
            int x, y, i, nWidth, nHeight;

            nWidth = PicMap.Width;
            nHeight = PicMap.Height;

            Result.Img = new byte[nWidth * nHeight];
            Result.Height = nHeight;
            Result.Width = nWidth;

            // 高斯滤波器的数组长度        	
            int nWindowSize;

            //  窗口长度的1/2
            int nHalfLen;

            // 一维高斯数据滤波器
            double[] pdKernel;

            // 高斯系数与图象数据的点乘
            double dDotMul;

            // 高斯滤波系数的总和
            double dWeightSum;

            // 中间变量
            double[] pdTmp;

            // 分配内存
            pdTmp = new double[nWidth * nHeight];

            // 产生一维高斯数据滤波器
            // MakeGauss(sigma, &dKernel, &nWindowSize);
            MakeGauss(sigma, out pdKernel, out nWindowSize);

            // MakeGauss返回窗口的长度，利用此变量计算窗口的半长
            nHalfLen = nWindowSize / 2;

            // x方向进行滤波
            for (y = 0; y < nHeight; y++)
            {
                for (x = 0; x < nWidth; x++)
                {
                    dDotMul = 0;
                    dWeightSum = 0;
                    for (i = (-nHalfLen); i <= nHalfLen; i++)
                    {
                        // 判断是否在图象内部
                        if ((i + x) >= 0 && (i + x) < nWidth)
                        {
                            dDotMul += (double)PicMap.Img[y * nWidth + (i + x)] * pdKernel[nHalfLen + i];
                            dWeightSum += pdKernel[nHalfLen + i];
                        }
                    }
                    pdTmp[y * nWidth + x] = dDotMul / dWeightSum;
                }
            }

            // y方向进行滤波
            for (x = 0; x < nWidth; x++)
            {
                for (y = 0; y < nHeight; y++)
                {
                    dDotMul = 0;
                    dWeightSum = 0;
                    for (i = (-nHalfLen); i <= nHalfLen; i++)
                    {
                        // 判断是否在图象内部
                        if ((i + y) >= 0 && (i + y) < nHeight)
                        {
                            dDotMul += (double)pdTmp[(y + i) * nWidth + x] * pdKernel[nHalfLen + i];
                            dWeightSum += pdKernel[nHalfLen + i];
                        }
                    }
                    Result.Img[y * nWidth + x] = (byte)(dDotMul / dWeightSum);
                }
            }
        }

        /// <summary>
        /// 产生一维高斯数据滤波器,生成一个一维的高斯函数的数字数据，理论上高斯数据的长度应该是无限长的，但是为了计算的简单和速度，实际的高斯数据只能是有限长的pnWindowSize就是数据长度
        /// </summary>
        /// <param name="sigma">高斯函数的标准差</param>
        /// <param name="pdKernel">指向高斯数据数组的引用（指针）</param>
        /// <param name="pnWindowSize">数据的长度</param>
        /// 
        static private void MakeGauss(double sigma, out double[] pdKernel, out int pnWindowSize)
        {
            // 循环控制变量
            int i;

            // 数组的中心点
            int nCenter;

            // 数组的某一点到中心点的距离
            double dDis;

            double PI = 3.14159265;

            // 中间变量
            double dValue;
            double dSum;
            dSum = 0;

            // 数组长度，根据概率论的知识，选取[-3*sigma, 3*sigma]以内的数据。
            // 这些数据会覆盖绝大部分的滤波系数
            pnWindowSize = (int)(1 + 2 * Math.Ceiling(3 * sigma));

            // 中心
            nCenter = (pnWindowSize) / 2;

            // 分配内存
            pdKernel = new double[pnWindowSize];

            for (i = 0; i < (pnWindowSize); i++)
            {
                dDis = (double)(i - nCenter);
                dValue = Math.Exp(-(1 / 2) * dDis * dDis / (sigma * sigma)) / (Math.Sqrt(2 * PI) * sigma);
                pdKernel[i] = dValue;
                dSum += dValue;
            }

            // 归一化
            for (i = 0; i < (pnWindowSize); i++)
            {
                pdKernel[i] /= dSum;
            }
        }

        #endregion

        #region 二值图像形态学运算

        #region 开运算
        /// <summary>
        /// 开运算
        /// </summary>
        /// <param name="inBase"></param>
        /// <param name="outBase"></param>
        /// <param name="Template"></param>
        /// <param name="TempSize"></param>
        public void Opening(PICLib.GrayImg inBase, out PICLib.GrayImg outBase, int[] Template, int TempSize)
        {
            Erosion(inBase, out outBase, Template, TempSize);
            Dilation(outBase, out outBase, Template, TempSize);
        }
        #endregion

        #region 闭运算
        /// <summary>
        /// 闭运算
        /// </summary>
        /// <param name="inBase"></param>
        /// <param name="outBase"></param>
        /// <param name="Template"></param>
        /// <param name="TempSize"></param>
        static public void Closing(PICLib.GrayImg inBase, out PICLib.GrayImg outBase, int[] Template, int TempSize)
        {
            Dilation(inBase, out outBase, Template, TempSize);
            Erosion(outBase, out outBase, Template, TempSize);
        }

        /// <summary>
        /// 闭运算——八临法
        /// </summary>
        static public void Closing_8(out GrayImg List0, GrayImg pSrcImg)
        {
            Process.Dilation_8(pSrcImg, out List0);
            Process.Erosion_8(List0, out List0);
        }
        #endregion

        #region 腐蚀运算
        /// <summary>
        /// 腐蚀运算，针对目标点为黑色点进行操作
        /// </summary>
        /// <param name="inBase"></param>
        /// <param name="outBase"></param>
        /// <param name="Template"></param>
        /// <param name="TempSize"></param>
        static public void Erosion(PICLib.GrayImg inBase, out GrayImg outBase, int[] Template, int TempSize)
        {
            int w = inBase.Width;
            int h = inBase.Height;

            PicBase.ImgMalloc(out outBase, w, h);
            //ImgClone(out outBase, inBase);

            int half = (int)Math.Floor((double)TempSize / 2D);

            for (int ww = half; ww < w - half; ww++)
            {
                for (int wh = half; wh < h - half; wh++)
                {
                    int sw = 0;
                    outBase.Img[ww + wh * w] = 255;
                    for (int ww2 = ww - half; ww2 <= ww + half; ww2++)
                    {
                        int sh = 0;
                        for (int wh2 = wh - half; wh2 <= wh + half; wh2++)
                        {
                            if (!(Template[sw + sh * TempSize] == 1 && inBase.Img[ww2 + wh2 * w] == 255))
                            {
                                outBase.Img[ww + wh * w] = 0;
                                ww2 = ww + half + 1;
                                wh2 = wh + half + 1;
                            }
                            sh++;
                        }
                        sw++;
                    }
                }
            }
        }

        /// <summary>
        /// 图像的八临法腐蚀算法
        /// </summary>
        static public void Erosion_8(GrayImg EMatrix_In, out GrayImg EMatrix_Out)
        {
            //EMatrix_Out = new int[_pW, _pH];
            //EMatrix_Out = (int[,])EMatrix_In.Clone();               //将EMatrix_In的值传
            //ImgMalloc(out EMatrix_Out, EMatrix_In.Width, EMatrix_In.Height);
            PicBase.ImgClone(out EMatrix_Out, EMatrix_In);
            int _pH = EMatrix_In.Height;
            int _pW = EMatrix_In.Width;

            /*先克隆图像*/
            for (int y1 = 0; y1 < _pH; ++y1)
            {
                for (int x1 = 0; x1 < _pW; ++x1)
                {
                    byte[] s = new byte[] { 128, 128, 128, 128, 128, 128, 128, 128, 128 };
                    /*最左上角*/
                    if (x1 - 1 >= 0 && y1 - 1 >= 0)
                    {
                        s[0] = EMatrix_In.Img[(y1 - 1) * _pW + (x1 - 1)];
                    }
                    /*中上*/
                    if (y1 - 1 >= 0)
                    {
                        //s[1] = EMatrix_In[x1, y1 - 1];
                        s[1] = EMatrix_In.Img[(y1 - 1) * _pW + x1];
                    }
                    /*右上*/
                    if (x1 + 1 < _pW && y1 - 1 >= 0)
                    {
                        //s[2] = EMatrix_In[x1 + 1, y1 - 1];
                        s[2] = EMatrix_In.Img[(y1 - 1) * _pW + x1 + 1];
                    }
                    /*左边*/
                    if (x1 - 1 >= 0)
                    {
                        //s[3] = EMatrix_In[x1 - 1, y1];
                        s[3] = EMatrix_In.Img[y1 * _pW + x1 - 1];
                    }
                    /*右边*/
                    if (x1 + 1 < _pW)
                    {
                        //s[4] = EMatrix_In[x1 + 1, y1];
                        s[4] = EMatrix_In.Img[y1 * _pW + x1 + 1];
                    }
                    /*左下*/
                    if (x1 - 1 >= 0 && y1 + 1 < _pH)
                    {
                        //s[5] = EMatrix_In[x1 - 1, y1 + 1];
                        s[5] = EMatrix_In.Img[(y1 + 1) * _pW + x1 - 1];
                    }
                    /*中下*/
                    if (y1 + 1 < _pH)
                    {
                        //s[6] = EMatrix_In[x1, y1 + 1];
                        s[6] = EMatrix_In.Img[(y1 + 1) * _pW + x1];
                    }
                    /*右下*/
                    if (x1 + 1 < _pW && y1 + 1 < _pH)
                    {
                        //s[7] = EMatrix_In[x1 + 1, y1 + 1];
                        s[7] = EMatrix_In.Img[(y1 + 1) * _pW + x1 + 1];
                    }
                    int i = 0;
                    /*腐蚀黑色区域*/
                    for (i = 0; i < 8; i++)
                    {
                        if (s[i] == 128)
                            continue;
                        /*如果存在一个白色点则退出循环*/
                        if (s[i] == 255)
                        {
                            break;
                        }
                    }
                    //若邻域内无象素，表示当前点为内点，将象素置黑
                    if (i != 8)
                        EMatrix_Out.Img[y1 * _pW + x1] = 255;
                    else
                        EMatrix_Out.Img[y1 * _pW + x1] = 0;
                }
            }
        }
        #endregion

        #region 膨胀运算
        /// <summary>
        /// 膨胀运算，针对黑色目标点
        /// </summary>
        /// <param name="inBase"></param>
        /// <param name="outBase"></param>
        /// <param name="Template"></param>
        /// <param name="TempSize"></param>
        static public void Dilation(GrayImg inBase, out GrayImg outBase, int[] Template, int TempSize)
        {
            int w = inBase.Width;
            int h = inBase.Height;

            //ImgClone(out outBase, inBase);
            PicBase.ImgMalloc(out outBase, w, h);

            int half = (int)Math.Floor((double)TempSize / 2D);

            for (int ww = half; ww < w - half; ww++)
            {
                for (int wh = half; wh < h - half; wh++)
                {
                    int sw = 0;
                    outBase.Img[ww + wh * w] = 0;
                    for (int ww2 = ww - half; ww2 <= ww + half; ww2++)
                    {
                        int sh = 0;
                        for (int wh2 = wh - half; wh2 <= wh + half; wh2++)
                        {
                            if (Template[sw + sh * TempSize] == 1 && inBase.Img[ww2 + wh2 * w] == 255)
                            {
                                outBase.Img[ww + wh * w] = 255;
                                ww2 = ww + half + 1;
                                wh2 = wh + half + 1;
                            }
                            sh++;
                        }
                        sw++;
                    }
                }
            }
        }


        /// <summary>
        /// 八临法膨胀处理
        /// </summary>
        static public void Dilation_8(GrayImg EMatrix_In, out GrayImg EMatrix_Out)
        {
            //ImgMalloc(out EMatrix_Out, EMatrix_In.Width, EMatrix_In.Height);
            PicBase.ImgClone(out EMatrix_Out, EMatrix_In);
            int _pH = EMatrix_In.Height;
            int _pW = EMatrix_In.Width;

            for (int y1 = 0; y1 < _pH; ++y1)
            {
                for (int x1 = 0; x1 < _pW; ++x1)
                {
                    byte[] s = new byte[] { 128, 128, 128, 128, 128, 128, 128, 128, 128 };
                    /*最左上角*/
                    if (x1 - 1 >= 0 && y1 - 1 >= 0)
                    {
                        //s[0] = EMatrix_In[x1 - 1, y1 - 1];
                        s[0] = EMatrix_In.Img[(y1 - 1) * _pW + x1 - 1];
                    }
                    /*中上*/
                    if (y1 - 1 >= 0)
                    {
                        //s[1] = EMatrix_In[x1, y1 - 1];
                        s[1] = EMatrix_In.Img[(y1 - 1) * _pW + x1];
                    }
                    /*右上*/
                    if (x1 + 1 < _pW && y1 - 1 >= 0)
                    {
                        //s[2] = EMatrix_In[x1 + 1, y1 - 1];
                        s[2] = EMatrix_In.Img[(y1 - 1) * _pW + x1 + 1];
                    }
                    /*左边*/
                    if (x1 - 1 >= 0)
                    {
                        //s[3] = EMatrix_In[x1 - 1, y1];
                        s[3] = EMatrix_In.Img[y1 * _pW + x1 - 1];
                    }
                    /*中间*/
                    //s[4] = EMatrix_In[x1, y1];
                    s[4] = EMatrix_In.Img[y1 * _pW + x1];
                    /*右边*/
                    if (x1 + 1 < _pW)
                    {
                        //s[5] = EMatrix_In[x1 + 1, y1];
                        s[5] = EMatrix_In.Img[y1 * _pW + x1 + 1];
                    }
                    /*左下*/
                    if (x1 - 1 >= 0 && y1 + 1 < _pH)
                    {
                        //s[6] = EMatrix_In[x1 - 1, y1 + 1];
                        s[6] = EMatrix_In.Img[(y1 + 1) * _pW + x1 - 1];
                    }
                    /*中下*/
                    if (y1 + 1 < _pH)
                    {
                        //s[7] = EMatrix_In[x1, y1 + 1];
                        s[7] = EMatrix_In.Img[(y1 + 1) * _pW + x1];
                    }
                    /*右下*/
                    if (x1 + 1 < _pW && y1 + 1 < _pH)
                    {
                        //s[8] = EMatrix_In[x1 + 1, y1 + 1];
                        s[8] = EMatrix_In.Img[(y1 + 1) * _pW + x1 + 1];
                    }
                    /*腐蚀黑色区域*/
                    for (int i = 0; i < 9; i++)
                    {
                        /*如果存在一个黑色点则退出循环*/
                        if (s[i] == 0)
                        {
                            EMatrix_Out.Img[y1 * _pW + x1] = 0;
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region 获取联通区域
        /// <summary>
        /// 获取联通区域
        /// </summary>
        /// <param name="inBase"></param>
        /// <param name="regions"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        private void grow(ref GrayImg inBase, ref ConnectedRegions regions, int x, int y, int w, int h)
        {
            Stack<int> indicies = new Stack<int>();
            indicies.Push(x + y * w);
            int minX = x;
            int maxX = x;
            int minY = y;
            int maxY = y;
            while (indicies.Count > 0)
            {
                int index = indicies.Pop();
                y = (int)Math.Floor((float)index / (float)w);
                x = index - y * w;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                        if (x + i >= 0 && x + i < w && y + j >= 0 && y + j < h)
                        {
                            int newIndex = x + i + (y + j) * w;
                            if (inBase.Img[newIndex] == 255)
                            {
                                indicies.Push(newIndex);
                                inBase.Img[newIndex] = 0;
                                if (x + i > maxX)
                                    maxX = x + i;
                                if (x + i < minX)
                                    minX = x + i;
                                if (y + i > maxY)
                                    maxY = y + i;
                                if (y + i < minY)
                                    minY = y + i;
                            }
                        }

                }
            }
            Rectangle r = new Rectangle();
            r.X = minX;
            r.Y = minY;
            r.Width = maxX - minX;
            r.Height = maxY - minY;
            if (r.Width * r.Height >= regions.maxRect.Width * regions.maxRect.Height)
                regions.maxRect = r;
            if (r.Width * r.Height <= regions.minRect.Width * regions.minRect.Height)
                regions.minRect = r;
            int midW = (r.Width / 100) * 40;
            int midH = (r.Height / 100) * 40;
            for (int i = minX + midW; i <= maxX - midW; i++)
            {
                for (int j = minY + midW; j <= maxY - midW; j++)
                    regions.connected[i + j * w] = 255;
            }
        }

        public ConnectedRegions getConnectedRegions(GrayImg inBase, int w, int h)
        {
            ConnectedRegions regions = new ConnectedRegions();
            regions.connected = new byte[w * h];
            regions.maxRect = new Rectangle(0, 0, 0, 0);
            regions.minRect = new Rectangle(0, 0, w, h);
            Rectangle c = new Rectangle();
            c.Width = c.Height = c.X = c.Y = 0;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                    if (inBase.Img[x + y * w] == 255)
                        grow(ref inBase, ref regions, x, y, w, h);
            }
            return regions;
        }
        #endregion
        #endregion

        #region Canny算子相关

        /// <summary>
        /// Canny算子（边缘检测）
        /// </summary>
        /// <param name="PicMap">传入图像</param>
        /// <param name="sigma">高斯滤波的标准方差</param>
        /// <param name="dRatioLow">低阈值和高阈值之间的比例</param>
        /// <param name="dRatioHigh">高阈值占图象象素总数的比例</param>
        /// <param name="Result"></param>
        static public void Canny(GrayImg PicMap, double sigma, double dRatioLow, double dRatioHigh, out GrayImg Result)
        {
            //对Result进行初始化
            Result.Height = PicMap.Height;
            Result.Width = PicMap.Width;
            Result.Img = new byte[Result.Height * Result.Width];

            // 经过高斯滤波后的图象数据
            //byte[]  pUnchSmooth ;

            // 指向x方向导数的指针
            int[] pnGradX;

            // 指向y方向导数的指针
            int[] pnGradY;

            // 梯度的幅度
            int[] pnGradMag;

            //pUnchSmooth = new byte[PicMap.Width * PicMap.Height];
            pnGradX = new int[PicMap.Width * PicMap.Height];
            pnGradY = new int[PicMap.Width * PicMap.Height];
            pnGradMag = new int[PicMap.Width * PicMap.Height];

            // 对原图象进行滤波
            GaussianSmooth(PicMap, sigma, out PicMap);

            // 计算方向导数
            DirGrad(PicMap, ref pnGradX, ref pnGradY);

            // 计算梯度的幅度
            GradMagnitude(pnGradX, pnGradY, PicMap.Width, PicMap.Height, ref pnGradMag);

            // 应用non-maximum 抑制
            NonmaxSuppress(pnGradMag, pnGradX, pnGradY, PicMap.Width, PicMap.Height, ref Result.Img);

            // 应用Hysteresis，找到所有的边界
            Hysteresis(pnGradMag, PicMap.Width, PicMap.Height, dRatioLow, dRatioHigh, ref Result.Img);

        }


        /// <summary>
        /// 计算方向倒数，采用的微分算子是(-1 0 1) 和 (-1 0 1)'(转置),计算的时候对边界象素采用了特殊处理
        /// </summary>
        /// <param name="pUnchSmthdImg">经过高斯滤波后的图象</param>
        /// <param name="pnGradX">X方向的方向导数</param>
        /// <param name="pnGradY">Y方向的方向导数</param>
        /// 
        static private void DirGrad(GrayImg pUnchSmthdImg, ref int[] pnGradX, ref int[] pnGradY)
        {
            // 循环控制变量
            int y;
            int x;
            int nHeight = pUnchSmthdImg.Height;
            int nWidth = pUnchSmthdImg.Width;

            // 计算x方向的方向导数，在边界出进行了处理，防止要访问的象素出界
            for (y = 0; y < nHeight; y++)
            {
                for (x = 0; x < nWidth; x++)
                {
                    pnGradX[y * nWidth + x] = (int)(pUnchSmthdImg.Img[y * nWidth + Math.Min(nWidth - 1, x + 1)] - pUnchSmthdImg.Img[y * nWidth + Math.Max(0, x - 1)]);
                }
            }

            // 计算y方向的方向导数，在边界出进行了处理，防止要访问的象素出界
            for (x = 0; x < nWidth; x++)
            {
                for (y = 0; y < nHeight; y++)
                {
                    pnGradY[y * nWidth + x] = (int)(pUnchSmthdImg.Img[Math.Min(nHeight - 1, y + 1) * nWidth + x] - pUnchSmthdImg.Img[Math.Max(0, y - 1) * nWidth + x]);
                }
            }
        }

        /// <summary>
        /// 利用方向倒数计算梯度幅度，方向倒数是DirGrad函数计算的结果
        /// </summary>
        /// <param name="pnGradX">X方向的方向导数</param>
        /// <param name="pnGradY">Y方向的方向导数</param>
        /// <param name="nWidth">图像宽度</param>
        /// <param name="nHeight">图像高度</param>
        /// <param name="pnMag">梯度幅度</param>
        static private void GradMagnitude(int[] pnGradX, int[] pnGradY, int nWidth, int nHeight, ref int[] pnMag)
        {

            // 循环控制变量
            int y;
            int x;

            // 中间变量
            double dSqtOne;
            double dSqtTwo;

            for (y = 0; y < nHeight; y++)
            {
                for (x = 0; x < nWidth; x++)
                {
                    dSqtOne = pnGradX[y * nWidth + x] * pnGradX[y * nWidth + x];
                    dSqtTwo = pnGradY[y * nWidth + x] * pnGradY[y * nWidth + x];
                    pnMag[y * nWidth + x] = (int)(Math.Sqrt(dSqtOne + dSqtTwo) + 0.5);
                }
            }
        }

        /// <summary>
        /// 抑止梯度图中非局部极值点的象素
        /// </summary>
        /// <param name="pnMag">梯度图</param>
        /// <param name="pnGradX">X方向的方向导数</param>
        /// <param name="pnGradY">Y方向的方向层数</param>
        /// <param name="nWidth">图像宽度</param>
        /// <param name="nHeight">图像高度</param>
        /// <param name="pUnchRst">处理后结果</param>
        static private void NonmaxSuppress(int[] pnMag, int[] pnGradX, int[] pnGradY, int nWidth, int nHeight, ref byte[] pUnchRst)
        {
            // 循环控制变量
            int y;
            int x;
            int nPos;

            // x方向梯度分量
            int gx;
            int gy;

            // 临时变量
            int g1, g2, g3, g4;
            double weight;
            double dTmp1;
            double dTmp2;
            double dTmp;

            // 设置图象边缘部分为不可能的边界点
            for (x = 0; x < nWidth; x++)
            {
                pUnchRst[x] = 0;
                pUnchRst[nHeight - 1 + x] = 0;
            }
            for (y = 0; y < nHeight; y++)
            {
                pUnchRst[y * nWidth] = 0;
                pUnchRst[y * nWidth + nWidth - 1] = 0;
            }

            for (y = 1; y < nHeight - 1; y++)
            {
                for (x = 1; x < nWidth - 1; x++)
                {
                    nPos = y * nWidth + x;

                    // 如果当前象素的梯度幅度为0，则不是边界点
                    if (pnMag[nPos] == 0)
                    {
                        pUnchRst[nPos] = 0;
                    }
                    else
                    {
                        // 当前象素的梯度幅度
                        dTmp = pnMag[nPos];

                        // x，y方向导数
                        gx = pnGradX[nPos];
                        gy = pnGradY[nPos];

                        // 如果方向导数y分量比x分量大，说明导数的方向更加“趋向”于y分量。
                        if (Math.Abs(gy) > Math.Abs(gx))
                        {
                            // 计算插值的比例
                            weight = Math.Abs(gx) / Math.Abs(gy);

                            g2 = pnMag[nPos - nWidth];
                            g4 = pnMag[nPos + nWidth];

                            // 如果x，y两个方向的方向导数的符号相同
                            // C是当前象素，与g1-g4的位置关系为：
                            //	g1 g2 
                            //		 C         
                            //		 g4 g3 
                            if (gx * gy > 0)
                            {
                                g1 = pnMag[nPos - nWidth - 1];
                                g3 = pnMag[nPos + nWidth + 1];
                            }

                            // 如果x，y两个方向的方向导数的符号相反
                            // C是当前象素，与g1-g4的位置关系为：
                            //	   g2 g1
                            //		 C         
                            //	g3 g4  
                            else
                            {
                                g1 = pnMag[nPos - nWidth + 1];
                                g3 = pnMag[nPos + nWidth - 1];
                            }
                        }

                        // 如果方向导数x分量比y分量大，说明导数的方向更加“趋向”于x分量
                        // 这个判断语句包含了x分量和y分量相等的情况
                        else
                        {
                            // 计算插值的比例
                            weight = Math.Abs(gy) / Math.Abs(gx);

                            g2 = pnMag[nPos + 1];
                            g4 = pnMag[nPos - 1];

                            // 如果x，y两个方向的方向导数的符号相同
                            // C是当前象素，与g1-g4的位置关系为：
                            //	g3   
                            //	g4 C g2       
                            //       g1
                            if (gx * gy > 0)
                            {
                                g1 = pnMag[nPos + nWidth + 1];
                                g3 = pnMag[nPos - nWidth - 1];
                            }
                            // 如果x，y两个方向的方向导数的符号相反
                            // C是当前象素，与g1-g4的位置关系为：
                            //	     g1
                            //	g4 C g2       
                            //  g3     
                            else
                            {
                                g1 = pnMag[nPos - nWidth + 1];
                                g3 = pnMag[nPos + nWidth - 1];
                            }
                        }

                        // 下面利用g1-g4对梯度进行插值
                        {
                            dTmp1 = weight * g1 + (1 - weight) * g2;
                            dTmp2 = weight * g3 + (1 - weight) * g4;

                            // 当前象素的梯度是局部的最大值
                            // 该点可能是个边界点
                            if (dTmp >= dTmp1 && dTmp >= dTmp2)
                            {
                                pUnchRst[nPos] = 128;
                            }
                            else
                            {
                                // 不可能是边界点
                                pUnchRst[nPos] = 0;
                            }
                        }
                    } //else
                } // for

            }
        }

        /*************************************************************************

         * \说明:
         *   本函数实现类似“磁滞现象”的一个功能，也就是，先调用EstimateThreshold
         *   函数对经过non-maximum处理后的数据pUnchSpr估计一个高阈值，然后判断
         *   pUnchSpr中可能的边界象素(=128)的梯度是不是大于高阈值nThdHigh，如果比
         *   该阈值大，该点将作为一个边界的起点，调用TraceEdge函数，把对应该边界
         *   的所有象素找出来。最后，当整个搜索完毕时，如果还有象素没有被标志成
         *   边界点，那么就一定不是边界点。
         *   
         *************************************************************************
         */

        /// <summary>
        /// 找出所有边界
        /// 本函数实现类似“磁滞现象”的一个功能，也就是，先调用EstimateThreshold
        /// 函数对经过non-maximum处理后的数据pUnchSpr估计一个高阈值，然后判断
        /// pUnchSpr中可能的边界象素(=128)的梯度是不是大于高阈值nThdHigh，如果比
        /// 该阈值大，该点将作为一个边界的起点，调用TraceEdge函数，把对应该边界
        /// 的所有象素找出来。最后，当整个搜索完毕时，如果还有象素没有被标志成
        /// 边界点，那么就一定不是边界点。
        /// </summary>
        /// <param name="pnMag">梯度幅度图</param>
        /// <param name="nWidth">图像宽度</param>
        /// <param name="nHeight">图像高度</param>
        /// <param name="dRatioLow">低阈值和高阈值之间的比例</param>
        /// <param name="dRatioHigh">高阈值占图象象素总数的比例</param>
        /// <param name="pUnchEdge">记录边界点的缓冲区</param>
        static private void Hysteresis(int[] pnMag, int nWidth, int nHeight, double dRatioLow, double dRatioHigh, ref byte[] pUnchEdge)
        {
            // 循环控制变量
            int y;
            int x;

            int nThdHigh = 0;
            int nThdLow = 0;

            int nPos;

            // 估计TraceEdge需要的低阈值，以及Hysteresis函数使用的高阈值
            EstimateThreshold(pnMag, nWidth, nHeight, ref nThdHigh, ref nThdLow, ref pUnchEdge, ref dRatioHigh, ref dRatioLow);

            // 这个循环用来寻找大于nThdHigh的点，这些点被用来当作边界点，然后用
            // TraceEdge函数来跟踪该点对应的边界
            for (y = 0; y < nHeight; y++)
            {
                for (x = 0; x < nWidth; x++)
                {
                    nPos = y * nWidth + x;

                    // 如果该象素是可能的边界点，并且梯度大于高阈值，该象素作为
                    // 一个边界的起点
                    if ((pUnchEdge[nPos] == 128) && (pnMag[nPos] >= nThdHigh))
                    {
                        // 设置该点为边界点
                        pUnchEdge[nPos] = 255;
                        TraceEdge(y, x, nThdLow, ref pUnchEdge, pnMag, nWidth);
                    }
                }
            }

            // 那些还没有被设置为边界点的象素已经不可能成为边界点
            for (y = 0; y < nHeight; y++)
            {
                for (x = 0; x < nWidth; x++)
                {
                    nPos = y * nWidth + x;
                    if (pUnchEdge[nPos] != 255)
                    {
                        // 设置为非边界点
                        pUnchEdge[nPos] = 0;
                    }
                }
            }
        }

        /// <summary>
        /// 经过non-maximum处理后的数据pUnchEdge，统计pnMag的直方图，确定阈值。
        /// </summary>
        /// <param name="pnMag">梯度幅度图</param>
        /// <param name="nWidth">图像数据宽度</param>
        /// <param name="nHeight">图像数据高度</param>
        /// <param name="pnThdHigh">高阈值</param>
        /// <param name="pnThdLow">低阈值</param>
        /// <param name="pUnchEdge">处理图像</param>
        /// <param name="dRatioHigh">低阈值和高阈值之间的比例</param>
        /// <param name="dRationLow">高阈值占图象象素总数的比例</param>
        static private void EstimateThreshold(int[] pnMag, int nWidth, int nHeight, ref int pnThdHigh, ref int pnThdLow, ref byte[] pUnchEdge, ref double dRatioHigh, ref double dRationLow)
        {
            // 循环控制变量
            int y;
            int x;
            int k;

            // 该数组的大小和梯度值的范围有关，如果采用本程序的算法，那么梯度的范围不会超过pow(2,10)
            int[] nHist = new int[1024];

            // 可能的边界数目
            int nEdgeNb;

            // 最大梯度值
            int nMaxMag;

            int nHighCount;

            nMaxMag = 0;

            // 初始化
            for (k = 0; k < 1024; k++)
            {
                nHist[k] = 0;
            }

            // 统计直方图，然后利用直方图计算阈值
            for (y = 0; y < nHeight; y++)
            {
                for (x = 0; x < nWidth; x++)
                {
                    // 只是统计那些可能是边界点，并且还没有处理过的象素
                    if (pUnchEdge[y * nWidth + x] == 128)
                    {
                        nHist[pnMag[y * nWidth + x]]++;
                    }
                }
            }

            nEdgeNb = nHist[0];
            nMaxMag = 0;
            // 统计经过“非最大值抑止(non-maximum suppression)”后有多少象素
            for (k = 1; k < 1024; k++)
            {
                if (nHist[k] != 0)
                {
                    // 最大梯度值
                    nMaxMag = k;
                }

                // 梯度为0的点是不可能为边界点的
                // 经过non-maximum suppression后有多少象素
                nEdgeNb += nHist[k];
            }

            // 梯度比高阈值*pnThdHigh小的象素点总数目
            nHighCount = (int)(dRatioHigh * nEdgeNb + 0.5);

            k = 1;
            nEdgeNb = nHist[1];

            // 计算高阈值
            while ((k < (nMaxMag - 1)) && (nEdgeNb < nHighCount))
            {
                k++;
                nEdgeNb += nHist[k];
            }

            // 设置高阈值
            pnThdHigh = k;

            // 设置低阈值
            pnThdLow = (int)(pnThdHigh * dRationLow + 0.5);
        }

        /// <summary>
        /// 从(x,y)坐标出发，进行边界点的跟踪，跟踪只考虑pUnchEdge中没有处理并且
        /// 可能是边界点的那些象素(=128),象素值为0表明该点不可能是边界点，象素值
        /// 为255表明该点已经被设置为边界点，不必再考虑
        /// </summary>
        /// <param name="y">跟踪起点的纵坐标</param>
        /// <param name="x">跟踪起点的横坐标</param>
        /// <param name="nLowThd">判断一个点是否为边界点的低阈值</param>
        /// <param name="pUnchEdge">记录边界点的缓冲区</param>
        /// <param name="pnMag">梯度幅度图</param>
        /// <param name="nWidth">图象数据宽度</param>
        /// 
        static private void TraceEdge(int y, int x, int nLowThd, ref byte[] pUnchEdge, int[] pnMag, int nWidth)
        {
            // 对8邻域象素进行查询
            int[] xNb = { 1, 1, 0, -1, -1, -1, 0, 1 };
            int[] yNb = { 0, 1, 1, 1, 0, -1, -1, -1 };

            int yy;
            int xx;

            int k;

            for (k = 0; k < 8; k++)
            {
                yy = y + yNb[k];
                xx = x + xNb[k];
                // 如果该象素为可能的边界点，又没有处理过
                // 并且梯度大于阈值
                //if (yy * nWidth + xx < pUnchEdge.Length)
                {
                    if (pUnchEdge[yy * nWidth + xx] == 128 && pnMag[yy * nWidth + xx] >= nLowThd)
                    {
                        // 把该点设置成为边界点
                        pUnchEdge[yy * nWidth + xx] = 255;

                        // 以该点为中心进行跟踪
                        TraceEdge(yy, xx, nLowThd, ref pUnchEdge, pnMag, nWidth);
                    }
                }
            }
        }

        /// <summary>
        /// 生成垂直投影
        /// </summary>
        /// <param name="psi">待投影灰度图</param>
        /// <param name="PW">投影数组</param>
        ///
        static  public void GetWPojX(GrayImg psi, out int[] PW, out int MaxValue)
        {
            MaxValue = 0;
            int Width = psi.Width;
            int Height = psi.Height;
            PW = new int[Width];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    PW[x] += psi.Img[y * Width + x];
                    if (PW[x] > MaxValue)
                        MaxValue = PW[x];
                }

            }
            MaxValue /= 255;
            for (int i = 0; i < Width; i++)
            {
                PW[i] /= 255;
                //去掉中间的小数点"."
                //if (PW[i] < 10&& i > 55 && i < 80)
                //    PW[i] = 0;
            }


            ////附加统计内容
            //int count = 0;
            //for (int i = 0; i < Width; i++)
            //{
            //    if (PW[i] == 0)
            //    {
            //        count++;
            //    }
            //    else
            //        count = 0;
            //    if (count > 10)
            //    {
            //        PW[i] = MaxValue;
            //        count = 0;
            //    }
            //}
            return;
        }

        /// <summary>
        /// 生成水平投影
        /// </summary>
        /// <param name="psi">待投影灰度图</param>
        /// <param name="PW">投影数组</param>
        ///
        static public void GetWPojY(GrayImg psi, out int[] PW, out int MaxValue)
        {
            MaxValue = 0;
            int Width = psi.Width;
            int Height = psi.Height;
            PW = new int[Height];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    PW[y] += psi.Img[y * Width + x];
                    if (PW[y] > MaxValue)
                        MaxValue = PW[y];
                }

            }
            MaxValue /= 255;
            for (int i = 0; i < Height; i++)
                PW[i] /= 255;
            return;
        }

        /// <summary>
        /// 根据垂直投影生成图像
        /// </summary>
        /// <param name="pDestImg"></param>
        /// <param name="PH"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        static public int GenWProjImgX(out GrayImg pDestImg, int[] PW, int Height)
        {
            int Width = PW.Length;
            PicBase.ImgMalloc(out pDestImg, Width, Height);
            for (int x = 0; x < Width; x++)
            {
                for (int y = Height - PW[x] - 1; y >= 0; y--)
                {
                    //pDestImg.Img[(Height - y - 1) * pDestImg.Width + x] = 255;
                    pDestImg.Img[y * Width + x] = 255;
                }
            }
            return 1;
        }

        /// <summary>
        /// 根据水平投影生成图像
        /// </summary>
        /// <param name="pDestImg"></param>
        /// <param name="PH"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        static public int GenWProjImgY(out GrayImg pDestImg, int[] PW, int Width)
        {
            int Height = PW.Length;
            PicBase.ImgMalloc(out pDestImg, Width, Height);
            for (int x = 0; x < Height; x++)
            {
                for (int y = 0; y < PW[x]; y++)
                {
                    //pDestImg.Img[(Height - y - 1) * pDestImg.Width + x] = 255;
                    pDestImg.Img[x * Width + y] = 255;
                }
            }
            return 1;
        }

        /// <summary>
        /// 统计水平变化率
        /// </summary>
        /// <param name="PicMap">传入图像</param>
        /// <param name="PW">变化率统计数组</param>
        /// <param name="MaxValue">变化率最大值</param>
        static public void TransY(GrayImg PicMap, out int[] PW, out int MaxValue)
        {
            //Edge = new int[2];
            int Height = PicMap.Height;
            int Width = PicMap.Width;
            MaxValue = 0;

            PW = new int[Height];
            for (int i = 0; i < Height; i++)
            {
                // MaxValue = 0;
                for (int j = 0; j < Width - 1; j++)
                {
                    if ((PicMap.Img[i * Width + j] - PicMap.Img[i * Width + j + 1]) != 0)
                    {
                        PW[i]++;
                    }
                }
                if (PW[i] > MaxValue)
                    MaxValue = PW[i];
            }

            //int low = 0;
            //int high = MaxValue;
            //while (low != high)
            //{
            //    if (PW[low] < MaxValue)
            //    {
            //        Edge[0] = low;
            //        low++;
            //    }
            //    else
            //        low++;
            //    if (PW[high] < MaxValue)
            //    {
            //        Edge[1] = low;
            //        high--;
            //    }
            //    else
            //        high--;
            //}
        }

        ///// <summary>
        ///// 灰度图反色处理
        ///// </summary>
        ///// <param name="pDestImg">输出灰度图</param>
        //public int Invert(out GrayImg pDestImg, GrayImg pSrcImg)
        //{
        //    int i, j;
        //    int res = 0;
        //    GrayImg TempImg;
        //    pDestImg = new GrayImg();

        //    res = ImgMalloc(out TempImg, pSrcImg.Width, pSrcImg.Height);
        //    if (0 == res)
        //        return 0;

        //    for (i = 0; i < pSrcImg.Height; i++)
        //    {
        //        for (j = 0; j < pSrcImg.Width; j++)
        //        {
        //            //*(TempImg.pImg + i * TempImg.Width + j) = 255 - *(pSrcImg->pImg + i * pSrcImg->Width + j);
        //            TempImg.Img[i * TempImg.Width + j] = (byte)(255 - pSrcImg.Img[i * pSrcImg.Width + j]);
        //        }
        //    }
        //    pDestImg.Width = TempImg.Width;
        //    pDestImg.Height = TempImg.Height;
        //    pDestImg.Img = TempImg.Img;
        //    return 1;
        //}

        #endregion

        #region 备用
        /// <summary>
        /// 边缘检测
        /// </summary>
        /// <param name="PicMap">传入的待处理图像</param>
        /// <param name="Result">处理后结果图像</param>
        /// <param name="Rect">计算用模板</param>
        public void EdgeCheck(GrayImg PicMap, out GrayImg Result, double[,] Rect)
        {
            int Width, Height; //原图像相关数据

            //传入图像信息
            Width = PicMap.Width;
            Height = PicMap.Height;

            //返回图像大小
            Result.Width = Width;
            Result.Height = Height;

            //开辟返回图像数据区
            Result.Img = new byte[Width * Height];

            ////3x3模板
            //double[,] h ={
            //                {   -1d,    -1d,    -1d},
            //                {   -1d,     8d,-   -1d},
            //                {   -1d,    -1d,    -1d},
            //              };

            for (int j = 1; j < Height - 1; j++)
            {
                for (int i = 1; i < Width - 1; i++)
                {
                    double pby_pt;

                    pby_pt = Rect[0, 0] * PicMap.Img[(j - 1) * Width + i - 1] + Rect[0, 1] * PicMap.Img[(j - 1) * Width + i] + Rect[0, 2] * PicMap.Img[(j - 1) * Width + i + 1]
                           + Rect[1, 0] * PicMap.Img[j * Width + i - 1] + Rect[1, 1] * PicMap.Img[j * Width + i] + Rect[1, 2] * PicMap.Img[j * Width + i + 1]
                           + Rect[2, 0] * PicMap.Img[(j + 1) * Width + i - 1] + Rect[2, 1] * PicMap.Img[(j + 1) * Width + i] + Rect[2, 2] * PicMap.Img[(j + 1) * Width + i + 1];

                    //pby_pt /= 9.0d;

                    //if (pby_pt > 20)
                    //    Result.Img[j * Width + i] = (byte)(Math.Abs(pby_pt) + 100);
                    //else
                    Result.Img[j * Width + i] = (byte)(Math.Abs(pby_pt) + 0.5);


                }
            }

            for (int i = 0; i < Width; i++)
            {
                Result.Img[i] = 0;
                Result.Img[(Height - 1) * Width + i] = 0;
            }
            for (int i = 0; i < Height; i++)
            {
                Result.Img[Width * i] = 0;
                Result.Img[Width * i + Width - 1] = 0;
            }

        }
        #endregion


        /// <summary>
        /// 计算灰度直方图
        /// </summary>
        /// <param name="out_histoGram">输出的灰度直方图数组</param>
        /// <param name="pSrcImg">待计算的灰度图像</param>
        static private int CalHistoGram(out int[] HistoGram, GrayImg pSrcImg)
        {
            int i, j;
            HistoGram = new int[256];

            for (i = 0; i < pSrcImg.Height; i++)
                for (j = 0; j < pSrcImg.Width; j++)
                    //HistoGram[*(pSrcImg->pImg + i * pSrcImg->Width + j)]++;
                    HistoGram[pSrcImg.Img[i * pSrcImg.Width + j]]++;

            return 1;
        }

        /// <summary>
        /// 改变图像亮度
        /// </summary>
        /// <param name="pDestImg"></param>
        /// <param name="pSrcImg"></param>
        /// <param name="Bright">量差</param>
        public void ChangeBright(out GrayImg pDestImg, GrayImg pSrcImg, int Bright)
        {
            pDestImg = new GrayImg();
            PicBase.ImgClone(out pDestImg, pSrcImg);

            int allPixel = pSrcImg.Width * pSrcImg.Height;
            int Bpoint = 0;
            for (int i = 0; i < allPixel; i++)
            {
                Bpoint = pSrcImg.Img[i] + Bright;
                if (Bpoint > 255)
                    Bpoint = 255;
                if (Bpoint < 0)
                    Bpoint = 0;
                pDestImg.Img[i] = (byte)Bpoint;
            }
        }

        /// <summary>
        /// 改变对比度
        /// </summary>
        /// <param name="inBase"></param>
        /// <param name="Pc1"></param>
        /// <param name="Pc2"></param>
        static public void GetContrastRange(ref GrayImg inBase, int Pc1, int Pc2)
        {
            int allPixel = inBase.Width * inBase.Height;
            float p = 0;
            for (int i = 0; i < allPixel; i++)
            {
                p = (float)255 / (float)(Pc2 - Pc1) * (float)(inBase.Img[i] - Pc1);
                if (p > 255)
                    p = 255;
                if (p < 0)
                    p = 0;
                inBase.Img[i] = (byte)p;
            }
        }

        /// <summary>
        /// 中值滤波
        /// </summary>
        /// <param name="pDestImg">滤波后的灰度图像</param>
        /// <param name="pSrcImg">输入待处理的图像</param>
        public int MedianFilter(out GrayImg pDestImg, GrayImg pSrcImg, int iFilterH, int iFilterW, int iFilterMX, int iFilterMY)
        {
            pDestImg = new GrayImg();
            int i, j, l, k;
            int res = 0;
            //BYTE* aValue;
            byte[] aValue = new byte[iFilterH * iFilterW];

            GrayImg TempImg;

            res = PicBase.ImgMalloc(out TempImg, pSrcImg.Width, pSrcImg.Height);
            if (res == 0)
                return 0;

            //aValue = (BYTE*)malloc(iFilterH * iFilterW);
            //if (aValue == NULL)
            //    return 0;

            for (i = iFilterMY; i <= TempImg.Height - iFilterH + iFilterMY; i++)
                for (j = iFilterMX; j <= TempImg.Width - iFilterW + iFilterMX; j++)
                {
                    for (k = 0; k < iFilterH; k++)
                        for (l = 0; l < iFilterW; l++)
                        {
                            //*(aValue + k * iFilterW + l) = *(pSrcImg->pImg + (i - iFilterMY + k) * TempImg.Width + j - iFilterMX + 1);
                            aValue[k * iFilterW + l] = pSrcImg.Img[(i - iFilterMY + k) * TempImg.Width + j - iFilterMX + 1];
                        }
                    //*(TempImg.pImg + i * TempImg.Width + j) = GetMedianNum(aValue, iFilterH * iFilterW);
                    TempImg.Img[i * TempImg.Width + j] = GetMedianNum(ref aValue, iFilterH * iFilterW);
                }

            //if (aValue != NULL)
            //    free(aValue);

            for (i = 0; i < TempImg.Height; i++)
                for (j = 0; j < iFilterMX; j++)
                    //*(TempImg.pImg + i * TempImg.Width + j) = *(pSrcImg->pImg + i * pSrcImg->Width + j);
                    TempImg.Img[i * TempImg.Width + j] = pSrcImg.Img[i * pSrcImg.Width + j];

            for (i = 0; i < TempImg.Height; i++)
                for (j = TempImg.Width - iFilterW + iFilterMX + 1; j < TempImg.Width; j++)
                    //*(TempImg.pImg + i * TempImg.Width + j) = *(pSrcImg->pImg + i * pSrcImg->Width + j);
                    TempImg.Img[i * TempImg.Width + j] = pSrcImg.Img[i * pSrcImg.Width + j];

            for (i = 0; i < iFilterMY; i++)
                for (j = 0; j < TempImg.Width; j++)
                    //*(TempImg.pImg + i * TempImg.Width + j) = *(pSrcImg->pImg + i * pSrcImg->Width + j);
                    TempImg.Img[i * TempImg.Width + j] = pSrcImg.Img[i * pSrcImg.Width + j];

            for (i = TempImg.Height - iFilterH + iFilterMY + 1; i < TempImg.Height; i++)
                for (j = 0; j < TempImg.Width; j++)
                    //*(TempImg.pImg + i * TempImg.Width + j) = *(pSrcImg->pImg + i * pSrcImg->Width + j);
                    TempImg.Img[i * TempImg.Width + j] = pSrcImg.Img[i * pSrcImg.Width + j];

            pDestImg.Width = TempImg.Width;
            pDestImg.Height = TempImg.Height;

            //if (pDestImg->pImg)
            //{
            //    pDestImg->pImg = NULL;
            //}

            pDestImg.Img = TempImg.Img;

            return res;
        }

        /// <summary>
        /// 辅助函数，用于获取中值
        /// </summary>
        private byte GetMedianNum(ref byte[] aValue, int iLength)
        {
            int i, j;
            byte tmp;

            for (j = 1; j < iLength; j++)
                for (i = 0; i < iLength - j; i++)
                {
                    //if ((*(aValue + i)) > (*(aValue + i + 1)))
                    if (aValue[i] > aValue[i + 1])
                    {
                        //tmp = *(aValue + i);
                        //*(aValue + i) = *(aValue + i + 1);
                        //*(aValue + i + 1) = tmp;
                        tmp = aValue[i];
                        aValue[i] = aValue[i + 1];
                        aValue[i + 1] = tmp;
                    }
                }

            if ((iLength & 1) > 0)
            {
                //tmp = *(aValue + (iLength + 1) / 2);
                tmp = aValue[(iLength + 1) / 2];
            }
            else
            {
                //tmp = (*(aValue + iLength / 2) + *(aValue + iLength / 2 + 1)) / 2;
                tmp = (byte)(((int)aValue[iLength / 2] + (int)aValue[iLength / 2 + 1]) / 2);
            }

            return tmp;
        }

        /// <summary>
        /// 模板边缘检测
        /// </summary>
        /// <param name="pDestImg">传出的目标图像</param>
        /// <param name="pSrcImg">传入的源图像</param>
        /// <param name="TemplateArr">处理模板</param>
        /// <param name="TemplateN"></param>
        /// <param name="Coef"></param>
        /// <returns></returns>
        static public int Template(out GrayImg pDestImg, GrayImg pSrcImg, int[] TemplateArr, int TemplateN, double Coef)
        {
            pDestImg = new GrayImg();
            int temp, y, x, i, j;
            int res = 0;
            GrayImg TempImg;

            res = PicBase.ImgMalloc(out TempImg, pSrcImg.Width, pSrcImg.Height);
            if (res == 0)
                return 0;

            for (x = TemplateN / 2; x < pSrcImg.Height - TemplateN / 2; x++)
                for (y = TemplateN / 2; y < pSrcImg.Width - TemplateN / 2; y++)
                {
                    temp = 0;
                    for (i = -TemplateN / 2; i <= TemplateN / 2; i++)
                        for (j = -TemplateN / 2; j <= TemplateN / 2; j++)
                            temp = temp + pSrcImg.Img[(x + i) * pSrcImg.Width + (y + j)] * TemplateArr[(i + TemplateN / 2) * TemplateN + j + TemplateN / 2];
                    temp = (int)(Math.Abs(temp) * Coef + 0.5);

                    if (temp > 255)
                        temp = 255;

                    TempImg.Img[x * TempImg.Width + y] = (byte)temp;
                }

            for (x = 0; x < pSrcImg.Height; x++)
                for (y = 0; y < TemplateN / 2; y++)
                    TempImg.Img[x * TempImg.Width + y] = 0;

            for (x = 0; x < pSrcImg.Height; x++)
                for (y = pSrcImg.Width - TemplateN / 2; y < pSrcImg.Width; y++)
                    TempImg.Img[x * TempImg.Width + y] = 0;

            for (x = 0; x < TemplateN / 2; x++)
                for (y = 0; y < pSrcImg.Width; y++)
                    TempImg.Img[x * TempImg.Width + y] = 0;

            for (x = pSrcImg.Height - TemplateN / 2; x < pSrcImg.Height; x++)
                for (y = 0; y < pSrcImg.Width; y++)
                    TempImg.Img[x * TempImg.Width + y] = 0;

            pDestImg.Width = TempImg.Width;
            pDestImg.Height = TempImg.Height;
            pDestImg.Img = TempImg.Img;

            return 1;
        }

        /// <summary>
        /// 给图像中的某个区块进行填充
        /// </summary>
        /// <param name="pDestImg">传出的目标图像</param>
        /// <param name="pSrcImg">传入的图像</param>
        /// <param name="rect">操作区域</param>
        /// <param name="Cor">填充颜色</param>
        /// <returns></returns>
        public int FillBlock(out GrayImg pDestImg, GrayImg pSrcImg, Rectangle rect, byte Cor)
        {
            PicBase.ImgClone(out pDestImg, pSrcImg);
            for (int y = rect.Top; y < rect.Top + rect.Height; y++)
            {
                for (int x = rect.Left; x < rect.Left + rect.Width; x++)
                {
                    pDestImg.Img[y * pDestImg.Width + x] = Cor;
                }
            }
            return 1;
        }

        /// <summary>
        /// 获取子图
        /// </summary>
        /// <param name="pDestImg">目标图像</param>
        /// <param name="pSrcImg">源图像</param>
        /// <param name="rect">截取区域</param>
        /// <returns></returns>
        static public int GetSubImg(out PICLib.GrayImg pDestImg, GrayImg pSrcImg, Rectangle rect)
        {
            PicBase.ImgMalloc(out pDestImg, rect.Width, rect.Height);
            for (int y = rect.Top; y < rect.Top + rect.Height; y++)
            {
                for (int x = rect.Left; x < rect.Left + rect.Width; x++)
                {
                    pDestImg.Img[(y - rect.Top) * rect.Width + (x - rect.Left)] = pSrcImg.Img[y * pSrcImg.Width + x];
                }
            }
            return 1;
        }

        /// <summary>
        /// 垂直投影——二值图
        /// </summary>
        /// <param name="psi"></param>
        /// <param name="PH"></param>
        static public void GetHPoj(GrayImg psi, out int[] PH)
        {
            PH = new int[psi.Width];
            for (int x = 0; x < psi.Width; x++)
            {
                for (int y = 0; y < psi.Height; y++)
                {
                    PH[x] += psi.Img[y * psi.Width + x];
                }
                PH[x] = PH[x] / 255;         //白点个数
            }
        }

        /// <summary>
        /// 根据投影生成图像
        /// </summary>
        /// <param name="pDestImg"></param>
        /// <param name="PH"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        static public int GenProjImg(out GrayImg pDestImg, int[] PH, int Height)
        {
            PicBase.ImgMalloc(out pDestImg, PH.Length, Height);
            for (int x = 0; x < pDestImg.Width; x++)
            {
                for (int y = 0; y < PH[x]; y++)
                {
                    pDestImg.Img[(Height - y - 1) * pDestImg.Width + x] = 255;
                }
            }
            return 1;
        }

        /// <summary>
        /// 根据投影生成图像
        /// </summary>
        /// <param name="pDestImg"></param>
        /// <param name="PH"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        static public int GenWProjImg(out GrayImg pDestImg, int[] PW, int Width)
        {
            PicBase.ImgMalloc(out pDestImg, Width, PW.Length);
            for (int y = 0; y < pDestImg.Height; y++)
            {
                for (int x = 0; x < PW[y]; x++)
                {
                    //pDestImg.Img[(Height - y - 1) * pDestImg.Width + x] = 255;
                    pDestImg.Img[y * pDestImg.Width + x] = 255;
                }
            }
            return 1;
        }


        /// <summary>
        /// 细化图像
        /// </summary>
        /// <param name="pDestImg"></param>
        /// <param name="pSrcImg"></param>
        static public void Thinning(out GrayImg pDestImg, GrayImg pSrcImg)
        {
            PicBase.ImgMalloc(out pDestImg, pSrcImg.Width, pSrcImg.Height);

            //脏标记
            bool bModified;

            //循环变量
            int i;
            int j;
            int n;
            int m;

            //四个条件
            bool bCondition1;
            bool bCondition2;
            bool bCondition3;
            bool bCondition4;

            //计数器
            int nCount;

            //像素值
            byte pixel;

            //5×5相邻区域像素值
            int[,] neighbour = new int[5, 5];

            bModified = true;
            for (int d = 0; d < pDestImg.Height * pDestImg.Width; d++)
            {
                pDestImg.Img[d] = 255;
            }

            //while (bModified)
            //{
            //    bModified = false;

            //// 初始化新分配的内存，设定初始值为255
            //lpDst = (char *)lpNewDIBBits;
            //memset(lpDst, (BYTE)255, lWidth * lHeight);

            int lHeight = pSrcImg.Height;
            int lWidth = pSrcImg.Width;

            for (j = 2; j < lHeight - 2; j++)
            {
                for (i = 2; i < lWidth - 2; i++)
                {

                    bCondition1 = false;
                    bCondition2 = false;
                    bCondition3 = false;
                    bCondition4 = false;

                    //由于使用5×5的结构元素，为防止越界，所以不处理外围的几行和几列像素
                    // 指向源图像倒数第j行，第i个象素的指针
                    int lpSrc = lWidth * j + i;

                    // 指向目标图像倒数第j行，第i个象素的指针			
                    int lpDst = lWidth * j + i;

                    //取得当前指针处的像素值
                    pixel = pSrcImg.Img[lpSrc];

                    //目标图像中含有0和255外的其它灰度值
                    if (pixel != 255 && pixel != 0)
                        continue;
                    //如果源图像中当前点为白色，则跳过
                    else
                        if (pixel == 255)
                            continue;

                    //获得当前点相邻的5×5区域内像素值，白色用0代表，黑色用1代表
                    for (m = 0; m < 5; m++)
                    {
                        for (n = 0; n < 5; n++)
                        {
                            neighbour[m, n] = (255 - pSrcImg.Img[lpSrc + ((4 - m) - 2) * lWidth + n - 2]) / 255;
                        }
                    }
                    //逐个判断条件。
                    //判断2<=NZ(P1)<=6
                    nCount = neighbour[1, 1] + neighbour[1, 2] + neighbour[1, 3]
                            + neighbour[2, 1] + neighbour[2, 3] +
                            +neighbour[3, 1] + neighbour[3, 2] + neighbour[3, 3];
                    if (nCount >= 2 && nCount <= 6)
                        bCondition1 = true;

                    //判断Z0(P1)=1
                    nCount = 0;
                    if (neighbour[1, 2] == 0 && neighbour[1, 1] == 1)
                        nCount++;
                    if (neighbour[1, 1] == 0 && neighbour[2, 1] == 1)
                        nCount++;
                    if (neighbour[2, 1] == 0 && neighbour[3, 1] == 1)
                        nCount++;
                    if (neighbour[3, 1] == 0 && neighbour[3, 2] == 1)
                        nCount++;
                    if (neighbour[3, 2] == 0 && neighbour[3, 3] == 1)
                        nCount++;
                    if (neighbour[3, 3] == 0 && neighbour[2, 3] == 1)
                        nCount++;
                    if (neighbour[2, 3] == 0 && neighbour[1, 3] == 1)
                        nCount++;
                    if (neighbour[1, 3] == 0 && neighbour[1, 2] == 1)
                        nCount++;
                    if (nCount == 1)
                        bCondition2 = true;

                    //判断P2*P4*P8=0 or Z0(p2)!=1
                    if (neighbour[1, 2] * neighbour[2, 1] * neighbour[2, 3] == 0)
                        bCondition3 = true;
                    else
                    {
                        nCount = 0;
                        if (neighbour[0, 2] == 0 && neighbour[0, 1] == 1)
                            nCount++;
                        if (neighbour[0, 1] == 0 && neighbour[1, 1] == 1)
                            nCount++;
                        if (neighbour[1, 1] == 0 && neighbour[2, 1] == 1)
                            nCount++;
                        if (neighbour[2, 1] == 0 && neighbour[2, 2] == 1)
                            nCount++;
                        if (neighbour[2, 2] == 0 && neighbour[2, 3] == 1)
                            nCount++;
                        if (neighbour[2, 3] == 0 && neighbour[1, 3] == 1)
                            nCount++;
                        if (neighbour[1, 3] == 0 && neighbour[0, 3] == 1)
                            nCount++;
                        if (neighbour[0, 3] == 0 && neighbour[0, 2] == 1)
                            nCount++;
                        if (nCount != 1)
                            bCondition3 = true;
                    }

                    //判断P2*P4*P6=0 or Z0(p4)!=1
                    if (neighbour[1, 2] * neighbour[2, 1] * neighbour[3, 2] == 0)
                        bCondition4 = true;
                    else
                    {
                        nCount = 0;
                        if (neighbour[1, 1] == 0 && neighbour[1, 0] == 1)
                            nCount++;
                        if (neighbour[1, 0] == 0 && neighbour[2, 0] == 1)
                            nCount++;
                        if (neighbour[2, 0] == 0 && neighbour[3, 0] == 1)
                            nCount++;
                        if (neighbour[3, 0] == 0 && neighbour[3, 1] == 1)
                            nCount++;
                        if (neighbour[3, 1] == 0 && neighbour[3, 2] == 1)
                            nCount++;
                        if (neighbour[3, 2] == 0 && neighbour[2, 2] == 1)
                            nCount++;
                        if (neighbour[2, 2] == 0 && neighbour[1, 2] == 1)
                            nCount++;
                        if (neighbour[1, 2] == 0 && neighbour[1, 1] == 1)
                            nCount++;
                        if (nCount != 1)
                            bCondition4 = true;
                    }
                    if (bCondition1 && bCondition2 && bCondition3 && bCondition4)
                    {
                        pDestImg.Img[lpDst] = 255;
                        bModified = true;
                    }
                    else
                    {
                        pDestImg.Img[lpDst] = 0;
                    }
                }
            }
            //}//end while
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outResult"></param>
        /// <param name="pSrcImg"></param>
        /// <param name="oW"></param>
        /// <param name="oH"></param>
        static public void ImgToDoubleArray(out double[] outResult, GrayImg pSrcImg, int oW, int oH)
        {
            outResult = new double[oW * oH];
            double xStep = (double)pSrcImg.Width / (double)oW;
            double yStep = (double)pSrcImg.Height / (double)oH;
            for (int i = 0; i < pSrcImg.Width; i++)
                for (int j = 0; j < pSrcImg.Height; j++)
                {
                    int x = (int)((double)i / xStep);
                    int y = (int)((double)j / yStep);
                    outResult[y * x + y] += 1.7320508075688772935274463415059D * (double)pSrcImg.Img[i * pSrcImg.Height + j];
                }
            int all = oW * oH;
            double max = double.NegativeInfinity;
            for (int i = 0; i < all; i++)
            {
                if (outResult[i] > max)
                    max = outResult[i];
            }
            if (max != 0)
            {
                for (int i = 0; i < all; i++)
                    outResult[i] = outResult[i] / max;
            }
        }
       

    }
}
