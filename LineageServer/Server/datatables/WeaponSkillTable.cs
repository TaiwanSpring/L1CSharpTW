using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Utils;
using System.Collections.Generic;

namespace LineageServer.Server.DataTables
{
    class WeaponSkillTable : IGameComponent
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.WeaponSkill);

        private static WeaponSkillTable _instance;

        private readonly IDictionary<int, L1WeaponSkill> _weaponIdIndex = MapFactory.NewMap<int, L1WeaponSkill>();

        public static WeaponSkillTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WeaponSkillTable();
                }
                return _instance;
            }
        }

        private WeaponSkillTable()
        {
            
        }
        public void Initialize()
        {
            loadWeaponSkill();
        }
        private void loadWeaponSkill()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                fillWeaponSkillTable(dataSourceRow);
            }
        }

        private void fillWeaponSkillTable(IDataSourceRow dataSourceRow)
        {
            int weaponId = dataSourceRow.getInt(WeaponSkill.Column_weapon_id);
            int probability = dataSourceRow.getInt(WeaponSkill.Column_probability);
            int fixDamage = dataSourceRow.getInt(WeaponSkill.Column_fix_damage);
            int randomDamage = dataSourceRow.getInt(WeaponSkill.Column_random_damage);
            int area = dataSourceRow.getInt(WeaponSkill.Column_area);
            int skillId = dataSourceRow.getInt(WeaponSkill.Column_skill_id);
            int skillTime = dataSourceRow.getInt(WeaponSkill.Column_skill_time);
            int effectId = dataSourceRow.getInt(WeaponSkill.Column_effect_id);
            int effectTarget = dataSourceRow.getInt(WeaponSkill.Column_effect_target);
            bool isArrowType = dataSourceRow.getBoolean(WeaponSkill.Column_arrow_type);
            int attr = dataSourceRow.getInt(WeaponSkill.Column_attr);
            L1WeaponSkill weaponSkill = new L1WeaponSkill(weaponId, probability, fixDamage, randomDamage, area, skillId, skillTime, effectId, effectTarget, isArrowType, attr);
            _weaponIdIndex[weaponId] = weaponSkill;
        }

        public virtual L1WeaponSkill getTemplate(int weaponId)
        {
            return _weaponIdIndex[weaponId];
        }

    }

}