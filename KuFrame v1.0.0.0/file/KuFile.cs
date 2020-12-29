using System.IO;

namespace Ku.file
{
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
    }
}
