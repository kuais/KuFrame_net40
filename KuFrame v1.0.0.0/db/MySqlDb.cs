using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Threading;

namespace Ku.db
{
    public class MySqlDb : KuDb
    {
        private static readonly ReaderWriterLockSlim lock0 = new ReaderWriterLockSlim();

        public MySqlDb() { }
        public MySqlDb(string connectstring) : base(connectstring) { }

        protected override DbConnection InitConnection() => new MySqlConnection(ConnectString);
        protected override KuSqlBuilder InitBuilder() => new MySqlBuilder();
    }
    public class MySqlBuilder : KuSqlBuilder
    {
        public MySqlBuilder() : base() {}
        public MySqlBuilder(string from) : base(from) {}

        protected override string ToField(string v) => $"`{v}`";

        public override string Page(int page, int pageSize)
        {
            if ((page <= 0) || (pageSize <= 0)) return Sql;
            if (string.IsNullOrEmpty(Sql)) return Sql;
            page--;
            return $"{Sql} LIMIT {pageSize * page},{pageSize * (page + 1)}";
        }
    }
}
