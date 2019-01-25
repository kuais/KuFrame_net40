using System;
using System.Net.Sockets;

namespace Ku.io
{
    public class KuSocketConnection
    {
        private static long _id = 0;
        private Socket socket;
        private SocketAsyncEventArgs argsReceive;
        
        #region Properties
        public long ID { get; private set; }
        public KuSocketServer Server { get; internal set; }
        public DateTime TimeActivated { get; set; }
        public KuBuffer RecvBuffer { get; private set;}
        public bool IsConnected { get => (socket != null) && (socket.Connected); }
        public string LocalAddress { get => socket.LocalEndPoint.ToString(); }
        public string RemoteAddress { get => socket.RemoteEndPoint.ToString();}
        public IConnectionListener Listener { get; set; }
        public Socket Socket
        {
            get => socket;
            set
            {
                Disconnect();
                socket = value;
            }
        }
        #endregion

        public KuSocketConnection()
        {
            RecvBuffer = new KuBuffer();
            argsReceive = new SocketAsyncEventArgs();
            argsReceive.Completed += ArgsReceive_Completed;
            argsReceive.SetBuffer(new byte[1024], 0, 1024);
            ID = _id++;
            TimeActivated = DateTime.Now;
        }

        public bool Connect(string ip, int port)
        {
            try
            {
                if (IsConnected) Disconnect();
                if (socket == null)
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ip, port);
                Connected();
                return true;
            }
            catch (Exception ex)
            {
                if (Listener != null) Listener.OnError(ex);
                return false;
            }
        }

        public void Disconnect()
        {
            if (socket != null) return;
            if (IsConnected) socket.Close();
            if (Listener != null) Listener.OnDisconnected(this);
            if (Server != null) 
            {
                Server.RemoveConnection(this);
                Server = null;
            }
        }

        public void Receive()
        {
            try
            {
                Array.Clear(argsReceive.Buffer, argsReceive.Offset, argsReceive.BytesTransferred); //清空缓存,避免脏读
                bool ret = socket.ReceiveAsync(argsReceive);
                if (!ret) ProcessReceived(argsReceive);
            }
            catch (Exception ex)
            {
                if (Listener != null) Listener.OnError(ex);
            }
        }

        public bool Send(byte[] data)
        {
            try
            {
                if (!IsConnected) throw new SocketException((int)SocketError.NotConnected);
                socket.Send(data);
                TimeActivated = DateTime.Now;
                if (Listener != null) Listener.OnSent(this, data);
                return true;
            }
            catch (Exception ex)
            {
                if (Listener != null) Listener.OnError(ex);
                return false;
            }
            
        }

        internal void Connected()
        {
            TimeActivated = DateTime.Now;
            if (Server != null) Server.AddConnection(this);
            if (Listener != null) Listener.OnConnected(this);
        }

        private void ArgsReceive_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceived(e);
                    break;
                case SocketAsyncOperation.Connect:
                case SocketAsyncOperation.Disconnect:
                case SocketAsyncOperation.Send:
                case SocketAsyncOperation.Accept:
                case SocketAsyncOperation.ReceiveFrom:
                case SocketAsyncOperation.ReceiveMessageFrom:
                case SocketAsyncOperation.SendPackets:
                case SocketAsyncOperation.SendTo:
                case SocketAsyncOperation.None:
                default:
                    break;
            }
        }

        private void ProcessReceived(SocketAsyncEventArgs e)
        {
            TimeActivated = DateTime.Now;
            if (e.SocketError != SocketError.Success)
            {   //接收出错
                if (Listener != null)
                    Listener.OnError(new KuSocketException(e));
                this.Disconnect();
            }
            else if (e.BytesTransferred == 0)
            {   //收到空数据 = 断线
                this.Disconnect();
            }
            else
            {
                if (RecvBuffer.CheckTimeout()) RecvBuffer.Clear();
                byte[] data = new byte[e.BytesTransferred];
                Buffer.BlockCopy(e.Buffer, 0, data, 0, e.BytesTransferred);
                RecvBuffer.Put(data);
                if (Listener != null) Listener.OnReceived(this, data);
                this.Receive();
            }
        }
    }
}
