using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
	class PetTypeTable
	{
		private readonly static IDataSource dataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.Pettypes);

		private static PetTypeTable _instance;

		private IDictionary<int, L1PetType> _types = MapFactory.NewMap<int, L1PetType>();

		private ISet<string> _defaultNames = new HashSet<string>();

		public static void load()
		{
			_instance = new PetTypeTable();
		}

		public static PetTypeTable Instance
		{
			get
			{
				return _instance;
			}
		}

		private PetTypeTable()
		{
			loadTypes();
		}

		private void loadTypes()
		{
			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				int baseNpcId = dataSourceRow.getInt(Pettypes.Column_BaseNpcId);
				string name = dataSourceRow.getString(Pettypes.Column_Name);
				int itemIdForTaming = dataSourceRow.getInt(Pettypes.Column_ItemIdForTaming);
				int hpUpMin = dataSourceRow.getInt(Pettypes.Column_HpUpMin);
				int hpUpMax = dataSourceRow.getInt(Pettypes.Column_HpUpMax);
				int mpUpMin = dataSourceRow.getInt(Pettypes.Column_MpUpMin);
				int mpUpMax = dataSourceRow.getInt(Pettypes.Column_MpUpMax);
				int evolvItemId = dataSourceRow.getInt(Pettypes.Column_EvolvItemId);
				int npcIdForEvolving = dataSourceRow.getInt(Pettypes.Column_NpcIdForEvolving);
				int[] msgIds = new int[5];
				for (int j = 0; j < 5; j++)
				{
					msgIds[j] = dataSourceRow.getInt($"MessageId{ j + 1 }");
				}
				int defyMsgId = dataSourceRow.getInt(Pettypes.Column_DefyMessageId);
				bool canUseEquipment = dataSourceRow.getBoolean(Pettypes.Column_canUseEquipment);
				IntRange hpUpRange = new IntRange(hpUpMin, hpUpMax);
				IntRange mpUpRange = new IntRange(mpUpMin, mpUpMax);
				_types[baseNpcId] = new L1PetType(baseNpcId, name, itemIdForTaming, hpUpRange, mpUpRange, evolvItemId, npcIdForEvolving, msgIds, defyMsgId, canUseEquipment);
				_defaultNames.Add(name.ToLower());
			}
		}

		public virtual L1PetType get(int baseNpcId)
		{
			return _types[baseNpcId];
		}

		public virtual bool isNameDefault(string name)
		{
			return _defaultNames.Contains(name.ToLower());
		}
	}

}