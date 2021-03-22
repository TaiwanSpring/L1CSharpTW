using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.utils;
using System;
namespace LineageServer.Server.Server.Clientpackets
{
	class C_SendLocation : ClientBasePacket
	{

		private const string C_SEND_LOCATION = "[C] C_SendLocation";

		public C_SendLocation(sbyte[] abyte0, ClientThread client) : base(abyte0)
		{
			int type = readC();

			// クライアントがアクティブ,ノンアクティブ転換時に
			// オペコード 0x57 0x0dパケットを送ってくるが詳細不明の為無視
			// マップ座標転送時は0x0bパケット
			if (type == 0x0d)
			{
				return;
				/*
				 * 視窗內:0d 01 xx // xx 就是值會變動，不知原因。
				 * 視窗外:0d 00 xx // xx 就是值會變動，不知原因。
				*/
			}

			if (type == 0x0b)
			{
				string name = readS();
				int mapId = readH();
				int x = readH();
				int y = readH();
				int msgId = readC();

				if (name.Length == 0)
				{
					return;
				}
				L1PcInstance target = L1World.Instance.getPlayer(name);
				if (target != null)
				{
					L1PcInstance pc = client.ActiveChar;
					string sender = pc.Name;
					target.sendPackets(new S_SendLocation(type, sender, mapId, x, y, msgId));
					// 将来的にtypeを使う可能性があるので送る
					pc.sendPackets(new S_ServerMessage(1783, name));
				}
			}
			else if (type == 0x06)
			{
				int objectId = readD();
				int gate = readD();
				int[] dragonGate = new int[] {81273, 81274, 81275, 81276};
				L1PcInstance pc = client.ActiveChar;
				if (gate >= 0 && gate <= 3)
				{
					DateTime nowTime = new DateTime();
					if (nowTime.Hour >= 8 && nowTime.Hour < 12)
					{
						pc.sendPackets(new S_ServerMessage(1643)); // 每日上午 8 點到 12 點為止，暫時無法使用龍之鑰匙。
					}
					else
					{
						bool limit = true;
						switch (gate)
						{
							case 0:
								for (int i = 0; i < 6; i++)
								{
									if (!L1DragonSlayer.Instance.PortalNumber[i])
									{
										limit = false;
									}
								}
								break;
							case 1:
								for (int i = 6; i < 12; i++)
								{
									if (!L1DragonSlayer.Instance.PortalNumber[i])
									{
										limit = false;
									}
								}
								break;
						}
						if (!limit)
						{ // 未達上限可開設龍門
							if (!pc.Inventory.consumeItem(47010, 1))
							{
								pc.sendPackets(new S_ServerMessage(1567)); // 需要龍之鑰匙。
								return;
							}
							L1SpawnUtil.spawn(pc, dragonGate[gate], 0, 120 * 60 * 1000); // 開啟 2 小時
						}
					}
				}
			}
			else if (type == 0x2e)
			{ // 識別盟徽 狀態
				L1PcInstance pc = client.ActiveChar;
				// 如果不是君主或聯盟王
				if (pc.ClanRank != 4 && pc.ClanRank != 10)
				{
					return;
				}

				int emblemStatus = readC(); // 0: 關閉 1:開啟

				L1Clan clan = pc.Clan;
				clan.EmblemStatus = emblemStatus;
				ClanTable.Instance.updateClan(clan);

				foreach (L1PcInstance member in clan.OnlineClanMember)
				{
					member.sendPackets(new S_PacketBox(S_PacketBox.PLEDGE_EMBLEM_STATUS, emblemStatus));
				}
			}
			else if (type == 0x30)
			{ // 村莊便利傳送
				int mapIndex = readH(); // 1: 亞丁 2:古魯丁 3: 奇岩
				int point = readH();
				int locx = 0;
				int locy = 0;
				L1PcInstance pc = client.ActiveChar;
				if (mapIndex == 1)
				{
					if (point == 0)
					{ // 亞丁-村莊北邊地區
						//X34079 Y33136 右下角 X 34090 Y 33150
						locx = 34079 + (int)(MathHelper.NextDouble * 12);
						locy = 33136 + (int)(MathHelper.NextDouble * 15);
					}
					else if (point == 1)
					{ // 亞丁-村莊中心地區
						//左上角 X 33970 Y 33243 右下角 X33979 Y33256 
						locx = 33970 + (int)(MathHelper.NextDouble * 10);
						locy = 33243 + (int)(MathHelper.NextDouble * 14);
					}
					else if (point == 2)
					{ // 亞丁-村莊教堂地區
						// 左上 X33925 Y33351 右下 X33938 Y33359
						locx = 33925 + (int)(MathHelper.NextDouble * 14);
						locy = 33351 + (int)(MathHelper.NextDouble * 9);
					}
				}
				else if (mapIndex == 2)
				{
					if (point == 0)
					{ // 古魯丁-北邊地區
						//左上 X32615 Y32719 右下 X32625 Y32725
						locx = 32615 + (int)(MathHelper.NextDouble * 11);
						locy = 32719 + (int)(MathHelper.NextDouble * 7);
					}
					else if (point == 1)
					{ // 古魯丁-南邊地區
						//左上 X32621 Y32788 右下 X32629 Y32800  
						locx = 32621 + (int)(MathHelper.NextDouble * 9);
						locy = 32788 + (int)(MathHelper.NextDouble * 13);
					}
				}
				else if (mapIndex == 3)
				{
					if (point == 0)
					{ // 奇岩-北邊地區
						//左上 X33501 Y32765 右下 X33511 Y32773
						locx = 33501 + (int)(MathHelper.NextDouble * 11);
						locy = 32765 + (int)(MathHelper.NextDouble * 9);
					}
					else if (point == 1)
					{ // 奇岩-南邊地區
						//左上 X33440 Y32784 右下 X33450 Y32794 
						locx = 33440 + (int)(MathHelper.NextDouble * 11);
						locy = 32784 + (int)(MathHelper.NextDouble * 11);
					}
				}
				L1Teleport.teleport(pc, locx, locy, pc.MapId, pc.Heading, true);
				pc.sendPackets(new S_PacketBox(S_PacketBox.TOWN_TELEPORT, pc));
			}
			else if (type == 0x32)
			{

			}
		}

		public override string Type
		{
			get
			{
				return C_SEND_LOCATION;
			}
		}
	}

}