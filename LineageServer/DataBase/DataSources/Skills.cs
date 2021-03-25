using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class Skills : DataSourceTable
    {
        public const string TableName = "skills";
        public const string Column_name = "name";
        public const string Column_target = "target";
        public const string Column_nameid = "nameid";
        public const string Column_skill_id = "skill_id";
        public const string Column_skill_level = "skill_level";
        public const string Column_skill_number = "skill_number";
        public const string Column_mpConsume = "mpConsume";
        public const string Column_hpConsume = "hpConsume";
        public const string Column_itemConsumeId = "itemConsumeId";
        public const string Column_itemConsumeCount = "itemConsumeCount";
        public const string Column_reuseDelay = "reuseDelay";
        public const string Column_buffDuration = "buffDuration";
        public const string Column_target_to = "target_to";
        public const string Column_damage_value = "damage_value";
        public const string Column_damage_dice = "damage_dice";
        public const string Column_damage_dice_count = "damage_dice_count";
        public const string Column_probability_value = "probability_value";
        public const string Column_probability_dice = "probability_dice";
        public const string Column_attr = "attr";
        public const string Column_type = "type";
        public const string Column_lawful = "lawful";
        public const string Column_ranged = "ranged";
        public const string Column_area = "area";
        public const string Column_through = "through";
        public const string Column_id = "id";
        public const string Column_action_id = "action_id";
        public const string Column_castgfx = "castgfx";
        public const string Column_castgfx2 = "castgfx2";
        public const string Column_sysmsgID_happen = "sysmsgID_happen";
        public const string Column_sysmsgID_stop = "sysmsgID_stop";
        public const string Column_sysmsgID_fail = "sysmsgID_fail";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_target, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_nameid, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_skill_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_skill_level, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_skill_number, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mpConsume, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hpConsume, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_itemConsumeId, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_itemConsumeCount, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_reuseDelay, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_buffDuration, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_target_to, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_damage_value, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_damage_dice, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_damage_dice_count, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_probability_value, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_probability_dice, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_attr, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_type, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_lawful, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ranged, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_area, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_through, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_action_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_castgfx, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_castgfx2, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_sysmsgID_happen, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_sysmsgID_stop, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_sysmsgID_fail, DbType = DbType.Int32, IsPKey = false},
        };
        public Skills() : base(TableName)
        {
            
        }
    }
}
