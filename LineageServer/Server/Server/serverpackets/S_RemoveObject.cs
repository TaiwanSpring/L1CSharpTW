
using LineageServer.Server.Server.Model;

namespace LineageServer.Server.Server.serverpackets
{
    class S_RemoveObject : ServerBasePacket
    {
        public S_RemoveObject(L1Object obj)
        {
            WriteC(Opcodes.S_OPCODE_REMOVE_OBJECT);
            WriteD(obj.Id);
        }
    }
}