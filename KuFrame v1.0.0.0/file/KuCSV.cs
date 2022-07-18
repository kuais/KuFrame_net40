using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Ku.file
{
    public class KuCSV : IKuFile
    {
        #region Properties
        public string Path { get; private set; }
        public List<string> fields { get; set; }
        public List<KuModel> datas { get; set; }
        public Encoding Encoding { get; set; } = Encoding.Default;
        #endregion

        public KuCSV() { }
        public KuCSV(string path) => Load(path);
        public void Load(string path)
        {
            Path = path;
            datas = new List<KuModel>();
            var line = "";
            using (var sr = new StreamReader(Path, Encoding))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim().Replace("\t", "").Replace("\"", "");
                    if (string.IsNullOrEmpty(line)) continue;
                    var arr = Regex.Split(line, ",", RegexOptions.None);
                    var m = new KuModel();
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (i >= fields.Count)
                            break;
                        m[fields[i]] = arr[i];
                    }
                    datas.Add(m);
                }
            }
        }
        public void Save(string path = "")
        {
            if (string.IsNullOrEmpty(path)) path = Path;
            using (var sw = new StreamWriter(path, false, Encoding))
            {
                var arr = new string[fields.Count];
                foreach (var m in datas)
                {
                    for (int i = 0; i < fields.Count; i++)
                        arr[i] = $"\"{m[fields[i]]}\"";
                    sw.WriteLine(string.Join(",", arr));
                }
            }
        }
    }
}
