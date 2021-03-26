using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來下載盟徽請求的封包
	/// </summary>
	class C_EmblemDownload : ClientBasePacket
	{

		private const string C_EMBLEMDOWNLOAD = "[C] C_EmblemDownload";

		public C_EmblemDownload(byte[] abyte0, ClientThread clientthread) : base(abyte0)
		{
			int emblemId = ReadD();

			L1PcInstance pc = clientthread.ActiveChar;
			if (pc == null)
			{
				return;
			}
			pc.sendPackets(new S_Emblem(emblemId));
		}

		public override string Type
		{
			get
			{
				return C_EMBLEMDOWNLOAD;
			}
		}
	}

}