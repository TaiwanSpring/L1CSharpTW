using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LineageServer.Server.DataTables
{
	class UBTable
	{
		private readonly static IDataSource ubSettingDataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.UbSettings);

		private readonly static IDataSource ubManagersDataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.UbManagers);

		private readonly static IDataSource ubTimesDataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.UbTimes);

		private static UBTable _instance = new UBTable();

		private IDictionary<int, L1UltimateBattle> _ub = MapFactory.NewMap<int, L1UltimateBattle>();

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
			IList<IDataSourceRow> dataSourceRows = ubSettingDataSource.Select().Query();

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				L1UltimateBattle ub = new L1UltimateBattle();
				ub.UbId = dataSourceRow.getInt(UbSettings.Column_ub_id);
				ub.MapId = dataSourceRow.getShort(UbSettings.Column_ub_mapid);
				ub.LocX1 = dataSourceRow.getInt(UbSettings.Column_ub_area_x1);
				ub.LocY1 = dataSourceRow.getInt(UbSettings.Column_ub_area_y1);
				ub.LocX2 = dataSourceRow.getInt(UbSettings.Column_ub_area_x2);
				ub.LocY2 = dataSourceRow.getInt(UbSettings.Column_ub_area_y2);
				ub.MinLevel = dataSourceRow.getInt(UbSettings.Column_min_lvl);
				ub.MaxLevel = dataSourceRow.getInt(UbSettings.Column_max_lvl);
				ub.MaxPlayer = dataSourceRow.getInt(UbSettings.Column_max_player);
				ub.EnterRoyal = dataSourceRow.getBoolean(UbSettings.Column_enter_royal);
				ub.EnterKnight = dataSourceRow.getBoolean(UbSettings.Column_enter_knight);
				ub.EnterMage = dataSourceRow.getBoolean(UbSettings.Column_enter_mage);
				ub.EnterElf = dataSourceRow.getBoolean(UbSettings.Column_enter_elf);
				ub.EnterDarkelf = dataSourceRow.getBoolean(UbSettings.Column_enter_darkelf);
				ub.EnterDragonKnight = dataSourceRow.getBoolean(UbSettings.Column_enter_dragonknight);
				ub.EnterIllusionist = dataSourceRow.getBoolean(UbSettings.Column_enter_illusionist);
				ub.EnterMale = dataSourceRow.getBoolean(UbSettings.Column_enter_male);
				ub.EnterFemale = dataSourceRow.getBoolean(UbSettings.Column_enter_female);
				ub.UsePot = dataSourceRow.getBoolean(UbSettings.Column_use_pot);
				ub.Hpr = dataSourceRow.getInt(UbSettings.Column_hpr_bonus);
				ub.Mpr = dataSourceRow.getInt(UbSettings.Column_mpr_bonus);
				ub.resetLoc();

				_ub[ub.UbId] = ub;
			}

			dataSourceRows = ubManagersDataSource.Select().Query();

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				L1UltimateBattle ub = getUb(dataSourceRow.getInt(UbManagers.Column_ub_id));
				if (ub != null)
				{
					ub.addManager(dataSourceRow.getInt(UbManagers.Column_ub_manager_npc_id));
				}
			}
			dataSourceRows = ubTimesDataSource.Select().Query();

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				L1UltimateBattle ub = getUb(dataSourceRow.getInt(UbTimes.Column_ub_id));
				if (ub != null)
				{
					ub.addUbTime(dataSourceRow.getInt(UbTimes.Column_ub_time));
				}
			}
		}

		public virtual L1UltimateBattle getUb(int ubId)
		{
			return _ub[ubId];
		}

		public virtual ICollection<L1UltimateBattle> AllUb
		{
			get
			{
				return _ub.Values.ToArray();
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
	}

}