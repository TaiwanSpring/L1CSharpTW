using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_Trap : ServerBasePacket
    {
        public S_Trap(L1TrapInstance trap, string name)
        {

            WriteC(Opcodes.S_OPCODE_DROPITEM);
            WriteH(trap.X);
            WriteH(trap.Y);
            WriteD(trap.Id);
            WriteH(7); // adena
            WriteC(0);
            WriteC(0);
            WriteC(0);
            WriteC(0);
            WriteD(0);
            WriteC(0);
            WriteC(0);
            WriteS(name);
            WriteC(0);
            WriteD(0);
            WriteD(0);
            WriteC(255);
            WriteC(0);
            WriteC(0);
            WriteC(0);
            WriteH(65535);
            // WriteD(0x401799a);
            WriteD(0);
            WriteC(8);
            WriteC(0);
        }
    }

}