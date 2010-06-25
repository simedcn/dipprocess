using System;
using System.Collections.Generic;
using System.Text;

namespace PICLib
{
    public class IrisCode
    {
        private Process Diptool;
        public int width;
        public int height;
        public byte[] code;

        public IrisCode clone()
        {
            IrisCode pDest = new IrisCode();
            pDest.width = this.width;
            pDest.height = this.height;
            pDest.code = (byte[])this.code.Clone();
            return pDest;
        }

        public IrisCode(byte[] Rcodein, int widthin, int heightin)
        {
            code = Rcodein;
            width = widthin;
            height = heightin;
            Diptool = Process.GetInstance();
        }

        public IrisCode(string strin)
        {
            string[] sep = strin.Split(',');
            width = int.Parse(sep[0]);
            height = int.Parse(sep[1]);

            code = new byte[sep[2].Length];
            int l = sep[2].Length;

            for (int i = 0; i < l; i++)
                if (sep[2][i] == '0')
                    code[i] = 0;
                else
                    code[i] = 255;
            Diptool = Process.GetInstance();
        }

        public IrisCode()
        {
            Diptool = Process.GetInstance();
        }

        public string toString()
        {
            string strout = "";
            strout = width.ToString() + "," + height.ToString() + ",";
            int l = code.Length;

            for (int i = 0; i < l; i++)
                if (code[i] == 255)
                    strout = strout + "1";
                else
                    strout = strout + "0";
            return strout;
        }

        public string CodeToStr()
        {
            string strout = "";
            //削减编码，将编码变为01编码，再将编码做16进制运算。
            byte[] newbytelist = normalizecode(code);
            return ToHexString(newbytelist);
        }

        public byte[] NewCode()
        {
            return normalizecode(code);
        }

        private byte[] normalizecode(byte[] code)
        {
            int len = code.GetLength(0);
            byte[] newcode = new byte[(len + 7) / 8];
            for (int i = 0; i < len; i += 8)
            {
                for (int j = i; j < i + 8; j++)
                {
                    //if (code[j] != 0)
                    //{
                    //}
                    newcode[i / 8] = (byte)((newcode[i / 8] << 1) + code[j] / 255);
                }
            }
            return newcode;
        }

        private string ToHexString(byte[] bytes)
        {
            StringBuilder s = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                s.Append(b.ToString("X2"));
            }

            //return s.Replace("00000", "").ToString();
            //去掉前后130
            return s.ToString().Substring(130, s.Length - 260);
        }

        public GrayImg toBaseImage()
        {
            GrayImg outBase = new GrayImg();
            PicBase.ImgMalloc(out outBase, width, height);

            int l = code.Length;

            for (int i = 0; i < l; i++)
                outBase.Img[i] = code[i];
            return outBase;
        }
    }

    public class FeatureCoding
    {
        private static FeatureCoding objSelf = null;
        private FeatureCoding()
        { }
        public static FeatureCoding GetInstance()
        {
            if (objSelf == null)
                objSelf = new FeatureCoding();
            return objSelf;
        }
    }
}
