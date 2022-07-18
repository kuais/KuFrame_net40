using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Threading;

namespace Ku.db
{
    public class SqliteDb : KuDb
    {
        private static readonly ReaderWriterLockSlim lock0 = new ReaderWriterLockSlim();

        public SqliteDb() { }
        public SqliteDb(string connectstring) : base(connectstring) { }

        protected override DbConnection InitConnection() => new SQLiteConnection(ConnectString);
        protected override KuSqlBuilder InitBuilder() => new SqliteBuilder();
        public List<DbModel> ListTableNames() => Query(((SqliteBuilder)Builder).ListTableNames());
        public void ChangePassword(string password) => DoAction(() => ((SQLiteConnection)_conn).ChangePassword(password));
        public void SetPassword(string password)
        {
            if (_conn == null) _conn = InitConnection();
            ((SQLiteConnection)_conn).SetPassword(password);
        }
        public override void DoAction(Action action)
        {
            try
            {
                lock0.EnterWriteLock();
                Open();
                action();
            }
            finally
            {
                lock0.ExitWriteLock();
            }
        }
        public override void Open()
        {
            if (_conn == null)
                _conn = InitConnection();
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Open();
                _cmd = _conn.CreateCommand();
                _cmd.CommandTimeout = TimeOut;
            }
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
