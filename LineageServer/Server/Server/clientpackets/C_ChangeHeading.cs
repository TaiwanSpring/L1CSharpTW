using LineageServer.Interfaces;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 收到由客戶端傳來改變面向的封包
    /// </summary>
    class C_ChangeHeading : ClientBasePacket
    {
        private const string C_CHANGE_HEADING = "[C] C_ChangeHeading";
        private static ILogger _log = Logger.getLogger(nameof(C_ChangeHeading));

        public C_ChangeHeading(sbyte[] decrypt, ClientThread client) : base(decrypt)
        {
            if (client.ActiveChar == null)
            {
                return;
            }

            L1PcInstance pc = client.ActiveChar;
            int heading = readC();
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