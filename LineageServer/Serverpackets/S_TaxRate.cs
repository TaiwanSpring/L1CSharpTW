using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_TaxRate : ServerBasePacket
    {

        public S_TaxRate(int objecId)
        {
            WriteC(Opcodes.S_OPCODE_TAXRATE);
            WriteD(objecId);
            WriteC(10); // 10%~50%
            WriteC(50);
        }

        public override string Type
        {
            get
            {
                return "[S] S_TaxRate";
            }
        }
    }

}