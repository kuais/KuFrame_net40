using System;
using System.Net;
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
        public int Timeout { get; set; } = 5;
        public KuSocketServer Server { get; internal set; }
        public DateTime TimeActivated { get; set; }
        public KuBuffer RecvBuffer { get; private set;}
        //public bool IsConnected { get => (socket != null) && (socket.Connected); }
        public bool IsConnected { get; private set; } = false;
        public string LocalAddress { get; private set; }
        public string RemoteAddress { get; private set; }
        public IConnectionListener Listener { get; set; }
        public Socket Socket
        {
            get => socket;
            set
            {
                Close();
                socket = value;
                if (socket != null)
                    argsReceive.Completed += ArgsReceive_Completed;
            }
        }
        #endregion

        public KuSocketConnection()
        {
            RecvBuffer = new KuBuffer();
            argsReceive = new SocketAsyncEventArgs();
            SetRecvBufferSize();
            ID = _id++;
            TimeActivated = DateTime.Now;
        }

        public void SetRecvBufferSize(int size = 1024)
        {
            RecvBuffer.Size = size;
            argsReceive.SetBuffer(RecvBuffer.Buffers, 0, RecvBuffer.Size);
        }

        public void Close()
        {
            Disconnect();
            if (socket == null) return;
            argsReceive.Completed -= ArgsReceive_Completed;
            socket.Close();
            socket = null;
        }

        public bool Connect(string ip, int port)
        {
            try
            {
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket.ReceiveTimeout = 0;
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                argsReceive.RemoteEndPoint = endPoint;
                bool ret = socket.ConnectAsync(argsReceive);
                if (!ret)
                {
                    ProcessConnected(argsReceive);
                } 
                else
                {
                    WaitConnect();
                }
                return socket.Connected;
            }
            catch (Exception ex)
            {
                Listener?.OnError(ex);
                return false;
            }
        }
        private void WaitConnect()
        {
            DateTime dt = DateTime.Now;
            while (!socket.Connected)
            {
                if (dt.AddSeconds(Timeout) <= DateTime.Now)
                {
                    Close();
                    throw new Exception("Connect Timeout!");
                }
            }
        }
        public void Disconnect()
        {
            if (!IsConnected) return;
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Disconnect(false);
            }
            catch (Exception ex)
            {
                Listener?.OnError(ex);
            }
            OnDisConnected();
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
                Listener?.OnError(ex);
            }
        }

        public bool Send(byte[] data)
        {
            try
            {
                if (!IsConnected) throw new SocketException((int)SocketError.NotConnected);
                socket.Send(data);
                TimeActivated = DateTime.Now;
                Listener?.OnSent(this, data);
                return true;
            }
            catch (Exception ex)
            {
                Listener?.OnError(ex);
                return false;
            }
            
        }

        internal void OnConnected()
        {
            IsConnected = true;
            TimeActivated = DateTime.Now;
            LocalAddress = socket.LocalEndPoint.ToString();
            RemoteAddress = socket.RemoteEndPoint.ToString();
            Server?.AddConnection(this);
            Listener?.OnConnected(this);
            Receive();
        }
        internal void OnDisConnected()
        {
            IsConnected = false;
            Listener?.OnDisconnected(this);
            Server?.RemoveConnection(this);
            Server = null;
            LocalAddress = null;
            RemoteAddress = null;
            Socket = null;
        }
        private void ArgsReceive_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    ProcessConnected(e);
                    break;
                case SocketAsyncOperation.Disconnect:
                    ProcessDisConnected(e);               //通过ProcessReceived收到0字节判断断线，这里再处理会重复
                    break;
                case SocketAsyncOperation.Receive:
                    ProcessReceived(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSent(e);
                    break;
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

        private void ProcessConnected(SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {   //连接失败
                Listener?.OnError(new KuSocketException(e));
                Close();
            }
            else
            {   //连接成功
                OnConnected();
            }
        }
        private void ProcessDisConnected(SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {   //断开连接失败
                Listener?.OnError(new KuSocketException(e));
            }
            else
            {   //断开连接成功
                OnDisConnected();
            }
        }
        private void ProcessReceived(SocketAsyncEventArgs e)
        {
            TimeActivated = DateTime.Now;
            if (e.SocketError != SocketError.Success)
            {   //接收出错
                Listener?.OnError(new KuSocketException(e));
                this.Close();
            }
            else if (e.BytesTransferred == 0)
            {   //收到空数据 = 断线
                this.Close();
            }
            else
            {
                //if (RecvBuffer.CheckTimeout()) RecvBuffer.Clear();
                byte[] data = new byte[e.BytesTransferred];
                Buffer.BlockCopy(e.Buffer, 0, data, 0, e.BytesTransferred);
                RecvBuffer.Put(data);
                Listener?.OnReceived(this, data);
                //Thread.Sleep(100);
                Receive();
            }
        }
        private void ProcessSent(SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {   //发送出错
                Listener?.OnError(new KuSocketException(e));
            }
            else
            {
                TimeActivated = DateTime.Now;
                Listener?.OnSent(this, e.Buffer);
            }
        }
    }
}
