using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class UbManagers : DataSourceTable
    {
        public const string TableName = "ub_managers";
        public const string Column_ub_id = "ub_id";
        public const string Column_ub_manager_npc_id = "ub_manager_npc_id";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_ub_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ub_manager_npc_id, DbType = DbType.Int32, IsPKey = false},
        };
        public UbManagers() : base(TableName)
        {
            
        }
    }
}
