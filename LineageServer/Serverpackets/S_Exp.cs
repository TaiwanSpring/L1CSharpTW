using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_Exp : ServerBasePacket
    {

        private const string S_EXP = "[S] S_Exp";

        /// <summary>
        /// レベルと経験値データを送る。
        /// </summary>
        /// <param name="pc">
        ///            - PC </param>
        public S_Exp(L1PcInstance pc)
        {
            WriteC(Opcodes.S_OPCODE_EXP);
            WriteC(pc.Level);
            WriteExp(pc.Exp);

            // WriteC(Opcodes.S_OPCODE_EXP);
            // WriteC(0x39);// level
            // WriteD(_objid);// ??
            // WriteC(0x0A);// ??
            // WriteH(getexp);// min exp
            // WriteH(getexpreward);// max exp
        }
        public override string Type
        {
            get
            {
                return S_EXP;
            }
        }
    }

}