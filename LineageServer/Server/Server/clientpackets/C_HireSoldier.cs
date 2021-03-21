
namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// TODO: 尚未實裝僱用傭兵的處理 處理收到由客戶端傳來僱用傭兵的封包
    /// </summary>
    class C_HireSoldier : ClientBasePacket
    {
        private const string C_HIRE_SOLDIER = "[C] C_HireSoldier";
        public C_HireSoldier(sbyte[] decrypt, ClientThread client) : base(decrypt)
        {
            int something1 = readH(); // S_HireSoldierパケットの引数
            int something2 = readH(); // S_HireSoldierパケットの引数
            int something3 = readD(); // 1以外入らない？
            int something4 = readD(); // S_HireSoldierパケットの引数
            int number = readH(); // 雇用する数

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