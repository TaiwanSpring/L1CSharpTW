private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Npcchat);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Npcchat.Column_npc_id, obj.NpcId)
.Where(Npcchat.Column_chat_timing, obj.ChatTiming)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Npcchat.Column_npc_id, obj.NpcId)
.Set(Npcchat.Column_chat_timing, obj.ChatTiming)
.Set(Npcchat.Column_note, obj.Note)
.Set(Npcchat.Column_start_delay_time, obj.StartDelayTime)
.Set(Npcchat.Column_chat_id1, obj.ChatId1)
.Set(Npcchat.Column_chat_id2, obj.ChatId2)
.Set(Npcchat.Column_chat_id3, obj.ChatId3)
.Set(Npcchat.Column_chat_id4, obj.ChatId4)
.Set(Npcchat.Column_chat_id5, obj.ChatId5)
.Set(Npcchat.Column_chat_interval, obj.ChatInterval)
.Set(Npcchat.Column_is_shout, obj.IsShout)
.Set(Npcchat.Column_is_world_chat, obj.IsWorldChat)
.Set(Npcchat.Column_is_repeat, obj.IsRepeat)
.Set(Npcchat.Column_repeat_interval, obj.RepeatInterval)
.Set(Npcchat.Column_game_time, obj.GameTime)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Npcchat.Column_npc_id, obj.NpcId)
.Where(Npcchat.Column_chat_timing, obj.ChatTiming)
.Set(Npcchat.Column_note, obj.Note)
.Set(Npcchat.Column_start_delay_time, obj.StartDelayTime)
.Set(Npcchat.Column_chat_id1, obj.ChatId1)
.Set(Npcchat.Column_chat_id2, obj.ChatId2)
.Set(Npcchat.Column_chat_id3, obj.ChatId3)
.Set(Npcchat.Column_chat_id4, obj.ChatId4)
.Set(Npcchat.Column_chat_id5, obj.ChatId5)
.Set(Npcchat.Column_chat_interval, obj.ChatInterval)
.Set(Npcchat.Column_is_shout, obj.IsShout)
.Set(Npcchat.Column_is_world_chat, obj.IsWorldChat)
.Set(Npcchat.Column_is_repeat, obj.IsRepeat)
.Set(Npcchat.Column_repeat_interval, obj.RepeatInterval)
.Set(Npcchat.Column_game_time, obj.GameTime)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Npcchat.Column_npc_id, obj.NpcId)
.Where(Npcchat.Column_chat_timing, obj.ChatTiming)
.Execute();


obj.NpcId = dataSourceRow.getString(Npcchat.Column_npc_id);
obj.ChatTiming = dataSourceRow.getString(Npcchat.Column_chat_timing);
obj.Note = dataSourceRow.getString(Npcchat.Column_note);
obj.StartDelayTime = dataSourceRow.getString(Npcchat.Column_start_delay_time);
obj.ChatId1 = dataSourceRow.getString(Npcchat.Column_chat_id1);
obj.ChatId2 = dataSourceRow.getString(Npcchat.Column_chat_id2);
obj.ChatId3 = dataSourceRow.getString(Npcchat.Column_chat_id3);
obj.ChatId4 = dataSourceRow.getString(Npcchat.Column_chat_id4);
obj.ChatId5 = dataSourceRow.getString(Npcchat.Column_chat_id5);
obj.ChatInterval = dataSourceRow.getString(Npcchat.Column_chat_interval);
obj.IsShout = dataSourceRow.getString(Npcchat.Column_is_shout);
obj.IsWorldChat = dataSourceRow.getString(Npcchat.Column_is_world_chat);
obj.IsRepeat = dataSourceRow.getString(Npcchat.Column_is_repeat);
obj.RepeatInterval = dataSourceRow.getString(Npcchat.Column_repeat_interval);
obj.GameTime = dataSourceRow.getString(Npcchat.Column_game_time);

