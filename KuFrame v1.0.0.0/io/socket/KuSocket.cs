using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Ku.io.socket
{
    public abstract class KuSocket : IDisposable
    {
        protected Socket socket;
        private readonly object lockO = new object();
        private readonly Stack<SocketAsyncEventArgs> stack_Recv = new Stack<SocketAsyncEventArgs>();
        private readonly Stack<SocketAsyncEventArgs> stack_Send = new Stack<SocketAsyncEventArgs>();

        #region Properties
        public Dictionary<string, KuSocket> DictConnection { get; } = new Dictionary<string, KuSocket>();
        public EndPoint LocalEndPoint { get; protected set; }
        public EndPoint RemoteEndPoint { get; set; }
        public KuBuffer RecvBuffer { get; private set; }
        public DateTime TimeActivated { get; private set; }
        public bool IsOpened => socket != null;
        public ISocketListener Listener { get; set; }
        public int BufferSize
        {
            get => RecvBuffer.Size;
            set => RecvBuffer = new KuBuffer(value);
        }
        public Socket Socket
        {
            get => socket;
            set
            {
                Close();
                if (value != null)
                    socket = value;
            }
        }
        #endregion

        protected KuSocket() : this(1024) { }
        protected KuSocket(int bufferSize)
        {
            BufferSize = bufferSize;
        }

        public virtual void Open() => throw new NotImplementedException();
        public virtual void Close()
        {
            lock (lockO)
            {   // 不加锁会出现socket == null
                if (socket == null) return;
                //DisConnected(null);
                socket.Close();
                socket = null;
                stack_Recv.Clear();
                stack_Send.Clear();
            }
        }
        public void ReuseSocket()
        {
            if (socket == null) return;
            Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public virtual void Bind(string ip, int port) => Bind(new IPEndPoint(IPAddress.Parse(ip), port));    //设置监听地址和端口   
        public virtual void Bind(IPEndPoint ep)
        {
            socket.Bind(ep);                                          //设置监听地址和端口   
            LocalEndPoint = socket.LocalEndPoint;
        }

        public virtual void Activate() => TimeActivated = DateTime.Now;
        public virtual void Receive() => throw new NotImplementedException();

        protected virtual void Accept(SocketAsyncEventArgs e = null) => throw new NotImplementedException();
        protected virtual void Receive(SocketAsyncEventArgs e) => throw new NotImplementedException();
        protected virtual void Send(byte[] data, SocketAsyncEventArgs e) => throw new NotImplementedException();
        protected virtual void Accepted(SocketAsyncEventArgs e) => throw new NotImplementedException();
        protected virtual void Connected(SocketAsyncEventArgs e)
        {
            this.RemoteEndPoint = e.RemoteEndPoint;
            Activate();
            lock (((ICollection)DictConnection).SyncRoot)
            {
                DictConnection[RemoteEndPoint.ToString()] = this;
            }
            Listener?.OnConnected(this);
            Receive();
        }
        protected virtual void DisConnected(SocketAsyncEventArgs e)
        {
            //if ((RemoteEndPoint == null) || (Socket == null)) return;
            if (Socket == null) 
                return;
            lock (((ICollection)DictConnection).SyncRoot)
            {
                DictConnection.Remove(RemoteEndPoint.ToString());
                //this.RemoteEndPoint = null;
            }
            Listener?.OnDisconnected(this);
        }
        protected virtual void Received(SocketAsyncEventArgs e)
        {
            var data = new byte[e.BytesTransferred];
            Buffer.BlockCopy(e.Buffer, 0, data, 0, e.BytesTransferred);
            var conn = DictConnection[e.RemoteEndPoint.ToString()];
            conn.RecvBuffer.Put(data);
            conn.Activate();
            Listener.OnReceived(conn, data);
        }
        protected virtual void Sent(SocketAsyncEventArgs e)
        {
            var conn = DictConnection[e.RemoteEndPoint.ToString()];
            conn.Activate();
            Listener.OnSent(conn, e.Buffer);
        }

        protected void SocketEvent_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    ProcessAccepted(e);
                    break;
                case SocketAsyncOperation.Connect:
                    ProcessConnected(e);
                    break;
                case SocketAsyncOperation.Disconnect:
                    ProcessDisConnected(e);               //通过ProcessReceived收到0字节判断断线，这里再处理会重复
                    break;
                case SocketAsyncOperation.Receive:
                case SocketAsyncOperation.ReceiveFrom:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                case SocketAsyncOperation.SendTo:
                    ProcessSend(e);
                    break;
            }
        }

        protected void ProcessAccepted(SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
                Listener?.OnError(new KuSocketException(e));
            else
                Accepted(e);
            e.AcceptSocket = null;
            Accept();
        }
        protected void ProcessConnected(SocketAsyncEventArgs e)
        {
            PushArgs(e, 1);
            if (e.SocketError != SocketError.Success)
            {   //连接失败
                Listener?.OnError(new KuSocketException(e));
            }
            else
            {
                Connected(e);                                     //连接成功
            }
        }
        protected void ProcessDisConnected(SocketAsyncEventArgs e)
        {
            PushArgs(e, 1);
            if (e.SocketError != SocketError.Success)               //断开连接失败
                Listener?.OnError(new KuSocketException(e));
            else  //断开连接成功
                DisConnected(e);
        }
        protected void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (Socket == null)
            {
                PushArgs(e, 0);
            }
            else if (e.SocketError != SocketError.Success)
            {
                Listener?.OnError(new KuSocketException(e));                //接收出错
                this.Close();
            }
            else if (e.BytesTransferred == 0)
            {                                                               //收到空数据 = 断线
                this.Close();
            }
            else
            {
                Received(e);
                Receive(e);
            }
        }
        protected void ProcessSend(SocketAsyncEventArgs e)
        {
            PushArgs(e, 1);
            if (e.SocketError != SocketError.Success)
                Listener?.OnError(new KuSocketException(e));        //发送出错
            else
                Sent(e);
        }

        protected SocketAsyncEventArgs PopArgs(int flag)
        {
            SocketAsyncEventArgs e;
            if (flag == 0)
            {
                if (stack_Recv.Count > 0)
                {
                    e = stack_Recv.Pop();
                }
                else
                {
                    e = new SocketAsyncEventArgs();
                    var datas = new byte[BufferSize];
                    e.SetBuffer(datas, 0, BufferSize);
                }
            }
            else
            {
                e = (stack_Send.Count > 0) ? stack_Send.Pop() : new SocketAsyncEventArgs();
            }

            e.Completed += SocketEvent_Completed;
            return e;
        }
        protected void PushArgs(SocketAsyncEventArgs e, int flag)
        {
            e.Completed -= SocketEvent_Completed;
            if (flag == 0)
                stack_Recv.Push(e);
            else
                stack_Send.Push(e);
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue) return;
            if (disposing) Close();
            disposedValue = true;
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
