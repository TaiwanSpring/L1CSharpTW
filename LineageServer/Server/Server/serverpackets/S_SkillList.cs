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
namespace LineageServer.Server.Server.serverpackets
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.Opcodes.S_OPCODE_ADDSKILL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.Opcodes.S_OPCODE_DELSKILL;

	using L1Skills = LineageServer.Server.Server.Templates.L1Skills;

	public class S_SkillList : ServerBasePacket
	{
		private const string S_SKILL_LIST = "[S] S_SkillList";

		/*
		 * [Length:40] S -> C 0000 4C 20 FF FF 37 00 00 00 00 00 00 00 00 00 00 00 L
		 * ..7........... 0010 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
		 * ................ 0020 00 00 00 2E EF 67 33 87 .....g3.
		 */
		public S_SkillList(bool Insert, params L1Skills[] skills)
		{
			if (Insert)
			{
				WriteC(S_OPCODE_ADDSKILL);
			}
			else
			{
				WriteC(S_OPCODE_DELSKILL);
			}

			int[] SkillList = new int[0x20];

			WriteC(SkillList.Length);

			foreach (L1Skills skill in skills)
			{
				int level = skill.SkillLevel - 1;

				SkillList[level] |= skill.Id;
			}

			foreach (int i in SkillList)
			{
				WriteC(i);
			}

			WriteC(0x00); // 區分用的數值
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
				return S_SKILL_LIST;
			}
		}
	}

}