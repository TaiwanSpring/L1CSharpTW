using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Clientpackets
{
	/// <summary>
	/// 收到由客戶端傳來呼叫玩家的封包
	/// </summary>
	class C_CallPlayer : ClientBasePacket
	{

		private const string C_CALL = "[C] C_Call";

		public C_CallPlayer(byte[] decrypt, ClientThread client) : base(decrypt)
		{

			L1PcInstance pc = client.ActiveChar;
			if ((pc == null) || (!pc.Gm))
			{
				return;
			}

			string name = ReadS();
			if (name.Length == 0)
			{
				return;
			}

			L1PcInstance target = L1World.Instance.getPlayer(name);

			if (target == null)
			{
				return;
			}

			L1Location loc = L1Location.randomLocation(target.Location, 1, 2, false);
			L1Teleport.teleport(pc, loc.X, loc.Y, target.MapId, pc.Heading, false);
		}

		public override string Type
		{
			get
			{
				return C_CALL;
			}
		}
	}

}