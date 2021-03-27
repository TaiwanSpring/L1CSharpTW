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
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_CharReset : ServerBasePacket
	{

		private const string S_CHAR_RESET = "[S] S_CharReset";

		private byte[] _byte = null;

		/// <summary>
		/// 重置升級能力更新 [Server] opcode = 43 0000: 2b /02/ 01 2d/ 0f 00/ 04 00/ 0a 00
		/// /0c 0c 0c 0c 12 09 +..-............
		/// </summary>
		public S_CharReset(L1PcInstance pc, int lv, int hp, int mp, int ac, int str, int intel, int wis, int dex, int con, int cha)
		{
			WriteC(Opcodes.S_OPCODE_CHARRESET);
			WriteC(0x02);
			WriteC(lv);
			WriteC(pc.TempMaxLevel); // max lv
			WriteH(hp);
			WriteH(mp);
			WriteH(ac);
			WriteC(str);
			WriteC(intel);
			WriteC(wis);
			WriteC(dex);
			WriteC(con);
			WriteC(cha);
		}

		public S_CharReset(int point)
		{
			WriteC(Opcodes.S_OPCODE_CHARRESET);
			WriteC(0x03);
			WriteC(point);
		}

		/// <summary>
		/// 『來源:伺服器』<位址:64>{長度:8}(時間:1233632532)<br>
		///  0000:  40 01 10 00 01 00 0a 34                     @......4<br>
		///  52等騎士重置
		/// </summary>
		public S_CharReset(L1PcInstance pc)
		{
			WriteC(Opcodes.S_OPCODE_CHARRESET);
			WriteC(0x01);
			if (pc.Crown)
			{
				WriteH(14);
				WriteH(2);
			}
			else if (pc.Knight)
			{
				WriteH(16);
				WriteH(1);
			}
			else if (pc.Elf)
			{
				WriteH(15);
				WriteH(4);
			}
			else if (pc.Wizard)
			{
				WriteH(12);
				WriteH(6);
			}
			else if (pc.Darkelf)
			{
				WriteH(12);
				WriteH(3);
			}
			else if (pc.DragonKnight)
			{
				WriteH(15);
				WriteH(4);
			}
			else if (pc.Illusionist)
			{
				WriteH(15);
				WriteH(4);
			}
			WriteC(0x0a); // AC
			WriteC(pc.TempMaxLevel); // Lv
		}

		/// <summary>
		///  給予角色盟徽編號</br>
		/// 『來源:伺服器』<位址:64>{長度:16}(時間:1607823495)</br>
		///       0000:  40 3c 15 ea 7a 00 33 b6 00 00 6a 6c cb 92 b5 2d    @<..z.3...jl...-
		/// </summary>
		public S_CharReset(int pcObjId, int emblemId)
		{
			WriteC(Opcodes.S_OPCODE_CHARRESET);
			WriteC(0x3c);
			WriteD(pcObjId);
			WriteD(emblemId);
		}

		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = Bytes;
				}
				return _byte;
			}
		}

		public override string Type
		{
			get
			{
				return S_CHAR_RESET;
			}
		}
	}

}