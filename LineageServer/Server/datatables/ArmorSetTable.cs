using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.DataSources
{
	public class ArmorSetTable
	{
		private static ArmorSetTable _instance;

		private readonly IList<L1ArmorSets> _armorSetList = ListFactory.NewList<L1ArmorSets>();

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
			IDataSourceTable dataSourceTable = new EmptyDataSource();
			fillTable(dataSourceTable.Select());
		}
		private void fillTable(IDataSourceRow[] dataSourceRows)
		{
			foreach (IDataSourceRow rs in dataSourceRows)
			{
				L1ArmorSets l1ArmorSets = new L1ArmorSets();
				l1ArmorSets.Id = dataSourceRow.getInt("id");
				l1ArmorSets.Sets = dataSourceRow.getString("sets");
				l1ArmorSets.PolyId = dataSourceRow.getInt("polyid");
				l1ArmorSets.Ac = dataSourceRow.getInt("ac");
				l1ArmorSets.Hp = dataSourceRow.getInt("hp");
				l1ArmorSets.Mp = dataSourceRow.getInt("mp");
				l1ArmorSets.Hpr = dataSourceRow.getInt("hpr");
				l1ArmorSets.Mpr = dataSourceRow.getInt("mpr");
				l1ArmorSets.Mr = dataSourceRow.getInt("mr");
				l1ArmorSets.Str = dataSourceRow.getInt("str");
				l1ArmorSets.Dex = dataSourceRow.getInt("dex");
				l1ArmorSets.Con = dataSourceRow.getInt("con");
				l1ArmorSets.Wis = dataSourceRow.getInt("wis");
				l1ArmorSets.Cha = dataSourceRow.getInt("cha");
				l1ArmorSets.Intl = dataSourceRow.getInt("intl");
				l1ArmorSets.HitModifier = dataSourceRow.getInt("hit_modifier");
				l1ArmorSets.DmgModifier = dataSourceRow.getInt("dmg_modifier");
				l1ArmorSets.BowHitModifier = dataSourceRow.getInt("bow_hit_modifier");
				l1ArmorSets.BowDmgModifier = dataSourceRow.getInt("bow_dmg_modifier");
				l1ArmorSets.Sp = dataSourceRow.getInt("sp");
				l1ArmorSets.DefenseWater = dataSourceRow.getInt("defense_water");
				l1ArmorSets.DefenseWind = dataSourceRow.getInt("defense_wind");
				l1ArmorSets.DefenseFire = dataSourceRow.getInt("defense_fire");
				l1ArmorSets.DefenseEarth = dataSourceRow.getInt("defense_earth");
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