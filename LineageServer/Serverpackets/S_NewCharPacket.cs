using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_NewCharPacket : ServerBasePacket
    {
        private const string _S__25_NEWCHARPACK = "[S] New Char Packet";
        private byte[] _byte = null;

        public S_NewCharPacket(L1PcInstance pc)
        {
            buildPacket(pc);
        }

        private void buildPacket(L1PcInstance pc)
        {
            WriteC(Opcodes.S_OPCODE_NEWCHARPACK);
            WriteS(pc.Name);
            WriteS("");
            WriteC(pc.Type);
            WriteC(pc.get_sex());
            WriteH(pc.Lawful);
            WriteH(pc.MaxHp);
            WriteH(pc.BaseMaxMp);
            WriteC(pc.Ac);
            WriteC(pc.Level);
            WriteC(pc.BaseStr);
            WriteC(pc.BaseDex);
            WriteC(pc.BaseCon);
            WriteC(pc.BaseWis);
            WriteC(pc.BaseCha);
            WriteC(pc.BaseInt);
            WriteC(0); // 是否為管理員
            WriteD(pc.SimpleBirthday);
            WriteC((pc.Level ^ pc.BaseStr ^ pc.BaseDex ^ pc.BaseCon ^ pc.BaseWis ^ pc.BaseCha ^ pc.BaseInt) & 0xff); // XOR 驗證
        }

        public override string Type
        {
            get
            {
                return _S__25_NEWCHARPACK;
            }
        }

    }

}