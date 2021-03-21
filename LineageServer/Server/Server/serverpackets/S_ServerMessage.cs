
namespace LineageServer.Server.Server.serverpackets
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

            writeC(Opcodes.S_OPCODE_SERVERMSG);
            writeH(type);

            if (check == 0)
            {
                writeC(0);
                if (type == 0x01a2)
                { // 隊伍解散
                    writeH(0x0a00); // 關閉組隊介面
                }
            }
            else if (check == 1)
            {
                if (type == 0x09b9)
                { // 沙漠紅色訊息
                    writeC(2);
                    writeD(4);
                    writeS(msg1);
                    return;
                }
                writeC(1);
                writeS(msg1);
            }
            else if (check == 2)
            {
                writeC(2);
                writeS(msg1);
                writeS(msg2);
            }
            else if (check == 3)
            {
                writeC(3);
                writeS(msg1);
                writeS(msg2);
                writeS(msg3);
            }
            else if (check == 4)
            {
                writeC(4);
                writeS(msg1);
                writeS(msg2);
                writeS(msg3);
                writeS(msg4);
            }
            else
            {
                writeC(5);
                writeS(msg1);
                writeS(msg2);
                writeS(msg3);
                writeS(msg4);
                writeS(msg5);
            }
        }

        public override sbyte[] Content
        {
            get
            {
                if (_byte == null)
                {

                    _byte = _bao.ToArray();
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