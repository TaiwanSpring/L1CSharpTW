using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class ClanRecommendApply : DataSource
    {
        public const string TableName = "clan_recommend_apply";
        public const string Column_clan_name = "clan_name";
        public const string Column_char_name = "char_name";
        public const string Column_id = "id";
        public const string Column_clan_id = "clan_id";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.ClanRecommendApply; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_clan_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_char_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_clan_id, DbType = DbType.Int32, IsPKey = false},
        };
        public ClanRecommendApply() : base(TableName)
        {
            
        }
    }
}
