namespace LineageServer.Server.Server.serverpackets
{
	using Opcodes = LineageServer.Server.Server.Opcodes;

	public class S_SkillIconThirdSpeed : ServerBasePacket
	{

		public S_SkillIconThirdSpeed(int j)
		{
			writeC(Opcodes.S_OPCODE_SKILLICONGFX);
			writeC(0x3c);
			writeC(j); // time / 4
			writeC(0x8);
		}

		public override sbyte[] Content
		{
			get
			{
				return Bytes;
			}
		}

		public override string Type
		{
			get
			{
				return "[S] S_SkillIconThirdSpeed";
			}
		}
	}

}