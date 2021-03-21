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
	// ServerBasePacket

	public class S_OwnCharPack : ServerBasePacket
	{

		private const string S_OWN_CHAR_PACK = "[S] S_OwnCharPack";

		private const int STATUS_INVISIBLE = 2;

		private const int STATUS_PC = 4;

		private const int STATUS_GHOST = 128;

		private byte[] _byte = null;

		public S_OwnCharPack(L1PcInstance pc)
		{
			buildPacket(pc);
		}

		private void buildPacket(L1PcInstance pc)
		{
			int status = STATUS_PC;

			// グール毒みたいな緑の毒
			// if (pc.isPoison()) {
			// status |= STATUS_POISON;
			// }

			if (pc.Invisble || pc.GmInvis)
			{
				status |= STATUS_INVISIBLE;
			}
			if (pc.BraveSpeed != 0)
			{ // 2段加速效果
				status |= pc.BraveSpeed * 16;
			}

			if (pc.Ghost)
			{
				status |= STATUS_GHOST;
			}

			writeC(Opcodes.S_OPCODE_CHARPACK);
			writeH(pc.X);
			writeH(pc.Y);
			writeD(pc.Id);
			writeH(pc.Dead ? pc.TempCharGfxAtDead : pc.TempCharGfx);
			writeC(pc.Dead ? pc.Status : pc.CurrentWeapon);
			writeC(pc.Heading);
			writeC(pc.OwnLightSize);
			writeC(pc.MoveSpeed);
			//writeD(pc.getExp());
			writeD(1);
			writeH(pc.Lawful);
			writeS(pc.Name);
			writeS(pc.Title);
			writeC(status);
			writeD(pc.Clanid > 0 ? pc.Clan.EmblemId : 0); // 盟徽編號
			writeS(pc.Clanname); // クラン名
			writeS(null); // ペッホチング？
			writeC(pc.ClanRank > 0 ? pc.ClanRank << 4 : 0xb0); // 階級  * 16
			if (pc.InParty) // パーティー中
			{
				writeC(100 * pc.CurrentHp / pc.MaxHp);
			}
			else
			{
				writeC(0xff);
			}
			if (pc.hasSkillEffect(L1SkillId.STATUS_THIRD_SPEED))
			{
				writeC(0x08); // 3段加速
			}
			else
			{
				writeC(0);
			}
			writeC(0); // PC = 0, Mon = Lv
			writeC(0); // ？
			writeC(0xff);
			writeC(0xff);
			writeS(null);
			writeC(0);
		}

		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = _bao.toByteArray();
				}
				return _byte;
			}
		}

		public override string Type
		{
			get
			{
				return S_OWN_CHAR_PACK;
			}
		}

	}
}