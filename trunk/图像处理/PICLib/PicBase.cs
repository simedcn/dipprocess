using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace PICLib
{
        #region 灰度图的存储结构
        public struct GrayImg
        {
            /// <summary>
            /// 灰度图实体
            /// </summary>
            public byte[] Img;
            /// <summary>
            /// 灰度图宽度
            /// </summary>
            public int Width;
            /// <summary>
            /// 灰度图高度
            /// </summary>
            public int Height;
        }
        #endregion

        #region 彩色图的存储结构
        public struct ColorImg
        {
            /// <summary>
            /// 红色分量
            /// </summary>
            public byte[] R;
            /// <summary>
            /// 蓝色分量
            /// </summary>
            public byte[] B;
            /// <summary>
            /// 绿色分量
            /// </summary>
            public byte[] G;
            /// <summary>
            /// 彩色图像宽度
            /// </summary>
            public int Width;
            /// <summary>
            /// 彩色图像高度
            /// </summary>
            public int Height;
        }
        #endregion

        public struct ConnectedRegions
        {
            public byte[] connected;
            public Rectangle maxRect;
            public Rectangle minRect;
            public Rectangle closestCenterRect;
        }

    public class PicBase
    {
       

         #region 启用单例模式定义图像处理类
        private static PicBase objSelf = null;
        private PicBase()
        { }
        public static PicBase GetInstance()
        {
            if (objSelf == null)
            {
                objSelf = new PicBase();
            }
            return objSelf;
        } 
        #endregion

        #region Bitmap与定义图像基互转
        /// <summary>
        /// 彩色图像转成定义灰度图类型
        /// </summary>
        /// <param name="out_corImg">输出的灰度图像</param>
        /// <param name="in_Bmp">输入的图像</param>
        static public int ImgToArray(out GrayImg out_gryImg, Bitmap in_Bmp)
        {
            int width = in_Bmp.Width;
            int height = in_Bmp.Height;
            int allPixel = width * height;
            int res = ImgMalloc(out out_gryImg, in_Bmp.Width, in_Bmp.Height);
            if (res == 0)
                return 0;

            BitmapData bmData = in_Bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            System.IntPtr PtrStart = bmData.Scan0;

            unsafe
            {
                //byte* Ptr = (byte*)(void*)PtrStart;
                //for (int i = 0; i < allPixel; i++)
                //{
                //    out_gryImg.Img[i] = (byte)(0.299f * Ptr[2] + 0.587f * Ptr[1] + 0.114f * Ptr[0]);
                //    Ptr += 3;
                //}

                byte* Ptr = (byte*)(void*)PtrStart;
                int nOffset = bmData.Stride - width * 3;

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        out_gryImg.Img[i * width + j] = (byte)(0.299f * Ptr[2] + 0.587f * Ptr[1] + 0.114f * Ptr[0]);
                        Ptr += 3;
                    }
                    Ptr += nOffset;
                }
            }
            in_Bmp.UnlockBits(bmData);
            return 1;
        }

        /// <summary>
        /// 彩色图像转成定义类型
        /// </summary>
        /// <param name="corImg">转换后的彩色图像</param>
        /// <param name="out_corImg">输出的彩色图像</param>
        /// <param name="in_Bmp">输入的图像</param>
        static public int ImgToArray(out ColorImg out_corImg, Bitmap in_Bmp)
        {
            int width = in_Bmp.Width;
            int height = in_Bmp.Height;
            int allPixel = width * height;
            int res = ImgMalloc(out out_corImg, in_Bmp.Width, in_Bmp.Height);
            if (res == 0)
                return 0;
            BitmapData bmData = in_Bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            System.IntPtr PtrStart = bmData.Scan0;
            unsafe
            {
                byte* Ptr = (byte*)(void*)PtrStart;

                int Height = in_Bmp.Height;
                int Width = in_Bmp.Width;
                int nOffset = bmData.Stride - Width * 3;

                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        out_corImg.R[i * width + j] = Ptr[2];
                        out_corImg.G[i * width + j] = Ptr[1];
                        out_corImg.B[i * width + j] = Ptr[0];
                        Ptr += 3;
                    }
                    Ptr += nOffset;
                }
            }
            in_Bmp.UnlockBits(bmData);
            return 1;
        }

        /// <summary>
        /// 预定义彩色图像矩阵转为Bitmap图像
        /// </summary>
        /// <param name="out_Bmp">输出的Bitmap图像</param>
        /// <param name="in_corImg">输入彩色图</param>
        static public int ArrayToImg(out Bitmap out_Bmp, ColorImg in_corImg)
        {
            int width = in_corImg.Width;
            int height = in_corImg.Height;
            out_Bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            int allPixel = width * height;

            BitmapData bmData = out_Bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            System.IntPtr PtrStart = bmData.Scan0;
            unsafe
            {
                byte* Ptr = (byte*)(void*)PtrStart;

                int nOffset = bmData.Stride - width * 3;

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Ptr[2] = in_corImg.R[i * width + j];
                        Ptr[1] = in_corImg.G[i * width + j];
                        Ptr[0] = in_corImg.B[i * width + j];
                        Ptr += 3;
                    }
                    Ptr += nOffset;
                }

                //for (int i = 0; i < allPixel; i++)
                //{
                //    Ptr[2] = in_corImg.R[i];
                //    Ptr[1] = in_corImg.G[i];
                //    Ptr[0] = in_corImg.B[i];
                //    Ptr += 3;
                //}
            }
            out_Bmp.UnlockBits(bmData);

            return 1;
        }

        /// <summary>
        /// 预定义灰度图像矩阵转为Bitmap图像
        /// </summary>
        /// <param name="out_Bmp">输出的Bitmap图像</param>
        /// <param name="in_gryImg">输入的灰度图像</param>
        static public int ArrayToImg(out Bitmap out_Bmp, GrayImg in_gryImg)
        {
            int width = in_gryImg.Width;
            int height = in_gryImg.Height;
            int allPixel = width * height;
            if (height * width == 0)
            {
                out_Bmp = new Bitmap(5, 5);
                return 0;
            }
            else
                out_Bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData bmData = out_Bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            System.IntPtr PtrStart = bmData.Scan0;
            unsafe
            {

                byte* Ptr = (byte*)(void*)PtrStart;

                int nOffset = bmData.Stride - width * 3;

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Ptr[1] = Ptr[0] = Ptr[2] = in_gryImg.Img[i * width + j];
                        Ptr += 3;
                    }
                    Ptr += nOffset;
                }

                //for (int i = 0; i < allPixel; i++)
                //{
                //    Ptr[1] = Ptr[0] = Ptr[2] = in_gryImg.Img[i];
                //    Ptr += 3;
                //}
            }
            out_Bmp.UnlockBits(bmData);
            return 1;
        }
        #endregion

        #region 图像复制
        /// <summary>
        /// 彩色图像复制
        /// </summary>
        /// <param name="out_corImg">输出的彩色图像</param>
        /// <param name="in_corImg">输入的彩色图像</param>
        static public int ImgClone(out ColorImg out_corImg, ColorImg in_corImg)
        {
            int res = ImgMalloc(out out_corImg, in_corImg.Width, in_corImg.Height);
            if (res == 0)
                return 0;

            out_corImg.Width = in_corImg.Width;
            out_corImg.Height = in_corImg.Height;

            out_corImg.R = (byte[])in_corImg.R.Clone();
            out_corImg.B = (byte[])in_corImg.B.Clone();
            out_corImg.G = (byte[])in_corImg.G.Clone();

            return 1;
        }

        /// <summary>
        /// 灰度图像复制
        /// </summary>
        /// <param name="out_gryImg">输出的灰度图像</param>
        /// <param name="in_gryImg">输入的灰度图像</param>
        static public int ImgClone(out GrayImg out_gryImg, GrayImg in_gryImg)
        {

            int res = ImgMalloc(out out_gryImg, in_gryImg.Width, in_gryImg.Height);
            if (res == 0)
                return 0;

            out_gryImg.Width = in_gryImg.Width;
            out_gryImg.Height = in_gryImg.Height;

            out_gryImg.Img = (byte[])in_gryImg.Img.Clone();

            return 1;
        }

        /// <summary>
        /// 给彩色图像预申请空间
        /// </summary>
        /// <param name="out_corImg">输出申请过空间的彩色图像</param>
        /// <param name="width">申请图像的宽度</param>
        /// <param name="height">申请图像的高度</param>
        static public int ImgMalloc(out ColorImg out_corImg, int width, int height)
        {
            out_corImg = new ColorImg();
            int allpixel = width * height;
            if (allpixel <= 0)
                return 0;
            out_corImg.R = new byte[allpixel];
            out_corImg.G = new byte[allpixel];
            out_corImg.B = new byte[allpixel];
            out_corImg.Height = height;
            out_corImg.Width = width;

            return 1;
        }

        /// <summary>
        /// 给灰度图像预申请空间
        /// </summary>
        /// <param name="out_gryImg">输出申请过空间的灰度图</param>
        /// <param name="width">申请图像的宽度</param>
        /// <param name="height">申请图像的高度</param>
        static public int ImgMalloc(out GrayImg out_gryImg, int width, int height)
        {
            out_gryImg = new GrayImg();
            int allpixel = width * height;
            if (allpixel <= 0)
                return 0;
            out_gryImg.Img = new byte[allpixel];
            out_gryImg.Height = height;
            out_gryImg.Width = width;

            return 1;
        }

        static public GrayImg CreateImg(byte[] data, int width, int height)
        {
            GrayImg result;
            ImgMalloc(out result, width, height);
            result.Img = data;
            return result;
        }
        #endregion

        #region 灰度<--->彩色
        /// <summary>
        /// 灰度图结构转彩色图结构
        /// </summary>
        /// <param name="out_corImg">输出的彩色图像</param>
        /// <param name="in_gryImg">输入的灰度图像</param>
        public int GryToCor(out ColorImg out_corImg, GrayImg in_gryImg)
        {
            out_corImg = new ColorImg();
            out_corImg.Width = in_gryImg.Width;
            out_corImg.Height = in_gryImg.Height;

            out_corImg.R = (byte[])in_gryImg.Img.Clone();
            out_corImg.G = (byte[])in_gryImg.Img.Clone();
            out_corImg.B = (byte[])in_gryImg.Img.Clone();

            return 1;
        }

        /// <summary>
        /// 彩色图结构转灰度图结构
        /// </summary>
        /// <param name="out_gryImg">输出的灰度图像</param>
        /// <param name="in_corImg">输入的彩色图像</param>
        public int CorToGry(out GrayImg out_gryImg, ColorImg in_corImg)
        {
            int width = in_corImg.Width;
            int height = in_corImg.Height;
            int allPixel = width * height;

            int res = ImgMalloc(out out_gryImg, width, height);
            if (res == 0)
                return 0;

            out_gryImg.Width = width;
            out_gryImg.Height = height;

            for (int i = 0; i < allPixel; i++)
            {
                out_gryImg.Img[i] = (byte)(0.299f * in_corImg.R[i] + 0.587f * in_corImg.G[i] + 0.114f * in_corImg.B[i]);
            }

            return 1;
        }
        #endregion

        //public PicBase()
        //{
        //    Width = 0;
        //    Height = 0;
        //     = null;
        //}

        //public PicBase(Bitmap PicIn)
        //{
        //    Width = PicIn.Width;
        //    Height = PicIn.Height;
        //    Data = PicIn.PropertyItems;
        //}
    }
}
