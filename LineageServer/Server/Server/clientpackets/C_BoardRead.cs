using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 收到由客戶端傳送讀取公告欄的封包
	/// </summary>
	class C_BoardRead : ClientBasePacket
	{

		private const string C_BOARD_READ = "[C] C_BoardRead";

		public C_BoardRead(sbyte[] decrypt, ClientThread client) : base(decrypt)
		{
			int objId = readD();
			int topicNumber = readD();
			L1Object obj = L1World.Instance.findObject(objId);
			L1BoardInstance board = (L1BoardInstance) obj;
			board.onActionRead(client.ActiveChar, topicNumber);
		}

		public override string Type
		{
			get
			{
				return C_BOARD_READ;
			}
		}

	}

}