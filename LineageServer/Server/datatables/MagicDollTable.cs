using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Templates;
using System.Collections.Generic;

namespace LineageServer.Server.DataTables
{
    class MagicDollTable
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.MagicDoll);

        private static MagicDollTable _instance;

        private readonly Dictionary<int, L1MagicDoll> _dolls = new Dictionary<int, L1MagicDoll>();

        public static MagicDollTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MagicDollTable();
                }
                return _instance;
            }
        }

        private MagicDollTable()
        {
            load();
        }

        private void load()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();
            L1MagicDoll doll;
            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                doll = new L1MagicDoll();
                doll.ItemId = dataSourceRow.getInt(MagicDoll.Column_item_id);
                doll.DollId = dataSourceRow.getInt(MagicDoll.Column_doll_id);
                doll.Ac = dataSourceRow.getInt(MagicDoll.Column_ac);
                doll.Hpr = dataSourceRow.getInt(MagicDoll.Column_hpr);
                doll.HprTime = dataSourceRow.getBoolean(MagicDoll.Column_hpr_time);
                doll.Mpr = dataSourceRow.getInt(MagicDoll.Column_mpr);
                doll.MprTime = dataSourceRow.getBoolean(MagicDoll.Column_mpr_time);
                doll.Hit = dataSourceRow.getInt(MagicDoll.Column_hit);
                doll.Dmg = dataSourceRow.getInt(MagicDoll.Column_dmg);
                doll.DmgChance = dataSourceRow.getInt(MagicDoll.Column_dmg_chance);
                doll.BowHit = dataSourceRow.getInt(MagicDoll.Column_bow_hit);
                doll.BowDmg = dataSourceRow.getInt(MagicDoll.Column_bow_dmg);
                doll.DmgReduction = dataSourceRow.getInt(MagicDoll.Column_dmg_reduction);
                doll.DmgReductionChance = dataSourceRow.getInt(MagicDoll.Column_dmg_reduction_chance);
                doll.DmgEvasionChance = dataSourceRow.getInt(MagicDoll.Column_dmg_evasion_chance);
                doll.WeightReduction = dataSourceRow.getInt(MagicDoll.Column_weight_reduction);
                doll.RegistStun = dataSourceRow.getInt(MagicDoll.Column_regist_stun);
                doll.RegistStone = dataSourceRow.getInt(MagicDoll.Column_regist_stone);
                doll.RegistSleep = dataSourceRow.getInt(MagicDoll.Column_regist_sleep);
                doll.RegistFreeze = dataSourceRow.getInt(MagicDoll.Column_regist_freeze);
                doll.RegistSustain = dataSourceRow.getInt(MagicDoll.Column_regist_sustain);
                doll.RegistBlind = dataSourceRow.getInt(MagicDoll.Column_regist_blind);
                doll.MakeItemId = dataSourceRow.getInt(MagicDoll.Column_make_itemid);
                doll.Effect = dataSourceRow.getByte(MagicDoll.Column_effect);
                doll.EffectChance = dataSourceRow.getInt(MagicDoll.Column_effect_chance);
                _dolls[doll.ItemId] = doll;
            }
        }

        public virtual L1MagicDoll getTemplate(int itemId)
        {
            if (_dolls.ContainsKey(itemId))
            {
                return _dolls[itemId];
            }
            else
            {
                return null;
            }
        }
    }

}