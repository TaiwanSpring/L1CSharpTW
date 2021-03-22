using System;
using System.Threading;

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
namespace LineageServer.Server.Server
{

	using Config = LineageServer.Server.Config;
	using NpcChatTable = LineageServer.Server.Server.DataSources.NpcChatTable;
	using L1Object = LineageServer.Server.Server.Model.L1Object;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using L1NpcChat = LineageServer.Server.Server.Templates.L1NpcChat;

	public class NpcChatTimeController : IRunnableStart
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(NpcChatTimeController).FullName);

		private static NpcChatTimeController _instance;

		public static NpcChatTimeController Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new NpcChatTimeController();
				}
				return _instance;
			}
		}

		public override void run()
		{
			try
			{
				while (true)
				{
					checkNpcChatTime(); // 檢查開始聊天時間
					Thread.Sleep(60000);
				}
			}
			catch (Exception e1)
			{
				_log.warning(e1.Message);
			}
		}

		private void checkNpcChatTime()
		{
			foreach (L1NpcChat npcChat in NpcChatTable.Instance.AllGameTime)
			{
				if (isChatTime(npcChat.GameTime))
				{
					int npcId = npcChat.NpcId;
					foreach (L1Object obj in L1World.Instance.Object)
					{
						if (!(obj is L1NpcInstance))
						{
							continue;
						}
						L1NpcInstance npc = (L1NpcInstance) obj;
						if (npc.NpcTemplate.get_npcId() == npcId)
						{
							npc.startChat(L1NpcInstance.CHAT_TIMING_GAME_TIME);
						}
					}
				}
			}
		}

		private bool isChatTime(int chatTime)
		{
			SimpleDateFormat sdf = new SimpleDateFormat("HHmm");
			DateTime realTime = RealTime;
			int nowTime = Convert.ToInt32(sdf.format(realTime));
			return (nowTime == chatTime);
		}

		private static DateTime RealTime
		{
			get
			{
				TimeZone _tz = TimeZone.getTimeZone(Config.TIME_ZONE);
				DateTime cal = DateTime.getInstance(_tz);
				return cal;
			}
		}

	}

}