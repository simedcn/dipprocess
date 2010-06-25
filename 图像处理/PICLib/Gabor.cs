using System;
using System.Collections.Generic;
using System.Text;

namespace PICLib
{
    public struct GaborMatrix
    {
        public int Width;
        public int Height;
        public int[] Arr;
    }

    public class Gabor
    {
        float[] ikernel;
        float[] rkernel;
        int[,] rout;
        int[,] iout;
        int ksize;
        public IrisCode RCode;
        public IrisCode ICode;

        #region 单例模式
        private static Gabor objSelf = null;
        private Gabor()
        { }
        public static Gabor GetInstance()
        {
            if (objSelf == null)
                objSelf = new Gabor();
            return objSelf;
        }
        #endregion

        public void generateCode(GrayImg inBase, bool isTrim)
        {
            int width = inBase.Width;
            int height = inBase.Height;

            int[,] unrolled2d = new int[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    unrolled2d[x, y] = (int)inBase.Img[y * width + x];
                }
            }
            applyFilter(3, 2, unrolled2d, 0.2F, 0.05F);
            int[,] imgarr = isTrim ? trimArr(iout) : iout;
            byte[] output = thresholdImageData(imgarr);
            ICode = new IrisCode(output, imgarr.GetLength(0), imgarr.GetLength(1));

            int[,] realarr = isTrim ? trimArr(rout) : rout;
            byte[] output1 = thresholdImageData(realarr);
            RCode = new IrisCode(output1, realarr.GetLength(0), imgarr.GetLength(1));
        }

        private void applyFilter(int scale, double orientation, int[,] imagein, float uh, float ul)
        {
            int width = imagein.GetLength(0);
            int height = imagein.GetLength(1);
            createKernel(width, height, scale, orientation, ul, uh);
            rout = convolve(imagein, rkernel, ksize);
            iout = convolve(imagein, ikernel, ksize);
        }

        private int[,] trimArr(int[,] datain)
        {
            int height = datain.GetLength(1);
            int width = datain.GetLength(0);
            int xstart = width - 40;
            int xend = width - 20;
            int ystart = 20;
            int yend = height - 20;
            int[,] outArr = new int[xend - xstart, yend - ystart];
            int x = xstart;
            for (int j = 0; x < xend; j++)
            {
                int y = ystart;
                for (int k = 0; y < yend; k++)
                {
                    outArr[j, k] = datain[x, y];
                    y++;
                }
                x++;
            }
            return outArr;
        }

        private byte[] thresholdImageData(int[,] datain)
        {
            int width = datain.GetLength(0);
            int height = datain.GetLength(1);
            byte[] Arrout = new byte[width * height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Arrout[y * width + x] = (byte)datain[x, y];
                }
            }
            return Arrout;
        }

        private void createKernel(int width, int height, int scale, double orientation, float ul, float uh)
        {
            int side = 8;
            ksize = 2 * side + 1;
            float[,] r2d = new float[ksize, ksize];
            float[,] i2d = new float[ksize, ksize];
            double n = orientation;
            int s = 3;

            double Mbase = uh / ul;
            double a = Math.Pow(Mbase, 1.0D / (double)(scale - 1));
            double u0 = (double)uh / Math.Pow(a, scale - s);
            double uvar = ((a - 1.0D) * u0) / ((a + 1.0D) * Math.Sqrt(2D * Math.Log(2D)));
            double z = (-2D * Math.Log(2D) * (uvar * uvar)) / u0;
            double vvar = (Math.Tan(Math.PI / (double)(2 * orientation)) * (u0 + z)) / Math.Sqrt(2D * Math.Log(2D) - (z * z) / (uvar * uvar));
            double xvar = 1.0D / (2 * Math.PI * uvar);
            double yvar = 1.0D / (2 * Math.PI * vvar);
            double t1 = Math.Cos((Math.PI / (double)orientation) * ((double)n - 1.0D));
            double t2 = Math.Sin((Math.PI / (double)orientation) * ((double)n - 1.0D));
            for (int x = 0; x < ksize; x++)
            {
                for (int y = 0; y < ksize; y++)
                {
                    double xd = (double)(x - side) * t1 + (double)(y - side) * t2;
                    double yd = (double)(-(x - side)) * t2 + (double)(y - side) * t1;
                    double g = (1.0D / (2 * Math.PI * xvar * yvar)) * Math.Pow(a, (double)scale - (double)s) * Math.Exp(-0.5D * ((xd * xd) / (xvar * xvar) + (yd * yd) / (yvar * yvar)));
                    r2d[x, y] = (float)(g * Math.Cos(2 * Math.PI * u0 * xd));
                    i2d[x, y] = (float)(g * Math.Sin(2 * Math.PI * u0 * xd));
                }
            }
            float rsum = 0.0F;
            float isum = 0.0F;
            int i = 0;
            rkernel = new float[ksize * ksize];
            ikernel = new float[ksize * ksize];
            for (int x = 0; x < 2 * side + 1; x++)
            {
                for (int y = 0; y < 2 * side + 1; y++)
                {
                    rkernel[i] = r2d[x, y];
                    rsum += rkernel[i];
                    ikernel[i] = i2d[x, y];
                    isum += ikernel[i];
                    i++;
                }
            }
            for (i = 0; i < rkernel.Length; i++)
                rkernel[i] = rkernel[i] / rsum;

            for (i = 0; i < ikernel.Length; i++)
                ikernel[i] = ikernel[i] / isum;
        }

        /// <summary>
        /// 卷积
        /// </summary>
        /// <param name="image"></param>
        /// <param name="kernel"></param>
        /// <param name="ksize"></param>
        /// <returns></returns>
        private int[,] convolve(int[,] image, float[] kernel, int ksize)
        {
            double[,] normimage = imageNormalise(image);
            int width = normimage.GetLength(0);
            int height = normimage.GetLength(1);
            double[,] cimage = new double[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double sum = 0;
                    if (x - ksize / 2 < 0 || y - ksize / 2 < 0 || x + ksize / 2 >= width || y + ksize / 2 >= height)
                    {
                        cimage[x, y] = 0.0D;
                    }
                    else
                    {
                        int m = 0;
                        for (int k = 0; k < ksize; k++)
                        {
                            for (int l = 0; l < ksize; l++)
                            {
                                sum = sum + normimage[x, y] * (double)kernel[m];
                                m++;
                            }
                        }
                        cimage[x, y] = sum;
                    }
                }
            }
            int[,] Aout = imageDeNormalise(cimage);
            return Aout;
        }

        private double[,] imageNormalise(int[,] imagein)
        {
            int width = imagein.GetLength(0);
            int height = imagein.GetLength(1);
            double[,] imageout = new double[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                    imageout[x, y] = (double)imagein[x, y] / 255D;
            }
            return imageout;
        }

        private int[,] imageDeNormalise(double[,] arrin)
        {
            int width = arrin.GetLength(0);
            int height = arrin.GetLength(1);
            int[,] imageout = new int[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                    if (arrin[x, y] > 0.0D)
                        imageout[x, y] = 255;
                    else
                        imageout[x, y] = 0;
            }
            return imageout;
        }
    }
}
