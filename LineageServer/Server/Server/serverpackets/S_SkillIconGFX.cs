﻿/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Server.serverpackets
{
	using Opcodes = LineageServer.Server.Server.Opcodes;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_SkillIconGFX : ServerBasePacket
	{

		public S_SkillIconGFX(int i, int j)
		{
			WriteC(Opcodes.S_OPCODE_SKILLICONGFX);
			WriteC(i);
			WriteH(j);
		}

		/// <summary>
		/// 『來源:伺服器』<位址:250>{長度:8}(時間:993003001)
		///  0000:  fa a0 01 39 00 02 30 27      ...9..0'
		/// </summary>
		public S_SkillIconGFX(int i)
		{
			WriteC(Opcodes.S_OPCODE_SKILLICONGFX);
			WriteC(0xa0);
			WriteC(1);
			WriteH(0);
			WriteC(2);
			WriteH(i);
		}

		public override sbyte[] Content
		{
			get
			{
				return Bytes;
			}
		}
	}

}