using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    /// <summary>
    /// 由3.0的S_Unkown1改名
    /// 正式命名為LoginGame
    /// </summary>
    class S_LoginGame : ServerBasePacket
    {
        public S_LoginGame(L1PcInstance pc)
        {
            /*
			 * 【Server】 id:223 size:8 time:1134908599
			 *  0000:  df 03 c1 55 b5 6e d1 dc
			 */
            WriteC(Opcodes.S_OPCODE_LOGINTOGAME);
            WriteC(0x03); // 語系
            if (pc.Clanid > 0)
            {
                WriteD(pc.ClanMemberId);
            }
            else
            {
                WriteC(0x53);
                WriteC(0x01);
                WriteC(0x00);
                WriteC(0x8b);
            }
            WriteC(0x9c);
            WriteC(0x1f);
        }
    }

}