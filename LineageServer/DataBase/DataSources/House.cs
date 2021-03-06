using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class House : DataSource
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
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.House; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_house_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_location, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_tax_deadline, MySqlDbType = MySqlDbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_house_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_house_area, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_keeper_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_is_on_sale, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_is_purchase_basement, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public House() : base(TableName)
        {
            
        }
    }
}
