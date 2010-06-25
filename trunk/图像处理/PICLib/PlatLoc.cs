using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;

namespace PICLib
{
    public struct extent
    {
        public int Start;
        public int End;
    }

    public struct PAngle
    {
        public double HAngle;
        public double VAngle;
    }

    /// <summary>
    /// 单位波形结构
    /// </summary>
    public struct UnitWave
    {
        public int Startw;
        public int Endw;
        public int GetWaveLen()
        {
            return Endw-Startw;
        }
    }

    public class PlateLoc
    {
        public int PltHMin = 20;                //过滤车牌最小高度
        public int PltHMax = 120;               //过滤车牌最大高度

        public int pltVMin = 80;                //过滤车牌最小宽度
        public int pltVMax = 270;               //车牌最大宽度

        public int HStartPosition = 20;         //过滤场启始高度
        public int HEndPosition = 288 - 60;     //过滤场结束高度

        public int PXStart = 50;                //过滤场左边开始宽度
        public int EPWidth = 170;               //预估计车牌宽度

        Process DipTool;
        #region 单例模式
        private static PlateLoc objSelf = null;
        private PlateLoc()
        {
            DipTool = Process.GetInstance();
        }
        public static PlateLoc GetInstance()
        {
            if (objSelf == null)
            {
                objSelf = new PlateLoc();
            }
            return objSelf;
        }
        #endregion

        #region 车牌方向投影
        /// <summary>
        /// 生成车牌水平投影
        /// </summary>
        /// <param name="pHProject"></param>
        /// <param name="pSrcImg"></param>
        /// <param name="FilterTH"></param>
        private void HProjectImg(ref int[] pHProject, GrayImg pSrcImg, int FilterTH)
        {
            //获取投影
            for (int i = 0; i < pSrcImg.Height; i++)
            {
                for (int j = 0; j < pSrcImg.Width; j++)
                {
                    if (pSrcImg.Img[i * pSrcImg.Width + j] == 255)
                        pHProject[i]++;
                }
            }

            //投影过滤
            for (int i = 0; i < pSrcImg.Height; i++)
            {
                if (pHProject[i] < FilterTH)
                    pHProject[i] = 0;
            }
            bool Flag1 = true;

            int Temp = 0;
            for (int i = 0; i < pSrcImg.Height; i++)
            {
                int avg = 0;
                if (pHProject[i] != 0 && Flag1)
                {
                    Temp = i;
                    Flag1 = false;
                }
                if (pHProject[i] == 0 && (!Flag1))
                {
                    for (int j = Temp; j <= i; j++)
                        avg = avg + pHProject[j];

                    avg = avg / (i - Temp);
                    if ((i - Temp < (int)(PltHMin * 0.35f + 0.5)) || (avg < (int)(PltHMax * 0.3f + 0.5f)))
                    {
                        for (int j = Temp; j < i; j++)
                            pHProject[j] = 0;
                    }
                    Flag1 = true;
                }
            }
        }


        /// <summary>
        /// 生成车牌垂直投影
        /// </summary>
        /// <param name="pVProject"></param>
        /// <param name="pSrcImg"></param>
        /// <param name="FilterTH"></param>
        private void VProjectImg(ref int[] pVProject, GrayImg pSrcImg, int FilterTH)
        {
            for (int i = 0; i < pSrcImg.Width; i++)
            {
                for (int j = 0; j < pSrcImg.Height; j++)
                {
                    if (pSrcImg.Img[j * pSrcImg.Width + i] == 255)
                        pVProject[i]++;
                }
            }
            //投影过滤
            for (int i = 0; i < pSrcImg.Width; i++)
            {
                if (pVProject[i] < FilterTH)
                    pVProject[i] = 0;
            }
        }
        #endregion

        #region 倾斜矫正
        /// <summary>
        /// 车牌校正
        /// </summary>
        public void RectifyLP(out GrayImg pDestImg, GrayImg pSrcImg, GrayImg pModelImg, ref PAngle Angle)
        {
            pDestImg = new GrayImg();

            GrayImg RowDiffImg;

            Diff(out RowDiffImg, pModelImg, "TPrjType.H_FILTER");
            HoughTrans(ref Angle, RowDiffImg, "TPrjType.V_FILTER");

            Diff(out RowDiffImg, pModelImg, "TPrjType.V_FILTER");
            HoughTrans(ref Angle, RowDiffImg, "TPrjType.H_FILTER");
            RectifyLP_2(out pDestImg, pSrcImg, Angle);

        }

        /// <summary>
        /// 倾斜度校正（新增）
        /// </summary>
        /// <param name="pDestImg"></param>
        /// <param name="pSrcImg"></param>
        /// <param name="Angle"></param>
        public void RectifyLP_2(out GrayImg pDestImg, GrayImg pSrcImg, PAngle Angle)
        {
            int i, j;

            double RectifyAngle = 0; ;
            int iAngle;

            GrayImg TempImg1, TempImg2;

            PicBase.ImgMalloc(out TempImg1, pSrcImg.Width, pSrcImg.Height);

            if (Math.Abs(Angle.HAngle + 90) != 90.0)
                RectifyAngle = -(Angle.HAngle + 90.0) / 180.0f * Math.PI;

            for (i = 0; i < pSrcImg.Width; i++)
                for (j = 0; j < pSrcImg.Height; j++)
                {
                    if (i * Math.Tan(RectifyAngle) < 0.0)
                        iAngle = (int)(i * Math.Tan(RectifyAngle) - 0.5);
                    else
                        iAngle = (int)(i * Math.Tan(RectifyAngle) + 0.5);

                    if ((j - iAngle >= 0) && (j - iAngle < pSrcImg.Height))
                    {
                        TempImg1.Img[j * TempImg1.Width + i] = pSrcImg.Img[(j - iAngle) * pSrcImg.Width + i];
                    }
                }

            PicBase.ImgMalloc(out TempImg2, pSrcImg.Width, pSrcImg.Height);

            if (Math.Abs(Angle.VAngle) != 90.0)
                RectifyAngle = Angle.VAngle / 180.0f * Math.PI;

            for (i = 0; i < TempImg1.Height; i++)
                for (j = 0; j < TempImg1.Width; j++)
                {
                    if (i * Math.Tan(RectifyAngle) < 0.0)
                        iAngle = (int)(i * Math.Tan(RectifyAngle) - 0.5);
                    else
                        iAngle = (int)(i * Math.Tan(RectifyAngle) + 0.5);
                    if ((j - iAngle >= 0) && (j - iAngle < TempImg1.Width))
                    {
                        TempImg2.Img[i * TempImg2.Width + j] = TempImg1.Img[i * TempImg1.Width + j - iAngle];
                    }
                }

            pDestImg.Width = TempImg2.Width;
            pDestImg.Height = TempImg2.Height;
            pDestImg.Img = TempImg2.Img;
        }

