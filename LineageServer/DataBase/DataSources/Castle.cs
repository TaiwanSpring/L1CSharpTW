using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Castle : DataSource
    {
        public const string TableName = "castle";
        public const string Column_name = "name";
        public const string Column_war_time = "war_time";
        public const string Column_castle_id = "castle_id";
        public const string Column_tax_rate = "tax_rate";
        public const string Column_public_money = "public_money";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Castle; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_war_time, MySqlDbType = MySqlDbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_castle_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_tax_rate, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_public_money, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public Castle() : base(TableName)
        {
            
        }
    }
}
