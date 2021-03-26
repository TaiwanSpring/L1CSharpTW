using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class LogEnchant : DataSource
    {
        public const string TableName = "log_enchant";
        public const string Column_id = "id";
        public const string Column_char_id = "char_id";
        public const string Column_item_id = "item_id";
        public const string Column_old_enchantlvl = "old_enchantlvl";
        public const string Column_new_enchantlvl = "new_enchantlvl";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.LogEnchant; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_char_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_item_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_old_enchantlvl, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_enchantlvl, DbType = DbType.Int32, IsPKey = false},
        };
        public LogEnchant() : base(TableName)
        {
            
        }
    }
}
