using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Utils;

namespace LineageServer.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來決鬥的封包
	/// </summary>
	class C_Fight : ClientBasePacket
	{

		private const string C_FIGHT = "[C] C_Fight";
		public C_Fight(byte[] abyte0, ClientThread client) : base(abyte0)
		{

			L1PcInstance pc = client.ActiveChar;
			if ((pc == null) || pc.Ghost)
			{
				return;
			}
			L1PcInstance target = FaceToFace.faceToFace(pc, false);
			if (target != null)
			{
				if (!target.Paralyzed)
				{
					if (pc.FightId != 0)
					{
						pc.sendPackets(new S_ServerMessage(633)); // \f1你已經與其他人決鬥中。
						return;
					}
					else if (target.FightId != 0)
					{
						target.sendPackets(new S_ServerMessage(634)); // \f11對方已經與其他人決鬥中。
						return;
					}
					pc.FightId = target.Id;
					target.FightId = pc.Id;
					target.sendPackets(new S_Message_YN(630, pc.Name)); // 要與你決鬥。你是否同意？(Y/N)
				}
			}
		}

		public override string Type
		{
			get
			{
				return C_FIGHT;
			}
		}

	}

}