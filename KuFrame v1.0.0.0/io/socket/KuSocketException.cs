using System;
using System.Net.Sockets;

namespace Ku.io
{
    [Serializable]
    public class KuSocketException : Exception
    {
        public SocketError Error { get; private set; } = SocketError.SocketError;
        public KuSocketException(string message) : base(message){}
        public KuSocketException(SocketAsyncEventArgs e) : this(e.SocketError.ToString())
        {
            this.HResult = (int)e.SocketError;
            this.Error = e.SocketError;
        }
    }
}
