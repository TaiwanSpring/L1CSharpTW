using System;

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
namespace LineageServer.Server.Model.Instance
{
	using HouseTable = LineageServer.Server.DataSources.HouseTable;
	using NPCTalkDataTable = LineageServer.Server.DataSources.NPCTalkDataTable;
	using L1Attack = LineageServer.Server.Model.L1Attack;
	using L1Clan = LineageServer.Server.Model.L1Clan;
	using L1NpcTalkData = LineageServer.Server.Model.L1NpcTalkData;
	using L1World = LineageServer.Server.Model.L1World;
	using S_NPCTalkReturn = LineageServer.Serverpackets.S_NPCTalkReturn;
	using L1House = LineageServer.Server.Templates.L1House;
	using L1Npc = LineageServer.Server.Templates.L1Npc;

	[Serializable]
	public class L1HousekeeperInstance : L1NpcInstance
	{
		/// 
		private const long serialVersionUID = 1L;

		/// <param name="template"> </param>
		public L1HousekeeperInstance(L1Npc template) : base(template)
		{
		}

		public override void onAction(L1PcInstance pc)
		{
			onAction(pc, 0);
		}

		public override void onAction(L1PcInstance pc, int skillId)
		{
			L1Attack attack = new L1Attack(pc, this, skillId);
			attack.calcHit();
			attack.action();
			attack.addChaserAttack();
			attack.calcDamage();
			attack.calcStaffOfMana();
			attack.addPcPoisonAttack(pc, this);
			attack.commit();
		}

		public override void onTalkAction(L1PcInstance pc)
		{
			int objid = Id;
			L1NpcTalkData talking = NPCTalkDataTable.Instance.getTemplate(NpcTemplate.get_npcId());
			int npcid = NpcTemplate.get_npcId();
			string htmlid = null;
			string[] htmldata = null;
			bool isOwner = false;

			if (talking != null)
			{
				// 話しかけたPCが所有者とそのクラン員かどうか調べる
				L1Clan clan = L1World.Instance.getClan(pc.Clanname);
				if (clan != null)
				{
					int houseId = clan.HouseId;
					if (houseId != 0)
					{
						L1House house = HouseTable.Instance.getHouseTable(houseId);
						if (npcid == house.KeeperId)
						{
							isOwner = true;
						}
					}
				}

				// 所有者とそのクラン員以外なら会話内容を変える
				if (!isOwner)
				{
					// Housekeeperが属するアジトを取得する
					L1House targetHouse = null;
					foreach (L1House house in HouseTable.Instance.HouseTableList)
					{
						if (npcid == house.KeeperId)
						{
							targetHouse = house;
							break;
						}
					}

					// アジトがに所有者が居るかどうか調べる
					bool isOccupy = false;
					string clanName = null;
					string leaderName = null;
					foreach (L1Clan targetClan in L1World.Instance.AllClans)
					{
						if (targetHouse.HouseId == targetClan.HouseId)
						{
							isOccupy = true;
							clanName = targetClan.ClanName;
							leaderName = targetClan.LeaderName;
							break;
						}
					}

					// 会話内容を設定する
					if (isOccupy)
					{ // 所有者あり
						htmlid = "agname";
						htmldata = new string[] {clanName, leaderName, targetHouse.HouseName};
					}
					else
					{ // 所有者なし(競売中)
						htmlid = "agnoname";
						htmldata = new string[] {targetHouse.HouseName};
					}
				}

				// html表示パケット送信
				if (!string.ReferenceEquals(htmlid, null))
				{ // htmlidが指定されている場合
					if (htmldata != null)
					{ // html指定がある場合は表示
						pc.sendPackets(new S_NPCTalkReturn(objid, htmlid, htmldata));
					}
					else
					{
						pc.sendPackets(new S_NPCTalkReturn(objid, htmlid));
					}
				}
				else
				{
					if (pc.Lawful < -1000)
					{ // プレイヤーがカオティック
						pc.sendPackets(new S_NPCTalkReturn(talking, objid, 2));
					}
					else
					{
						pc.sendPackets(new S_NPCTalkReturn(talking, objid, 1));
					}
				}
			}
		}

		public override void onFinalAction(L1PcInstance pc, string action)
		{
		}

		public virtual void doFinalAction(L1PcInstance pc)
		{
		}

	}

}