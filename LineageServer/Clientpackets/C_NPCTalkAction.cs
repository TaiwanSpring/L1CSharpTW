using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來NPC講話動作的封包
    /// </summary>
    class C_NPCTalkAction : ClientBasePacket
    {
        private const string C_NPC_TALK_ACTION = "[C] C_NPCTalkAction";
        public C_NPCTalkAction(byte[] decrypt, ClientThread client) : base(decrypt)
        {
            L1PcInstance activeChar = client.ActiveChar;

            if (activeChar == null)
            {
                return;
            }

            int objectId = ReadD();

            string action = ReadS();

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