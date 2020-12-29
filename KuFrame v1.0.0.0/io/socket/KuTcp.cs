using System;
using System.Net;
using System.Net.Sockets;

namespace Ku.io.socket
{
    public class KuTcp : KuSocket
    {
        #region Properties
        public bool IsConnected => (socket !=null && socket.Connected);
        public KuTcpServer Server { get; set; }
        public Action AfterDisconnected { get; set; }
        #endregion

        public override void Open()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public override void Close()
        {
            Disconnect();
            base.Close();
        }
        public bool Connect(string ip, int port) => Connect(new IPEndPoint(IPAddress.Parse(ip), port));
        public bool Connect(IPEndPoint ep)
        {
            try
            {
                if (Socket == null) Open();
                var e = PopArgs(1);
                e.RemoteEndPoint = ep;
                if (!socket.ConnectAsync(e)) ProcessConnected(e);
                else WaitConnect();
            }
            catch (Exception ex)
            {
                Listener?.OnError(ex);
                //Close();
            }
            return IsConnected;
        }
        public void Disconnect(bool reuse = false)
        {
            if (!IsConnected) return;
            //if (!socket.Connected) return;
            try
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception ex)
                {
                    Listener?.OnError(ex);
                }
                socket.Disconnect(reuse);
            }
            catch (Exception ex)
            {
                Listener?.OnError(ex);
            }
            DisConnected(null);
        }
        public void Send(byte[] data)
        {
            var e = PopArgs(1);
            Send(data, e);
        }
        public override void Receive()
        {
            var e = PopArgs(0);
            Receive(e);
        }
        private void WaitConnect()
        {
            DateTime dt = DateTime.Now;
            while (!IsConnected)
            {
                if (dt.AddSeconds(5) <= DateTime.Now)
                    throw new Exception("Timeout!");
            }
        }

        protected override void Receive(SocketAsyncEventArgs e)
        {
            try
            {
                if (!socket.ReceiveAsync(e)) ProcessReceive(e);
            }
            catch (Exception ex)
            {
                Listener?.OnError(ex);
                DisConnected(e);
                this.Close();
            }
        }
        protected override void Send(byte[] data, SocketAsyncEventArgs e)
        {
            try
            {
                e.SetBuffer(data, 0, data.Length);
                e.RemoteEndPoint = RemoteEndPoint;
                if (!socket.SendAsync(e)) ProcessSend(e);
            }
            catch (Exception ex)
            {
                Listener?.OnError(ex);
            }
        }
        internal void Connected(Socket socket)
        {
            Socket = socket;
            LocalEndPoint = socket.LocalEndPoint;
            RemoteEndPoint = socket.RemoteEndPoint;
            Server?.AddConnection(this);
            Activate();
            DictConnection[RemoteEndPoint.ToString()] = this;
            Listener?.OnConnected(this);
            Receive();
        }
        protected override void Connected(SocketAsyncEventArgs e)
        {
            RemoteEndPoint = e.RemoteEndPoint;
            Server?.AddConnection(this);
            base.Connected(e);
        }
        protected override void DisConnected(SocketAsyncEventArgs e)
        {
            //if((RemoteEndPoint == null) || (Socket == null)) return;
            if (Socket == null) return;
            base.DisConnected(e);
            Server?.RemoveConnection(this);
            Server = null;
            AfterDisconnected?.Invoke();
        }
        protected override void Received(SocketAsyncEventArgs e)
        {
            byte[] data = new byte[e.BytesTransferred];
            Buffer.BlockCopy(e.Buffer, 0, data, 0, e.BytesTransferred);
            RecvBuffer.Put(data);
            Activate();
            Listener?.OnReceived(this, data);
        }
    }
}
