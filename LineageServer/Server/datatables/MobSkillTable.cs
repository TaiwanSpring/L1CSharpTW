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
	using L1MobSkill = LineageServer.Server.Templates.L1MobSkill;
	using SQLUtil = LineageServer.Utils.SQLUtil;
	using MapFactory = LineageServer.Utils.MapFactory;

	public class MobSkillTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(MobSkillTable).FullName);

		private readonly bool _initialized;

		private static MobSkillTable _instance;

		private readonly IDictionary<int, L1MobSkill> _mobskills;

		public static MobSkillTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new MobSkillTable();
				}
				return _instance;
			}
		}

		public virtual bool Initialized
		{
			get
			{
				return _initialized;
			}
		}

		private MobSkillTable()
		{
			_mobskills = MapFactory.newMap();
			loadMobSkillData();
			_initialized = true;
		}

		private void loadMobSkillData()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm1 = null;
			PreparedStatement pstm2 = null;
			ResultSet rs1 = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm1 = con.prepareStatement("SELECT mobid,count(*) as cnt FROM mobskill group by mobid");

				int count = 0;
				int mobid = 0;
				int actNo = 0;

				pstm2 = con.prepareStatement("SELECT * FROM mobskill where mobid = ? order by mobid,actNo");

				for (rs1 = pstm1.executeQuery(); rs1.next();)
				{
					mobid = rs1.getInt("mobid");
					count = rs1.getInt("cnt");

					ResultSet rs2 = null;

					try
					{
						pstm2.setInt(1, mobid);
						L1MobSkill mobskill = new L1MobSkill(count);
						mobskill.set_mobid(mobid);

						rs2 = pstm2.executeQuery();
						while (rs2.next())
						{
							actNo = rs2.getInt("actNo");
							mobskill.MobName = rs2.getString("mobname");
							mobskill.setType(actNo, rs2.getInt("type"));
							mobskill.setMpConsume(actNo, rs2.getInt("mpConsume"));
							mobskill.setTriggerRandom(actNo, rs2.getInt("TriRnd"));
							mobskill.setTriggerHp(actNo, rs2.getInt("TriHp"));
							mobskill.setTriggerCompanionHp(actNo, rs2.getInt("TriCompanionHp"));
							mobskill.setTriggerRange(actNo, rs2.getInt("TriRange"));
							mobskill.setTriggerCount(actNo, rs2.getInt("TriCount"));
							mobskill.setChangeTarget(actNo, rs2.getInt("ChangeTarget"));
							mobskill.setRange(actNo, rs2.getInt("Range"));
							mobskill.setAreaWidth(actNo, rs2.getInt("AreaWidth"));
							mobskill.setAreaHeight(actNo, rs2.getInt("AreaHeight"));
							mobskill.setLeverage(actNo, rs2.getInt("Leverage"));
							mobskill.setSkillId(actNo, rs2.getInt("SkillId"));
							mobskill.setSkillArea(actNo, rs2.getInt("SkillArea"));
							mobskill.setGfxid(actNo, rs2.getInt("Gfxid"));
							mobskill.setActid(actNo, rs2.getInt("Actid"));
							mobskill.setSummon(actNo, rs2.getInt("SummonId"));
							mobskill.setSummonMin(actNo, rs2.getInt("SummonMin"));
							mobskill.setSummonMax(actNo, rs2.getInt("SummonMax"));
							mobskill.setPolyId(actNo, rs2.getInt("PolyId"));
						}

						_mobskills[mobid] = mobskill;
					}
					catch (SQLException e1)
					{
						_log.log(Enum.Level.Server, e1.LocalizedMessage, e1);

					}
					finally
					{
						SQLUtil.close(rs2);
					}
				}

			}
			catch (SQLException e2)
			{
				_log.log(Enum.Level.Server, "error while creating mobskill table", e2);

			}
			finally
			{
				SQLUtil.close(rs1);
				SQLUtil.close(pstm1);
				SQLUtil.close(pstm2);
				SQLUtil.close(con);
			}
		}

		public virtual L1MobSkill getTemplate(int id)
		{
			return _mobskills[id];
		}
	}

}