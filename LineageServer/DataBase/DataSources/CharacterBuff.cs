using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class CharacterBuff : DataSourceTable
    {
        public const string TableName = "character_buff";
        public const string Column_char_obj_id = "char_obj_id";
        public const string Column_skill_id = "skill_id";
        public const string Column_remaining_time = "remaining_time";
        public const string Column_poly_id = "poly_id";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_char_obj_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_skill_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_remaining_time, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_poly_id, DbType = DbType.Int32, IsPKey = false},
        };
        public CharacterBuff() : base(TableName)
        {
            
        }
    }
}
