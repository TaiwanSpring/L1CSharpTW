using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class CharacterQuests : DataSourceTable
    {
        public const string TableName = "character_quests";
        public const string Column_char_id = "char_id";
        public const string Column_quest_id = "quest_id";
        public const string Column_quest_step = "quest_step";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_char_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_quest_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_quest_step, DbType = DbType.Int32, IsPKey = false},
        };
        public CharacterQuests() : base(TableName)
        {
            
        }
    }
}
