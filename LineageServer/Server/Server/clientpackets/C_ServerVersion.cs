
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來要求伺服器版本的封包
    /// </summary>
    class C_ServerVersion : ClientBasePacket
    {

        private const string C_SERVER_VERSION = "[C] C_ServerVersion";
        public C_ServerVersion(byte[] decrypt, ClientThread client) : base(decrypt)
        {

            /* From client: client version
			 * [Client] opcode = 14
			 * 0000: 0e 34 00/ b6 /03 00 00 00 /09 f0 6e f0 65 51 c7 00 .4........n.eQ..
			 * 0010: 01 00 06 00 ....
			 */
            ReadH();
            ReadC();

            int clientLanguage = ReadD(); // 主程式語系
            int unknownVer1 = ReadH(); // 未知的版本號
            int unknownVer2 = ReadH(); // 未知的版本號
            int clientVersion = ReadD(); // 主程式版本號

            client.SendPacket(new S_ServerVersion());
        }

        public override string Type
        {
            get
            {
                return C_SERVER_VERSION;
            }
        }

    }

}