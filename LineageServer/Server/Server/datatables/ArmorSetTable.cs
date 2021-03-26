using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Server.Templates;
using LineageServer.Server.Server.utils.collections;
using System.Collections.Generic;
namespace LineageServer.Server.Server.DataSources
{
	public class ArmorSetTable
	{
		private static ArmorSetTable _instance;

		private readonly IList<L1ArmorSets> _armorSetList = Lists.newList<L1ArmorSets>();

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
			IDataSourceTable dataSourceTable = new EmptyDataSourceTable();
			fillTable(dataSourceTable.Select());
		}
		private void fillTable(IDataSourceRow[] dataSourceRows)
		{
			foreach (IDataSourceRow rs in dataSourceRows)
			{
				L1ArmorSets l1ArmorSets = new L1ArmorSets();
				l1ArmorSets.Id = rs.getInt("id");
				l1ArmorSets.Sets = rs.getString("sets");
				l1ArmorSets.PolyId = rs.getInt("polyid");
				l1ArmorSets.Ac = rs.getInt("ac");
				l1ArmorSets.Hp = rs.getInt("hp");
				l1ArmorSets.Mp = rs.getInt("mp");
				l1ArmorSets.Hpr = rs.getInt("hpr");
				l1ArmorSets.Mpr = rs.getInt("mpr");
				l1ArmorSets.Mr = rs.getInt("mr");
				l1ArmorSets.Str = rs.getInt("str");
				l1ArmorSets.Dex = rs.getInt("dex");
				l1ArmorSets.Con = rs.getInt("con");
				l1ArmorSets.Wis = rs.getInt("wis");
				l1ArmorSets.Cha = rs.getInt("cha");
				l1ArmorSets.Intl = rs.getInt("intl");
				l1ArmorSets.HitModifier = rs.getInt("hit_modifier");
				l1ArmorSets.DmgModifier = rs.getInt("dmg_modifier");
				l1ArmorSets.BowHitModifier = rs.getInt("bow_hit_modifier");
				l1ArmorSets.BowDmgModifier = rs.getInt("bow_dmg_modifier");
				l1ArmorSets.Sp = rs.getInt("sp");
				l1ArmorSets.DefenseWater = rs.getInt("defense_water");
				l1ArmorSets.DefenseWind = rs.getInt("defense_wind");
				l1ArmorSets.DefenseFire = rs.getInt("defense_fire");
				l1ArmorSets.DefenseEarth = rs.getInt("defense_earth");
				_armorSetList.Add(l1ArmorSets);
			}
		}

		public virtual L1ArmorSets[] AllList
		{
			get
			{
				return ( (List<L1ArmorSets>)_armorSetList ).ToArray();
			}
		}

	}

}