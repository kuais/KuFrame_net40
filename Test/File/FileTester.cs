using Ku.file;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Test
{
    class FileTester
    {
        internal void Start()
        {
            KuCSV kuCsv = new KuCSV();
            kuCsv.Load("test.csv");
            kuCsv.Save();

            KuINI kuIni = new KuINI();
            kuIni.Load("test.ini");
            kuIni.Save();

            KuXML kuXml = new KuXML();
            kuXml.Load("test.xml");
            XmlElement elem = kuXml.GetElement("appSettings");
            List<XmlElement> list = kuXml.GetElements("add", elem);
            list[0].SetAttribute("value", "127.0.0.1");
            kuXml.Save();
        }
    }
}
