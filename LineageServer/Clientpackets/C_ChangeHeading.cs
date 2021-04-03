using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 收到由客戶端傳來改變面向的封包
    /// </summary>
    class C_ChangeHeading : ClientBasePacket
    {
        private const string C_CHANGE_HEADING = "[C] C_ChangeHeading";
        private static ILogger _log = Logger.GetLogger(nameof(C_ChangeHeading));

        public C_ChangeHeading(byte[] decrypt, ClientThread client) : base(decrypt)
        {
            if (client.ActiveChar == null)
            {
                return;
            }

            L1PcInstance pc = client.ActiveChar;
            int heading = ReadC();
            pc.Heading = heading;

            if (pc.GmInvis || pc.Ghost)
            {
            }
            else if (pc.Invisble)
            {
                pc.broadcastPacketForFindInvis(new S_ChangeHeading(pc), true);
            }
            else
            {
                pc.broadcastPacket(new S_ChangeHeading(pc));
            }
        }

        public override string Type
        {
            get
            {
                return C_CHANGE_HEADING;
            }
        }
    }
}