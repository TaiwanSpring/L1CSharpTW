using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Mail : DataSource
    {
        public const string TableName = "mail";
        public const string Column_sender = "sender";
        public const string Column_receiver = "receiver";
        public const string Column_date = "date";
        public const string Column_id = "id";
        public const string Column_type = "type";
        public const string Column_inbox_id = "inbox_id";
        public const string Column_read_status = "read_status";
        public const string Column_subject = "subject";
        public const string Column_content = "content";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Mail; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_sender, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_receiver, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_date, MySqlDbType = MySqlDbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_type, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_inbox_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_read_status, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_subject, MySqlDbType = MySqlDbType.Binary, IsPKey = false},
            new ColumnInfo() { Column = Column_content, MySqlDbType = MySqlDbType.Binary, IsPKey = false},
        };
        public Mail() : base(TableName)
        {
            
        }
    }
}
