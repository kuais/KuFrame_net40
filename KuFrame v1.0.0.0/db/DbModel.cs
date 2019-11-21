using System.Collections.Generic;
using System.Reflection;

namespace Ku.db
{
    public class DbModel : Dictionary<string, object>
    {
        protected string table;
        public virtual string Table { get; set; }

        public DbModel CopyFrom(DbModel m)
        {
            foreach (var key in m.Keys) this[key] = m[key];
            return this;
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
        public DbModel FromObject<T>(T t)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo p in properties)
            {
                this[p.Name] = p.GetValue(t, null);
            }
            return this;
        }

        public virtual string Filter(DbModel input) => "WHERE 1=1";
        public string Filter() => Filter(this);
        public virtual string Order() => "";

        public virtual void InsertCheck(KuDb db) { }
        public virtual void UpdateCheck(KuDb db) { }

        
    }
}
