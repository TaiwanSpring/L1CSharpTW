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
namespace LineageServer.Server.Server.DataSources
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1UltimateBattle = LineageServer.Server.Server.Model.L1UltimateBattle;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	public class UBTable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(UBTable).FullName);

		private static UBTable _instance = new UBTable();

		private IDictionary<int, L1UltimateBattle> _ub = Maps.newMap();

		public static UBTable Instance
		{
			get
			{
				return _instance;
			}
		}

		private UBTable()
		{
			loadTable();
		}

		private void loadTable()
		{

			java.sql.IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM ub_settings");
				rs = pstm.executeQuery();

				while (rs.next())
				{

					L1UltimateBattle ub = new L1UltimateBattle();
					ub.UbId = rs.getInt("ub_id");
					ub.MapId = rs.getShort("ub_mapid");
					ub.LocX1 = rs.getInt("ub_area_x1");
					ub.LocY1 = rs.getInt("ub_area_y1");
					ub.LocX2 = rs.getInt("ub_area_x2");
					ub.LocY2 = rs.getInt("ub_area_y2");
					ub.MinLevel = rs.getInt("min_lvl");
					ub.MaxLevel = rs.getInt("max_lvl");
					ub.MaxPlayer = rs.getInt("max_player");
					ub.EnterRoyal = rs.getBoolean("enter_royal");
					ub.EnterKnight = rs.getBoolean("enter_knight");
					ub.EnterMage = rs.getBoolean("enter_mage");
					ub.EnterElf = rs.getBoolean("enter_elf");
					ub.EnterDarkelf = rs.getBoolean("enter_darkelf");
					ub.EnterDragonKnight = rs.getBoolean("enter_dragonknight");
					ub.EnterIllusionist = rs.getBoolean("enter_illusionist");
					ub.EnterMale = rs.getBoolean("enter_male");
					ub.EnterFemale = rs.getBoolean("enter_female");
					ub.UsePot = rs.getBoolean("use_pot");
					ub.Hpr = rs.getInt("hpr_bonus");
					ub.Mpr = rs.getInt("mpr_bonus");
					ub.resetLoc();

					_ub[ub.UbId] = ub;
				}
			}
			catch (SQLException e)
			{
				_log.warning("ubsettings couldnt be initialized:" + e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
			}

			// ub_managers load
			try
			{
				pstm = con.prepareStatement("SELECT * FROM ub_managers");
				rs = pstm.executeQuery();

				while (rs.next())
				{
					L1UltimateBattle ub = getUb(rs.getInt("ub_id"));
					if (ub != null)
					{
						ub.addManager(rs.getInt("ub_manager_npc_id"));
					}
				}
			}
			catch (SQLException e)
			{
				_log.warning("ub_managers couldnt be initialized:" + e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
			}

			// ub_times load
			try
			{
				pstm = con.prepareStatement("SELECT * FROM ub_times");
				rs = pstm.executeQuery();

				while (rs.next())
				{
					L1UltimateBattle ub = getUb(rs.getInt("ub_id"));
					if (ub != null)
					{
						ub.addUbTime(rs.getInt("ub_time"));
					}
				}
			}
			catch (SQLException e)
			{
				_log.warning("ub_times couldnt be initialized:" + e);
			}
			finally
			{
				SQLUtil.close(rs, pstm, con);
			}
			_log.config("UBリスト " + _ub.Count + "件ロード");
		}

		public virtual L1UltimateBattle getUb(int ubId)
		{
			return _ub[ubId];
		}

		public virtual ICollection<L1UltimateBattle> AllUb
		{
			get
			{
				return Collections.unmodifiableCollection(_ub.Values);
			}
		}

		public virtual L1UltimateBattle getUbForNpcId(int npcId)
		{
			foreach (L1UltimateBattle ub in _ub.Values)
			{
				if (ub.containsManager(npcId))
				{
					return ub;
				}
			}
			return null;
		}

		/// <summary>
		/// 指定されたUBIDに対するパターンの最大数を返す。
		/// </summary>
		/// <param name="ubId">
		///            調べるUBID。 </param>
		/// <returns> パターンの最大数。 </returns>
		public virtual int getMaxPattern(int ubId)
		{
			int n = 0;
			java.sql.IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;

			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT MAX(pattern) FROM spawnlist_ub WHERE ub_id=?");
				pstm.setInt(1, ubId);
				rs = pstm.executeQuery();
				if (rs.next())
				{
					n = rs.getInt(1);
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
			return n;
		}

	}

}