using System.Data;
using LineageServer.Enum;
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
            new ColumnInfo() { Column = Column_note, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_sets, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_polyid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ac, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hpr, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mpr, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mr, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_str, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_dex, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_con, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_wis, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_cha, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_intl, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hit_modifier, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_dmg_modifier, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_bow_hit_modifier, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_bow_dmg_modifier, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_sp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_defense_water, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_defense_wind, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_defense_fire, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_defense_earth, DbType = DbType.Int32, IsPKey = false},
        };
        public ArmorSet() : base(TableName)
        {
            
        }
    }
}
