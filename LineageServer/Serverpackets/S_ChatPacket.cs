using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_ChatPacket : ServerBasePacket
    {

        private const string _S__1F_NORMALCHATPACK = "[S] S_ChatPacket";

        private byte[] _byte = null;

        public S_ChatPacket(L1PcInstance pc, string chat, int opcode, int type)
        {

            if (type == 0)
            { // 一般頻道
                WriteC(opcode);
                WriteC(type);
                if (pc.Invisble)
                {
                    WriteD(0);
                }
                else
                {
                    WriteD(pc.Id);
                }
                WriteS($"{pc.Name}: {chat}");
            }
            else if (type == 2)
            { // 大喊
                WriteC(opcode);
                WriteC(type);
                if (pc.Invisble)
                {
                    WriteD(0);
                }
                else
                {
                    WriteD(pc.Id);
                }
                WriteS($"<{pc.Name}> {chat}");
                WriteH(pc.X);
                WriteH(pc.Y);
            }
            else if (type == 3)
            { // 世界頻道
                WriteC(opcode);
                WriteC(type);
                if (pc.Gm == true)
                {
                    if (Config.GM_TALK)
                    {
                        WriteS($"[*{pc.Name}*] {chat}");
                    }
                    else
                    {
                        WriteS("[******] " + chat);
                    }
                }
            }
            else if (type == 4)
            { // 血盟騎士頻道
                WriteC(opcode);
                WriteC(type);
                WriteS($"{{{pc.Name}}} {chat}");
            }
            else if (type == 9)
            { // 密語頻道
                WriteC(opcode);
                WriteC(type);
                WriteS($"-> ({pc.Name}) {chat}");
            }
            else if (type == 11)
            { // 血盟頻道
                WriteC(opcode);
                WriteC(type);
                WriteS("(" + pc.Name + ") " + chat);
            }
            else if (type == 12)
            { // 交易頻道
                WriteC(opcode);
                WriteC(type);
                WriteS($"[{pc.Name}] {chat}");
            }
            else if (type == 13)
            { // 聯盟頻道
                WriteC(opcode);
                WriteC(0x04);
                WriteS($"{{{{{pc.Name}}}}} {chat}");
            }
            else if (type == 14)
            { // 隊伍頻道
                WriteC(opcode);
                WriteC(type);
                if (pc.Invisble)
                {
                    WriteD(0);
                }
                else
                {
                    WriteD(pc.Id);
                }
                WriteS($"({pc.Name}) {chat}");
            }
            else if (type == 16)
            { // 密語頻道
                WriteC(opcode);
                WriteS(pc.Name);
                WriteS(chat);
            }
            else if (type == 17)
            { // 血盟王族公告頻道
                WriteC(opcode);
                WriteC(type);
                WriteS($"{{{pc.Name}}}{chat}");
            }
        }
        public override string Type
        {
            get
            {
                return _S__1F_NORMALCHATPACK;
            }
        }

    }
}