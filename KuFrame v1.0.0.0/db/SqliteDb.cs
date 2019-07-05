using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;

namespace Ku.db
{
    public class SqliteDb : KuDb
    {
        public SqliteDb(string connectstring) : base(connectstring) { }

        protected override DbConnection InitConnection()
        {
            return new SQLiteConnection(ConnectString);
        }
        public int GetCurrentRowID(string table)
        {
            string sql = string.Format("SELECT ident_current('{0}');", table);
            var e = Query(sql)[0].Values.GetEnumerator();               //可以用ExecuteScalar 简化
            e.MoveNext();
            return System.Convert.ToInt32(e.Current) + 1;
        }
        public void ChangePassword(string password)
        {
            SQLiteConnection conn = _conn as SQLiteConnection;
            conn.ChangePassword(password);
        }
        public void SetPassword(string password)
        {
            SQLiteConnection conn = _conn as SQLiteConnection;
            conn.SetPassword(password);
        }

        public List<DbModel> ListTableNames()
        {
            string sql = "SELECT name FROM sqlite_master WHERE TYPE='table'";
            return Query(sql);
        }
    }
    public class SqliteBuilder : KuSqlBuilder
    {
        public SqliteBuilder() : base() { }
        public SqliteBuilder(string from) : base(from) { }
    }
}
