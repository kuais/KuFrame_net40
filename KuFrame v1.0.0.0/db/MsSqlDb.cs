using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace Ku.db
{
    public class MsSqlDb : KuDb
    {
        
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
    }
}
