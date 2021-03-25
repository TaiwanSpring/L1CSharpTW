using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class ConnectionTestTable : DataSourceTable
    {
        public const string TableName = "connection_test_table";
        public const string Column_a = "a";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_a, DbType = DbType.String, IsPKey = false},
        };
        public ConnectionTestTable() : base(TableName)
        {
            
        }
    }
}
