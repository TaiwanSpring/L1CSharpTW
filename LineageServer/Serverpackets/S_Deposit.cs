using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_Deposit : ServerBasePacket
    {

        public S_Deposit(int objecId)
        {
            WriteC(Opcodes.S_OPCODE_DEPOSIT);
            WriteD(objecId);
        }

        public override string Type
        {
            get
            {
                return "[S] S_Deposit";
            }
        }
    }

}