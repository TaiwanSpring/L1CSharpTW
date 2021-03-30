using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
    class MobSkillTable
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.Mobskill);

        private readonly bool _initialized;

        private static MobSkillTable _instance;

        private readonly IDictionary<int, L1MobSkill> _mobskills;

        public static MobSkillTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MobSkillTable();
                }
                return _instance;
            }
        }
        private MobSkillTable()
        {
            _mobskills = MapFactory.NewMap<int, L1MobSkill>();
            loadMobSkillData();
        }

        private void loadMobSkillData()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();
            Dictionary<int, List<IDataSourceRow>> group = new Dictionary<int, List<IDataSourceRow>>();
            int count = 0;
            int mobid = 0;
            int actNo = 0;
            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                mobid = dataSourceRow.getInt(Mobskill.Column_mobid);
                if (group.ContainsKey(mobid))
                {
                    group[mobid].Add(dataSourceRow);
                }
                else
                {
                    group.Add(mobid, new List<IDataSourceRow>());
                }
            }

            foreach (var pair in group)
            {
                mobid = pair.Key;
                L1MobSkill mobskill = new L1MobSkill(count);
                mobskill.set_mobid(mobid);
                foreach (IDataSourceRow dataRow in pair.Value)
                {
                    actNo = dataRow.getInt(Mobskill.Column_actNo);
                    mobskill.MobName = dataRow.getString(Mobskill.Column_mobname);
                    mobskill.setType(actNo, dataRow.getInt(Mobskill.Column_Type));
                    mobskill.setMpConsume(actNo, dataRow.getInt(Mobskill.Column_mpConsume));
                    mobskill.setTriggerRandom(actNo, dataRow.getInt(Mobskill.Column_TriRnd));
                    mobskill.setTriggerHp(actNo, dataRow.getInt(Mobskill.Column_TriHp));
                    mobskill.setTriggerCompanionHp(actNo, dataRow.getInt(Mobskill.Column_TriCompanionHp));
                    mobskill.setTriggerRange(actNo, dataRow.getInt(Mobskill.Column_TriRange));
                    mobskill.setTriggerCount(actNo, dataRow.getInt(Mobskill.Column_TriCount));
                    mobskill.setChangeTarget(actNo, dataRow.getInt(Mobskill.Column_ChangeTarget));
                    mobskill.setRange(actNo, dataRow.getInt(Mobskill.Column_Range));
                    mobskill.setAreaWidth(actNo, dataRow.getInt(Mobskill.Column_AreaWidth));
                    mobskill.setAreaHeight(actNo, dataRow.getInt(Mobskill.Column_AreaHeight));
                    mobskill.setLeverage(actNo, dataRow.getInt(Mobskill.Column_Leverage));
                    mobskill.setSkillId(actNo, dataRow.getInt(Mobskill.Column_SkillId));
                    mobskill.setSkillArea(actNo, dataRow.getInt(Mobskill.Column_SkillArea));
                    mobskill.setGfxid(actNo, dataRow.getInt(Mobskill.Column_Gfxid));
                    mobskill.setActid(actNo, dataRow.getInt(Mobskill.Column_ActId));
                    mobskill.setSummon(actNo, dataRow.getInt(Mobskill.Column_SummonId));
                    mobskill.setSummonMin(actNo, dataRow.getInt(Mobskill.Column_SummonMin));
                    mobskill.setSummonMax(actNo, dataRow.getInt(Mobskill.Column_SummonMax));
                    mobskill.setPolyId(actNo, dataRow.getInt(Mobskill.Column_PolyId));
                }
                _mobskills[mobid] = mobskill;
            }
        }

        public virtual L1MobSkill getTemplate(int id)
        {
            return _mobskills[id];
        }
    }

}