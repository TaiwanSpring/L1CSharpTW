using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_NpcChangeShape : ServerBasePacket
    {

        private byte[] _byte = null;

        /// <summary>
        /// 使用於怪物變身 </summary>
        public S_NpcChangeShape(int objId, int polyId, int lawful, int status)
        {
            WriteC(Opcodes.S_OPCODE_SPOLY);
            WriteD(objId);
            WriteD(0); // ???
            WriteH(polyId);
            WriteH(lawful); // 正義值
            WriteH(status); // 狀態
        }
    }
}