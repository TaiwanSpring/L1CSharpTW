using System;
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
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	public class NpcTable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		internal static Logger _log = Logger.getLogger(typeof(NpcTable).FullName);

		private readonly bool _initialized;

		private static NpcTable _instance;

		private readonly IDictionary<int, L1Npc> _npcs = Maps.newMap();

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: private final java.util.Map<String, java.lang.reflect.Constructor<?>> _constructorCache = l1j.server.server.utils.collections.Maps.newMap();
		private readonly IDictionary<string, System.Reflection.ConstructorInfo<object>> _constructorCache = Maps.newMap();

		private static readonly IDictionary<string, int> _familyTypes = NpcTable.buildFamily();

		public static NpcTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new NpcTable();
				}
				return _instance;
			}
		}

		public virtual bool Initialized
		{
			get
			{
				return _initialized;
			}
		}

		private NpcTable()
		{
			loadNpcData();
			_initialized = true;
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: private java.lang.reflect.Constructor<?> getConstructor(String implName)
		private System.Reflection.ConstructorInfo<object> getConstructor(string implName)
		{
			try
			{
				string implFullName = "l1j.server.server.model.Instance." + implName + "Instance";
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: java.lang.reflect.Constructor<?> con = Class.forName(implFullName).getConstructors()[0];
				System.Reflection.ConstructorInfo<object> con = Type.GetType(implFullName).GetConstructors()[0];
				return con;
			}
			catch (ClassNotFoundException e)
			{
				_log.log(Level.WARNING, e.Message, e);
			}
			return null;
		}

		private void registerConstructorCache(string implName)
		{
			if (implName.Length == 0 || _constructorCache.ContainsKey(implName))
			{
				return;
			}
			_constructorCache[implName] = getConstructor(implName);
		}

		private void loadNpcData()
		{
			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM npc");
				rs = pstm.executeQuery();
				while (rs.next())
				{
					L1Npc npc = new L1Npc();
					int npcId = rs.getInt("npcid");
					npc.set_npcId(npcId);
					npc.set_name(rs.getString("name"));
					npc.set_nameid(rs.getString("nameid"));
					npc.Impl = rs.getString("impl");
					npc.set_gfxid(rs.getInt("gfxid"));
					npc.set_level(rs.getInt("lvl"));
					npc.set_hp(rs.getInt("hp"));
					npc.set_mp(rs.getInt("mp"));
					npc.set_ac(rs.getInt("ac"));
					npc.set_str(rs.getByte("str"));
					npc.set_con(rs.getByte("con"));
					npc.set_dex(rs.getByte("dex"));
					npc.set_wis(rs.getByte("wis"));
					npc.set_int(rs.getByte("intel"));
					npc.set_mr(rs.getInt("mr"));
					npc.set_exp(rs.getInt("exp"));
					npc.set_lawful(rs.getInt("lawful"));
					npc.set_size(rs.getString("size"));
					npc.set_weakAttr(rs.getInt("weakAttr"));
					npc.set_ranged(rs.getInt("ranged"));
					npc.Tamable = rs.getBoolean("tamable");
					npc.set_passispeed(rs.getInt("passispeed"));
					npc.set_atkspeed(rs.getInt("atkspeed"));
					npc.AltAtkSpeed = rs.getInt("alt_atk_speed");
					npc.AtkMagicSpeed = rs.getInt("atk_magic_speed");
					npc.SubMagicSpeed = rs.getInt("sub_magic_speed");
					npc.set_undead(rs.getInt("undead"));
					npc.set_poisonatk(rs.getInt("poison_atk"));
					npc.set_paralysisatk(rs.getInt("paralysis_atk"));
					npc.set_agro(rs.getBoolean("agro"));
					npc.set_agrososc(rs.getBoolean("agrososc"));
					npc.set_agrocoi(rs.getBoolean("agrocoi"));
					int? family = _familyTypes[rs.getString("family")];
					if (family == null)
					{
						npc.set_family(0);
					}
					else
					{
						npc.set_family(family.Value);
					}
					int agrofamily = rs.getInt("agrofamily");
					if ((npc.get_family() == 0) && (agrofamily == 1))
					{
						npc.set_agrofamily(0);
					}
					else
					{
						npc.set_agrofamily(agrofamily);
					}
					npc.set_agrogfxid1(rs.getInt("agrogfxid1"));
					npc.set_agrogfxid2(rs.getInt("agrogfxid2"));
					npc.set_picupitem(rs.getBoolean("picupitem"));
					npc.set_digestitem(rs.getInt("digestitem"));
					npc.set_bravespeed(rs.getBoolean("bravespeed"));
					npc.set_hprinterval(rs.getInt("hprinterval"));
					npc.set_hpr(rs.getInt("hpr"));
					npc.set_mprinterval(rs.getInt("mprinterval"));
					npc.set_mpr(rs.getInt("mpr"));
					npc.set_teleport(rs.getBoolean("teleport"));
					npc.set_randomlevel(rs.getInt("randomlevel"));
					npc.set_randomhp(rs.getInt("randomhp"));
					npc.set_randommp(rs.getInt("randommp"));
					npc.set_randomac(rs.getInt("randomac"));
					npc.set_randomexp(rs.getInt("randomexp"));
					npc.set_randomlawful(rs.getInt("randomlawful"));
					npc.set_damagereduction(rs.getInt("damage_reduction"));
					npc.set_hard(rs.getBoolean("hard"));
					npc.set_doppel(rs.getBoolean("doppel"));
					npc.set_IsTU(rs.getBoolean("IsTU"));
					npc.set_IsErase(rs.getBoolean("IsErase"));
					npc.BowActId = rs.getInt("bowActId");
					npc.Karma = rs.getInt("karma");
					npc.TransformId = rs.getInt("transform_id");
					npc.TransformGfxId = rs.getInt("transform_gfxid");
					npc.LightSize = rs.getInt("light_size");
					npc.AmountFixed = rs.getBoolean("amount_fixed");
					npc.ChangeHead = rs.getBoolean("change_head");
					npc.CantResurrect = rs.getBoolean("cant_resurrect");

					registerConstructorCache(npc.Impl);
					_npcs[npcId] = npc;
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

		public virtual L1Npc getTemplate(int id)
		{
			return _npcs[id];
		}

		public virtual L1NpcInstance newNpcInstance(int id)
		{
			L1Npc npcTemp = getTemplate(id);
			if (npcTemp == null)
			{
				throw new System.ArgumentException(string.Format("NpcTemplate: {0:D} not found", id));
			}
			return newNpcInstance(npcTemp);
		}

		public virtual L1NpcInstance newNpcInstance(L1Npc template)
		{
			try
			{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: java.lang.reflect.Constructor<?> con = _constructorCache.get(template.getImpl());
				System.Reflection.ConstructorInfo<object> con = _constructorCache[template.Impl];
				return (L1NpcInstance) con.Invoke(new object[] {template});
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			return null;
		}

		public static IDictionary<string, int> buildFamily()
		{
			IDictionary<string, int> result = Maps.newMap();
			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("select distinct(family) as family from npc WHERE NOT trim(family) =''");
				rs = pstm.executeQuery();
				int id = 1;
				while (rs.next())
				{
					string family = rs.getString("family");
					result[family] = id++;
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
			return result;
		}

		public virtual int findNpcIdByName(string name)
		{
			foreach (L1Npc npc in _npcs.Values)
			{
				if (npc.get_name().Equals(name))
				{
					return npc.get_npcId();
				}
			}
			return 0;
		}

		public virtual int findNpcIdByNameWithoutSpace(string name)
		{
			foreach (L1Npc npc in _npcs.Values)
			{
				if (npc.get_name().Replace(" ", "").Equals(name))
				{
					return npc.get_npcId();
				}
			}
			return 0;
		}
	}

}