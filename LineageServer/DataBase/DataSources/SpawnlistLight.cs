using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class SpawnlistLight : DataSource
    {
        public const string TableName = "spawnlist_light";
        public const string Column_id = "id";
        public const string Column_npcid = "npcid";
        public const string Column_locx = "locx";
        public const string Column_locy = "locy";
        public const string Column_mapid = "mapid";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.SpawnlistLight; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_npcid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locx, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locy, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mapid, DbType = DbType.Int32, IsPKey = false},
        };
        public SpawnlistLight() : base(TableName)
        {
            
        }
    }
}
