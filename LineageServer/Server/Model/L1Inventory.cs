using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.Map;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Collections.Generic;

namespace LineageServer.Server.Model
{
    [Serializable]
    class L1Inventory : GameObject
    {

        private const long serialVersionUID = 1L;

        protected internal IList<L1ItemInstance> _items = ListFactory.NewConcurrentList<L1ItemInstance>();

        public const int MAX_AMOUNT = 2000000000; // 2G

        public const int MAX_WEIGHT = 1500;

        public L1Inventory()
        {
            //
        }

        // インベントリ内のアイテムの総数
        public virtual int Size
        {
            get
            {
                return _items.Count;
            }
        }

        // インベントリ内の全てのアイテム
        public virtual IList<L1ItemInstance> Items
        {
            get
            {
                return _items;
            }
        }

        // インベントリ内の総重量
        public virtual int Weight
        {
            get
            {
                int weight = 0;

                foreach (L1ItemInstance item in _items)
                {
                    weight += item.Weight;
                }

                return weight;
            }
        }

        // 引数のアイテムを追加しても容量と重量が大丈夫か確認
        public const int OK = 0;

        public const int SIZE_OVER = 1;

        public const int WEIGHT_OVER = 2;

        public const int AMOUNT_OVER = 3;

        public virtual int checkAddItem(L1ItemInstance item, int count)
        {
            if (item == null)
            {
                return -1;
            }
            if ((item.Count <= 0) || (count <= 0))
            {
                return -1;
            }
            if ((Size > Config.MAX_NPC_ITEM) || ((Size == Config.MAX_NPC_ITEM) && (!item.Stackable || !checkItem(item.Item.ItemId))))
            { // 容量確認
                return SIZE_OVER;
            }

            int weight = Weight + item.Item.Weight * count / 1000 + 1;
            if ((weight < 0) || ((item.Item.Weight * count / 1000) < 0))
            {
                return WEIGHT_OVER;
            }
            if (weight > (MAX_WEIGHT * Config.RATE_WEIGHT_LIMIT_PET))
            { // その他の重量確認（主にサモンとペット）
                return WEIGHT_OVER;
            }

            L1ItemInstance itemExist = findItemId(item.ItemId);
            if ((itemExist != null) && ((itemExist.Count + count) > MAX_AMOUNT))
            {
                return AMOUNT_OVER;
            }

            return OK;
        }

        // 引数のアイテムを追加しても倉庫の容量が大丈夫か確認
        public const int WAREHOUSE_TYPE_PERSONAL = 0;

        public const int WAREHOUSE_TYPE_CLAN = 1;

        public virtual int checkAddItemToWarehouse(L1ItemInstance item, int count, int type)
        {
            if (item == null)
            {
                return -1;
            }
            if ((item.Count <= 0) || (count <= 0))
            {
                return -1;
            }

            int maxSize = 100;
            if (type == WAREHOUSE_TYPE_PERSONAL)
            {
                maxSize = Config.MAX_PERSONAL_WAREHOUSE_ITEM;
            }
            else if (type == WAREHOUSE_TYPE_CLAN)
            {
                maxSize = Config.MAX_CLAN_WAREHOUSE_ITEM;
            }
            if ((Size > maxSize) || ((Size == maxSize) && (!item.Stackable || !checkItem(item.Item.ItemId))))
            { // 容量確認
                return SIZE_OVER;
            }

            return OK;
        }

        // 新しいアイテムの格納
        public virtual L1ItemInstance storeItem(int id, int count)
        {
            lock (this)
            {
                if (count <= 0)
                {
                    return null;
                }
                L1Item temp = ItemTable.Instance.getTemplate(id);
                if (temp == null)
                {
                    return null;
                }

                if (id == 40312)
                {
                    L1ItemInstance item = new L1ItemInstance(temp, count);

                    if (findKeyId(id) == null)
                    { // 新しく生成する必要がある場合のみIDの発行とL1Worldへの登録を行う
                        item.Id = IdFactory.Instance.nextId();
                        L1World.Instance.storeObject(item);
                    }

                    return storeItem(item);
                }
                else if (temp.Stackable)
                {
                    L1ItemInstance item = new L1ItemInstance(temp, count);

                    if (findItemId(id) == null)
                    { // 新しく生成する必要がある場合のみIDの発行とL1Worldへの登録を行う
                        item.Id = IdFactory.Instance.nextId();
                        L1World.Instance.storeObject(item);
                    }

                    return storeItem(item);
                }

                // スタックできないアイテムの場合
                L1ItemInstance result = null;
                for (int i = 0; i < count; i++)
                {
                    L1ItemInstance item = new L1ItemInstance(temp, 1);
                    item.Id = IdFactory.Instance.nextId();
                    L1World.Instance.storeObject(item);
                    storeItem(item);
                    result = item;
                }
                // 最後に作ったアイテムを返す。配列を戻すようにメソッド定義を変更したほうが良いかもしれない。
                return result;
            }
        }

