using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Npc;

namespace LineageServer.Serverpackets
{
    class S_NPCTalkReturn : ServerBasePacket
    {
        private const string _S__25_TalkReturn = "[S] _S__25_TalkReturn";
        private byte[] _byte = null;

        public S_NPCTalkReturn(L1NpcTalkData npc, int objid, int action, string[] data)
        {

            string htmlid;

            if (action == 1)
            {
                htmlid = npc.NormalAction;
            }
            else if (action == 2)
            {
                htmlid = npc.CaoticAction;
            }
            else
            {
                throw new System.ArgumentException();
            }

            buildPacket(objid, htmlid, data);
        }

        public S_NPCTalkReturn(L1NpcTalkData npc, int objid, int action) : this(npc, objid, action, null)
        {
        }

        public S_NPCTalkReturn(int objid, string htmlid, string[] data)
        {
            buildPacket(objid, htmlid, data);
        }

        public S_NPCTalkReturn(int objid, string htmlid)
        {
            buildPacket(objid, htmlid, null);
        }

        public S_NPCTalkReturn(int objid, L1NpcHtml html)
        {
            buildPacket(objid, html.Name, html.Args);
        }

        private void buildPacket(int objid, string htmlid, string[] data)
        {

            WriteC(Opcodes.S_OPCODE_SHOWHTML);
            WriteD(objid);
            WriteS(htmlid);
            if (data != null && 1 <= data.Length)
            {
                WriteH(0x01); // 不明バイト 分かる人居たら修正願います
                WriteH(data.Length); // 引数の数
                foreach (string datum in data)
                {
                    WriteS(datum);
                }
            }
            else
            {
                WriteH(0x00);
                WriteH(0x00);
            }
        }
        public override string Type
        {
            get
            {
                return _S__25_TalkReturn;
            }
        }
    }

}