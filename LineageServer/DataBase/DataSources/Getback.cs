using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Getback : DataSource
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
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Getback; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_area_x1, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_area_y1, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_area_x2, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_area_y2, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_area_mapid, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_getback_x1, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_getback_y1, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_getback_x2, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_getback_y2, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_getback_x3, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_getback_y3, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_getback_mapid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_getback_townid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_getback_townid_elf, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_getback_townid_darkelf, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_scrollescape, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public Getback() : base(TableName)
        {
            
        }
    }
}
