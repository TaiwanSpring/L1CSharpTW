using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class Npcaction : DataSourceTable
    {
        public const string TableName = "npcaction";
        public const string Column_normal_action = "normal_action";
        public const string Column_caotic_action = "caotic_action";
        public const string Column_teleport_url = "teleport_url";
        public const string Column_teleport_urla = "teleport_urla";
        public const string Column_npcid = "npcid";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_normal_action, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_caotic_action, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_teleport_url, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_teleport_urla, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_npcid, DbType = DbType.Int32, IsPKey = true},
        };
        public Npcaction() : base(TableName)
        {
            
        }
    }
}
