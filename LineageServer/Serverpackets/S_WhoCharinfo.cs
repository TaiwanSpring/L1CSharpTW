using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_WhoCharinfo : ServerBasePacket
    {
        private const string S_WHO_CHARINFO = "[S] S_WhoCharinfo";
        private static ILogger _log = Logger.GetLogger(nameof(S_WhoCharinfo));

        public S_WhoCharinfo(L1PcInstance pc)
        {
            _log.Log("Who charpack for : " + pc.Name);

            string lawfulness = "";
            int lawful = pc.Lawful;
            if (lawful < 0)
            {
                lawfulness = "(Chaotic)";
            }
            else if (lawful >= 0 && lawful < 500)
            {
                lawfulness = "(Neutral)";
            }
            else if (lawful >= 500)
            {
                lawfulness = "(Lawful)";
            }

            WriteC(Opcodes.S_OPCODE_GLOBALCHAT);
            WriteC(0x08);

            string title = "";
            string clan = "";

            if (!string.IsNullOrEmpty(pc.Title))
            {
                title = pc.Title + " ";
            }

            if (pc.Clanid > 0)
            {
                clan = "[" + pc.Clanname + "]";
            }

            WriteS(title + pc.Name + " " + lawfulness + " " + clan);
            // WriteD(0x80157FE4);
            WriteD(0);
        }
        public override string Type
        {
            get
            {
                return S_WHO_CHARINFO;
            }
        }
    }

}