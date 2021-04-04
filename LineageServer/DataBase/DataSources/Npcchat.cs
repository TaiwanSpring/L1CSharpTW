using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Npcchat : DataSource
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
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Npcchat; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_chat_id1, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_chat_id2, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_chat_id3, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_chat_id4, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_chat_id5, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_npc_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_start_delay_time, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_chat_interval, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_repeat_interval, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_game_time, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_chat_timing, MySqlDbType = MySqlDbType.Bit, IsPKey = true},
            new ColumnInfo() { Column = Column_is_shout, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_is_world_chat, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_is_repeat, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
        };
        public Npcchat() : base(TableName)
        {
            
        }
    }
}
