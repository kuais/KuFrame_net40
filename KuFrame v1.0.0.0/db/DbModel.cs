using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ku.db
{
    public class DbModel : Dictionary<string, object>, ICloneable
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
        public DbModel FromObject<T>(T t)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo p in properties)
            {
                this[p.Name] = p.GetValue(t, null);
            }
            return this;
        }

        public object Clone()
        {
            DbModel result = new DbModel();
            foreach (string key in this.Keys)
            {
                result[key] = this[key];
            }
            return result;
        }
    }
}
