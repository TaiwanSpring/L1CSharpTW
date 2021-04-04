using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Mobskill : DataSource
    {
        public const string TableName = "mobskill";
        public const string Column_mobname = "mobname";
        public const string Column_mobid = "mobid";
        public const string Column_actNo = "actNo";
        public const string Column_Type = "Type";
        public const string Column_mpConsume = "mpConsume";
        public const string Column_TriRnd = "TriRnd";
        public const string Column_TriHp = "TriHp";
        public const string Column_TriCompanionHp = "TriCompanionHp";
        public const string Column_TriRange = "TriRange";
        public const string Column_TriCount = "TriCount";
        public const string Column_ChangeTarget = "ChangeTarget";
        public const string Column_Range = "Range";
        public const string Column_AreaWidth = "AreaWidth";
        public const string Column_AreaHeight = "AreaHeight";
        public const string Column_Leverage = "Leverage";
        public const string Column_SkillId = "SkillId";
        public const string Column_Gfxid = "Gfxid";
        public const string Column_ActId = "ActId";
        public const string Column_SummonId = "SummonId";
        public const string Column_SummonMin = "SummonMin";
        public const string Column_SummonMax = "SummonMax";
        public const string Column_PolyId = "PolyId";
        public const string Column_SkillArea = "SkillArea";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Mobskill; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_mobname, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_mobid, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_actNo, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_Type, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mpConsume, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_TriRnd, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_TriHp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_TriCompanionHp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_TriRange, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_TriCount, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ChangeTarget, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Range, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_AreaWidth, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_AreaHeight, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Leverage, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_SkillId, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Gfxid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ActId, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_SummonId, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_SummonMin, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_SummonMax, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_PolyId, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_SkillArea, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
        };
        public Mobskill() : base(TableName)
        {
            
        }
    }
}
