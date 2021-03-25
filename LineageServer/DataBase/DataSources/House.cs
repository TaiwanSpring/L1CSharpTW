using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class House : DataSourceTable
    {
        public const string TableName = "house";
        public const string Column_house_name = "house_name";
        public const string Column_location = "location";
        public const string Column_tax_deadline = "tax_deadline";
        public const string Column_house_id = "house_id";
        public const string Column_house_area = "house_area";
        public const string Column_keeper_id = "keeper_id";
        public const string Column_is_on_sale = "is_on_sale";
        public const string Column_is_purchase_basement = "is_purchase_basement";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_house_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_location, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_tax_deadline, DbType = DbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_house_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_house_area, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_keeper_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_is_on_sale, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_is_purchase_basement, DbType = DbType.Int32, IsPKey = false},
        };
        public House() : base(TableName)
        {
            
        }
    }
}
