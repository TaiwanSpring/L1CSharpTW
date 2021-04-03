using LineageServer.Server;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;

namespace LineageServer.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來停止釣魚的封包
	/// </summary>
	class C_FishClick : ClientBasePacket
	{

		private const string C_FISHCLICK = "[C] C_FishClick";
		public C_FishClick(byte[] abyte0, ClientThread clientthread) : base(abyte0)
		{
			L1PcInstance pc = clientthread.ActiveChar;
			if (pc == null || pc.Dead)
			{
				return;
			}
			pc.FishingTime = 0;
			pc.FishingReady = false;
			pc.Fishing = false;
			pc.sendPackets(new S_CharVisualUpdate(pc));
			pc.broadcastPacket(new S_CharVisualUpdate(pc));
			FishingTimeController.Instance.removeMember(pc);
		}

		public override string Type
		{
			get
			{
				return C_FISHCLICK;
			}
		}

	}

}