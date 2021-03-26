using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Utils;

namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來傳送的封包
    /// </summary>
    class C_Teleport : ClientBasePacket
    {

        private const string C_TELEPORT = "[C] C_Teleport";

        public C_Teleport(byte[] abyte0, ClientThread clientthread) : base(abyte0)
        {
            L1PcInstance pc = clientthread.ActiveChar;
            if (pc == null)
            {
                return;
            }
            Teleportation.actionTeleportation(pc);
        }

        public override string Type
        {
            get
            {
                return C_TELEPORT;
            }
        }
    }

}