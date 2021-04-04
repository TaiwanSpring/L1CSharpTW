using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Pettypes : DataSource
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
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Pettypes; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_Name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_BaseNpcId, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_ItemIdForTaming, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_HpUpMin, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_HpUpMax, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MpUpMin, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MpUpMax, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_EvolvItemId, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_NpcIdForEvolving, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MessageId1, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MessageId2, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MessageId3, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MessageId4, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MessageId5, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_DefyMessageId, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_canUseEquipment, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
        };
        public Pettypes() : base(TableName)
        {
            
        }
    }
}
