using System.Data.Common;
using System.Data.OleDb;

namespace Ku.db
{
    public class AccessDb : KuDb
    {
        public AccessDb() { }
        public AccessDb(string connectstring) : base(connectstring) { }

        protected override DbConnection InitConnection() => new OleDbConnection(ConnectString);
        protected override KuSqlBuilder InitBuilder() => new AccessBuilder();
    }
    public class AccessBuilder : KuSqlBuilder
    {
        public AccessBuilder() : base() { }
        public AccessBuilder(string from) : base(from) { }
    }
}
