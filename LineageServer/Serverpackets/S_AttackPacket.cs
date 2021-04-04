using LineageServer.Server;
using LineageServer.Server.Model;

namespace LineageServer.Serverpackets
{
	class S_AttackPacket : ServerBasePacket
	{
		private const string S_ATTACK_PACKET = "[S] S_AttackPacket";

		private byte[] _byte = null;

		public S_AttackPacket(L1Character atk, int objid, int[] data)
		{
			buildpacket(atk, objid, data);
		}

		public S_AttackPacket(L1Character atk, int objid, int actid)
		{
			int[] data = new int[] {actid, 0, 0};
			buildpacket(atk, objid, data);
		}

		private void buildpacket(L1Character atk, int objid, int[] data)
		{ // data = {actid, dmg, effect}
			WriteC(Opcodes.S_OPCODE_ATTACKPACKET);
			WriteC(data[0]); // actid
			WriteD(atk.Id);
			WriteD(objid);
			WriteH(data[1]); // dmg
			WriteC(atk.Heading);
			WriteD(0x00000000);
			WriteC(data[2]); // effect 0:none 2:爪痕 4:雙擊 8:鏡返射
		}

		public override string Type
		{
			get
			{
				return S_ATTACK_PACKET;
			}
		}
	}

}