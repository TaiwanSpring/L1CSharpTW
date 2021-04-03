
using LineageServer.Server;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來維持連線的封包
    /// </summary>
    class C_KeepALIVE : ClientBasePacket
    {
        private const string C_KEEP_ALIVE = "[C] C_KeepALIVE";

        public C_KeepALIVE(byte[] decrypt, ClientThread client) : base(decrypt)
        {
            // XXX:GameTimeを送信（3バイトのデータを送って来ているのでそれを何かに利用しないといけないかもしれない）
            // L1PcInstance pc = client.getActiveChar();
            // pc.sendPackets(new S_GameTime());
        }

        public override string Type
        {
            get
            {
                return C_KEEP_ALIVE;
            }
        }
    }
}