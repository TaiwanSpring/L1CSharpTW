using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class UbManagers : DataSource
    {
        public const string TableName = "ub_managers";
        public const string Column_ub_id = "ub_id";
        public const string Column_ub_manager_npc_id = "ub_manager_npc_id";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.UbManagers; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
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
