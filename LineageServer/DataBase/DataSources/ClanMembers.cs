using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class ClanMembers : DataSourceTable
    {
        public const string TableName = "clan_members";
        public const string Column_char_name = "char_name";
        public const string Column_notes = "notes";
        public const string Column_clan_id = "clan_id";
        public const string Column_index_id = "index_id";
        public const string Column_char_id = "char_id";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_char_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_notes, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_clan_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_index_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_char_id, DbType = DbType.Int32, IsPKey = false},
        };
        public ClanMembers() : base(TableName)
        {
            
        }
    }
}
