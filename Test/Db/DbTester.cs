using Ku.db;

namespace Test.Db
{
    class DbTester
    {
        KuDb db;
        string dbString = "";
        internal void Start()
        {
            db = new SqliteDb("data.db");
            
        }
    }
}
