using Ku;
using Ku.io;
using Ku.util;
using System;

namespace Test
{
    class SerialTester : IDataHandler
    {
        KuSerial serial = new KuSerial("COM3");
        KuLog logger = new KuLog();
        
        public SerialTester()
        {
            serial.DataReceived += Serial_DataReceived;
        }

        private void Serial_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            byte[] recv = serial.Read();
            Console.Out.WriteLine("Received: " + string.Join(" ", KuConvert.HexFromDec(recv)));
            Handle(serial.recvBuffer);
        }

        public void Start()
        {
            try
            {
                serial.Open();
                serial.Write("Hello");
                Console.Out.WriteLine("Writed: Hello");
            }
            catch (Exception ex)
            {
                serial.Close();
                Console.Out.WriteLine(ex.Message);
            }
        }

        public void Handle(KuBuffer buffer)
        {
            string target = "Hello";
            byte[] temp = serial.Encoding.GetBytes(target);
            int index = buffer.Find(temp);
            if (index < 0)
                buffer.Clear();
            else
            {
                byte[] datas = buffer.Get(temp.Length, index);
                buffer.Remove(index + datas.Length);
                serial.Write(datas);
                Console.Out.WriteLine("Writed: " + string.Join(" ", KuConvert.HexFromDec(datas)));
            }
        }
    }
}
