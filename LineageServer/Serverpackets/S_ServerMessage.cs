
namespace LineageServer.Serverpackets
{
    public class S_ServerMessage : ServerBasePacket
    {
        private const string S_SERVER_MESSAGE = "[S] S_ServerMessage";

        public const int NO_PLEDGE = 208;

        public const int CANNOT_GLOBAL = 195;

        public const int CANNOT_BOOKMARK_LOCATION = 214;

        public const int USER_NOT_ON = 73;

        public const int NOT_ENOUGH_MP = 278;

        public const int YOU_FEEL_BETTER = 77;

        public const int YOUR_WEAPON_BLESSING = 693;

        public const int YOUR_Are_Slowed = 29;

        private byte[] _byte = null;

        public S_ServerMessage(int type)
        {
            buildPacket(type, null, null, null, null, null, 0);
        }

        public S_ServerMessage(int type, string msg1)
        {
            buildPacket(type, msg1, null, null, null, null, 1);
        }

        public S_ServerMessage(int type, string msg1, string msg2)
        {
            buildPacket(type, msg1, msg2, null, null, null, 2);
        }

        public S_ServerMessage(int type, string msg1, string msg2, string msg3)
        {
            buildPacket(type, msg1, msg2, msg3, null, null, 3);
        }

        public S_ServerMessage(int type, string msg1, string msg2, string msg3, string msg4)
        {
            buildPacket(type, msg1, msg2, msg3, msg4, null, 4);
        }

        public S_ServerMessage(int type, string msg1, string msg2, string msg3, string msg4, string msg5)
        {

            buildPacket(type, msg1, msg2, msg3, msg4, msg5, 5);
        }

        private void buildPacket(int type, string msg1, string msg2, string msg3, string msg4, string msg5, int check)
        {

            WriteC(Opcodes.S_OPCODE_SERVERMSG);
            WriteH(type);

            if (check == 0)
            {
                WriteC(0);
                if (type == 0x01a2)
                { // 隊伍解散
                    WriteH(0x0a00); // 關閉組隊介面
                }
            }
            else if (check == 1)
            {
                if (type == 0x09b9)
                { // 沙漠紅色訊息
                    WriteC(2);
                    WriteD(4);
                    WriteS(msg1);
                    return;
                }
                WriteC(1);
                WriteS(msg1);
            }
            else if (check == 2)
            {
                WriteC(2);
                WriteS(msg1);
                WriteS(msg2);
            }
            else if (check == 3)
            {
                WriteC(3);
                WriteS(msg1);
                WriteS(msg2);
                WriteS(msg3);
            }
            else if (check == 4)
            {
                WriteC(4);
                WriteS(msg1);
                WriteS(msg2);
                WriteS(msg3);
                WriteS(msg4);
            }
            else
            {
                WriteC(5);
                WriteS(msg1);
                WriteS(msg2);
                WriteS(msg3);
                WriteS(msg4);
                WriteS(msg5);
            }
        }

        public override sbyte[] Content
        {
            get
            {
                if (_byte == null)
                {

                    _byte = memoryStream.ToArray();
                }

                return _byte;
            }
        }

        public override string Type
        {
            get
            {
                return S_SERVER_MESSAGE;
            }
        }
    }

}