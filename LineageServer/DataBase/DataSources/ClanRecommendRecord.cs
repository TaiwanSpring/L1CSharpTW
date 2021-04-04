using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class ClanRecommendRecord : DataSource
    {
        public const string TableName = "clan_recommend_record";
        public const string Column_clan_name = "clan_name";
        public const string Column_crown_name = "crown_name";
        public const string Column_type_message = "type_message";
        public const string Column_clan_id = "clan_id";
        public const string Column_clan_type = "clan_type";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.ClanRecommendRecord; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_clan_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_crown_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_type_message, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_clan_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_clan_type, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
        };
        public ClanRecommendRecord() : base(TableName)
        {
            
        }
    }
}
