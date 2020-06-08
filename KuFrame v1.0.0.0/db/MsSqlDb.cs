using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace Ku.db
{
    public class MsSqlDb : KuDb
    {
        public MsSqlDb(){}
        public MsSqlDb(string connectstring) : base(connectstring) {
            Builder = new MsSqlBuilder();
        }
        protected override DbConnection InitConnection()
        {
            return new SqlConnection(ConnectString);
        }

        public int GetCurrentRowID(string table)
        {
            return Convert.ToInt32(ExecuteScalar(((MsSqlBuilder)Builder).GetCurrentRowID(table))) + 1;
            //var e = Query(sql)[0].Values.GetEnumerator();               //可以用ExecuteScalar 简化
            //e.MoveNext();
            //return System.Convert.ToInt32(e.Current) + 1;
        }
    }
    public class MsSqlBuilder : KuSqlBuilder
    {
        public MsSqlBuilder() : base() { }
        public MsSqlBuilder(string from) : base(from) {}

        public string GetCurrentRowID(string table) => $"SELECT ident_current('{table}')";

        public override KuSqlBuilder Order(string order = "")
        {
            if (string.IsNullOrEmpty(order))
                _order = "";
            else
                _order = string.Format("row_number() OVER(ORDER BY {0}) as RowNum,", order);
            return this;
        }
        public override string Select(string select = "*", bool distinct = false)
        {
            Sql = "";
            if (string.IsNullOrEmpty(_from)) return Sql;
            if (string.IsNullOrEmpty(select)) select = "*";
            var _distinct = (distinct) ? "DISTINCT " : "";
            Sql = $"SELECT {_distinct}{_order}{select} FROM {_from}{_filter}";
            return Sql;

        }
        /// <summary>
        /// 须在Order和Select之后执行
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pageSize">单页记录数</param>
        /// <returns>查询语句</returns>
        public override string Page(int page, int pageSize)
        {
            if ((page <= 0) || (pageSize <= 0)) return Sql;
            if (string.IsNullOrEmpty(Sql)) return Sql;
            if (!Sql.ToUpper().Contains("ORDER BY ")) return Sql;
            page--;
            return $"SELECT * FROM ({Sql}) PageT WHERE RowNum >{pageSize * page} AND RowNum <={pageSize * (page + 1)}";
        }
    }
}
