using LineageServer.Interfaces;
using LineageServer.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace LineageServer.DataBase
{
    class DataSourceRow : ContainerObject, IDataSourceRow, IDataSourceQuery
    {
        enum DataBaseRowStatusEnum
        {
            Select,
            Insert,
            Update,
            Delete,
        }

        private static object locker = new object();

        private readonly IDbConnection dbConnection;
        private readonly string tableName;
        private readonly ColumnInfo[] columnInfos;
        private Dictionary<string, object> data = new Dictionary<string, object>();
        private Dictionary<string, object> where = new Dictionary<string, object>();
        private Dictionary<string, object> whereNot = new Dictionary<string, object>();
        private Dictionary<string, object> set = new Dictionary<string, object>();
        private Dictionary<string, string> setToColumn = new Dictionary<string, string>();

        private string orderBy;
        private string orderByDesc;
        private DataBaseRowStatusEnum dataBaseRowStatus;

        public bool HaveData { get { return this.data.Count != 0; } }

        public DataSourceRow(string tableName, ColumnInfo[] columnInfos)
        {
            this.dbConnection = Container.Resolve<IDbConnection>();
            this.tableName = tableName;
            this.columnInfos = columnInfos;
            if (this.dbConnection.State != ConnectionState.Open)
            {
                this.dbConnection.Open();
            }
        }
        private IDbCommand BuilderSelectCommand()
        {
            IDbCommand dbCommand = this.dbConnection.CreateCommand();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"SELECT * FROM {this.tableName} WHERE 1 = 1 ");
            for (int i = 0; i < this.columnInfos.Length; i++)
            {
                if (this.columnInfos[i].IsPKey)
                {
                    if (this.where.ContainsKey(this.columnInfos[i].Column))
                    {
                        MySqlParameter dataParameter = dbCommand.CreateParameter() as MySqlParameter;
                        stringBuilder.Append($"AND {this.columnInfos[i].Column} = @{this.columnInfos[i].Column} ");
                        dataParameter.ParameterName = $"@{this.columnInfos[i].Column}";
                        dataParameter.Value = this.where[this.columnInfos[i].Column];
                        dataParameter.MySqlDbType = this.columnInfos[i].MySqlDbType;
                        dbCommand.Parameters.Add(dataParameter);
                    }
                    else if (this.whereNot.ContainsKey(this.columnInfos[i].Column))
                    {
                        MySqlParameter dataParameter = dbCommand.CreateParameter() as MySqlParameter;
                        stringBuilder.Append($"AND {this.columnInfos[i].Column} != @{this.columnInfos[i].Column} ");
                        dataParameter.ParameterName = $"@{this.columnInfos[i].Column}";
                        dataParameter.Value = this.whereNot[this.columnInfos[i].Column];
                        dataParameter.MySqlDbType = this.columnInfos[i].MySqlDbType;
                        dbCommand.Parameters.Add(dataParameter);
                    }

                }
            }

            if (!string.IsNullOrEmpty(this.orderBy))
            {
                stringBuilder.Append($" ORDER BY {this.orderBy}");
            }
            else if (!string.IsNullOrEmpty(this.orderByDesc))
            {
                stringBuilder.Append($" ORDER BY {this.orderByDesc} DESC");
            }
            dbCommand.CommandText = stringBuilder.ToString();
            return dbCommand;
        }
        private IDbCommand BuilderInsertCommand()
        {
            if (this.set.Count == 0)
            {
                throw new Exception("No data to insert.");
            }
            IDbCommand dbCommand = this.dbConnection.CreateCommand();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"INSERT INTO {this.tableName} (");
            for (int i = 0; i < this.columnInfos.Length; i++)
            {
                if (this.set.ContainsKey(this.columnInfos[i].Column))
                {
                    stringBuilder.Append($"{this.columnInfos[i].Column}, ");
                }
            }

            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            stringBuilder.Append(") VALUES (");
            for (int i = 0; i < this.columnInfos.Length; i++)
            {
                if (this.set.ContainsKey(this.columnInfos[i].Column))
                {
                    MySqlParameter dataParameter = dbCommand.CreateParameter() as MySqlParameter;
                    stringBuilder.Append($"@{this.columnInfos[i].Column}, ");
                    dataParameter.ParameterName = $"@{this.columnInfos[i].Column}";
                    dataParameter.Value = this.set[this.columnInfos[i].Column];
                    dataParameter.MySqlDbType = this.columnInfos[i].MySqlDbType;
                    dbCommand.Parameters.Add(dataParameter);
                }
            }
            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            stringBuilder.Append(")");
            dbCommand.CommandText = stringBuilder.ToString();
            return dbCommand;
        }
        private IDbCommand BuilderUpdateCommand()
        {
            if (this.set.Count == 0)
            {
                throw new Exception("No data to insert.");
            }
            IDbCommand dbCommand = this.dbConnection.CreateCommand();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"UPDATE {this.tableName} SET ");

            StringBuilder setBuilder = new StringBuilder();
            StringBuilder setToColumnBuilder = new StringBuilder();

            for (int i = 0; i < this.columnInfos.Length; i++)
            {
                //if (!this.columnInfos[i].IsPKey)
                {
                    if (this.set.ContainsKey(this.columnInfos[i].Column))
                    {
                        MySqlParameter dataParameter = dbCommand.CreateParameter() as MySqlParameter;
                        setBuilder.Append($"{this.columnInfos[i].Column} = @{this.columnInfos[i].Column}, ");
                        dataParameter.ParameterName = $"@{this.columnInfos[i].Column}";
                        dataParameter.Value = this.set[this.columnInfos[i].Column];
                        dataParameter.MySqlDbType = this.columnInfos[i].MySqlDbType;
                        dbCommand.Parameters.Add(dataParameter);
                    }
                    else if (this.setToColumn.ContainsKey(this.columnInfos[i].Column))
                    {
                        setToColumnBuilder.Append($"{this.columnInfos[i].Column} = {this.setToColumn[this.columnInfos[i].Column]}, ");
                    }
                }
            }
            stringBuilder.Append(setToColumnBuilder);
            stringBuilder.Append(setBuilder);
            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            if (this.where.Count > 0 || this.whereNot.Count > 0)
            {
                stringBuilder.Append($" WHERE ");

                for (int i = 0; i < this.columnInfos.Length; i++)
                {
                    if (this.columnInfos[i].IsPKey)
                    {
                        if (this.where.ContainsKey(this.columnInfos[i].Column))
                        {
                            MySqlParameter dataParameter = dbCommand.CreateParameter() as MySqlParameter;
                            stringBuilder.Append($"{this.columnInfos[i].Column} = @{this.columnInfos[i].Column} AND ");
                            dataParameter.ParameterName = $"@{this.columnInfos[i].Column}";
                            dataParameter.Value = this.where[this.columnInfos[i].Column];
                            dataParameter.MySqlDbType = this.columnInfos[i].MySqlDbType;
                            dbCommand.Parameters.Add(dataParameter);
                        }
                        else if (this.whereNot.ContainsKey(this.columnInfos[i].Column))
                        {
                            MySqlParameter dataParameter = dbCommand.CreateParameter() as MySqlParameter;
                            stringBuilder.Append($"{this.columnInfos[i].Column} != @{this.columnInfos[i].Column} AND ");
                            dataParameter.ParameterName = $"@{this.columnInfos[i].Column}";
                            dataParameter.Value = this.whereNot[this.columnInfos[i].Column];
                            dataParameter.MySqlDbType = this.columnInfos[i].MySqlDbType;
                            dbCommand.Parameters.Add(dataParameter);
                        }
                    }
                }
                stringBuilder.Remove(stringBuilder.Length - 5, 5);
            }
            dbCommand.CommandText = stringBuilder.ToString();
            return dbCommand;
        }

        private void Reset()
        {
            this.data.Clear();
            this.set.Clear();
            this.setToColumn.Clear();
            this.where.Clear();
            this.whereNot.Clear();
            this.orderBy = string.Empty;
        }

        public bool FillData(IDataReader dataReader)
        {
            this.dataBaseRowStatus = DataBaseRowStatusEnum.Select;
            Reset();
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                if (dataReader.IsDBNull(i))
                {
                    continue;
                }
                string columnName = dataReader.GetName(i);
                if (this.data.ContainsKey(columnName))
                {
                    this.data[columnName] = dataReader.GetValue(i);
                }
                else
                {
                    this.data.Add(columnName, dataReader.GetValue(i));
                }
            }
            return true;
        }
        public IDbCommand BuilderDeleteCommand()
        {
            IDbCommand dbCommand = this.dbConnection.CreateCommand();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"DELETE FROM {this.tableName} WHERE ");
            for (int i = 0; i < this.columnInfos.Length; i++)
            {
                if (this.columnInfos[i].IsPKey)
                {
                    if (this.where.ContainsKey(this.columnInfos[i].Column))
                    {
                        MySqlParameter dataParameter = dbCommand.CreateParameter() as MySqlParameter;
                        stringBuilder.Append($"{this.columnInfos[i].Column} = @{this.columnInfos[i].Column} AND ");
                        dataParameter.ParameterName = $"@{this.columnInfos[i].Column}";
                        dataParameter.Value = where[this.columnInfos[i].Column];
                        dataParameter.MySqlDbType = this.columnInfos[i].MySqlDbType;
                        dbCommand.Parameters.Add(dataParameter);
                    }
                    else if (this.whereNot.ContainsKey(this.columnInfos[i].Column))
                    {
                        MySqlParameter dataParameter = dbCommand.CreateParameter() as MySqlParameter;
                        stringBuilder.Append($"{this.columnInfos[i].Column} != @{this.columnInfos[i].Column} AND ");
                        dataParameter.ParameterName = $"@{this.columnInfos[i].Column}";
                        dataParameter.Value = whereNot[this.columnInfos[i].Column];
                        dataParameter.MySqlDbType = this.columnInfos[i].MySqlDbType;
                        dbCommand.Parameters.Add(dataParameter);
                    }
                }
            }
            dbCommand.CommandText = stringBuilder.ToString().Substring(0, stringBuilder.Length - 5);
            return dbCommand;
        }

        IDataSourceRow IDataSourceRow.Select()
        {
            this.dataBaseRowStatus = DataBaseRowStatusEnum.Select;
            Reset();
            return this;
        }

        public IDataSourceRow Insert()
        {
            this.dataBaseRowStatus = DataBaseRowStatusEnum.Insert;
            Reset();
            return this;
        }

        public IDataSourceRow Update()
        {
            this.dataBaseRowStatus = DataBaseRowStatusEnum.Update;
            Reset();
            return this;
        }

        public IDataSourceRow Delete()
        {
            this.dataBaseRowStatus = DataBaseRowStatusEnum.Delete;
            Reset();
            return this;
        }

        IDataSourceRow IDataSourceRow.Where(string column, object value)
        {
            this.where.Add(column, value);
            return this;
        }

        public IDataSourceRow Set(string column, object value)
        {
            Debug.Assert(this.dataBaseRowStatus != DataBaseRowStatusEnum.Delete);
            this.set.Add(column, value);
            return this;
        }

        public IDataSourceRow SetToColumn(string column, string toColumn)
        {
            Debug.Assert(this.dataBaseRowStatus != DataBaseRowStatusEnum.Delete);
            this.setToColumn.Add(column, toColumn);
            return this;
        }
        IDataSourceQuery IDataSourceQuery.OrderBy(string column)
        {
            this.orderBy = column;
            return this;
        }

        //IDataSourceRow IDataSourceRow.OrderBy(string column)
        //{
        //    this.orderBy = column;
        //    return this;
        //}
        IDataSourceQuery IDataSourceQuery.OrderByDesc(string column)
        {
            this.orderByDesc = column;
            return this;
        }
        //IDataSourceQuery IDataSourceRow.OrderByDesc(string column)
        //{
        //    this.orderByDesc = column;
        //    return this;
        //}
        public void Execute()
        {
            switch (this.dataBaseRowStatus)
            {
                case DataBaseRowStatusEnum.Select:
                    {
                        lock (locker)
                        {
                            IDataReader dataReader = null;
                            try
                            {
                                IDbCommand dbCommand = BuilderSelectCommand();
                                dataReader = dbCommand.ExecuteReader(); System.Diagnostics.Debug.Print("Open");

                                if (dataReader.Read())
                                {
                                    FillData(dataReader);
                                }
                           
                            }
                            catch (Exception e)
                            {

                                throw;
                            }
                            finally
                            {
                                if (dataReader != null)
                                {
                                    dataReader.Close(); System.Diagnostics.Debug.Print("Close");
                                }
                            }
                        }
                    }
                    break;
                case DataBaseRowStatusEnum.Insert:
                    {
                        lock (locker)
                        {
                            IDbCommand dbCommand = BuilderInsertCommand();
                            dbCommand.Transaction = this.dbConnection.BeginTransaction();
                            int executeCount;
                            try
                            {
                                executeCount = dbCommand.ExecuteNonQuery();
                                dbCommand.Transaction.Commit();
                                dbCommand.Transaction.Dispose();
                            }
                            catch (Exception e)
                            {
                                dbCommand.Transaction.Rollback();
                                throw e;
                            }
                            //if (executeCount == 1)
                            //{
                            //    dbCommand.Transaction.Commit();
                            //    dbCommand = BuilderSelectCommand();
                            //    lock (locker)
                            //    {
                            //        IDataReader dataReader = dbCommand.ExecuteReader(); System.Diagnostics.Debug.Print("Open");
                            //        FillData(dataReader);
                            //        dataReader.Close(); System.Diagnostics.Debug.Print("Close");
                            //    }
                            //}
                            //else
                            //{
                            //    dbCommand.Transaction.Rollback();
                            //    throw new Exception($"{dbCommand.CommandText}");
                            //}
                        }
                    }
                    break;
                case DataBaseRowStatusEnum.Update:
                    {
                        lock (locker)
                        {
                            IDbCommand dbCommand = BuilderUpdateCommand();
                            dbCommand.Transaction = this.dbConnection.BeginTransaction();
                            int executeCount;
                            try
                            {
                                executeCount = dbCommand.ExecuteNonQuery();
                                dbCommand.Transaction.Commit();
                                dbCommand.Transaction.Dispose();
                            }
                            catch (Exception e)
                            {
                                dbCommand.Transaction.Rollback();
                                throw e;
                            }
                        }
                        //if (executeCount == 1)
                        //{
                        //    dbCommand.Transaction.Commit();
                        //    dbCommand = BuilderSelectCommand();
                        //    lock (locker)
                        //    {
                        //        IDataReader dataReader = dbCommand.ExecuteReader(); System.Diagnostics.Debug.Print("Open");
                        //        FillData(dataReader);
                        //        dataReader.Close(); System.Diagnostics.Debug.Print("Close");
                        //    }
                        //}
                        //else
                        //{
                        //    dbCommand.Transaction.Rollback();
                        //    throw new Exception($"{dbCommand.CommandText}");
                        //}
                    }
                    break;
                case DataBaseRowStatusEnum.Delete:
                    {
                        lock (locker)
                        {
                            IDbCommand dbCommand = BuilderDeleteCommand();
                            dbCommand.Transaction = this.dbConnection.BeginTransaction();
                            int executeCount;
                            try
                            {
                                executeCount = dbCommand.ExecuteNonQuery();
                                dbCommand.Transaction.Commit();
                                dbCommand.Transaction.Dispose();
                            }
                            catch (Exception e)
                            {
                                dbCommand.Transaction.Rollback();
                                throw e;
                            }
                            //if (executeCount == 1)
                            //{
                            //    this.data.Clear();
                            //    dbCommand.Transaction.Commit();
                            //}
                            //else
                            //{
                            //    dbCommand.Transaction.Rollback();
                            //    throw new Exception($"{dbCommand.CommandText}");
                            //}
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        IDataSourceRow IDataSourceRow.WhereNot(string column, object value)
        {
            this.whereNot.Add(column, value);
            return this;
        }


        IDataSourceQuery IDataSourceQuery.WhereNot(string column, object value)
        {
            this.whereNot.Add(column, value);
            return this;
        }
        IDataSourceQuery IDataSourceQuery.Where(string column, object value)
        {
            this.where.Add(column, value);
            return this;
        }

        public IList<IDataSourceRow> Query()
        {
            lock (locker)
            {
                IDataReader dataReader = null;
                try
                {
                    IList<IDataSourceRow> result = new List<IDataSourceRow>();
                    IDbCommand dbCommand = BuilderSelectCommand();

                    dataReader = dbCommand.ExecuteReader(); System.Diagnostics.Debug.Print("Open");
                    while (dataReader.Read())
                    {
                        IDataSourceRow dataSourceRow = new DataSourceRow(this.tableName, this.columnInfos);
                        dataSourceRow.FillData(dataReader);
                        result.Add(dataSourceRow);
                    }
                    return result;
                }
                catch (Exception e)
                {

                    throw;
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                        System.Diagnostics.Debug.Print("Close");
                    }
                }
            }
        }
        public IList<IDataSourceRow> Query(string command)
        {
            IList<IDataSourceRow> result = new List<IDataSourceRow>();
            IDbCommand dbCommand = this.dbConnection.CreateCommand();
            dbCommand.CommandText = command;
            lock (locker)
            {
                IDataReader dataReader = null;
                try
                {
                    dataReader = dbCommand.ExecuteReader(); System.Diagnostics.Debug.Print("Open");
                    while (dataReader.Read())
                    {
                        IDataSourceRow dataSourceRow = new DataSourceRow(this.tableName, this.columnInfos);
                        dataSourceRow.FillData(dataReader);
                        result.Add(dataSourceRow);
                    }
                }
                catch (Exception e)
                {

                    throw;
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close(); System.Diagnostics.Debug.Print("Close");
                    }
                }
            }
            return result;
        }

        public int getInt(string column)
        {
            if (this.data.ContainsKey(column))
            {
                return Convert.ToInt32(this.data[column]);
            }
            else
            {
                return 0;
            }
        }

        public string getString(string column)
        {
            if (this.data.ContainsKey(column))
            {
                return Convert.ToString(this.data[column]);
            }
            else
            {
                return string.Empty;
            }
        }

        public DateTime getTimestamp(string column)
        {
            if (this.data.ContainsKey(column))
            {
                return Convert.ToDateTime(this.data[column]);
            }
            else
            {
                return default;
            }
        }

        public short getShort(string column)
        {
            if (this.data.ContainsKey(column))
            {
                return Convert.ToInt16(this.data[column]);
            }
            else
            {
                return default;
            }
        }

        public bool getBoolean(string column)
        {
            if (this.data.ContainsKey(column))
            {
                return Convert.ToBoolean(this.data[column]);
            }
            else
            {
                return default;
            }
        }

        public byte getByte(string column)
        {
            if (this.data.ContainsKey(column))
            {
                return Convert.ToByte(this.data[column]);
            }
            else
            {
                return default;
            }
        }

        public long getLong(string column)
        {
            if (this.data.ContainsKey(column))
            {
                return Convert.ToInt64(this.data[column]);
            }
            else
            {
                return default;
            }
        }

        public byte[] getBlob(string column)
        {
            if (this.data.ContainsKey(column))
            {
                return (byte[])this.data[column];
            }
            else
            {
                return default;
            }
        }

        public double getDouble(string column)
        {
            if (this.data.ContainsKey(column))
            {
                return Convert.ToDouble(this.data[column]);
            }
            else
            {
                return default;
            }
        }

        public TimeSpan getTimeSpan(string column)
        {
            if (this.data.ContainsKey(column))
            {
                return (TimeSpan)this.data[column];
            }
            else
            {
                return default;
            }
        }
    }
}
