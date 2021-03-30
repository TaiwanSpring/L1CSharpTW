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
namespace LineageServer.Server.Model
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using SQLUtil = LineageServer.Utils.SQLUtil;
	using MapFactory = LineageServer.Utils.MapFactory;

	public class L1Quest
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(L1Quest).FullName);

		public const int QUEST_LEVEL15 = 1;

		public const int QUEST_LEVEL30 = 2;

		public const int QUEST_LEVEL45 = 3;

		public const int QUEST_LEVEL50 = 4;

		public const int QUEST_LYRA = 10;

		public const int QUEST_OILSKINMANT = 11;

		public const int QUEST_DOROMOND = 20;

		public const int QUEST_RUBA = 21;

		public const int QUEST_AREX = 22;

		public const int QUEST_LUKEIN1 = 23;

		public const int QUEST_TBOX1 = 24;

		public const int QUEST_TBOX2 = 25;

		public const int QUEST_TBOX3 = 26;

		public const int QUEST_SIMIZZ = 27;

		public const int QUEST_DOIL = 28;

		public const int QUEST_RUDIAN = 29;

		public const int QUEST_RESTA = 30;

		public const int QUEST_CADMUS = 31;

		public const int QUEST_KAMYLA = 32;

		public const int QUEST_CRYSTAL = 33;

		public const int QUEST_LIZARD = 34;

		public const int QUEST_KEPLISHA = 35;

		public const int QUEST_DESIRE = 36;

		public const int QUEST_SHADOWS = 37;

		public const int QUEST_ROI = 38;

		public const int QUEST_TOSCROLL = 39;

		public const int QUEST_MOONOFLONGBOW = 40;

		public const int QUEST_GENERALHAMELOFRESENTMENT = 41;

		public const int QUEST_TUTOR = 300; //新手導師
		public const int QUEST_TUTOR2 = 304; //修練場管理員

		public const int QUEST_END = 255; // 終了済みクエストのステップ

		private L1PcInstance _owner = null;

		private IDictionary<int, int> _quest = null;

		public L1Quest(L1PcInstance owner)
		{
			_owner = owner;
		}

		public virtual L1PcInstance get_owner()
		{
			return _owner;
		}

		public virtual int get_step(int quest_id)
		{

			if (_quest == null)
			{

				IDataBaseConnection con = null;
				PreparedStatement pstm = null;
				ResultSet rs = null;
				try
				{
					_quest = MapFactory.NewMap();

					con = L1DatabaseFactory.Instance.Connection;
					pstm = con.prepareStatement("SELECT * FROM character_quests WHERE char_id=?");
					pstm.setInt(1, _owner.Id);
					rs = pstm.executeQuery();

					while (rs.next())
					{
						_quest[dataSourceRow.getInt(2)] = dataSourceRow.getInt(3);
					}

				}
				catch (SQLException e)
				{
					_log.log(Enum.Level.Server, e.Message, e);
				}
				finally
				{
					SQLUtil.close(rs);
					SQLUtil.close(pstm);
					SQLUtil.close(con);
				}
			}
			int? step = _quest[quest_id];
			if (step == null)
			{
				return 0;
			}
			else
			{
				return step.Value;
			}
		}

		public virtual void set_step(int quest_id, int step)
		{

			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;

				if (_quest[quest_id] == null)
				{

					pstm = con.prepareStatement("INSERT INTO character_quests " + "SET char_id = ?, quest_id = ?, quest_step = ?");
					pstm.setInt(1, _owner.Id);
					pstm.setInt(2, quest_id);
					pstm.setInt(3, step);
					pstm.execute();
				}
				else
				{
					pstm = con.prepareStatement("UPDATE character_quests " + "SET quest_step = ? WHERE char_id = ? AND quest_id = ?");
					pstm.setInt(1, step);
					pstm.setInt(2, _owner.Id);
					pstm.setInt(3, quest_id);
					pstm.execute();
				}
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);

			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);

			}
			_quest[quest_id] = step;
		}

		public virtual void add_step(int quest_id, int add)
		{
			int step = get_step(quest_id);
			step += add;
			set_step(quest_id, step);
		}

		public virtual void set_end(int quest_id)
		{
			set_step(quest_id, QUEST_END);
		}

		public virtual bool isEnd(int quest_id)
		{
			if (get_step(quest_id) == QUEST_END)
			{
				return true;
			}
			return false;
		}

	}

}