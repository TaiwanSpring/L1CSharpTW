using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Model.skill;

namespace LineageServer.Server.Server.serverpackets
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

        public override string Type
        {
            get
            {
                return S_OTHER_CHAR_PACKS;
            }
        }

    }
}