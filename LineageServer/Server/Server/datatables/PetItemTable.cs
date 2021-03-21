using System.Collections.Generic;

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
	using L1PetItem = LineageServer.Server.Server.Templates.L1PetItem;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	public class PetItemTable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(PetItemTable).FullName);

		private static PetItemTable _instance;

		private readonly IDictionary<int, L1PetItem> _petItemIdIndex = Maps.newMap();

		private static readonly IDictionary<string, int> _useTypes = Maps.newMap();

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
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM petitem");
				rs = pstm.executeQuery();
				fillPetItemTable(rs);
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, "error while creating etcitem_petitem table", e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private void fillPetItemTable(java.sql.ResultSet rs) throws java.sql.SQLException
		private void fillPetItemTable(ResultSet rs)
		{
			while (rs.next())
			{
				L1PetItem petItem = new L1PetItem();
				petItem.ItemId = rs.getInt("item_id");
				petItem.UseType = (_useTypes[rs.getString("use_type")]);
				petItem.HitModifier = rs.getInt("hitmodifier");
				petItem.DamageModifier = rs.getInt("dmgmodifier");
				petItem.AddAc = rs.getInt("ac");
				petItem.AddStr = rs.getInt("add_str");
				petItem.AddCon = rs.getInt("add_con");
				petItem.AddDex = rs.getInt("add_dex");
				petItem.AddInt = rs.getInt("add_int");
				petItem.AddWis = rs.getInt("add_wis");
				petItem.AddHp = rs.getInt("add_hp");
				petItem.AddMp = rs.getInt("add_mp");
				petItem.AddSp = rs.getInt("add_sp");
				petItem.AddMr = rs.getInt("m_def");
				_petItemIdIndex[petItem.ItemId] = petItem;
			}
		}

		public virtual L1PetItem getTemplate(int itemId)
		{
			return _petItemIdIndex[itemId];
		}

	}

}