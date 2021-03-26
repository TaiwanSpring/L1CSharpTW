using LineageServer.Interfaces;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System;

namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來封鎖密語的封包
    /// </summary>
    class C_Exclude : ClientBasePacket
    {

        private const string C_EXCLUDE = "[C] C_Exclude";

        private static ILogger _log = Logger.getLogger(nameof(C_Exclude));

        /// <summary>
        /// C_1 輸入 /exclude 指令的時候 
        /// </summary>
        public C_Exclude(byte[] decrypt, ClientThread client) : base(decrypt)
        {
            L1PcInstance pc = client.ActiveChar;
            if (pc == null)
            {
                return;
            }

            string name = ReadS();
            if (name.Length == 0)
            {
                return;
            }

            try
            {
                L1ExcludingList exList = pc.ExcludingList;
                if (exList.Full)
                {
                    pc.sendPackets(new S_ServerMessage(472)); // 被拒絕的玩家太多。
                    return;
                }
                if (exList.contains(name))
                {
                    string temp = exList.remove(name);
                    pc.sendPackets(new S_PacketBox(S_PacketBox.REM_EXCLUDE, temp));
                }
                else
                {
                    exList.add(name);
                    pc.sendPackets(new S_PacketBox(S_PacketBox.ADD_EXCLUDE, name));
                }
            }
            catch (Exception e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
        }

        public override string Type
        {
            get
            {
                return C_EXCLUDE;
            }
        }
    }

}