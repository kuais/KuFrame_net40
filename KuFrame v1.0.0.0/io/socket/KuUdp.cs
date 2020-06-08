using System;
using System.Net;
using System.Net.Sockets;

namespace Ku.io.socket
{
    public class KuUdp : KuSocket
    {
        #region Properties
        public IPEndPoint EndPoint_Received { get; set; } = new IPEndPoint(IPAddress.Any, 0);
        #endregion

        public void Start(string ip, int port)
        {
            Open();
            Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            Receive();
        }
        public override void Open()
        {
            Close();
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }
        public void Send(byte[] data, EndPoint ep)
        {
            var e = PopArgs(1);
            e.RemoteEndPoint = ep;
            Send(data, e);
        }
        public override void Receive()
        {
            var e = PopArgs(0);
            e.RemoteEndPoint = EndPoint_Received;
            Receive(e);
        }

        protected override void Receive(SocketAsyncEventArgs e)
        {
            if (!IsOpened) return;
            try
            {
                if (!socket.ReceiveFromAsync(e)) ProcessReceive(e);
            }
            catch (Exception ex)
            {
                Listener?.OnError(ex);
            }
        }
        protected override void Send(byte[] data, SocketAsyncEventArgs e)
        {
            if (!IsOpened) return;
            try
            {
                e.SetBuffer(data, 0, data.Length);
                if (!socket.SendToAsync(e)) ProcessSend(e);
            }
            catch (Exception ex)
            {
                Listener?.OnError(ex);
            }
        }
        protected override void Received(SocketAsyncEventArgs e)
        {
            var addr = e.RemoteEndPoint.ToString();
            if (!DictConnection.ContainsKey(addr))
                new KuUdp().Connected(e);
            base.Received(e);
        }
        protected override void Sent(SocketAsyncEventArgs e)
        {
            var addr = e.RemoteEndPoint.ToString();
            if (!DictConnection.ContainsKey(addr))
                new KuUdp().Connected(e);
            base.Sent(e);
        }
    }
}
