using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_MPUpdate : ServerBasePacket
    {
        public S_MPUpdate(int currentmp, int maxmp)
        {
            WriteC(Opcodes.S_OPCODE_MPUPDATE);

            if (currentmp < 0)
            {
                WriteH(0);
            }
            else if (currentmp > 32767)
            {
                WriteH(32767);
            }
            else
            {
                WriteH(currentmp);
            }

            if (maxmp < 1)
            {
                WriteH(1);
            }
            else if (maxmp > 32767)
            {
                WriteH(32767);
            }
            else
            {
                WriteH(maxmp);
            }

            // WriteH(currentmp);
            // WriteH(maxmp);
            // WriteC(0);
            // WriteD(GameTimeController.getInstance().getGameTime());
        }
    }
}