using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
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
        public static void present(string account, int itemid, int enchant, int count)
        {

            L1Item temp = ItemTable.Instance.getTemplate(itemid);
            if (temp == null)
            {
                return;
            }

            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            ResultSet rs = null;
            try
            {
                con = L1DatabaseFactory.Instance.Connection;

                if (account == "*")
                {
                    pstm = con.prepareStatement("SELECT * FROM accounts");
                }
                else
                {
                    pstm = con.prepareStatement("SELECT * FROM accounts WHERE login=?");
                    pstm.setString(1, account);
                }
                rs = pstm.executeQuery();

                IList<string> accountList = ListFactory.NewList();
                while (rs.next())
                {
                    accountList.Add(dataSourceRow.getString("login"));
                }

                present(accountList, itemid, enchant, count);

            }
            catch (SQLException e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
                throw e;
            }
            finally
            {
                SQLUtil.close(rs);
                SQLUtil.close(pstm);
                SQLUtil.close(con);
            }

        }
        public static void present(int minlvl, int maxlvl, int itemid, int enchant, int count)
        {

            L1Item temp = ItemTable.Instance.getTemplate(itemid);
            if (temp == null)
            {
                return;
            }

            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            ResultSet rs = null;
            try
            {
                con = L1DatabaseFactory.Instance.Connection;

                pstm = con.prepareStatement("SELECT distinct(account_name) as account_name FROM characters WHERE level between ? and ?");
                pstm.setInt(1, minlvl);
                pstm.setInt(2, maxlvl);
                rs = pstm.executeQuery();

                IList<string> accountList = ListFactory.NewList();
                while (rs.next())
                {
                    accountList.Add(dataSourceRow.getString("account_name"));
                }

                present(accountList, itemid, enchant, count);

            }
            catch (SQLException e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
                throw e;
            }
            finally
            {
                SQLUtil.close(rs);
                SQLUtil.close(pstm);
                SQLUtil.close(con);
            }

        }
        private static void present(IList<string> accountList, int itemid, int enchant, int count)
        {
            L1Item itemtemp = ItemTable.Instance.getTemplate(itemid);

            if (ItemTable.Instance.getTemplate(itemid) == null)
            {
                throw new Exception("道具編號不存在。");
            }
            else
            {
                IDataBaseConnection con = null;
                PreparedStatement ps = null;

                try
                {
                    con = L1DatabaseFactory.Instance.Connection;
                    con.AutoCommit = false;

                    foreach (string account in accountList)
                    {
                        if (itemtemp.Stackable)
                        {
                            L1ItemInstance item = ItemTable.Instance.createItem(itemid);
                            item.EnchantLevel = enchant;
                            item.Count = count;

                            ps = con.prepareStatement("INSERT INTO character_warehouse SET " + "id = ?," + "account_name = ?," + "item_id = ?," + "item_name = ?," + "count = ?," + "is_equipped=0," + "enchantlvl = ?," + "is_id = ?," + "durability = ?," + "charge_count = ?," + "remaining_time = ?," + "last_used = ?," + "bless = ?," + "attr_enchant_kind = ?," + "attr_enchant_level = ?," + "firemr = ?," + "watermr = ?," + "earthmr = ?," + "windmr = ?," + "addsp = ?," + "addhp = ?," + "addmp = ?," + "hpr = ?," + "mpr = ?," + "m_def = ?");

                            ps.setInt(1, item.Id);
                            ps.setString(2, account);
                            ps.setInt(3, item.ItemId);
                            ps.setString(4, item.Name);
                            ps.setInt(5, item.Count);
                            ps.setInt(6, item.EnchantLevel);
                            ps.setInt(7, item.Identified ? 1 : 0);
                            ps.setInt(8, item.get_durability());
                            ps.setInt(9, item.ChargeCount);
                            ps.setInt(10, item.RemainingTime);
                            ps.setTimestamp(11, item.LastUsed);
                            ps.setInt(12, item.Bless);
                            ps.setInt(13, item.AttrEnchantKind);
                            ps.setInt(14, item.AttrEnchantLevel);
                            ps.setInt(15, item.FireMr);
                            ps.setInt(16, item.WaterMr);
                            ps.setInt(17, item.EarthMr);
                            ps.setInt(18, item.WindMr);
                            ps.setInt(19, item.getaddSp());
                            ps.setInt(20, item.getaddHp());
                            ps.setInt(21, item.getaddMp());
                            ps.setInt(22, item.Hpr);
                            ps.setInt(23, item.Mpr);
                            ps.setInt(24, item.M_Def);
                            ps.execute();
                        }
                        else
                        {
                            L1ItemInstance item = null;
                            int createCount;

                            for (createCount = 0; createCount < count; createCount++)
                            {
                                item = ItemTable.Instance.createItem(itemid);
                                item.EnchantLevel = enchant;

                                ps = con.prepareStatement("INSERT INTO character_warehouse SET " + "id = ?," + "account_name = ?," + "item_id = ?," + "item_name = ?," + "count = ?," + "is_equipped=0," + "enchantlvl = ?," + "is_id = ?," + "durability = ?," + "charge_count = ?," + "remaining_time = ?," + "last_used = ?," + "bless = ?," + "attr_enchant_kind = ?," + "attr_enchant_level = ?," + "firemr = ?," + "watermr = ?," + "earthmr = ?," + "windmr = ?," + "addsp = ?," + "addhp = ?," + "addmp = ?," + "hpr = ?," + "mpr = ?," + "m_def = ?");

                                ps.setInt(1, item.Id);
                                ps.setString(2, account);
                                ps.setInt(3, item.ItemId);
                                ps.setString(4, item.Name);
                                ps.setInt(5, item.Count);
                                ps.setInt(6, item.EnchantLevel);
                                ps.setInt(7, item.Identified ? 1 : 0);
                                ps.setInt(8, item.get_durability());
                                ps.setInt(9, item.ChargeCount);
                                ps.setInt(10, item.RemainingTime);
                                ps.setTimestamp(11, item.LastUsed);
                                ps.setInt(12, item.Bless);
                                ps.setInt(13, item.AttrEnchantKind);
                                ps.setInt(14, item.AttrEnchantLevel);
                                ps.setInt(15, item.FireMr);
                                ps.setInt(16, item.WaterMr);
                                ps.setInt(17, item.EarthMr);
                                ps.setInt(18, item.WindMr);
                                ps.setInt(19, item.getaddSp());
                                ps.setInt(20, item.getaddHp());
                                ps.setInt(21, item.getaddMp());
                                ps.setInt(22, item.Hpr);
                                ps.setInt(23, item.Mpr);
                                ps.setInt(24, item.M_Def);
                                ps.execute();
                            }
                        }
                    }

                    con.commit();
                    con.AutoCommit = true;
                }
                catch (SQLException e)
                {
                    try
                    {
                        con.rollback();
                    }
                    catch (SQLException)
                    {
                    }

                    _log.log(Enum.Level.Server, e.Message, e);
                    throw new Exception(".present 處理時發生了例外的錯誤。");
                }
                finally
                {
                    SQLUtil.close(ps);
                    SQLUtil.close(con);
                }
            }
        }
    }

}