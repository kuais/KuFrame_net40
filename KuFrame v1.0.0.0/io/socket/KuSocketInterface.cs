namespace Ku.io
{
    public interface IConnectionListener : IError
    {
        void OnConnected(KuSocketConnection conn);
        void OnDisconnected(KuSocketConnection conn);
        void OnReceived(KuSocketConnection conn, byte[] data);
        void OnSent(KuSocketConnection conn, byte[] data);
    }
}
