using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_CharVisualUpdate : ServerBasePacket
    {
        private const string _S__0B_S_CharVisualUpdate = "[C] S_CharVisualUpdate";

        public S_CharVisualUpdate(L1PcInstance pc)
        {
            WriteC(Opcodes.S_OPCODE_CHARVISUALUPDATE);
            WriteD(pc.Id);
            WriteC(pc.CurrentWeapon);
            WriteC(0xff);
            WriteC(0xff);
        }

        public S_CharVisualUpdate(L1Character cha, int status)
        {
            WriteC(Opcodes.S_OPCODE_CHARVISUALUPDATE);
            WriteD(cha.Id);
            WriteC(status);
            WriteC(0xff);
            WriteC(0xff);
        }

        public override string Type
        {
            get
            {
                return _S__0B_S_CharVisualUpdate;
            }
        }
    }

}