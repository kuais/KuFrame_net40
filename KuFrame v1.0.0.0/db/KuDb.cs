using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Ku.db
{
    public delegate int Transaction();
    public enum DbType { MsSql, MySql, Oracle, Sqlite }

    public abstract class KuDb
    {
        private DbCommand _cmd;
        protected DbConnection _conn;

        private KuDb() { }
        protected KuDb(string connectstring)
        {
            TimeOut = 10;
            ConnectString = connectstring;
        }
        public static KuDb NewDb(string connectstring, DbType type = DbType.MsSql)
        {
            switch (type)
            {
                case DbType.MsSql:
                    return new MsSqlDb(connectstring);
                default:
                    return null;
            }
        }
        #region 属性
        /// <summary>
        /// 超时时间
        /// </summary>
        public int TimeOut { get; set; }
        public string ConnectString { get; }
        #endregion

        #region 方法
        protected abstract DbConnection InitConnection();
        public void DoAction(System.Action action)
        {
            try
            {
                Open();
                action();
            }
            finally
            {
                Close();
            }
        }
        public void Open()
        {
            if (_conn == null)
            {
                _conn = InitConnection();
                _cmd = _conn.CreateCommand();
                _cmd.CommandTimeout = TimeOut;
            }
            Close();
            _conn.Open();
        }
        public void Close()
        {
            if (_conn == null)
                return;
            if (_conn.State == ConnectionState.Open)
                _conn.Close();
        }
        public DbModel QueryOne(string sql)
        {
            var r = Query(sql);
            return (r.Count == 0) ? null : r[0];
        }
        public List<DbModel> Query(string sql)
        {
            List<DbModel> result = new List<DbModel>();
            _cmd.CommandText = sql;
            _cmd.CommandType = CommandType.Text;
            using (DbDataReader reader = _cmd.ExecuteReader())
            {
                if (!reader.HasRows)
                    return result;
                string[] colNames = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                    colNames[i] = reader.GetName(i);                            //获取列名
                while (reader.Read())
                {                                                               //获取行数据
                    DbModel m = new DbModel();
                    for (int i = 0; i < reader.FieldCount; i++)
                        m[colNames[i]] = (reader.IsDBNull(i)) ? null : reader[i];
                    result.Add(m);
                }
                return result;
            }
        }
        public DataTable QueryTable(string sql)
        {
            DataTable result = new DataTable();
            _cmd.CommandText = sql;
            _cmd.CommandType = CommandType.Text;
            using (DbDataReader reader = _cmd.ExecuteReader())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    DataColumn myDataColumn = new DataColumn(reader.GetName(i), reader.GetFieldType(i));
                    result.Columns.Add(myDataColumn);
                }
                while (reader.Read())
                {
                    DataRow myDataRow = result.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        myDataRow[i] = reader[i];
                    }
                    result.Rows.Add(myDataRow);
                }
                return result;
            }
        }
        public List<T> Query<T>(string sql) where T : new()
        {
            List<T> result = new List<T>();
            _cmd.CommandText = sql;
            _cmd.CommandType = CommandType.Text;
            using (DbDataReader reader = _cmd.ExecuteReader())
            {
                if (!reader.HasRows)
                    return result;
                string[] colNames = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                    colNames[i] = reader.GetName(i);                            //获取列名
                PropertyInfo[] properties = typeof(T).GetProperties();
                while (reader.Read())
                {                                                               //获取行数据
                    T t = new T();
                    foreach (PropertyInfo p in properties)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (p.Name.Equals(colNames[i]))
                            {
                                p.SetValue(t, (reader.IsDBNull(i)) ? null : reader[i], null);
                                break;
                            }
                        }
                    }
                    result.Add(t);
                }
                return result;
            }
        }
        public int Execute(string sql)
        {
            _cmd.CommandType = CommandType.Text;
            _cmd.CommandText = sql;
            return _cmd.ExecuteNonQuery();
        }
        public object ExecuteScalar(string sql)
        {
            _cmd.CommandType = CommandType.Text;
            _cmd.CommandText = sql;
            return _cmd.ExecuteScalar();
        }
        public int ExecuteTransaction(List<string> sqlList)
        {
            int transaction()
            {
                int result = 0;
                foreach (string sql in sqlList)
                {
                    _cmd.CommandText = sql;
                    result += _cmd.ExecuteNonQuery();
                }
                return result;
            }
            return ExecuteTransaction(transaction);
        }
        public int ExecuteTransaction(Transaction d)
        {
            int result = 0;
            _cmd.Transaction = _conn.BeginTransaction();
            try
            {
                result = d();
                _cmd.Transaction.Commit();
            }
            catch
            {
                _cmd.Transaction.Rollback();
                throw;
            }
            finally
            {
                _cmd.Transaction = null;
            }
            return result;
        }
        #endregion
    }
}
