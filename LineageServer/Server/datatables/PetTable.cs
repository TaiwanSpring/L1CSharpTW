﻿using System;
using System.Collections.Generic;
using System.Linq;

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
namespace LineageServer.Server.DataSources
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1NpcInstance = LineageServer.Server.Model.Instance.L1NpcInstance;
	using L1Pet = LineageServer.Server.Templates.L1Pet;
	using L1PetType = LineageServer.Server.Templates.L1PetType;
	using Random = LineageServer.Utils.Random;
	using SQLUtil = LineageServer.Utils.SQLUtil;
	using MapFactory = LineageServer.Utils.MapFactory;

	// Referenced classes of package l1j.server.server:
	// IdFactory

	public class PetTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(PetTable).FullName);

		private static PetTable _instance;

		private readonly IDictionary<int, L1Pet> _pets = MapFactory.newMap();

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
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM pets");

				rs = pstm.executeQuery();
				while (rs.next())
				{
					L1Pet pet = new L1Pet();
					int itemobjid = dataSourceRow.getInt(1);
					pet.set_itemobjid(itemobjid);
					pet.set_objid(dataSourceRow.getInt(2));
					pet.set_npcid(dataSourceRow.getInt(3));
					pet.set_name(dataSourceRow.getString(4));
					pet.set_level(dataSourceRow.getInt(5));
					pet.set_hp(dataSourceRow.getInt(6));
					pet.set_mp(dataSourceRow.getInt(7));
					pet.set_exp(dataSourceRow.getInt(8));
					pet.set_lawful(dataSourceRow.getInt(9));
					pet.set_food(dataSourceRow.getInt(10));

					_pets[itemobjid] = pet;
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
			short randomhp = (short)((hpUpMin + hpUpMax) / 2);
			short randommp = (short)((mpUpMin + mpUpMax) / 2);
			for (int i = 1; i < upLv; i++)
			{
				randomhp += (short)(RandomHelper.Next(hpUpMax - hpUpMin) + hpUpMin + 1);
				randommp += (short)(RandomHelper.Next(mpUpMax - mpUpMin) + mpUpMin + 1);
			}
			l1pet.set_hp(randomhp);
			l1pet.set_mp(randommp);
			l1pet.set_exp((int) lvExp); // upLv EXP
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