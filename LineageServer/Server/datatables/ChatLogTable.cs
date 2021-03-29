using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using System;

namespace LineageServer.Server.DataTables
{
    class ChatLogTable
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.LogChat);
        /*
		 * コード的にはHashMapを利用すべきだが、パフォーマンス上の問題があるかもしれない為、配列で妥協。
		 * HashMapへの変更を検討する場合は、パフォーマンス上問題が無いか十分注意すること。
		 */
        private readonly bool[] loggingConfig = new bool[15];

        private ChatLogTable()
        {
            loadConfig();
        }

        private void loadConfig()
        {
            loggingConfig[0] = Config.LOGGING_CHAT_NORMAL;
            loggingConfig[1] = Config.LOGGING_CHAT_WHISPER;
            loggingConfig[2] = Config.LOGGING_CHAT_SHOUT;
            loggingConfig[3] = Config.LOGGING_CHAT_WORLD;
            loggingConfig[4] = Config.LOGGING_CHAT_CLAN;
            loggingConfig[11] = Config.LOGGING_CHAT_PARTY;
            loggingConfig[13] = Config.LOGGING_CHAT_COMBINED;
            loggingConfig[14] = Config.LOGGING_CHAT_CHAT_PARTY;
        }

        private static ChatLogTable _instance;

        public static ChatLogTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ChatLogTable();
                }
                return _instance;
            }
        }

        private bool isLoggingTarget(int type)
        {
            return loggingConfig[type];
        }

        public virtual void storeChat(L1PcInstance pc, L1PcInstance target, string text, int type)
        {
            if (!isLoggingTarget(type))
            {
                return;
            }

            // type
            // 0:通常チャット
            // 1:Whisper
            // 2:叫び
            // 3:全体チャット
            // 4:血盟チャット
            // 11:パーティチャット
            // 13:連合チャット
            // 14:チャットパーティ
            // 17:血盟王族公告頻道
            IDataSourceRow dataSourceRow = dataSource.NewRow();


            if (target == null)
            {
                dataSourceRow.Insert()
                .Set(LogChat.Column_id, pc.Id)
                .Set(LogChat.Column_account_name, pc.AccountName)
                .Set(LogChat.Column_char_id, pc.Id)
                .Set(LogChat.Column_name, pc.Name)
                .Set(LogChat.Column_clan_id, pc.Clanid)
                .Set(LogChat.Column_clan_name, pc.Clanname)
                .Set(LogChat.Column_locx, pc.X)
                .Set(LogChat.Column_locy, pc.Y)
                .Set(LogChat.Column_mapid, pc.MapId)
                .Set(LogChat.Column_type, pc.Type)
                .Set(LogChat.Column_content, text)
                .Set(LogChat.Column_datetime, DateTime.Now)
                .Execute();
            }
            else
            {
                dataSourceRow.Insert()
 .Set(LogChat.Column_id, pc.Id)
 .Set(LogChat.Column_account_name, pc.AccountName)
 .Set(LogChat.Column_char_id, pc.Id)
 .Set(LogChat.Column_name, pc.Name)
 .Set(LogChat.Column_clan_id, pc.Clanid)
 .Set(LogChat.Column_clan_name, pc.Clanname)
 .Set(LogChat.Column_locx, pc.X)
 .Set(LogChat.Column_locy, pc.Y)
 .Set(LogChat.Column_mapid, pc.MapId)
 .Set(LogChat.Column_type, pc.Type)
 .Set(LogChat.Column_target_account_name, target.AccountName)
 .Set(LogChat.Column_target_id, target.Id)
 .Set(LogChat.Column_target_name, target.Name)
 .Set(LogChat.Column_target_clan_id, target.Clanid)
 .Set(LogChat.Column_target_clan_name, target.Clanname)
 .Set(LogChat.Column_target_locx, target.X)
 .Set(LogChat.Column_target_locy, target.Y)
 .Set(LogChat.Column_target_mapid, target.MapId)
 .Set(LogChat.Column_content, text)
 .Set(LogChat.Column_datetime, DateTime.Now)
 .Execute();
            }
        }

    }
}