        public int Diff(out GrayImg pDestImg, GrayImg pSrcImg, string PrjType)
        {
            pDestImg = new GrayImg();
            int i, j;
            int Temp;

            GrayImg TempImg;

            PicBase.ImgMalloc(out TempImg, pSrcImg.Width, pSrcImg.Height);

            switch (PrjType)
            {

                case "TPrjType.H_FILTER":

                    for (i = 0; i < pSrcImg.Height; i++)
                        TempImg.Img[i * TempImg.Width] = 0;

                    for (i = 0; i < pSrcImg.Height; i++)
                        for (j = 1; j < pSrcImg.Width; j++)
                        {
                            Temp = Math.Abs(pSrcImg.Img[i * pSrcImg.Width + j] - pSrcImg.Img[i * pSrcImg.Width + j - 1]);
                            if (Temp > 80)
                                Temp = 255;
                            else
                                Temp = 0;

                            TempImg.Img[i * TempImg.Width + j] = (byte)Temp;
                        }

                    break;

                case "TPrjType.V_FILTER":

                    for (j = 0; j < pSrcImg.Width; j++)
                        TempImg.Img[j] = 0;

                    for (j = 0; j < pSrcImg.Width; j++)
                        for (i = 1; i < pSrcImg.Height; i++)
                        {
                            Temp = Math.Abs(pSrcImg.Img[i * pSrcImg.Width + j] - pSrcImg.Img[(i - 1) * pSrcImg.Width + j]);

                            if (Temp > 80)
                                Temp = 255;
                            else
                                Temp = 0;

                            TempImg.Img[i * TempImg.Width + j] = (byte)Temp;
                        }
                    break;

                default:
                    return 0;
            }

            pDestImg.Width = TempImg.Width;
            pDestImg.Height = TempImg.Height;
            pDestImg.Img = TempImg.Img;

            return 1;
        }
        /// <summary>
        /// Hough变换进行车牌校正
        /// </summary>
        /// 
        public int HoughTrans(ref PAngle pAngle, GrayImg pSrcImg, string PrjType)
        {
            int i, j, p, o, Max, Tempo;
            int[] H;
            int HWidth, HHeight;
            double temp;

            o = -90; p = 0;

            HHeight = 4 * pSrcImg.Width + 1;
            HWidth = 181;
            H = new int[HWidth * HHeight];

            for (i = -(HHeight / 2); i <= (HHeight / 2); i++)
                for (j = -(HWidth / 2); j <= (HWidth / 2); j++)
                    H[(i + HHeight / 2) * HWidth + j + HWidth / 2] = 0;

            switch (PrjType)
            {
                case "TPrjType.H_FILTER":

                    for (i = 0; i < pSrcImg.Height; i++)
                        for (j = 0; j < pSrcImg.Width; j++)
                        {
                            if (pSrcImg.Img[i * pSrcImg.Width + j] == 255)
                            {
                                for (o = -10; o <= 10; o++)
                                {
                                    if (o >= 0)
                                        Tempo = o + 80;
                                    else
                                        Tempo = o - 80;

                                    temp = j * Math.Cos(Tempo * Math.PI / 180.0f) + i * Math.Sin(Tempo * Math.PI / 180.0f);
                                    if (temp < 0.0)
                                        p = (int)(temp - 0.5);
                                    else
                                        p = (int)(temp + 0.5);

                                    if ((p + HHeight / 2 >= 0) && (p + HHeight / 2 < HHeight))
                                        H[(p + HHeight / 2) * HWidth + Tempo + HWidth / 2]++;
                                }
                            }
                        }
                    Max = 0;
                    for (i = -HHeight / 2; i <= HHeight / 2; i++)
                        for (j = -10; j <= 10; j++)
                        {
                            if (j >= 0)
                                Tempo = j + 80;
                            else
                                Tempo = j - 80;

                            if (Max < H[(i + HHeight / 2) * HWidth + Tempo + HWidth / 2])
                            {
                                Max = H[(i + HHeight / 2) * HWidth + Tempo + HWidth / 2];
                                o = Tempo;
                                p = i;
                            }
                        }
                    pAngle.HAngle = o;

                    break;

                case "TPrjType.V_FILTER":

                    for (i = 0; i < pSrcImg.Height; i++)
                        for (j = 0; j < pSrcImg.Width; j++)
                        {
                            if (pSrcImg.Img[i * pSrcImg.Width + j] == 255)
                            {

                                for (o = -10; o <= 10; o++)
                                {
                                    temp = j * Math.Cos(o * Math.PI / 180.0f) + i * Math.Sin(o * Math.PI / 180.0f);
                                    if (temp < 0.0)
                                        p = (int)(temp - 0.5);
                                    else
                                        p = (int)(temp + 0.5);

                                    if ((p + HHeight / 2 >= 0) && (p + HHeight / 2 < HHeight))
                                        H[(p + HHeight / 2) * HWidth + o + HWidth / 2]++;
                                }
                            }
                        }

                    Max = 0;

                    for (i = -HHeight / 2; i <= HHeight / 2; i++)
                        for (j = -10; j <= 10; j++)
                        {
                            if (Max < H[(i + HHeight / 2) * HWidth + j + HWidth / 2])
                            {
                                Max = H[(i + HHeight / 2) * HWidth + j + HWidth / 2];
                                o = j;
                                p = i;
                            }
                        }
                    pAngle.VAngle = o;
                    break;
                default:
                    return 0;
            }
            return 1;
        }
        #endregion

