using LineageServer.Server;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;

namespace LineageServer.Serverpackets
{
    class S_OtherCharPacks : ServerBasePacket
    {

        private const string S_OTHER_CHAR_PACKS = "[S] S_OtherCharPacks";

        private const int STATUS_POISON = 1;

        private const int STATUS_INVISIBLE = 2;

        private const int STATUS_PC = 4;

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

            WriteC(Opcodes.S_OPCODE_CHARPACK);
            WriteH(pc.X);
            WriteH(pc.Y);
            WriteD(pc.Id);
            if (pc.Dead)
            {
                WriteH(pc.TempCharGfxAtDead);
            }
            else
            {
                WriteH(pc.TempCharGfx);
            }
            if (pc.Dead)
            {
                WriteC(pc.Status);
            }
            else
            {
                WriteC(pc.CurrentWeapon);
            }
            WriteC(pc.Heading);
            // WriteC(0); // makes char invis (0x01), cannot move. spells display
            WriteC(pc.ChaLightSize);
            WriteC(pc.MoveSpeed);
            WriteD(1); // exp
                       // WriteC(0x00);
            WriteH(pc.Lawful);
            WriteS(pc.Name);
            WriteS(pc.Title);
            WriteC(status);
            WriteD(pc.Clanid > 0 ? pc.Clan.EmblemId : 0); // 盟徽編號
            WriteS(pc.Clanname); // クラン名
            WriteS(null); // ペッホチング？
            WriteC(0); // ？
            /*
			 * if(pc.is_isInParty()) // パーティー中 { WriteC(100 * pc.get_currentHp() /
			 * pc.get_maxHp()); } else { WriteC(0xFF); }
			 */

            WriteC(0xFF);
            if (pc.hasSkillEffect(L1SkillId.STATUS_THIRD_SPEED))
            {
                WriteC(0x08); // 3段加速
            }
            else
            {
                WriteC(0);
            }
            WriteC(0); // PC = 0, Mon = Lv
            if (pc.PrivateShop)
            {

            }
            else
            {
                WriteS(null);
            }
            WriteC(0xFF);
            WriteC(0xFF);
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