using LineageServer.Server.Server.Model.Gametime;
using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.serverpackets
{
    class S_OwnCharStatus : ServerBasePacket
    {
        private const string S_OWB_CHAR_STATUS = "[S] S_OwnCharStatus";

        public S_OwnCharStatus(L1PcInstance pc)
        {
            int time = L1GameTimeClock.Instance.currentTime().Seconds;
            time = time - (time % 300);
            writeC(Opcodes.S_OPCODE_OWNCHARSTATUS);
            writeD(pc.Id);
            if (pc.Level < 1)
            {
                writeC(1);
            }
            else if (pc.Level > 127)
            {
                writeC(127);
            }
            else
            {
                writeC(pc.Level);
            }
            writeExp(pc.Exp);
            writeC(pc.Str);
            writeC(pc.Int);
            writeC(pc.Wis);
            writeC(pc.Dex);
            writeC(pc.Con);
            writeC(pc.Cha);
            writeH(pc.CurrentHp);
            writeH(pc.MaxHp);
            writeH(pc.CurrentMp);
            writeH(pc.MaxMp);
            writeC(pc.Ac);
            writeD(time);
            writeC(pc.get_food());
            writeC(pc.Inventory.Weight242);
            writeH(pc.Lawful);
            writeH(pc.Fire);
            writeH(pc.Water);
            writeH(pc.Wind);
            writeH(pc.Earth);
            writeD(pc.MonsKill); // 狩獵數量
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