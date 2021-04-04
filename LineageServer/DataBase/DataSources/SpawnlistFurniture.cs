using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class SpawnlistFurniture : DataSource
    {
        public const string TableName = "spawnlist_furniture";
        public const string Column_item_obj_id = "item_obj_id";
        public const string Column_npcid = "npcid";
        public const string Column_locx = "locx";
        public const string Column_locy = "locy";
        public const string Column_mapid = "mapid";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.SpawnlistFurniture; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_item_obj_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_npcid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locx, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locy, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mapid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public SpawnlistFurniture() : base(TableName)
        {
            
        }
    }
}
