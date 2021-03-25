using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class Board : DataSourceTable
    {
        public const string TableName = "board";
        public const string Column_name = "name";
        public const string Column_date = "date";
        public const string Column_title = "title";
        public const string Column_content = "content";
        public const string Column_id = "id";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_date, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_title, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_content, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = true},
        };
        public Board() : base(TableName)
        {
            
        }
    }
}
