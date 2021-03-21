using System;
using System.Collections.Generic;

/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Server.Model.Instance
{

	using L1Attack = LineageServer.Server.Server.Model.L1Attack;
	using L1Character = LineageServer.Server.Server.Model.L1Character;
	using S_ChangeHeading = LineageServer.Server.Server.serverpackets.S_ChangeHeading;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;
	using CalcExp = LineageServer.Server.Server.utils.CalcExp;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;

	[Serializable]
	public class L1ScarecrowInstance : L1NpcInstance
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
						L1PcInstance pc = (L1PcInstance) attacker;
						pc.PetTarget = this;

						if (pc.Level < 5)
						{
							IList<L1Character> targetList = Lists.newList();
							targetList.Add(pc);
							IList<int> hateList = Lists.newList();
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