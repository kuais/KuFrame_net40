using System.IO;
using System.Text;

namespace Ku.file
{
    interface IKuFile
    {
        void Load(string path);
        void Save(string path = "");
    }
    public class KuFile
    {
        public static void MkDir(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        public static void Copy(string from, string to)
        {
            File.Copy(from, to, true);
        }

        public static void Write(string path, string text, Encoding encoding = null)
        {
            encoding ??= Encoding.Default;
            using (var s = new StreamWriter(path, false, encoding))
            {
                s.Write(text);
            }
        }
        public static void Append(string path, string text, Encoding encoding = null)
        {
            encoding ??= Encoding.Default;
            using (var s = new StreamWriter(path, true, encoding))
            {
                s.Write(text);
            }
        }
        public static string Read(string path, Encoding encoding = null)
        {
            encoding ??= Encoding.Default;
            using (var s = new StreamReader(path, encoding))
            {
                return s.ReadToEnd();
            }
        }

        public static byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }
        public static void WriteAllBytes(string path, byte[] datas)
        {
            File.WriteAllBytes(path, datas);
        }
    }
}
