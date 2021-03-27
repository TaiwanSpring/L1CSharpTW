using LineageServer.Server.DataSources;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.Model.Instance
{
	[Serializable]
	class L1CrownInstance : L1NpcInstance
	{
		public L1CrownInstance(L1Npc template) : base(template)
		{
		}

		public override void onAction(L1PcInstance player)
		{
			bool in_war = false;
			if (player.Clanid == 0)
			{ // クラン未所属
				return;
			}
			string playerClanName = player.Clanname;
			L1Clan clan = L1World.Instance.getClan(playerClanName);
			if (clan == null)
			{
				return;
			}
			if (!player.Crown)
			{ // 君主以外
				return;
			}
			if (( player.TempCharGfx != 0 ) && ( player.TempCharGfx != 1 ))
			{
				return;
			}
			if (player.Id != clan.LeaderId)
			{ // 血盟主以外
				return;
			}
			if (!checkRange(player))
			{ // クラウンの1セル以内
				return;
			}
			if (clan.CastleId != 0)
			{
				// 城主クラン
				// あなたはすでに城を所有しているので、他の城を取ることは出来ません。
				player.sendPackets(new S_ServerMessage(474));
				return;
			}

			// クラウンの座標からcastle_idを取得
			int castle_id = L1CastleLocation.getCastleId(X, Y, MapId);

			// 布告しているかチェック。但し、城主が居ない場合は布告不要
			bool existDefenseClan = false;
			L1Clan defence_clan = null;
			foreach (L1Clan defClan in L1World.Instance.AllClans)
			{
				if (castle_id == defClan.CastleId)
				{
					// 元の城主クラン
					defence_clan = L1World.Instance.getClan(defClan.ClanName);
					existDefenseClan = true;
					break;
				}
			}
			IList<L1War> wars = L1World.Instance.WarList; // 全戦争リストを取得
			foreach (L1War war in wars)
			{
				if (castle_id == war.GetCastleId())
				{ // 今居る城の戦争
					in_war = war.CheckClanInWar(playerClanName);
					break;
				}
			}
			if (existDefenseClan && ( in_war == false ))
			{ // 城主が居て、布告していない場合
				return;
			}

			// clan_dataのhascastleを更新し、キャラクターにクラウンを付ける
			if (existDefenseClan && ( defence_clan != null ))
			{ // 元の城主クランが居る
				defence_clan.CastleId = 0;
				ClanTable.Instance.updateClan(defence_clan);
				L1PcInstance[] defence_clan_member = defence_clan.OnlineClanMember;
				foreach (L1PcInstance element in defence_clan_member)
				{
					if (element.Id == defence_clan.LeaderId)
					{ // 元の城主クランの君主
						element.sendPackets(new S_CastleMaster(0, element.Id));
						element.broadcastPacket(new S_CastleMaster(0, element.Id));
						break;
					}
				}
			}
			clan.CastleId = castle_id;
			ClanTable.Instance.updateClan(clan);
			player.sendPackets(new S_CastleMaster(castle_id, player.Id));
			player.broadcastPacket(new S_CastleMaster(castle_id, player.Id));

			// クラン員以外を街に強制テレポート
			int[] loc = new int[3];
			foreach (L1PcInstance pc in L1World.Instance.AllPlayers)
			{
				if (( pc.Clanid != player.Clanid ) && !pc.Gm)
				{

					if (L1CastleLocation.checkInWarArea(castle_id, pc))
					{
						// 旗内に居る
						loc = L1CastleLocation.getGetBackLoc(castle_id);
						int locx = loc[0];
						int locy = loc[1];
						short mapid = (short)loc[2];
						L1Teleport.teleport(pc, locx, locy, mapid, 5, true);
					}
				}
			}

			// メッセージ表示
			foreach (L1War war in wars)
			{
				if (war.CheckClanInWar(playerClanName) && existDefenseClan)
				{
					// 自クランが参加中で、城主が交代
					war.WinCastleWar(playerClanName);
					break;
				}
			}
			L1PcInstance[] clanMember = clan.OnlineClanMember;

			if (clanMember.Length > 0)
			{
				// 城を占拠しました。
				S_ServerMessage s_serverMessage = new S_ServerMessage(643);
				foreach (L1PcInstance pc in clanMember)
				{
					pc.sendPackets(s_serverMessage);
				}
			}

			// クラウンを消す
			deleteMe();

			// タワーを消して再出現させる
			foreach (GameObject l1object in L1World.Instance.Object)
			{
				if (l1object is L1TowerInstance)
				{
					L1TowerInstance tower = (L1TowerInstance)l1object;
					if (L1CastleLocation.checkInWarArea(castle_id, tower))
					{
						tower.deleteMe();
					}
				}
			}
			L1WarSpawn warspawn = new L1WarSpawn();
			warspawn.SpawnTower(castle_id);

			// 城門を元に戻す
			foreach (L1DoorInstance door in DoorTable.Instance.DoorList)
			{
				if (L1CastleLocation.checkInWarArea(castle_id, door))
				{
					door.repairGate();
				}
			}
		}

		public override void deleteMe()
		{
			_destroyed = true;
			if (Inventory != null)
			{
				Inventory.clearItems();
			}
			allTargetClear();
			_master = null;
			L1World.Instance.removeVisibleObject(this);
			L1World.Instance.removeObject(this);
			foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(this))
			{
				pc.removeKnownObject(this);
				pc.sendPackets(new S_RemoveObject(this));
			}
			removeAllKnownObjects();
		}

		private bool checkRange(L1PcInstance pc)
		{
			return ( ( X - 1 <= pc.X ) && ( pc.X <= X + 1 ) && ( Y - 1 <= pc.Y ) && ( pc.Y <= Y + 1 ) );
		}
	}

}