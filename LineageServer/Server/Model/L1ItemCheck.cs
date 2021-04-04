using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.Instance;
using System;
using System.Collections.Generic;

namespace LineageServer.Server.Model
{
    /// <summary>
    /// 負責物品狀態檢查是否作弊
    /// </summary>
    class L1ItemCheck
    {
        private readonly static IDataSource weaponDataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Weapon);
        private readonly static IDataSource armorDataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Armor);
        private readonly static IDataSource etcitemDataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Etcitem);
        private int itemId;
        private bool isStackable = false;

        public virtual bool ItemCheck(L1ItemInstance item, L1PcInstance pc)
        {
            itemId = item.Item.ItemId;
            int itemCount = item.Count;
            bool isCheat = false;

            if ((findWeapon() || findArmor()) && itemCount != 1)
            {
                isCheat = true;
            }
            else if (findEtcItem())
            {
                // 不可堆疊的道具卻堆疊，就視為作弊
                if (!isStackable && itemCount != 1)
                {
                    isCheat = true;
                    // 金幣大於20億以及金幣負值則為作弊
                }
                else if (itemId == L1ItemId.ADENA && (itemCount > 2000000000 || itemCount < 0))
                {
                    isCheat = true;
                    // 可堆疊道具(金幣除外)堆疊超過十萬個以及堆疊負值設定為作弊
                }
                else if (isStackable && itemId != L1ItemId.ADENA && (itemCount > 100000 || itemCount < 0))
                {
                    isCheat = true;
                }
            }
            if (isCheat)
            {
                // 作弊直接刪除物品
                pc.Inventory.removeItem(item, itemCount);
            }
            return isCheat;
        }

        private bool findWeapon()
        {
            IList<IDataSourceRow> dataSourceRows = weaponDataSource.Select()
                .Where(Weapon.Column_item_id, itemId).Query();

            return dataSourceRows.Count > 0;
        }

        private bool findArmor()
        {
            IList<IDataSourceRow> dataSourceRows = armorDataSource.Select()
                .Where(Armor.Column_item_id, itemId).Query();

            return dataSourceRows.Count > 0;
        }

        private bool findEtcItem()
        {
            IList<IDataSourceRow> dataSourceRows = etcitemDataSource.Select()
              .Where(Etcitem.Column_item_id, itemId).Query();

            return dataSourceRows.Count > 0;
        }
    }
}