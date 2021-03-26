using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.identity;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來建立組隊的封包
	/// </summary>
	class C_CreateParty : ClientBasePacket
	{

		private const string C_CREATE_PARTY = "[C] C_CreateParty";

		public C_CreateParty(byte[] decrypt, ClientThread client) : base(decrypt)
		{
			L1PcInstance pc = client.ActiveChar;
			if (pc == null)
			{
				return;
			}

			int type = ReadC();
			if ((type == 0) || (type == 1))
			{ // 自動接受組隊 on 與 off 的同
				int targetId = ReadD();
				L1Object temp = L1World.Instance.findObject(targetId);
				if (temp is L1PcInstance)
				{
					L1PcInstance targetPc = (L1PcInstance) temp;
					if (pc.Id == targetPc.Id)
					{
						return;
					}
					if ((!pc.Location.isInScreen(targetPc.Location) || (pc.Location.getTileLineDistance(targetPc.Location) > 7)))
					{
						// 邀請組隊時，對象不再螢幕內或是7步內
						pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message952));
						return;
					}
					if (targetPc.InParty)
					{
						// 您無法邀請已經參加其他隊伍的人。
						pc.sendPackets(new S_ServerMessage(415));
						return;
					}

					if (pc.InParty)
					{
						if (pc.Party.isLeader(pc))
						{
							targetPc.PartyType = type;
							targetPc.PartyID = pc.Id;
							switch (type)
							{
							case 0:
								// 玩家 %0%s 邀請您加入隊伍？(Y/N)
								targetPc.sendPackets(new S_Message_YN(953, pc.Name));
								break;
							case 1:
								// 玩家 %0%s 邀請您加入自動分配隊伍？(Y/N)
								targetPc.sendPackets(new S_Message_YN(954, pc.Name));
								break;
							}
						}
						else
						{
							// 只有領導者才能邀請其他的成員。
							pc.sendPackets(new S_ServerMessage(416));
						}
					}
					else
					{
						pc.PartyType = type;
						targetPc.PartyID = pc.Id;
						switch (type)
						{
						case 0:
							// 玩家 %0%s 邀請您加入隊伍？(Y/N)
							targetPc.sendPackets(new S_Message_YN(953, pc.Name));
							break;
						case 1:
							targetPc.sendPackets(new S_Message_YN(954, pc.Name));
							break;
						}
					}
				}
			}
			else if (type == 2)
			{ // 聊天組隊
				string name = ReadS();
				L1PcInstance targetPc = L1World.Instance.getPlayer(name);
				if (targetPc == null)
				{
					// 沒有叫%0的人。
					pc.sendPackets(new S_ServerMessage(109));
					return;
				}
				if (pc.Id == targetPc.Id)
				{
					return;
				}
				if ((!pc.Location.isInScreen(targetPc.Location) || (pc.Location.getTileLineDistance(targetPc.Location) > 7)))
				{
					// 邀請組隊時，對象不再螢幕內或是7步內
					pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message952));
					return;
				}
				if (targetPc.InChatParty)
				{
					// 您無法邀請已經參加其他隊伍的人。
					pc.sendPackets(new S_ServerMessage(415));
					return;
				}

				if (pc.InChatParty)
				{
					if (pc.ChatParty.isLeader(pc))
					{
						targetPc.PartyID = pc.Id;
						// 您要接受玩家 %0%s 提出的隊伍對話邀請嗎？(Y/N)
						targetPc.sendPackets(new S_Message_YN(951, pc.Name));
					}
					else
					{
						// 只有領導者才能邀請其他的成員。
						pc.sendPackets(new S_ServerMessage(416));
					}
				}
				else
				{
					targetPc.PartyID = pc.Id;
					// 您要接受玩家 %0%s 提出的隊伍對話邀請嗎？(Y/N)
					targetPc.sendPackets(new S_Message_YN(951, pc.Name));
				}
			}
			// 隊長委任
			else if (type == 3)
			{
				// 不是隊長時, 不可使用
				if ((pc.Party == null) || !pc.Party.isLeader(pc))
				{
					pc.sendPackets(new S_ServerMessage(1697));
					return;
				}

				// 取得目標物件編號
				int targetId = ReadD();

				// 嘗試取得目標
				L1Object obj = L1World.Instance.findObject(targetId);

				// 判斷目標是否合理
				if ((obj == null) || (pc.Id == obj.Id) || !(obj is L1PcInstance))
				{
					return;
				}
				if ((!pc.Location.isInScreen(obj.Location) || (pc.Location.getTileLineDistance(obj.Location) > 7)))
				{
					// 邀請組隊時，對象不再螢幕內或是7步內
					pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message1695));
					return;
				}

				// 轉型為玩家物件
				L1PcInstance targetPc = (L1PcInstance) obj;

				// 判斷目標是否屬於相同隊伍
				if (!targetPc.InParty)
				{
					pc.sendPackets(new S_ServerMessage(1696));
					return;
				}
				// 委任給其他玩家?
				pc.sendPackets(new S_Message_YN(L1SystemMessageId.Message1703, ""));

				// 指定隊長給新的目標
				pc.Party.passLeader(targetPc);
			}
		}

		public override string Type
		{
			get
			{
				return C_CREATE_PARTY;
			}
		}

	}

}