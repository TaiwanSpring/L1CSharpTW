using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 收到由客戶端傳送寫入公告欄的封包
    /// </summary>
    class C_BoardWrite : ClientBasePacket
    {

        private const string C_BOARD_WRITE = "[C] C_BoardWrite";
        private static ILogger _log = Logger.GetLogger(nameof(C_BoardWrite));

        public C_BoardWrite(byte[] decrypt, ClientThread client) : base(decrypt)
        {
            int id = ReadD();
            string title = ReadS();
            string content = ReadS();

            GameObject tg = Container.Instance.Resolve<IGameWorld>().findObject(id);

            if (tg == null)
            {
                _log.Warning("不正確的 NPCID : " + id);
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