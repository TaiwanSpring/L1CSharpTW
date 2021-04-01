using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
	class PolyTable
	{
		private readonly static IDataSource dataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.Polymorphs);

		private static PolyTable _instance;

		private readonly IDictionary<string, L1PolyMorph> _polymorphs = MapFactory.NewMap<string, L1PolyMorph>();

		private readonly IDictionary<int, L1PolyMorph> _polyIdIndex = MapFactory.NewMap<int, L1PolyMorph>();

		public static PolyTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new PolyTable();
				}
				return _instance;
			}
		}

		private PolyTable()
		{
			loadPolymorphs();
		}

		private void loadPolymorphs()
		{
			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				fillPolyTable(dataSourceRow);
			}
		}
		private void fillPolyTable(IDataSourceRow dataSourceRow)
		{

			int id = dataSourceRow.getInt(Polymorphs.Column_id);
			string name = dataSourceRow.getString(Polymorphs.Column_name);
			int polyId = dataSourceRow.getInt(Polymorphs.Column_polyid);
			int minLevel = dataSourceRow.getInt(Polymorphs.Column_minlevel);
			int weaponEquipFlg = dataSourceRow.getInt(Polymorphs.Column_weaponequip);
			int armorEquipFlg = dataSourceRow.getInt(Polymorphs.Column_armorequip);
			bool canUseSkill = dataSourceRow.getBoolean(Polymorphs.Column_isSkillUse);
			int causeFlg = dataSourceRow.getInt(Polymorphs.Column_cause);

			L1PolyMorph poly = new L1PolyMorph(id, name, polyId, minLevel, weaponEquipFlg, armorEquipFlg, canUseSkill, causeFlg);

			_polymorphs[name] = poly;
			_polyIdIndex[polyId] = poly;

		}

		public virtual L1PolyMorph getTemplate(string name)
		{
			return _polymorphs[name];
		}

		public virtual L1PolyMorph getTemplate(int polyId)
		{
			return _polyIdIndex[polyId];
		}

	}

}