
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Utils;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來結婚的封包
	/// </summary>
	class C_Propose : ClientBasePacket
	{

		private const string C_PROPOSE = "[C] C_Propose";

		public C_Propose(byte[] abyte0, ClientThread clientthread) : base(abyte0)
		{

			L1PcInstance pc = clientthread.ActiveChar;
			if (pc == null)
			{
				return;
			}

			int c = ReadC();

			if (c == 0)
			{ // /propose（/結婚）
				if (pc.Ghost)
				{
					return;
				}
				L1PcInstance target = FaceToFace.faceToFace(pc, false);
				if (target != null)
				{
					if (pc.PartnerId != 0)
					{
						pc.sendPackets(new S_ServerMessage(657)); // \f1あなたはすでに結婚しています。
						return;
					}
					if (target.PartnerId != 0)
					{
						pc.sendPackets(new S_ServerMessage(658)); // \f1その相手はすでに結婚しています。
						return;
					}
					if (pc.get_sex() == target.get_sex())
					{
						pc.sendPackets(new S_ServerMessage(661)); // \f1結婚相手は異性でなければなりません。
						return;
					}
					if ((pc.X >= 33974) && (pc.X <= 33976) && (pc.Y >= 33362) && (pc.Y <= 33365) && (pc.MapId == 4) && (target.X >= 33974) && (target.X <= 33976) && (target.Y >= 33362) && (target.Y <= 33365) && (target.MapId == 4))
					{
						target.TempID = pc.Id; // 暫時儲存對象的角色ID
						target.sendPackets(new S_Message_YN(654, pc.Name)); // %0%sあなたと結婚したがっています。%0と結婚しますか？（Y/N）
					}
				}
			}
			else if (c == 1)
			{ // /divorce（/離婚）
				if (pc.PartnerId == 0)
				{
					pc.sendPackets(new S_ServerMessage(662)); // \f1あなたは結婚していません。
					return;
				}
				pc.sendPackets(new S_Message_YN(653, "")); // 離婚をするとリングは消えてしまいます。離婚を望みますか？（Y/N）
			}
		}

		public override string Type
		{
			get
			{
				return C_PROPOSE;
			}
		}
	}

}