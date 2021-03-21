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
	using L1Character = LineageServer.Server.Server.Model.L1Character;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_UseArrowSkill : ServerBasePacket
	{

		private const string S_USE_ARROW_SKILL = "[S] S_UseArrowSkill";

		private static AtomicInteger _sequentialNumber = new AtomicInteger(0);

		private byte[] _byte = null;

		public S_UseArrowSkill(L1Character cha, int targetobj, int x, int y, int[] data)
		{ // data = {actid, dmg, spellgfx}
			writeC(Opcodes.S_OPCODE_ATTACKPACKET);
			writeC(data[0]); // actid
			writeD(cha.Id);
			writeD(targetobj);
			writeH(data[1]); // dmg
			writeC(cha.Heading);
			writeD(_sequentialNumber.incrementAndGet());
			writeH(data[2]); // spellgfx
			writeC(0); // use_type 箭
			writeH(cha.X);
			writeH(cha.Y);
			writeH(x);
			writeH(y);
			writeC(0);
			writeC(0);
			writeC(0); // 0:none 2:爪痕 4:雙擊 8:鏡返射
		}

		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = _bao.toByteArray();
				}
				else
				{
					int seq = 0;
					lock (this)
					{
						seq = _sequentialNumber.incrementAndGet();
					}
					_byte[13] = unchecked((sbyte)(seq & 0xff));
					_byte[14] = unchecked((sbyte)(seq >> 8 & 0xff));
					_byte[15] = unchecked((sbyte)(seq >> 16 & 0xff));
					_byte[16] = unchecked((sbyte)(seq >> 24 & 0xff));
				}
				return _byte;
			}
		}

		public override string Type
		{
			get
			{
				return S_USE_ARROW_SKILL;
			}
		}

	}

}