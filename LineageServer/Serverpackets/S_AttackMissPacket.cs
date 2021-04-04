using LineageServer.Server;
using LineageServer.Server.Model;

namespace LineageServer.Serverpackets
{
	class S_AttackMissPacket : ServerBasePacket
	{

		private const string S__ATTACKMISSPACKET = "[S] S_AttackMissPacket";

		private byte[] _byte = null;

		public S_AttackMissPacket(L1Character attacker, int targetId)
		{
			WriteC(Opcodes.S_OPCODE_ATTACKPACKET);
			WriteC(1);
			WriteD(attacker.Id);
			WriteD(targetId);
			WriteH(0);
			WriteC(attacker.Heading);
			WriteD(0);
			WriteC(0);
		}

		public S_AttackMissPacket(L1Character attacker, int targetId, int actId)
		{
			WriteC(Opcodes.S_OPCODE_ATTACKPACKET);
			WriteC(actId);
			WriteD(attacker.Id);
			WriteD(targetId);
			WriteH(0);
			WriteC(attacker.Heading);
			WriteD(0);
			WriteC(0);
		}

		public S_AttackMissPacket(int attackId, int targetId)
		{
			WriteC(Opcodes.S_OPCODE_ATTACKPACKET);
			WriteC(1);
			WriteD(attackId);
			WriteD(targetId);
			WriteH(0);
			WriteC(0);
			WriteD(0);
		}

		public S_AttackMissPacket(int attackId, int targetId, int actId)
		{
			WriteC(Opcodes.S_OPCODE_ATTACKPACKET);
			WriteC(actId);
			WriteD(attackId);
			WriteD(targetId);
			WriteH(0);
			WriteC(0);
			WriteD(0);
		}
		public override string Type
		{
			get
			{
				return S__ATTACKMISSPACKET;
			}
		}
	}

}