using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;

namespace Ku.db
{
    public class SqliteDb : KuDb
    {
        public SqliteDb(string connectstring) : base(connectstring) {
            Builder = new SqliteBuilder();
        }

        protected override DbConnection InitConnection()
        {
            return new SQLiteConnection(ConnectString);
        }
        public List<DbModel> ListTableNames() => Query(((SqliteBuilder)Builder).ListTableNames());

        //public void ChangePassword(string password)
        //{
        //    SQLiteConnection conn = _conn as SQLiteConnection;
        //    conn.ChangePassword(password);
        //}
        //public void SetPassword(string password)
        //{
        //    SQLiteConnection conn = _conn as SQLiteConnection;
        //    conn.SetPassword(password);
        //}
    }
    public class SqliteBuilder : KuSqlBuilder
    {
        public SqliteBuilder() : base() { }
        public SqliteBuilder(string from) : base(from) { }

        public string ListTableNames() => "SELECT name FROM sqlite_master WHERE TYPE='table'";
    }
}
