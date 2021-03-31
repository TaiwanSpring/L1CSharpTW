using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;

namespace LineageServer.Server.DataTables
{
	class PetItemTable
	{
		private readonly static IDataSource dataSource =
			   Container.Instance.Resolve<IDataSourceFactory>()
			   .Factory(Enum.DataSourceTypeEnum.Petitem);

		private static PetItemTable _instance;

		private readonly IDictionary<int, L1PetItem> _petItemIdIndex = MapFactory.NewMap<int, L1PetItem>();

		private static readonly IDictionary<string, int> _useTypes = MapFactory.NewMap<string, int>();

		static PetItemTable()
		{
			_useTypes["armor"] = 0;
			_useTypes["tooth"] = 1;
		}

		public static PetItemTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new PetItemTable();
				}
				return _instance;
			}
		}

		private PetItemTable()
		{
			loadPetItem();
		}

		private void loadPetItem()
		{
			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();
			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				fillPetItemTable(dataSourceRow);
			}
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: private void fillPetItemTable(java.sql.ResultSet rs) throws java.sql.SQLException
		private void fillPetItemTable(IDataSourceRow dataSourceRow)
		{
			L1PetItem petItem = new L1PetItem();
			petItem.ItemId = dataSourceRow.getInt(Petitem.Column_item_id);
			petItem.UseType = _useTypes[dataSourceRow.getString(Petitem.Column_use_type)];
			petItem.HitModifier = dataSourceRow.getInt(Petitem.Column_hitmodifier);
			petItem.DamageModifier = dataSourceRow.getInt(Petitem.Column_dmgmodifier);
			petItem.AddAc = dataSourceRow.getInt(Petitem.Column_ac);
			petItem.AddStr = dataSourceRow.getInt(Petitem.Column_add_str);
			petItem.AddCon = dataSourceRow.getInt(Petitem.Column_add_con);
			petItem.AddDex = dataSourceRow.getInt(Petitem.Column_add_dex);
			petItem.AddInt = dataSourceRow.getInt(Petitem.Column_add_int);
			petItem.AddWis = dataSourceRow.getInt(Petitem.Column_add_wis);
			petItem.AddHp = dataSourceRow.getInt(Petitem.Column_add_hp);
			petItem.AddMp = dataSourceRow.getInt(Petitem.Column_add_mp);
			petItem.AddSp = dataSourceRow.getInt(Petitem.Column_add_sp);
			petItem.AddMr = dataSourceRow.getInt(Petitem.Column_m_def);
			_petItemIdIndex[petItem.ItemId] = petItem;
		}

		public virtual L1PetItem getTemplate(int itemId)
		{
			return _petItemIdIndex[itemId];
		}

	}

}