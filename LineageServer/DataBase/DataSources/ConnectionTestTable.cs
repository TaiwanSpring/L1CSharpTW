using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class ConnectionTestTable : DataSource
    {
        public const string TableName = "connection_test_table";
        public const string Column_a = "a";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.ConnectionTestTable; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_a, DbType = DbType.String, IsPKey = false},
        };
        public ConnectionTestTable() : base(TableName)
        {
            
        }
    }
}
