using LineageServer.Server;
using LineageServer.Server.Model;

namespace LineageServer.Serverpackets
{
    class S_RangeSkill : ServerBasePacket
    {

        private const string S_RANGE_SKILL = "[S] S_RangeSkill";

        private byte[] _byte = null;

        public const int TYPE_NODIR = 0;

        public const int TYPE_DIR = 8;

        private static int i = 0;

        public S_RangeSkill(L1Character cha, L1Character[] target, int spellgfx, int actionId, int type)
        {
            buildPacket(cha, target, spellgfx, actionId, type);
        }

        private void buildPacket(L1Character cha, L1Character[] target, int spellgfx, int actionId, int type)
        {
            WriteC(Opcodes.S_OPCODE_RANGESKILLS);
            WriteC(actionId);
            WriteD(cha.Id);
            WriteH(cha.X);
            WriteH(cha.Y);
            if (type == TYPE_NODIR)
            {
                WriteC(cha.Heading);
            }
            else if (type == TYPE_DIR)
            {
                int newHeading = calcheading(cha.X, cha.Y, target[0].X, target[0].Y);
                cha.Heading = newHeading;
                WriteC(cha.Heading);
            }
            System.Threading.Interlocked.Increment(ref i);
            WriteD(i); // 番号がダブらないように送る。
            WriteH(spellgfx);
            WriteC(type); // 0:範囲 6:遠距離 8:範囲&遠距離
            WriteH(0);
            WriteH(target.Length);
            foreach (L1Character element in target)
            {
                WriteD(element.Id);
                WriteH(0x20); // 0:ダメージモーションあり 0以外:なし
            }
        }
        private static int calcheading(int myx, int myy, int tx, int ty)
        {
            int newheading = 0;
            if ((tx > myx) && (ty > myy))
            {
                newheading = 3;
            }
            if ((tx < myx) && (ty < myy))
            {
                newheading = 7;
            }
            if ((tx > myx) && (ty == myy))
            {
                newheading = 2;
            }
            if ((tx < myx) && (ty == myy))
            {
                newheading = 6;
            }
            if ((tx == myx) && (ty < myy))
            {
                newheading = 0;
            }
            if ((tx == myx) && (ty > myy))
            {
                newheading = 4;
            }
            if ((tx < myx) && (ty > myy))
            {
                newheading = 5;
            }
            if ((tx > myx) && (ty < myy))
            {
                newheading = 1;
            }
            return newheading;
        }

        public override string Type
        {
            get
            {
                return S_RANGE_SKILL;
            }
        }

    }
}