using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來結束 ghost 狀態的封包
    /// </summary>
    class C_ExitGhost : ClientBasePacket
    {
        private const string C_EXIT_GHOST = "[C] C_ExitGhost";

        // 移動
        public C_ExitGhost(byte[] decrypt, ClientThread client) : base(decrypt)
        {
            L1PcInstance pc = client.ActiveChar;
            if ((pc == null) || (!pc.Ghost))
            {
                return;
            }

            pc.makeReadyEndGhost();
        }

        public override string Type
        {
            get
            {
                return C_EXIT_GHOST;
            }
        }
    }
}