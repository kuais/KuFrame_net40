using Ku.winapi;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

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
        public static Process StartInControl(string path, Control c)
        {
            var p = Start(path, true);
            p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            while (p.MainWindowHandle == IntPtr.Zero)
                Thread.Sleep(2);
            EmbedToControl(p, c);
            return p;
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

        public static bool EmbedToControl(Process p, Control c)
        {
            if (p == null || c == null ||p.MainWindowHandle == IntPtr.Zero)
                return false;
            User32.SetParent(p.MainWindowHandle, c.Handle);
            // Move the window to overlay it on this window
            User32.MoveWindow(p.MainWindowHandle, 0, 0, c.Width, c.Height, true);
            // Remove border and whatnot            
            var hwnd = new HandleRef(c, p.MainWindowHandle);
            User32.SetWindowLong(hwnd, User32.GWL_STYLE, User32.WS_VISIBLE);
            return true;
        }
    }
}
