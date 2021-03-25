using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class Castle : DataSourceTable
    {
        public const string TableName = "castle";
        public const string Column_name = "name";
        public const string Column_war_time = "war_time";
        public const string Column_castle_id = "castle_id";
        public const string Column_tax_rate = "tax_rate";
        public const string Column_public_money = "public_money";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_war_time, DbType = DbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_castle_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_tax_rate, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_public_money, DbType = DbType.Int32, IsPKey = false},
        };
        public Castle() : base(TableName)
        {
            
        }
    }
}
