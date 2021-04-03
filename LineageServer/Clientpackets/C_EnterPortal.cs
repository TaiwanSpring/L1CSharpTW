using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來進入傳點的封包
	/// </summary>
	class C_EnterPortal : ClientBasePacket
	{

		private const string C_ENTER_PORTAL = "[C] C_EnterPortal";
		public C_EnterPortal(byte[] abyte0, ClientThread client) : base(abyte0)
		{
			L1PcInstance pc = client.ActiveChar;
			if (pc == null)
			{
				return;
			}

			int locx = ReadH();
			int locy = ReadH();

			if (pc.Teleport)
			{ // 傳送中
				return;
			}
			// 取得傳送的點
			DungeonController.Instance.dg(locx, locy, pc.Map.Id, pc);
		}

		public override string Type
		{
			get
			{
				return C_ENTER_PORTAL;
			}
		}
	}

}