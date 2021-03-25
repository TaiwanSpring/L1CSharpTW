using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class Area : DataSourceTable
    {
        public const string TableName = "area";
        public const string Column_areaname = "areaname";
        public const string Column_areaid = "areaid";
        public const string Column_mapid = "mapid";
        public const string Column_x1 = "x1";
        public const string Column_y1 = "y1";
        public const string Column_x2 = "x2";
        public const string Column_y2 = "y2";
        public const string Column_flag = "flag";
        public const string Column_restart = "restart";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_areaname, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_areaid, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_mapid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_x1, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_y1, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_x2, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_y2, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_flag, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_restart, DbType = DbType.Int32, IsPKey = false},
        };
        public Area() : base(TableName)
        {
            
        }
    }
}
