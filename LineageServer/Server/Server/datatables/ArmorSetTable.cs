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
namespace LineageServer.Server.Server.datatables
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1ArmorSets = LineageServer.Server.Server.Templates.L1ArmorSets;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;

	public class ArmorSetTable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(ArmorSetTable).FullName);

		private static ArmorSetTable _instance;

		private readonly IList<L1ArmorSets> _armorSetList = Lists.newList();

		public static ArmorSetTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ArmorSetTable();
				}
				return _instance;
			}
		}

		private ArmorSetTable()
		{
			load();
		}

		private void load()
		{
			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM armor_set");
				rs = pstm.executeQuery();
				fillTable(rs);
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, "error while creating armor_set table", e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private void fillTable(java.sql.ResultSet rs) throws java.sql.SQLException
		private void fillTable(ResultSet rs)
		{
			while (rs.next())
			{
				L1ArmorSets @as = new L1ArmorSets();
				@as.Id = rs.getInt("id");
				@as.Sets = rs.getString("sets");
				@as.PolyId = rs.getInt("polyid");
				@as.Ac = rs.getInt("ac");
				@as.Hp = rs.getInt("hp");
				@as.Mp = rs.getInt("mp");
				@as.Hpr = rs.getInt("hpr");
				@as.Mpr = rs.getInt("mpr");
				@as.Mr = rs.getInt("mr");
				@as.Str = rs.getInt("str");
				@as.Dex = rs.getInt("dex");
				@as.Con = rs.getInt("con");
				@as.Wis = rs.getInt("wis");
				@as.Cha = rs.getInt("cha");
				@as.Intl = rs.getInt("intl");
				@as.HitModifier = rs.getInt("hit_modifier");
				@as.DmgModifier = rs.getInt("dmg_modifier");
				@as.BowHitModifier = rs.getInt("bow_hit_modifier");
				@as.BowDmgModifier = rs.getInt("bow_dmg_modifier");
				@as.Sp = rs.getInt("sp");
				@as.DefenseWater = rs.getInt("defense_water");
				@as.DefenseWind = rs.getInt("defense_wind");
				@as.DefenseFire = rs.getInt("defense_fire");
				@as.DefenseEarth = rs.getInt("defense_earth");

				_armorSetList.Add(@as);
			}
		}

		public virtual L1ArmorSets[] AllList
		{
			get
			{
				return ((List<L1ArmorSets>)_armorSetList).ToArray();
			}
		}

	}

}