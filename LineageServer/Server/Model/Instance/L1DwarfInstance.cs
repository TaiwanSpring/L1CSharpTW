using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataSources;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
namespace LineageServer.Server.Model.Instance
{
    class L1DwarfInstance : L1NpcInstance
    {
        private static ILogger _log = Logger.GetLogger(nameof(L1DwarfInstance));

        /// <param name="template"> </param>
        public L1DwarfInstance(L1Npc template) : base(template)
        {
        }

        public override void onAction(L1PcInstance pc)
        {
            onAction(pc, 0);
        }

        public override void onAction(L1PcInstance pc, int skillId)
        {
            L1Attack attack = new L1Attack(pc, this, skillId);
            attack.calcHit();
            attack.action();
            attack.calcDamage();
            attack.addChaserAttack();
            attack.calcStaffOfMana();
            attack.addPcPoisonAttack(pc, this);
            attack.commit();
        }

        public override void onTalkAction(L1PcInstance pc)
        {
            int objid = Id;
            L1NpcTalkData talking = NPCTalkDataTable.Instance.getTemplate(NpcTemplate.get_npcId());
            int npcId = NpcTemplate.get_npcId();
            string htmlid = null;

            if (talking != null)
            {
                if (npcId == 60028)
                { // エル
                    if (!pc.Elf)
                    {
                        htmlid = "elCE1";
                    }
                }

                if (!string.ReferenceEquals(htmlid, null))
                { // htmlidが指定されている場合
                    pc.sendPackets(new S_NPCTalkReturn(objid, htmlid));
                }
                else
                {
                    if (pc.Level < 5)
                    {
                        pc.sendPackets(new S_NPCTalkReturn(talking, objid, 2));
                    }
                    else
                    {
                        pc.sendPackets(new S_NPCTalkReturn(talking, objid, 1));
                    }
                }
            }
        }

        public override void onFinalAction(L1PcInstance pc, string Action)
        {
            if (Action == "retrieve")
            {
                _log.Log("Retrive items in storage");
            }
            else if (Action == "retrieve-pledge")
            {
                _log.Log("Retrive items in pledge storage");

                if (string.IsNullOrEmpty(pc.Clanname) || pc.Clanname == " ")
                {
                    _log.Log("pc isnt in a pledge");
                    S_ServerMessage talk = new S_ServerMessage((S_ServerMessage.NO_PLEDGE), Action);
                    pc.sendPackets(talk);
                }
                else
                {
                    _log.Log("pc is in a pledge");
                }
            }
        }
    }

}