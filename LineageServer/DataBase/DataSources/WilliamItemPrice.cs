using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class WilliamItemPrice : DataSource
    {
        public const string TableName = "william_item_price";
        public const string Column_name = "name";
        public const string Column_item_id = "item_id";
        public const string Column_price = "price";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.WilliamItemPrice; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_item_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_price, DbType = DbType.Int32, IsPKey = false},
        };
        public WilliamItemPrice() : base(TableName)
        {
            
        }
    }
}
