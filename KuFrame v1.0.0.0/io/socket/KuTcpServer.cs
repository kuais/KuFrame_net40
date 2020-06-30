using System;
using System.Collections;
using System.Net.Sockets;

namespace Ku.io.socket
{
    public class KuTcpServer : KuSocket
    {
        private SocketAsyncEventArgs argsAccept;

        #region Properties
        public bool IsStarted { get; private set; } = false;
        public int ConnectionTimeout { get; set; } = 300;                 //0 不断开连接, 300秒无数据则断开连接
        public int ListenLimit { get; set; } = 1000;                                                               //排队Accept的连接上限
        public int ConnectionCount { get => DictConnection.Count; }
        #endregion

        public KuTcpServer() {}
        public virtual void Start()
        {
            argsAccept = new SocketAsyncEventArgs();
            argsAccept.Completed += SocketEvent_Completed;
            socket.Listen(ListenLimit);
            Accept();
            IsStarted = true;
        }
        public override void Open()
        {
            Close();
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public override void Close()
        {
            if (socket == null) return;
            if (argsAccept != null)
            {
                argsAccept.Completed -= SocketEvent_Completed;
                argsAccept = null;
            }
            //关闭所有连接
            var addrList = new string[DictConnection.Count];
            DictConnection.Keys.CopyTo(addrList, 0);
            try
            {
                foreach (string addr in addrList)
                {
                    if (DictConnection.ContainsKey(addr))
                        DictConnection[addr].Close();
                }
            }
            catch { }
            base.Close();
            IsStarted = false;
        }
        public void Disconnect(string addr)
        {
            ((KuTcp)DictConnection[addr]).Close();
        }
        public void Send(byte[] data, string addr)
        {
            ((KuTcp)DictConnection[addr]).Send(data);
        }
        public void CheckAlive()
        {
            if (ConnectionTimeout <= 0) return;
            var keys = new string[DictConnection.Keys.Count];
            DictConnection.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; i++)
            {
                TimeSpan t = (DateTime.Now - DictConnection[keys[i]].TimeActivated);
                if (t.TotalSeconds > ConnectionTimeout)
                    Disconnect(keys[i]);
            }
        }
        protected override void Accept(SocketAsyncEventArgs e = null)
        {
            if (socket == null) return;                                                 //监听Socket未初始化
            try
            {
                if (!socket.AcceptAsync(argsAccept))
                    ProcessAccepted(argsAccept);
            }
            catch (Exception ex)
            {
                Listener?.OnError(ex);
            }
        }
        protected override void Accepted(SocketAsyncEventArgs e)
        {
            var tcp = new KuTcp();
            tcp.Server = this;
            tcp.Listener = Listener;
            tcp.Connected(e.AcceptSocket);
        }
        public void RemoveConnection(KuTcp conn)
        {
            var addr = conn.RemoteEndPoint.ToString();
            if (DictConnection.ContainsKey(addr))
            {
                lock (((ICollection)DictConnection).SyncRoot)
                {
                    DictConnection.Remove(addr);
                }
            } 
        }
        public void AddConnection(KuTcp conn)
        {
            lock (((ICollection)DictConnection).SyncRoot)
            {
                var addr = conn.RemoteEndPoint.ToString();
                DictConnection[addr] = conn;
            }
        }
    }
}
