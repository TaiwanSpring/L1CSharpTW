using System;

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
namespace LineageServer.Server.Model
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.ABSOLUTE_BARRIER;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.EARTH_BIND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.FREEZING_BLIZZARD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.FREEZING_BREATH;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.ICE_LANCE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_CUBE_BALANCE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_CUBE_IGNITION_TO_ENEMY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_CUBE_QUAKE_TO_ENEMY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_CUBE_SHOCK_TO_ENEMY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_FREEZE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_MR_REDUCTION_BY_CUBE_SHOCK;


	using ActionCodes = LineageServer.Server.ActionCodes;
	using RunnableExecuter = LineageServer.Server.RunnableExecuter;
	using L1MonsterInstance = LineageServer.Server.Model.Instance.L1MonsterInstance;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using S_DoActionGFX = LineageServer.Serverpackets.S_DoActionGFX;
	using S_Paralysis = LineageServer.Serverpackets.S_Paralysis;

	public class L1Cube : TimerTask
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(L1Cube).FullName);

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: private java.util.concurrent.ScheduledFuture<?> _future = null;
		private ScheduledFuture<object> _future = null;

		private int _timeCounter = 0;

		private readonly L1Character _effect;

		private readonly L1Character _cha;

		private readonly int _skillId;

		public L1Cube(L1Character effect, L1Character cha, int skillId)
		{
			_effect = effect;
			_cha = cha;
			_skillId = skillId;
		}

		public override void run()
		{
			try
			{
				if (_cha.Dead)
				{
					stop();
					return;
				}
				if (!_cha.hasSkillEffect(_skillId))
				{
					stop();
					return;
				}
				_timeCounter++;
				giveEffect();
			}
			catch (Exception e)
			{
				_log.log(Level.WARNING, e.Message, e);
			}
		}

		public virtual void begin()
		{
			// 効果時間が8秒のため、4秒毎のスキルの場合処理時間を考慮すると実際には1回しか効果が現れない
			// よって開始時間を0.9秒後に設定しておく
			_future = RunnableExecuter.Instance.scheduleAtFixedRate(this, 900, 1000);
		}

		public virtual void stop()
		{
			if (_future != null)
			{
				_future.cancel(false);
			}
		}

		public virtual void giveEffect()
		{
			if (_skillId == STATUS_CUBE_IGNITION_TO_ENEMY)
			{
				if (_timeCounter % 4 != 0)
				{
					return;
				}
				if (_cha.hasSkillEffect(STATUS_FREEZE))
				{
					return;
				}
				if (_cha.hasSkillEffect(ABSOLUTE_BARRIER))
				{
					return;
				}
				if (_cha.hasSkillEffect(ICE_LANCE))
				{
					return;
				}
				if (_cha.hasSkillEffect(FREEZING_BLIZZARD))
				{
					return;
				}
				if (_cha.hasSkillEffect(FREEZING_BREATH))
				{
					return;
				}
				if (_cha.hasSkillEffect(EARTH_BIND))
				{
					return;
				}

				if (_cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) _cha;
					pc.sendPackets(new S_DoActionGFX(pc.Id, ActionCodes.ACTION_Damage));
					pc.broadcastPacket(new S_DoActionGFX(pc.Id, ActionCodes.ACTION_Damage));
					pc.receiveDamage(_effect, 10, false);
				}
				else if (_cha is L1MonsterInstance)
				{
					L1MonsterInstance mob = (L1MonsterInstance) _cha;
					mob.broadcastPacket(new S_DoActionGFX(mob.Id, ActionCodes.ACTION_Damage));
					mob.receiveDamage(_effect, 10);
				}
			}
			else if (_skillId == STATUS_CUBE_QUAKE_TO_ENEMY)
			{
				if (_timeCounter % 4 != 0)
				{
					return;
				}
				if (_cha.hasSkillEffect(STATUS_FREEZE))
				{
					return;
				}
				if (_cha.hasSkillEffect(ABSOLUTE_BARRIER))
				{
					return;
				}
				if (_cha.hasSkillEffect(ICE_LANCE))
				{
					return;
				}
				if (_cha.hasSkillEffect(FREEZING_BLIZZARD))
				{
					return;
				}
				if (_cha.hasSkillEffect(FREEZING_BREATH))
				{
					return;
				}
				if (_cha.hasSkillEffect(EARTH_BIND))
				{
					return;
				}

				if (_cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) _cha;
					pc.setSkillEffect(STATUS_FREEZE, 1000);
					pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_BIND, true));
				}
				else if (_cha is L1MonsterInstance)
				{
					L1MonsterInstance mob = (L1MonsterInstance) _cha;
					mob.setSkillEffect(STATUS_FREEZE, 1000);
					mob.Paralyzed = true;
				}
			}
			else if (_skillId == STATUS_CUBE_SHOCK_TO_ENEMY)
			{
				// if (_timeCounter % 5 != 0) {
				// return;
				// }
				// _cha.addMr(-10);
				// if (_cha instanceof L1PcInstance) {
				// L1PcInstance pc = (L1PcInstance) _cha;
				// pc.sendPackets(new S_SPMR(pc));
				// }
				_cha.setSkillEffect(STATUS_MR_REDUCTION_BY_CUBE_SHOCK, 4000);
			}
			else if (_skillId == STATUS_CUBE_BALANCE)
			{
				if (_timeCounter % 4 == 0)
				{
					int newMp = _cha.CurrentMp + 5;
					if (newMp < 0)
					{
						newMp = 0;
					}
					_cha.CurrentMp = newMp;
				}
				if (_timeCounter % 5 == 0)
				{
					if (_cha is L1PcInstance)
					{
						L1PcInstance pc = (L1PcInstance) _cha;
						pc.receiveDamage(_effect, 25, false);
					}
					else if (_cha is L1MonsterInstance)
					{
						L1MonsterInstance mob = (L1MonsterInstance) _cha;
						mob.receiveDamage(_effect, 25);
					}
				}
			}
		}

	}

}