        #region 一次定位

        /// <summary>
        /// 一次精定位
        /// </summary>
        /// <param name="pSrcImg"></param>
        /// <returns></returns>
        public Rectangle GetLP_1(GrayImg pSrcImg)
        {
            Rectangle retRect = new Rectangle(0, 0, pSrcImg.Width, pSrcImg.Height);
            /*定义一个矩形区域*/
            int[] pHProject = new int[pSrcImg.Height];
            //获取垂直投影
            HProjectImg(ref pHProject, pSrcImg, pltVMin);
            //初判断车牌水平位置

            int pDownYPos = pSrcImg.Height, pUpYPos = 0;
            ArrayList WaveCollect = new ArrayList();

            bool Flag = true;
            UnitWave aWave = new UnitWave();
            for (int y = pDownYPos - 1; y >= pUpYPos; y--)
            {
                if (pHProject[y] != 0 && Flag)
                {
                    aWave.Endw = y;
                    Flag = false;
                }
                if (pHProject[y] == 0 && (!Flag))
                {
                    aWave.Startw = y;
                    WaveCollect.Add(aWave);
                    aWave = new UnitWave();
                    Flag = true;
                }
            }
            if (!Flag)
            {
                aWave.Startw = 0;
                WaveCollect.Add(aWave);
            }

            if (WaveCollect.Count <= 0)
            {
                return retRect;
            }

            /*开始过滤，过滤车牌靠上与靠下，与高度不够或高度过高的情况*/
            for (int i = WaveCollect.Count - 1; i >= 0; i--)
            {
                UnitWave aunit = (UnitWave)WaveCollect[i];
                if (aunit.Startw < HStartPosition || aunit.Endw > HEndPosition || aunit.GetWaveLen() < PltHMin || aunit.GetWaveLen() > PltHMax)
                {
                    WaveCollect.RemoveAt(i);
                }
            }

            int tmp = 0;
            /*开始针对每一组区域定位车牌水平区域*/
            for (int i = 0; i < WaveCollect.Count; i++)
            {
                UnitWave aunit = (UnitWave)WaveCollect[i];
                //获取区域
                Rectangle rect = new Rectangle(0, aunit.Startw, pSrcImg.Width, aunit.GetWaveLen());
                GrayImg tmpRect;
                Process.GetSubImg(out tmpRect, pSrcImg, rect);

                int[] pVProject = new int[tmpRect.Width];
                //计算过滤因子
                int th = (int)(0.16f * tmpRect.Height);
                VProjectImg(ref pVProject, tmpRect, th);

                int LDentisty = 0;
                int StartX = PXStart;

                for (int i1 = PXStart; i1 < pSrcImg.Width - EPWidth; i1++)
                {
                    if (pVProject[i1] == 0)
                    {
                        continue;
                    }
                    else
                    {
                        int tempdet = 0;
                        //计算区域线密度，并将最大线密度存入临时变量
                        for (int p = i1; p < i1 + EPWidth; p++)
                        {
                            if (pVProject[p] != 0)
                            {
                                tempdet++;
                            }
                        }
                        if (tempdet > LDentisty)
                        {
                            LDentisty = tempdet;
                            StartX = i1;
                        }
                    }
                }

                //获取区域
                Rectangle rect1 = new Rectangle(StartX, aunit.Startw, EPWidth, aunit.GetWaveLen());
                Process.GetSubImg(out tmpRect, pSrcImg, rect1);

                //对区域作二次投影分析
                int[] PVrect = new int[tmpRect.Width];
                th = (int)(0.16f * tmpRect.Height);
                VProjectImg(ref PVrect, tmpRect, th);
                //计算投影中有多少有效列
                int n = 0;
                for (int x = 0; x < tmpRect.Width; x++)
                {
                    if (PVrect[x] != 0)
                    {
                        n++;
                    }
                }
                int wfmax = (int)((float)tmpRect.Width * 1.0f);
                int wfmin = (int)((float)tmpRect.Width * 0.15f);
                //如果线过密或过稀疏，则不是车牌
                if (n > wfmax || n < wfmin)
                {
                    continue;
                }
                else
                {
                    if (n > tmp)
                    {
                        retRect = rect1;
                        tmp = n;
                    }
                }
            }

            if (retRect.Width != pSrcImg.Width)
            {
                retRect.X -= 10;
                retRect.Y -= 5;
                retRect.Width += 20;
                retRect.Height += 10;
            }

            return retRect;
        }

        #endregion      

        #region 二次定位

        /// <summary>
        /// 二次精确定位
        /// </summary>
        /// <param name="PicMap">传入图像</param>
        /// <returns></returns>
        public Rectangle GetSubImg(GrayImg PicMap)
        {
            int[] level, vertical;
            //GetWPojl(PicMap, out level);
            GetWPojv(PicMap, out vertical);
            EdgeY(PicMap, out level);
            Rectangle result = new Rectangle(vertical[0], level[0], vertical[1] - vertical[0], level[1] - level[0]);
            return result;
        }

