using System.Data.Common;
using System.Data.SqlClient;

namespace Ku.db
{
    public class MsSqlDb : KuDb
    {
        internal MsSqlDb(string connectstring) : base(connectstring) {}

        protected override DbConnection InitConnection()
        {
            return new SqlConnection(ConnectString);
        }

        public int GetCurrentRowID(string table)
        {
            string sql = string.Format("SELECT ident_current('{0}');", table);
            var e = Query(sql)[0].Values.GetEnumerator();
            e.MoveNext();
            return System.Convert.ToInt32(e.Current) + 1;
        }
    }
    public class MsSqlBuilder : KuSqlBuilder
    {
        public MsSqlBuilder() : base() { }
        public MsSqlBuilder(string from) : base(from) {}
    }
}
