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

        public static long Length(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return fs.Length;
            }
        }
        /// <summary>
        /// 从offset位置开始反向查找sub出现的位置
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sub"></param>
        /// <param name="offset"></param>
        /// <param name="encoding"></param>
        public static long SeekLast(string path, string sub, long offset, Encoding encoding = null)
        {
            encoding ??= Encoding.Default;
            long pos = 0;
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                pos = fs.Seek(offset, SeekOrigin.Begin);
                if (offset > fs.Length)
                    offset = fs.Length;
                while (true)
                {
                    var seekLength = (int)(pos < 1024 ? pos : 1024);
                    pos = fs.Seek(-seekLength, SeekOrigin.Current);
                    byte[] buffer = new byte[seekLength];
                    fs.Read(buffer, 0, buffer.Length);
                    var s = encoding.GetString(buffer);
                    if (s.LastIndexOf(sub) >= 0)
                        return pos + s.LastIndexOf(sub);
                    if (pos == 0)
                        return -1;                          // 没找到
                }
            }
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
        public static byte[] ReadBytes(string path, int count, long pos = 0)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[count];
                fs.Seek(pos, SeekOrigin.Begin);
                fs.Read(buffer, 0, count);
                return buffer;
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
