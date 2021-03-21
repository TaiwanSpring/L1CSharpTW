using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來NPC講話動作的封包
    /// </summary>
    class C_NPCTalkAction : ClientBasePacket
    {
        private const string C_NPC_TALK_ACTION = "[C] C_NPCTalkAction";
        public C_NPCTalkAction(sbyte[] decrypt, ClientThread client) : base(decrypt)
        {
            L1PcInstance activeChar = client.ActiveChar;

            if (activeChar == null)
            {
                return;
            }

            int objectId = readD();

            string action = readS();

            if (L1World.Instance.findObject(objectId) is L1NpcInstance npc)
            {
                npc.onFinalAction(activeChar, action);
            }
        }

        public override string Type
        {
            get
            {
                return C_NPC_TALK_ACTION;
            }
        }

    }

}