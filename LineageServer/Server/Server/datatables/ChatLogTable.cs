/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Server.datatables
{

	using Config = LineageServer.Server.Config;
	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;

	public class ChatLogTable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(ChatLogTable).FullName);

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
			Connection con = null;
			PreparedStatement pstm = null;
			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				if (target != null)
				{
					pstm = con.prepareStatement("INSERT INTO log_chat (account_name, char_id, name, clan_id, clan_name, locx, locy, mapid, type, target_account_name, target_id, target_name, target_clan_id, target_clan_name, target_locx, target_locy, target_mapid, content, datetime) VALUE (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, SYSDATE())");
					pstm.setString(1, pc.AccountName);
					pstm.setInt(2, pc.Id);
					pstm.setString(3, pc.Gm ? "******" : pc.Name);
					pstm.setInt(4, pc.Clanid);
					pstm.setString(5, pc.Clanname);
					pstm.setInt(6, pc.X);
					pstm.setInt(7, pc.Y);
					pstm.setInt(8, pc.MapId);
					pstm.setInt(9, type);
					pstm.setString(10, target.AccountName);
					pstm.setInt(11, target.Id);
					pstm.setString(12, target.Name);
					pstm.setInt(13, target.Clanid);
					pstm.setString(14, target.Clanname);
					pstm.setInt(15, target.X);
					pstm.setInt(16, target.Y);
					pstm.setInt(17, target.MapId);
					pstm.setString(18, text);
				}
				else
				{
					pstm = con.prepareStatement("INSERT INTO log_chat (account_name, char_id, name, clan_id, clan_name, locx, locy, mapid, type, content, datetime) VALUE (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, SYSDATE())");
					pstm.setString(1, pc.AccountName);
					pstm.setInt(2, pc.Id);
					pstm.setString(3, pc.Gm ? "******" : pc.Name);
					pstm.setInt(4, pc.Clanid);
					pstm.setString(5, pc.Clanname);
					pstm.setInt(6, pc.X);
					pstm.setInt(7, pc.Y);
					pstm.setInt(8, pc.MapId);
					pstm.setInt(9, type);
					pstm.setString(10, text);
				}
				pstm.execute();

			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

	}
}