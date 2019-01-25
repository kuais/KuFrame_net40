using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Ku.io
{
    public class KuSocketServer
    {
        private static long _id = 0;
        private Socket socket;
        private SocketAsyncEventArgs argsAccept;
        private Stack<KuSocketConnection> connnectionPool;

        #region Properties
        public long ID { get; private set; }
        public bool IsStarted { get => socket.IsBound; }
        public string LocalAddress { get => socket.LocalEndPoint.ToString(); }
        public int ListenLimit { get; set; }                        //排队Accept的连接上限
        public int ConnectionCount { get => DictConnection.Count; }
        public IServerListener Listener { get; set; }
        public Dictionary<string, KuSocketConnection> DictConnection { get; } = new Dictionary<string, KuSocketConnection>();           //RemoteAddress - Connection
        
        public Socket Socket
        {
            get => socket;
            set
            {
                Stop();
                socket = value;
            }
        }
        #endregion

        public KuSocketServer(int maxConnectionCount = 1000, int connectionBufferSize = 1024)
        {
            this.InitConnectionPool(maxConnectionCount, connectionBufferSize);
            this.ListenLimit = 1000;
            argsAccept = new SocketAsyncEventArgs();
            argsAccept.Completed += ArgsAccept_Completed;
            ID = _id++;
        }

        public bool Start(string ip, int port)
        {
            this.Stop();
            if (socket == null)
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                socket.Bind(endPoint);
                socket.Listen(ListenLimit);
                this.Accept();
                if (Listener != null)
                    Listener.OnStarted(this);
                return true;
            }
            catch (Exception ex)
            {
                this.Stop();
                if (Listener != null)
                    Listener.OnError(ex);
                return false;
            }
        }

        public void Stop()
        {
            if (socket == null) return;
            socket.Close();
            socket = null;
            //关闭所有连接
            string[] addrList = new string[DictConnection.Count];
            DictConnection.Keys.CopyTo(addrList, 0);
            foreach (string addr in addrList)
            {
                if (DictConnection.ContainsKey(addr))
                    DictConnection[addr].Disconnect();
            }
            if (Listener != null) Listener.OnStopped(this);
        }

        private void Accept()
        {
            if (this.connnectionPool.Count == 0) return;                                //连接池已空,不再接受新连接
            try
            {
                if (!socket.AcceptAsync(argsAccept))
                    ProcessAccepted(argsAccept);
            }
            catch (Exception ex)
            {
                if (Listener != null) Listener.OnError(ex);
            }
        }

        private void ArgsAccept_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    ProcessAccepted(e);
                    break;
                case SocketAsyncOperation.Connect:
                case SocketAsyncOperation.Disconnect:
                case SocketAsyncOperation.Send:
                case SocketAsyncOperation.Receive:
                case SocketAsyncOperation.ReceiveFrom:
                case SocketAsyncOperation.ReceiveMessageFrom:
                case SocketAsyncOperation.SendPackets:
                case SocketAsyncOperation.SendTo:
                case SocketAsyncOperation.None:
                default:
                    break;
            }
        }

        private void ProcessAccepted(SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                if (Listener != null) Listener.OnError(new KuSocketException(e));
            }
            else
            {
                KuSocketConnection conn = null;
                lock (((ICollection)connnectionPool).SyncRoot)
                {
                    conn = this.connnectionPool.Pop();
                }
                conn.Socket = e.AcceptSocket;
                conn.Server = this;
                if (Listener != null) Listener.OnAccepted(conn);
                conn.Connected();
            }
            e.AcceptSocket = null;
            this.Accept();                                      //继续接受新连接
        }

        private void InitConnectionPool(int maxConnectionCount, int connectionBufferSize)
        {
            connnectionPool = new Stack<KuSocketConnection>(maxConnectionCount);
            //初始化连接池
            int i = maxConnectionCount;
            while (i > 0)
            {
                KuSocketConnection c = new KuSocketConnection();
                c.RecvBuffer.Size = connectionBufferSize;
                this.connnectionPool.Push(c);
                i--;
            }
        }
        internal void RemoveConnection(KuSocketConnection conn)
        {
            lock (((ICollection)this.DictConnection).SyncRoot)
            {
                string addr = conn.RemoteAddress;
                if (this.DictConnection.ContainsKey(addr))
                    this.DictConnection.Remove(addr);
            }
            lock (((ICollection)this.connnectionPool).SyncRoot)
            {
                this.connnectionPool.Push(conn);
            }
            if (this.connnectionPool.Count == 1)       //从0->1，重新启动监听
                this.Accept();
        }
        internal void AddConnection(KuSocketConnection conn)
        {
            lock (((ICollection)this.DictConnection).SyncRoot)
            {
                string addr = conn.RemoteAddress;
                if (this.DictConnection.ContainsKey(addr))
                    this.DictConnection[addr] = conn;
                else
                    this.DictConnection.Add(addr, conn);
            }
        }
    }
}
