using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_WhoAmount : ServerBasePacket
    {

        private const string S_WHO_AMOUNT = "[S] S_WhoAmount";

        public S_WhoAmount(string amount)
        {
            WriteC(Opcodes.S_OPCODE_SERVERMSG);
            WriteH(0x0051);
            WriteC(0x01);
            WriteS(amount);
        }
        public override string Type
        {
            get
            {
                return S_WHO_AMOUNT;
            }
        }
    }

}