/// <summary>
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
namespace LineageServer.Serverpackets
{

	using Opcodes = LineageServer.Server.Opcodes;
	using L1Character = LineageServer.Server.Model.L1Character;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_UseArrowSkill : ServerBasePacket
	{

		private const string S_USE_ARROW_SKILL = "[S] S_UseArrowSkill";

		private static AtomicInteger _sequentialNumber = new AtomicInteger(0);

		private byte[] _byte = null;

		public S_UseArrowSkill(L1Character cha, int targetobj, int x, int y, int[] data)
		{ // data = {actid, dmg, spellgfx}
			WriteC(Opcodes.S_OPCODE_ATTACKPACKET);
			WriteC(data[0]); // actid
			WriteD(cha.Id);
			WriteD(targetobj);
			WriteH(data[1]); // dmg
			WriteC(cha.Heading);
			WriteD(_sequentialNumber.incrementAndGet());
			WriteH(data[2]); // spellgfx
			WriteC(0); // use_type 箭
			WriteH(cha.X);
			WriteH(cha.Y);
			WriteH(x);
			WriteH(y);
			WriteC(0);
			WriteC(0);
			WriteC(0); // 0:none 2:爪痕 4:雙擊 8:鏡返射
		}

		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = memoryStream.toByteArray();
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