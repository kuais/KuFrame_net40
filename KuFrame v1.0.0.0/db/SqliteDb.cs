using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;

namespace Ku.db
{
    public class SqliteDb : KuDb
    {
        public SqliteDb()
        {
            Builder = new SqliteBuilder();
        }
        public SqliteDb(string connectstring) : base(connectstring)
        {
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
        public void SetPassword(string password)
        {
            SQLiteConnection conn = _conn as SQLiteConnection;
            var connSb = new SQLiteConnectionStringBuilder();
            connSb.Password = password;
            conn.ConnectionString = connSb.ToString();
        }
    }
    public class SqliteBuilder : KuSqlBuilder
    {
        public SqliteBuilder() : base() { }
        public SqliteBuilder(string from) : base(from) { }

        public string ListTableNames() => "SELECT name FROM sqlite_master WHERE TYPE='table'";

        public override string Page(int page, int pageSize)
        {
            if ((page <= 0) || (pageSize <= 0)) return Sql;
            if (string.IsNullOrEmpty(Sql)) return Sql;
            page--;
            return $"{Sql} LIMIT {pageSize * page},{pageSize * (page + 1)}";
        }
    }
}
