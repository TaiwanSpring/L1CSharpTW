using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理客戶端傳來之血盟注視封包
	/// </summary>
	class C_ClanAttention : ClientBasePacket todo
	{
		private const string C_PledgeRecommendation = "[C] C_PledgeRecommendation";

		public C_ClanAttention(sbyte[] decrypt, ClientThread client) : base(decrypt)
		{

			L1PcInstance pc = client.ActiveChar;
			if (pc == null)
			{
				return;
			}

			int data = readC();

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