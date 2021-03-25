using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class Pettypes : DataSourceTable
    {
        public const string TableName = "pettypes";
        public const string Column_Name = "Name";
        public const string Column_BaseNpcId = "BaseNpcId";
        public const string Column_ItemIdForTaming = "ItemIdForTaming";
        public const string Column_HpUpMin = "HpUpMin";
        public const string Column_HpUpMax = "HpUpMax";
        public const string Column_MpUpMin = "MpUpMin";
        public const string Column_MpUpMax = "MpUpMax";
        public const string Column_EvolvItemId = "EvolvItemId";
        public const string Column_NpcIdForEvolving = "NpcIdForEvolving";
        public const string Column_MessageId1 = "MessageId1";
        public const string Column_MessageId2 = "MessageId2";
        public const string Column_MessageId3 = "MessageId3";
        public const string Column_MessageId4 = "MessageId4";
        public const string Column_MessageId5 = "MessageId5";
        public const string Column_DefyMessageId = "DefyMessageId";
        public const string Column_canUseEquipment = "canUseEquipment";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_Name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_BaseNpcId, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_ItemIdForTaming, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_HpUpMin, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_HpUpMax, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MpUpMin, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MpUpMax, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_EvolvItemId, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_NpcIdForEvolving, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MessageId1, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MessageId2, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MessageId3, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MessageId4, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MessageId5, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_DefyMessageId, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_canUseEquipment, DbType = DbType.Boolean, IsPKey = false},
        };
        public Pettypes() : base(TableName)
        {
            
        }
    }
}
