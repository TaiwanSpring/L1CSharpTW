using System.Text;

namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來回到登入畫面的封包
    /// </summary>
    class C_ReturnToLogin : ClientBasePacket
    {
        public C_ReturnToLogin(byte[] decrypt, ClientThread client) : base(decrypt)
        {
            LoginController.Instance.logout(client);
        }

        public override string Type
        {
            get
            {
                return "[C] C_ReturnToLogin";
            }
        }

    }

}