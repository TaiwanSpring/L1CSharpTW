using System.Data;
using LineageServer.Enum;
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
            new ColumnInfo() { Column = Column_item_obj_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_round, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_victory, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_runner_num, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_allotment_percentage, DbType = DbType.Double, IsPKey = false},
        };
        public RaceTicket() : base(TableName)
        {
            
        }
    }
}
