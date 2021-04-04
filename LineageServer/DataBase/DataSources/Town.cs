using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Town : DataSource
    {
        public const string TableName = "town";
        public const string Column_name = "name";
        public const string Column_leader_name = "leader_name";
        public const string Column_town_id = "town_id";
        public const string Column_leader_id = "leader_id";
        public const string Column_tax_rate = "tax_rate";
        public const string Column_tax_rate_reserved = "tax_rate_reserved";
        public const string Column_sales_money = "sales_money";
        public const string Column_sales_money_yesterday = "sales_money_yesterday";
        public const string Column_town_tax = "town_tax";
        public const string Column_town_fix_tax = "town_fix_tax";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Town; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_leader_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_town_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_leader_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_tax_rate, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_tax_rate_reserved, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_sales_money, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_sales_money_yesterday, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_town_tax, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_town_fix_tax, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public Town() : base(TableName)
        {
            
        }
    }
}
