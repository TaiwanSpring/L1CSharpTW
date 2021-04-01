using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
namespace LineageServer.Server.DataTables
{
	class PetTable
	{
		private readonly static IDataSource dataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.Pets);
		private static PetTable _instance;

		private readonly IDictionary<int, L1Pet> _pets = MapFactory.NewMap<int, L1Pet>();

		public static PetTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new PetTable();
				}
				return _instance;
			}
		}

		private PetTable()
		{
			load();
		}

		private void load()
		{
			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();
			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				L1Pet pet = new L1Pet();
				int itemobjid = dataSourceRow.getInt(Pets.Column_item_obj_id);
				pet.set_itemobjid(itemobjid);
				pet.set_objid(dataSourceRow.getInt(Pets.Column_objid));
				pet.set_npcid(dataSourceRow.getInt(Pets.Column_npcid));
				pet.set_name(dataSourceRow.getString(Pets.Column_name));
				pet.set_level(dataSourceRow.getInt(Pets.Column_lvl));
				pet.set_hp(dataSourceRow.getInt(Pets.Column_hp));
				pet.set_mp(dataSourceRow.getInt(Pets.Column_mp));
				pet.set_exp(dataSourceRow.getInt(Pets.Column_exp));
				pet.set_lawful(dataSourceRow.getInt(Pets.Column_lawful));
				pet.set_food(dataSourceRow.getInt(Pets.Column_food));

				_pets[itemobjid] = pet;
			}
		}

		public virtual void storeNewPet(L1NpcInstance pet, int objid, int itemobjid)
		{
			// XXX 呼ばれる前と処理の重複
			L1Pet l1pet = new L1Pet();
			l1pet.set_itemobjid(itemobjid);
			l1pet.set_objid(objid);
			l1pet.set_npcid(pet.NpcTemplate.get_npcId());
			l1pet.set_name(pet.NpcTemplate.get_name());
			l1pet.set_level(pet.NpcTemplate.get_level());
			l1pet.set_hp(pet.MaxHp);
			l1pet.set_mp(pet.MaxMp);
			l1pet.set_exp(750); // Lv.5のEXP
			l1pet.set_lawful(0);
			l1pet.set_food(50);

			IDataSourceRow dataSourceRow = dataSource.NewRow();
			dataSourceRow.Insert()
			.Set(Pets.Column_item_obj_id, l1pet.get_itemobjid())
			.Set(Pets.Column_objid, l1pet.get_objid())
			.Set(Pets.Column_npcid, l1pet.get_npcid())
			.Set(Pets.Column_name, l1pet.get_name())
			.Set(Pets.Column_lvl, l1pet.get_level())
			.Set(Pets.Column_hp, l1pet.get_hp())
			.Set(Pets.Column_mp, l1pet.get_mp())
			.Set(Pets.Column_exp, l1pet.get_exp())
			.Set(Pets.Column_lawful, l1pet.get_lawful())
			.Set(Pets.Column_food, l1pet.get_food())
			.Execute();

			_pets[itemobjid] = l1pet;
		}

		public virtual void storePet(L1Pet pet)
		{
			IDataSourceRow dataSourceRow = dataSource.NewRow();
			dataSourceRow.Update()
			.Where(Pets.Column_item_obj_id, pet.get_itemobjid())
			.Set(Pets.Column_objid, pet.get_objid())
			.Set(Pets.Column_npcid, pet.get_npcid())
			.Set(Pets.Column_name, pet.get_name())
			.Set(Pets.Column_lvl, pet.get_level())
			.Set(Pets.Column_hp, pet.get_hp())
			.Set(Pets.Column_mp, pet.get_mp())
			.Set(Pets.Column_exp, pet.get_exp())
			.Set(Pets.Column_lawful, pet.get_lawful())
			.Set(Pets.Column_food, pet.get_food())
			.Execute();
		}

		/// <summary>
		/// 更新寵物飽食度 </summary>
		public virtual void storePetFood(L1Pet pet)
		{
			IDataSourceRow dataSourceRow = dataSource.NewRow();
			dataSourceRow.Update()
			.Where(Pets.Column_item_obj_id, pet.get_itemobjid())
			.Set(Pets.Column_food, pet.get_food())
			.Execute();
		}

		public virtual void deletePet(int itemobjid)
		{
			IDataSourceRow dataSourceRow = dataSource.NewRow();
			dataSourceRow.Delete()
			.Where(Pets.Column_item_obj_id, itemobjid)
			.Execute();
			_pets.Remove(itemobjid);
		}

		/// <summary>
		/// Petsテーブルに既に名前が存在するかを返す。
		/// </summary>
		/// <param name="nameCaseInsensitive">
		///            調べるペットの名前。大文字小文字の差異は無視される。 </param>
		/// <returns> 既に名前が存在すればtrue </returns>
		public static bool isNameExists(string nameCaseInsensitive)
		{
			string nameLower = nameCaseInsensitive.ToLower();

			if (PetTypeTable.Instance.isNameDefault(nameLower))
			{ // デフォルトの名前なら重複していないとみなす
				return false;
			}

			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query($"SELECT item_obj_id FROM pets WHERE LOWER(name)={nameCaseInsensitive}");
			return dataSourceRows.Count > 0;
		}

		/// <summary>
		/// 經過 C_NPCAction 獲得 pet
		/// </summary>
		public virtual void buyNewPet(int petNpcId, int objid, int itemobjid, int upLv, long lvExp)
		{
			L1PetType petType = PetTypeTable.Instance.get(petNpcId);
			L1Pet l1pet = new L1Pet();
			l1pet.set_itemobjid(itemobjid);
			l1pet.set_objid(objid);
			l1pet.set_npcid(petNpcId);
			l1pet.set_name(petType.Name);
			l1pet.set_level(upLv);
			int hpUpMin = petType.HpUpRange.Low;
			int hpUpMax = petType.HpUpRange.High;
			int mpUpMin = petType.MpUpRange.Low;
			int mpUpMax = petType.MpUpRange.High;
			short randomhp = (short)( ( hpUpMin + hpUpMax ) / 2 );
			short randommp = (short)( ( mpUpMin + mpUpMax ) / 2 );
			for (int i = 1; i < upLv; i++)
			{
				randomhp += (short)( RandomHelper.Next(hpUpMax - hpUpMin) + hpUpMin + 1 );
				randommp += (short)( RandomHelper.Next(mpUpMax - mpUpMin) + mpUpMin + 1 );
			}
			l1pet.set_hp(randomhp);
			l1pet.set_mp(randommp);
			l1pet.set_exp((int)lvExp); // upLv EXP
			l1pet.set_lawful(0);
			l1pet.set_food(50);
			_pets[itemobjid] = l1pet;

			IDataSourceRow dataSourceRow = dataSource.NewRow();
			dataSourceRow.Insert()
			.Set(Pets.Column_item_obj_id, l1pet.get_itemobjid())
			.Set(Pets.Column_objid, l1pet.get_objid())
			.Set(Pets.Column_npcid, l1pet.get_npcid())
			.Set(Pets.Column_name, l1pet.get_name())
			.Set(Pets.Column_lvl, l1pet.get_level())
			.Set(Pets.Column_hp, l1pet.get_hp())
			.Set(Pets.Column_mp, l1pet.get_mp())
			.Set(Pets.Column_exp, l1pet.get_exp())
			.Set(Pets.Column_lawful, l1pet.get_lawful())
			.Set(Pets.Column_food, l1pet.get_food())
			.Execute();
		}

		public virtual L1Pet getTemplate(int itemobjid)
		{
			return _pets[itemobjid];
		}

		public virtual L1Pet[] PetTableList
		{
			get
			{
				return _pets.Values.ToArray();
			}
		}
	}

}