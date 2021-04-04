using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Npcaction : DataSource
    {
        public const string TableName = "npcaction";
        public const string Column_normal_action = "normal_action";
        public const string Column_caotic_action = "caotic_action";
        public const string Column_teleport_url = "teleport_url";
        public const string Column_teleport_urla = "teleport_urla";
        public const string Column_npcid = "npcid";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Npcaction; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_normal_action, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_caotic_action, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_teleport_url, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_teleport_urla, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_npcid, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
        };
        public Npcaction() : base(TableName)
        {
            
        }
    }
}