        /// <summary>
        /// 确定车牌垂直边界
        /// </summary>
        /// <param name="PicMap"></param>
        /// <param name="Edge">垂直边界</param>
        public void EdgeY(GrayImg PicMap, out int[] Edge)
        {
            int Height = PicMap.Height;
            int Width = PicMap.Width;
            int MaxValue = 0;
            Edge = new int[2];
            Edge[1] = Height - 1;

            int[] PW = new int[Height];
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

            int low = 0;
            int high = Height - 1;
            MaxValue = (int)(MaxValue * 0.4);
            while (low != high && low < high)
            {
                if (PW[low] < MaxValue)
                {
                    Edge[0] = low;
                    low++;
                }
                else
                    low++;
                if (PW[high] < MaxValue)
                {
                    Edge[1] = high;
                    high--;
                }
                else
                    high--;
            }
        }

        /// <summary>
        /// 确定车牌垂直边界
        /// </summary>
        /// <param name="psi">待投影灰度图</param>
        /// <param name="PW">垂直边界</param>
        ///
        public void GetWPojl(GrayImg psi, out int[] Edge)
        {
            //int MaxValue = 0;
            int Width = psi.Width;
            int Height = psi.Height;
            int[] PW = new int[Height];
            int low = 0, high = Height - 1;
            double sum = 0;

            //垂直边界
            Edge = new int[2];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    PW[y] += psi.Img[y * Width + x];
                }
                if (PW[y] > sum)
                    sum = PW[y];
            }


            //for ( int i = 0; i < Height; i++)
            //    sum += PW[i];
            sum = sum * 0.35;

