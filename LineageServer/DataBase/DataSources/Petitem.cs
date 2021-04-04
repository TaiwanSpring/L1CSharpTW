using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Petitem : DataSource
    {
        public const string TableName = "petitem";
        public const string Column_note = "note";
        public const string Column_use_type = "use_type";
        public const string Column_item_id = "item_id";
        public const string Column_hitmodifier = "hitmodifier";
        public const string Column_dmgmodifier = "dmgmodifier";
        public const string Column_ac = "ac";
        public const string Column_add_str = "add_str";
        public const string Column_add_con = "add_con";
        public const string Column_add_dex = "add_dex";
        public const string Column_add_int = "add_int";
        public const string Column_add_wis = "add_wis";
        public const string Column_add_hp = "add_hp";
        public const string Column_add_mp = "add_mp";
        public const string Column_add_sp = "add_sp";
        public const string Column_m_def = "m_def";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Petitem; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_use_type, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_item_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_hitmodifier, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_dmgmodifier, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ac, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_str, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_con, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_dex, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_int, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_wis, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_hp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_mp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_sp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_m_def, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public Petitem() : base(TableName)
        {
            
        }
    }
}
