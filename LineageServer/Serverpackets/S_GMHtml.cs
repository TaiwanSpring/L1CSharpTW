using LineageServer.Server;

namespace LineageServer.Serverpackets
{
	class S_GMHtml : ServerBasePacket
	{
		public S_GMHtml(int _objid, string html)
		{
			WriteC(Opcodes.S_OPCODE_SHOWHTML);
			WriteD(_objid);
			WriteS("hsiw");
			WriteS(html);
		}
	}
}