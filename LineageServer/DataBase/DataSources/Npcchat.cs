using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class Npcchat : DataSourceTable
    {
        public const string TableName = "npcchat";
        public const string Column_note = "note";
        public const string Column_chat_id1 = "chat_id1";
        public const string Column_chat_id2 = "chat_id2";
        public const string Column_chat_id3 = "chat_id3";
        public const string Column_chat_id4 = "chat_id4";
        public const string Column_chat_id5 = "chat_id5";
        public const string Column_npc_id = "npc_id";
        public const string Column_start_delay_time = "start_delay_time";
        public const string Column_chat_interval = "chat_interval";
        public const string Column_repeat_interval = "repeat_interval";
        public const string Column_game_time = "game_time";
        public const string Column_chat_timing = "chat_timing";
        public const string Column_is_shout = "is_shout";
        public const string Column_is_world_chat = "is_world_chat";
        public const string Column_is_repeat = "is_repeat";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_chat_id1, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_chat_id2, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_chat_id3, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_chat_id4, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_chat_id5, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_npc_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_start_delay_time, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_chat_interval, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_repeat_interval, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_game_time, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_chat_timing, DbType = DbType.Boolean, IsPKey = true},
            new ColumnInfo() { Column = Column_is_shout, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_is_world_chat, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_is_repeat, DbType = DbType.Boolean, IsPKey = false},
        };
        public Npcchat() : base(TableName)
        {
            
        }
    }
}
