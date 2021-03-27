using LineageServer.Server.Model.Instance;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來登入到伺服器OK的封包
    /// </summary>
    class C_LoginToServerOK : ClientBasePacket
    {

        private const string C_LOGIN_TO_SERVER_OK = "[C] C_LoginToServerOK";

        public C_LoginToServerOK(byte[] decrypt, ClientThread client) : base(decrypt)
        {
            L1PcInstance pc = client.ActiveChar;
            if (pc == null)
            {
                return;
            }

            int type = ReadC();
            int button = ReadC();

            if (type == 255)
            { // 全體聊天 && 密語
                if ((button == 95) || (button == 127))
                {
                    pc.ShowWorldChat = true; // open
                    pc.CanWhisper = true; // open
                }
                else if ((button == 91) || (button == 123))
                {
                    pc.ShowWorldChat = true; // open
                    pc.CanWhisper = false; // close
                }
                else if ((button == 94) || (button == 126))
                {
                    pc.ShowWorldChat = false; // close
                    pc.CanWhisper = true; // open
                }
                else if ((button == 90) || (button == 122))
                {
                    pc.ShowWorldChat = false; // close
                    pc.CanWhisper = false; // close
                }
            }
            else if (type == 0)
            { // 全體聊天
                if (button == 0)
                { // close
                    pc.ShowWorldChat = false;
                }
                else if (button == 1)
                { // open
                    pc.ShowWorldChat = true;
                }
            }
            else if (type == 2)
            { // 密語
                if (button == 0)
                { // close
                    pc.CanWhisper = false;
                }
                else if (button == 1)
                { // open
                    pc.CanWhisper = true;
                }
            }
            else if (type == 6)
            { // 交易頻道
                if (button == 0)
                { // close
                    pc.ShowTradeChat = false;
                }
                else if (button == 1)
                { // open
                    pc.ShowTradeChat = true;
                }
            }
            else if (type == 9)
            { // 血盟
                if (button == 0)
                { // open
                    pc.ShowClanChat = true;
                }
                else if (button == 1)
                { // close
                    pc.ShowClanChat = false;
                }
            }
            else if (type == 10)
            { // 組隊
                if (button == 0)
                { // close
                    pc.ShowPartyChat = false;
                }
                else if (button == 1)
                { // open
                    pc.ShowPartyChat = true;
                }
            }
        }

        public override string Type
        {
            get
            {
                return C_LOGIN_TO_SERVER_OK;
            }
        }
    }

}