using LineageServer.Server;

namespace LineageServer.Serverpackets
{
	class S_Liquor : ServerBasePacket
	{

		public S_Liquor(int objecId, int j)
		{
			WriteC(Opcodes.S_OPCODE_LIQUOR);
			WriteD(objecId);
			WriteC(j);
		}
		public override string Type
		{
			get
			{
				return "[S] S_Liquor";
			}
		}
	}

}