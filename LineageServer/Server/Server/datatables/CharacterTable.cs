using System;
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
namespace LineageServer.Server.Server.datatables
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1Map = LineageServer.Server.Server.Model.map.L1Map;
	using L1WorldMap = LineageServer.Server.Server.Model.map.L1WorldMap;
	using CharacterStorage = LineageServer.Server.Server.storage.CharacterStorage;
	using MySqlCharacterStorage = LineageServer.Server.Server.storage.mysql.MySqlCharacterStorage;
	using L1CharName = LineageServer.Server.Server.Templates.L1CharName;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	public class CharacterTable
	{
		private CharacterStorage _charStorage;

		private static CharacterTable _instance;

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(CharacterTable).FullName);

		private readonly IDictionary<string, L1CharName> _charNameList = Maps.newConcurrentMap();

		private CharacterTable()
		{
			_charStorage = new MySqlCharacterStorage();
		}

		public static CharacterTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new CharacterTable();
				}
				return _instance;
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public void storeNewCharacter(l1j.server.server.model.Instance.L1PcInstance pc) throws Exception
		public virtual void storeNewCharacter(L1PcInstance pc)
		{
			lock (pc)
			{
				_charStorage.createCharacter(pc);
				string name = pc.Name;
				if (!_charNameList.ContainsKey(name))
				{
					L1CharName cn = new L1CharName();
					cn.Name = name;
					cn.Id = pc.Id;
					_charNameList[name] = cn;
				}
				_log.finest("storeNewCharacter");
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public void storeCharacter(l1j.server.server.model.Instance.L1PcInstance pc) throws Exception
		public virtual void storeCharacter(L1PcInstance pc)
		{
			lock (pc)
			{
				_charStorage.storeCharacter(pc);
				_log.finest("storeCharacter: " + pc.Name);
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public void deleteCharacter(String accountName, String charName) throws Exception
		public virtual void deleteCharacter(string accountName, string charName)
		{
			// 多分、同期は必要ない
			_charStorage.deleteCharacter(accountName, charName);
			if (_charNameList.ContainsKey(charName))
			{
				_charNameList.Remove(charName);
			}
			_log.finest("deleteCharacter");
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public l1j.server.server.model.Instance.L1PcInstance restoreCharacter(String charName) throws Exception
		public virtual L1PcInstance restoreCharacter(string charName)
		{
			L1PcInstance pc = _charStorage.loadCharacter(charName);
			return pc;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public l1j.server.server.model.Instance.L1PcInstance loadCharacter(String charName) throws Exception
		public virtual L1PcInstance loadCharacter(string charName)
		{
			L1PcInstance pc = null;
			try
			{
				pc = restoreCharacter(charName);

				// マップの範囲外ならSKTに移動させる
				L1Map map = L1WorldMap.Instance.getMap(pc.MapId);

				if (!map.isInMap(pc.X, pc.Y))
				{
					pc.X = 33087;
					pc.Y = 33396;
					pc.Map = (short) 4;
				}

				/*
				 * if(l1pcinstance.getClanid() != 0) { L1Clan clan = new L1Clan();
				 * ClanTable clantable = new ClanTable(); clan =
				 * clantable.getClan(l1pcinstance.getClanname());
				 * l1pcinstance.setClanname(clan.GetClanName()); }
				 */
				_log.finest("loadCharacter: " + pc.Name);
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			return pc;

		}

		public static void clearOnlineStatus()
		{
			Connection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("UPDATE characters SET OnlineStatus=0");
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
		}

		public static void updateOnlineStatus(L1PcInstance pc)
		{
			Connection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("UPDATE characters SET OnlineStatus=? WHERE objid=?");
				pstm.setInt(1, pc.OnlineStatus);
				pstm.setInt(2, pc.Id);
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
		}

		public static void updatePartnerId(int targetId)
		{
			updatePartnerId(targetId, 0);
		}

		public static void updatePartnerId(int targetId, int partnerId)
		{
			Connection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("UPDATE characters SET PartnerID=? WHERE objid=?");
				pstm.setInt(1, partnerId);
				pstm.setInt(2, targetId);
				pstm.execute();
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		public static void saveCharStatus(L1PcInstance pc)
		{
			Connection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("UPDATE characters SET OriginalStr= ?" + ", OriginalCon= ?, OriginalDex= ?, OriginalCha= ?" + ", OriginalInt= ?, OriginalWis= ?" + " WHERE objid=?");
				pstm.setInt(1, pc.BaseStr);
				pstm.setInt(2, pc.BaseCon);
				pstm.setInt(3, pc.BaseDex);
				pstm.setInt(4, pc.BaseCha);
				pstm.setInt(5, pc.BaseInt);
				pstm.setInt(6, pc.BaseWis);
				pstm.setInt(7, pc.Id);
				pstm.execute();
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		public virtual void restoreInventory(L1PcInstance pc)
		{
			pc.Inventory.loadItems();
			pc.DwarfInventory.loadItems();
			pc.DwarfForElfInventory.loadItems();
		}

		public static bool doesCharNameExist(string name)
		{
			bool result = true;
			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT account_name FROM characters WHERE char_name=?");
				pstm.setString(1, name);
				rs = pstm.executeQuery();
				result = rs.next();
			}
			catch (SQLException e)
			{
				_log.warning("could not check existing charname:" + e.Message);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
			return result;
		}

		public virtual void loadAllCharName()
		{
			L1CharName cn = null;
			string name = null;
			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM characters");
				rs = pstm.executeQuery();
				while (rs.next())
				{
					cn = new L1CharName();
					name = rs.getString("char_name");
					cn.Name = name;
					cn.Id = rs.getInt("objid");
					_charNameList[name] = cn;
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

		public virtual L1CharName[] CharNameList
		{
			get
			{
				return _charNameList.Values.ToArray();
			}
		}

	}

}