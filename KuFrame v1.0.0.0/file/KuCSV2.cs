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
            string line = "";
            using (StreamReader sr = new StreamReader(Path, Encoding))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (string.IsNullOrEmpty(line)) continue;
                    line = line.Replace("\t", "");
                    line = line.Replace("\"", "");
                    string[] arr = Regex.Split(line, ",", RegexOptions.None);
                    Dict[arr[0]] = arr[1];
                }
            }
        }
        public void Save(string path = "")
        {
            if (string.IsNullOrEmpty(path)) path = Path;
            using (StreamWriter sw = new StreamWriter(path, false, Encoding))
            {
                foreach (string k1 in Dict.Keys)
                {
                    string temp = string.Format("\"{0}\",\"{1}\"", k1, Dict[k1]);
                    sw.WriteLine(temp);
                }
            }
        }
    }
}
