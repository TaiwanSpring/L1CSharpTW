using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_NpcChatPacket : ServerBasePacket
    {
        private const string S_NPC_CHAT_PACKET = "[S] S_NpcChatPacket";

        private byte[] _byte = null;

        public S_NpcChatPacket(L1NpcInstance npc, string chat, int type)
        {
            buildPacket(npc, chat, type);
        }

        private void buildPacket(L1NpcInstance npc, string chat, int type)
        {
            switch (type)
            {
                case 0: // normal chat
                    WriteC(Opcodes.S_OPCODE_NPCSHOUT); // Key is 16 , can use
                                                       // desc-?.tbl
                    WriteC(type); // Color
                    WriteD(npc.Id);
                    WriteS($"{npc.Name}: {chat}");
                    break;

                case 2: // shout
                    WriteC(Opcodes.S_OPCODE_NPCSHOUT); // Key is 16 , can use
                                                       // desc-?.tbl
                    WriteC(type); // Color
                    WriteD(npc.Id);
                    WriteS($"<{npc.Name}> {chat}");
                    break;

                case 3: // world chat
                    WriteC(Opcodes.S_OPCODE_NPCSHOUT);
                    WriteC(type); // XXX 白色になる
                    WriteD(npc.Id);
                    WriteS($"[{npc.Name}] {chat}");
                    break;

                default:
                    break;
            }
        }
        public override string Type
        {
            get
            {
                return S_NPC_CHAT_PACKET;
            }
        }
    }

}