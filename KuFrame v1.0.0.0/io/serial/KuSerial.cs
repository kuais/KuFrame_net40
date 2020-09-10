using System;
using System.IO.Ports;

namespace Ku.io
{
    public class KuSerial : SerialPort, IOHandler
    {
        public KuBuffer recvBuffer = new KuBuffer();
        long timeActivate = 0;
        public KuSerial() : base()
        {
        }
        public KuSerial(string portName) : base(portName)
        {
        }

        public byte[] Read()
        {
            if (BytesToRead <= 0) return new byte[0];
            timeActivate = DateTime.Now.Ticks;
            byte[] temp = new byte[BytesToRead];
            Read(temp, 0, temp.Length);
            recvBuffer.Put(temp);
            return temp;
        }
        public bool Write(byte[] temp)
        {
            Write(temp, 0, temp.Length);
            return true;
        }
        public bool CheckActivated(long ms)
        {
            return DateTime.Now.Ticks < (timeActivate + ms * 10000);
        }
    }
}
