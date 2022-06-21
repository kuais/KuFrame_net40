using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Ku.file
{
    public class KuCSV2 : IKuFile
    {
        #region Properties
        public string Path { get; private set; }
        public Dictionary<string, string> Dict { get; private set; }
        public Encoding Encoding { get; set; } = Encoding.Default;
        #endregion

        public KuCSV2() { }
        public KuCSV2(string path) => Load(path);
        public void Load(string path)
        {
            Path = path;
            Dict = new Dictionary<string, string>();
            var line = "";
            using (var sr = new StreamReader(Path, Encoding))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim().Replace("\t", "").Replace("\"", "");
                    if (string.IsNullOrEmpty(line)) continue;
                    var arr = Regex.Split(line, ",", RegexOptions.None);
                    Dict[arr[0]] = arr[1];
                }
            }
        }
        public void Save(string path = "")
        {
            if (string.IsNullOrEmpty(path)) path = Path;
            using (var sw = new StreamWriter(path, false, Encoding))
            {
                foreach (var k1 in Dict.Keys)
                    sw.WriteLine($"\"{k1}\",\"{Dict[k1]}\"");
            }
        }
    }
}
