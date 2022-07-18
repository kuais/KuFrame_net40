using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Ku.db
{
    public delegate int Transaction();
    public enum DbType { MsSql, MySql, Oracle, Sqlite }

    public abstract class KuDb : IDisposable
    {
        protected DbCommand _cmd;
        protected DbConnection _conn;

        public KuSqlBuilder Builder { get; set; }
        public KuLog Logger { get; set; } = null;

        protected KuDb() => Builder = InitBuilder();
        protected KuDb(string connectstring) : this() => ConnectString = connectstring;

        public static KuDb NewDb(string connectstring, DbType type = DbType.MsSql)
        {
            switch (type)
            {
                case DbType.MsSql:
                    return new MsSqlDb(connectstring);
                case DbType.Sqlite:
                    return new SqliteDb(connectstring);
                default:
                    return null;
            }
        }

        #region 属性
        /// <summary>
        /// 超时时间
        /// </summary>
        public int TimeOut { get; set; } = 10;
        public string ConnectString { get; set; }
        #endregion

        #region 方法
        protected abstract DbConnection InitConnection();
        protected abstract KuSqlBuilder InitBuilder();
        private void LogSQL(string sql) => Logger?.Log(sql, "SQL");
        public virtual void DoAction(Action action)
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
        public virtual void Open()
        {
            if (_conn == null)
                _conn = InitConnection();
            if (_conn.State == ConnectionState.Open)
                _conn.Close();
            _conn.Open();
            _cmd = _conn.CreateCommand();
            _cmd.CommandTimeout = TimeOut;
        }
        public void Close()
        {
            if (_conn == null) return;
            if (_conn.State == ConnectionState.Open)
                _conn.Close();
            if (_cmd != null) _cmd.Dispose();
            //_conn = null;
        }
        public void Dispose()
        {
            Close();
            _conn = null;
        }

        public List<DbModel> Query(string sql)
        {
            _cmd.CommandText = sql;
            _cmd.CommandType = CommandType.Text;
            using (DbDataReader reader = _cmd.ExecuteReader())
            {
                List<DbModel> result = new List<DbModel>();
                if (!reader.HasRows) return result;
                if (reader.VisibleFieldCount == 0) return result;
                var colNames = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                    colNames[i] = reader.GetName(i);                            //获取列名
                while (reader.Read())
                {                                                               //获取行数据
                    var m = new DbModel();
                    for (int i = 0; i < reader.FieldCount; i++)
                        m[colNames[i]] = (reader.IsDBNull(i)) ? null : reader[i];
                    result.Add(m);
                }
                return result;
            }
        }
        public DbModel QueryOne(string sql)
        {
            _cmd.CommandText = sql;
            _cmd.CommandType = CommandType.Text;
            using (DbDataReader reader = _cmd.ExecuteReader())
            {
                if (!reader.HasRows) return null;
                var colNames = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                    colNames[i] = reader.GetName(i);                            //获取列名
                reader.Read();
                var m = new DbModel();
                for (int i = 0; i < reader.FieldCount; i++)
                    m[colNames[i]] = (reader.IsDBNull(i)) ? null : reader[i];
                return m;
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
            LogSQL(sql);
            _cmd.CommandType = CommandType.Text;
            _cmd.CommandText = sql;
            return _cmd.ExecuteNonQuery();
        }
        public object ExecuteScalar(string sql)
        {
            LogSQL(sql);
            _cmd.CommandType = CommandType.Text;
            _cmd.CommandText = sql;
            return _cmd.ExecuteScalar();
        }
        public int ExecuteTransaction(List<string> sqlList)
        {
            return ExecuteTransaction(() =>
            {
                int result = 0;
                foreach (var sql in sqlList)
                {
                    LogSQL(sql);
                    _cmd.CommandText = sql;
                    result += _cmd.ExecuteNonQuery();
                }
                return result;
            });
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

        #region 快捷方法
        public int QueryCount(string from, string filter = "")
        {
            return Convert.ToInt32(ExecuteScalar(Builder.From(from).Filter(filter).Select("COUNT(1)")));
        }
        public DbModel QueryOne(QueryParam param)
        {
            Builder.From(param.From).Filter(param.Filter);
            return QueryOne(Builder.Order(param.Order).Select(param.Select));
        }
        public dynamic Query(QueryParam param = null)
        {
            Builder.From(param.From).Filter(param.Filter);
            if (param.Page == 0 || param.PageSize == 0)
            {
                return Query(Builder.Order(param.Order).Select(param.Select));
            }
            else
            {
                //total
                var total = ExecuteScalar(Builder.Select("COUNT(1)"));
                //datas
                Builder.Order(param.Order).Select(param.Select);
                var datalist = Query(Builder.Page(param.Page, param.PageSize));
                return new DbModel
                {
                    ["total"] = total,
                    ["list"] = datalist
                };
            }
        }
        public int Insert(DbModel m)
        {
            return Execute(InsertSQL(m));
        }
        public int Update(DbModel m, string filter = "")
        {
            return Execute(UpdateSQL(m, filter));
        }
        public int Delete(DbModel m, string filter = "")
        {
            return Execute(DeleteSQL(m, filter));
        }
        public int Delete(string from, string filter = "")
        {
            return Execute(DeleteSQL(from, filter));
        }
        //public string SelectSQL(DbModel m, string filter = "")
        //{
        //    if (string.IsNullOrEmpty(filter)) filter = m.Filter();
        //    return Builder.From(m.Table).Filter(filter).Select();
        //}
        public string SelectSQL(QueryParam param)
        {
            Builder.From(param.From).Filter(param.Filter).Order(param.Order).Select(param.Select);
            return Builder.Page(param.Page, param.PageSize);
        }
        public virtual string InsertSQL(DbModel m)
        {
            return Builder.From(m.Table).Insert(m);
        }
        public virtual string UpdateSQL(DbModel m, string filter = "")
        {
            if (string.IsNullOrEmpty(filter)) filter = m.Filter();
            return Builder.From(m.Table).Filter(filter).Update(m);
        }
        public string DeleteSQL(DbModel m, string filter = "")
        {
            if (string.IsNullOrEmpty(filter)) filter = m.Filter();
            return DeleteSQL(m.Table, filter);
        }
        public string DeleteSQL(string from, string filter = "")
        {
            return Builder.From(from).Filter(filter).Delete();
        }
        #endregion
    }

    public class QueryParam
    {
        public string From { get; set; }
        public string Filter { get; set; } = "";
        public string Select { get; set; } = "*";
        public string Order { get; set; }
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 0;

        public QueryParam()
        {
        }
        public QueryParam(DbModel m)
        {
            From = m.Table;
            Filter = m.Filter();
            Order = m.Order();
        }
        public QueryParam FromModel(DbModel m)
        {
            if (string.IsNullOrEmpty(From)) From = m.Table;
            if (string.IsNullOrEmpty(Filter)) Filter = m.Filter();
            if (string.IsNullOrEmpty(Order)) Order = m.Order();
            return this;
        }
    }
}
