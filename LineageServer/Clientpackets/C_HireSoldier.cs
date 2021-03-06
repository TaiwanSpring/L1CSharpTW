
using LineageServer.Server;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// TODO: 尚未實裝僱用傭兵的處理 處理收到由客戶端傳來僱用傭兵的封包
    /// </summary>
    class C_HireSoldier : ClientBasePacket
    {
        private const string C_HIRE_SOLDIER = "[C] C_HireSoldier";
        public C_HireSoldier(byte[] decrypt, ClientThread client) : base(decrypt)
        {
            int something1 = ReadH(); // S_HireSoldierパケットの引数
            int something2 = ReadH(); // S_HireSoldierパケットの引数
            int something3 = ReadD(); // 1以外入らない？
            int something4 = ReadD(); // S_HireSoldierパケットの引数
            int number = ReadH(); // 雇用する数

            // < 傭兵雇用処理
        }

        public override string Type
        {
            get
            {
                return C_HIRE_SOLDIER;
            }
        }
    }

}