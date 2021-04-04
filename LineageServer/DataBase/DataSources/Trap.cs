using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Trap : DataSource
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
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Trap; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_poisonType, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_note, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_type, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_gfxId, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_base, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_dice, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_diceCount, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_poisonDelay, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_poisonTime, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_poisonDamage, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_monsterNpcId, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_monsterCount, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_teleportX, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_teleportY, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_teleportMapId, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_skillId, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_skillTimeSeconds, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_isDetectionable, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
        };
        public Trap() : base(TableName)
        {
            
        }
    }
}
