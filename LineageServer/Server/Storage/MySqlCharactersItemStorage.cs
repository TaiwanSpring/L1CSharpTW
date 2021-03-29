using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.Storage
{
    class MySqlCharactersItemStorage : CharactersItemStorage
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.CharacterItems);
        public override IList<L1ItemInstance> loadItems(int objId)
        {
            IList<L1ItemInstance> items = ListFactory.NewList<L1ItemInstance>();
            IList<IDataSourceRow> dataSourceRows = dataSource.Select()
                .Where(CharacterItems.Column_id, objId).Query();

            L1ItemInstance item;

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                int itemId = dataSourceRow.getInt(CharacterItems.Column_item_id);
                L1Item itemTemplate = ItemTable.Instance.getTemplate(itemId);
                if (itemTemplate == null)
                {
                    Logger.GenericLogger.Warning(string.Format("item id:{0:D} not found", itemId));
                    continue;
                }

                item = new L1ItemInstance();
                item.Id = dataSourceRow.getInt(CharacterItems.Column_id);
                item.Item = itemTemplate;
                item.Count = dataSourceRow.getInt(CharacterItems.Column_count);
                item.Equipped = dataSourceRow.getInt(CharacterItems.Column_is_equipped) != 0 ? true : false;
                item.EnchantLevel = dataSourceRow.getInt(CharacterItems.Column_enchantlvl);
                item.Identified = dataSourceRow.getInt(CharacterItems.Column_is_id) != 0 ? true : false;
                item.set_durability(dataSourceRow.getInt(CharacterItems.Column_durability));
                item.ChargeCount = dataSourceRow.getInt(CharacterItems.Column_charge_count);
                item.RemainingTime = dataSourceRow.getInt(CharacterItems.Column_remaining_time);
                item.LastUsed = dataSourceRow.getTimestamp(CharacterItems.Column_last_used);
                item.Bless = dataSourceRow.getInt(CharacterItems.Column_bless);
                item.AttrEnchantKind = dataSourceRow.getInt(CharacterItems.Column_attr_enchant_kind);
                item.AttrEnchantLevel = dataSourceRow.getInt(CharacterItems.Column_attr_enchant_level);
                item.FireMr = dataSourceRow.getInt(CharacterItems.Column_firemr);
                item.WaterMr = dataSourceRow.getInt(CharacterItems.Column_watermr);
                item.EarthMr = dataSourceRow.getInt(CharacterItems.Column_earthmr);
                item.WindMr = dataSourceRow.getInt(CharacterItems.Column_windmr);
                item.setaddSp(dataSourceRow.getInt(CharacterItems.Column_addsp));
                item.setaddHp(dataSourceRow.getInt(CharacterItems.Column_addhp));
                item.setaddMp(dataSourceRow.getInt(CharacterItems.Column_addmp));
                item.Hpr = dataSourceRow.getInt(CharacterItems.Column_hpr);
                item.Mpr = dataSourceRow.getInt(CharacterItems.Column_mpr);
                item.M_Def = dataSourceRow.getInt(CharacterItems.Column_m_def);
                item.LastStatus.updateAll();
                // 登入鑰匙紀錄
                if (item.Item.ItemId == 40312)
                {
                    InnKeyTable.checkey(item);
                }
                items.Add(item);
            }
            return items;
        }

        public override void storeItem(int objId, L1ItemInstance item)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Insert()
            .Set(CharacterItems.Column_id, item.Id)
            .Set(CharacterItems.Column_item_id, item.ItemId)
            .Set(CharacterItems.Column_char_id, objId)
            .Set(CharacterItems.Column_item_name, item.Name)
            .Set(CharacterItems.Column_count, item.Count)
            .Set(CharacterItems.Column_is_equipped, item.Equipped)
            .Set(CharacterItems.Column_enchantlvl, item.EnchantLevel)
            .Set(CharacterItems.Column_is_id, item.Identified ? 1 : 0)
            .Set(CharacterItems.Column_durability, item.get_durability())
            .Set(CharacterItems.Column_charge_count, item.ChargeCount)
            .Set(CharacterItems.Column_remaining_time, item.RemainingTime)
            .Set(CharacterItems.Column_last_used, item.LastUsed)
            .Set(CharacterItems.Column_bless, item.Bless)
            .Set(CharacterItems.Column_attr_enchant_kind, item.AttrEnchantKind)
            .Set(CharacterItems.Column_attr_enchant_level, item.AttrEnchantLevel)
            .Set(CharacterItems.Column_firemr, item.FireMr)
            .Set(CharacterItems.Column_watermr, item.WaterMr)
            .Set(CharacterItems.Column_earthmr, item.EarthMr)
            .Set(CharacterItems.Column_windmr, item.WindMr)
            .Set(CharacterItems.Column_addsp, item.getaddSp())
            .Set(CharacterItems.Column_addhp, item.getaddHp())
            .Set(CharacterItems.Column_addmp, item.getaddMp())
            .Set(CharacterItems.Column_hpr, item.Hpr)
            .Set(CharacterItems.Column_mpr, item.Mpr)
            .Set(CharacterItems.Column_m_def, item.M_Def)
            .Execute();
            item.LastStatus.updateAll();
        }
        public override void deleteItem(L1ItemInstance item)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Delete()
            .Where(CharacterItems.Column_id, item.Id)
            .Execute();
        }
        public override void updateItemId(L1ItemInstance item)
        {
            dataSource.NewRow()
            .Update()
            .Where(CharacterItems.Column_id, item.Id)
            .Set(CharacterItems.Column_item_id, item.ItemId)
            .Execute();
            item.LastStatus.updateItemId();
        }
        public override void updateItemCount(L1ItemInstance item)
        {
            dataSource.NewRow()
              .Update()
              .Where(CharacterItems.Column_id, item.Id)
              .Set(CharacterItems.Column_count, item.Count)
              .Execute();
            item.LastStatus.updateCount();
        }
        public override void updateItemDurability(L1ItemInstance item)
        {
            dataSource.NewRow()
            .Update()
            .Where(CharacterItems.Column_id, item.Id)
            .Set(CharacterItems.Column_durability, item.get_durability())
            .Execute();
            item.LastStatus.updateDuraility();
        }
        public override void updateItemChargeCount(L1ItemInstance item)
        {
            dataSource.NewRow()
               .Update()
               .Where(CharacterItems.Column_id, item.Id)
               .Set(CharacterItems.Column_charge_count, item.ChargeCount)
               .Execute();
            item.LastStatus.updateChargeCount();
        }
        public override void updateItemRemainingTime(L1ItemInstance item)
        {
            dataSource.NewRow()
               .Update()
               .Where(CharacterItems.Column_id, item.Id)
               .Set(CharacterItems.Column_remaining_time, item.RemainingTime)
               .Execute();
            item.LastStatus.updateRemainingTime();
        }
        public override void updateItemEnchantLevel(L1ItemInstance item)
        {
            dataSource.NewRow()
                .Update()
                .Where(CharacterItems.Column_id, item.Id)
                .Set(CharacterItems.Column_enchantlvl, item.EnchantLevel)
                .Execute();
            item.LastStatus.updateEnchantLevel();
        }
        public override void updateItemEquipped(L1ItemInstance item)
        {
            dataSource.NewRow()
                .Update()
                .Where(CharacterItems.Column_id, item.Id)
                .Set(CharacterItems.Column_is_equipped, item.Equipped ? 1 : 0)
                .Execute();
            item.LastStatus.updateEquipped();
        }
        public override void updateItemIdentified(L1ItemInstance item)
        {
            dataSource.NewRow()
                .Update()
                .Where(CharacterItems.Column_id, item.Id)
                .Set(CharacterItems.Column_is_id, item.Identified ? 1 : 0)
                .Execute();
            item.LastStatus.updateIdentified();
        }
        public override void updateItemDelayEffect(L1ItemInstance item)
        {
            dataSource.NewRow()
                .Update()
                .Where(CharacterItems.Column_id, item.Id)
                .Set(CharacterItems.Column_last_used, item.LastUsed)
                .Execute();
            item.LastStatus.updateLastUsed();
        }
        public override void updateItemBless(L1ItemInstance item)
        {
            dataSource.NewRow()
                .Update()
                .Where(CharacterItems.Column_id, item.Id)
                .Set(CharacterItems.Column_bless, item.Bless)
                .Execute();
            item.LastStatus.updateBless();
        }
        public override void updateItemAttrEnchantKind(L1ItemInstance item)
        {
            dataSource.NewRow()
                .Update()
                .Where(CharacterItems.Column_id, item.Id)
                .Set(CharacterItems.Column_attr_enchant_kind, item.AttrEnchantKind)
                .Execute();
            item.LastStatus.updateAttrEnchantKind();
        }
        public override void updateItemAttrEnchantLevel(L1ItemInstance item)
        {
            dataSource.NewRow()
                .Update()
                .Where(CharacterItems.Column_id, item.Id)
                .Set(CharacterItems.Column_attr_enchant_level, item.AttrEnchantLevel)
                .Execute();
            item.LastStatus.updateAttrEnchantLevel();
        }

        /// <summary>
        /// 飾品強化卷軸 
        /// </summary>
        public override void updateFireMr(L1ItemInstance item)
        {
            dataSource.NewRow()
                .Update()
                .Where(CharacterItems.Column_id, item.Id)
                .Set(CharacterItems.Column_firemr, item.FireMr)
                .Execute();
            item.LastStatus.updateFireMr();
        }
        public override void updateWaterMr(L1ItemInstance item)
        {
            dataSource.NewRow()
                .Update()
                .Where(CharacterItems.Column_id, item.Id)
                .Set(CharacterItems.Column_watermr, item.WaterMr)
                .Execute();
            item.LastStatus.updateWaterMr();
        }
        public override void updateEarthMr(L1ItemInstance item)
        {
            dataSource.NewRow()
                .Update()
                .Where(CharacterItems.Column_id, item.Id)
                .Set(CharacterItems.Column_earthmr, item.EarthMr)
                .Execute();
            item.LastStatus.updateEarthMr();
        }
        public override void updateWindMr(L1ItemInstance item)
        {
            dataSource.NewRow()
                .Update()
                .Where(CharacterItems.Column_id, item.Id)
                .Set(CharacterItems.Column_windmr, item.WindMr)
                .Execute();
            item.LastStatus.updateWindMr();
        }
        public override void updateaddSp(L1ItemInstance item)
        {
            dataSource.NewRow()
                .Update()
                .Where(CharacterItems.Column_id, item.Id)
                .Set(CharacterItems.Column_addsp, item.getaddSp())
                .Execute();
            item.LastStatus.updateSp();
        }
        public override void updateaddHp(L1ItemInstance item)
        {
            dataSource.NewRow()
                .Update()
                .Where(CharacterItems.Column_id, item.Id)
                .Set(CharacterItems.Column_addhp, item.getaddHp())
                .Execute();
            item.LastStatus.updateaddHp();
        }
        public override void updateaddMp(L1ItemInstance item)
        {
            dataSource.NewRow()
                .Update()
                .Where(CharacterItems.Column_id, item.Id)
                .Set(CharacterItems.Column_addmp, item.getaddMp())
                .Execute();
            item.LastStatus.updateaddMp();
        }
        public override void updateHpr(L1ItemInstance item)
        {
            dataSource.NewRow()
                .Update()
                .Where(CharacterItems.Column_id, item.Id)
                .Set(CharacterItems.Column_hpr, item.Hpr)
                .Execute();
            item.LastStatus.updateHpr();
        }
        public override void updateMpr(L1ItemInstance item)
        {
            dataSource.NewRow()
                .Update()
                .Where(CharacterItems.Column_id, item.Id)
                .Set(CharacterItems.Column_mpr, item.Mpr)
                .Execute();
            item.LastStatus.updateMpr();
        }
        public override void updateM_Def(L1ItemInstance item)
        {
            dataSource.NewRow()
                .Update()
                .Where(CharacterItems.Column_id, item.Id)
                .Set(CharacterItems.Column_m_def, item.M_Def)
                .Execute();
            item.LastStatus.updateM_Def();
        }
        public override int getItemCount(int objId)
        {
            return dataSource.Select()
                 .Where(CharacterItems.Column_id, objId).Query().Count;
        }
    }

}