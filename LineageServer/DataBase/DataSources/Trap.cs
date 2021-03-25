using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class Trap : DataSourceTable
    {
        public const string TableName = "trap";
        public const string Column_poisonType = "poisonType";
        public const string Column_note = "note";
        public const string Column_type = "type";
        public const string Column_id = "id";
        public const string Column_gfxId = "gfxId";
        public const string Column_base = "base";
        public const string Column_dice = "dice";
        public const string Column_diceCount = "diceCount";
        public const string Column_poisonDelay = "poisonDelay";
        public const string Column_poisonTime = "poisonTime";
        public const string Column_poisonDamage = "poisonDamage";
        public const string Column_monsterNpcId = "monsterNpcId";
        public const string Column_monsterCount = "monsterCount";
        public const string Column_teleportX = "teleportX";
        public const string Column_teleportY = "teleportY";
        public const string Column_teleportMapId = "teleportMapId";
        public const string Column_skillId = "skillId";
        public const string Column_skillTimeSeconds = "skillTimeSeconds";
        public const string Column_isDetectionable = "isDetectionable";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_poisonType, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_note, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_type, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_gfxId, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_base, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_dice, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_diceCount, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_poisonDelay, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_poisonTime, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_poisonDamage, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_monsterNpcId, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_monsterCount, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_teleportX, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_teleportY, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_teleportMapId, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_skillId, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_skillTimeSeconds, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_isDetectionable, DbType = DbType.Boolean, IsPKey = false},
        };
        public Trap() : base(TableName)
        {
            
        }
    }
}
