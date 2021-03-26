using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class Etcitem : DataSource
    {
        public const string TableName = "etcitem";
        public const string Column_name = "name";
        public const string Column_unidentified_name_id = "unidentified_name_id";
        public const string Column_identified_name_id = "identified_name_id";
        public const string Column_item_type = "item_type";
        public const string Column_use_type = "use_type";
        public const string Column_material = "material";
        public const string Column_item_id = "item_id";
        public const string Column_weight = "weight";
        public const string Column_invgfx = "invgfx";
        public const string Column_grdgfx = "grdgfx";
        public const string Column_itemdesc_id = "itemdesc_id";
        public const string Column_stackable = "stackable";
        public const string Column_max_charge_count = "max_charge_count";
        public const string Column_dmg_small = "dmg_small";
        public const string Column_dmg_large = "dmg_large";
        public const string Column_min_lvl = "min_lvl";
        public const string Column_max_lvl = "max_lvl";
        public const string Column_locx = "locx";
        public const string Column_locy = "locy";
        public const string Column_mapid = "mapid";
        public const string Column_bless = "bless";
        public const string Column_trade = "trade";
        public const string Column_cant_delete = "cant_delete";
        public const string Column_can_seal = "can_seal";
        public const string Column_delay_id = "delay_id";
        public const string Column_delay_time = "delay_time";
        public const string Column_delay_effect = "delay_effect";
        public const string Column_food_volume = "food_volume";
        public const string Column_save_at_once = "save_at_once";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Etcitem; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_unidentified_name_id, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_identified_name_id, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_item_type, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_use_type, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_material, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_item_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_weight, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_invgfx, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_grdgfx, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_itemdesc_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_stackable, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_max_charge_count, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_dmg_small, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_dmg_large, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_min_lvl, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_max_lvl, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locx, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locy, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mapid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_bless, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_trade, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_cant_delete, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_can_seal, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_delay_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_delay_time, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_delay_effect, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_food_volume, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_save_at_once, DbType = DbType.Boolean, IsPKey = false},
        };
        public Etcitem() : base(TableName)
        {
            
        }
    }
}
