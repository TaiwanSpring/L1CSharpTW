using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來的查詢PK次數封包
	/// </summary>
	class C_CheckPK : ClientBasePacket
	{

		private const string C_CHECK_PK = "[C] C_CheckPK";

		public C_CheckPK(sbyte[] abyte0, ClientThread clientthread) : base(abyte0)
		{

			L1PcInstance player = clientthread.ActiveChar;
			if (player == null)
			{
				return;
			}
			player.sendPackets(new S_ServerMessage(562, player.get_PKcount().ToString())); // 你的PK次數為%0次。
		}

		public override string Type
		{
			get
			{
				return C_CHECK_PK;
			}
		}

	}

}