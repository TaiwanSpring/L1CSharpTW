using LineageServer.Server;

namespace LineageServer.Serverpackets
{
	class S_Invis : ServerBasePacket
	{

		private byte[] _byte = null;

		public S_Invis(int objid, int type)
		{
			buildPacket(objid, type);
		}

		private void buildPacket(int objid, int type)
		{
			WriteC(Opcodes.S_OPCODE_INVIS);
			WriteD(objid);
			WriteC(type);
		}

		public override string Type
		{
			get
			{
				return S_INVIS;
			}
		}

		private const string S_INVIS = "[S] S_Invis";
	}

}