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
namespace LineageServer.Server.DataSources
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1FurnitureItem = LineageServer.Server.Templates.L1FurnitureItem;
	using SQLUtil = LineageServer.Utils.SQLUtil;

	public class FurnitureItemTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(FurnitureItemTable).FullName);

		private static FurnitureItemTable _instance;

		private readonly Dictionary<int, L1FurnitureItem> _furnishings = new Dictionary<int, L1FurnitureItem>();

		public static FurnitureItemTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new FurnitureItemTable();
				}
				return _instance;
			}
		}

		private FurnitureItemTable()
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
				pstm = con.prepareStatement("SELECT * FROM furniture_item");
				rs = pstm.executeQuery();
				while (rs.next())
				{
					L1FurnitureItem furniture = new L1FurnitureItem();
					int itemId = dataSourceRow.getInt("item_id");
					furniture.FurnitureItemId = itemId;
					furniture.FurnitureNpcId = dataSourceRow.getInt("npc_id");
					_furnishings[itemId] = furniture;
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

		public virtual L1FurnitureItem getTemplate(int itemId)
		{
			if (_furnishings.ContainsKey(itemId))
			{
				return _furnishings[itemId];
			}
			return null;
		}

	}

}