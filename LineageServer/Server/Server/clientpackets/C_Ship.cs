using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來搭船的封包
	/// </summary>
	class C_Ship : ClientBasePacket
	{

		private const string C_SHIP = "[C] C_Ship";

		public C_Ship(sbyte[] abyte0, ClientThread client) : base(abyte0)
		{

			L1PcInstance pc = client.ActiveChar;
			if (pc == null)
			{
				return;
			}

			int shipMapId = readH();
			int locX = readH();
			int locY = readH();
			int mapId = pc.MapId;

			if (mapId == 5)
			{ // Talking Island Ship to Aden Mainland
				pc.Inventory.consumeItem(40299, 1);
			}
			else if (mapId == 6)
			{ // Aden Mainland Ship to Talking Island
				pc.Inventory.consumeItem(40298, 1);
			}
			else if (mapId == 83)
			{ // Forgotten Island Ship to Aden Mainland
				pc.Inventory.consumeItem(40300, 1);
			}
			else if (mapId == 84)
			{ // Aden Mainland Ship to Forgotten Island
				pc.Inventory.consumeItem(40301, 1);
			}
			else if (mapId == 446)
			{ // Ship Hidden dock to Pirate island
				pc.Inventory.consumeItem(40303, 1);
			}
			else if (mapId == 447)
			{ // Ship Pirate island to Hidden dock
				pc.Inventory.consumeItem(40302, 1);
			}
			pc.sendPackets(new S_OwnCharPack(pc));
			L1Teleport.teleport(pc, locX, locY, (short) shipMapId, 0, false);
		}

		public override string Type
		{
			get
			{
				return C_SHIP;
			}
		}
	}

}