            while (low + high * 0.6 < high)
            {
                if (PW[low] < sum)
                {
                    Edge[0] = low;
                    low++;
                }
                else
                    low++;
                if (PW[high] < sum)
                {
                    Edge[1] = high;
                    high--;
                }
                else
                    high--;
            }

        }

        /// <summary>
        /// 确定车牌水平边界
        /// </summary>
        /// <param name="psi">待投影灰度图</param>
        /// <param name="PW">水平边界</param>
        ///
        public void GetWPojv(GrayImg psi, out int[] Edge)
        {
            //int MaxValue = 0;
            int Width = psi.Width;
            int Height = psi.Height;
            int[] PW = new int[Width];
            Edge = new int[2];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    PW[x] += psi.Img[y * Width + x];
                }
            }

            int i = 0;
            while (PW[i] < 255 * 5)
                i++;
            if (PW[i] != 0 && i < Width)
            {
                Edge[0] = i;
                i = Width - 1;
                while (PW[i] < 255 * 5 && i > 0)
                    i--;
                if (PW[i] != 0)
                    Edge[1] = i;
            }
            return;
        }

        #endregion

        #region 字符切割

        /// <summary>
        /// 使用Canny算子处理结果切割车牌区域
        /// </summary>
        /// <param name="pltImg1"></param>
        /// <param name="result"></param>
        static public GrayImg[] CutCharByCanny(GrayImg pltImg1, GrayImg result)
        {
            //将车牌区域放大至200*80
            Process.Zoomup(out result, result, 200, 80);
            Process.Zoomup(out pltImg1, pltImg1, 200, 80);
            //LoadSP(result, PictureBoxSizeMode.Normal, "放大");
            //LoadSP(pltImg1, PictureBoxSizeMode.Normal, "放大");


            ////对车牌区域水平变化率进行投影
            //Pictool.TransY(result, out width, out Height);
            //Pictool.GenWProjImgY(out result, width, Height);
            //LoadSP(result, PictureBoxSizeMode.Normal, "水平变化率投影");

            //投影所需参数
            int[] width;
            int Height;

            //对Canny算子处理结果(车牌区域)进行垂直投影
            Process.GetWPojX(result, out width, out Height);
            Process.GenWProjImgX(out result, width, Height);
            //diptool.Invert(out pltImg1, pltImg1);
            //Pictool.GetWPojX(pltImg1, out width, out Height);
            //Pictool.GenWProjImgX(out pltImg1, width, Height);
            //LoadSP(result, PictureBoxSizeMode.Normal, "垂直投影");



            ////二值化
            int t2 = Process.BinaryImg(pltImg1);
            Process.BinaryImg(pltImg1,out pltImg1, t2);
            //diptool.Zoomup(out pltImg1, pltImg1, 200, 40);
            //LoadSP(pltImg1, PictureBoxSizeMode.Normal, "二值化");

            //LoadSP(pltImg1, PictureBoxSizeMode.Normal, "原始");


            //对车牌区域进行闭运算
            //Closing(out pltImg1, pltImg1);
            ////Pictool.Dilation_8(pltImg1,out pltImg1);
            //LoadSP(pltImg1, PictureBoxSizeMode.Normal, "闭运算");


            GrayImg[] Chars;

            //固定卡位法切割
            ////Pictool.AccurateGetXLP3(width, pltImg1, out Chars);
            ////for (int i = 0; i < 7; i++)
            ////{
            ////    //diptool.GetSubImg(out Chars[i], pltImg1, new Rectangle(CharsRegion[i].Start, 0, CharLength[i], 80));
            ////    LoadSP(Chars[i], PictureBoxSizeMode.Normal, "切割后字符");
            ////}


            //使用Canny算子处理结算进行字符切割
            extent[] CharsRegion;
            CutChar(result, out CharsRegion, width);
            if (CharsRegion.Length < 7)
                Chars = new GrayImg[10];
            else
                Chars = new GrayImg[CharsRegion.Length + 3];

            //计算每个可能字符区域的宽度
            int[] CharLength = new int[CharsRegion.Length];
            for (int temp = 0; temp < CharsRegion.Length; temp++)
                CharLength[temp] = CharsRegion[temp].End - CharsRegion[temp].Start;
            int curr = 0;

            for (int i = 0; i < CharsRegion.Length; i++)
            {
                if (CharLength[i] > 110)
                {
                    Process.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start, 0, CharLength[i] / 5, pltImg1.Height));
                    //Pictool.Closing(out Chars[i], Chars[i]);
                    //LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");
                    curr++;

                    Process.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start + CharLength[i] / 5, 0, CharLength[i] / 5, pltImg1.Height));
                    //Pictool.Closing(out Chars[i], Chars[i]); 
                    //LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");
                    curr++;

                    Process.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start + 2 * CharLength[i] / 5, 0, CharLength[i] / 5, pltImg1.Height));
                    //Pictool.Closing(out Chars[i], Chars[i]); 
                    //LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");
                    curr++;

                    Process.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start + 3 * CharLength[i] / 5, 0, CharLength[i] / 5, pltImg1.Height));
                    //Pictool.Closing(out Chars[i], Chars[i]); 
                    //LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");
                    curr++;

                    Process.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start + 4 * CharLength[i] / 5, 0, CharLength[i] / 5, pltImg1.Height));
                    //Pictool.Closing(out Chars[i], Chars[i]); 
                    //LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");
                    curr++;
                }
                else if (CharLength[i] > 85)
                {
                    Process.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start, 0, CharLength[i] / 4, pltImg1.Height));
                    //Pictool.Closing(out Chars[i], Chars[i]);
                    //LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");
                    curr++;

                    Process.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start + CharLength[i] / 3, 0, CharLength[i] / 3, pltImg1.Height));
                    //Pictool.Closing(out Chars[i], Chars[i]); 
                    //LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");
                    curr++;

                    Process.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start + 2 * CharLength[i] / 4, 0, CharLength[i] / 4, pltImg1.Height));
                    //Pictool.Closing(out Chars[i], Chars[i]); 
                    //LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");
                    curr++;

                    Process.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start + 3 * CharLength[i] / 4, 0, CharLength[i] / 4, pltImg1.Height));
                    //Pictool.Closing(out Chars[i], Chars[i]); 
                    //LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");
                    curr++;
                }
                else if (CharLength[i] > 65)
                {
                    Process.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start, 0, CharLength[i] / 3, pltImg1.Height));
                    //Pictool.Closing(out Chars[i], Chars[i]);
                    //LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");
                    curr++;
                    Process.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start + CharLength[i] / 3, 0, CharLength[i] / 3, pltImg1.Height));
                    //Pictool.Closing(out Chars[i], Chars[i]); 
                    //LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");
                    curr++;
                    Process.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start + 2 * CharLength[i] / 3, 0, CharLength[i] / 3, pltImg1.Height));
                    //Pictool.Closing(out Chars[i], Chars[i]); 
                    //LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");
                    curr++;
                }
                else if (CharLength[i] > 35)
                {
                    Process.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start, 0, CharLength[i] / 2, pltImg1.Height));
                    //Pictool.Closing(out Chars[i], Chars[i]); 
                    //LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");
                    curr++;
                    Process.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start + CharLength[i] / 2, 0, CharLength[i] / 2, pltImg1.Height));
                    //Pictool.Closing(out Chars[i], Chars[i]); 
                    //LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");
                    curr++;
                }
                else if (CharLength[i] > 0)
                {
                    Process.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start, 0, CharLength[i], pltImg1.Height));
                    //Pictool.Closing(out Chars[i], Chars[i]); 
                    //LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");
                    curr++;
                }
            }


            return Chars;

            //////int curr = 0;
            ////////目前区域和下一个波形区域均不能构成一个字符
            //////while ((CharLength[i]+CharLength[i+1]< 18) &&  (i < (CharsRegion.Length - 1)))
            //////{
            //////    i++;
            //////}

            ////////目前区域能构成一个字符
            //////if (CharLength[i] > 18 && (i < (CharsRegion.Length - 1)))
            //////{
            //////    diptool.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start, 0, CharLength[i], 80));
            //////    LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");

            //////    i++;
            //////    curr++;
            //////}
            ////////目前区域和下一个波形区域能构成一个字符
            //////else if (CharLength[i] + CharLength[i + 1] > 18 && CharLength[i] + CharLength[i + 1] < 36 && (i < (CharsRegion.Length - 1)))
            //////{
            //////    diptool.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start, 0, CharLength[i] + CharLength[i + 1], 80));
            //////    LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");

            //////    i += 2;
            //////    curr++;
            //////}
            //////else
            //////    i++;

            ////////目前区域能构成一个字符
            //////if (CharLength[i] > 18 && (i < (CharsRegion.Length - 1)))
            //////{
            //////    diptool.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start, 0, CharLength[i], 80));
            //////    LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");

            //////    i++;
            //////    curr++;
            //////}
            ////////目前区域和下一个波形区域能构成一个字符
            //////else if (CharLength[i] + CharLength[i + 1] > 18 && CharLength[i] + CharLength[i + 1] < 30 && (i < (CharsRegion.Length - 1)))
            //////{
            //////    diptool.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start, 0, CharLength[i] + CharLength[i + 1], 80));
            //////    LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");

            //////    i += 2;
            //////    curr++;
            //////}

            //////while (curr < 7)
            //////{
            //////    if (CharsRegion[i].End - CharsRegion[i].Start > 30)
            //////    {
            //////        diptool.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start, 0, (CharsRegion[i].End - CharsRegion[i].Start) / 2, 80));
            //////        LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");
            //////        curr++;
            //////        diptool.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start + (CharsRegion[i].End - CharsRegion[i].Start) / 2, 0, (CharsRegion[i].End - CharsRegion[i].Start) / 2, 80));
            //////        LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");
            //////    }
            //////    else
            //////    {
            //////        diptool.GetSubImg(out Chars[curr], pltImg1, new Rectangle(CharsRegion[i].Start, 0, CharsRegion[i].End - CharsRegion[i].Start, 80));
            //////        LoadSP(Chars[curr], PictureBoxSizeMode.Normal, "切割后字符");
            //////        curr++;
            //////    }

            //////    i++;
            //////}



            //PlateE2.GrayImg[] Chars;
            //PlateE2.GrayImg[] Chars1;
            //Plttool.AccurateGetXLP3(PHCombine, R3tmp, out Chars1, gryImgtmp2, out Chars);

        }

        /// <summary>
        /// 以车牌图像为原始图像进行字符切割
        /// </summary>
        /// <param name="PicMap">Canny算子处理后结果</param>
        /// <param name="Result">各个字符水平起始位置</param>
        public GrayImg[] CutCharOld(GrayImg R2tmp)
        {

            GrayImg[] Chars;
            int[] PH2, PH3;

            //传入图像副本
            GrayImg R3tmp;

            //二值化
            int Yz1 = Process.BinaryImg(R2tmp);
            Process.BinaryImg(R2tmp, out R2tmp, Yz1);

            //对传入车牌图像进行闭运算
            Process.Closing_8(out R2tmp, R2tmp);

            PicBase.ImgClone(out R3tmp, R2tmp);

            ////反色
            //Invert(out R3tmp, R3tmp);

            //将车牌放大至标准
            Process.Zoomup(out R3tmp, R3tmp, 200, 40);
            Process.Zoomup(out R2tmp, R2tmp, 200, 40);
            //////LoadSP(R3tmp, PictureBoxSizeMode.Normal, "再放大至标准");
            //////LoadSP(gryImgtmp2, PictureBoxSizeMode.Normal, "再放大至标准");

            //对车牌区域进行垂直投影
            Process.GetHPoj(R2tmp, out PH2);

            //做水平边缘检测
            int[] TemplateArr2 = new int[9] { 1, 4, 1, 0, 0, 0, -1, -4, -1 };
            Process.Template(out R2tmp, R2tmp, TemplateArr2, 3, 1);

            //再二值化
            //二值化
            int Yz2 = Process.BinaryImg(R2tmp);
            Process.BinaryImg(R2tmp, out R2tmp, Yz2);
            //LoadSP(R2tmp, PictureBoxSizeMode.Normal, "水平边缘检测");


            Process.GetHPoj(R2tmp, out PH3);

            /*两个波形做一次与运算*/
            int[] PHCombine = new int[PH2.Length];

            for (int ws = 0; ws < PH2.Length; ws++)
            {
                //if (PH2[ws] != 0 && PH3[ws] != 0)
                if (Math.Abs(PH2[ws] - PH3[ws]) <= 18 && PH3[ws] > 4)
                    PHCombine[ws] = R2tmp.Height;
            }


            /*通过组合投影卡定车牌左定位*/
            LocVPlateLeft(out R3tmp, R3tmp, PHCombine, ref R2tmp);
            ////LoadSP(R3tmp, PictureBoxSizeMode.AutoSize, "垂直精确定位车牌：");
            ////LoadSP(gryImgtmp2, PictureBoxSizeMode.AutoSize, "原垂直精确定位车牌：");

            //int[] PH_Last1;
            //GetHPoj(R3tmp, out PH_Last1);

            /////*再边缘检测*/
            ////PlateE2.GrayImg RLast1tmp;
            ////int[] TemplateArr3 = new int[9] { 1, 2, 1, 0, 0, 0, -1, -2, -1 };
            ////Diptool.Template(out RLast1tmp, R3tmp, TemplateArr3, 3, 1);

            ////int[] PH_Last2;
            ////Plttool.GetHPoj(RLast1tmp, out PH_Last2);

            ////PlateE2.GrayImg[] Chars;
            ////Plttool.AccurateGetXLP2(PH_Last1, PH_Last2, R3tmp, out Chars);

            //GrayImg[] Chars;
            //GrayImg[] Chars1;

            //将车牌放大至标准
            Process.Zoomup(out R3tmp, R3tmp, 200, 40);
            Process.Zoomup(out R2tmp, R2tmp, 200, 40);

            //Invert(out R3tmp, R3tmp);
            AccurateGetXLP3(R3tmp, out Chars);

            return Chars;

        }

        /// <summary>
        /// 查找车牌中第一个字符
        /// </summary>
        /// <param name="pDestImg">处理后车牌图像</param>
        /// <param name="pSrcImg">传入车牌区域图像</param>
        /// <param name="PCombine">综合投影数组</param>
        /// <param name="pGryImg"></param>
        public void LocVPlateLeft(out GrayImg pDestImg, GrayImg pSrcImg, int[] PCombine, ref GrayImg pGryImg)
        {
            //搜索波形
            ArrayList StartX = new ArrayList();
            ArrayList EndX = new ArrayList();

            bool Flag = false;
            for (int i = 0; i < PCombine.Length; i++)
            {
                if (PCombine[i] != 0 && !Flag)
                {
                    StartX.Add(i);
                    Flag = true;
                }
                else if (PCombine[i] == 0 && Flag)
                {
                    EndX.Add(i);
                    Flag = false;
                }
                if (i == PCombine.Length - 1 && Flag)
                {
                    EndX.Add(i);
                    Flag = false;
                }
            }

            /*预估计车牌文字的最小宽度首部字母12，尾部字母为8*/
            for (int i = StartX.Count - 1; i >= 0; i--)
            {
                int wlen = (int)EndX[i] - (int)StartX[i];
                if (wlen < 4 && i <= 2)  //除掉此波形
                {
                    StartX.RemoveAt(i);
                    EndX.RemoveAt(i);
                }
                if (wlen < 3 && i > 2)  //除掉此波形
                {
                    StartX.RemoveAt(i);
                    EndX.RemoveAt(i);
                }
            }

            int sx = 0;
            int ex = PCombine.Length - 1;
            if (StartX.Count > 0)
            {
                sx = (int)StartX[0];
                ex = (int)EndX[EndX.Count - 1];
            }

            //CutImg(out pDestImg, pSrcImg, sx, ex, 0, pSrcImg.Height - 1);
            //CutImg(out pGryImg, pGryImg, sx, ex, 0, pSrcImg.Height - 1);
            Process.GetSubImg(out pDestImg, pSrcImg, new Rectangle(sx, 0, ex - sx, pSrcImg.Height));
            Process.GetSubImg(out pGryImg, pGryImg, new Rectangle(sx, 0, ex - sx, pSrcImg.Height));
        }

        /// <summary>
        /// 字符切割
        /// </summary>
        /// <param name="PicMap">传入车牌图像</param>
        /// <param name="CountRegion">字符所在水平区间</param>
        /// <param name="Value">canny算子处理后图像垂直投影数组</param>
        static public void CutChar(GrayImg PicMap, out extent[] CountRegion, int[] Value)
        {
            //Result = new GrayImg[7];
            int Length = Value.Length;
            int[] Count = new int[60];
            int curr = 1;
            int i;

            ////抹去第三个位置的小数点
            //for (i = 55; i < 80; i++)
            //{
            //    if (Value[i] < 10)
            //        Value[i] = 0;
            //}

            //for (int i = 0; i < Length; i++)
            //{
            //if (Value[i] == 0)
            //{
            //    Count[curr] = i;
            //    curr++;
            //    Count[0]++;
            //    i++;

            //    while (Value[i] == 0 && i < Length)
            //        i++;
            //    if (Value[i] != 0 && i < Length)
            //    {
            //        Count[curr] = i;
            //        curr++;
            //    }


            //}
            //else
            //    i++;

            //}

            i = 0;
            while (i < Length)
            {
                //寻找可能为字符区域的左边界
                while (i < Length && Value[i] == 0)
                    i++;
                if (i < Length)
                {
                    Count[curr] = i;
                    curr++;
                    Count[0]++;
                    i++;
                }
                else
                    break;

                //寻找为可能为字符区域的右边界
                while (i < Length && Value[i] != 0)
                    i++;
                if (i < Length)
                {
                    Count[curr] = i - 1;
                    curr++;
                    //Count[0]++;
                }
                else
                    break;
            }


            CountRegion = new extent[Count[0]];
            for (i = 0; i < Count[0]; i++)
            {
                CountRegion[i].Start = Count[2 * i + 1];
                CountRegion[i].End = Count[2 * i + 2];
            }
            if (CountRegion[Count[0] - 1].End == 0)
                CountRegion[Count[0] - 1].End = Value.Length - 1;

            //去除大边缘小面积噪音
            i = 0;
            while (i < CountRegion.Length && CountRegion[i].End - CountRegion[i].Start < 8)
            {
                CountRegion[i].End = CountRegion[i].Start;
                i++;
            }

            //去除左边缘较大面积噪音
            if (i < CountRegion.Length && CountRegion[i].End - CountRegion[i].Start < 13 && CountRegion[i + 1].End - CountRegion[i + 1].Start > 17)
            {
                CountRegion[i].End = CountRegion[i].Start;
                i++;
            }

            //处理字母被切割成两部分
            i++;
            if (i < CountRegion.Length - 1 && CountRegion[i + 1].End - CountRegion[i].Start > 18 && CountRegion[i + 1].End - CountRegion[i].Start < 36 && CountRegion[i].End - CountRegion[i].Start < 18)
            {
                CountRegion[i].End = CountRegion[i + 1].End;
                CountRegion[i + 1].End = CountRegion[i + 1].Start;
                i++;
            }
            i++;
            if (i < CountRegion.Length && CountRegion[i].End - CountRegion[i].Start < 10 && CountRegion[i].End - CountRegion[i].Start > 0)
            {
                int sum = 0;
                for (int temp = CountRegion[i].Start; temp <= CountRegion[i].End; temp++)
                {
                    sum += Value[temp];
                }
                sum /= (CountRegion[i].End - CountRegion[i].Start);
                if (sum < 10)
                {
                    CountRegion[i].End = CountRegion[i].Start;
                    i++;
                }
            }

            while (i < CountRegion.Length)
            {
                if (CountRegion[i].End - CountRegion[i].Start < 8 && CountRegion[i].End - CountRegion[i].Start != 0)
                {
                    int sum = 0;
                    for (int temp = CountRegion[i].Start; temp <= CountRegion[i].End; temp++)
                    {
                        sum += Value[temp];
                    }
                    sum /= (CountRegion[i].End - CountRegion[i].Start);
                    if (sum < 10)
                    {
                        CountRegion[i].End = CountRegion[i].Start;
                        i++;
                    }
                    else
                        i++;
                }
                else
                    i++;
            }


            //i = 0;
            //while (!(CountRegion[i].End > 50))
            //    i++;
            //int CountLeft =0;
            ////寻找前两个字符区间
            //for (int Now = i; curr > 0; curr--)
            //{
            //    //区域不足以构成一个字符
            //    if (CountRegion[Now].End - CountRegion[Now].Start < 18 && Now > 0)
            //    {
            //        CountRegion[Now - 1].End = CountRegion[Now].End;
            //        CountRegion[Now].Start = CountRegion[Now].End;
            //        CountLeft++;
            //    }
            //    else if (CountLeft == 2)
            //    {
            //        CountRegion[Now].End = CountRegion[Now].Start;
            //    }
            //    else
            //        CountLeft++;
            //}
            ////处理第三个区域为小数点情况
            //i++;
            //while (CountRegion[i].End - CountRegion[i].Start < 10 && CountRegion[i].End - CountRegion[i].Start > 0)
            //{
            //    int Avg = 0;
            //    for (int temp = CountRegion[i + 1].Start; temp <= CountRegion[i + 1].End; temp++)
            //        Avg += Value[temp];
            //    Avg /= (CountRegion[i].End - CountRegion[i].Start);
            //    if (Avg < 15)
            //        i++;
            //}


            ////寻找后五个字符区间
            //i += 5;
            //while (i < CountRegion.Length)
            //{
            //    CountRegion[i].End = CountRegion[i].Start;
            //    i++;
            //}
        }

        /// <summary>
        /// 固定卡出字符区域
        /// </summary>
        /// <param name="PCombine">投影数组</param>
        /// <param name="pSrcImg">待切割图像</param>
        /// <param name="pDestImgs">切割后图像</param>
        /// <param name="pGrayImg"></param>
        /// <param name="outchargry"></param>
        public void AccurateGetXLP3(GrayImg pSrcImg, out GrayImg[] pDestImgs/*, GrayImg pGrayImg, out GrayImg[] outchargry*/)
        {
            pDestImgs = new GrayImg[7];
            int Height = pSrcImg.Height;

            /*统计*/
            int[] PH;
            Process.GetHPoj(pSrcImg, out PH);

            //int cut1 = 18, cut2 = 50, cut3 = 85, cut4 = 115, cut5 = 125, cut6 = 165;

            int[] cut = { 25, 48, 85, 115, 135, 155 }; //
            int[] cutd = { 35, 66, 95, 125, 145, 175 }; //
            for (int i = 0; i < 6; i++)
            {
                int low = PH[cut[i]];
                for (int c = cut[i]; c <= cutd[i]; c++)
                {
                    if (PH[c] < low)
                    {
                        low = PH[c];
                        cut[i] = c;
                    }
                }
            }

            //CutImg(out pDestImgs[0], pSrcImg, 0, cut[0], 1, 39);
            //CutImg(out pDestImgs[1], pSrcImg, cut[0], cut[1], 1, 39);
            //CutImg(out pDestImgs[2], pSrcImg, cut[1], cut[2], 1, 39);
            //CutImg(out pDestImgs[3], pSrcImg, cut[2], cut[3], 1, 39);
            //CutImg(out pDestImgs[4], pSrcImg, cut[3], cut[4], 1, 39);
            //CutImg(out pDestImgs[5], pSrcImg, cut[4], cut[5], 1, 39);
            //CutImg(out pDestImgs[6], pSrcImg, cut[5], 196, 1, 39);
            Process.GetSubImg(out pDestImgs[0], pSrcImg, new Rectangle(0, 0, cut[0], Height));
            Process.GetSubImg(out pDestImgs[1], pSrcImg, new Rectangle(cut[0], 0, cut[1] - cut[0], Height));
            Process.GetSubImg(out pDestImgs[2], pSrcImg, new Rectangle(cut[1], 0, cut[2] - cut[1], Height));
            Process.GetSubImg(out pDestImgs[3], pSrcImg, new Rectangle(cut[2], 0, cut[3] - cut[2], Height));
            Process.GetSubImg(out pDestImgs[4], pSrcImg, new Rectangle(cut[3], 0, cut[4] - cut[3], Height));
            Process.GetSubImg(out pDestImgs[5], pSrcImg, new Rectangle(cut[4], 0, cut[5] - cut[4], Height));
            Process.GetSubImg(out pDestImgs[6], pSrcImg, new Rectangle(cut[5], 0, pSrcImg.Width - 1 - cut[5], Height));
            //GetSubImg(out pDestImgs[1], pSrcImg, new Rectangle(cut[0], 0, cut[1] - cut[0], Height));
            //outchargry = new GrayImg[7];
            //CutImg(out outchargry[0], pGrayImg, 0, cut[0], 1, 39);
            //CutImg(out outchargry[1], pGrayImg, cut[0], cut[1], 1, 39);
            //CutImg(out outchargry[2], pGrayImg, cut[1], cut[2], 1, 39);
            //CutImg(out outchargry[3], pGrayImg, cut[2], cut[3], 1, 39);
            //CutImg(out outchargry[4], pGrayImg, cut[3], cut[4], 1, 39);
            //CutImg(out outchargry[5], pGrayImg, cut[4], cut[5], 1, 39);
            //CutImg(out outchargry[6], pGrayImg, cut[5], 196, 1, 39);
        }

        #endregion

        #region 备用
        ///// <summary>
        ///// 全图垂直投影
        ///// </summary>
        ///// <param name="psi"></param>
        ///// <param name="PW"></param>
        //public void GetWPoj(GrayImg psi, out int[] PW)
        //{
        //    PW = new int[psi.Height];
        //    for (int y = 0; y < psi.Height; y++)
        //    {
        //        for (int x = 0; x < psi.Width; x++)
        //        {
        //            PW[y] += psi.Img[y * psi.Width + x];
        //        }
        //        PW[y] = PW[y] / 255;         //白点个数
        //    }
        //}

        ////全图水平投影过滤
        //public void HPojFilter(ref int[] PH, int Height, float FilterScale)
        //{
        //    int CScale = (int)((float)Height * FilterScale);
        //    for (int x = 0; x < PH.Length; x++)
        //    {
        //        if (PH[x] < CScale)
        //            PH[x] = 0;
        //    }
        //}

        //public void PojFilter(ref int[] PH, int Min, int Max)
        //{
        //    for (int x = 0; x < PH.Length; x++)
        //    {
        //        if (PH[x] < Min || PH[x] > Max)
        //            PH[x] = 0;
        //    }
        //}
        ////public Rectangle GetLP_2(GrayImg pSrcImg)
        ////{

        ////}
        #endregion
    }
}
