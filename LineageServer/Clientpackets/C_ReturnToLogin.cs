using LineageServer.Interfaces;
using LineageServer.Server;
using System.Text;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來回到登入畫面的封包
    /// </summary>
    class C_ReturnToLogin : ClientBasePacket
    {
        public C_ReturnToLogin(byte[] decrypt, ClientThread client) : base(decrypt)
        {
            Container.Instance.Resolve<ILoginController>().logout(client);
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