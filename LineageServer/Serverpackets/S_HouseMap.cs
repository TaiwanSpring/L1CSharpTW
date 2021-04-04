using LineageServer.Server;
using System;
namespace LineageServer.Serverpackets
{
	class S_HouseMap : ServerBasePacket
	{

		private const string S_HOUSEMAP = "[S] S_HouseMap";

		private byte[] _byte = null;

		public S_HouseMap(int objectId, string house_number)
		{
			buildPacket(objectId, house_number);
		}

		private void buildPacket(int objectId, string house_number)
		{
			int number = Convert.ToInt32(house_number);

			WriteC(Opcodes.S_OPCODE_HOUSEMAP);
			WriteD(objectId);
			WriteD(number);
		}
		public override string Type
		{
			get
			{
				return S_HOUSEMAP;
			}
		}
	}

}