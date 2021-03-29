using LineageServer.Server.Model.Instance;
using System.Collections.Generic;
namespace LineageServer.Server.Storage
{
    abstract class CharactersItemStorage
    {
        public abstract IList<L1ItemInstance> loadItems(int objId);
        public abstract void storeItem(int objId, L1ItemInstance item);
        public abstract void deleteItem(L1ItemInstance item);
        public abstract void updateItemId(L1ItemInstance item);
        public abstract void updateItemCount(L1ItemInstance item);
        public abstract void updateItemIdentified(L1ItemInstance item);
        public abstract void updateItemEquipped(L1ItemInstance item);
        public abstract void updateItemEnchantLevel(L1ItemInstance item);
        public abstract void updateItemDurability(L1ItemInstance item);
        public abstract void updateItemChargeCount(L1ItemInstance item);
        public abstract void updateItemRemainingTime(L1ItemInstance item);
        public abstract void updateItemDelayEffect(L1ItemInstance item);
        public abstract int getItemCount(int objId);
        public abstract void updateItemBless(L1ItemInstance item);
        public abstract void updateItemAttrEnchantKind(L1ItemInstance item);
        public abstract void updateItemAttrEnchantLevel(L1ItemInstance item);
        public abstract void updateaddHp(L1ItemInstance item);
        public abstract void updateaddMp(L1ItemInstance item);
        public abstract void updateHpr(L1ItemInstance item);
        public abstract void updateMpr(L1ItemInstance item); public abstract void updateFireMr(L1ItemInstance item);
        public abstract void updateWaterMr(L1ItemInstance item);
        public abstract void updateEarthMr(L1ItemInstance item);
        public abstract void updateWindMr(L1ItemInstance item);
        public abstract void updateaddSp(L1ItemInstance item);
        public abstract void updateM_Def(L1ItemInstance item);

        public static CharactersItemStorage create()
        {
            if (_instance == null)
            {
                _instance = new MySqlCharactersItemStorage();
            }
            return _instance;
        }

        private static CharactersItemStorage _instance;
    }

}