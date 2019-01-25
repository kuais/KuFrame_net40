using System.IO.Ports;

namespace Ku.io
{
    public class KuSerial : SerialPort, IOHandler
    {
        public KuBuffer recvBuffer = new KuBuffer();
        public KuSerial() : base()
        {
        }
        public KuSerial(string portName) : base(portName)
        {
        }

        public byte[] Read()
        {
            if (BytesToRead <= 0)
                return new byte[0];
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
    }
}
