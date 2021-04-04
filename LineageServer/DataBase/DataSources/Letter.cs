using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Letter : DataSource
    {
        public const string TableName = "letter";
        public const string Column_sender = "sender";
        public const string Column_receiver = "receiver";
        public const string Column_date = "date";
        public const string Column_item_object_id = "item_object_id";
        public const string Column_code = "code";
        public const string Column_template_id = "template_id";
        public const string Column_subject = "subject";
        public const string Column_content = "content";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Letter; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_sender, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_receiver, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_date, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_item_object_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_code, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_template_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_subject, MySqlDbType = MySqlDbType.Binary, IsPKey = false},
            new ColumnInfo() { Column = Column_content, MySqlDbType = MySqlDbType.Binary, IsPKey = false},
        };
        public Letter() : base(TableName)
        {

        }
    }
}
