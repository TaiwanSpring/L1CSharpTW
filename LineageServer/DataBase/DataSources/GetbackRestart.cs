using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class GetbackRestart : DataSource
    {
        public const string TableName = "getback_restart";
        public const string Column_note = "note";
        public const string Column_area = "area";
        public const string Column_locx = "locx";
        public const string Column_locy = "locy";
        public const string Column_mapid = "mapid";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.GetbackRestart; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_area, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_locx, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locy, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mapid, DbType = DbType.Int32, IsPKey = false},
        };
        public GetbackRestart() : base(TableName)
        {
            
        }
    }
}
