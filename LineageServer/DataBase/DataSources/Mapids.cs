using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class Mapids : DataSourceTable
    {
        public const string TableName = "mapids";
        public const string Column_locationname = "locationname";
        public const string Column_mapid = "mapid";
        public const string Column_startX = "startX";
        public const string Column_endX = "endX";
        public const string Column_startY = "startY";
        public const string Column_endY = "endY";
        public const string Column_underwater = "underwater";
        public const string Column_markable = "markable";
        public const string Column_teleportable = "teleportable";
        public const string Column_escapable = "escapable";
        public const string Column_resurrection = "resurrection";
        public const string Column_painwand = "painwand";
        public const string Column_penalty = "penalty";
        public const string Column_take_pets = "take_pets";
        public const string Column_recall_pets = "recall_pets";
        public const string Column_usable_item = "usable_item";
        public const string Column_usable_skill = "usable_skill";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_locationname, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_mapid, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_startX, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_endX, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_startY, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_endY, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_underwater, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_markable, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_teleportable, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_escapable, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_resurrection, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_painwand, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_penalty, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_take_pets, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_recall_pets, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_usable_item, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_usable_skill, DbType = DbType.Boolean, IsPKey = false},
        };
        public Mapids() : base(TableName)
        {
            
        }
    }
}
