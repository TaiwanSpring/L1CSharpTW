using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_Strup : ServerBasePacket
    {

        public S_Strup(L1PcInstance pc, int type, int time)
        {
            WriteC(Opcodes.S_OPCODE_STRUP);
            WriteH(time);
            WriteC(pc.getStr());
            WriteC((pc.Inventory as L1PcInventory).Weight242);
            WriteC(type);
            WriteH(0);
        }
        public override string Type
        {
            get
            {
                return "[S] S_Strup";
            }
        }
    }

}