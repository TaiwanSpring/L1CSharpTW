using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_OwnCharAttrDef : ServerBasePacket
    {

        private const string S_OWNCHARATTRDEF = "[S] S_OwnCharAttrDef";

        private byte[] _byte = null;

        /// <summary>
        /// 更新防禦以及四種屬性 </summary>
        public S_OwnCharAttrDef(L1PcInstance pc)
        {
            buildPacket(pc);
        }

        private void buildPacket(L1PcInstance pc)
        {
            WriteC(Opcodes.S_OPCODE_OWNCHARATTRDEF);
            WriteC(pc.Ac);
            WriteH(pc.Fire);
            WriteH(pc.Water);
            WriteH(pc.Wind);
            WriteH(pc.Earth);
        }
        public override string Type
        {
            get
            {
                return S_OWNCHARATTRDEF;
            }
        }
    }

}