using LineageServer.Server;
using LineageServer.Server.Templates;

namespace LineageServer.Serverpackets
{
    class S_BoardRead : ServerBasePacket
    {
        private const string S_BoardRead_Conflict = "[S] S_BoardRead";

        private byte[] _byte = null;

        public S_BoardRead(int number)
        {
            BuildPacket(number);
        }

        private void BuildPacket(int number)
        {
            L1BoardTopic topic = L1BoardTopic.findById(number);
            WriteC(Opcodes.S_OPCODE_BOARDREAD);
            WriteD(number);
            WriteS(topic.Name);
            WriteS(topic.Title);
            WriteS(topic.Date);
            WriteS(topic.Content);
        }

        public override string Type
        {
            get
            {
                return S_BoardRead_Conflict;
            }
        }
    }

}