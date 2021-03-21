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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_WISDOM_POTION;
	using Opcodes = LineageServer.Server.Server.Opcodes;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;

	public class S_SPMR : ServerBasePacket
	{

//JAVA TO C# CONVERTER NOTE: Members cannot have the same name as their enclosing type:
		private const string S_SPMR_Conflict = "[S] S_S_SPMR";

		private byte[] _byte = null;

		/// <summary>
		/// 更新魔防Mr以及魔攻Sp </summary>
		public S_SPMR(L1PcInstance pc)
		{
			buildPacket(pc);
		}

		private void buildPacket(L1PcInstance pc)
		{
			writeC(Opcodes.S_OPCODE_SPMR);
			// ウィズダムポーションのSPはS_SkillBrave送信時に更新されるため差し引いておく
			if (pc.hasSkillEffect(L1SkillId.STATUS_WISDOM_POTION))
			{
				writeC(pc.Sp - pc.TrueSp - 2); // 装備増加したSP
			}
			else
			{
				writeC(pc.Sp - pc.TrueSp); // 装備増加したSP
			}
			writeH(pc.TrueMr - pc.BaseMr); // 装備や魔法で増加したMR
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
				return S_SPMR_Conflict;
			}
		}
	}

}