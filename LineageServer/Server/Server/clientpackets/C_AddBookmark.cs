using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Templates;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理收到客戶端傳來新增書籤的封包
	/// </summary>
	class C_AddBookmark : ClientBasePacket
	{

		private const string C_ADD_BOOKMARK = "[C] C_AddBookmark";

		public C_AddBookmark(byte[] decrypt, ClientThread client) : base(decrypt)
		{

			L1PcInstance pc = client.ActiveChar;
			if ((pc == null) || pc.Ghost)
			{
				return;
			}

			string s = ReadS();

			if (pc.Map.Markable || pc.Gm)
			{
				if ((L1CastleLocation.checkInAllWarArea(pc.X, pc.Y, pc.MapId) || L1HouseLocation.isInHouse(pc.X, pc.Y, pc.MapId)) && !pc.Gm)
				{
					// \f1ここを記憶することができません。
					pc.sendPackets(new S_ServerMessage(214));
				}
				else
				{
					L1BookMark.addBookmark(pc, s);
				}
			}
			else
			{
				// \f1ここを記憶することができません。
				pc.sendPackets(new S_ServerMessage(214));
			}
		}

		public override string Type
		{
			get
			{
				return C_ADD_BOOKMARK;
			}
		}
	}

}