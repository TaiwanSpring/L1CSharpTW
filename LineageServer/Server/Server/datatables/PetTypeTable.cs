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
namespace LineageServer.Server.Server.DataSources
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1PetType = LineageServer.Server.Server.Templates.L1PetType;
	using IntRange = LineageServer.Server.Server.utils.IntRange;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	public class PetTypeTable
	{
		private static PetTypeTable _instance;

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(PetTypeTable).FullName);

		private IDictionary<int, L1PetType> _types = Maps.newMap();

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
					int baseNpcId = rs.getInt("BaseNpcId");
					string name = rs.getString("Name");
					int itemIdForTaming = rs.getInt("ItemIdForTaming");
					int hpUpMin = rs.getInt("HpUpMin");
					int hpUpMax = rs.getInt("HpUpMax");
					int mpUpMin = rs.getInt("MpUpMin");
					int mpUpMax = rs.getInt("MpUpMax");
					int evolvItemId = rs.getInt("EvolvItemId");
					int npcIdForEvolving = rs.getInt("NpcIdForEvolving");
					int[] msgIds = new int[5];
					for (int i = 0; i < 5; i++)
					{
						msgIds[i] = rs.getInt("MessageId" + (i + 1));
					}
					int defyMsgId = rs.getInt("DefyMessageId");
					bool canUseEquipment = rs.getBoolean("canUseEquipment");
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