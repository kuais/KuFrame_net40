using System.Net.Sockets;

namespace Ku.io.socket
{
    public interface ISocketListener : IError
    {
        void OnConnected(KuSocket c);
        void OnDisconnected(KuSocket c);
        void OnReceived(KuSocket c, byte[] data);
        void OnSent(KuSocket c, byte[] data);
    }
}
