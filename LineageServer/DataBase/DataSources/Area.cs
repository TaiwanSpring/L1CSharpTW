using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Area : DataSource
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
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Area; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_areaname, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_areaid, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_mapid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_x1, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_y1, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_x2, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_y2, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_flag, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_restart, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public Area() : base(TableName)
        {
            
        }
    }
}
