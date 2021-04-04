using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_ClanName : ServerBasePacket
    {

        private const string S_ClanName_Conflict = "[S] S_ClanName";

        private byte[] _byte = null;

        public S_ClanName(L1PcInstance pc, bool OnOff)
        {
            WriteC(Opcodes.S_OPCODE_CLANNAME);
            WriteD(pc.Id);
            WriteS(OnOff ? pc.Clanname : "");
            WriteD(0);
            WriteC(0);
            WriteC(OnOff ? 0x0a : 0x0b);
            if (!OnOff)
            {
                WriteD(pc.Clanid);
            }
            else
            {
                WriteD(0);
            }
        }

        public override string Type
        {
            get
            {
                return S_ClanName_Conflict;
            }
        }
    }

}