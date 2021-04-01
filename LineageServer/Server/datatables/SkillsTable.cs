using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
	class SkillsTable
	{
		private readonly static IDataSource skillsDataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.Skills);

		private readonly static IDataSource characterSkillsDataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.CharacterSkills);

		private static SkillsTable _instance;

		private readonly IDictionary<int, L1Skills> _skills = MapFactory.NewMap<int, L1Skills>();

		private readonly bool _initialized;

		public static SkillsTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new SkillsTable();
				}
				return _instance;
			}
		}

		private SkillsTable()
		{
			_initialized = true;
			RestoreSkills();
		}

		private void RestoreSkills()
		{
			IList<IDataSourceRow> dataSourceRows = skillsDataSource.Select().Query();

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];

				FillSkillsTable(dataSourceRow);
			}
		}
		private void FillSkillsTable(IDataSourceRow dataSourceRow)
		{
			L1Skills l1skills = new L1Skills();
			int skill_id = dataSourceRow.getInt(Skills.Column_skill_id);
			l1skills.SkillId = skill_id;
			l1skills.Name = dataSourceRow.getString(Skills.Column_name);
			l1skills.SkillLevel = dataSourceRow.getInt(Skills.Column_skill_level);
			l1skills.SkillNumber = dataSourceRow.getInt(Skills.Column_skill_number);
			l1skills.MpConsume = dataSourceRow.getInt(Skills.Column_mpConsume);
			l1skills.HpConsume = dataSourceRow.getInt(Skills.Column_hpConsume);
			l1skills.ItemConsumeId = dataSourceRow.getInt(Skills.Column_itemConsumeId);
			l1skills.ItemConsumeCount = dataSourceRow.getInt(Skills.Column_itemConsumeCount);
			l1skills.ReuseDelay = dataSourceRow.getInt(Skills.Column_reuseDelay);
			l1skills.BuffDuration = dataSourceRow.getInt(Skills.Column_buffDuration);
			l1skills.Target = dataSourceRow.getString(Skills.Column_target);
			l1skills.TargetTo = dataSourceRow.getInt(Skills.Column_target_to);
			l1skills.DamageValue = dataSourceRow.getInt(Skills.Column_damage_value);
			l1skills.DamageDice = dataSourceRow.getInt(Skills.Column_damage_dice);
			l1skills.DamageDiceCount = dataSourceRow.getInt(Skills.Column_damage_dice_count);
			l1skills.ProbabilityValue = dataSourceRow.getInt(Skills.Column_probability_value);
			l1skills.ProbabilityDice = dataSourceRow.getInt(Skills.Column_probability_dice);
			l1skills.Attr = dataSourceRow.getInt(Skills.Column_attr);
			l1skills.Type = dataSourceRow.getInt(Skills.Column_type);
			l1skills.Lawful = dataSourceRow.getInt(Skills.Column_lawful);
			l1skills.Ranged = dataSourceRow.getInt(Skills.Column_ranged);
			l1skills.Area = dataSourceRow.getInt(Skills.Column_area);
			l1skills.Through = dataSourceRow.getBoolean(Skills.Column_through);
			l1skills.Id = dataSourceRow.getInt(Skills.Column_id);
			l1skills.NameId = dataSourceRow.getString(Skills.Column_nameid);
			l1skills.ActionId = dataSourceRow.getInt(Skills.Column_action_id);
			l1skills.CastGfx = dataSourceRow.getInt(Skills.Column_castgfx);
			l1skills.CastGfx2 = dataSourceRow.getInt(Skills.Column_castgfx2);
			l1skills.SysmsgIdHappen = dataSourceRow.getInt(Skills.Column_sysmsgID_happen);
			l1skills.SysmsgIdStop = dataSourceRow.getInt(Skills.Column_sysmsgID_stop);
			l1skills.SysmsgIdFail = dataSourceRow.getInt(Skills.Column_sysmsgID_fail);
			_skills[skill_id] = l1skills;
		}

		public virtual void spellMastery(int playerobjid, int skillid, string skillname, int active, int time)
		{
			if (spellCheck(playerobjid, skillid))
			{
				return;
			}

			L1PcInstance pc = (L1PcInstance)L1World.Instance.findObject(playerobjid);

			if (pc != null)
			{
				pc.SkillMastery = skillid;
				IDataSourceRow dataSourceRow = characterSkillsDataSource.NewRow();
				dataSourceRow.Insert()
				.Set(CharacterSkills.Column_char_obj_id, playerobjid)
				.Set(CharacterSkills.Column_skill_id, skillid)
				.Set(CharacterSkills.Column_skill_name, skillname)
				.Set(CharacterSkills.Column_is_active, active)
				.Set(CharacterSkills.Column_activetimeleft, time)
				.Execute();
			}
		}

		public virtual void spellLost(int playerobjid, int skillid)
		{
			L1PcInstance pc = (L1PcInstance)L1World.Instance.findObject(playerobjid);
			if (pc != null)
			{
				pc.removeSkillMastery(skillid);
				IDataSourceRow dataSourceRow = characterSkillsDataSource.NewRow();
				dataSourceRow.Delete()
				.Where(CharacterSkills.Column_char_obj_id, playerobjid)
				.Where(CharacterSkills.Column_skill_id, skillid)
				.Execute();
			}
		}

		public virtual bool spellCheck(int playerobjid, int skillid)
		{
			IDataSourceRow dataSourceRow = characterSkillsDataSource.NewRow();
			dataSourceRow.Select()
			.Where(CharacterSkills.Column_char_obj_id, playerobjid)
			.Where(CharacterSkills.Column_skill_id, skillid)
			.Execute();
			return dataSourceRow.HaveData;
		}

		public virtual bool Initialized
		{
			get
			{
				return _initialized;
			}
		}
		public virtual L1Skills getTemplate(int i)
		{
			return _skills[i];
		}
	}
}