using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class ClanWarehouseHistory : DataSource
    {
        public const string TableName = "clan_warehouse_history";
        public const string Column_char_name = "char_name";
        public const string Column_item_name = "item_name";
        public const string Column_record_time = "record_time";
        public const string Column_id = "id";
        public const string Column_clan_id = "clan_id";
        public const string Column_item_count = "item_count";
        public const string Column_type = "type";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.ClanWarehouseHistory; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_char_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_item_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_record_time, DbType = DbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_clan_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_item_count, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_type, DbType = DbType.Boolean, IsPKey = false},
        };
        public ClanWarehouseHistory() : base(TableName)
        {
            
        }
    }
}
