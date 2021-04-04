using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Commands : DataSource
    {
        public const string TableName = "commands";
        public const string Column_name = "name";
        public const string Column_class_name = "class_name";
        public const string Column_access_level = "access_level";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Commands; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_name, MySqlDbType = MySqlDbType.Text, IsPKey = true},
            new ColumnInfo() { Column = Column_class_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_access_level, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public Commands() : base(TableName)
        {
            
        }
    }
}
