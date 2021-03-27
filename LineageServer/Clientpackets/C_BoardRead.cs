using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Clientpackets
{
	/// <summary>
	/// 收到由客戶端傳送讀取公告欄的封包
	/// </summary>
	class C_BoardRead : ClientBasePacket
	{

		private const string C_BOARD_READ = "[C] C_BoardRead";

		public C_BoardRead(byte[] decrypt, ClientThread client) : base(decrypt)
		{
			int objId = ReadD();
			int topicNumber = ReadD();
			GameObject obj = L1World.Instance.findObject(objId);
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