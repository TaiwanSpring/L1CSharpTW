using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Clientpackets
{
	/// <summary>
	/// 處理客戶端傳來之血盟注視封包
	/// </summary>
	class C_ClanAttention : ClientBasePacket //todo
	{
		private const string C_PledgeRecommendation = "[C] C_PledgeRecommendation";

		public C_ClanAttention(byte[] decrypt, ClientThread client) : base(decrypt)
		{

			L1PcInstance pc = client.ActiveChar;
			if (pc == null)
			{
				return;
			}

			int data = ReadC();

			if (data == 0)
			{ // 新增注視血盟
				//String clanName = readS();

			}
			else if (data == 2)
			{ // 查詢新增的注視名單

			}

		}

		public override string Type
		{
			get
			{
				return C_PledgeRecommendation;
			}
		}
	}

}