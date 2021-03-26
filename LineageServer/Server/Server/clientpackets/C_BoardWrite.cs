using LineageServer.Interfaces;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.identity;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Templates;

namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 收到由客戶端傳送寫入公告欄的封包
    /// </summary>
    class C_BoardWrite : ClientBasePacket
    {

        private const string C_BOARD_WRITE = "[C] C_BoardWrite";
        private static ILogger _log = Logger.getLogger(nameof(C_BoardWrite));

        public C_BoardWrite(byte[] decrypt, ClientThread client) : base(decrypt)
        {
            int id = ReadD();
            string title = ReadS();
            string content = ReadS();

            L1Object tg = L1World.Instance.findObject(id);

            if (tg == null)
            {
                _log.warning("不正確的 NPCID : " + id);
                return;
            }

            L1PcInstance pc = client.ActiveChar;
            L1BoardTopic.create(pc.Name, title, content);
            pc.Inventory.consumeItem(L1ItemId.ADENA, 300);
        }

        public override string Type
        {
            get
            {
                return C_BOARD_WRITE;
            }
        }
    }

}