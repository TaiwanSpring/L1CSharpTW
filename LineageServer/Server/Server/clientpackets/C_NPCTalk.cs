
using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Model.Npc;
using LineageServer.Server.Server.Model.Npc.Action;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來NPC講話的封包
    /// </summary>
    class C_NPCTalk : ClientBasePacket
    {

        private const string C_NPC_TALK = "[C] C_NPCTalk";
        public C_NPCTalk(sbyte[] abyte0, ClientThread client) : base(abyte0)
        {
            L1PcInstance pc = client.ActiveChar;
            if (pc == null)
            {
                return;
            }

            int objid = readD();
            L1Object obj = L1World.Instance.findObject(objid);

            if (obj != null)
            {
                INpcAction action = NpcActionTable.Instance.get(pc, obj);
                if (action != null)
                {
                    L1NpcHtml html = action.execute("", pc, obj, new sbyte[0]);
                    if (html != null)
                    {
                        pc.sendPackets(new S_NPCTalkReturn(obj.Id, html));
                    }
                    return;
                }
                obj.onTalkAction(pc);
            }
        }

        public override string Type
        {
            get
            {
                return C_NPC_TALK;
            }
        }
    }

}