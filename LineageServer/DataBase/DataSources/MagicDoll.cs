using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class MagicDoll : DataSource
    {
        public const string TableName = "magic_doll";
        public const string Column_note = "note";
        public const string Column_item_id = "item_id";
        public const string Column_doll_id = "doll_id";
        public const string Column_make_itemid = "make_itemid";
        public const string Column_ac = "ac";
        public const string Column_hpr = "hpr";
        public const string Column_hpr_time = "hpr_time";
        public const string Column_mpr = "mpr";
        public const string Column_mpr_time = "mpr_time";
        public const string Column_hit = "hit";
        public const string Column_dmg = "dmg";
        public const string Column_dmg_chance = "dmg_chance";
        public const string Column_bow_hit = "bow_hit";
        public const string Column_bow_dmg = "bow_dmg";
        public const string Column_dmg_reduction = "dmg_reduction";
        public const string Column_dmg_reduction_chance = "dmg_reduction_chance";
        public const string Column_dmg_evasion_chance = "dmg_evasion_chance";
        public const string Column_weight_reduction = "weight_reduction";
        public const string Column_regist_stun = "regist_stun";
        public const string Column_regist_stone = "regist_stone";
        public const string Column_regist_sleep = "regist_sleep";
        public const string Column_regist_freeze = "regist_freeze";
        public const string Column_regist_sustain = "regist_sustain";
        public const string Column_regist_blind = "regist_blind";
        public const string Column_effect = "effect";
        public const string Column_effect_chance = "effect_chance";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.MagicDoll; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_item_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_doll_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_make_itemid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ac, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_hpr, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_hpr_time, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_mpr, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_mpr_time, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_hit, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_dmg, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_dmg_chance, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_bow_hit, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_bow_dmg, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_dmg_reduction, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_dmg_reduction_chance, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_dmg_evasion_chance, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_weight_reduction, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_regist_stun, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_regist_stone, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_regist_sleep, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_regist_freeze, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_regist_sustain, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_regist_blind, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_effect, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_effect_chance, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
        };
        public MagicDoll() : base(TableName)
        {
            
        }
    }
}
