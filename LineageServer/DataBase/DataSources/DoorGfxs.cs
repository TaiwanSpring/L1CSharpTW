using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class DoorGfxs : DataSource
    {
        public const string TableName = "door_gfxs";
        public const string Column_note = "note";
        public const string Column_gfxid = "gfxid";
        public const string Column_direction = "direction";
        public const string Column_left_edge_offset = "left_edge_offset";
        public const string Column_right_edge_offset = "right_edge_offset";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.DoorGfxs; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_gfxid, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_direction, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_left_edge_offset, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_right_edge_offset, DbType = DbType.Int32, IsPKey = false},
        };
        public DoorGfxs() : base(TableName)
        {
            
        }
    }
}
