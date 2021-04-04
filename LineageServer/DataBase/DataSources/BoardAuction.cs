using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class BoardAuction : DataSource
    {
        public const string TableName = "board_auction";
        public const string Column_house_name = "house_name";
        public const string Column_location = "location";
        public const string Column_old_owner = "old_owner";
        public const string Column_bidder = "bidder";
        public const string Column_deadline = "deadline";
        public const string Column_house_id = "house_id";
        public const string Column_house_area = "house_area";
        public const string Column_price = "price";
        public const string Column_old_owner_id = "old_owner_id";
        public const string Column_bidder_id = "bidder_id";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.BoardAuction; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_house_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_location, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_old_owner, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_bidder, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_deadline, MySqlDbType = MySqlDbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_house_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_house_area, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_price, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_old_owner_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_bidder_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public BoardAuction() : base(TableName)
        {
            
        }
    }
}
