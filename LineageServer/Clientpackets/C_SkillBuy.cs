using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來買魔法的封包
    /// </summary>
    class C_SkillBuy : ClientBasePacket
    {

        private const string C_SKILL_BUY = "[C] C_SkillBuy";
        public C_SkillBuy(byte[] abyte0, ClientThread clientthread) : base(abyte0)
        {
            L1PcInstance pc = clientthread.ActiveChar;
            if ((pc == null) || pc.Ghost)
            {
                return;
            }

            int i = ReadD();
            pc.sendPackets(new S_SkillBuy(i, pc));
            /*
			 * int type = player.get_type(); int lvl = player.get_level();
			 * 
			 * switch(type) { case 0: // 君主 if(lvl >= 10) { player.sendPackets(new
			 * S_SkillBuy(i, player)); } break;
			 * 
			 * case 1: // ナイト if(lvl >= 50) { player.sendPackets(new S_SkillBuy(i,
			 * player)); } break;
			 * 
			 * case 2: // エルフ if(lvl >= 8) { player.sendPackets(new S_SkillBuy(i,
			 * player)); } break;
			 * 
			 * case 3: // WIZ if(lvl >= 4) { player.sendPackets(new S_SkillBuy(i,
			 * player)); } break;
			 * 
			 * case 4: //DE if(lvl >= 12) { player.sendPackets(new S_SkillBuy(i,
			 * player)); } break;
			 * 
			 * default: break; }
			 */
        }

        public override string Type
        {
            get
            {
                return C_SKILL_BUY;
            }
        }

    }

}