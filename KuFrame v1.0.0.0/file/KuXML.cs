using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace Ku.file
{
    public class KuXML : IKuFile
    {
        #region Properties
        public string Path { get; private set; }
        public XmlDocument Doc { get; private set; }
        #endregion

        public KuXML()
        {
            Doc = new XmlDocument();
            XmlNode Xnode = Doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            Doc.AppendChild(Xnode);
        }
        public void Load(string path)
        {
            Path = path;
            Doc.Load(Path);
        }

        public void Save(string path = "")
        {
            if (string.IsNullOrEmpty(path)) path = Path;
            Doc.Save(path);
        }
        /// <summary>
        /// 获取指定名称的第一个节点
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public XmlElement GetElement(string name = "")
        {
            if (string.IsNullOrEmpty(name)) return Doc.DocumentElement;
            XmlNodeList list = Doc.GetElementsByTagName(name);
            return (list.Count <= 0) ? null : (XmlElement)list[0];
        }
        /// <summary>
        /// 获取指定名称的第一个子节点
        /// </summary>
        /// <param name="name">节点名称</param>
        /// <param name="parent">父节点</param>
        /// <returns></returns>
        public XmlElement GetElement(string name, XmlElement parent)
        {
            if (parent == null) return null;
            XmlNodeList list = parent.GetElementsByTagName(name);
            return (list.Count <= 0) ? null : (XmlElement)list[0];
        }
        /// <summary>
        /// 获取xml子节点列表
        /// </summary>
        /// <param name="name">子节点名称</param>
        /// <param name="parent">父节点</param>
        /// <returns></returns>
        public List<XmlElement> GetElements(string name, XmlElement parent)
        {
            List<XmlElement> result = new List<XmlElement>();
            if (parent == null) return result;
            if (string.IsNullOrEmpty(name)) return result;
            XmlNodeList list = parent.GetElementsByTagName(name);
            foreach (XmlNode node in list)
            {
                if (node.NodeType == XmlNodeType.Element)
                    result.Add((XmlElement)node);
            }
            return result;
        }
        public XmlElement AddElement(string name, XmlElement parent = null)
        {
            XmlElement elemRoot = Doc.CreateElement(name);
            if (parent == null)
                return Doc.AppendChild(elemRoot) as XmlElement;
            else
                return parent.AppendChild(elemRoot) as XmlElement;
        }

        /// <summary>
        /// 加载指定节点下的元素到指定对象列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="node">节点名称</param>
        /// <returns>元素转换后的对象列表</returns>
        public List<T> GetElementlist<T>(XmlElement elem) where T : new()
        {
            if (elem == null) return null;
            List<T> result = new List<T>();
            XmlNodeList nodeList = elem.ChildNodes;
            int count = nodeList.Count;
            XmlElement e;
            for (int i = 0; i < count; i++)
            {
                if (nodeList[i].NodeType != XmlNodeType.Element) continue;
                T t = new T();
                Type type = t.GetType();
                e = (XmlElement)nodeList[i];
                XmlAttributeCollection atrList = e.Attributes;
                foreach (XmlAttribute a in atrList)
                {
                    PropertyInfo p = type.GetProperty(a.Name);
                    if (p != null)
                        p.SetValue(t, a.Value, null);
                }
                result.Add(t);
            }
            return result;
        }
    }
}
