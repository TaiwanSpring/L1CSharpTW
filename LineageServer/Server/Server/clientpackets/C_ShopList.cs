using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來商店清單的封包
	/// </summary>
	class C_ShopList : ClientBasePacket
	{

		private const string C_SHOP_LIST = "[C] C_ShopList";

		public C_ShopList(sbyte[] abyte0, ClientThread clientthread) : base(abyte0)
		{

			L1PcInstance pc = clientthread.ActiveChar;
			if ((pc == null) || pc.Ghost)
			{
				return;
			}

			int type = readC();
			int objectId = readD();

			pc.sendPackets(new S_PrivateShop(pc, objectId, type));
		}

		public override string Type
		{
			get
			{
				return C_SHOP_LIST;
			}
		}

	}

}