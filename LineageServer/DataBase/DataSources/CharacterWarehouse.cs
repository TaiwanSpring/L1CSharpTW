using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class CharacterWarehouse : DataSource
    {
        public const string TableName = "character_warehouse";
        public const string Column_account_name = "account_name";
        public const string Column_item_name = "item_name";
        public const string Column_last_used = "last_used";
        public const string Column_id = "id";
        public const string Column_item_id = "item_id";
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
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.CharacterWarehouse; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_account_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_item_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_last_used, MySqlDbType = MySqlDbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_item_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_count, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_is_equipped, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_enchantlvl, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_is_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_durability, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_charge_count, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_remaining_time, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_bless, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_attr_enchant_kind, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_attr_enchant_level, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_firemr, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_watermr, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_earthmr, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_windmr, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_addsp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_addhp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_addmp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hpr, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mpr, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_m_def, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public CharacterWarehouse() : base(TableName)
        {
            
        }
    }
}
