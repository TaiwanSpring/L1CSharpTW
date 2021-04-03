using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using System;
using System.Collections.Generic;

namespace LineageServer.Server.Model
{
    class L1DwarfForClanInventory : L1Inventory
    {
        private readonly static IDataSource clanWarehouseDataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.ClanWarehouse);

        private readonly static IDataSource clanWarehouseHistoryDataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.ClanWarehouseHistory);

        private readonly L1Clan _clan;

        public L1DwarfForClanInventory(L1Clan clan)
        {
            _clan = clan;
        }

        // ＤＢのcharacter_itemsの読込
        public override void loadItems()
        {

            IList<IDataSourceRow> dataSourceRows = clanWarehouseDataSource.Select()
                .Where(ClanWarehouse.Column_clan_name, _clan.ClanName).Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                L1ItemInstance item = new L1ItemInstance();
                int objectId = dataSourceRow.getInt(ClanWarehouse.Column_id);
                item.Id = objectId;
                int itemId = dataSourceRow.getInt(ClanWarehouse.Column_item_id);
                L1Item itemTemplate = ItemTable.Instance.getTemplate(itemId);
                if (itemTemplate == null)
                {
                    throw new System.NullReferenceException($"item_id={itemId} not found");
                }
                item.Item = itemTemplate;
                item.Count = dataSourceRow.getInt(ClanWarehouse.Column_count);
                item.Equipped = false;
                item.EnchantLevel = dataSourceRow.getInt(ClanWarehouse.Column_enchantlvl);
                item.Identified = dataSourceRow.getInt(ClanWarehouse.Column_is_id) != 0 ? true : false;
                item.set_durability(dataSourceRow.getInt(ClanWarehouse.Column_durability));
                item.ChargeCount = dataSourceRow.getInt(ClanWarehouse.Column_charge_count);
                item.RemainingTime = dataSourceRow.getInt(ClanWarehouse.Column_remaining_time);
                item.LastUsed = dataSourceRow.getTimestamp(ClanWarehouse.Column_last_used);
                item.Bless = dataSourceRow.getInt(ClanWarehouse.Column_bless);
                item.AttrEnchantKind = dataSourceRow.getInt(ClanWarehouse.Column_attr_enchant_kind);
                item.AttrEnchantLevel = dataSourceRow.getInt(ClanWarehouse.Column_attr_enchant_level);
                item.FireMr = dataSourceRow.getInt(ClanWarehouse.Column_firemr);
                item.WaterMr = dataSourceRow.getInt(ClanWarehouse.Column_watermr);
                item.EarthMr = dataSourceRow.getInt(ClanWarehouse.Column_earthmr);
                item.WindMr = dataSourceRow.getInt(ClanWarehouse.Column_windmr);
                item.setaddSp(dataSourceRow.getInt(ClanWarehouse.Column_addsp));
                item.setaddHp(dataSourceRow.getInt(ClanWarehouse.Column_addhp));
                item.setaddMp(dataSourceRow.getInt(ClanWarehouse.Column_addmp));
                item.Hpr = dataSourceRow.getInt(ClanWarehouse.Column_hpr);
                item.Mpr = dataSourceRow.getInt(ClanWarehouse.Column_mpr);
                item.M_Def = dataSourceRow.getInt(ClanWarehouse.Column_m_def);
                // 登入鑰匙紀錄
                if (item.Item.ItemId == 40312)
                {
                    InnKeyTable.checkey(item);
                }
                _items.Add(item);
                Container.Instance.Resolve<IGameWorld>().storeObject(item);
            }

        }

        // ＤＢのclan_warehouseへ登録
        public override void insertItem(L1ItemInstance item)
        {

            IDataSourceRow dataSourceRow = clanWarehouseDataSource.NewRow();
            dataSourceRow.Insert()
            .Set(ClanWarehouse.Column_id, item.Id)
            .Set(ClanWarehouse.Column_clan_name, _clan.ClanName)
            .Set(ClanWarehouse.Column_item_id, item.ItemId)
            .Set(ClanWarehouse.Column_item_name, item.Name)
            .Set(ClanWarehouse.Column_count, item.Count)
            .Set(ClanWarehouse.Column_is_equipped, item.Equipped ? 1 : 0)
            .Set(ClanWarehouse.Column_enchantlvl, item.EnchantLevel)
            .Set(ClanWarehouse.Column_is_id, item.Identified ? 1 : 0)
            .Set(ClanWarehouse.Column_durability, item.get_durability())
            .Set(ClanWarehouse.Column_charge_count, item.ChargeCount)
            .Set(ClanWarehouse.Column_remaining_time, item.RemainingTime)
            .Set(ClanWarehouse.Column_last_used, item.LastUsed)
            .Set(ClanWarehouse.Column_bless, item.Bless)
            .Set(ClanWarehouse.Column_attr_enchant_kind, item.AttrEnchantKind)
            .Set(ClanWarehouse.Column_attr_enchant_level, item.AttrEnchantLevel)
            .Set(ClanWarehouse.Column_firemr, item.FireMr)
            .Set(ClanWarehouse.Column_watermr, item.WaterMr)
            .Set(ClanWarehouse.Column_earthmr, item.EarthMr)
            .Set(ClanWarehouse.Column_windmr, item.WindMr)
            .Set(ClanWarehouse.Column_addsp, item.getaddSp())
            .Set(ClanWarehouse.Column_addhp, item.getaddHp())
            .Set(ClanWarehouse.Column_addmp, item.getaddMp())
            .Set(ClanWarehouse.Column_hpr, item.Hpr)
            .Set(ClanWarehouse.Column_mpr, item.Mpr)
            .Set(ClanWarehouse.Column_m_def, item.M_Def)
            .Execute();
            lock (_items)
            {
                _items.Add(item);
            }
        }

        // ＤＢのclan_warehouseを更新
        public override void updateItem(L1ItemInstance item)
        {
            IDataSourceRow dataSourceRow = clanWarehouseDataSource.NewRow();
            dataSourceRow.Update()
            .Where(ClanWarehouse.Column_id, item.Id)
            .Set(ClanWarehouse.Column_count, item.Count)
            .Execute();
        }

        // ＤＢのclan_warehouseから削除
        public override void deleteItem(L1ItemInstance item)
        {
            IDataSourceRow dataSourceRow = clanWarehouseDataSource.NewRow();
            dataSourceRow.Delete()
            .Where(ClanWarehouse.Column_id, item)
            .Execute();
            lock (_items)
            {
                _items.Remove(item);
            }
        }

        // DBのクラン倉庫のアイテムを全て削除(血盟解散時のみ使用)
        public virtual void deleteAllItems()
        {
            IDataSourceRow dataSourceRow = clanWarehouseDataSource.NewRow();
            dataSourceRow.Delete()
            .Where(ClanWarehouse.Column_clan_name, _clan.ClanName)
            .Execute();
        }

        /// <summary>
        /// 寫入血盟使用紀錄 </summary>
        /// <param name="pc">    L1PcInstance</br> </param>
        /// <param name="item">  L1ItemInstance</br> </param>
        /// <param name="count"> 物品數量</br> </param>
        /// <param name="type">  領出: 1, 存入: 0 </br> </param>
        public virtual void writeHistory(L1PcInstance pc, L1ItemInstance item, int count, int type)
        {
            IDataSourceRow dataSourceRow = clanWarehouseHistoryDataSource.NewRow();
            dataSourceRow.Insert()
            .Set(ClanWarehouseHistory.Column_clan_id, _clan.ClanId)
            .Set(ClanWarehouseHistory.Column_char_name, pc.Name)
            .Set(ClanWarehouseHistory.Column_type, type)
            .Set(ClanWarehouseHistory.Column_item_name, item.Name)
            .Set(ClanWarehouseHistory.Column_item_count, count)
            .Set(ClanWarehouseHistory.Column_record_time, DateTime.Now)
            .Execute();
        }
    }
}