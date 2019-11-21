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
            _filter = filter;
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
            if (string.IsNullOrEmpty(_from)) throw new Exception("Please call From first!");
            if (string.IsNullOrEmpty(select)) select = "*";
            string _distinct = (distinct) ? "DISTINCT " : "";
            Sql = $"SELECT {_distinct}{_order}{select} FROM {_from} {_filter}";
            return Sql;
        }
        public string Delete(string target = "")
        {
            Sql = $"DELETE {target} FROM {_from} {_filter}";
            return Sql;
        }
        public string Insert(IDictionary<string, object> fields)
        {
            int i = 0;
            string[] arr = new string[fields.Count];
            foreach (string k in fields.Keys) arr[i++] = Raw(fields[k]);
            Sql = $"INSERT INTO {_from}({string.Join(",", fields.Keys)}) VALUES ({string.Join(",", arr)})";
            return Sql;
        }
        public string Update(IDictionary<string, object> fields)
        {
            int i = 0;
            string[] arr = new string[fields.Count];
            foreach (string k in fields.Keys) arr[i++] = string.Format("{0}={1}", k, Raw(fields[k]));
            Sql = $"UPDATE {_from} SET {string.Join(",", arr)} {_filter}";
            return Sql;
        }
        /// <summary>
        /// 须在Order和Select之后执行
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pageSize">单页记录数</param>
        /// <returns>查询语句</returns>
        public string Page(int page, int pageSize)
        {
            //if (string.IsNullOrEmpty(_order)) throw new Exception("Please call Order Function first!");
            if (string.IsNullOrEmpty(Sql)) throw new Exception("Please call Select Function first!");
            if ((page <= 0) || (pageSize <= 0)) return Sql;
            if (!Sql.ToUpper().Contains("ORDER BY ")) throw new Exception("Please call Order Function first!");
            page--;
            return $"SELECT * FROM ({Sql}) PageT WHERE RowNum >{pageSize * page} AND RowNum <={pageSize * (page + 1)}";
        }

        /// <summary>
        /// 根据对象类型格式化SQL字符串
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>格式化后的字符串</returns>
        public static string Raw(object input)
        {
            if (input is null) return "NULL";
            if (input is string) return $"'{input}'";
            if (input is DateTime) return $"'{((DateTime)input).ToString("yyyy-MM-dd HH:mm:ss")}'";
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
