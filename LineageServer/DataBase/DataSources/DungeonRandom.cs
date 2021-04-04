using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class DungeonRandom : DataSource
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
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.DungeonRandom; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_src_x, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_src_y, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_src_mapid, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_new_x1, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_y1, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_mapid1, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_x2, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_y2, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_mapid2, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_x3, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_y3, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_mapid3, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_x4, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_y4, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_mapid4, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_x5, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_y5, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_mapid5, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_heading, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public DungeonRandom() : base(TableName)
        {
            
        }
    }
}
