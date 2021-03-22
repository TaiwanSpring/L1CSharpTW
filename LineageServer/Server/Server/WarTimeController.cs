using System;
using System.Collections.Generic;
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
	using CastleTable = LineageServer.Server.Server.DataSources.CastleTable;
	using DoorTable = LineageServer.Server.Server.DataSources.DoorTable;
	using L1CastleLocation = LineageServer.Server.Server.Model.L1CastleLocation;
	using L1Clan = LineageServer.Server.Server.Model.L1Clan;
	using L1Object = LineageServer.Server.Server.Model.L1Object;
	using L1Teleport = LineageServer.Server.Server.Model.L1Teleport;
	using L1WarSpawn = LineageServer.Server.Server.Model.L1WarSpawn;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1CrownInstance = LineageServer.Server.Server.Model.Instance.L1CrownInstance;
	using L1DoorInstance = LineageServer.Server.Server.Model.Instance.L1DoorInstance;
	using L1FieldObjectInstance = LineageServer.Server.Server.Model.Instance.L1FieldObjectInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1TowerInstance = LineageServer.Server.Server.Model.Instance.L1TowerInstance;
	using S_PacketBox = LineageServer.Server.Server.serverpackets.S_PacketBox;
	using L1Castle = LineageServer.Server.Server.Templates.L1Castle;

	public class WarTimeController : IRunnableStart
	{
		private static WarTimeController _instance;

		private L1Castle[] _l1castle = new L1Castle[8];

		private DateTime[] _war_start_time = new DateTime[8];

		private DateTime[] _war_end_time = new DateTime[8];

		private bool[] _is_now_war = new bool[8];

		private WarTimeController()
		{
			for (int i = 0; i < _l1castle.Length; i++)
			{
				_l1castle[i] = CastleTable.Instance.getCastleTable(i + 1);
				_war_start_time[i] = _l1castle[i].WarTime;
				_war_end_time[i] = (DateTime) _l1castle[i].WarTime.clone();
				_war_end_time[i].add(Config.ALT_WAR_TIME_UNIT, Config.ALT_WAR_TIME);
			}
		}

		public static WarTimeController Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new WarTimeController();
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
					checkWarTime(); // 檢查攻城時間
					Thread.Sleep(1000);
				}
			}
			catch (Exception)
			{
			}
		}

		public virtual DateTime RealTime
		{
			get
			{
				TimeZone _tz = TimeZone.getTimeZone(Config.TIME_ZONE);
				DateTime cal = DateTime.getInstance(_tz);
				return cal;
			}
		}

		public virtual bool isNowWar(int castle_id)
		{
			return _is_now_war[castle_id - 1];
		}

		// TODO 
		public virtual void checkCastleWar(L1PcInstance player)
		{
			IList<string> castle = new List<string>();
			for (int i = 0; i < 8; i++)
			{
				if (_is_now_war[i])
				{
					castle.Add(CastleTable.Instance.getCastleTable(i + 1).Name);
					// 攻城戰進行中。
					player.sendPackets(new S_PacketBox(S_PacketBox.MSG_WAR_IS_GOING_ALL, castle.ToArray()));
				}
			}
		}

		private void checkWarTime()
		{
			for (int i = 0; i < 8; i++)
			{
				if (_war_start_time[i] < RealTime && _war_end_time[i] > RealTime)
				{
					if (_is_now_war[i] == false)
					{
						_is_now_war[i] = true;
						// 招出攻城的旗子
						L1WarSpawn warspawn = new L1WarSpawn();
						warspawn.SpawnFlag(i + 1);
						// 修理城門並設定為關閉
						foreach (L1DoorInstance door in DoorTable.Instance.DoorList)
						{
							if (L1CastleLocation.checkInWarArea(i + 1, door))
							{
								door.repairGate();
							}
						}

						L1World.Instance.broadcastPacketToAll(new S_PacketBox(S_PacketBox.MSG_WAR_BEGIN, i + 1)); // %sの攻城戦が始まりました。
						int[] loc = new int[3];
						foreach (L1PcInstance pc in L1World.Instance.AllPlayers)
						{
							int castleId = i + 1;
							if (L1CastleLocation.checkInWarArea(castleId, pc) && !pc.Gm)
							{ // 剛好在攻城範圍內
								L1Clan clan = L1World.Instance.getClan(pc.Clanname);
								if (clan != null)
								{
									if (clan.CastleId == castleId)
									{ // 如果是城血盟
										continue;
									}
								}
								loc = L1CastleLocation.getGetBackLoc(castleId);
								L1Teleport.teleport(pc, loc[0], loc[1],(short) loc[2], 5, true);
							}
						}
					}
				}
				else if (_war_end_time[i] < RealTime)
				{ // 攻城結束
					if (_is_now_war[i] == true)
					{
						_is_now_war[i] = false;
						L1World.Instance.broadcastPacketToAll(new S_PacketBox(S_PacketBox.MSG_WAR_END, i + 1)); // %sの攻城戦が終了しました。
						// 更新攻城時間
						WarUpdate(i);

						int castle_id = i + 1;
						foreach (L1Object l1object in L1World.Instance.Object)
						{
							// 取消攻城的旗子
							if (l1object is L1FieldObjectInstance)
							{
								L1FieldObjectInstance flag = (L1FieldObjectInstance) l1object;
								if (L1CastleLocation.checkInWarArea(castle_id, flag))
								{
									flag.deleteMe();
								}
							}
							// 移除皇冠
							if (l1object is L1CrownInstance)
							{
								L1CrownInstance crown = (L1CrownInstance) l1object;
								if (L1CastleLocation.checkInWarArea(castle_id, crown))
								{
									crown.deleteMe();
								}
							}
							// 移除守護塔
							if (l1object is L1TowerInstance)
							{
								L1TowerInstance tower = (L1TowerInstance) l1object;
								if (L1CastleLocation.checkInWarArea(castle_id,tower))
								{
									tower.deleteMe();
								}
							}
						}
						// 塔重新出現
						L1WarSpawn warspawn = new L1WarSpawn();
						warspawn.SpawnTower(castle_id);

						// 移除城門
						foreach (L1DoorInstance door in DoorTable.Instance.DoorList)
						{
							if (L1CastleLocation.checkInWarArea(castle_id, door))
							{
								door.repairGate();
							}
						}
					}
					else
					{ // 更新過期的攻城時間
						_war_start_time[i] = RealTime;
						_war_end_time[i] = (DateTime) _war_start_time[i].clone();
						WarUpdate(i);
					}
				}

			}
		}

		private void WarUpdate(int i)
		{
			_war_start_time[i].add(Config.ALT_WAR_INTERVAL_UNIT,Config.ALT_WAR_INTERVAL);
			_war_end_time[i].add(Config.ALT_WAR_INTERVAL_UNIT,Config.ALT_WAR_INTERVAL);
			_l1castle[i].WarTime = _war_start_time[i];
			_l1castle[i].TaxRate = 10; // 稅率10%
			_l1castle[i].PublicMoney = 0; // 清除城堡稅收
			CastleTable.Instance.updateCastle(_l1castle[i]);
		}
	}

}