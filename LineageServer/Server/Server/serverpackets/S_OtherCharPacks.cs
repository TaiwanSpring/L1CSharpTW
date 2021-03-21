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
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_THIRD_SPEED;

	using Opcodes = LineageServer.Server.Server.Opcodes;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket, S_OtherCharPacks

	public class S_OtherCharPacks : ServerBasePacket
	{

		private const string S_OTHER_CHAR_PACKS = "[S] S_OtherCharPacks";

		private const int STATUS_POISON = 1;

		private const int STATUS_INVISIBLE = 2;

		private const int STATUS_PC = 4;

		private byte[] _byte = null;

		public S_OtherCharPacks(L1PcInstance pc, bool isFindInvis)
		{
			buildPacket(pc, isFindInvis);
		}

		public S_OtherCharPacks(L1PcInstance pc)
		{
			buildPacket(pc, false);
		}

		private void buildPacket(L1PcInstance pc, bool isFindInvis)
		{
			int status = STATUS_PC;

			if (pc.Poison != null)
			{ // 毒状態
				if (pc.Poison.EffectId == 1)
				{
					status |= STATUS_POISON;
				}
			}
			if (pc.Invisble && !isFindInvis)
			{
				status |= STATUS_INVISIBLE;
			}
			if (pc.BraveSpeed != 0)
			{ // 2段加速效果
				status |= pc.BraveSpeed * 16;
			}

			// int addbyte = 0;
			// int addbyte1 = 1;

			writeC(Opcodes.S_OPCODE_CHARPACK);
			writeH(pc.X);
			writeH(pc.Y);
			writeD(pc.Id);
			if (pc.Dead)
			{
				writeH(pc.TempCharGfxAtDead);
			}
			else
			{
				writeH(pc.TempCharGfx);
			}
			if (pc.Dead)
			{
				writeC(pc.Status);
			}
			else
			{
				writeC(pc.CurrentWeapon);
			}
			writeC(pc.Heading);
			// writeC(0); // makes char invis (0x01), cannot move. spells display
			writeC(pc.ChaLightSize);
			writeC(pc.MoveSpeed);
			writeD(1); // exp
			// writeC(0x00);
			writeH(pc.Lawful);
			writeS(pc.Name);
			writeS(pc.Title);
			writeC(status);
			writeD(pc.Clanid > 0 ? pc.Clan.EmblemId : 0); // 盟徽編號
			writeS(pc.Clanname); // クラン名
			writeS(null); // ペッホチング？
			writeC(0); // ？
			/*
			 * if(pc.is_isInParty()) // パーティー中 { writeC(100 * pc.get_currentHp() /
			 * pc.get_maxHp()); } else { writeC(0xFF); }
			 */

			writeC(0xFF);
			if (pc.hasSkillEffect(L1SkillId.STATUS_THIRD_SPEED))
			{
				writeC(0x08); // 3段加速
			}
			else
			{
				writeC(0);
			}
			writeC(0); // PC = 0, Mon = Lv
			if (pc.PrivateShop)
			{

			}
			else
			{
				writeS(null);
			}
			writeC(0xFF);
			writeC(0xFF);
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
				return S_OTHER_CHAR_PACKS;
			}
		}

	}
}