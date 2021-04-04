using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

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
            new ColumnInfo() { Column = Column_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_due_time, MySqlDbType = MySqlDbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_npcid, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_room_number, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_key_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_lodger_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hall, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
        };
        public Inn() : base(TableName)
        {
            
        }
    }
}