        // DROP、購入、GMコマンドで入手した新しいアイテムの格納
        public virtual L1ItemInstance storeItem(L1ItemInstance item)
        {
            lock (this)
            {
                if (item.Count <= 0)
                {
                    return null;
                }
                int itemId = item.Item.ItemId;
                if (item.Stackable)
                {
                    L1ItemInstance findItem = findItemId(itemId);
                    if (itemId == 40309)
                    { // Race Tickets
                        findItem = findItemNameId(item.Item.IdentifiedNameId);
                    }
                    else if (itemId == 40312)
                    { // 旅館鑰匙
                        findItem = findKeyId(itemId);
                    }
                    else
                    {
                        findItem = findItemId(itemId);
                    }
                    if (findItem != null)
                    {
                        findItem.Count = findItem.Count + item.Count;
                        updateItem(findItem);
                        return findItem;
                    }
                }

                if (itemId == 40309)
                { // Race Tickets
                    string[] temp = item.Item.IdentifiedNameId.Split(" ", true);
                    temp = temp[temp.Length - 1].Split("-", true);
                    L1RaceTicket ticket = new L1RaceTicket();
                    ticket.set_itemobjid(item.Id);
                    ticket.set_round(int.Parse(temp[0]));
                    ticket.set_allotment_percentage(0.0);
                    ticket.set_victory(0);
                    ticket.set_runner_num(int.Parse(temp[1]));
                    RaceTicketTable.Instance.storeNewTiket(ticket);
                }
                item.X = X;
                item.Y = Y;
                item.Map = L1WorldMap.Instance.getMap(MapId);
                int chargeCount = item.Item.MaxChargeCount;
                if ((itemId == 40006) || (itemId == 40007) || (itemId == 40008) || (itemId == 140006) || (itemId == 140008) || (itemId == 41401))
                {
                    chargeCount -= RandomHelper.Next(5);
                }
                if (itemId == 20383)
                {
                    chargeCount = 50;
                }
                item.ChargeCount = chargeCount;
                if ((item.Item.Type2 == 0) && (item.Item.Type == 2))
                { // light系アイテム
                    item.RemainingTime = item.Item.LightFuel;
                }
                else
                {
                    item.RemainingTime = item.Item.MaxUseTime;
                }
                item.Bless = item.Item.Bless;
                // 登入鑰匙紀錄
                if (item.Item.ItemId == 40312)
                {
                    if (!InnKeyTable.checkey(item))
                    {
                        InnKeyTable.StoreKey(item);
                    }
                }
                _items.Add(item);
                insertItem(item);
                return item;
            }
        }

        // /trade、倉庫から入手したアイテムの格納
        public virtual L1ItemInstance storeTradeItem(L1ItemInstance item)
        {
            lock (this)
            {
                if (item.Item.ItemId == 40312)
                { // 旅館鑰匙
                    L1ItemInstance findItem = findKeyId(item.KeyId); // 檢查鑰匙編號是否相同
                    if (findItem != null)
                    {
                        findItem.Count = findItem.Count + item.Count;
                        updateItem(findItem);
                        return findItem;
                    }
                }
                else if (item.Stackable)
                {
                    L1ItemInstance findItem = findItemId(item.Item.ItemId);
                    if (findItem != null)
                    {
                        findItem.Count = findItem.Count + item.Count;
                        updateItem(findItem);
                        return findItem;
                    }
                }
                item.X = X;
                item.Y = Y;
                item.Map = L1WorldMap.Instance.getMap(MapId);
                // 登入鑰匙紀錄
                if (item.Item.ItemId == 40312)
                {
                    if (!InnKeyTable.checkey(item))
                    {
                        InnKeyTable.StoreKey(item);
                    }
                }
                _items.Add(item);
                insertItem(item);
                return item;
            }
        }

