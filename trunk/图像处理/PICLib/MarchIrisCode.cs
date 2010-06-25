using System;
using System.Collections.Generic;
using System.Text;

namespace PICLib
{
    public class MarchIrisCode
    {
        /// <summary>
        /// 两组虹膜匹配，返回虹膜匹配率
        /// </summary>
        /// <param name="pSrcIris"></param>
        /// <param name="pDestIris"></param>
        /// <returns></returns>
        public static float MarchIirs_2(byte[] pSrcIris, byte[] pDestIris)
        {
            int len = pSrcIris.GetLength(0);
            byte byteResult = 0;
            int MatchCount = 0;
            for (int i = 0; i < len; i++)
            {
                byteResult = (byte)(pSrcIris[i] & pDestIris[i]);
                if (byteResult != 0)
                {
                }
                MatchCount += CountNumofOne(byteResult);
            }
            float Drate = (float)MatchCount / (float)(len * 8);
            return Drate;
        }

        //计算二进制字节中1的个数
        public static int CountNumofOne(byte abyte)
        {
            int count = 0;
            byte tmp = abyte;
            byte one = 0x00000001;
            for (int i = 0; i < 8; i++)
            {
                count += tmp & one;
                tmp = (byte)(tmp >> 1);
            }
            return count;
        }
    }
}
