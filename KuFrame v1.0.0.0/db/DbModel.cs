using System.Collections.Generic;

namespace Ku.db
{
    public class DbModel : KuModel
    {
        protected string table;
        public virtual string Table { get; set; }

        public virtual List<string> Create() { return null; }
        public virtual List<string> Upgrade() { return null; }
        public virtual string Filter(DbModel input) => "WHERE 1=1";
        public string Filter() => Filter(this);
        public virtual string Order() => "";

        public virtual void InsertCheck(KuDb db) { }
        public virtual void UpdateCheck(KuDb db) { }

        protected List<string> Sql_UpgradeTable(string[] fields)
        {
            var s = string.Join(",", fields);
            List<string> list = new List<string>();
            list.Add($"ALTER TABLE {this.Table} RENAME TO tmp");     //将当前表重命名为tmp
            list.AddRange(this.Create());
            list.Add($"INSERT INTO {this.Table} ({s}) SELECT {s} FROM tmp"); //从tmp将数据拷回来
            list.Add($@"
UPDATE sqlite_sequence SET seq=(
    SELECT seq FROM sqlite_sequence WHERE name = 'tmp'
) WHERE name='{this.Table}'");                                //更新sqlite_sequence
            list.Add("DROP TABLE tmp");                                                 //删除tmp
            return list;
        }

    }
}
