using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;

namespace LineageServer.Serverpackets
{
    class S_UseAttackSkill : ServerBasePacket
    {

        private const string S_USE_ATTACK_SKILL = "[S] S_UseAttackSkill";

        private static AtomicInteger _sequentialNumber = new AtomicInteger(0);

        private byte[] _byte = null;

        public S_UseAttackSkill(L1Character cha, int targetobj, int x, int y, int[] data)
        {
            buildPacket(cha, targetobj, x, y, data, true);
        }

        public S_UseAttackSkill(L1Character cha, int targetobj, int x, int y, int[] data, bool withCastMotion)
        {
            buildPacket(cha, targetobj, x, y, data, withCastMotion);
        }

        private void buildPacket(L1Character cha, int targetobj, int x, int y, int[] data, bool withCastMotion)
        {
            if (cha is L1PcInstance)
            {
                // シャドウ系変身中に攻撃魔法を使用するとクライアントが落ちるため暫定対応
                if (cha.hasSkillEffect(L1SkillId.SHAPE_CHANGE) && (data[0] == ActionCodes.ACTION_SkillAttack))
                {
                    int tempchargfx = cha.TempCharGfx;
                    if ((tempchargfx == 5727) || (tempchargfx == 5730))
                    {
                        data[0] = ActionCodes.ACTION_SkillBuff;
                    }
                    else if ((tempchargfx == 5733) || (tempchargfx == 5736))
                    {
                        // 補助魔法モーションにすると攻撃魔法のグラフィックと
                        // 対象へのダメージモーションが発生しなくなるため
                        // 攻撃モーションで代用
                        data[0] = ActionCodes.ACTION_Attack;
                    }
                }
            }
            // 火の精の主がデフォルトだと攻撃魔法のグラフィックが発生しないので強制置き換え
            // どこか別で管理した方が良い？
            if (cha.TempCharGfx == 4013)
            {
                data[0] = ActionCodes.ACTION_Attack;
            }

            int newheading = calcheading(cha.X, cha.Y, x, y);
            cha.Heading = newheading;
            WriteC(Opcodes.S_OPCODE_ATTACKPACKET);
            WriteC(data[0]); // actionId
            WriteD(withCastMotion ? cha.Id : 0);
            WriteD(targetobj);
            WriteH(data[1]); // dmg
            WriteC(newheading);
            WriteD(_sequentialNumber.incrementAndGet()); // 番号がダブらないように送る。
            WriteH(data[2]); // spellgfx
            WriteC(data[3]); // use_type 0:弓箭 6:遠距離魔法 8:遠距離範圍魔法
            WriteH(cha.X);
            WriteH(cha.Y);
            WriteH(x);
            WriteH(y);
            WriteC(0);
            WriteC(0);
            WriteC(0); // 0:none 2:爪痕 4:雙擊 8:鏡返射
        }

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
                    _byte[13] = unchecked((sbyte)(seq & 0xff));
                    _byte[14] = unchecked((sbyte)(seq >> 8 & 0xff));
                    _byte[15] = unchecked((sbyte)(seq >> 16 & 0xff));
                    _byte[16] = unchecked((sbyte)(seq >> 24 & 0xff));
                }

                return _byte;
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
                return S_USE_ATTACK_SKILL;
            }
        }

    }
}