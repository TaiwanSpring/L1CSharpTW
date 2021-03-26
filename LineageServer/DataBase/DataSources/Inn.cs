using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class Inn : DataSource
    {
        public const string TableName = "inn";
        public const string Column_name = "name";
        public const string Column_due_time = "due_time";
        public const string Column_npcid = "npcid";
        public const string Column_room_number = "room_number";
        public const string Column_key_id = "key_id";
        public const string Column_lodger_id = "lodger_id";
        public const string Column_hall = "hall";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Inn; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_due_time, DbType = DbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_npcid, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_room_number, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_key_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_lodger_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hall, DbType = DbType.Boolean, IsPKey = false},
        };
        public Inn() : base(TableName)
        {
            
        }
    }
}
