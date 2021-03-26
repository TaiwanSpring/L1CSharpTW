using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class CharacterTeleport : DataSource
    {
        public const string TableName = "character_teleport";
        public const string Column_name = "name";
        public const string Column_id = "id";
        public const string Column_char_id = "char_id";
        public const string Column_locx = "locx";
        public const string Column_locy = "locy";
        public const string Column_mapid = "mapid";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.CharacterTeleport; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_char_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locx, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locy, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mapid, DbType = DbType.Int32, IsPKey = false},
        };
        public CharacterTeleport() : base(TableName)
        {
            
        }
    }
}
