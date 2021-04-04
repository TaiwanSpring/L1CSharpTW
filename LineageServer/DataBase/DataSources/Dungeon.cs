using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

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
            new ColumnInfo() { Column = Column_note, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_src_x, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_src_y, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_src_mapid, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_new_x, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_y, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_mapid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_new_heading, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public Dungeon() : base(TableName)
        {
            
        }
    }
}
