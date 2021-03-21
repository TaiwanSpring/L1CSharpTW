using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 收到由客戶端傳送打開公告欄的封包
    /// </summary>
    class C_Board : ClientBasePacket
    {

        private const string C_BOARD = "[C] C_Board";

        private bool isBoardInstance(L1Object obj)
        {
            return ((obj is L1BoardInstance) || (obj is L1AuctionBoardInstance));
        }

        public C_Board(sbyte[] abyte0, ClientThread client) : base(abyte0)
        {
            int objectId = readD();
            L1Object obj = L1World.Instance.findObject(objectId);
            if (!isBoardInstance(obj))
            {
                return; // 對象不是佈告欄停止
            }
            obj.onAction(client.ActiveChar);
        }

        public override string Type
        {
            get
            {
                return C_BOARD;
            }
        }
    }
}