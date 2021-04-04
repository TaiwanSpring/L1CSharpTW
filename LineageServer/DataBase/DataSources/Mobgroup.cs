using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Mobgroup : DataSource
    {
        public const string TableName = "mobgroup";
        public const string Column_note = "note";
        public const string Column_id = "id";
        public const string Column_remove_group_if_leader_die = "remove_group_if_leader_die";
        public const string Column_leader_id = "leader_id";
        public const string Column_minion1_id = "minion1_id";
        public const string Column_minion1_count = "minion1_count";
        public const string Column_minion2_id = "minion2_id";
        public const string Column_minion2_count = "minion2_count";
        public const string Column_minion3_id = "minion3_id";
        public const string Column_minion3_count = "minion3_count";
        public const string Column_minion4_id = "minion4_id";
        public const string Column_minion4_count = "minion4_count";
        public const string Column_minion5_id = "minion5_id";
        public const string Column_minion5_count = "minion5_count";
        public const string Column_minion6_id = "minion6_id";
        public const string Column_minion6_count = "minion6_count";
        public const string Column_minion7_id = "minion7_id";
        public const string Column_minion7_count = "minion7_count";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Mobgroup; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_remove_group_if_leader_die, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_leader_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_minion1_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_minion1_count, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_minion2_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_minion2_count, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_minion3_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_minion3_count, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_minion4_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_minion4_count, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_minion5_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_minion5_count, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_minion6_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_minion6_count, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_minion7_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_minion7_count, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public Mobgroup() : base(TableName)
        {
            
        }
    }
}
