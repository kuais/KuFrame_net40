using System;
using System.Collections.Generic;

namespace Ku.db
{
    public class KuSqlBuilder
    {
        public string Sql { get; set; }
        protected string _from = "";
        protected string _order = "";
        protected string _filter = "";

        protected KuSqlBuilder() => Sql = "";
        protected KuSqlBuilder(string from) : this() => _from = from;

        public KuSqlBuilder Clear()
        {
            _filter = "";
            _order = "";
            return this;
        }
        public KuSqlBuilder From(string from)
        {
            _from = from;
            return this;
        }
        public KuSqlBuilder Filter(string filter = "")
        {
            _filter = filter;
            if (!string.IsNullOrEmpty(_filter) && (!_filter.StartsWith(" ")))
                _filter = " " + _filter;
            return this;
        }
        public virtual KuSqlBuilder Order(string order = "")
        {
            if (string.IsNullOrEmpty(order))
                _order = "";
            else
                _order = string.Format(" ORDER BY {0}", order);
            return this;
        }
        public virtual string Select(string select = "*", bool distinct = false)
        {
            Sql = "";
            if (string.IsNullOrEmpty(_from)) return Sql;
            if (string.IsNullOrEmpty(select)) select = "*";
            var _distinct = (distinct) ? "DISTINCT " : "";
            Sql = $"SELECT {_distinct}{select} FROM {_from}{_filter}{_order}";
            return Sql;
        }
        public string Delete(string target = "")
        {
            Sql = "";
            if (string.IsNullOrEmpty(_from)) return Sql;
            Sql = $"DELETE {target} FROM {_from} {_filter}";
            return Sql;
        }
        public string Insert(IDictionary<string, object> fields)
        {
            Sql = "";
            if (string.IsNullOrEmpty(_from)) return Sql;
            var keys = new string[fields.Count];
            var values = new string[fields.Count];
            fields.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; i++)
            {
                values[i] = Raw(fields[keys[i]]);
                keys[i] = ToField(keys[i]);
            }
            Sql = $"INSERT INTO {_from}({string.Join(",", keys)}) VALUES ({string.Join(",", values)})";
            return Sql;
        }
        public string Update(IDictionary<string, object> fields)
        {
            Sql = "";
            if (string.IsNullOrEmpty(_from)) return Sql;
            var keys = new string[fields.Count];
            var values = new string[fields.Count];
            fields.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; i++)
            {
                values[i] = $"{ToField(keys[i])}={Raw(fields[keys[i]])}";
            }
            Sql = $"UPDATE {_from} SET {string.Join(",", values)} {_filter}";
            return Sql;
        }
        public virtual string Page(int page, int pageSize) => Sql;

        protected virtual string ToField(string v) => $"[{v}]";

        /// <summary>
        /// 根据对象类型格式化SQL字符串
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>格式化后的字符串</returns>
        public static string Raw(object input)
        {
            if (input is null) return "NULL";
            if (input is string)
			{
                input = ((string)input).Replace("'", "''");
                return $"'{input}'";
			}
            if (input is DateTime) return $"'{(DateTime)input:yyyy-MM-dd HH:mm:ss}'";
            return input.ToString();
        }

        /// <summary>
        /// 修正SQL里的特殊符号
        /// </summary>
        /// <param name="sql">要修正的SQL语句</param>
        /// <returns></returns>
        public static string Fix(string sql)
        {
            sql = sql.Replace("'", "''");
            return sql;
        }

    }
}
