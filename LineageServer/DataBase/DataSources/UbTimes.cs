using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class UbTimes : DataSource
    {
        public const string TableName = "ub_times";
        public const string Column_ub_id = "ub_id";
        public const string Column_ub_time = "ub_time";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.UbTimes; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_ub_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ub_time, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public UbTimes() : base(TableName)
        {
            
        }
    }
}
