using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Server.Model.Instance
{
    class L1QuestInstance : L1NpcInstance
    {

        /// 
        private const long serialVersionUID = 1L;

        public L1QuestInstance(L1Npc template) : base(template)
        {
        }

        public override void onNpcAI()
        {
            int npcId = NpcTemplate.get_npcId();
            if (AiRunning)
            {
                return;
            }
            if ((npcId == 71075) || (npcId == 70957) || (npcId == 81209))
            {
                return;
            }
            else
            {
                Actived = false;
                startAI();
            }
        }

        public override void onAction(L1PcInstance pc)
        {
            onAction(pc, 0);
        }

        public override void onAction(L1PcInstance pc, int skillId)
        {
            L1Attack attack = new L1Attack(pc, this, skillId);
            if (attack.calcHit())
            {
                attack.calcDamage();
                attack.calcStaffOfMana();
                attack.addPcPoisonAttack(pc, this);
                attack.addChaserAttack();
            }
            attack.action();
            attack.commit();
        }

        public override void onTalkAction(L1PcInstance pc)
        {
            int pcX = pc.X;
            int pcY = pc.Y;
            int npcX = X;
            int npcY = Y;

            if ((pcX == npcX) && (pcY < npcY))
            {
                Heading = 0;
            }
            else if ((pcX > npcX) && (pcY < npcY))
            {
                Heading = 1;
            }
            else if ((pcX > npcX) && (pcY == npcY))
            {
                Heading = 2;
            }
            else if ((pcX > npcX) && (pcY > npcY))
            {
                Heading = 3;
            }
            else if ((pcX == npcX) && (pcY > npcY))
            {
                Heading = 4;
            }
            else if ((pcX < npcX) && (pcY > npcY))
            {
                Heading = 5;
            }
            else if ((pcX < npcX) && (pcY == npcY))
            {
                Heading = 6;
            }
            else if ((pcX < npcX) && (pcY < npcY))
            {
                Heading = 7;
            }
            broadcastPacket(new S_ChangeHeading(this));

            int npcId = NpcTemplate.get_npcId();
            if ((npcId == 71092) || (npcId == 71093))
            { // 調査員
                if (pc.Knight && (pc.Quest.get_step(3) == 4))
                {
                    pc.sendPackets(new S_NPCTalkReturn(Id, "searcherk1"));
                }
                else
                {
                    pc.sendPackets(new S_NPCTalkReturn(Id, "searcherk4"));
                }
            }
            else if (npcId == 71094)
            { // 安迪亞
                if (pc.Darkelf && (pc.Quest.get_step(4) == 2))
                {
                    pc.sendPackets(new S_NPCTalkReturn(Id, "endiaq1"));
                }
                else
                {
                    pc.sendPackets(new S_NPCTalkReturn(Id, "endiaq4"));
                }
            }
            else if (npcId == 71062)
            { // 卡米特
                if (pc.Quest.get_step(L1Quest.QUEST_CADMUS) == 2)
                {
                    pc.sendPackets(new S_NPCTalkReturn(Id, "kamit1b"));
                }
                else
                {
                    pc.sendPackets(new S_NPCTalkReturn(Id, "kamit1"));
                }
            }
            else if (npcId == 71075)
            { // 疲憊的蜥蜴人戰士
                if (pc.Quest.get_step(L1Quest.QUEST_LIZARD) == 1)
                {
                    pc.sendPackets(new S_NPCTalkReturn(Id, "llizard1b"));
                }
                else
                {
                    pc.sendPackets(new S_NPCTalkReturn(Id, "llizard1a"));
                }
            }
            else if ((npcId == 70957) || (npcId == 81209))
            { // ロイ
                if (pc.Quest.get_step(L1Quest.QUEST_ROI) != 1)
                {
                    pc.sendPackets(new S_NPCTalkReturn(Id, "roi1"));
                }
                else
                {
                    pc.sendPackets(new S_NPCTalkReturn(Id, "roi2"));
                }
            }
            else if (npcId == 81350)
            { // 迪嘉勒廷的女間諜
                if (pc.Elf && (pc.Quest.get_step(4) == 3))
                {
                    pc.sendPackets(new S_NPCTalkReturn(Id, "dspy2"));
                }
                else
                {
                    pc.sendPackets(new S_NPCTalkReturn(Id, "dspy1"));
                }
            }

            lock (this)
            {
                if (_monitor != null)
                {
                    _monitor.cancel();
                }
                Rest = true;
                _monitor = new RestMonitor(this);
                Container.Instance.Resolve<ITaskController>().execute(this._monitor, REST_MILLISEC);
            }
        }

        public override void onFinalAction(L1PcInstance pc, string action)
        {
            if (action == "start")
            {
                int npcId = NpcTemplate.get_npcId();
                if (((npcId == 71092) || (npcId == 71093)) && pc.Knight && (pc.Quest.get_step(3) == 4))
                {
                    L1Npc l1npc = Container.Instance.Resolve<INpcController>().getTemplate(71093);
                    new L1FollowerInstance(l1npc, this, pc);
                    pc.sendPackets(new S_NPCTalkReturn(Id, ""));
                }
                else if ((npcId == 71094) && pc.Darkelf && (pc.Quest.get_step(4) == 2))
                {
                    L1Npc l1npc = Container.Instance.Resolve<INpcController>().getTemplate(71094);
                    new L1FollowerInstance(l1npc, this, pc);
                    pc.sendPackets(new S_NPCTalkReturn(Id, ""));
                }
                else if ((npcId == 71062) && (pc.Quest.get_step(L1Quest.QUEST_CADMUS) == 2))
                {
                    L1Npc l1npc = Container.Instance.Resolve<INpcController>().getTemplate(71062);
                    new L1FollowerInstance(l1npc, this, pc);
                    pc.sendPackets(new S_NPCTalkReturn(Id, ""));
                }
                else if ((npcId == 71075) && (pc.Quest.get_step(L1Quest.QUEST_LIZARD) == 1))
                {
                    L1Npc l1npc = Container.Instance.Resolve<INpcController>().getTemplate(71075);
                    new L1FollowerInstance(l1npc, this, pc);
                    pc.sendPackets(new S_NPCTalkReturn(Id, ""));
                }
                else if ((npcId == 70957) || (npcId == 81209))
                {
                    L1Npc l1npc = Container.Instance.Resolve<INpcController>().getTemplate(70957);
                    new L1FollowerInstance(l1npc, this, pc);
                    pc.sendPackets(new S_NPCTalkReturn(Id, ""));
                }
                else if ((npcId == 81350) && (pc.Quest.get_step(4) == 3))
                {
                    L1Npc l1npc = Container.Instance.Resolve<INpcController>().getTemplate(81350);
                    new L1FollowerInstance(l1npc, this, pc);
                    pc.sendPackets(new S_NPCTalkReturn(Id, ""));
                }

            }
        }

        private const int REST_MILLISEC = 10000;

        private RestMonitor _monitor;

        public class RestMonitor : TimerTask
        {
            private readonly L1QuestInstance outerInstance;

            public RestMonitor(L1QuestInstance outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public override void run()
            {
                outerInstance.Rest = false;
            }
        }

    }

}