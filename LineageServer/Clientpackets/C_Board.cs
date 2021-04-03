using LineageServer.Interfaces;
using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 收到由客戶端傳送打開公告欄的封包
    /// </summary>
    class C_Board : ClientBasePacket
    {

        private const string C_BOARD = "[C] C_Board";

        private bool isBoardInstance(GameObject obj)
        {
            return ((obj is L1BoardInstance) || (obj is L1AuctionBoardInstance));
        }

        public C_Board(byte[] abyte0, ClientThread client) : base(abyte0)
        {
            int objectId = ReadD();
            GameObject obj = Container.Instance.Resolve<IGameWorld>().findObject(objectId);
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