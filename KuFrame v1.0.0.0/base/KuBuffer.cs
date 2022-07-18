using System;

namespace Ku
{
    public class KuBuffer
    {
        #region private
        private int _curPosition;                                       /* 当前指向位置 */
        private int _dataCount;                                         /* 当前已保存数据数 */
        #endregion

        #region public
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        public byte[] Buffers { get; private set; }
        /// <summary>
        /// 最后接收时间
        /// </summary>
        public DateTime TimeReceived { get; private set; }
        /// <summary>
        /// 还未处理的数据数
        /// </summary>
        public int DataCount => _dataCount - _curPosition;
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        public int Size { get => Buffers.Length; set { Resize(value); } }
        /// <summary>
        /// 单帧数据接收超时时间
        /// </summary>
        public int Timeout { get; set; } = 500;

        #endregion

        public KuBuffer(int size = 1024)
        {
            _curPosition = 0;
            _dataCount = 0;
            Buffers = new byte[size];
        }
        private void Resize(int size)
        {
            if (size == Size) return;
            byte[] temp = Buffers;
            Buffers = new byte[size];
            int count = (temp.Length > size) ? size : temp.Length;
            Buffer.BlockCopy(temp, 0, Buffers, 0, count);
        }
        public void Put(byte[] newData)
        {
            Put(newData, newData.Length, 0);
        }
        public void Put(byte[] newData, int length, int offset)
        {
            TimeReceived = DateTime.Now;
            if ((length + _dataCount) > this.Size)
            {   //数据总数超过缓冲区，移除已处理的数据，重建缓冲区
                _dataCount = DataCount;
                Buffer.BlockCopy(Buffers, _curPosition, Buffers, 0, _dataCount);
                _curPosition = 0;
                if ((length + _dataCount) > Size)  //清除后依然超出缓冲区，增大缓冲区
                    Resize(length + _dataCount);
            }
            Buffer.BlockCopy(newData, offset, Buffers, _dataCount, length);
            _dataCount += length;
        }
        /// <summary>
        /// 取出数据
        /// </summary>
        /// <param name="length">数据数</param>
        /// <param name="offset">偏移量</param>
        /// <returns>取出的数据</returns>
        public byte[] GetArray(int length, int offset = 0)
        {
            byte[] ret = new byte[length];
            Buffer.BlockCopy(Buffers, _curPosition + offset, ret, 0, length);
            return ret;
        }
        /// <summary>
        /// 取出所有数据
        /// </summary>
        /// <returns>取出的数据</returns>
        public byte[] GetAll()
        {
            return GetArray(DataCount);
        }
        /// <summary>
        /// 取出某一位置的值
        /// </summary>
        /// <param name="pos">要取出的位置</param>
        /// <returns>取出的值</returns>
        public byte Get(int offset = 0)
        {
            return Buffers[_curPosition + offset];
        }
        /// <summary>
        /// 查找指定数据的位置，找不到返回-1
        /// </summary>
        /// <param name="value">要找的值</param>
        /// <param name="offset">开始查找的位置</param>
        /// <returns>值所在的位置</returns>
        public int Find(byte value, int offset = 0)
        {
            if (offset < 0) offset = 0;
            while (offset < DataCount)
            {
                if (Get(offset) == value) return offset;
                offset++;
            }
            return -1;
        }
        public int Find(byte[] values, int offset = 0)
        {
            if (offset < 0) offset = 0;
            int i = 0, valueLen = values.Length;
            while ((offset + valueLen) <= DataCount)
            {
                i = 0;
                offset = Find(values[i], offset);
                if (offset < 0)
                    break;
                while (true)
                {
                    if (i == (valueLen - 1))
                        return offset;
                    i++;
                    if (Get(offset + i) != values[i])
                        break;
                }
                offset++;
            }
            return -1;
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="length">要移除数据的长度</param>
        public void Remove(int length)
        {
            length = (length > DataCount) ? DataCount : length;
            _curPosition += length;
        }

        /// <summary>
        /// 新的一帧，位置移到新数据开始
        /// </summary>
        public void Clear()
        {
            _curPosition = _dataCount;
        }

        /// <summary>
        /// 检查数据是否超时
        /// </summary>
        /// <returns>true: 超时, false:还未超时</returns>
        public bool CheckTimeout()
        {
            return (TimeReceived.AddMilliseconds(Timeout) < DateTime.Now);
        }
    }
}
