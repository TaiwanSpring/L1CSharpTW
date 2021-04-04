using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_SkillIconGFX : ServerBasePacket
    {

        public S_SkillIconGFX(int i, int j)
        {
            WriteC(Opcodes.S_OPCODE_SKILLICONGFX);
            WriteC(i);
            WriteH(j);
        }

        /// <summary>
        /// 『來源:伺服器』<位址:250>{長度:8}(時間:993003001)
        ///  0000:  fa a0 01 39 00 02 30 27      ...9..0'
        /// </summary>
        public S_SkillIconGFX(int i)
        {
            WriteC(Opcodes.S_OPCODE_SKILLICONGFX);
            WriteC(0xa0);
            WriteC(1);
            WriteH(0);
            WriteC(2);
            WriteH(i);
        }
    }

}