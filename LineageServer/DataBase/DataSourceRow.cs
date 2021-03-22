using LineageServer.DataStruct;
using LineageServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LineageServer.DataBase
{
    abstract class DataSourceRow<TPrimarykey> : ContainerObject, IDataSourceRow
    {
        private readonly string tableName;
        private readonly string primarykeyColumn;

        protected IDictionary<string, object> dataRow = new System.Collections.Concurrent.ConcurrentDictionary<string, object>();
        public bool IsNewRow { get; private set; } = true;
        public bool IsDirty { get; private set; }

        private readonly IDbConnection connection;

        public DataSourceRow(string tableName, string primarykeyColumn)
        {
            this.connection = Container.Resolve<IDbConnection>();

            this.tableName = tableName;

            this.primarykeyColumn = primarykeyColumn;
        }

        public void FillRow(IDictionary<string, object> data)
        {
            IsNewRow = false;
            IsDirty = false;
            this.dataRow.Clear();
            foreach (var item in data)
            {
                this.dataRow.Add(item.Key, item.Value);
            }
        }

        public bool Delete()
        {
            if (this.dataRow.ContainsKey(this.primarykeyColumn))
            {
                if (ExecuteNonQuery($"DELETE FROM {this.tableName} WHERE {this.primarykeyColumn} = {GetValueFormat(this.dataRow[this.primarykeyColumn])}") == 0)
                {
                    return false;
                }
                else
                {
                    this.dataRow.Clear();
                    IsNewRow = true;
                    return true;
                };
            }
            else
            {
                return false;
            }
        }

        public T GetColumn<T>(string column)
        {
            if (this.dataRow.ContainsKey(column))
            {
                return (T)this.dataRow[column];
            }
            else
            {
                throw new Exception($"Column {column} not find.");
            }
        }

        public bool Selecte()
        {
            if (this.dataRow.Count == 0)
            {
                throw new Exception();
            }

            IsNewRow = false;

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"SELECT * FROM {this.tableName} WHERE");

            foreach (var item in this.dataRow)
            {
                stringBuilder.Append($"{item.Key} = {GetValueFormat(item.Value)}");
            }

            this.dataRow.Clear();

            IDbCommand dbCommand = this.connection.CreateCommand();
            dbCommand.CommandText = stringBuilder.ToString();
            IDataReader dataReader = dbCommand.ExecuteReader();

            if (dataReader.Read())
            {
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    this.dataRow.Add(dataReader.GetName(i), dataReader.GetValue(i));
                }
                IsDirty = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetColumn<T>(string column, T value)
        {
            if (column == this.primarykeyColumn)
            {
                throw new Exception($"Primary Column {column} can't modify.");
            }

            if (this.dataRow.ContainsKey(column))
            {
                this.dataRow[column] = value;
                IsDirty = true;
            }
            else
            {
                throw new Exception($"Column {column} not find.");
            }
        }

        public bool Update()
        {
            if (this.dataRow.ContainsKey(this.primarykeyColumn))
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (IsNewRow)
                {
                    stringBuilder.Append($"INSERT INTO {this.tableName} (");
                    StringBuilder valueStringBuilder = new StringBuilder();
                    foreach (var item in this.dataRow)
                    {
                        stringBuilder.Append(item.Key);
                        stringBuilder.Append(", ");

                        valueStringBuilder.Append(GetValueFormat(item.Value));
                        valueStringBuilder.Append(", ");
                    }
                    stringBuilder.Remove(stringBuilder.Length - 2, 2);
                    valueStringBuilder.Remove(stringBuilder.Length - 2, 2);
                    stringBuilder.Append(") VALUES (");
                    stringBuilder.Append(valueStringBuilder);
                    stringBuilder.Append(")");
                }
                else
                {
                    stringBuilder.Append($"UPDATE {this.tableName} SET ");
                    foreach (var item in this.dataRow)
                    {
                        if (item.Key == this.primarykeyColumn)
                        {
                            continue;
                        }
                        stringBuilder.Append($"{item.Key} = {GetValueFormat(item.Value)}");
                        stringBuilder.Append(", ");
                    }
                    stringBuilder.Remove(stringBuilder.Length - 2, 2);
                    stringBuilder.Append($" WHERE {this.primarykeyColumn} = {GetValueFormat(this.dataRow[this.primarykeyColumn])}");
                }
                if (ExecuteNonQuery(stringBuilder.ToString()) == 0)
                {
                    return false;
                }
                else
                {
                    IsDirty = false;
                    return true;
                };
            }
            return false;
        }

        private int ExecuteNonQuery(string command)
        {
            IDbCommand dbCommand = this.connection.CreateCommand();
            IDbTransaction dbTransaction = this.connection.BeginTransaction();
            dbCommand.CommandText = command;
            dbCommand.Transaction = dbTransaction;
            int i;
            try
            {
                i = dbCommand.ExecuteNonQuery();
                dbTransaction.Commit();
            }
            catch (Exception e)
            {
                dbTransaction.Rollback();
                throw e;
            }
            return i;
        }

        protected static string GetValueFormat(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (value.GetType() == typeof(string))
            {
                return $"'{value}'";
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
