using LineageServer.Server;
using LineageServer.Server.Model.Instance;
using System;
namespace LineageServer.Serverpackets
{
    class S_SkillBuy : ServerBasePacket
    {
        private const string _S_SKILL_BUY = "[S] S_SkillBuy";

        private byte[] _byte = null;

        public S_SkillBuy(int o, L1PcInstance pc)
        {
            int count = Scount(pc);
            int inCount = 0;
            for (int k = 0; k < count; k++)
            {
                if (!pc.isSkillMastery((k + 1)))
                {
                    inCount++;
                }
            }

            try
            {
                WriteC(Opcodes.S_OPCODE_SKILLBUY);
                WriteD(100);
                WriteH(inCount);
                for (int k = 0; k < count; k++)
                {
                    if (!pc.isSkillMastery((k + 1)))
                    {
                        WriteD(k);
                    }
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
        }

        public virtual int Scount(L1PcInstance pc)
        {
            int RC = 0;
            switch (pc.Type)
            {
                case 0: // 君主
                    if (pc.Level > 20 || pc.Gm)
                    {
                        RC = 16;
                    }
                    else if (pc.Level > 10)
                    {
                        RC = 8;
                    }
                    break;

                case 1: // ナイト
                    if (pc.Level >= 50 || pc.Gm)
                    {
                        RC = 8;
                    }
                    break;

                case 2: // エルフ
                    if (pc.Level >= 24 || pc.Gm)
                    {
                        RC = 23;
                    }
                    else if (pc.Level >= 16)
                    {
                        RC = 16;
                    }
                    else if (pc.Level >= 8)
                    {
                        RC = 8;
                    }
                    break;

                case 3: // WIZ
                    if (pc.Level >= 12 || pc.Gm)
                    {
                        RC = 23;
                    }
                    else if (pc.Level >= 8)
                    {
                        RC = 16;
                    }
                    else if (pc.Level >= 4)
                    {
                        RC = 8;
                    }
                    break;

                case 4: // DE
                    if (pc.Level >= 24 || pc.Gm)
                    {
                        RC = 16;
                    }
                    else if (pc.Level >= 12)
                    {
                        RC = 8;
                    }
                    break;

                default:
                    break;
            }
            return RC;
        }

        public override string Type
        {
            get
            {
                return _S_SKILL_BUY;
            }
        }

    }

}