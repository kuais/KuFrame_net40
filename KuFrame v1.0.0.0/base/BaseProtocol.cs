using System;
using System.Collections.Generic;

namespace Ku
{
    public abstract class BaseProtocol : IProtocol
    {
        public virtual byte[] Decode(KuBuffer buf)
        {
            int dataLength = LengthToHandle(buf);
            if (dataLength == 0) return null;                           //exit 1    无数据匹配
            if (buf.DataCount < dataLength) return null;              //exit 2    数据未收完整
            var result = buf.GetArray(dataLength);
            buf.Remove(dataLength);
            return DePack(result);                                      //exit 0    返回有效数据
        }
        public virtual List<byte[]> Encode(string name, params object[] args) => throw new NotImplementedException();
        public virtual void HandleRecv(KuBuffer buf, IEventTrigger cb)
        {
            while (true)
            {
                var datas = Decode(buf);    // 解析一帧数据
                if (datas == null) return;  // 无有效数据,退出
                Handle(datas, cb);          // 处理一帧数据
            }
        }
        public virtual byte[] DePack(byte[] d) => d;
        public virtual List<byte[]> Pack(byte[] d) => new List<byte[]>() { d };

        /// <summary>
        /// 根据协议实现
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual int LengthToHandle(KuBuffer input) => input.DataCount;
        /// <summary>
        /// 根据协议实现
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="cb"></param>
        /// <returns></returns>
        protected virtual bool Handle(byte[] datas, IEventTrigger cb) => cb == null;
    }
}
