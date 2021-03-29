using LineageServer.Enum;
using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
    class NpcTable
    {
        //JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
        internal static ILogger _log = Logger.GetLogger(nameof(NpcTable));

        private readonly bool _initialized;

        private static NpcTable _instance;

        private readonly IDictionary<int, L1Npc> _npcs = MapFactory.NewMap<int, L1Npc>();

        //JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
        //ORIGINAL LINE: private final java.util.Map<String, java.lang.reflect.Constructor<?>> _constructorCache = l1j.server.server.utils.collections.Maps.newMap();
        private readonly IDictionary<string, System.Reflection.ConstructorInfo> _constructorCache = MapFactory.NewMap<string, System.Reflection.ConstructorInfo>();

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
        private System.Reflection.ConstructorInfo getConstructor(string implName)
        {
            try
            {
                string implFullName = "l1j.server.server.model.Instance." + implName + "Instance";
                //JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
                //ORIGINAL LINE: java.lang.reflect.Constructor<?> con = Class.forName(implFullName).getConstructors()[0];
                System.Reflection.ConstructorInfo con = Type.GetType(implFullName).GetConstructors()[0];
                return con;
            }
            catch (Exception e)
            {
                _log.Error(e);
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
            IDataBaseConnection con = null;
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
                    int npcId = dataSourceRow.getInt("npcid");
                    npc.set_npcId(npcId);
                    npc.set_name(dataSourceRow.getString("name"));
                    npc.set_nameid(dataSourceRow.getString("nameid"));
                    npc.Impl = dataSourceRow.getString("impl");
                    npc.set_gfxid(dataSourceRow.getInt("gfxid"));
                    npc.set_level(dataSourceRow.getInt("lvl"));
                    npc.set_hp(dataSourceRow.getInt("hp"));
                    npc.set_mp(dataSourceRow.getInt("mp"));
                    npc.set_ac(dataSourceRow.getInt("ac"));
                    npc.set_str(dataSourceRow.getByte("str"));
                    npc.set_con(dataSourceRow.getByte("con"));
                    npc.set_dex(dataSourceRow.getByte("dex"));
                    npc.set_wis(dataSourceRow.getByte("wis"));
                    npc.set_int(dataSourceRow.getByte("intel"));
                    npc.set_mr(dataSourceRow.getInt("mr"));
                    npc.set_exp(dataSourceRow.getInt("exp"));
                    npc.set_lawful(dataSourceRow.getInt("lawful"));
                    npc.set_size(dataSourceRow.getString("size"));
                    npc.set_weakAttr(dataSourceRow.getInt("weakAttr"));
                    npc.set_ranged(dataSourceRow.getInt("ranged"));
                    npc.Tamable = dataSourceRow.getBoolean("tamable");
                    npc.set_passispeed(dataSourceRow.getInt("passispeed"));
                    npc.set_atkspeed(dataSourceRow.getInt("atkspeed"));
                    npc.AltAtkSpeed = dataSourceRow.getInt("alt_atk_speed");
                    npc.AtkMagicSpeed = dataSourceRow.getInt("atk_magic_speed");
                    npc.SubMagicSpeed = dataSourceRow.getInt("sub_magic_speed");
                    npc.set_undead(dataSourceRow.getInt("undead"));
                    npc.set_poisonatk(dataSourceRow.getInt("poison_atk"));
                    npc.set_paralysisatk(dataSourceRow.getInt("paralysis_atk"));
                    npc.set_agro(dataSourceRow.getBoolean("agro"));
                    npc.set_agrososc(dataSourceRow.getBoolean("agrososc"));
                    npc.set_agrocoi(dataSourceRow.getBoolean("agrocoi"));
                    int? family = _familyTypes[dataSourceRow.getString("family")];
                    if (family == null)
                    {
                        npc.set_family(0);
                    }
                    else
                    {
                        npc.set_family(family.Value);
                    }
                    int agrofamily = dataSourceRow.getInt("agrofamily");
                    if ((npc.get_family() == 0) && (agrofamily == 1))
                    {
                        npc.set_agrofamily(0);
                    }
                    else
                    {
                        npc.set_agrofamily(agrofamily);
                    }
                    npc.set_agrogfxid1(dataSourceRow.getInt("agrogfxid1"));
                    npc.set_agrogfxid2(dataSourceRow.getInt("agrogfxid2"));
                    npc.set_picupitem(dataSourceRow.getBoolean("picupitem"));
                    npc.set_digestitem(dataSourceRow.getInt("digestitem"));
                    npc.set_bravespeed(dataSourceRow.getBoolean("bravespeed"));
                    npc.set_hprinterval(dataSourceRow.getInt("hprinterval"));
                    npc.set_hpr(dataSourceRow.getInt("hpr"));
                    npc.set_mprinterval(dataSourceRow.getInt("mprinterval"));
                    npc.set_mpr(dataSourceRow.getInt("mpr"));
                    npc.set_teleport(dataSourceRow.getBoolean("teleport"));
                    npc.set_randomlevel(dataSourceRow.getInt("randomlevel"));
                    npc.set_randomhp(dataSourceRow.getInt("randomhp"));
                    npc.set_randommp(dataSourceRow.getInt("randommp"));
                    npc.set_randomac(dataSourceRow.getInt("randomac"));
                    npc.set_randomexp(dataSourceRow.getInt("randomexp"));
                    npc.set_randomlawful(dataSourceRow.getInt("randomlawful"));
                    npc.set_damagereduction(dataSourceRow.getInt("damage_reduction"));
                    npc.set_hard(dataSourceRow.getBoolean("hard"));
                    npc.set_doppel(dataSourceRow.getBoolean("doppel"));
                    npc.set_IsTU(dataSourceRow.getBoolean("IsTU"));
                    npc.set_IsErase(dataSourceRow.getBoolean("IsErase"));
                    npc.BowActId = dataSourceRow.getInt("bowActId");
                    npc.Karma = dataSourceRow.getInt("karma");
                    npc.TransformId = dataSourceRow.getInt("transform_id");
                    npc.TransformGfxId = dataSourceRow.getInt("transform_gfxid");
                    npc.LightSize = dataSourceRow.getInt("light_size");
                    npc.AmountFixed = dataSourceRow.getBoolean("amount_fixed");
                    npc.ChangeHead = dataSourceRow.getBoolean("change_head");
                    npc.CantResurrect = dataSourceRow.getBoolean("cant_resurrect");

                    registerConstructorCache(npc.Impl);
                    _npcs[npcId] = npc;
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
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
                System.Reflection.ConstructorInfo con = _constructorCache[template.Impl];
                return (L1NpcInstance)con.Invoke(new object[] { template });
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
            return null;
        }

        public static IDictionary<string, int> buildFamily()
        {
            IDictionary<string, int> result = MapFactory.NewMap<string, int>();
            IDataBaseConnection con = null;
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
                    string family = dataSourceRow.getString("family");
                    result[family] = id++;
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
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