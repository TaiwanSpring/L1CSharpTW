using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using System;
using System.Collections.Generic;

namespace LineageServer.Server.Model
{
    class L1DwarfForElfInventory : L1Inventory
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.CharacterElfWarehouse);
        private readonly L1PcInstance _owner;

        public L1DwarfForElfInventory(L1PcInstance owner)
        {
            _owner = owner;
        }

        // ＤＢのcharacter_itemsの読込
        public override void loadItems()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select()
                .Where(CharacterElfWarehouse.Column_account_name, _owner.AccountName).Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                L1ItemInstance item = new L1ItemInstance();
                int objectId = dataSourceRow.getInt(CharacterElfWarehouse.Column_id);
                item.Id = objectId;
                L1Item itemTemplate = ItemTable.Instance.getTemplate(dataSourceRow.getInt(CharacterElfWarehouse.Column_item_id));
                item.Item = itemTemplate;
                item.Count = dataSourceRow.getInt(CharacterElfWarehouse.Column_count);
                item.Equipped = false;
                item.EnchantLevel = dataSourceRow.getInt(CharacterElfWarehouse.Column_enchantlvl);
                item.Identified = dataSourceRow.getInt(CharacterElfWarehouse.Column_is_id) != 0 ? true : false;
                item.set_durability(dataSourceRow.getInt(CharacterElfWarehouse.Column_durability));
                item.ChargeCount = dataSourceRow.getInt(CharacterElfWarehouse.Column_charge_count);
                item.RemainingTime = dataSourceRow.getInt(CharacterElfWarehouse.Column_remaining_time);
                item.LastUsed = dataSourceRow.getTimestamp(CharacterElfWarehouse.Column_last_used);
                item.Bless = dataSourceRow.getInt(CharacterElfWarehouse.Column_bless);
                item.AttrEnchantKind = dataSourceRow.getInt(CharacterElfWarehouse.Column_attr_enchant_kind);
                item.AttrEnchantLevel = dataSourceRow.getInt(CharacterElfWarehouse.Column_attr_enchant_level);
                item.FireMr = dataSourceRow.getInt(CharacterElfWarehouse.Column_firemr);
                item.WaterMr = dataSourceRow.getInt(CharacterElfWarehouse.Column_watermr);
                item.EarthMr = dataSourceRow.getInt(CharacterElfWarehouse.Column_earthmr);
                item.WindMr = dataSourceRow.getInt(CharacterElfWarehouse.Column_windmr);
                item.setaddSp(dataSourceRow.getInt(CharacterElfWarehouse.Column_addsp));
                item.setaddHp(dataSourceRow.getInt(CharacterElfWarehouse.Column_addhp));
                item.setaddMp(dataSourceRow.getInt(CharacterElfWarehouse.Column_addmp));
                item.Hpr = dataSourceRow.getInt(CharacterElfWarehouse.Column_hpr);
                item.Mpr = dataSourceRow.getInt(CharacterElfWarehouse.Column_mpr);
                item.M_Def = dataSourceRow.getInt(CharacterElfWarehouse.Column_m_def);
                // 登入鑰匙紀錄
                if (item.Item.ItemId == 40312)
                {
                    InnKeyTable.checkey(item);
                }
                _items.Add(item);
                Container.Instance.Resolve<IGameWorld>().storeObject(item);
            }
        }

        // ＤＢのcharacter_elf_warehouseへ登録
        public override void insertItem(L1ItemInstance item)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Insert()
            .Set(CharacterElfWarehouse.Column_id, item.Id)
            .Set(CharacterElfWarehouse.Column_account_name, _owner.AccountName)
            .Set(CharacterElfWarehouse.Column_item_id, item.ItemId)
            .Set(CharacterElfWarehouse.Column_item_name, item.Name)
            .Set(CharacterElfWarehouse.Column_count, item.Count)
            .Set(CharacterElfWarehouse.Column_is_equipped, item.Equipped ? 1 : 0)
            .Set(CharacterElfWarehouse.Column_enchantlvl, item.EnchantLevel)
            .Set(CharacterElfWarehouse.Column_is_id, item.Identified ? 1 : 0)
            .Set(CharacterElfWarehouse.Column_durability, item.get_durability())
            .Set(CharacterElfWarehouse.Column_charge_count, item.ChargeCount)
            .Set(CharacterElfWarehouse.Column_remaining_time, item.RemainingTime)
            .Set(CharacterElfWarehouse.Column_last_used, item.LastUsed)
            .Set(CharacterElfWarehouse.Column_bless, item.Bless)
            .Set(CharacterElfWarehouse.Column_attr_enchant_kind, item.AttrEnchantKind)
            .Set(CharacterElfWarehouse.Column_attr_enchant_level, item.AttrEnchantLevel)
            .Set(CharacterElfWarehouse.Column_firemr, item.FireMr)
            .Set(CharacterElfWarehouse.Column_watermr, item.WaterMr)
            .Set(CharacterElfWarehouse.Column_earthmr, item.EarthMr)
            .Set(CharacterElfWarehouse.Column_windmr, item.WindMr)
            .Set(CharacterElfWarehouse.Column_addsp, item.getaddSp())
            .Set(CharacterElfWarehouse.Column_addhp, item.getaddHp())
            .Set(CharacterElfWarehouse.Column_addmp, item.getaddMp())
            .Set(CharacterElfWarehouse.Column_hpr, item.Hpr)
            .Set(CharacterElfWarehouse.Column_mpr, item.Mpr)
            .Set(CharacterElfWarehouse.Column_m_def, item.M_Def)
            .Execute();
        }

        // ＤＢのcharacter_elf_warehouseを更新
        public override void updateItem(L1ItemInstance item)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Update()
            .Where(CharacterElfWarehouse.Column_id, item.Id)
            .Set(CharacterElfWarehouse.Column_count, item.Count)
            .Execute();
        }

        // ＤＢのcharacter_elf_warehouseから削除
        public override void deleteItem(L1ItemInstance item)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Delete()
            .Where(CharacterElfWarehouse.Column_id, item)
            .Execute();
            lock (_items)
            {
                _items.Remove(item);
            }
        }
    }

}