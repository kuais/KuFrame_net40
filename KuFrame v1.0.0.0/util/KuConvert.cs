using System;
using System.Text;

namespace Ku.util
{
    public class KuConvert
    {
        /// <summary>
        /// 8位十进制数据转换成维根26编码,输入数据前3位必须小于256
        /// </summary>
        /// <param name="strData">十进制数据</param>
        /// <returns>维根26编码</returns>
        public static byte[] DecToVG26(string strData)
        {
            int intTemp;
            while (strData.Length < 8)
            {
                strData = "0" + strData;
            }
            byte[] bytData = new byte[3];
            bytData[0] = byte.Parse(strData.Substring(0, 3));
            intTemp = int.Parse(strData.Substring(3));
            bytData[1] = (byte)(intTemp / 256);
            bytData[2] = (byte)(intTemp % 256);
            return bytData;
        }

        /// <summary>
        /// 维根26编码转换成十进制
        /// </summary>
        /// <param name="bytData"></param>
        /// <returns></returns>
        public static string VG26ToDec(byte[] bytData)
        {
            string strData = "";
            string strTemp = "";
            strData = bytData[0].ToString();
            while (strData.Length < 3)
            {
                strData = "0" + strData;
            }
            strTemp = (bytData[1] * 256 + bytData[2]).ToString();
            while (strTemp.Length < 5)
            {
                strTemp = "0" + strTemp;
            }
            strData += strTemp;
            return strData;
        }

        /// <summary>
        /// 将字节数组转为16进制字符串显示
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string[] HexFromDec(byte[] p)
        {
            string[] s = new string[p.Length];
            for (int i = 0; i < p.Length; i++)
            {
                s[i] = p[i].ToString("X2");
            }
            return s;
        }
        /// <summary>
        /// 将字节数组转为16进制字符串显示
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static byte[] HexToDec(string[] p)
        {
            byte[] s = new byte[p.Length];
            for (int i = 0; i < p.Length; i++)
            {
                s[i] = Convert.ToByte(p[i], 16);
            }
            return s;
        }

        public static byte IntToBcd(int p)
        {
            return (byte)((p / 10) * 16 + (p % 10));
        }
        public static byte[] IntToBcd(int p, int count)
        {
            byte[] result = new byte[count];
            int i = 0;
            while (i < count)
            {
                if (p <= 0) break;
                result[i++] = IntToBcd(p % 100);
                p /= 100;
            }
            return result;
        }
        public static int BcdToInt(int data)
        {
            return (data / 16) * 10 + (data % 16);
        }
        public static int BcdToInt(byte[] data)
        {
            int result = 0;
            for (int i = data.Length - 1; i >= 0; i--)
            {
                result *= 100;
                result += BcdToInt(data[i]);
            }
            return result;
        }
        public static byte[] StringToAscii(string p, int length, byte defaultValue = 0)
        {
            byte[] result = new byte[length];
            char[] arr = p.ToCharArray();
            for (int i = 0; i < length; i++)
                result[i] = (i >= arr.Length) ? defaultValue : (byte)arr[i];
            return result;
        }
        public static string AsciiToString(byte[] data, int start, int length)
        {
            StringBuilder sb = new StringBuilder();
            int end = start + length;
            end = (data.Length < end) ? data.Length : end;
            while (start < end)
            {
                if (!KuFunction.IsAscii(data[start]))
                    break;
                sb.Append((char)data[start]);
                start++;
            }
            return sb.ToString();
        }
    }
}
