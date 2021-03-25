using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class Getback : DataSourceTable
    {
        public const string TableName = "getback";
        public const string Column_note = "note";
        public const string Column_area_x1 = "area_x1";
        public const string Column_area_y1 = "area_y1";
        public const string Column_area_x2 = "area_x2";
        public const string Column_area_y2 = "area_y2";
        public const string Column_area_mapid = "area_mapid";
        public const string Column_getback_x1 = "getback_x1";
        public const string Column_getback_y1 = "getback_y1";
        public const string Column_getback_x2 = "getback_x2";
        public const string Column_getback_y2 = "getback_y2";
        public const string Column_getback_x3 = "getback_x3";
        public const string Column_getback_y3 = "getback_y3";
        public const string Column_getback_mapid = "getback_mapid";
        public const string Column_getback_townid = "getback_townid";
        public const string Column_getback_townid_elf = "getback_townid_elf";
        public const string Column_getback_townid_darkelf = "getback_townid_darkelf";
        public const string Column_scrollescape = "scrollescape";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_area_x1, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_area_y1, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_area_x2, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_area_y2, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_area_mapid, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_getback_x1, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_getback_y1, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_getback_x2, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_getback_y2, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_getback_x3, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_getback_y3, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_getback_mapid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_getback_townid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_getback_townid_elf, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_getback_townid_darkelf, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_scrollescape, DbType = DbType.Int32, IsPKey = false},
        };
        public Getback() : base(TableName)
        {
            
        }
    }
}
