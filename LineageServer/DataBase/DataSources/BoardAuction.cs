using System.Data;
using LineageServer.Enum;
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
            new ColumnInfo() { Column = Column_house_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_location, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_old_owner, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_bidder, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_deadline, DbType = DbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_house_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_house_area, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_price, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_old_owner_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_bidder_id, DbType = DbType.Int32, IsPKey = false},
        };
        public BoardAuction() : base(TableName)
        {
            
        }
    }
}
