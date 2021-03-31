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
			_pets[itemobjid] = l1pet;

			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("INSERT INTO pets SET item_obj_id=?,objid=?,npcid=?,name=?,lvl=?,hp=?,mp=?,exp=?,lawful=?,food=?");
				pstm.setInt(1, l1pet.get_itemobjid());
				pstm.setInt(2, l1pet.get_objid());
				pstm.setInt(3, l1pet.get_npcid());
				pstm.setString(4, l1pet.get_name());
				pstm.setInt(5, l1pet.get_level());
				pstm.setInt(6, l1pet.get_hp());
				pstm.setInt(7, l1pet.get_mp());
				pstm.setInt(8, l1pet.get_exp());
				pstm.setInt(9, l1pet.get_lawful());
				pstm.setInt(10, l1pet.get_food());
				pstm.execute();
			}
			catch (Exception e)
			{
				_log.Error(e);

			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);

			}
		}

		public virtual void storePet(L1Pet pet)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("UPDATE pets SET objid=?,npcid=?,name=?,lvl=?,hp=?,mp=?,exp=?,lawful=?,food=? WHERE item_obj_id=?");
				pstm.setInt(1, pet.get_objid());
				pstm.setInt(2, pet.get_npcid());
				pstm.setString(3, pet.get_name());
				pstm.setInt(4, pet.get_level());
				pstm.setInt(5, pet.get_hp());
				pstm.setInt(6, pet.get_mp());
				pstm.setInt(7, pet.get_exp());
				pstm.setInt(8, pet.get_lawful());
				pstm.setInt(9, pet.get_food());
				pstm.setInt(10, pet.get_itemobjid());
				pstm.execute();
			}
			catch (Exception e)
			{
				_log.Error(e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		/// <summary>
		/// 更新寵物飽食度 </summary>
		public virtual void storePetFood(L1Pet pet)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("UPDATE pets SET food=? WHERE item_obj_id=?");
				pstm.setInt(1, pet.get_food());
				pstm.setInt(2, pet.get_itemobjid());
				pstm.execute();
			}
			catch (Exception e)
			{
				_log.Error(e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		public virtual void deletePet(int itemobjid)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("DELETE FROM pets WHERE item_obj_id=?");
				pstm.setInt(1, itemobjid);
				pstm.execute();
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
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				/*
				 * 同じ名前を探す。MySQLはデフォルトでcase insensitiveなため
				 * 本来LOWERは必要ないが、binaryに変更された場合に備えて。
				 */
				pstm = con.prepareStatement("SELECT item_obj_id FROM pets WHERE LOWER(name)=?");
				pstm.setString(1, nameLower);
				rs = pstm.executeQuery();
				if (!rs.next())
				{ // 同じ名前が無かった
					return false;
				}
				if (PetTypeTable.Instance.isNameDefault(nameLower))
				{ // デフォルトの名前なら重複していないとみなす
					return false;
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
			return true;
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

			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("INSERT INTO pets SET item_obj_id=?,objid=?,npcid=?,name=?,lvl=?,hp=?,mp=?,exp=?,lawful=?,food=?");
				pstm.setInt(1, l1pet.get_itemobjid());
				pstm.setInt(2, l1pet.get_objid());
				pstm.setInt(3, l1pet.get_npcid());
				pstm.setString(4, l1pet.get_name());
				pstm.setInt(5, l1pet.get_level());
				pstm.setInt(6, l1pet.get_hp());
				pstm.setInt(7, l1pet.get_mp());
				pstm.setInt(8, l1pet.get_exp());
				pstm.setInt(9, l1pet.get_lawful());
				pstm.setInt(10, l1pet.get_food());
				pstm.execute();
			}
			catch (Exception e)
			{
				_log.Error(e);

			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);

			}
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