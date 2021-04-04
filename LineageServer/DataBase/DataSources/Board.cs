using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Board : DataSource
    {
        public const string TableName = "board";
        public const string Column_name = "name";
        public const string Column_date = "date";
        public const string Column_title = "title";
        public const string Column_content = "content";
        public const string Column_id = "id";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Board; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_date, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_title, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_content, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
        };
        public Board() : base(TableName)
        {
            
        }
    }
}
