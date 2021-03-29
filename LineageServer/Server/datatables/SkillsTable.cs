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
namespace LineageServer.Server.DataTables
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1World = LineageServer.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using L1Skills = LineageServer.Server.Templates.L1Skills;
	using SQLUtil = LineageServer.Utils.SQLUtil;
	using MapFactory = LineageServer.Utils.MapFactory;

	public class SkillsTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(SkillsTable).FullName);

		private static SkillsTable _instance;

		private readonly IDictionary<int, L1Skills> _skills = MapFactory.newMap();

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
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM skills");
				rs = pstm.executeQuery();
				FillSkillsTable(rs);

			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, "error while creating skills table", e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private void FillSkillsTable(java.sql.ResultSet rs) throws java.sql.SQLException
		private void FillSkillsTable(ResultSet rs)
		{

			while (rs.next())
			{
				L1Skills l1skills = new L1Skills();
				int skill_id = dataSourceRow.getInt("skill_id");
				l1skills.SkillId = skill_id;
				l1skills.Name = dataSourceRow.getString("name");
				l1skills.SkillLevel = dataSourceRow.getInt("skill_level");
				l1skills.SkillNumber = dataSourceRow.getInt("skill_number");
				l1skills.MpConsume = dataSourceRow.getInt("mpConsume");
				l1skills.HpConsume = dataSourceRow.getInt("hpConsume");
				l1skills.ItemConsumeId = dataSourceRow.getInt("itemConsumeId");
				l1skills.ItemConsumeCount = dataSourceRow.getInt("itemConsumeCount");
				l1skills.ReuseDelay = dataSourceRow.getInt("reuseDelay");
				l1skills.BuffDuration = dataSourceRow.getInt("buffDuration");
				l1skills.Target = dataSourceRow.getString("target");
				l1skills.TargetTo = dataSourceRow.getInt("target_to");
				l1skills.DamageValue = dataSourceRow.getInt("damage_value");
				l1skills.DamageDice = dataSourceRow.getInt("damage_dice");
				l1skills.DamageDiceCount = dataSourceRow.getInt("damage_dice_count");
				l1skills.ProbabilityValue = dataSourceRow.getInt("probability_value");
				l1skills.ProbabilityDice = dataSourceRow.getInt("probability_dice");
				l1skills.Attr = dataSourceRow.getInt("attr");
				l1skills.Type = dataSourceRow.getInt("type");
				l1skills.Lawful = dataSourceRow.getInt("lawful");
				l1skills.Ranged = dataSourceRow.getInt("ranged");
				l1skills.Area = dataSourceRow.getInt("area");
				l1skills.Through = dataSourceRow.getBoolean("through");
				l1skills.Id = dataSourceRow.getInt("id");
				l1skills.NameId = dataSourceRow.getString("nameid");
				l1skills.ActionId = dataSourceRow.getInt("action_id");
				l1skills.CastGfx = dataSourceRow.getInt("castgfx");
				l1skills.CastGfx2 = dataSourceRow.getInt("castgfx2");
				l1skills.SysmsgIdHappen = dataSourceRow.getInt("sysmsgID_happen");
				l1skills.SysmsgIdStop = dataSourceRow.getInt("sysmsgID_stop");
				l1skills.SysmsgIdFail = dataSourceRow.getInt("sysmsgID_fail");

				_skills[skill_id] = l1skills;
			}
			_log.config("スキル " + _skills.Count + "件ロード");
		}

		public virtual void spellMastery(int playerobjid, int skillid, string skillname, int active, int time)
		{
			if (spellCheck(playerobjid, skillid))
			{
				return;
			}
			L1PcInstance pc = (L1PcInstance) L1World.Instance.findObject(playerobjid);
			if (pc != null)
			{
				pc.SkillMastery = skillid;
			}

			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("INSERT INTO character_skills SET char_obj_id=?, skill_id=?, skill_name=?, is_active=?, activetimeleft=?");
				pstm.setInt(1, playerobjid);
				pstm.setInt(2, skillid);
				pstm.setString(3, skillname);
				pstm.setInt(4, active);
				pstm.setInt(5, time);
				pstm.execute();
			}
			catch (Exception e)
			{
				_log.Error(e);

			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		public virtual void spellLost(int playerobjid, int skillid)
		{
			L1PcInstance pc = (L1PcInstance) L1World.Instance.findObject(playerobjid);
			if (pc != null)
			{
				pc.removeSkillMastery(skillid);
			}

			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("DELETE FROM character_skills WHERE char_obj_id=? AND skill_id=?");
				pstm.setInt(1, playerobjid);
				pstm.setInt(2, skillid);
				pstm.execute();
			}
			catch (Exception e)
			{
				_log.Error(e);

			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		public virtual bool spellCheck(int playerobjid, int skillid)
		{
			bool ret = false;
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM character_skills WHERE char_obj_id=? AND skill_id=?");
				pstm.setInt(1, playerobjid);
				pstm.setInt(2, skillid);
				rs = pstm.executeQuery();
				if (rs.next())
				{
					ret = true;
				}
				else
				{
					ret = false;
				}
			}
			catch (Exception e)
			{
				_log.Error(e);

			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
			return ret;
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