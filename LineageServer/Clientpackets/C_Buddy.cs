using LineageServer.Server.DataSources;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;

namespace LineageServer.Clientpackets
{
	/// <summary>
	/// 收到由客戶端傳來取得好友名單的封包
	/// </summary>
	class C_Buddy : ClientBasePacket
	{

		private const string C_BUDDY = "[C] C_Buddy";

		public C_Buddy(byte[] abyte0, ClientThread clientthread) : base(abyte0)
		{

			L1PcInstance pc = clientthread.ActiveChar;
			if (pc == null)
			{
				return;
			}
			L1Buddy buddy = BuddyTable.Instance.getBuddyTable(pc.Id);
			pc.sendPackets(new S_Buddy(pc.Id, buddy));
		}

		public override string Type
		{
			get
			{
				return C_BUDDY;
			}
		}
	}

}