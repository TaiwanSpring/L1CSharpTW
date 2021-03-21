using System.Collections.Generic;

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
namespace LineageServer.Server.Server.utils
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.MEDITATION;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.WIND_SHACKLE;

	using L1Clan = LineageServer.Server.Server.Model.L1Clan;
	using L1DragonSlayer = LineageServer.Server.Server.Model.L1DragonSlayer;
	using L1Location = LineageServer.Server.Server.Model.L1Location;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1DollInstance = LineageServer.Server.Server.Model.Instance.L1DollInstance;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1PetInstance = LineageServer.Server.Server.Model.Instance.L1PetInstance;
	using L1SummonInstance = LineageServer.Server.Server.Model.Instance.L1SummonInstance;
	using L1Map = LineageServer.Server.Server.Model.map.L1Map;
	using L1WorldMap = LineageServer.Server.Server.Model.map.L1WorldMap;
	using S_CharVisualUpdate = LineageServer.Server.Server.serverpackets.S_CharVisualUpdate;
	using S_DollPack = LineageServer.Server.Server.serverpackets.S_DollPack;
	using S_MapID = LineageServer.Server.Server.serverpackets.S_MapID;
	using S_OtherCharPacks = LineageServer.Server.Server.serverpackets.S_OtherCharPacks;
	using S_OwnCharPack = LineageServer.Server.Server.serverpackets.S_OwnCharPack;
	using S_PetPack = LineageServer.Server.Server.serverpackets.S_PetPack;
	using S_SkillIconWindShackle = LineageServer.Server.Server.serverpackets.S_SkillIconWindShackle;
	using S_SummonPack = LineageServer.Server.Server.serverpackets.S_SummonPack;

	// Referenced classes of package l1j.server.server.utils:
	// FaceToFace

	public class Teleportation
	{
		private Teleportation()
		{
		}

		public static void actionTeleportation(in L1PcInstance pc)
		{
			if (pc.Dead || pc.Teleport)
			{
				return;
			}

			int x = pc.TeleportX;
			int y = pc.TeleportY;
			short mapId = pc.TeleportMapId;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int head = pc.getTeleportHeading();
			int head = pc.TeleportHeading;

			// テレポート先が不正であれば元の座標へ(GMは除く)
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final l1j.server.server.model.map.L1Map map = l1j.server.server.model.map.L1WorldMap.getInstance().getMap(mapId);
			L1Map map = L1WorldMap.Instance.getMap(mapId);

			if (!map.isInMap(x, y) && !pc.Gm)
			{
				x = pc.X;
				y = pc.Y;
				mapId = pc.MapId;
			}

			pc.Teleport = true;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final l1j.server.server.model.L1Clan clan = l1j.server.server.model.L1World.getInstance().getClan(pc.getClanname());
			L1Clan clan = L1World.Instance.getClan(pc.Clanname);
			if (clan != null)
			{
				if (clan.WarehouseUsingChar == pc.Id)
				{ // 自キャラがクラン倉庫使用中
					clan.WarehouseUsingChar = 0; // クラン倉庫のロックを解除
				}
			}

			L1World.Instance.moveVisibleObject(pc, mapId);
			pc.setLocation(x, y, mapId);
			pc.Heading = head;
			pc.sendPackets(new S_MapID(pc.MapId, pc.Map.Underwater));

			if (pc.ReserveGhost)
			{ // ゴースト状態解除
				pc.endGhost();
			}
			if (pc.Ghost || pc.GmInvis)
			{
			}
			else if (pc.Invisble)
			{
				pc.broadcastPacketForFindInvis(new S_OtherCharPacks(pc, true), true);
			}
			else
			{
				pc.broadcastPacket(new S_OtherCharPacks(pc));
			}
			pc.sendPackets(new S_OwnCharPack(pc));

			pc.removeAllKnownObjects();
			pc.sendVisualEffectAtTeleport(); // クラウン、毒、水中等の視覚効果を表示
			pc.updateObject();
			// spr番号6310, 5641の変身中にテレポートするとテレポート後に移動できなくなる
			// 武器を着脱すると移動できるようになるため、S_CharVisualUpdateを送信する
			pc.sendPackets(new S_CharVisualUpdate(pc));

			pc.killSkillEffectTimer(MEDITATION);
			pc.CallClanId = 0; // コールクランを唱えた後に移動すると召喚無効

			/*
			 * subjects ペットとサモンのテレポート先画面内へ居たプレイヤー。
			 * 各ペット毎にUpdateObjectを行う方がコード上ではスマートだが、
			 * ネットワーク負荷が大きくなる為、一旦Setへ格納して最後にまとめてUpdateObjectする。
			 */
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.HashSet<l1j.server.server.model.Instance.L1PcInstance> subjects = new java.util.HashSet<l1j.server.server.model.Instance.L1PcInstance>();
			HashSet<L1PcInstance> subjects = new HashSet<L1PcInstance>();
			subjects.Add(pc);

			if (!pc.Ghost)
			{
				if (pc.Map.TakePets)
				{
					// ペットとサモンも一緒に移動させる。
					foreach (L1NpcInstance petNpc in pc.PetList.Values)
					{
						// テレポート先の設定
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final l1j.server.server.model.L1Location loc = pc.getLocation().randomLocation(3, false);
						L1Location loc = pc.Location.randomLocation(3, false);
						int nx = loc.X;
						int ny = loc.Y;
						if ((pc.MapId == 5125) || (pc.MapId == 5131) || (pc.MapId == 5132) || (pc.MapId == 5133) || (pc.MapId == 5134))
						{ // ペットマッチ会場
							nx = 32799 + RandomHelper.Next(5) - 3;
							ny = 32864 + RandomHelper.Next(5) - 3;
						}
						teleport(petNpc, nx, ny, mapId, head);
						if (petNpc is L1SummonInstance)
						{ // サモンモンスター
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final l1j.server.server.model.Instance.L1SummonInstance summon = (l1j.server.server.model.Instance.L1SummonInstance) petNpc;
							L1SummonInstance summon = (L1SummonInstance) petNpc;
							pc.sendPackets(new S_SummonPack(summon, pc));
						}
						else if (petNpc is L1PetInstance)
						{ // ペット
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final l1j.server.server.model.Instance.L1PetInstance pet = (l1j.server.server.model.Instance.L1PetInstance) petNpc;
							L1PetInstance pet = (L1PetInstance) petNpc;
							pc.sendPackets(new S_PetPack(pet, pc));
						}

						foreach (L1PcInstance visiblePc in L1World.Instance.getVisiblePlayer(petNpc))
						{
							// テレポート元と先に同じPCが居た場合、正しく更新されない為、一度removeする。
							visiblePc.removeKnownObject(petNpc);
							subjects.Add(visiblePc);
						}
					}

					// マジックドールも一緒に移動させる。
					foreach (L1DollInstance doll in pc.DollList.Values)
					{
						// テレポート先の設定
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final l1j.server.server.model.L1Location loc = pc.getLocation().randomLocation(3, false);
						L1Location loc = pc.Location.randomLocation(3, false);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nx = loc.getX();
						int nx = loc.X;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int ny = loc.getY();
						int ny = loc.Y;

						teleport(doll, nx, ny, mapId, head);
						pc.sendPackets(new S_DollPack(doll));

						foreach (L1PcInstance visiblePc in L1World.Instance.getVisiblePlayer(doll))
						{
							// テレポート元と先に同じPCが居た場合、正しく更新されない為、一度removeする。
							visiblePc.removeKnownObject(doll);
							subjects.Add(visiblePc);
						}
					}
				}
				else
				{
					foreach (L1DollInstance doll in pc.DollList.Values)
					{
						// テレポート先の設定
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final l1j.server.server.model.L1Location loc = pc.getLocation().randomLocation(3, false);
						L1Location loc = pc.Location.randomLocation(3, false);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nx = loc.getX();
						int nx = loc.X;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int ny = loc.getY();
						int ny = loc.Y;

						teleport(doll, nx, ny, mapId, head);
						pc.sendPackets(new S_DollPack(doll));

						foreach (L1PcInstance visiblePc in L1World.Instance.getVisiblePlayer(doll))
						{
							// テレポート元と先に同じPCが居た場合、正しく更新されない為、一度removeする。
							visiblePc.removeKnownObject(doll);
							subjects.Add(visiblePc);
						}
					}
				}
			}

			foreach (L1PcInstance updatePc in subjects)
			{
				updatePc.updateObject();
			}

			pc.Teleport = false;

			if (pc.hasSkillEffect(L1SkillId.WIND_SHACKLE))
			{
				pc.sendPackets(new S_SkillIconWindShackle(pc.Id, pc.getSkillEffectTimeSec(WIND_SHACKLE)));
			}

			// 副本編號與副本地圖不符
			if (pc.PortalNumber != -1 && (pc.MapId != (1005 + pc.PortalNumber)))
			{
				L1DragonSlayer.Instance.removePlayer(pc, pc.PortalNumber);
				pc.PortalNumber = -1;
			}
			// 離開旅館地圖，旅館鑰匙歸零
			if (pc.MapId <= 10000 && pc.InnKeyId != 0)
			{
				pc.InnKeyId = 0;
			}
		}

		private static void teleport(L1NpcInstance npc, int x, int y, short map, int head)
		{
			L1World.Instance.moveVisibleObject(npc, map);
			L1WorldMap.Instance.getMap(npc.MapId).setPassable(npc.X, npc.Y, true);
			npc.X = x;
			npc.Y = y;
			npc.Map = map;
			npc.Heading = head;
			L1WorldMap.Instance.getMap(npc.MapId).setPassable(npc.X, npc.Y, false);
		}

	}

}