using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 收到由客戶端傳送公告欄回到上一頁的封包
    /// </summary>
    class C_BoardBack : ClientBasePacket
    {

        private const string C_BOARD_BACK = "[C] C_BoardBack";

        public C_BoardBack(byte[] abyte0, ClientThread client) : base(abyte0)
        {
            int objId = ReadD();
            int topicNumber = ReadD();
            GameObject obj = L1World.Instance.findObject(objId);
            L1BoardInstance board = (L1BoardInstance)obj;
            board.onAction(client.ActiveChar, topicNumber);
        }

        public override string Type
        {
            get
            {
                return C_BOARD_BACK;
            }
        }
    }
}