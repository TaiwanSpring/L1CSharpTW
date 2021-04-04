using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class CharacterBuddys : DataSource
    {
        public const string TableName = "character_buddys";
        public const string Column_buddy_name = "buddy_name";
        public const string Column_id = "id";
        public const string Column_char_id = "char_id";
        public const string Column_buddy_id = "buddy_id";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.CharacterBuddys; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_buddy_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_char_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_buddy_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
        };
        public CharacterBuddys() : base(TableName)
        {
            
        }
    }
}
