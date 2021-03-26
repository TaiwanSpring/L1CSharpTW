using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class Dungeon : DataSource
    {
        public const string TableName = "dungeon";
        public const string Column_note = "note";
        public const string Column_src_x = "src_x";
        public const string Column_src_y = "src_y";
        public const string Column_src_mapid = "src_mapid";
        public const string Column_new_x = "new_x";
        public const string Column_new_y = "new_y";
        public const string Column_new_mapid = "new_mapid";
        public const string Column_new_heading = "new_heading";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Dungeon; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_src_x, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_src_y, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_src_mapid, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_new_x, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_y, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_mapid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_heading, DbType = DbType.Int32, IsPKey = false},
        };
        public Dungeon() : base(TableName)
        {
            
        }
    }
}
