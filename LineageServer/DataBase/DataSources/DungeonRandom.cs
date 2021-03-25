using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class DungeonRandom : DataSourceTable
    {
        public const string TableName = "dungeon_random";
        public const string Column_note = "note";
        public const string Column_src_x = "src_x";
        public const string Column_src_y = "src_y";
        public const string Column_src_mapid = "src_mapid";
        public const string Column_new_x1 = "new_x1";
        public const string Column_new_y1 = "new_y1";
        public const string Column_new_mapid1 = "new_mapid1";
        public const string Column_new_x2 = "new_x2";
        public const string Column_new_y2 = "new_y2";
        public const string Column_new_mapid2 = "new_mapid2";
        public const string Column_new_x3 = "new_x3";
        public const string Column_new_y3 = "new_y3";
        public const string Column_new_mapid3 = "new_mapid3";
        public const string Column_new_x4 = "new_x4";
        public const string Column_new_y4 = "new_y4";
        public const string Column_new_mapid4 = "new_mapid4";
        public const string Column_new_x5 = "new_x5";
        public const string Column_new_y5 = "new_y5";
        public const string Column_new_mapid5 = "new_mapid5";
        public const string Column_new_heading = "new_heading";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_src_x, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_src_y, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_src_mapid, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_new_x1, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_y1, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_mapid1, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_x2, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_y2, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_mapid2, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_x3, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_y3, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_mapid3, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_x4, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_y4, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_mapid4, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_x5, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_y5, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_mapid5, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_heading, DbType = DbType.Int32, IsPKey = false},
        };
        public DungeonRandom() : base(TableName)
        {
            
        }
    }
}
