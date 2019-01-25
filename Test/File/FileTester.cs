using Ku.file;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Test
{
    class FileTester
    {
        KuCSV kuCsv;
        KuINI kuIni;
        KuXML kuXml;
        public FileTester()
        {
            kuCsv = new KuCSV();
            kuIni = new KuINI();
            kuXml = new KuXML();
        }

        internal void Start()
        {
            try {
                kuCsv.Load("test.csv");
                kuCsv.Save();
                kuIni.Load("test.ini");
                kuIni.Save();

                kuXml.Load("test.xml");
                XmlElement elem = kuXml.GetElement("appSettings");
                List<XmlElement> list = kuXml.GetElements("add", elem);
                list[0].SetAttribute("value", "127.0.0.1");
                kuXml.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
