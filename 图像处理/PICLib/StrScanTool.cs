using System;
using System.Collections.Generic;
using System.Text;

namespace PICLib
{
    public class StrScanTool
    {
        /*判断一个输入字符是否为字符*/
        public static bool isCharactor(char InputChar)
        {
            if ((InputChar < 97 || InputChar > 122) && (InputChar < 65 || InputChar > 90))
                return false;
            else
                return true;
        }
        /*功能重载*/
        public static bool isCharactor(string InputStr)
        {
            foreach (char InputChar in InputStr.ToCharArray())
            {
                if (!isCharactor(InputChar))
                    return false;
            }
            return true;
        }
        /*判断一个输入字符是否是数字*/
        public static bool isDigit(char InputChar)
        {
            if (InputChar < 48 || InputChar > 57)
                return false;
            else
                return true;
        }
        public static bool isDigit(string InputStr)
        {
            foreach (char InputChar in InputStr.ToCharArray())
            {
                if (!isDigit(InputChar))
                    return false;
            }
            return true;
        }
        /*判断一个输入符是否是一个数*/
        public static bool isANumber(string InputStr)
        {
            try
            {
                double a = double.Parse(InputStr);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /*判断输入字符是否是字符或是数字*/
        public static bool isCorD(char InputChar)
        {
            if (isCharactor(InputChar) || isDigit(InputChar))
                return true;
            else
                return false;
        }
        public static bool isCorD(string InputStr)
        {
            foreach (char achar in InputStr.ToCharArray())
            {
                if (!isCharactor(achar) && !isDigit(achar))
                    return false;
            }
            return true;
        }
        /*判断一个输入串是否符合做变量的标准，以字母开头，串中不可以有异常字符*/
        public static bool isVarName(string InputStr)
        {
            if (isCorD(InputStr))
            {
                if (isCharactor(InputStr.Substring(0, 1)))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        /*判断是否是匹配字符*/
        public static bool isBracket(char InputChar, char CompareChar)
        {
            if (InputChar == CompareChar)
                return true;
            else
                return false;
        }
        public static bool isBracket(string InputStr, string CompareStr)
        {
            if (InputStr == CompareStr)
                return true;
            else
                return false;
        }
        /*从左到右提取相邻两个匹配串的所有字符*/
        public static string PickNear(string InputStr, string MatchLeft, string MatchRight)
        {
            int left = InputStr.IndexOf(MatchLeft) + MatchLeft.Length;
            int right = InputStr.IndexOf(MatchRight);
            return InputStr.Substring(left, right - left);
        }
        /*从头计算在串中连续数字和字母的长度*/
        public static int RetStrLen(string InputStr)
        {
            int Ptr = 0;
            foreach (char aChar in InputStr.ToCharArray())
            {
                if (isCorD(aChar))
                {
                    Ptr++;
                }
                else
                    return Ptr;
            }
            return Ptr;
        }
        /*计算一个串里，去掉所有的空格后的串长*/
        public static int StrLenTrim(string InputStr)
        {
            int p = 0;
            char[] token = InputStr.ToCharArray();
            for (int i = 0; i < InputStr.Length; i++)
            {
                if (token[i] != ' ')
                    p++;
            }
            return p;
        }
        /*去掉一个串内所有多余的空字符*/
        public static string TrimAllBlank(string InputStr)
        {
            string Str = "";
            char[] token = InputStr.ToCharArray();
            for (int i = 0; i < InputStr.Length; i++)
            {
                if (token[i] != ' ')
                    Str += token[i].ToString();
            }
            return Str;
        }
        /*从头部去一次匹配字符串*/
        public static string TrimHead(string InputStr, string RemoveStr)
        {
            if (InputStr.IndexOf(RemoveStr) == 0)
            {
                InputStr = InputStr.Remove(0, RemoveStr.Length);
            }
            return InputStr;
        }
        /*去掉尾部一次匹配字符串*/
        public static string TrimTail(string InputStr, string RemoveStr)
        {
            if (InputStr.LastIndexOf(RemoveStr) == -1)
                return InputStr;
            if (InputStr.LastIndexOf(RemoveStr) == InputStr.Length - RemoveStr.Length)
            {
                InputStr = InputStr.Remove(InputStr.Length - RemoveStr.Length, RemoveStr.Length);
            }
            return InputStr;
        }
    }
}
