using LineageServer.Server;
using LineageServer.Server.Model;

namespace LineageServer.Serverpackets
{
    class S_NPCTalkActionTPUrl : ServerBasePacket
    {
        private const string _S__25_TalkReturnAction = "[S] S_NPCTalkActionTPUrl";
        private byte[] _byte = null;

        public S_NPCTalkActionTPUrl(L1NpcTalkData cha, object[] prices, int objid)
        {
            buildPacket(cha, prices, objid);
        }

        private void buildPacket(L1NpcTalkData npc, object[] prices, int objid)
        {
            string htmlid = "";
            htmlid = npc.TeleportURL;
            WriteC(Opcodes.S_OPCODE_SHOWHTML);
            WriteD(objid);
            WriteS(htmlid);
            WriteH(0x01); // 不明
            WriteH(prices.Length); // 引数の数

            foreach (object price in prices)
            {
                WriteS((((int?)price).Value).ToString());
            }
        }
        public override string Type
        {
            get
            {
                return _S__25_TalkReturnAction;
            }
        }
    }

}