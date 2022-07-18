using System.Diagnostics;

namespace Ku.util
{
    public class KuProcess
    {
        public static Process Start(string path, bool hide = false)
        {
            var info = new ProcessStartInfo(path);
            if (hide)
            {
                info.CreateNoWindow = true;
                info.UseShellExecute = true;
                info.WindowStyle = ProcessWindowStyle.Minimized;
            }
            return Process.Start(info);
        }
        public static void Stop(Process p)
        {
            try
            {
                p.CloseMainWindow();
            }
            finally
            {
                p.Kill();
            }
        }

        public static Process[] Find(string name) => Process.GetProcessesByName(name);
        public static Process FindOne(string name) => Find(name)[0];
    }
}
