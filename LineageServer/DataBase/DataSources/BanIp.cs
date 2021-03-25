using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class BanIp : DataSourceTable
    {
        public const string TableName = "ban_ip";
        public const string Column_ip = "ip";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_ip, DbType = DbType.String, IsPKey = true},
        };
        public BanIp() : base(TableName)
        {
            
        }
    }
}
