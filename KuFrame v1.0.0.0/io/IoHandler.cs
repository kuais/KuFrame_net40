namespace Ku.io
{
    internal interface IOHandler
    {
        byte[] Read();
        bool Write(byte[] temp);
    }
}
