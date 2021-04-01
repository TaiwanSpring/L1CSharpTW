using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
    class NpcTable : IGameComponent, INpcController
    {
        private readonly static IDataSource dataSource =
             Container.Instance.Resolve<IDataSourceFactory>()
             .Factory(Enum.DataSourceTypeEnum.Npc);

        private readonly IDictionary<int, L1Npc> _npcs = MapFactory.NewMap<int, L1Npc>();

        private readonly IDictionary<string, int> _familyTypes;

        public NpcTable()
        {
            _familyTypes = buildFamily();
        }

        private void loadNpcData()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                L1Npc npc = new L1Npc();
                int npcId = dataSourceRow.getInt(Npc.Column_npcid);
                npc.set_npcId(npcId);
                npc.set_name(dataSourceRow.getString(Npc.Column_name));
                npc.set_nameid(dataSourceRow.getString(Npc.Column_nameid));
                npc.Impl = dataSourceRow.getString(Npc.Column_impl);
                npc.set_gfxid(dataSourceRow.getInt(Npc.Column_gfxid));
                npc.set_level(dataSourceRow.getInt(Npc.Column_lvl));
                npc.set_hp(dataSourceRow.getInt(Npc.Column_hp));
                npc.set_mp(dataSourceRow.getInt(Npc.Column_mp));
                npc.set_ac(dataSourceRow.getInt(Npc.Column_ac));
                npc.set_str(dataSourceRow.getByte(Npc.Column_str));
                npc.set_con(dataSourceRow.getByte(Npc.Column_con));
                npc.set_dex(dataSourceRow.getByte(Npc.Column_dex));
                npc.set_wis(dataSourceRow.getByte(Npc.Column_wis));
                npc.set_int(dataSourceRow.getByte(Npc.Column_intel));
                npc.set_mr(dataSourceRow.getInt(Npc.Column_mr));
                npc.set_exp(dataSourceRow.getInt(Npc.Column_exp));
                npc.set_lawful(dataSourceRow.getInt(Npc.Column_lawful));
                npc.set_size(dataSourceRow.getString(Npc.Column_size));
                npc.set_weakAttr(dataSourceRow.getInt(Npc.Column_weakAttr));
                npc.set_ranged(dataSourceRow.getInt(Npc.Column_ranged));
                npc.Tamable = dataSourceRow.getBoolean(Npc.Column_tamable);
                npc.set_passispeed(dataSourceRow.getInt(Npc.Column_passispeed));
                npc.set_atkspeed(dataSourceRow.getInt(Npc.Column_atkspeed));
                npc.AltAtkSpeed = dataSourceRow.getInt(Npc.Column_alt_atk_speed);
                npc.AtkMagicSpeed = dataSourceRow.getInt(Npc.Column_atk_magic_speed);
                npc.SubMagicSpeed = dataSourceRow.getInt(Npc.Column_sub_magic_speed);
                npc.set_undead(dataSourceRow.getInt(Npc.Column_undead));
                npc.set_poisonatk(dataSourceRow.getInt(Npc.Column_poison_atk));
                npc.set_paralysisatk(dataSourceRow.getInt(Npc.Column_paralysis_atk));
                npc.set_agro(dataSourceRow.getBoolean(Npc.Column_agro));
                npc.set_agrososc(dataSourceRow.getBoolean(Npc.Column_agrososc));
                npc.set_agrocoi(dataSourceRow.getBoolean(Npc.Column_agrocoi));
                string familyString = dataSourceRow.getString(Npc.Column_family);
                if (_familyTypes.ContainsKey(familyString))
                {
                    npc.set_family(_familyTypes[familyString]);
                }
                else
                {
                    npc.set_family(0);
                }
                int agrofamily = dataSourceRow.getInt(Npc.Column_agrofamily);

                if ((npc.get_family() == 0) && (agrofamily == 1))
                {
                    npc.set_agrofamily(0);
                }
                else
                {
                    npc.set_agrofamily(agrofamily);
                }

                npc.set_agrogfxid1(dataSourceRow.getInt(Npc.Column_agrogfxid1));
                npc.set_agrogfxid2(dataSourceRow.getInt(Npc.Column_agrogfxid2));
                npc.set_picupitem(dataSourceRow.getBoolean(Npc.Column_picupitem));
                npc.set_digestitem(dataSourceRow.getInt(Npc.Column_digestitem));
                npc.set_bravespeed(dataSourceRow.getBoolean(Npc.Column_bravespeed));
                npc.set_hprinterval(dataSourceRow.getInt(Npc.Column_hprinterval));
                npc.set_hpr(dataSourceRow.getInt(Npc.Column_hpr));
                npc.set_mprinterval(dataSourceRow.getInt(Npc.Column_mprinterval));
                npc.set_mpr(dataSourceRow.getInt(Npc.Column_mpr));
                npc.set_teleport(dataSourceRow.getBoolean(Npc.Column_teleport));
                npc.set_randomlevel(dataSourceRow.getInt(Npc.Column_randomlevel));
                npc.set_randomhp(dataSourceRow.getInt(Npc.Column_randomhp));
                npc.set_randommp(dataSourceRow.getInt(Npc.Column_randommp));
                npc.set_randomac(dataSourceRow.getInt(Npc.Column_randomac));
                npc.set_randomexp(dataSourceRow.getInt(Npc.Column_randomexp));
                npc.set_randomlawful(dataSourceRow.getInt(Npc.Column_randomlawful));
                npc.set_damagereduction(dataSourceRow.getInt(Npc.Column_damage_reduction));
                npc.set_hard(dataSourceRow.getBoolean(Npc.Column_hard));
                npc.set_doppel(dataSourceRow.getBoolean(Npc.Column_doppel));
                npc.set_IsTU(dataSourceRow.getBoolean(Npc.Column_IsTU));
                npc.set_IsErase(dataSourceRow.getBoolean(Npc.Column_IsErase));
                npc.BowActId = dataSourceRow.getInt(Npc.Column_bowActId);
                npc.Karma = dataSourceRow.getInt(Npc.Column_karma);
                npc.TransformId = dataSourceRow.getInt(Npc.Column_transform_id);
                npc.TransformGfxId = dataSourceRow.getInt(Npc.Column_transform_gfxid);
                npc.LightSize = dataSourceRow.getInt(Npc.Column_light_size);
                npc.AmountFixed = dataSourceRow.getBoolean(Npc.Column_amount_fixed);
                npc.ChangeHead = dataSourceRow.getBoolean(Npc.Column_change_head);
                npc.CantResurrect = dataSourceRow.getBoolean(Npc.Column_cant_resurrect);

                _npcs[npcId] = npc;
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
            return L1NpcInstance.Factory(template);
        }
        private IDictionary<string, int> buildFamily()
        {
            IDictionary<string, int> result = MapFactory.NewMap<string, int>();
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query("select distinct(family) as family from npc WHERE NOT trim(family) =''");
            int id = 1;
            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                string family = dataSourceRow.getString(Npc.Column_family);
                result[family] = id++;
            }
            return result;
        }
        public virtual int findNpcIdByName(string name)
        {
            foreach (L1Npc npc in _npcs.Values)
            {
                if (npc.get_name() == name)
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
                if (npc.get_name().Replace(" ", "") == name)
                {
                    return npc.get_npcId();
                }
            }
            return 0;
        }
        public void Initialize()
        {

            loadNpcData();
        }
    }
}