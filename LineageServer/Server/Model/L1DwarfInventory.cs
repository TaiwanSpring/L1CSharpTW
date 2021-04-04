using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Collections.Generic;

namespace LineageServer.Server.Model
{
    class L1DwarfInventory : L1Inventory
    {
        private readonly static IDataSource dataSource =
              Container.Instance.Resolve<IDataSourceFactory>()
              .Factory(Enum.DataSourceTypeEnum.CharacterWarehouse);

        private readonly L1PcInstance _owner;

        public L1DwarfInventory(L1PcInstance owner)
        {
            _owner = owner;
        }

        // ＤＢのcharacter_itemsの読込
        public override void loadItems()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select()
               .Where(CharacterWarehouse.Column_account_name, _owner.AccountName).Query();


            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                L1ItemInstance item = new L1ItemInstance();
                int objectId = dataSourceRow.getInt(CharacterWarehouse.Column_id);
                item.Id = objectId;
                L1Item itemTemplate = ItemTable.Instance.getTemplate(dataSourceRow.getInt(CharacterWarehouse.Column_item_id));
                item.Item = itemTemplate;
                item.Count = dataSourceRow.getInt(CharacterWarehouse.Column_count);
                item.Equipped = false;
                item.EnchantLevel = dataSourceRow.getInt(CharacterWarehouse.Column_enchantlvl);
                item.Identified = dataSourceRow.getInt(CharacterWarehouse.Column_is_id) != 0 ? true : false;
                item.set_durability(dataSourceRow.getInt(CharacterWarehouse.Column_durability));
                item.ChargeCount = dataSourceRow.getInt(CharacterWarehouse.Column_charge_count);
                item.RemainingTime = dataSourceRow.getInt(CharacterWarehouse.Column_remaining_time);
                item.LastUsed = dataSourceRow.getTimestamp(CharacterWarehouse.Column_last_used);
                item.Bless = dataSourceRow.getInt(CharacterWarehouse.Column_bless);
                item.AttrEnchantKind = dataSourceRow.getInt(CharacterWarehouse.Column_attr_enchant_kind);
                item.AttrEnchantLevel = dataSourceRow.getInt(CharacterWarehouse.Column_attr_enchant_level);
                item.FireMr = dataSourceRow.getInt(CharacterWarehouse.Column_firemr);
                item.WaterMr = dataSourceRow.getInt(CharacterWarehouse.Column_watermr);
                item.EarthMr = dataSourceRow.getInt(CharacterWarehouse.Column_earthmr);
                item.WindMr = dataSourceRow.getInt(CharacterWarehouse.Column_windmr);
                item.setaddSp(dataSourceRow.getInt(CharacterWarehouse.Column_addsp));
                item.setaddHp(dataSourceRow.getInt(CharacterWarehouse.Column_addhp));
                item.setaddMp(dataSourceRow.getInt(CharacterWarehouse.Column_addmp));
                item.Hpr = dataSourceRow.getInt(CharacterWarehouse.Column_hpr);
                item.Mpr = dataSourceRow.getInt(CharacterWarehouse.Column_mpr);
                item.M_Def = dataSourceRow.getInt(CharacterWarehouse.Column_m_def);
                // 登入鑰匙紀錄
                if (item.Item.ItemId == 40312)
                {
                    InnKeyTable.checkey(item);
                }
                _items.Add(item);
                Container.Instance.Resolve<IGameWorld>().storeObject(item);
            }
        }
        // ＤＢのcharacter_warehouseへ登録
        public override void insertItem(L1ItemInstance item)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Insert()
            .Set(CharacterWarehouse.Column_id, item.Id)
            .Set(CharacterWarehouse.Column_account_name, _owner.AccountName)
            .Set(CharacterWarehouse.Column_item_id, item.ItemId)
            .Set(CharacterWarehouse.Column_item_name, item.Name)
            .Set(CharacterWarehouse.Column_count, item.Count)
            .Set(CharacterWarehouse.Column_is_equipped, item.Equipped ? 1 : 0)
            .Set(CharacterWarehouse.Column_enchantlvl, item.EnchantLevel)
            .Set(CharacterWarehouse.Column_is_id, item.Identified ? 1 : 0)
            .Set(CharacterWarehouse.Column_durability, item.get_durability())
            .Set(CharacterWarehouse.Column_charge_count, item.ChargeCount)
            .Set(CharacterWarehouse.Column_remaining_time, item.RemainingTime)
            .Set(CharacterWarehouse.Column_last_used, item.LastUsed)
            .Set(CharacterWarehouse.Column_bless, item.Bless)
            .Set(CharacterWarehouse.Column_attr_enchant_kind, item.AttrEnchantKind)
            .Set(CharacterWarehouse.Column_attr_enchant_level, item.AttrEnchantLevel)
            .Set(CharacterWarehouse.Column_firemr, item.FireMr)
            .Set(CharacterWarehouse.Column_watermr, item.WaterMr)
            .Set(CharacterWarehouse.Column_earthmr, item.EarthMr)
            .Set(CharacterWarehouse.Column_windmr, item.WindMr)
            .Set(CharacterWarehouse.Column_addsp, item.getaddSp())
            .Set(CharacterWarehouse.Column_addhp, item.getaddHp())
            .Set(CharacterWarehouse.Column_addmp, item.getaddMp())
            .Set(CharacterWarehouse.Column_hpr, item.Hpr)
            .Set(CharacterWarehouse.Column_mpr, item.Mpr)
            .Set(CharacterWarehouse.Column_m_def, item.M_Def)
            .Execute();
        }

        // ＤＢのcharacter_warehouseを更新
        public override void updateItem(L1ItemInstance item)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Update()
            .Where(CharacterWarehouse.Column_id, item.Id)
            .Set(CharacterWarehouse.Column_count, item.Count)
            .Execute();
        }

        // ＤＢのcharacter_warehouseから削除
        public override void deleteItem(L1ItemInstance item)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Delete()
            .Where(CharacterWarehouse.Column_id, item)
            .Execute();
            lock (_items)
            {
                _items.Remove(item);
            }
        }
        private readonly static IDataSource accountDataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Accounts);
        public static void present(string account, int itemid, int enchant, int count)
        {

            L1Item temp = ItemTable.Instance.getTemplate(itemid);
            if (temp == null)
            {
                return;
            }
            IList<IDataSourceRow> dataSourceRows;

            if (account == "*")
            {
                dataSourceRows = dataSource.Select().Query();
            }
            else
            {
                dataSourceRows = dataSource.Select().Where(Accounts.Column_login, account).Query();
            }

            IList<string> accountList = ListFactory.NewList<string>();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                accountList.Add(dataSourceRow.getString(Accounts.Column_login));
            }

            present(accountList, itemid, enchant, count);
        }
        public static void present(int minlvl, int maxlvl, int itemid, int enchant, int count)
        {
            L1Item temp = ItemTable.Instance.getTemplate(itemid);
            if (temp == null)
            {
                return;
            }
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();
            IList<string> accountList = ListFactory.NewList<string>();
            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                int level = dataSourceRow.getInt(Accounts.Column_access_level);
                if (level >= minlvl && level <= maxlvl)
                {
                    accountList.Add(dataSourceRow.getString(Accounts.Column_login));
                }
            }
            present(accountList, itemid, enchant, count);
        }
        private readonly static IDataSource characterWarehouseDataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.CharacterWarehouse);
        private static void present(IList<string> accountList, int itemid, int enchant, int count)
        {
            L1Item itemtemp = ItemTable.Instance.getTemplate(itemid);

            if (ItemTable.Instance.getTemplate(itemid) == null)
            {
                throw new Exception("道具編號不存在。");
            }
            else
            {
                foreach (string account in accountList)
                {
                    if (itemtemp.Stackable)
                    {
                        L1ItemInstance item = ItemTable.Instance.createItem(itemid);
                        item.EnchantLevel = enchant;
                        item.Count = count;
                        IDataSourceRow dataSourceRow = characterWarehouseDataSource.NewRow();
                        dataSourceRow.Insert()
                        .Set(CharacterWarehouse.Column_id, item.Id)
                        .Set(CharacterWarehouse.Column_account_name, account)
                        .Set(CharacterWarehouse.Column_item_id, item.ItemId)
                        .Set(CharacterWarehouse.Column_item_name, item.Name)
                        .Set(CharacterWarehouse.Column_count, item.Count)
                        .Set(CharacterWarehouse.Column_is_equipped, item.Equipped ? 1 : 0)
                        .Set(CharacterWarehouse.Column_enchantlvl, item.EnchantLevel)
                        .Set(CharacterWarehouse.Column_is_id, item.Identified ? 1 : 0)
                        .Set(CharacterWarehouse.Column_durability, item.get_durability())
                        .Set(CharacterWarehouse.Column_charge_count, item.ChargeCount)
                        .Set(CharacterWarehouse.Column_remaining_time, item.RemainingTime)
                        .Set(CharacterWarehouse.Column_last_used, item.LastUsed)
                        .Set(CharacterWarehouse.Column_bless, item.Bless)
                        .Set(CharacterWarehouse.Column_attr_enchant_kind, item.AttrEnchantKind)
                        .Set(CharacterWarehouse.Column_attr_enchant_level, item.AttrEnchantLevel)
                        .Set(CharacterWarehouse.Column_firemr, item.FireMr)
                        .Set(CharacterWarehouse.Column_watermr, item.WaterMr)
                        .Set(CharacterWarehouse.Column_earthmr, item.EarthMr)
                        .Set(CharacterWarehouse.Column_windmr, item.WindMr)
                        .Set(CharacterWarehouse.Column_addsp, item.getaddSp())
                        .Set(CharacterWarehouse.Column_addhp, item.getaddHp())
                        .Set(CharacterWarehouse.Column_addmp, item.getaddMp())
                        .Set(CharacterWarehouse.Column_hpr, item.Hpr)
                        .Set(CharacterWarehouse.Column_mpr, item.Mpr)
                        .Set(CharacterWarehouse.Column_m_def, item.M_Def)
                        .Execute();
                    }
                    else
                    {
                        L1ItemInstance item = null;
                        int createCount;

                        for (createCount = 0; createCount < count; createCount++)
                        {
                            item = ItemTable.Instance.createItem(itemid);
                            item.EnchantLevel = enchant;
                            IDataSourceRow dataSourceRow = characterWarehouseDataSource.NewRow();
                            dataSourceRow.Insert()
                            .Set(CharacterWarehouse.Column_id, item.Id)
                            .Set(CharacterWarehouse.Column_account_name, account)
                            .Set(CharacterWarehouse.Column_item_id, item.ItemId)
                            .Set(CharacterWarehouse.Column_item_name, item.Name)
                            .Set(CharacterWarehouse.Column_count, item.Count)
                            .Set(CharacterWarehouse.Column_is_equipped, item.Equipped ? 1 : 0)
                            .Set(CharacterWarehouse.Column_enchantlvl, item.EnchantLevel)
                            .Set(CharacterWarehouse.Column_is_id, item.Identified ? 1 : 0)
                            .Set(CharacterWarehouse.Column_durability, item.get_durability())
                            .Set(CharacterWarehouse.Column_charge_count, item.ChargeCount)
                            .Set(CharacterWarehouse.Column_remaining_time, item.RemainingTime)
                            .Set(CharacterWarehouse.Column_last_used, item.LastUsed)
                            .Set(CharacterWarehouse.Column_bless, item.Bless)
                            .Set(CharacterWarehouse.Column_attr_enchant_kind, item.AttrEnchantKind)
                            .Set(CharacterWarehouse.Column_attr_enchant_level, item.AttrEnchantLevel)
                            .Set(CharacterWarehouse.Column_firemr, item.FireMr)
                            .Set(CharacterWarehouse.Column_watermr, item.WaterMr)
                            .Set(CharacterWarehouse.Column_earthmr, item.EarthMr)
                            .Set(CharacterWarehouse.Column_windmr, item.WindMr)
                            .Set(CharacterWarehouse.Column_addsp, item.getaddSp())
                            .Set(CharacterWarehouse.Column_addhp, item.getaddHp())
                            .Set(CharacterWarehouse.Column_addmp, item.getaddMp())
                            .Set(CharacterWarehouse.Column_hpr, item.Hpr)
                            .Set(CharacterWarehouse.Column_mpr, item.Mpr)
                            .Set(CharacterWarehouse.Column_m_def, item.M_Def)
                            .Execute();
                        }
                    }
                }
            }
        }
    }
}