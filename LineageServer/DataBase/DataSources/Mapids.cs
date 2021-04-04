using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Mapids : DataSource
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
        public const string Column_monster_amount = "monster_amount";
        public const string Column_drop_rate = "drop_rate";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Mapids; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_locationname, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_mapid, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_startX, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_endX, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_startY, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_endY, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_underwater, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_markable, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_teleportable, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_escapable, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_resurrection, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_painwand, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_penalty, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_take_pets, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_recall_pets, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_usable_item, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_usable_skill, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_monster_amount, MySqlDbType = MySqlDbType.Float, IsPKey = false},
            new ColumnInfo() { Column = Column_drop_rate, MySqlDbType = MySqlDbType.Float, IsPKey = false},
        };
        public Mapids() : base(TableName)
        {

        }
    }
}
