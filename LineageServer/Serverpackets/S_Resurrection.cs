using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
	class S_Resurrection : ServerBasePacket
	{

		public S_Resurrection(L1PcInstance target, L1PcInstance use, int type)
		{
			WriteC(Opcodes.S_OPCODE_RESURRECTION);
			WriteD(target.Id);
			WriteC(type);
			WriteD(use.Id);
			WriteD(target.ClassId);
		}

		public override string Type
		{
			get
			{
				return "[S] S_Resurrection";
			}
		}
	}

}