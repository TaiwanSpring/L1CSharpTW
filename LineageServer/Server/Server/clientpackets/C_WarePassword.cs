﻿/// <summary>
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
namespace LineageServer.Server.Server.Clientpackets
{
	using Account = LineageServer.Server.Server.Account;
	using ClientThread = LineageServer.Server.Server.ClientThread;
	using L1Clan = LineageServer.Server.Server.Model.L1Clan;
	using L1Object = LineageServer.Server.Server.Model.L1Object;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1SystemMessageId = LineageServer.Server.Server.Model.identity.L1SystemMessageId;
	using S_RetrieveElfList = LineageServer.Server.Server.serverpackets.S_RetrieveElfList;
	using S_RetrieveList = LineageServer.Server.Server.serverpackets.S_RetrieveList;
	using S_RetrievePledgeList = LineageServer.Server.Server.serverpackets.S_RetrievePledgeList;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

	class C_WarePassword : ClientBasePacket
	{
		public C_WarePassword(sbyte[] abyte0, ClientThread client) : base(abyte0)
		{

			L1PcInstance pc = client.ActiveChar;
			if (pc == null)
			{
				return;
			}

			// 類型(0: 密碼變更, 1: 一般倉庫, 2: 血盟倉庫)
			int type = readC();

			// 取得第一組數值(舊密碼, 或待驗證的密碼)
			int pass1 = readD();

			// 取得第二組數值(新密碼, 或倉庫 NPC 的 objId)
			int pass2 = readD();

			// 不明的2個位元組
			readH();

			// 取得角色物件
			Account account = client.Account;

			// 變更密碼
			if (type == 0)
			{
				// 兩次皆直接跳過密碼輸入
				if ((pass1 < 0) && (pass2 < 0))
				{
					pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message79));
				}

				// 進行新密碼的設定
				else if ((pass1 < 0) && (account.WarePassword == 0))
				{
					// 進行密碼變更
					account.changeWarePassword(pass2);
					pc.sendPackets(new S_SystemMessage("倉庫密碼設定完成，請牢記您的新密碼。"));
				}

				// 進行密碼變更
				else if ((pass1 > 0) && (pass1 == account.WarePassword))
				{
					// 進行密碼變更
					if (pass1 == pass2)
					{
						// [342::你不能使用舊的密碼當作新的密碼。請再次輸入密碼。]
						pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message342));
						return;
					}
					else if (pass2 > 0)
					{
						account.changeWarePassword(pass2);
						pc.sendPackets(new S_SystemMessage("倉庫密碼變更完成，請牢記您的新密碼。"));
					}
					else
					{
						account.changeWarePassword(0);
						pc.sendPackets(new S_SystemMessage("倉庫密碼取消完成。"));
					}
				}
				else
				{
					// 送出密碼錯誤的提示訊息[835:密碼錯誤。]
					pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message835));
				}
			}

			// 密碼驗證
			else
			{
				if (account.WarePassword == pass1)
				{
					int objid = pass2;
					L1Object obj = L1World.Instance.findObject(objid);
					if (pc.Level >= 5)
					{ // 判斷玩家等級
						if (type == 1)
						{
							if (obj != null)
							{
								if (obj is L1NpcInstance)
								{
									L1NpcInstance npc = (L1NpcInstance) obj;
									// 判斷npc所屬倉庫類別
									switch (npc.NpcId)
									{
									case 60028: // 倉庫-艾爾(妖森)
										// 密碼吻合 輸出倉庫視窗
										if (pc.Elf) // 判斷是否為妖精
										{
											pc.sendPackets(new S_RetrieveElfList(objid, pc));
										}
										break;
									default:
										// 密碼吻合 輸出倉庫視窗
										pc.sendPackets(new S_RetrieveList(objid, pc));
										break;
									}
								}
							}
						}
						else if (type == 2)
						{
							if (pc.Clanid == 0)
							{
								// \f1若想使用血盟倉庫，必須加入血盟。
								pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message208));
								return;
							}
							int rank = pc.ClanRank;
							if (rank == L1Clan.CLAN_RANK_PUBLIC)
							{
								// 只有收到稱謂的人才能使用血盟倉庫。
								pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message728));
								return;
							}
							if ((rank != L1Clan.CLAN_RANK_PROBATION) && (rank != L1Clan.CLAN_RANK_GUARDIAN) && (rank != L1Clan.CLAN_RANK_LEAGUE_PROBATION) && (rank != L1Clan.CLAN_RANK_PRINCE) && (rank != L1Clan.CLAN_RANK_LEAGUE_VICEPRINCE) && (rank != L1Clan.CLAN_RANK_LEAGUE_GUARDIAN) && (rank != L1Clan.CLAN_RANK_LEAGUE_PRINCE))
							{
								// 只有收到稱謂的人才能使用血盟倉庫。
								pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message728));
								return;
							}
							pc.sendPackets(new S_RetrievePledgeList(objid, pc));

						}
					}
				}
				else
				{
					// 送出密碼錯誤的提示訊息
					pc.sendPackets(new S_ServerMessage(835));
				}
			}
		}
	}

}