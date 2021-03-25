using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class Mobskill : DataSourceTable
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
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_mobname, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_mobid, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_actNo, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_Type, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mpConsume, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_TriRnd, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_TriHp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_TriCompanionHp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_TriRange, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_TriCount, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ChangeTarget, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Range, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_AreaWidth, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_AreaHeight, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Leverage, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_SkillId, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Gfxid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ActId, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_SummonId, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_SummonMin, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_SummonMax, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_PolyId, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_SkillArea, DbType = DbType.Boolean, IsPKey = false},
        };
        public Mobskill() : base(TableName)
        {
            
        }
    }
}
