using System;
using System.Collections.Generic;

namespace Ku.db
{
    public class KuSqlBuilder
    {
        public string Sql { get; set; }
        private string _from = "";
        private string _order = "";
        private string _filter = "";

        protected KuSqlBuilder()
        {
            Sql = "";
        }
        protected KuSqlBuilder(string from)
        {
            _from = from;
        }
        public KuSqlBuilder Clear()
        {
            _filter = "";
            _order = "";
            return this;
        }
        public KuSqlBuilder From(string from)
        {
            if (string.IsNullOrEmpty(from))
                throw new Exception("From should not be empty!");
            _from = from;
            return this;
        }
        public KuSqlBuilder Filter(string filter = "")
        {
            if (string.IsNullOrEmpty(filter))
                _order = "";
            else 
                _filter += filter;
            return this;
        }
        public KuSqlBuilder Page(int page, int pageSize)
        {
            page--;
            _filter += string.Format(" AND RowNum >{0} AND RowNum <={1}"
                , pageSize * page, pageSize * (page + 1));
            return this;
        }
        public KuSqlBuilder Order(string order = "")
        {
            if (string.IsNullOrEmpty(order))
                _order = "";
            else 
                _order = string.Format("row_number() OVER(ORDER BY {0}) as RowNum,", order);
            return this;
        }
        public string Select(string select = "*", bool distinct = false)
        {
            if (string.IsNullOrEmpty(_from)) throw new Exception("Please set From first!");
            if (string.IsNullOrEmpty(select)) select = "*";
            string _distinct = (distinct) ? "DISTINCT " : "";
            Sql = string.Format("SELECT {0}{1}{2} FROM {3}",_distinct, _order, select, _from);
            if (!string.IsNullOrEmpty(_filter)) Sql += " WHERE 1=1" + _filter;
            return Sql;
        }
        public string Delete(string target = "")
        {
            Sql = string.Format("DELETE {0} FROM {1}", target, _from);
            if (!string.IsNullOrEmpty(_filter)) Sql += " WHERE 1=1" + _filter;
            return Sql;
        }
        public string Insert(IDictionary<string, object> fields)
        {
            int i = 0;
            string[] arr = new string[fields.Count];
            foreach (string k in fields.Keys) arr[i++] = Raw(fields[k]);
            Sql = string.Format("INSERT INTO {0}({1}) VALUES ({2})", _from, string.Join(',', fields.Keys), string.Join(',', arr));
            return Sql;
        }
        public string Update(IDictionary<string, object> fields)
        {
            int i = 0;
            string[] arr = new string[fields.Count];
            foreach (string k in fields.Keys) arr[i++] = string.Format("{0}={1}", k, Raw(fields[k]));
            Sql = string.Format("UPDATE {0} SET {1}" , _from, string.Join(',', arr));
            if (!string.IsNullOrEmpty(_filter)) Sql += " WHERE 1=1" + _filter;
            return Sql;
        }

        public static string Raw(object value)
        {
            if (value is null) return "NULL";
            if (value is string) return "'" + value + "'" ;
            if (value is DateTime) return "'" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            return value.ToString();
        }
    }
}
