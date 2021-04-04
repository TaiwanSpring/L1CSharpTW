using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
	class S_Dexup : ServerBasePacket
	{

		public S_Dexup(L1PcInstance pc, int type, int time)
		{
			WriteC(Opcodes.S_OPCODE_DEXUP);
			WriteH(time);
			WriteC(pc.BaseDex);
			WriteC(type);
			WriteC(0);
			WriteC(0);
			WriteC(0);
		}

		public override string Type
		{
			get
			{
				return "[S] S_Dexup";
			}
		}
	}

}