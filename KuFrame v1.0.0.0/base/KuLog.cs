using System;
using System.IO;
using System.Threading;

namespace Ku
{
    public class KuLog
    {
        public string BasePath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        public KuLog(string basepath = "")
        {
            if (!string.IsNullOrEmpty(basepath)) BasePath = basepath;
        }
        public void Debug(string content) => Log(content, "Debug");
        public void Info(string content) => Log(content, "Info");
        public void Error(string content) => Log(content, "Error");
        public void Error(Exception ex) => Error($"Message:{ex.Message}\r\nStackTrace:{ex.StackTrace}");
        public void Log(string content, string subject)
        {
            var path = Path.Combine(BasePath, "Log", subject);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = Path.Combine(path, DateTime.Now.ToString("yyyyMMdd") + ".log");
            Write(content, path);
        }

        private ReaderWriterLockSlim _rw = new ReaderWriterLockSlim();
        private void Write(string content, string path)
        {
            try
            {
                _rw.EnterWriteLock();
                content = string.Format("[{0}]\r\n{1}\r\n"
                    , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), content);
                File.AppendAllText(path, content);
            }
            finally 
            {
                _rw.ExitWriteLock();
            }
        }

    }
}
