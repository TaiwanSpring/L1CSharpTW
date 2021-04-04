using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class ArmorSet : DataSource
    {
        public const string TableName = "armor_set";
        public const string Column_note = "note";
        public const string Column_sets = "sets";
        public const string Column_id = "id";
        public const string Column_polyid = "polyid";
        public const string Column_ac = "ac";
        public const string Column_hp = "hp";
        public const string Column_mp = "mp";
        public const string Column_hpr = "hpr";
        public const string Column_mpr = "mpr";
        public const string Column_mr = "mr";
        public const string Column_str = "str";
        public const string Column_dex = "dex";
        public const string Column_con = "con";
        public const string Column_wis = "wis";
        public const string Column_cha = "cha";
        public const string Column_intl = "intl";
        public const string Column_hit_modifier = "hit_modifier";
        public const string Column_dmg_modifier = "dmg_modifier";
        public const string Column_bow_hit_modifier = "bow_hit_modifier";
        public const string Column_bow_dmg_modifier = "bow_dmg_modifier";
        public const string Column_sp = "sp";
        public const string Column_defense_water = "defense_water";
        public const string Column_defense_wind = "defense_wind";
        public const string Column_defense_fire = "defense_fire";
        public const string Column_defense_earth = "defense_earth";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.ArmorSet; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_sets, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_polyid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ac, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hpr, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mpr, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mr, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_str, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_dex, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_con, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_wis, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_cha, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_intl, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hit_modifier, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_dmg_modifier, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_bow_hit_modifier, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_bow_dmg_modifier, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_sp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_defense_water, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_defense_wind, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_defense_fire, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_defense_earth, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public ArmorSet() : base(TableName)
        {
            
        }
    }
}
