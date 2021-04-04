using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class CharacterQuests : DataSource
    {
        public const string TableName = "character_quests";
        public const string Column_char_id = "char_id";
        public const string Column_quest_id = "quest_id";
        public const string Column_quest_step = "quest_step";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.CharacterQuests; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_char_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_quest_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_quest_step, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public CharacterQuests() : base(TableName)
        {
            
        }
    }
}
