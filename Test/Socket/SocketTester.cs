using Ku.io;
using Ku.util;
using System;
using System.Threading;

namespace Test
{
    class SocketTester: IServerListener,IConnectionListener
    {
        KuSocketServer server;
        KuSocketConnection conn;

        public SocketTester()
        {
            server = new KuSocketServer();
            conn = new KuSocketConnection();
            server.Listener = this;
        }

        internal void Start()
        {
            if (!server.Start("127.0.0.1", 10000)) return;
            if (!conn.Connect("127.0.0.1", 10000)) return;
            byte[] data = { 0x1, 0x2, 0x3, 0x4 };
            Thread.Sleep(100);
            if (!conn.Send(data)) return;
            conn.Disconnect();
        }

        #region IServerListener
        public void OnAccepted(KuSocketConnection conn)
        {
            Console.Out.WriteLine("accept a new connection!");
            conn.Listener = this;
        }
        public void OnStarted(KuSocketServer sender)
        {
            Console.Out.WriteLine("server started!");
        }

        public void OnStopped(KuSocketServer sender)
        {
            Console.Out.WriteLine("server stopped!");
        }
        public void OnError(Exception ex)
        {
            Console.Out.WriteLine(ex.Message);
        }
        #endregion
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

        public void OnSent(KuSocketConnection sender, byte[] data)
        {
            Console.Out.WriteLine(string.Format("Local[{0}] send datas from Remote[{1}]: {2}"
                , conn.LocalAddress, conn.RemoteAddress, string.Join(" ", KuConvert.HexFromDec(data))));
        }
        #endregion

        
    }
}
