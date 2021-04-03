using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;
using System.Linq;
namespace LineageServer.Server.DataTables
{
    class NpcChatTable : IGameComponent
    {
        private readonly static IDataSource dataSource =
           Container.Instance.Resolve<IDataSourceFactory>()
           .Factory(Enum.DataSourceTypeEnum.Npcchat);

        private static NpcChatTable _instance;

        private IDictionary<int, L1NpcChat> _npcChatAppearance = MapFactory.NewMap<int, L1NpcChat>();

        private IDictionary<int, L1NpcChat> _npcChatDead = MapFactory.NewMap<int, L1NpcChat>();

        private IDictionary<int, L1NpcChat> _npcChatHide = MapFactory.NewMap<int, L1NpcChat>();

        private IDictionary<int, L1NpcChat> _npcChatGameTime = MapFactory.NewMap<int, L1NpcChat>();

        public static NpcChatTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NpcChatTable();
                }
                return _instance;
            }
        }

        private NpcChatTable()
        {
          
        }
        public void Initialize()
        {
            FillNpcChatTable();
        }
        private void FillNpcChatTable()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                L1NpcChat npcChat = new L1NpcChat();
                npcChat.NpcId = dataSourceRow.getInt(Npcchat.Column_npc_id);
                npcChat.ChatTiming = dataSourceRow.getInt(Npcchat.Column_chat_timing);
                npcChat.StartDelayTime = dataSourceRow.getInt(Npcchat.Column_start_delay_time);
                npcChat.ChatId1 = dataSourceRow.getString(Npcchat.Column_chat_id1);
                npcChat.ChatId2 = dataSourceRow.getString(Npcchat.Column_chat_id2);
                npcChat.ChatId3 = dataSourceRow.getString(Npcchat.Column_chat_id3);
                npcChat.ChatId4 = dataSourceRow.getString(Npcchat.Column_chat_id4);
                npcChat.ChatId5 = dataSourceRow.getString(Npcchat.Column_chat_id5);
                npcChat.ChatInterval = dataSourceRow.getInt(Npcchat.Column_chat_interval);
                npcChat.Shout = dataSourceRow.getBoolean(Npcchat.Column_is_shout);
                npcChat.WorldChat = dataSourceRow.getBoolean(Npcchat.Column_is_world_chat);
                npcChat.Repeat = dataSourceRow.getBoolean(Npcchat.Column_is_repeat);
                npcChat.RepeatInterval = dataSourceRow.getInt(Npcchat.Column_repeat_interval);
                npcChat.GameTime = dataSourceRow.getInt(Npcchat.Column_game_time);

                if (npcChat.ChatTiming == L1NpcInstance.CHAT_TIMING_APPEARANCE)
                {
                    _npcChatAppearance[npcChat.NpcId] = npcChat;
                }
                else if (npcChat.ChatTiming == L1NpcInstance.CHAT_TIMING_DEAD)
                {
                    _npcChatDead[npcChat.NpcId] = npcChat;
                }
                else if (npcChat.ChatTiming == L1NpcInstance.CHAT_TIMING_HIDE)
                {
                    _npcChatHide[npcChat.NpcId] = npcChat;
                }
                else if (npcChat.ChatTiming == L1NpcInstance.CHAT_TIMING_GAME_TIME)
                {
                    _npcChatGameTime[npcChat.NpcId] = npcChat;
                }
            }
        }

        public virtual L1NpcChat getTemplateAppearance(int i)
        {
            return _npcChatAppearance[i];
        }

        public virtual L1NpcChat getTemplateDead(int i)
        {
            return _npcChatDead[i];
        }

        public virtual L1NpcChat getTemplateHide(int i)
        {
            return _npcChatHide[i];
        }

        public virtual L1NpcChat getTemplateGameTime(int i)
        {
            return _npcChatGameTime[i];
        }

        public virtual L1NpcChat[] AllGameTime
        {
            get
            {
                return _npcChatGameTime.Values.ToArray();
            }
        }
    }

}