        /// <summary>
        /// インベントリから指定されたアイテムIDのアイテムを削除する。L1ItemInstanceへの参照
        /// がある場合はremoveItemの方を使用するのがよい。 （こちらは矢とか魔石とか特定のアイテムを消費させるときに使う）
        /// </summary>
        /// <param name="itemid">
        ///            - 削除するアイテムのitemid(objidではない) </param>
        /// <param name="count">
        ///            - 削除する個数 </param>
        /// <returns> 実際に削除された場合はtrueを返す。 </returns>
        public virtual bool consumeItem(int itemid, int count)
        {
            if (count <= 0)
            {
                return false;
            }
            if (ItemTable.Instance.getTemplate(itemid).Stackable)
            {
                L1ItemInstance item = findItemId(itemid);
                if ((item != null) && (item.Count >= count))
                {
                    removeItem(item, count);
                    return true;
                }
            }
            else
            {
                L1ItemInstance[] itemList = findItemsId(itemid);
                if (itemList.Length == count)
                {
                    for (int i = 0; i < count; i++)
                    {
                        removeItem(itemList[i], 1);
                    }
                    return true;
                }
                else if (itemList.Length > count)
                { // 指定個数より多く所持している場合
                    DataComparator<L1ItemInstance> dc = new DataComparator<L1ItemInstance>(this);
                    Array.Sort(itemList, dc); // エンチャント順にソートし、エンチャント数の少ないものから消費させる
                    for (int i = 0; i < count; i++)
                    {
                        removeItem(itemList[i], 1);
                    }
                    return true;
                }
            }
            return false;
        }

        public class DataComparator<T> : IComparer<L1ItemInstance>
        {
            private readonly L1Inventory outerInstance;

            public DataComparator(L1Inventory outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public virtual int Compare(L1ItemInstance item1, L1ItemInstance item2)
            {
                return item1.EnchantLevel - item2.EnchantLevel;
            }
        }

        // 指定したアイテムから指定個数を削除（使ったりゴミ箱に捨てられたとき）戻り値：実際に削除した数
        public virtual int removeItem(int objectId, int count)
        {
            L1ItemInstance item = getItem(objectId);
            return removeItem(item, count);
        }

        public virtual int removeItem(L1ItemInstance item)
        {
            return removeItem(item, item.Count);
        }

        public virtual int removeItem(L1ItemInstance item, int count)
        {
            if (item == null)
            {
                return 0;
            }
            if ((item.Count <= 0) || (count <= 0))
            {
                return 0;
            }
            if (item.Count < count)
            {
                count = item.Count;
            }
            if (item.Count == count)
            {
                int itemId = item.Item.ItemId;
                if ((itemId == 40314) || (itemId == 40316))
                { // ペットのアミュレット
                    PetTable.Instance.deletePet(item.Id);
                }
                else if ((itemId >= 49016) && (itemId <= 49025))
                { // 便箋
                    LetterTable lettertable = new LetterTable();
                    lettertable.deleteLetter(item.Id);
                }
                else if ((itemId >= 41383) && (itemId <= 41400))
                { // 家具
                    foreach (GameObject l1object in L1World.Instance.Object)
                    {
                        if (l1object is L1FurnitureInstance)
                        {
                            L1FurnitureInstance furniture = (L1FurnitureInstance)l1object;
                            if (furniture.ItemObjId == item.Id)
                            { // 既に引き出している家具
                                FurnitureSpawnTable.Instance.deleteFurniture(furniture);
                            }
                        }
                    }
                }
                else if (item.ItemId == 40309)
                { // Race Tickets
                    RaceTicketTable.Instance.deleteTicket(item.Id);
                }
                deleteItem(item);
                L1World.Instance.removeObject(item);
            }
            else
            {
                item.Count = item.Count - count;
                updateItem(item);
            }
            return count;
        }

        // _itemsから指定オブジェクトを削除(L1PcInstance、L1DwarfInstance、L1GroundInstanceでこの部分をオーバライドする)
        public virtual void deleteItem(L1ItemInstance item)
        {
            // 刪除鑰匙紀錄
            if (item.Item.ItemId == 40312)
            {
                InnKeyTable.DeleteKey(item);
            }
            _items.Remove(item);
        }

        // 引数のインベントリにアイテムを移譲
        public virtual L1ItemInstance tradeItem(int objectId, int count, L1Inventory inventory)
        {
            lock (this)
            {
                L1ItemInstance item = getItem(objectId);
                return tradeItem(item, count, inventory);
            }
        }

