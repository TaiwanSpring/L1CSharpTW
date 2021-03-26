using LineageServer.Enum;
using LineageServer.Interfaces;
using LineageServer.Models;
using System.Data;

namespace LineageServer.DataBase
{
    abstract class DataSource : ContainerObject, IDataSource
    {
        public abstract DataSourceTypeEnum DataSourceType { get; }
        protected abstract ColumnInfo[] ColumnInfos { get; }
        private readonly string tableName;

        public DataSource(string tableName)
        {
            this.tableName = tableName;
        }

        public IDataSourceRow NewRow()
        {
            return new DataSourceRow(this.tableName, ColumnInfos);
        }
        public IDataSourceQuery Select()
        {
            return new DataSourceRow(this.tableName, ColumnInfos);
        }
    }
}
