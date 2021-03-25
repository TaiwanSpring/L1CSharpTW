using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class Commands : DataSourceTable
    {
        public const string TableName = "commands";
        public const string Column_name = "name";
        public const string Column_class_name = "class_name";
        public const string Column_access_level = "access_level";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_name, DbType = DbType.String, IsPKey = true},
            new ColumnInfo() { Column = Column_class_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_access_level, DbType = DbType.Int32, IsPKey = false},
        };
        public Commands() : base(TableName)
        {
            
        }
    }
}