        public virtual L1ItemInstance tradeItem(L1ItemInstance item, int count, L1Inventory inventory)
        {
            lock (this)
            {
                if (item == null)
                {
                    return null;
                }
                if ((item.Count <= 0) || (count <= 0))
                {
                    return null;
                }
                if (item.Equipped)
                {
                    return null;
                }
                if (!checkItem(item.Item.ItemId, count))
                {
                    return null;
                }
                L1ItemInstance carryItem;
                if (item.Count <= count)
                {
                    deleteItem(item);
                    carryItem = item;
                }
                else
                {
                    item.Count = item.Count - count;
                    updateItem(item);
                    carryItem = ItemTable.Instance.createItem(item.Item.ItemId);
                    carryItem.Count = count;
                    carryItem.EnchantLevel = item.EnchantLevel;
                    carryItem.Identified = item.Identified;
                    carryItem.set_durability(item.get_durability());
                    carryItem.ChargeCount = item.ChargeCount;
                    carryItem.RemainingTime = item.RemainingTime;
                    carryItem.LastUsed = item.LastUsed;
                    carryItem.Bless = item.Bless;
                    // 旅館鑰匙
                    if (carryItem.Item.ItemId == 40312)
                    {
                        carryItem.InnNpcId = item.InnNpcId; // 旅館NPC
                        carryItem.KeyId = item.KeyId; // 鑰匙編號
                        carryItem.Hall = item.checkRoomOrHall(); // 房間或會議室
                        carryItem.DueTime = item.DueTime; // 租用時間
                    }
                }
                return inventory.storeTradeItem(carryItem);
            }
        }

        /*
		 * アイテムを損傷・損耗させる（武器・防具も含む） アイテムの場合、損耗なのでマイナスするが 武器・防具は損傷度を表すのでプラスにする。
		 */
        public virtual L1ItemInstance receiveDamage(int objectId)
        {
            L1ItemInstance item = getItem(objectId);
            return receiveDamage(item);
        }

        public virtual L1ItemInstance receiveDamage(L1ItemInstance item)
        {
            return receiveDamage(item, 1);
        }

        public virtual L1ItemInstance receiveDamage(L1ItemInstance item, int count)
        {
            int itemType = item.Item.Type2;
            int currentDurability = item.get_durability();

            if (((currentDurability == 0) && (itemType == 0)) || (currentDurability < 0))
            {
                item.set_durability(0);
                return null;
            }

            // 武器・防具のみ損傷度をプラス
            if (itemType == 0)
            {
                int minDurability = (item.EnchantLevel + 5) * -1;
                int durability = currentDurability - count;
                if (durability < minDurability)
                {
                    durability = minDurability;
                }
                if (currentDurability > durability)
                {
                    item.set_durability(durability);
                }
            }
            else
            {
                int maxDurability = item.EnchantLevel + 5;
                int durability = currentDurability + count;
                if (durability > maxDurability)
                {
                    durability = maxDurability;
                }
                if (currentDurability < durability)
                {
                    item.set_durability(durability);
                }
            }

            updateItem(item, L1PcInventory.COL_DURABILITY);
            return item;
        }

        public virtual L1ItemInstance recoveryDamage(L1ItemInstance item)
        {
            if (item == null)
            {
                return null;
            }

            int itemType = item.Item.Type2;
            int durability = item.get_durability();

            if (((durability == 0) && (itemType != 0)) || (durability < 0))
            {
                item.set_durability(0);
                return null;
            }

            if (itemType == 0)
            {
                // 耐久度をプラスしている。
                item.set_durability(durability + 1);
            }
            else
            {
                // 損傷度をマイナスしている。
                item.set_durability(durability - 1);
            }

            updateItem(item, L1PcInventory.COL_DURABILITY);
            return item;
        }

        // アイテムＩＤから検索
        public virtual L1ItemInstance findItemId(int id)
        {
            foreach (L1ItemInstance item in _items)
            {
                if (item.Item.ItemId == id)
                {
                    return item;
                }
            }
            return null;
        }

        public virtual L1ItemInstance findKeyId(int id)
        {
            foreach (L1ItemInstance item in _items)
            {
                if (item.KeyId == id)
                {
                    return item;
                }
            }
            return null;
        }

        public virtual L1ItemInstance[] findItemsId(int id)
        {
            IList<L1ItemInstance> itemList = ListFactory.NewList<L1ItemInstance>();
            foreach (L1ItemInstance item in _items)
            {
                if (item.ItemId == id)
                {
                    itemList.Add(item);
                }
            }
            return ((List<L1ItemInstance>)itemList).ToArray();
        }

        public virtual L1ItemInstance[] findItemsIdNotEquipped(int id)
        {
            IList<L1ItemInstance> itemList = ListFactory.NewList<L1ItemInstance>();
            foreach (L1ItemInstance item in _items)
            {
                if (item.ItemId == id)
                {
                    if (!item.Equipped)
                    {
                        itemList.Add(item);
                    }
                }
            }
            return ((List<L1ItemInstance>)itemList).ToArray();
        }

