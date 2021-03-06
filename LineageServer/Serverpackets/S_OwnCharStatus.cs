using LineageServer.Interfaces;
using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_OwnCharStatus : ServerBasePacket
    {
        private const string S_OWB_CHAR_STATUS = "[S] S_OwnCharStatus";

        public S_OwnCharStatus(L1PcInstance pc)
        {
            int time = Container.Instance.Resolve<IGameTimeClock>().CurrentTime().Seconds;
            time = time - (time % 300);
            WriteC(Opcodes.S_OPCODE_OWNCHARSTATUS);
            WriteD(pc.Id);
            if (pc.Level < 1)
            {
                WriteC(1);
            }
            else if (pc.Level > 127)
            {
                WriteC(127);
            }
            else
            {
                WriteC(pc.Level);
            }
            WriteExp(pc.Exp);
            WriteC(pc.getStr());
            WriteC(pc.getInt());
            WriteC(pc.getWis());
            WriteC(pc.getDex());
            WriteC(pc.getCon());
            WriteC(pc.getCha());
            WriteH(pc.CurrentHp);
            WriteH(pc.getMaxHp());
            WriteH(pc.CurrentMp);
            WriteH(pc.getMaxMp());
            WriteC(pc.Ac);
            WriteD(time);
            WriteC(pc.get_food());
            WriteC((pc.Inventory as L1PcInventory).Weight242);
            WriteH(pc.Lawful);
            WriteH(pc.Fire);
            WriteH(pc.Water);
            WriteH(pc.Wind);
            WriteH(pc.Earth);
            WriteD(pc.MonsKill); // 狩獵數量
        }

        public override string Type
        {
            get
            {
                return S_OWB_CHAR_STATUS;
            }
        }
    }
}