using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class Mail : DataSourceTable
    {
        public const string TableName = "mail";
        public const string Column_sender = "sender";
        public const string Column_receiver = "receiver";
        public const string Column_date = "date";
        public const string Column_id = "id";
        public const string Column_type = "type";
        public const string Column_inbox_id = "inbox_id";
        public const string Column_read_status = "read_status";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_sender, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_receiver, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_date, DbType = DbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_type, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_inbox_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_read_status, DbType = DbType.Boolean, IsPKey = false},
        };
        public Mail() : base(TableName)
        {
            
        }
    }
}
