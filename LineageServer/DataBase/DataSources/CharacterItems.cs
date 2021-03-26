using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class CharacterItems : DataSource
    {
        public const string TableName = "character_items";
        public const string Column_item_name = "item_name";
        public const string Column_last_used = "last_used";
        public const string Column_id = "id";
        public const string Column_item_id = "item_id";
        public const string Column_char_id = "char_id";
        public const string Column_count = "count";
        public const string Column_is_equipped = "is_equipped";
        public const string Column_enchantlvl = "enchantlvl";
        public const string Column_is_id = "is_id";
        public const string Column_durability = "durability";
        public const string Column_charge_count = "charge_count";
        public const string Column_remaining_time = "remaining_time";
        public const string Column_bless = "bless";
        public const string Column_attr_enchant_kind = "attr_enchant_kind";
        public const string Column_attr_enchant_level = "attr_enchant_level";
        public const string Column_firemr = "firemr";
        public const string Column_watermr = "watermr";
        public const string Column_earthmr = "earthmr";
        public const string Column_windmr = "windmr";
        public const string Column_addsp = "addsp";
        public const string Column_addhp = "addhp";
        public const string Column_addmp = "addmp";
        public const string Column_hpr = "hpr";
        public const string Column_mpr = "mpr";
        public const string Column_m_def = "m_def";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.CharacterItems; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_item_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_last_used, DbType = DbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_item_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_char_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_count, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_is_equipped, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_enchantlvl, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_is_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_durability, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_charge_count, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_remaining_time, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_bless, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_attr_enchant_kind, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_attr_enchant_level, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_firemr, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_watermr, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_earthmr, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_windmr, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_addsp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_addhp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_addmp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hpr, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mpr, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_m_def, DbType = DbType.Int32, IsPKey = false},
        };
        public CharacterItems() : base(TableName)
        {
            
        }
    }
}
