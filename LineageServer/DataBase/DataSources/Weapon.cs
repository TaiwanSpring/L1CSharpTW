using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class Weapon : DataSource
    {
        public const string TableName = "weapon";
        public const string Column_name = "name";
        public const string Column_unidentified_name_id = "unidentified_name_id";
        public const string Column_identified_name_id = "identified_name_id";
        public const string Column_type = "type";
        public const string Column_material = "material";
        public const string Column_item_id = "item_id";
        public const string Column_weight = "weight";
        public const string Column_invgfx = "invgfx";
        public const string Column_grdgfx = "grdgfx";
        public const string Column_itemdesc_id = "itemdesc_id";
        public const string Column_dmg_small = "dmg_small";
        public const string Column_dmg_large = "dmg_large";
        public const string Column_range = "range";
        public const string Column_safenchant = "safenchant";
        public const string Column_use_royal = "use_royal";
        public const string Column_use_knight = "use_knight";
        public const string Column_use_mage = "use_mage";
        public const string Column_use_elf = "use_elf";
        public const string Column_use_darkelf = "use_darkelf";
        public const string Column_use_dragonknight = "use_dragonknight";
        public const string Column_use_illusionist = "use_illusionist";
        public const string Column_hitmodifier = "hitmodifier";
        public const string Column_dmgmodifier = "dmgmodifier";
        public const string Column_add_str = "add_str";
        public const string Column_add_con = "add_con";
        public const string Column_add_dex = "add_dex";
        public const string Column_add_int = "add_int";
        public const string Column_add_wis = "add_wis";
        public const string Column_add_cha = "add_cha";
        public const string Column_add_hp = "add_hp";
        public const string Column_add_mp = "add_mp";
        public const string Column_add_hpr = "add_hpr";
        public const string Column_add_mpr = "add_mpr";
        public const string Column_add_sp = "add_sp";
        public const string Column_m_def = "m_def";
        public const string Column_haste_item = "haste_item";
        public const string Column_double_dmg_chance = "double_dmg_chance";
        public const string Column_magicdmgmodifier = "magicdmgmodifier";
        public const string Column_canbedmg = "canbedmg";
        public const string Column_min_lvl = "min_lvl";
        public const string Column_max_lvl = "max_lvl";
        public const string Column_bless = "bless";
        public const string Column_trade = "trade";
        public const string Column_cant_delete = "cant_delete";
        public const string Column_max_use_time = "max_use_time";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Weapon; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_unidentified_name_id, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_identified_name_id, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_type, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_material, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_item_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_weight, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_invgfx, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_grdgfx, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_itemdesc_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_dmg_small, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_dmg_large, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_range, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_safenchant, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_use_royal, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_use_knight, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_use_mage, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_use_elf, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_use_darkelf, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_use_dragonknight, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_use_illusionist, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hitmodifier, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_dmgmodifier, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_str, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_con, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_dex, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_int, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_wis, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_cha, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_hp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_mp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_hpr, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_mpr, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_sp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_m_def, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_haste_item, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_double_dmg_chance, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_magicdmgmodifier, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_canbedmg, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_min_lvl, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_max_lvl, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_bless, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_trade, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_cant_delete, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_max_use_time, DbType = DbType.Int32, IsPKey = false},
        };
        public Weapon() : base(TableName)
        {
            
        }
    }
}
