using LineageServer.Server;
using LineageServer.Server.Model.Instance;
using LineageServer.Utils;

namespace LineageServer.Serverpackets
{
    class S_HPUpdate : ServerBasePacket
    {
        private static readonly IntRange hpRange = new IntRange(1, 32767);

        public S_HPUpdate(int currentHp, int maxHp)
        {
            buildPacket(currentHp, maxHp);
        }

        public S_HPUpdate(L1PcInstance pc)
        {
            buildPacket(pc.CurrentHp, pc.MaxHp);
        }

        public virtual void buildPacket(int currentHp, int maxHp)
        {
            WriteC(Opcodes.S_OPCODE_HPUPDATE);
            WriteH(hpRange.ensure(currentHp));
            WriteH(hpRange.ensure(maxHp));
            // WriteC(0);
            // WriteD(GameTimeController.getInstance().getGameTime());
        }
    }
}