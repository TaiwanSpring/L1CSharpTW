
using LineageServer.Server;
using LineageServer.Server.Model;

namespace LineageServer.Serverpackets
{
    class S_RemoveObject : ServerBasePacket
    {
        public S_RemoveObject(GameObject obj)
        {
            WriteC(Opcodes.S_OPCODE_REMOVE_OBJECT);
            WriteD(obj.Id);
        }
    }
}