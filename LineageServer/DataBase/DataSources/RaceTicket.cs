using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class RaceTicket : DataSource
    {
        public const string TableName = "race_ticket";
        public const string Column_item_obj_id = "item_obj_id";
        public const string Column_round = "round";
        public const string Column_victory = "victory";
        public const string Column_runner_num = "runner_num";
        public const string Column_allotment_percentage = "allotment_percentage";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.RaceTicket; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_item_obj_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_round, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_victory, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_runner_num, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_allotment_percentage, MySqlDbType = MySqlDbType.Float, IsPKey = false},
        };
        public RaceTicket() : base(TableName)
        {
            
        }
    }
}
