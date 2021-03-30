using System.Collections.Generic;
using System.Linq;

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
namespace LineageServer.Server.DataTables
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1NpcInstance = LineageServer.Server.Model.Instance.L1NpcInstance;
	using L1NpcChat = LineageServer.Server.Templates.L1NpcChat;
	using SQLUtil = LineageServer.Utils.SQLUtil;
	using MapFactory = LineageServer.Utils.MapFactory;

	public class NpcChatTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(NpcChatTable).FullName);

		private static NpcChatTable _instance;

		private IDictionary<int, L1NpcChat> _npcChatAppearance = MapFactory.NewMap();

		private IDictionary<int, L1NpcChat> _npcChatDead = MapFactory.NewMap();

		private IDictionary<int, L1NpcChat> _npcChatHide = MapFactory.NewMap();

		private IDictionary<int, L1NpcChat> _npcChatGameTime = MapFactory.NewMap();

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
			FillNpcChatTable();
		}

		private void FillNpcChatTable()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM npcchat");
				rs = pstm.executeQuery();
				while (rs.next())
				{
					L1NpcChat npcChat = new L1NpcChat();
					npcChat.NpcId = dataSourceRow.getInt("npc_id");
					npcChat.ChatTiming = dataSourceRow.getInt("chat_timing");
					npcChat.StartDelayTime = dataSourceRow.getInt("start_delay_time");
					npcChat.ChatId1 = dataSourceRow.getString("chat_id1");
					npcChat.ChatId2 = dataSourceRow.getString("chat_id2");
					npcChat.ChatId3 = dataSourceRow.getString("chat_id3");
					npcChat.ChatId4 = dataSourceRow.getString("chat_id4");
					npcChat.ChatId5 = dataSourceRow.getString("chat_id5");
					npcChat.ChatInterval = dataSourceRow.getInt("chat_interval");
					npcChat.Shout = dataSourceRow.getBoolean("is_shout");
					npcChat.WorldChat = dataSourceRow.getBoolean("is_world_chat");
					npcChat.Repeat = dataSourceRow.getBoolean("is_repeat");
					npcChat.RepeatInterval = dataSourceRow.getInt("repeat_interval");
					npcChat.GameTime = dataSourceRow.getInt("game_time");

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
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
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