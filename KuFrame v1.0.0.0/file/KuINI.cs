using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ku.file
{
    public class KuINI : IKuFile
    {
        #region Properties
        public string Path { get; private set; }
        public Dictionary<string, Dictionary<string, string>> Dict { get; private set; }
        public Encoding Encoding { get; set; } = Encoding.Default;
        #endregion

        public KuINI() { }
        public void Load(string path)
        {
            Path = path;
            Dict = new Dictionary<string, Dictionary<string, string>>();
            var line = "";
            var currentSection = "";
            var dictItems = new Dictionary<string, string>();
            using (var sr = new StreamReader(Path, Encoding))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (string.IsNullOrEmpty(line)) continue;
                    if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        // End last section  
                        if ((dictItems.Count > 0) && (!string.IsNullOrEmpty(currentSection)))
                            Dict[currentSection] = dictItems;
                        // Start new section
                        currentSection = line.Substring(1, line.Length - 2).Trim();
                        dictItems = new Dictionary<string, string>();
                    }
                    else
                    {
                        int index = line.IndexOf("=");
                        if (index != -1)
                        {
                            var key = line.Substring(0, index);
                            var value = line.Substring(index + 1);
                            dictItems.Add(key, value);
                        }
                        //else
                        //{
                        //    dictItems.Add(line, line);
                        //}
                    }
                }
                // Ends last section  
                if ((!string.IsNullOrEmpty(currentSection)) && (dictItems.Count > 0))
                    Dict[currentSection] = dictItems;
            }
        }

        public void Save(string path = "")
        {
            if (string.IsNullOrEmpty(path)) path = Path;
            using (var sw = new StreamWriter(path, false, Encoding))
            {
                foreach (string k1 in Dict.Keys)
                {
                    sw.WriteLine("[{0}]", k1);
                    foreach (string k2 in Dict[k1].Keys)
                    {
                        sw.WriteLine("{0}={1}", k2, Dict[k1][k2]);
                    }
                }
            }
        }

        public void Parse(string input)
        {
            input = input.Trim();
            var arr = input.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            Dict = new Dictionary<string, Dictionary<string, string>>();
            var currentSection = "";
            var dictItems = new Dictionary<string, string>();
            foreach (var line in arr)
            {
                if (string.IsNullOrEmpty(line)) continue;
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    // End last section  
                    if ((dictItems.Count > 0) && (!string.IsNullOrEmpty(currentSection)))
                        Dict[currentSection] = dictItems;
                    // Start new section
                    currentSection = line.Substring(1, line.Length - 2).Trim();
                    dictItems = new Dictionary<string, string>();
                }
                else
                {
                    int index = line.IndexOf("=");
                    if (index != -1)
                    {
                        string key = line.Substring(0, index);
                        string value = line.Substring(index + 1);
                        dictItems.Add(key, value);
                    }
                }
            }
            // Ends last section  
            if ((!string.IsNullOrEmpty(currentSection)) && (dictItems.Count > 0))
                Dict[currentSection] = dictItems;
        }

        public override string ToString()
        {
            var text = new StringBuilder();
            foreach (string k1 in Dict.Keys)
            {
                text.AppendLine($"[{k1}]");
                foreach (string k2 in Dict[k1].Keys)
                    text.AppendLine($"{k2}={Dict[k1][k2]}");
                text.AppendLine();
            }
            return text.ToString();
        }
    }
}
