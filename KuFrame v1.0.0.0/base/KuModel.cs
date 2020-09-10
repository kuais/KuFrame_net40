using System.Collections.Generic;
//using System.Dynamic;
using System.Reflection;

namespace Ku
{
    public class KuModel : Dictionary<string, object>
    {
        public void FromModel(KuModel m)
        {
            foreach (var key in m.Keys) this[key] = m[key];
        }
        public T ToObject<T>() where T : new()
        {
            T t = new T();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo p in properties)
            {
                this.ContainsKey(p.Name);
                p.SetValue(t, this[p.Name], null);
            }
            return t;
        }
        public void FromObject<T>(T t)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo p in properties)
                this[p.Name] = p.GetValue(t, null);
        }

        //public dynamic ToDynamic()
        //{
        //    dynamic result = new ExpandoObject();
        //    foreach (var entry in this)
        //        ((ICollection<KeyValuePair<string, object>>)result).Add(new KeyValuePair<string, object>(entry.Key, entry.Value));
        //    return result;
        //}

    }
}
