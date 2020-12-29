using System;
using System.ComponentModel;

namespace Ku.util
{
    public class KuFunction
    {
        public static bool IsAscii(byte p)
        {
            return (0x20 <= p) && (p <= 0x7E);
        }
        /// <summary>
        /// 计算累加和
        /// </summary>
        /// <param name="input">要计算的数据</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">要计算的数据数</param>
        /// <returns></returns>
        public static int GetSum(int[] input, int index, int count)
        {
            int sum = 0;
            while (count-- > 0)
                sum += input[index++];
            return sum;
        }
        public static byte GetSum(byte[] input, int index, int count)
        {
            int[] temp = new int[count];
            Array.Copy(input, index, temp, 0, count);
            return (byte)GetSum(temp, 0, count);
        }

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
        /// 获取当前年龄
        /// </summary>
        /// <param name="birth">出生日期</param>
        /// <param name="now">当前时间</param>
        /// <returns>年龄</returns>
        public static int GetAge(DateTime birth, DateTime now)
        {
            int result;
            result = now.Year - birth.Year;
            birth = birth.AddYears(result);
            if (birth > now)
                result--;  //出生时间+年差 > 当前时间，说明生日还没到，岁数-1
            return result;
        }

        public static string GetError(Exception ex) => string.IsNullOrEmpty(ex.Message) ? ex.ToString() : ex.Message;

        /// <summary>
        /// 执行操作,兼容多线程
        /// </summary>
        /// <param name="o"></param>
        /// <param name="action"></param>
        /// <param name="args"></param>
        public static void Invoke(ISynchronizeInvoke o, Action action)
        {
            if (o.InvokeRequired) o.Invoke(action, null);
            else action();
        }

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
