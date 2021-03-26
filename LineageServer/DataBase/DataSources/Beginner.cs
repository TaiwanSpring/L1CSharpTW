using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class Beginner : DataSource
    {
        public const string TableName = "beginner";
        public const string Column_activate = "activate";
        public const string Column_item_name = "item_name";
        public const string Column_id = "id";
        public const string Column_item_id = "item_id";
        public const string Column_count = "count";
        public const string Column_charge_count = "charge_count";
        public const string Column_enchantlvl = "enchantlvl";
        public const string Column_bless = "bless";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Beginner; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_activate, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_item_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_item_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_count, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_charge_count, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_enchantlvl, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_bless, DbType = DbType.Int32, IsPKey = false},
        };
        public Beginner() : base(TableName)
        {
            
        }
    }
}
