using System;
using System.IO;

namespace Ku
{
    public class KuLog
    {
        public string BasePath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        public KuLog(string basepath = "")
        {
            if (!string.IsNullOrEmpty(basepath)) BasePath = basepath;
        }
        public void Debug(string content)
        {
            Log(content, "Debug");
        }

        public void Info(string content)
        {
            Log(content, "Info");
        }

        public void Error(string content)
        {
            Log(content, "Error");
        }

        public void Error(Exception ex)
        {
            string text = string.Format("Message:{0}\r\nStackTrace:{1}"
                , ex.Message, ex.StackTrace);
            Error(text);
        }

        public void Log(string content, string subject)
        {
            string path = BasePath + Path.DirectorySeparatorChar + "Log" 
                + Path.DirectorySeparatorChar + subject;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path += Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyyMMdd") + ".log";
            Write(content, path);
        }

        private static readonly object oLock = new object();
        private void Write(string content, string path)
        {
            lock (oLock)
            {
                content = string.Format("[{0}]\r\n{1}\r\n"
                    , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), content);
                File.AppendAllText(path, content);
            }
        }

    }
}
