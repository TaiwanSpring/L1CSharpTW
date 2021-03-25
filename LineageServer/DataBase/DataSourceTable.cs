using LineageServer.Interfaces;
using LineageServer.Models;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace LineageServer.DataBase
{
    abstract class DataSourceTable : ContainerObject, IDataSourceTable
    {
        protected abstract ColumnInfo[] ColumnInfos { get; }
        private readonly string tableName;
        private readonly IDbConnection connection;

        public DataSourceTable(string tableName)
        {
            this.connection = Container.Resolve<IDbConnection>();

            this.tableName = tableName;
        }

        public IList<IDataSourceRow> Selecte()
        {
            List<IDataSourceRow> result = new List<IDataSourceRow>();

            IDbCommand dbCommand = this.connection.CreateCommand();
            dbCommand.CommandText = $"SELECT * FROM {this.tableName}";
            IDataReader dataReader = dbCommand.ExecuteReader();
            while (dataReader.Read())
            {
                DataSourceRow dataSourceRow = new DataSourceRow(this.tableName, ColumnInfos);
                dataSourceRow.FillData(dataReader);
                result.Add(dataSourceRow);
            }
            dataReader.Close();
            return result;
        }

        public IDataSourceRow CreateRow()
        {
            return new DataSourceRow(this.tableName, ColumnInfos);
        }
    }
}
