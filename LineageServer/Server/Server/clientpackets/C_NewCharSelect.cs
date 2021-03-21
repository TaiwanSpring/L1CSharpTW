using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來選擇新角色的封包
    /// </summary>
    class C_NewCharSelect : ClientBasePacket
    {
        private const string C_NEW_CHAR_SELECT = "[C] C_NewCharSelect";

        public C_NewCharSelect(sbyte[] decrypt, ClientThread client) : base(decrypt)
        {
            client.SendPacket(new S_PacketBox(S_PacketBox.LOGOUT)); // 2.70C->3.0追加
            client.CharReStart(true);
            if (client.ActiveChar != null)
            {
                L1PcInstance pc = client.ActiveChar;

                //XXX 修正死亡洗血bug
                if (pc.Dead)
                {
                    return;
                }
                ClientThread.quitGame(pc);

                lock (pc)
                {
                    pc.logout();
                    client.ActiveChar = null;
                }
            }
        }

        public override string Type
        {
            get
            {
                return C_NEW_CHAR_SELECT;
            }
        }
    }

}