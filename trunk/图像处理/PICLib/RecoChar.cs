using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.IO;

namespace PICLib
{
    public struct FontStru
    {
        public Hashtable Number;
        public Hashtable Alph;
        public Hashtable Character;
    }
    public struct MyRect
    {
        /// <summary>
        /// 区域左
        /// </summary>
        public int Left;
        /// <summary>
        /// 区域右
        /// </summary>
        public int Right;
        /// <summary>
        /// 区域顶
        /// </summary>
        public int Top;
        /// <summary>
        /// 区域底
        /// </summary>
        public int Bottom;
    }
    public class RecoChar
    {
        Process dipTools;

        FontStru Lib;
        private static RecoChar objself = null;
        private RecoChar()
        {
            dipTools = Process.GetInstance();
            Lib = new FontStru();
            LoadMaster();
        }
        public static RecoChar GetInstance()
        {
            if (objself == null)
            {
                objself = new RecoChar();
            }
            return objself;
        }


        public int GetStdImg(out GrayImg pDestImg, GrayImg pModelImg, GrayImg pSrcImg, int aLeft, int aRight, int aTop, int aBottom)
        {
            pDestImg = new GrayImg();
            int i, j, res = 0;
            MyRect Rect1;
            GrayImg TempImg;
            bool bFlag = false;

            Rect1.Top = 0;
            Rect1.Bottom = pSrcImg.Height - 1;
            Rect1.Left = 0;
            Rect1.Right = pSrcImg.Width - 1;
            bFlag = false;
            int ca = 0;

            for (i = 0; i < pSrcImg.Height; i++)
            {
                for (j = 0; j < pSrcImg.Width; j++)
                    //if (*(pSrcImg->pImg + i * pSrcImg->Width + j) == 255)
                    if (pSrcImg.Img[i * pSrcImg.Width + j] == 255)
                    {
                        if (ca >= aTop)
                        {
                            Rect1.Top = i;
                            bFlag = true;
                            break;
                        }
                        else
                            ca++;
                    }
                if (bFlag)
                    break;
            }


            bFlag = false;
            ca = 0;
            for (i = pSrcImg.Height - 1; i >= 0; i--)
            {
                for (j = 0; j < pSrcImg.Width; j++)
                    //if (*(pSrcImg->pImg + i * pSrcImg->Width + j) == 255)
                    if (pSrcImg.Img[i * pSrcImg.Width + j] == 255)
                    {
                        if (ca >= aBottom)
                        {
                            Rect1.Bottom = i;
                            bFlag = true;
                            break;
                        }
                        else
                            ca++;
                    }
                if (bFlag)
                    break;
            }

            bFlag = false;
            ca = 0;
            for (j = 0; j < pSrcImg.Width; j++)
            {
                for (i = 0; i < pSrcImg.Height; i++)
                    //if (*(pSrcImg->pImg + i * pSrcImg->Width + j) == 255)
                    if (pSrcImg.Img[i * pSrcImg.Width + j] == 255)
                    {
                        if (ca > aLeft)
                        {
                            Rect1.Left = j;
                            bFlag = true;
                            break;
                        }
                        else
                            ca++;
                    }
                if (bFlag)
                    break;
            }

            bFlag = false;
            ca = 0;
            for (j = pSrcImg.Width - 1; j >= 0; j--)
            {
                for (i = 0; i < pSrcImg.Height; i++)
                    //if (*(pSrcImg->pImg + i * pSrcImg->Width + j) == 255)
                    if (pSrcImg.Img[i * pSrcImg.Width + j] == 255)
                    {
                        if (ca > aRight)
                        {
                            Rect1.Right = j;
                            bFlag = true;
                            break;
                        }
                        else
                            ca++;
                    }
                if (bFlag)
                    break;
            }

            res = PicBase.ImgMalloc(out TempImg, Rect1.Right - Rect1.Left + 1, Rect1.Bottom - Rect1.Top + 1);
            if (0 == res)
                return 0;

            for (i = 0; i < TempImg.Height; i++)
                for (j = 0; j < TempImg.Width; j++)
                {
                    //*(TempImg.pImg + i * TempImg.Width + j) =*(pSrcImg->pImg + (i + Rect.top) * pSrcImg->Width + (j + Rect.left));
                    TempImg.Img[i * TempImg.Width + j] = pModelImg.Img[(i + Rect1.Top) * pModelImg.Width + (j + Rect1.Left)];
                }

            pDestImg.Width = TempImg.Width;
            pDestImg.Height = TempImg.Height;
            pDestImg.Img = TempImg.Img;

            return 1;
        }

