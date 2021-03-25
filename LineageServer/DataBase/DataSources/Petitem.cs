using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class Petitem : DataSourceTable
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
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_use_type, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_item_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_hitmodifier, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_dmgmodifier, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ac, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_str, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_con, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_dex, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_int, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_wis, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_hp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_mp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_add_sp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_m_def, DbType = DbType.Int32, IsPKey = false},
        };
        public Petitem() : base(TableName)
        {
            
        }
    }
}
