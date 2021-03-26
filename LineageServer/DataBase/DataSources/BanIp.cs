using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class BanIp : DataSource
    {
        public const string TableName = "ban_ip";
        public const string Column_ip = "ip";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.BanIp; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_ip, DbType = DbType.String, IsPKey = true},
        };
        public BanIp() : base(TableName)
        {
            
        }
    }
}
