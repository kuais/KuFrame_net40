using System.Collections.Generic;
using System.Reflection;

namespace Ku.db
{
    public class DbModel : Dictionary<string, object>
    {
        public T ToObject<T>() where T: new()
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
    }
}
