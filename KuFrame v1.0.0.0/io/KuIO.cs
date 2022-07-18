using System;
using System.IO;

namespace Ku.io
{
    public class KuIO
    {
        public static void Transfer(Stream input, Stream output, IProgress listener = null)
        {
            long total = input.Length;
            long current = 0;
            byte[] buffer = new byte[4096];
            int l;
            listener?.OnStart();
            try
            {
                while (true)
                {
                    l = input.Read(buffer, 0, buffer.Length);
                    if (l <= 0)
                        break;
                    output.Write(buffer, 0, l);
                    current += l;
                    listener?.OnProgress(current, total);
                }
                output.Flush();
            }
            catch (Exception ex)
            {
                listener?.OnError(ex);
            }
            listener?.OnStop();
        }
    }
    internal interface IOHandler
    {
        byte[] Read();
        bool Write(byte[] temp);
    }
}
