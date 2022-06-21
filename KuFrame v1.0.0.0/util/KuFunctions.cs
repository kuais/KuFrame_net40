using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Ku.util
{
    public class KuFunction
    {
        /// <summary>
        /// Check if p is ascii
        /// </summary>
        /// <param name="p">The data need to be check</param>
        /// <returns>True:yes, False: no</returns>
        public static bool IsAscii(byte p)
        {
            return (0x20 <= p) && (p <= 0x7E);
        }
        /// <summary>
        /// 计算累加和
        /// </summary>
        /// <param name="input">要计算的数据</param>
        /// <param name="offset">起始位置</param>
        /// <param name="count">要计算的数据数</param>
        /// <returns></returns>
        public static int GetSum(int[] input, int offset, int count)
        {
            int sum = 0;
            while (count-- > 0)
            {
                if ((offset % 2048) == 0)
                    Debug.WriteLine($"Chksum: {sum}");
                sum += input[offset++];
            }
            Debug.WriteLine($"Chksum: {sum}");
            return sum;
        }
        /// <summary>
        /// Get CheckSum(byte)
        /// </summary>
        /// <param name="input">Datas to calculate</param>
        /// <param name="offset">Offset</param>
        /// <param name="count">Count</param>
        /// <returns>Sum</returns>
        public static byte GetSum(byte[] input, int offset, int count)
        {
            int[] temp = new int[count];
            Array.Copy(input, offset, temp, 0, count);
            return (byte)GetSum(temp, 0, count);
        }

        /// <summary>
        /// Get value of bits
        /// </summary>
        /// <param name="b">A byte</param>
        /// <param name="pos">the bit position, 0 - 7</param>
        /// <param name="count">The count of bits, 1 - 8</param>
        /// <returns>value</returns>
        public static int GetBit(byte b, int pos, int count = 1)
        {
            byte temp = 1;
            while (--count > 0)
            {
                temp <<= 1;
                temp += 1;
            }
            return (b >> pos) & temp;
        }

        /// <summary>
        /// Set value of bits
        /// </summary>
        /// <param name="b">A byte</param>
        /// <param name="pos">the bit position, 0 - 7</param>
        /// <param name="value">The value to be set</param>
        /// <param name="count">The count of bits, 1 - 8</param>
        /// <returns>new value</returns>
        public static byte SetBit(ref byte b, int pos, int value, int count = 1)
        {
            byte temp = 1;
            while (--count > 0)
            {
                temp <<= 1;
                temp += 1;
            }
            temp <<= pos;
            value <<= pos;
            b = (byte)((b & ~temp) | value);
            return b;
        }

        /// <summary>
        /// Get Age
        /// </summary>
        /// <param name="birth">birth day</param>
        /// <param name="now">Time of now</param>
        /// <returns>Age</returns>
        public static int GetAge(DateTime birth, DateTime now)
        {
            int result;
            result = now.Year - birth.Year;
            birth = birth.AddYears(result);
            if (birth > now)
                result--;  //出生时间+年差 > 当前时间，说明生日还没到，岁数-1
            return result;
        }

        /// <summary>
        /// Get error description
        /// </summary>
        /// <param name="ex">error</param>
        /// <returns>description</returns>
        public static string GetError(Exception ex) => string.IsNullOrEmpty(ex.Message) ? ex.ToString() : ex.Message;

        public static string PadLeft(string s, int count, char c)
        {
            return s.PadLeft(count, c);
        }
        public static string PadRight(string s, int count, char c)
        {
            return s.PadRight(count, c);
        }
        public static string Reverse(string input)
        {
            var arr = input.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
    }
}
