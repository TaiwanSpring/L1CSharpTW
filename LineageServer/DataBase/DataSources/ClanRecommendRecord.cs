using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class ClanRecommendRecord : DataSourceTable
    {
        public const string TableName = "clan_recommend_record";
        public const string Column_clan_name = "clan_name";
        public const string Column_crown_name = "crown_name";
        public const string Column_type_message = "type_message";
        public const string Column_clan_id = "clan_id";
        public const string Column_clan_type = "clan_type";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_clan_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_crown_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_type_message, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_clan_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_clan_type, DbType = DbType.Boolean, IsPKey = false},
        };
        public ClanRecommendRecord() : base(TableName)
        {
            
        }
    }
}
