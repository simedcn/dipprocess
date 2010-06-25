using System;
using System.Collections.Generic;
using System.Text;

namespace PICLib
{
    public struct Circle
    {
        public int radius;
        public int x0;
        public int y0;
    }

    public struct IrisLocation
    {
        public Circle pupil;
        public Circle limbus;
    }
    public class IrisLocator
    {
        private double?[] storedCos;
        private double?[] storedSin;

        PICLib.Process DipTool;

        #region 单例模式
        private static IrisLocator objSelf = null;
        private IrisLocator()
        {
            DipTool = PICLib.Process.GetInstance();
        }
        public static IrisLocator GetInstance()
        {
            if (objSelf == null)
                objSelf = new IrisLocator();
            return objSelf;
        }
        #endregion

        public IrisLocation findIris(GrayImg inBase)
        {
            Circle p = findPupil(inBase, 1, 1.0D);
            Circle l = findLimbus(inBase, p, 7, 10D);

            IrisLocation i = new IrisLocation();
            i.limbus = ScaleCircle(l, 1);
            i.pupil = ScaleCircle(p, 1);
            return i;
        }

        public Circle ScaleCircle(Circle c, float scale)
        {
            Circle ret = new Circle();
            ret.radius = (int)((float)c.radius / scale);
            ret.x0 = (int)((float)c.x0 / scale);
            ret.y0 = (int)((float)c.y0 / scale);
            return ret;
        }

        Circle findPupil(PICLib.GrayImg inBase, int size, double sigma)
        {
            //GreyImage im = RandomImageProcessing.greyscale(image);
            //RandomImageProcessing.gaussianSmooth(im, size, sigma);
            //RandomImageProcessing.threshold(im);
            //im.image = RandomImageProcessing.toBufferedImage(im);

            Process.GaussianSmooth(inBase, out inBase, size, sigma);
            Process.BinaryImg(inBase, out inBase);

            int[] Template = new int[25];
            for (int i = 0; i < 25; i++)
                Template[i] = 1;
            PICLib.GrayImg OpenedBase;
            DipTool.Opening(inBase, out OpenedBase, Template, 5);

            ConnectedRegions regions   = DipTool.getConnectedRegions(OpenedBase, inBase.Width, inBase.Height);
            Circle c = new Circle();
            int maxRadius = (int)((regions.maxRect.Width * 1.2D) / 2D);
            int maxDif = -1;
            int minRadius = (int)((regions.maxRect.Width * 0.5D) / 2D);
            int maxX = 0;
            int maxY = 0;
            int maxR = 5;
            int prevX = 0;
            int prevY = 0;
            int prevR = 5;
            int halfSize = (int)(Math.Floor((double)(float)size / 2D) <= (double)minRadius ? minRadius : Math.Floor((double)(float)size / 2D));
            int validCount = 0;
            int invalidCount = 0;
            for (int x = halfSize; x < inBase.Width - halfSize; x++)
            {
                for (int y = halfSize; y < inBase.Height - halfSize; y++)
                {
                    if (regions.connected[x + y * inBase.Width] == 0)
                    {
                        invalidCount++;
                    }
                    else
                    {
                        int prevSum = -1;
                        for (int radius = minRadius; radius < maxRadius; radius++)
                        {
                            if (x - radius < halfSize || x + radius > inBase.Width - halfSize || y - radius < halfSize || y + radius > inBase.Height - halfSize)
                                break;
                            c.x0 = x;
                            c.y0 = y;
                            c.radius = radius;
                            int radSum = sumCircle(inBase, c, 1.0471975511965976D);
                            if (prevSum != -1 && Math.Abs(prevSum - radSum) > maxDif)
                            {
                                maxDif = Math.Abs(prevSum - radSum);
                                maxX = prevX;
                                maxY = prevY;
                                maxR = prevR;
                            }
                            prevSum = radSum;
                            prevX = x;
                            prevY = y;
                            prevR = radius;
                        }
                        validCount++;
                    }
                }
            }
            c.radius = maxR;
            c.x0 = maxX;
            c.y0 = maxY;
            return c;
        }

