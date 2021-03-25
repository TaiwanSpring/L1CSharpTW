using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class SpawnlistTime : DataSourceTable
    {
        public const string TableName = "spawnlist_time";
        public const string Column_spawn_id = "spawn_id";
        public const string Column_delete_at_endtime = "delete_at_endtime";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_spawn_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_delete_at_endtime, DbType = DbType.Boolean, IsPKey = false},
        };
        public SpawnlistTime() : base(TableName)
        {
            
        }
    }
}
