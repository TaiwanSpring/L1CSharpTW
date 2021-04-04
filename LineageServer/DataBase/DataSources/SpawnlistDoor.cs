using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class SpawnlistDoor : DataSource
    {
        public const string TableName = "spawnlist_door";
        public const string Column_location = "location";
        public const string Column_id = "id";
        public const string Column_gfxid = "gfxid";
        public const string Column_locx = "locx";
        public const string Column_locy = "locy";
        public const string Column_mapid = "mapid";
        public const string Column_hp = "hp";
        public const string Column_keeper = "keeper";
        public const string Column_isOpening = "isOpening";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.SpawnlistDoor; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_location, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_gfxid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locx, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locy, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mapid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_keeper, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_isOpening, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
        };
        public SpawnlistDoor() : base(TableName)
        {
            
        }
    }
}
