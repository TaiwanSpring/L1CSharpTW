using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
    public class ArmorSetTable
    {
        private static ArmorSetTable _instance;

        private readonly IList<L1ArmorSets> _armorSetList = ListFactory.NewList<L1ArmorSets>();
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.ArmorSet);

        public static ArmorSetTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ArmorSetTable();
                }
                return _instance;
            }
        }

        private ArmorSetTable()
        {
            load();
        }

        private void load()
        {
            IList<IDataSourceRow> dataSourceTable = dataSource.Select().Query();
            fillTable(dataSourceTable);
        }
        private void fillTable(IList<IDataSourceRow> dataSourceRows)
        {
            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                L1ArmorSets l1ArmorSets = new L1ArmorSets();
                l1ArmorSets.Id = dataSourceRow.getInt(ArmorSet.Column_id);
                l1ArmorSets.Sets = dataSourceRow.getString(ArmorSet.Column_sets);
                l1ArmorSets.PolyId = dataSourceRow.getInt(ArmorSet.Column_polyid);
                l1ArmorSets.Ac = dataSourceRow.getInt(ArmorSet.Column_ac);
                l1ArmorSets.Hp = dataSourceRow.getInt(ArmorSet.Column_hp);
                l1ArmorSets.Mp = dataSourceRow.getInt(ArmorSet.Column_mp);
                l1ArmorSets.Hpr = dataSourceRow.getInt(ArmorSet.Column_hpr);
                l1ArmorSets.Mpr = dataSourceRow.getInt(ArmorSet.Column_mpr);
                l1ArmorSets.Mr = dataSourceRow.getInt(ArmorSet.Column_mr);
                l1ArmorSets.Str = dataSourceRow.getInt(ArmorSet.Column_str);
                l1ArmorSets.Dex = dataSourceRow.getInt(ArmorSet.Column_dex);
                l1ArmorSets.Con = dataSourceRow.getInt(ArmorSet.Column_con);
                l1ArmorSets.Wis = dataSourceRow.getInt(ArmorSet.Column_wis);
                l1ArmorSets.Cha = dataSourceRow.getInt(ArmorSet.Column_cha);
                l1ArmorSets.Intl = dataSourceRow.getInt(ArmorSet.Column_intl);
                l1ArmorSets.HitModifier = dataSourceRow.getInt(ArmorSet.Column_hit_modifier);
                l1ArmorSets.DmgModifier = dataSourceRow.getInt(ArmorSet.Column_dmg_modifier);
                l1ArmorSets.BowHitModifier = dataSourceRow.getInt(ArmorSet.Column_bow_hit_modifier);
                l1ArmorSets.BowDmgModifier = dataSourceRow.getInt(ArmorSet.Column_bow_dmg_modifier);
                l1ArmorSets.Sp = dataSourceRow.getInt(ArmorSet.Column_sp);
                l1ArmorSets.DefenseWater = dataSourceRow.getInt(ArmorSet.Column_defense_water);
                l1ArmorSets.DefenseWind = dataSourceRow.getInt(ArmorSet.Column_defense_wind);
                l1ArmorSets.DefenseFire = dataSourceRow.getInt(ArmorSet.Column_defense_fire);
                l1ArmorSets.DefenseEarth = dataSourceRow.getInt(ArmorSet.Column_defense_earth);
                _armorSetList.Add(l1ArmorSets);
            }
        }

        public virtual L1ArmorSets[] AllList
        {
            get
            {
                return ((List<L1ArmorSets>)_armorSetList).ToArray();
            }
        }

    }

}