using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.serverpackets
{
    class S_CharVisualUpdate : ServerBasePacket
    {
        private const string _S__0B_S_CharVisualUpdate = "[C] S_CharVisualUpdate";

        public S_CharVisualUpdate(L1PcInstance pc)
        {
            writeC(Opcodes.S_OPCODE_CHARVISUALUPDATE);
            writeD(pc.Id);
            writeC(pc.CurrentWeapon);
            writeC(0xff);
            writeC(0xff);
        }

        public S_CharVisualUpdate(L1Character cha, int status)
        {
            writeC(Opcodes.S_OPCODE_CHARVISUALUPDATE);
            writeD(cha.Id);
            writeC(status);
            writeC(0xff);
            writeC(0xff);
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