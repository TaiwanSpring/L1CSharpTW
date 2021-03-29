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
namespace LineageServer.Server.DataTables
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1PetType = LineageServer.Server.Templates.L1PetType;
	using IntRange = LineageServer.Utils.IntRange;
	using SQLUtil = LineageServer.Utils.SQLUtil;
	using MapFactory = LineageServer.Utils.MapFactory;

	public class PetTypeTable
	{
		private static PetTypeTable _instance;

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(PetTypeTable).FullName);

		private IDictionary<int, L1PetType> _types = MapFactory.newMap();

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
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM pettypes");

				rs = pstm.executeQuery();

				while (rs.next())
				{
					int baseNpcId = dataSourceRow.getInt("BaseNpcId");
					string name = dataSourceRow.getString("Name");
					int itemIdForTaming = dataSourceRow.getInt("ItemIdForTaming");
					int hpUpMin = dataSourceRow.getInt("HpUpMin");
					int hpUpMax = dataSourceRow.getInt("HpUpMax");
					int mpUpMin = dataSourceRow.getInt("MpUpMin");
					int mpUpMax = dataSourceRow.getInt("MpUpMax");
					int evolvItemId = dataSourceRow.getInt("EvolvItemId");
					int npcIdForEvolving = dataSourceRow.getInt("NpcIdForEvolving");
					int[] msgIds = new int[5];
					for (int i = 0; i < 5; i++)
					{
						msgIds[i] = dataSourceRow.getInt("MessageId" + (i + 1));
					}
					int defyMsgId = dataSourceRow.getInt("DefyMessageId");
					bool canUseEquipment = dataSourceRow.getBoolean("canUseEquipment");
					IntRange hpUpRange = new IntRange(hpUpMin, hpUpMax);
					IntRange mpUpRange = new IntRange(mpUpMin, mpUpMax);
					_types[baseNpcId] = new L1PetType(baseNpcId, name, itemIdForTaming, hpUpRange, mpUpRange, evolvItemId, npcIdForEvolving, msgIds, defyMsgId, canUseEquipment);
					_defaultNames.Add(name.ToLower());
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