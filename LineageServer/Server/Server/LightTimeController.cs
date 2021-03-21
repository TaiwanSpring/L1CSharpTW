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
	using LightSpawnTable = LineageServer.Server.Server.datatables.LightSpawnTable;
	using L1Object = LineageServer.Server.Server.Model.L1Object;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1FieldObjectInstance = LineageServer.Server.Server.Model.Instance.L1FieldObjectInstance;
	using L1GameTimeClock = LineageServer.Server.Server.Model.gametime.L1GameTimeClock;

	public class LightTimeController : IRunnableStart
	{
		private static LightTimeController _instance;

		private bool isSpawn = false;

		public static LightTimeController Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new LightTimeController();
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
					checkLightTime();
					Thread.Sleep(60000);
				}
			}
			catch (Exception)
			{
			}
		}

		private void checkLightTime()
		{
			int serverTime = L1GameTimeClock.Instance.currentTime().Seconds;
			int nowTime = serverTime % 86400;
			if ((nowTime >= ((5 * 3600) + 3300)) && (nowTime < ((17 * 3600) + 3300)))
			{ // 5:55~17:55
				if (isSpawn)
				{
					isSpawn = false;
					foreach (L1Object @object in L1World.Instance.Object)
					{
						if (@object is L1FieldObjectInstance)
						{
							L1FieldObjectInstance npc = (L1FieldObjectInstance) @object;
							if (((npc.NpcTemplate.get_npcId() == 81177) || (npc.NpcTemplate.get_npcId() == 81178) || (npc.NpcTemplate.get_npcId() == 81179) || (npc.NpcTemplate.get_npcId() == 81180) || (npc.NpcTemplate.get_npcId() == 81181)) && ((npc.MapId == 0) || (npc.MapId == 4)))
							{
								npc.deleteMe();
							}
						}
					}
				}
			}
			else if (((nowTime >= ((17 * 3600) + 3300)) && (nowTime <= 24 * 3600)) || ((nowTime >= 0 * 3600) && (nowTime < ((5 * 3600) + 3300))))
			{ // 17:55~24:00,0:00~5:55
				if (!isSpawn)
				{
					isSpawn = true;
					LightSpawnTable.Instance;
				}
			}
		}

	}

}