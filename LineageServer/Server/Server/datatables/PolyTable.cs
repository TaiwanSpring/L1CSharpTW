﻿using System.Collections.Generic;

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
	using L1PolyMorph = LineageServer.Server.Server.Model.L1PolyMorph;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	public class PolyTable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(PolyTable).FullName);

		private static PolyTable _instance;

		private readonly IDictionary<string, L1PolyMorph> _polymorphs = Maps.newMap();

		private readonly IDictionary<int, L1PolyMorph> _polyIdIndex = Maps.newMap();

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
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM polymorphs");
				rs = pstm.executeQuery();
				fillPolyTable(rs);
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, "error while creating polymorph table", e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private void fillPolyTable(java.sql.ResultSet rs) throws java.sql.SQLException
		private void fillPolyTable(ResultSet rs)
		{
			while (rs.next())
			{
				int id = rs.getInt("id");
				string name = rs.getString("name");
				int polyId = rs.getInt("polyid");
				int minLevel = rs.getInt("minlevel");
				int weaponEquipFlg = rs.getInt("weaponequip");
				int armorEquipFlg = rs.getInt("armorequip");
				bool canUseSkill = rs.getBoolean("isSkillUse");
				int causeFlg = rs.getInt("cause");

				L1PolyMorph poly = new L1PolyMorph(id, name, polyId, minLevel, weaponEquipFlg, armorEquipFlg, canUseSkill, causeFlg);

				_polymorphs[name] = poly;
				_polyIdIndex[polyId] = poly;
			}

			_log.config("変身リスト " + _polymorphs.Count + "件ロード");
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