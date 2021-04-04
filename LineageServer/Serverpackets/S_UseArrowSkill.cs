using LineageServer.Server;
using LineageServer.Server.Model;

namespace LineageServer.Serverpackets
{
    class S_UseArrowSkill : ServerBasePacket
    {

        private const string S_USE_ARROW_SKILL = "[S] S_UseArrowSkill";

        private byte[] _byte = null;

        private static int i;

        public S_UseArrowSkill(L1Character cha, int targetobj, int x, int y, int[] data)
        {
            // data = {actid, dmg, spellgfx}
            WriteC(Opcodes.S_OPCODE_ATTACKPACKET);
            WriteC(data[0]); // actid
            WriteD(cha.Id);
            WriteD(targetobj);
            WriteH(data[1]); // dmg
            WriteC(cha.Heading);
            System.Threading.Interlocked.Increment(ref i);
            WriteD(i);
            WriteH(data[2]); // spellgfx
            WriteC(0); // use_type 箭
            WriteH(cha.X);
            WriteH(cha.Y);
            WriteH(x);
            WriteH(y);
            WriteC(0);
            WriteC(0);
            WriteC(0); // 0:none 2:爪痕 4:雙擊 8:鏡返射
        }
        /*
        public override sbyte[] Content
        {
            get
            {
                if (_byte == null)
                {
                    _byte = memoryStream.toByteArray();
                }
                else
                {
                    int seq = 0;
                    lock (this)
                    {
                        seq = _sequentialNumber.incrementAndGet();
                    }
                    _byte[13] = unchecked((seq & 0xff));
                    _byte[14] = unchecked((seq >> 8 & 0xff));
                    _byte[15] = unchecked((seq >> 16 & 0xff));
                    _byte[16] = unchecked((seq >> 24 & 0xff));
                }
                return _byte;
            }
        }
        */
        public override string Type
        {
            get
            {
                return S_USE_ARROW_SKILL;
            }
        }

    }

}