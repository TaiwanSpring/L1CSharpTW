using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class CharacterBuff : DataSource
    {
        public const string TableName = "character_buff";
        public const string Column_char_obj_id = "char_obj_id";
        public const string Column_skill_id = "skill_id";
        public const string Column_remaining_time = "remaining_time";
        public const string Column_poly_id = "poly_id";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.CharacterBuff; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_char_obj_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_skill_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_remaining_time, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_poly_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public CharacterBuff() : base(TableName)
        {
            
        }
    }
}