        Circle findLimbus(PICLib.GrayImg inBase, Circle pupil, int size, double sigma)
        {
            //DipTool.GaussianSmooth(inBase, out inBase, size, sigma);
            Process.GaussianSmooth(inBase, out inBase, size, sigma);

            int maxRadius = inBase.Width <= inBase.Height ? inBase.Width : inBase.Height;
            int maxDif = -1;
            int minRadius = 30;
            int maxX = 0;
            int maxY = 0;
            int maxR = 5;
            int prevX = 0;
            int prevY = 0;
            int prevR = 5;
            int halfSize = pupil.radius / 5;
            Circle c = new Circle();
            for (int x = pupil.x0 - halfSize; x < pupil.x0 + halfSize; x++)
            {
                for (int y = pupil.y0 - halfSize; y < pupil.y0 + halfSize; y++)
                    if (Math.Sqrt(Math.Pow(x - pupil.x0, 2D) + Math.Pow(y - pupil.y0, 2D)) < (double)pupil.radius)
                    {
                        int prevSum = -1;
                        for (int radius = (int)((double)pupil.radius + Math.Sqrt(Math.Pow(x - pupil.x0, 2D) + Math.Pow(y - pupil.y0, 2D)) + (double)(float)pupil.radius / 2D); radius < maxRadius; radius++)
                        {
                            if (x - radius < size || x + radius > inBase.Width - size || y - radius < 0 || y + radius > inBase.Height)
                                break;
                            c.x0 = x;
                            c.y0 = y;
                            c.radius = radius;
                            int radSum = sumCircle(inBase, c, 0.78539816339744828D);
                            if (prevSum != -1 && Math.Abs(prevSum - radSum) > maxDif)
                            {
                                maxDif = Math.Abs(prevSum - radSum);
                                maxX = prevX;
                                maxY = prevY;
                                maxR = prevR;
                            }
                            prevSum = radSum;
                            prevX = x;
                            prevY = y;
                            prevR = radius;
                        }
                    }
            }
            c.x0 = maxX;
            c.y0 = maxY;
            c.radius = maxR;
            return c;
        }

        public int sumCircle(PICLib.GrayImg inBase, Circle c, double cone)
        {
            int w = inBase.Width;
            int h = inBase.Height;

            double inc = 0.078539816339744828D;
            int count = (int)((cone * 4D) / inc) + 1;
            if (storedCos == null || storedCos.Length != count)
            {
                storedCos = new double?[count];
                storedSin = new double?[count];
            }
            int radSum = 0;
            int i = 0;
            for (double angle = Math.PI; angle > 0.0D; angle -= inc)
                if (angle <= cone || angle >= Math.PI - cone && angle <= Math.PI + cone || angle >= 2 * Math.PI - cone)
                {
                    double? cosAngle = 0;
                    if ((cosAngle = storedCos[i]) == null)
                        storedCos[i] = cosAngle = Math.Cos(angle);

                    double? sinAngle = 0;
                    if ((sinAngle = storedSin[i]) == null)
                        storedSin[i] = sinAngle = Math.Sin(angle);

                    i++;
                    double floatX = (double)c.radius * (double)cosAngle;
                    double floatY = (double)c.radius * (double)sinAngle;
                    int circX = (int)Math.Round(floatX);
                    if (c.x0 + circX >= 0 && c.x0 + circX < w && c.x0 - circX >= 0 && c.x0 - circX < w)
                    {
                        int circY = (int)Math.Round(floatY);
                        if (c.y0 + circY >= 0 && c.y0 + circY < h && c.y0 - circY >= 0 && c.y0 - circY < h)
                        {
                            radSum += inBase.Img[c.x0 + circX + (c.y0 + circY) * w];
                            radSum += inBase.Img[(c.x0 - circX) + (c.y0 - circY) * w];
                        }
                    }
                }
            return radSum;
        }

        /// <summary>
        /// 虹膜展开
        /// </summary>
        /// <param name="inBase"></param>
        /// <param name="outBase"></param>
        /// <param name="pupil"></param>
        /// <param name="iris"></param>
        /// <param name="unrollx"></param>
        /// <param name="unrolly"></param>
        public void IrisRoll(PICLib.GrayImg inBase, out PICLib.GrayImg outBase, Circle pupil, Circle iris, int unrollx, int unrolly)
        {
            int pcx = pupil.x0;
            int pcy = pupil.y0;
            int pr = pupil.radius;
            int icx = iris.x0;
            int icy = iris.y0;
            int ir = iris.radius;
            int rad = (int)Math.Round((float)((double)ir - Math.Sqrt((pcx - icx) * (pcx - icx) + (pcy - icy) * (pcy - icy))));
            int iw = inBase.Width;
            int ih = inBase.Height;

            PicBase.ImgMalloc(out outBase, unrollx, unrolly);

            for (int ww = 0; ww < unrollx; ww++)
            {
                for (int wh = 0; wh < unrolly; wh++)
                {
                    int irisx = (int)Math.Round((float)((double)(float)pcx + Math.Sin((double)((float)ww / (float)unrollx) * 6.2831853071795862D) * (double)((float)pr + ((float)wh / (float)unrolly) * ((float)rad - (float)pr))));
                    int irisy = (int)Math.Round((float)((double)(float)pcy + Math.Cos((double)((float)ww / (float)unrollx) * 6.2831853071795862D) * (double)((float)pr + ((float)wh / (float)unrolly) * ((float)rad - (float)pr))));
                    outBase.Img[ww + wh * unrollx] = inBase.Img[irisx + irisy * iw];
                }
            }
        }
    }

}
