using LineageServer.Interfaces;
using LineageServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace LineageServer.DataBase
{
    class DataSourceRow : ContainerObject, IDataSourceRow
    {
        enum DataBaseRowStatusEnum
        {
            Select,
            Insert,
            Update,
            Delete,
        }
        private readonly IDbConnection dbConnection;
        private readonly string tableName;
        private readonly ColumnInfo[] columnInfos;
        private Dictionary<string, object> data = new Dictionary<string, object>();
        private Dictionary<string, object> where = new Dictionary<string, object>();
        private Dictionary<string, object> set = new Dictionary<string, object>();

        private DataBaseRowStatusEnum dataBaseRowStatus;

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
            stringBuilder.Append($"SELECT * FROM {this.tableName} WHERE ");
            for (int i = 0; i < this.columnInfos.Length; i++)
            {
                if (this.columnInfos[i].IsPKey)
                {
                    if (this.where.ContainsKey(this.columnInfos[i].Column))
                    {
                        IDataParameter dataParameter = dbCommand.CreateParameter();
                        stringBuilder.Append($"{this.columnInfos[i].Column} = @{this.columnInfos[i].Column} AND ");
                        dataParameter.ParameterName = $"@{this.columnInfos[i].Column}";
                        dataParameter.Value = this.where[this.columnInfos[i].Column];
                        dataParameter.DbType = this.columnInfos[i].DbType;
                        dbCommand.Parameters.Add(dataParameter);
                    }
                }
            }
            dbCommand.CommandText = stringBuilder.ToString().Substring(0, stringBuilder.Length - 5);
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
                    if (this.columnInfos[i].IsPKey)
                    {
                        this.where.Add(this.columnInfos[i].Column, this.set[this.columnInfos[i].Column]);
                    }
                    stringBuilder.Append($"{this.columnInfos[i].Column}, ");
                }
            }

            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            stringBuilder.Append(") VALUES (");
            for (int i = 0; i < this.columnInfos.Length; i++)
            {
                if (this.set.ContainsKey(this.columnInfos[i].Column))
                {
                    IDataParameter dataParameter = dbCommand.CreateParameter();
                    stringBuilder.Append($"@{this.columnInfos[i].Column}, ");
                    dataParameter.ParameterName = $"@{this.columnInfos[i].Column}";
                    dataParameter.Value = this.set[this.columnInfos[i].Column];
                    dataParameter.DbType = this.columnInfos[i].DbType;
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
            for (int i = 0; i < this.columnInfos.Length; i++)
            {
                if (!this.columnInfos[i].IsPKey)
                {
                    if (this.set.ContainsKey(this.columnInfos[i].Column))
                    {
                        IDataParameter dataParameter = dbCommand.CreateParameter();
                        stringBuilder.Append($"{this.columnInfos[i].Column} = @{this.columnInfos[i].Column}, ");
                        dataParameter.ParameterName = $"@{this.columnInfos[i].Column}";
                        dataParameter.Value = this.set[this.columnInfos[i].Column];
                        dataParameter.DbType = this.columnInfos[i].DbType;
                        dbCommand.Parameters.Add(dataParameter);
                    }
                }
            }
            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            stringBuilder.Append($" WHERE ");

            for (int i = 0; i < this.columnInfos.Length; i++)
            {
                if (this.columnInfos[i].IsPKey)
                {
                    if (this.where.ContainsKey(this.columnInfos[i].Column))
                    {
                        IDataParameter dataParameter = dbCommand.CreateParameter();
                        stringBuilder.Append($"{this.columnInfos[i].Column} = @{this.columnInfos[i].Column} AND ");
                        dataParameter.ParameterName = $"@{this.columnInfos[i].Column}";
                        dataParameter.Value = this.where[this.columnInfos[i].Column];
                        dataParameter.DbType = this.columnInfos[i].DbType;
                        dbCommand.Parameters.Add(dataParameter);
                    }
                }
            }
            dbCommand.CommandText = stringBuilder.ToString().Substring(0, stringBuilder.Length - 5);
            return dbCommand;
        }
        public bool FillData(IDataReader dataReader)
        {
            Select();

            for (int i = 0; i < dataReader.FieldCount; i++)
            {
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
                    if (this.data.ContainsKey(this.columnInfos[i].Column))
                    {
                        IDataParameter dataParameter = dbCommand.CreateParameter();
                        stringBuilder.Append($"{this.columnInfos[i].Column} = @{this.columnInfos[i].Column} AND ");
                        dataParameter.ParameterName = $"@{this.columnInfos[i].Column}";
                        dataParameter.Value = data[this.columnInfos[i].Column];
                        dataParameter.DbType = this.columnInfos[i].DbType;
                        dbCommand.Parameters.Add(dataParameter);
                    }
                }
            }
            dbCommand.CommandText = stringBuilder.ToString().Substring(0, stringBuilder.Length - 5);
            return dbCommand;
        }

        public IDataSourceRow Select()
        {
            this.dataBaseRowStatus = DataBaseRowStatusEnum.Select;
            this.data.Clear();
            this.set.Clear();
            this.where.Clear();
            return this;
        }

        public IDataSourceRow Insert()
        {
            this.dataBaseRowStatus = DataBaseRowStatusEnum.Insert;
            this.data.Clear();
            this.set.Clear();
            this.where.Clear();
            return this;
        }

        public IDataSourceRow Update()
        {
            this.dataBaseRowStatus = DataBaseRowStatusEnum.Update;
            this.data.Clear();
            this.set.Clear();
            this.where.Clear();
            return this;
        }

        public IDataSourceRow Delete()
        {
            this.dataBaseRowStatus = DataBaseRowStatusEnum.Delete;
            this.data.Clear();
            this.set.Clear();
            this.where.Clear();
            return this;
        }

        public IDataSourceRow Where(string column, object value)
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

        public void Execute()
        {
            switch (this.dataBaseRowStatus)
            {
                case DataBaseRowStatusEnum.Select:
                    {
                        IDbCommand dbCommand = BuilderSelectCommand();
                        IDataReader dataReader = dbCommand.ExecuteReader();
                        FillData(dataReader);
                        dataReader.Close();
                    }
                    break;
                case DataBaseRowStatusEnum.Insert:
                    {
                        IDbCommand dbCommand = BuilderInsertCommand();
                        dbCommand.Transaction = this.dbConnection.BeginTransaction();
                        int executeCount;
                        try
                        {
                            executeCount = dbCommand.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            dbCommand.Transaction.Rollback();
                            throw e;
                        }
                        if (executeCount == 1)
                        {
                            dbCommand.Transaction.Commit();
                            dbCommand = BuilderSelectCommand();
                            IDataReader dataReader = dbCommand.ExecuteReader();
                            FillData(dataReader);
                            dataReader.Close();
                        }
                        else
                        {
                            dbCommand.Transaction.Rollback();
                            throw new Exception($"{dbCommand.CommandText}");
                        }
                    }
                    break;
                case DataBaseRowStatusEnum.Update:
                    {
                        IDbCommand dbCommand = BuilderUpdateCommand();
                        dbCommand.Transaction = this.dbConnection.BeginTransaction();
                        int executeCount;
                        try
                        {
                            executeCount = dbCommand.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            dbCommand.Transaction.Rollback();
                            throw e;
                        }
                        if (executeCount == 1)
                        {
                            dbCommand.Transaction.Commit();
                            dbCommand = BuilderSelectCommand();
                            IDataReader dataReader = dbCommand.ExecuteReader();
                            FillData(dataReader);
                            dataReader.Close();
                        }
                        else
                        {
                            dbCommand.Transaction.Rollback();
                            throw new Exception($"{dbCommand.CommandText}");
                        }
                    }
                    break;
                case DataBaseRowStatusEnum.Delete:
                    {
                        IDbCommand dbCommand = BuilderDeleteCommand();
                        dbCommand.Transaction = this.dbConnection.BeginTransaction();
                        int executeCount;
                        try
                        {
                            executeCount = dbCommand.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            dbCommand.Transaction.Rollback();
                            throw e;
                        }
                        if (executeCount == 1)
                        {
                            this.data.Clear();
                            dbCommand.Transaction.Commit();
                        }
                        else
                        {
                            dbCommand.Transaction.Rollback();
                            throw new Exception($"{dbCommand.CommandText}");
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
