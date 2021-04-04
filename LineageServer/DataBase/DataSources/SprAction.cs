using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class SprAction : DataSource
    {
        public const string TableName = "spr_action";
        public const string Column_spr_id = "spr_id";
        public const string Column_act_id = "act_id";
        public const string Column_framecount = "framecount";
        public const string Column_framerate = "framerate";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.SprAction; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_spr_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_act_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_framecount, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_framerate, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public SprAction() : base(TableName)
        {
            
        }
    }
}
