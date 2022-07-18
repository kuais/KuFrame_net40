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

        public KuXML(string path = "")
        {
            Load(path);
        }
        public void Load(string path)
        {
            Path = path;
            Doc = new XmlDocument();
            if (string.IsNullOrEmpty(path))
                Init();
            else
                Doc.Load(Path);
        }
        public void Save(string path = "")
        {
            if (string.IsNullOrEmpty(path)) path = Path;
            Doc.Save(path);
        }
        public void Init()
        {
            Doc.AppendChild(Doc.CreateXmlDeclaration("1.0", "UTF-8", null));
        }
        public void LoadXml(string xml)
        {
            Doc = new XmlDocument();
            Doc.LoadXml(xml);
        }
        ///// <summary>
        ///// 获取指定名称的第一个节点
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //public XmlElement GetElement(string name = "", bool find = false)
        //{
        //    return GetElement(name, null, find);
        //}
        /// <summary>
        /// 获取指定名称的第一个子节点
        /// </summary>
        /// <param name="name">节点名称</param>
        /// <param name="parent">父节点</param>
        /// <returns></returns>
        public XmlElement GetElement(string name, XmlNode parent = null, bool find = false)
        {
            if (parent == null)
                parent = Doc.DocumentElement;
            if (string.IsNullOrEmpty(name)) return (XmlElement)parent;
            if (find)
            {
                XmlNodeList list = parent.SelectNodes(name);
                if (list.Count > 0)
                    return (XmlElement)list[0];
            }
            else
            {
                foreach (XmlNode e in parent.ChildNodes)
                {
                    if (e.Name.Equals(name) && (e.NodeType == XmlNodeType.Element))
                        return (XmlElement)e;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取xml子节点列表
        /// </summary>
        /// <param name="name">子节点名称</param>
        /// <param name="parent">父节点</param>
        /// <returns></returns>
        public List<XmlElement> GetElements(string name, XmlElement parent, bool find = false)
        {
            List<XmlElement> result = new List<XmlElement>();
            if (parent == null) return result;
            if (string.IsNullOrEmpty(name)) return result;
            XmlNodeList list;
            list = (find) ? parent.SelectNodes(name) : parent.GetElementsByTagName(name);
            foreach (XmlNode node in list)
            {
                if (node.NodeType == XmlNodeType.Element)
                    result.Add((XmlElement)node);
            }
            return result;
        }
        public XmlElement AddElement(string name, XmlNode parent = null)
        {
            var elemRoot = Doc.CreateElement(name);
            if (parent == null)
                parent = Doc.DocumentElement;
            if (parent == null)
            {
                parent = Doc;
            }
            else
            {
                var elem = GetElement(name, parent);
                if (elem != null)
                    return elem;
            }
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

        public string getAttribute(XmlElement elem, string attribute, string defaultValue = "")
        {
            var v = elem.GetAttribute(attribute);
            if (string.IsNullOrEmpty(v))
                v = defaultValue;
            return v;
        }
    }
}
