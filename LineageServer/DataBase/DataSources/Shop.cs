using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Shop : DataSource
    {
        public const string TableName = "shop";
        public const string Column_npc_id = "npc_id";
        public const string Column_item_id = "item_id";
        public const string Column_order_id = "order_id";
        public const string Column_selling_price = "selling_price";
        public const string Column_pack_count = "pack_count";
        public const string Column_purchasing_price = "purchasing_price";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Shop; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_npc_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_item_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_order_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_selling_price, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_pack_count, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_purchasing_price, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public Shop() : base(TableName)
        {
            
        }
    }
}
