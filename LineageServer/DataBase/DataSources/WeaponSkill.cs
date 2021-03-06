using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class WeaponSkill : DataSource
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
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.WeaponSkill; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_weapon_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_probability, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_fix_damage, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_random_damage, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_area, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_skill_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_skill_time, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_effect_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_effect_target, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_arrow_type, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_attr, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_gfx_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_gfx_id_target, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public WeaponSkill() : base(TableName)
        {
            
        }
    }
}
