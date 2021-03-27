
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;

namespace LineageServer.Serverpackets
{
	class S_OwnCharPack : ServerBasePacket
	{

		private const string S_OWN_CHAR_PACK = "[S] S_OwnCharPack";

		private const int STATUS_INVISIBLE = 2;

		private const int STATUS_PC = 4;

		private const int STATUS_GHOST = 128;

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

			WriteC(Opcodes.S_OPCODE_CHARPACK);
			WriteH(pc.X);
			WriteH(pc.Y);
			WriteD(pc.Id);
			WriteH(pc.Dead ? pc.TempCharGfxAtDead : pc.TempCharGfx);
			WriteC(pc.Dead ? pc.Status : pc.CurrentWeapon);
			WriteC(pc.Heading);
			WriteC(pc.OwnLightSize);
			WriteC(pc.MoveSpeed);
			//WriteD(pc.getExp());
			WriteD(1);
			WriteH(pc.Lawful);
			WriteS(pc.Name);
			WriteS(pc.Title);
			WriteC(status);
			WriteD(pc.Clanid > 0 ? pc.Clan.EmblemId : 0); // 盟徽編號
			WriteS(pc.Clanname); // クラン名
			WriteS(null); // ペッホチング？
			WriteC(pc.ClanRank > 0 ? pc.ClanRank << 4 : 0xb0); // 階級  * 16
			if (pc.InParty) // パーティー中
			{
				WriteC(100 * pc.CurrentHp / pc.MaxHp);
			}
			else
			{
				WriteC(0xff);
			}
			if (pc.hasSkillEffect(L1SkillId.STATUS_THIRD_SPEED))
			{
				WriteC(0x08); // 3段加速
			}
			else
			{
				WriteC(0);
			}
			WriteC(0); // PC = 0, Mon = Lv
			WriteC(0); // ？
			WriteC(0xff);
			WriteC(0xff);
			WriteS(null);
			WriteC(0);
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