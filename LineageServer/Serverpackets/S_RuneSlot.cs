using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_RuneSlot : ServerBasePacket
    {
        private const string S_RUNESLOT = "[S] S_RuneSlot";

        public static int RUNE_CLOSE_SLOT = 1;
        public static int RUNE_OPEN_SLOT = 2;

        public S_RuneSlot(int type, int slotNum)
        {
            WriteC(Opcodes.S_OPCODE_CHARRESET);
            WriteC(0x43);
            WriteD(type);
            WriteD(slotNum);
            WriteD(0);
            WriteD(0);
            WriteD(0);
            WriteH(0);
        }

        public override string Type
        {
            get
            {
                return S_RUNESLOT;
            }
        }
    }


}