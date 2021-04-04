using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_ChangeShape : ServerBasePacket
    {

        private byte[] _byte = null;

        public S_ChangeShape(int objId, int polyId, int currentWeapon)
        {
            WriteC(Opcodes.S_OPCODE_POLY);
            WriteD(objId);
            WriteH(polyId);
            WriteC(currentWeapon);
            WriteC(0xff);
            WriteC(0xff);
        }
    }

}