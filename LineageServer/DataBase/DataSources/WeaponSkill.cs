using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class WeaponSkill : DataSourceTable
    {
        public const string TableName = "weapon_skill";
        public const string Column_note = "note";
        public const string Column_weapon_id = "weapon_id";
        public const string Column_probability = "probability";
        public const string Column_fix_damage = "fix_damage";
        public const string Column_random_damage = "random_damage";
        public const string Column_area = "area";
        public const string Column_skill_id = "skill_id";
        public const string Column_skill_time = "skill_time";
        public const string Column_effect_id = "effect_id";
        public const string Column_effect_target = "effect_target";
        public const string Column_arrow_type = "arrow_type";
        public const string Column_attr = "attr";
        public const string Column_gfx_id = "gfx_id";
        public const string Column_gfx_id_target = "gfx_id_target";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_weapon_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_probability, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_fix_damage, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_random_damage, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_area, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_skill_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_skill_time, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_effect_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_effect_target, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_arrow_type, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_attr, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_gfx_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_gfx_id_target, DbType = DbType.Int32, IsPKey = false},
        };
        public WeaponSkill() : base(TableName)
        {
            
        }
    }
}