        // オブジェクトＩＤから検索
        public virtual L1ItemInstance getItem(int objectId)
        {
            foreach (object itemObject in _items)
            {
                L1ItemInstance item = (L1ItemInstance)itemObject;
                if (item.Id == objectId)
                {
                    return item;
                }
            }
            return null;
        }

        // 特定のアイテムを指定された個数以上所持しているか確認（矢とか魔石の確認）
        public virtual bool checkItem(int id)
        {
            return checkItem(id, 1);
        }

        public virtual bool checkItem(int id, int count)
        {
            if (count == 0)
            {
                return true;
            }
            if (ItemTable.Instance.getTemplate(id).Stackable)
            {
                L1ItemInstance item = findItemId(id);
                if ((item != null) && (item.Count >= count))
                {
                    return true;
                }
            }
            else
            {
                object[] itemList = findItemsId(id);
                if (itemList.Length >= count)
                {
                    return true;
                }
            }
            return false;
        }

        // 強化された特定のアイテムを指定された個数以上所持しているか確認
        // 装備中のアイテムは所持していないと判別する
        public virtual bool checkEnchantItem(int id, int enchant, int count)
        {
            int num = 0;
            foreach (L1ItemInstance item in _items)
            {
                if (item.Equipped)
                { // 装備しているものは該当しない
                    continue;
                }
                if ((item.ItemId == id) && (item.EnchantLevel == enchant))
                {
                    num++;
                    if (num == count)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // 強化された特定のアイテムを消費する
        // 装備中のアイテムは所持していないと判別する
        public virtual bool consumeEnchantItem(int id, int enchant, int count)
        {
            foreach (L1ItemInstance item in _items)
            {
                if (item.Equipped)
                { // 装備しているものは該当しない
                    continue;
                }
                if ((item.ItemId == id) && (item.EnchantLevel == enchant))
                {
                    removeItem(item);
                    return true;
                }
            }
            return false;
        }

        // 特定のアイテムを指定された個数以上所持しているか確認
        // 装備中のアイテムは所持していないと判別する
        public virtual bool checkItemNotEquipped(int id, int count)
        {
            if (count == 0)
            {
                return true;
            }
            return count <= countItems(id);
        }

        // 特定のアイテムを全て必要な個数所持しているか確認（イベントとかで複数のアイテムを所持しているか確認するため）
        public virtual bool checkItem(int[] ids)
        {
            int len = ids.Length;
            int[] counts = new int[len];
            for (int i = 0; i < len; i++)
            {
                counts[i] = 1;
            }
            return checkItem(ids, counts);
        }

        public virtual bool checkItem(int[] ids, int[] counts)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                if (!checkItem(ids[i], counts[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// このインベントリ内にある、指定されたIDのアイテムの数を数える。
        /// 
        /// @return
        /// </summary>
        public virtual int countItems(int id)
        {
            if (ItemTable.Instance.getTemplate(id).Stackable)
            {
                L1ItemInstance item = findItemId(id);
                if (item != null)
                {
                    return item.Count;
                }
            }
            else
            {
                object[] itemList = findItemsIdNotEquipped(id);
                return itemList.Length;
            }
            return 0;
        }

        public virtual void shuffle()
        {
            _items.Shuffle();
        }

        // インベントリ内の全てのアイテムを消す（所有者を消すときなど）
        public virtual void clearItems()
        {
            foreach (object itemObject in _items)
            {
                L1ItemInstance item = (L1ItemInstance)itemObject;
                L1World.Instance.removeObject(item);
            }
            _items.Clear();
        }

        /// <summary>
        /// スタック可能なアイテムリストからnameIdと同じ値を持つitemを返す
        /// </summary>
        /// <param name="nameId"> </param>
        /// <returns> item null 如果沒有找到。 </returns>
        public virtual L1ItemInstance findItemNameId(string nameId)
        {
            foreach (L1ItemInstance item in _items)
            {
                if (nameId.Equals(item.Item.IdentifiedNameId))
                {
                    return item;
                }
            }
            return null;
        }

        // オーバーライド用
        public virtual void loadItems()
        {
        }

        public virtual void insertItem(L1ItemInstance item)
        {
        }

        public virtual void updateItem(L1ItemInstance item)
        {
        }

        public virtual void updateItem(L1ItemInstance item, int colmn)
        {
        }

        public virtual void updateEnchantAccessory(L1ItemInstance item, int colmn)
        {
        }

    }

}