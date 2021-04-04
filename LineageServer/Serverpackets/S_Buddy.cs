using LineageServer.Server;
using LineageServer.Server.Model;

namespace LineageServer.Serverpackets
{
    class S_Buddy : ServerBasePacket
    {
        private const string _S_Buddy = "[S] _S_Buddy";
        private const string _HTMLID = "buddy";

        private byte[] _byte = null;

        public S_Buddy(int objId, L1Buddy buddy)
        {
            buildPacket(objId, buddy);
        }

        private void buildPacket(int objId, L1Buddy buddy)
        {
            WriteC(Opcodes.S_OPCODE_SHOWHTML);
            WriteD(objId);
            WriteS(_HTMLID);
            WriteC(0x00);
            WriteH(0x02);

            WriteS(buddy.BuddyListString);
            WriteS(buddy.OnlineBuddyListString);
        }

        public override string Type
        {
            get
            {
                return _S_Buddy;
            }
        }
    }

}