        /// <summary>
        /// 切割图片提取特征码
        /// </summary>
        /// <param name="SW">横向切割条数，一般情况下将切割分为20×20</param>
        /// <param name="SH">纵向切割条数</param>
        public int Eigenvalue(int SW, int SH, out byte[] EMatrix, GrayImg inBase)
        {
            float Wsmall = (float)inBase.Width / (float)SW;
            float Hsmall = (float)inBase.Height / (float)SH;
            EMatrix = new byte[SW * SH];

            for (int y = 0; y < SH; y++)
            {
                for (int x = 0; x < SW; x++)
                {
                    /*统计黑点个数，超过40%则将其定为1*/
                    int cout = 0;
                    for (int y1 = (int)(y * Hsmall); y1 < (int)((y + 1) * Hsmall); y1++)
                    {
                        for (int x1 = (int)(x * Wsmall); x1 < (int)((x + 1) * Wsmall); x1++)
                        {
                            if (inBase.Img[y1 * inBase.Width + x1] == 255)
                                cout++;
                        }
                    }
                    if (cout >= Wsmall * Hsmall / 2)
                        EMatrix[y * SW + x] = 1;
                    else
                        EMatrix[y * SW + x] = 0;
                }
            }
            return 1;
        }

        public int LoadMaster()
        {
            string LibFile = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\")) + @"\FEvalue.dat";

            Lib.Alph = new Hashtable();
            Lib.Character = new Hashtable();
            Lib.Number = new Hashtable();

            /*从文件中读取字模，并将字模存入内存容器*/
            if (File.Exists(LibFile))
            {
                StreamReader sr = new StreamReader(LibFile);
                string readstr = null;
                while ((readstr = sr.ReadLine()) != null)
                {
                    string[] token = readstr.Split('=');
                    //MasterBase m1 = new MasterBase();
                    //m1.CharaStr = token[0].Trim();
                    //m1.Evalue = Encoding.GetEncoding("UTF-8").GetBytes(token[1].Trim());
                    string C = token[0].Trim();
                    byte[] E = Encoding.GetEncoding("UTF-8").GetBytes(token[1].Trim());

                    if (StrScanTool.isDigit(C))
                        Lib.Number.Add(E, C);
                    else if (StrScanTool.isCharactor(C))
                        Lib.Alph.Add(E, C);
                    else
                        Lib.Character.Add(E, C);
                }
                sr.Close();
            }
            return 1;
        }

        /// <summary>
        /// 匹配汉字
        /// </summary>
        /// <param name="inBase">输入待匹配的模版基</param>
        public void MatchChr(byte[] Evalue, out string Achar, out double f)
        {
            int diff0 = Evalue.Length;
            f = 0;
            Achar = "";
            foreach (byte[] m1 in Lib.Character.Keys)
            {
                int diff1 = Differ(m1, Evalue);
                if (diff1 < diff0)
                {
                    Achar = Lib.Character[m1].ToString();
                    f = ((double)(Evalue.Length - diff1)) / ((double)Evalue.Length);
                    diff0 = diff1;
                }
            }
        }

        private int Differ(byte[] m1, byte[] m2)
        {
            int d = 0;
            for (int i = 0; i < m1.Length; i++)
            {
                if (m1[i] != m2[i])
                    d++;
            }
            return d;
        }

        /// <summary>
        /// 匹配字母
        /// </summary>
        /// <param name="inBase">输入的待匹配的模版基</param>
        public void MatchAlph(byte[] Evalue, out string Achar, out double f)
        {
            int diff0 = Evalue.Length;
            f = 0;
            Achar = "";
            foreach (byte[] m1 in Lib.Alph.Keys)
            {
                int diff1 = Differ(m1, Evalue);
                if (diff1 <= diff0)
                {
                    Achar = Lib.Alph[m1].ToString();
                    f = ((double)(Evalue.Length - diff1)) / ((double)Evalue.Length);
                    diff0 = diff1;
                }
            }
        }

        /// <summary>
        /// 匹配数字
        /// </summary>
        /// <param name="inBase">输入的待匹配的模版基</param>
        public void MatchNum(byte[] Evalue, out string Achar, out double f)
        {
            int diff0 = Evalue.Length;
            f = 0;
            Achar = "";
            foreach (byte[] m1 in Lib.Number.Keys)
            {
                int diff1 = Differ(m1, Evalue);
                if (diff1 < diff0)
                {
                    Achar = Lib.Number[m1].ToString();
                    f = ((double)(Evalue.Length - diff1)) / ((double)Evalue.Length);
                    diff0 = diff1;
                }
            }
        }

        public void MatchAN(byte[] Evalue, out string Achar, out double f)
        {
            double f1, f2;
            string Achar1, Achar2;
            MatchNum(Evalue, out Achar1, out f1);
            MatchAlph(Evalue, out Achar2, out f2);
            if (f1 > f2)
            {
                f = f1;
                Achar = Achar1;
            }
            else
            {
                f = f2;
                Achar = Achar2;
            }
        }

        public void MatchAll(byte[] Evalue, out string Achar, out double f)
        {
            f = 0.0;
            Achar = "";
            double f1, f2, f3;
            string Achar1, Achar2, Achar3;
            MatchNum(Evalue, out Achar1, out f1);
            MatchAlph(Evalue, out Achar2, out f2);
            MatchChr(Evalue, out Achar3, out f3);
            if (f1 >= f2 && f1 >= f3)
            {
                f = f1;
                Achar = Achar1;
            }
            else if (f2 >= f1 && f2 >= f3)
            {
                f = f2;
                Achar = Achar2;
            }
            else if (f3 >= f1 && f3 >= f2)
            {
                f = f3;
                Achar = Achar3;
            }
        }
    }
}
