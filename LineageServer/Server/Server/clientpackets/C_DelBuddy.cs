
using LineageServer.Server.Server.datatables;
using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來刪除好友的封包
    /// </summary>
    class C_DelBuddy : ClientBasePacket
    {
        private const string C_DEL_BUDDY = "[C] C_DelBuddy";

        public C_DelBuddy(sbyte[] abyte0, ClientThread clientthread) : base(abyte0)
        {

            L1PcInstance pc = clientthread.ActiveChar;
            if (pc == null)
            {
                return;
            }
            string charName = readS();
            BuddyTable.Instance.removeBuddy(pc.Id, charName);
        }

        public override string Type
        {
            get
            {
                return C_DEL_BUDDY;
            }
        }
    }
}