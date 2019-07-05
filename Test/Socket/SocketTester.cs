using Ku.io;
using Ku.util;
using System;
using System.Threading;

namespace Test
{
    class SocketTester: IConnectionListener
    {
        internal void Start()
        {
            KuSocketServer server = new KuSocketServer();
            KuSocketConnection conn = new KuSocketConnection();
            server.Listener = this;
            server.Start("127.0.0.1", 10000);
            if (!conn.Connect("127.0.0.1", 10000)) return;
            byte[] data = { 0x1, 0x2, 0x3, 0x4 };
            Thread.Sleep(100);
            if (!conn.Send(data)) return;
            conn.Disconnect();
        }

        #region IConnectionListener
        public void OnConnected(KuSocketConnection conn)
        {
            Console.Out.WriteLine(string.Format("Local[{0}] connected to Remote[{1}]"
                , conn.LocalAddress, conn.RemoteAddress));
            conn.Receive();
        }

        public void OnDisconnected(KuSocketConnection conn)
        {
            Console.Out.WriteLine(string.Format("Local[{0}] disconnected to Remote[{1}]"
                , conn.LocalAddress, conn.RemoteAddress));
        }

        public void OnReceived(KuSocketConnection conn, byte[] data)
        {
            Console.Out.WriteLine(string.Format("Local[{0}] receive datas from Remote[{1}]: {2}"
                , conn.LocalAddress, conn.RemoteAddress, string.Join(" ", KuConvert.HexFromDec(data))));
        }

        public void OnSent(KuSocketConnection conn, byte[] data)
        {
            Console.Out.WriteLine(string.Format("Local[{0}] send datas from Remote[{1}]: {2}"
                , conn.LocalAddress, conn.RemoteAddress, string.Join(" ", KuConvert.HexFromDec(data))));
        }

        public void OnError(Exception ex)
        {
            Console.Out.WriteLine(ex.Message);
        }
        #endregion


    }
}
