using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class ClanMembers : DataSource
    {
        public const string TableName = "clan_members";
        public const string Column_char_name = "char_name";
        public const string Column_notes = "notes";
        public const string Column_clan_id = "clan_id";
        public const string Column_index_id = "index_id";
        public const string Column_char_id = "char_id";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.ClanMembers; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_char_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_notes, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_clan_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_index_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_char_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public ClanMembers() : base(TableName)
        {
            
        }
    }
}
