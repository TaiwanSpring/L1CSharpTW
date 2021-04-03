using LineageServer.Server;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;

namespace LineageServer.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來商店清單的封包
	/// </summary>
	class C_ShopList : ClientBasePacket
	{

		private const string C_SHOP_LIST = "[C] C_ShopList";

		public C_ShopList(byte[] abyte0, ClientThread clientthread) : base(abyte0)
		{

			L1PcInstance pc = clientthread.ActiveChar;
			if ((pc == null) || pc.Ghost)
			{
				return;
			}

			int type = ReadC();
			int objectId = ReadD();

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