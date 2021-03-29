using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Server.Model.Instance
{
    class L1RequestInstance : L1NpcInstance
    {
        private static ILogger _log = Logger.GetLogger(nameof(L1RequestInstance));

        public L1RequestInstance(L1Npc template) : base(template)
        {
        }

        public override void onAction(L1PcInstance player)
        {
            int objid = Id;

            L1NpcTalkData talking = NPCTalkDataTable.Instance.getTemplate(NpcTemplate.get_npcId());

            if (talking != null)
            {
                if (player.Lawful < -1000)
                { // プレイヤーがカオティック
                    player.sendPackets(new S_NPCTalkReturn(talking, objid, 2));
                }
                else
                {
                    player.sendPackets(new S_NPCTalkReturn(talking, objid, 1));
                }
            }
            else
            {
                _log.Log("No actions for npc id : " + objid);
            }
        }

        public override void onFinalAction(L1PcInstance player, string action)
        {

        }

        public virtual void doFinalAction(L1PcInstance player)
        {

        }
    }

}