using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.Model.Instance
{
    class L1ScarecrowInstance : L1NpcInstance
    {

        private const long serialVersionUID = 1L;

        public L1ScarecrowInstance(L1Npc template) : base(template)
        {
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

        public override void receiveDamage(L1Character attacker, int damage)
        {
            if ((CurrentHp > 0) && !Dead)
            {
                if (damage > 0)
                {
                    if (Heading < 7)
                    {
                        Heading = Heading + 1;
                    }
                    else
                    {
                        Heading = 0;
                    }
                    broadcastPacket(new S_ChangeHeading(this));

                    if ((attacker is L1PcInstance))
                    {
                        L1PcInstance pc = (L1PcInstance)attacker;
                        pc.PetTarget = this;

                        if (pc.Level < 5)
                        {
                            IList<L1Character> targetList = ListFactory.NewList<L1Character>();
                            targetList.Add(pc);
                            IList<int> hateList = ListFactory.NewList<int>();
                            hateList.Add(1);
                            CalcExp.calcExp(pc, Id, targetList, hateList, Exp);
                        }
                    }
                }
            }
        }

        public override void onTalkAction(L1PcInstance l1pcinstance)
        {

        }

        public virtual void onFinalAction()
        {

        }

        public virtual void doFinalAction()
        {
        }
    }

}