using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Armor : DataSource
    {
        public const string TableName = "armor";
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
        public const string Column_ac = "ac";
        public const string Column_safenchant = "safenchant";
        public const string Column_use_royal = "use_royal";
        public const string Column_use_knight = "use_knight";
        public const string Column_use_mage = "use_mage";
        public const string Column_use_elf = "use_elf";
        public const string Column_use_darkelf = "use_darkelf";
        public const string Column_use_dragonknight = "use_dragonknight";
        public const string Column_use_illusionist = "use_illusionist";
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
        public const string Column_min_lvl = "min_lvl";
        public const string Column_max_lvl = "max_lvl";
        public const string Column_m_def = "m_def";
        public const string Column_haste_item = "haste_item";
        public const string Column_damage_reduction = "damage_reduction";
        public const string Column_weight_reduction = "weight_reduction";
        public const string Column_hit_modifier = "hit_modifier";
        public const string Column_dmg_modifier = "dmg_modifier";
        public const string Column_bow_hit_modifier = "bow_hit_modifier";
        public const string Column_bow_dmg_modifier = "bow_dmg_modifier";
        public const string Column_bless = "bless";
        public const string Column_trade = "trade";
        public const string Column_cant_delete = "cant_delete";
        public const string Column_max_use_time = "max_use_time";
        public const string Column_defense_water = "defense_water";
        public const string Column_defense_wind = "defense_wind";
        public const string Column_defense_fire = "defense_fire";
        public const string Column_defense_earth = "defense_earth";
        public const string Column_regist_stun = "regist_stun";
        public const string Column_regist_stone = "regist_stone";
        public const string Column_regist_sleep = "regist_sleep";
        public const string Column_regist_freeze = "regist_freeze";
        public const string Column_regist_sustain = "regist_sustain";
        public const string Column_regist_blind = "regist_blind";
        public const string Column_grade = "grade";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Armor; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_unidentified_name_id, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_identified_name_id, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_type, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_material, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_item_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_weight, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_invgfx, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_grdgfx, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_itemdesc_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ac, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_safenchant, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_use_royal, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_use_knight, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_use_mage, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_use_elf, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_use_darkelf, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_use_dragonknight, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_use_illusionist, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_str, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_con, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_dex, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_int, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_wis, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_cha, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_hp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_mp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_hpr, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_mpr, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_sp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_min_lvl, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_max_lvl, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_m_def, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_haste_item, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_damage_reduction, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_weight_reduction, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hit_modifier, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_dmg_modifier, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_bow_hit_modifier, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_bow_dmg_modifier, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_bless, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_trade, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_cant_delete, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_max_use_time, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_defense_water, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_defense_wind, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_defense_fire, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_defense_earth, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_regist_stun, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_regist_stone, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_regist_sleep, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_regist_freeze, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_regist_sustain, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_regist_blind, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_grade, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public Armor() : base(TableName)
        {

        }
    }
}
