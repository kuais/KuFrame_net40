using System.Collections.Generic;
//using System.Dynamic;

namespace Ku
{
    public class KuModel : Dictionary<string, object>
    {
        public void FromModel(KuModel m)
        {
            foreach (var key in m.Keys) this[key] = m[key];
        }
        public void FromObject<T>(T t)
        {
            var properties = typeof(T).GetProperties();
            foreach (var p in properties)
                this[p.Name] = p.GetValue(t, null);
        }
        public T ToObject<T>() where T : new()
        {
            T t = new T();
            var properties = typeof(T).GetProperties();
            foreach (var p in properties)
            {
                this.ContainsKey(p.Name);
                p.SetValue(t, this[p.Name], null);
            }
            return t;
        }
        //public dynamic ToDynamic()
        //{
        //    dynamic result = new ExpandoObject();
        //    foreach (var entry in this)
        //        ((ICollection<KeyValuePair<string, object>>)result).Add(new KeyValuePair<string, object>(entry.Key, entry.Value));
        //    return result;
        //}

        public string ToQueryString()
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in this)
            {
                if (pair.Value == null)
                    continue;
                var v = pair.Value.ToString().Trim();
                if (v != "")
                    buff += pair.Key + "=" + pair.Value + "&";
            }
            return buff.Trim('&');
        }
    }
}
