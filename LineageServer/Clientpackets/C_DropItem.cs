using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Utils;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來丟道具到地上的封包
    /// </summary>
    class C_DropItem : ClientBasePacket
    {
        private const string C_DROP_ITEM = "[C] C_DropItem";
        private readonly ILogger logger = Logger.GetLogger(nameof(C_DropItem));

        public C_DropItem(byte[] decrypt, ClientThread client) : base(decrypt)
        {
            L1PcInstance pc = client.ActiveChar;
            if (pc == null)
            {
                return;
            }

            int x = ReadH();
            int y = ReadH();
            int objectId = ReadD();
            int count = ReadD();

            if (count > 0x77359400 || count < 0)
            { // 確保數量不會溢位
                count = 0;
            }

            if (pc.Ghost)
            {
                return;
            }
            else if (pc.MapId >= 16384 && pc.MapId <= 25088)
            { // 旅館內判斷
                pc.sendPackets(new S_ServerMessage(539)); // \f1你無法將它放在這。
                return;
            }

            L1ItemInstance item = pc.Inventory.getItem(objectId);
            if (item != null)
            {
                L1ItemCheck checkItem = new L1ItemCheck(); // 物品狀態檢查
                if (checkItem.ItemCheck(item, pc))
                { // 是否作弊
                    return;
                }
                if (!item.Item.Tradable)
                {
                    // \f1%0%d是不可轉移的…
                    pc.sendPackets(new S_ServerMessage(210, item.Item.Name));
                    return;
                }

                // 使用中的寵物項鍊 - 無法丟棄
                foreach (L1NpcInstance petNpc in pc.PetList.Values)
                {
                    if (petNpc is L1PetInstance)
                    {
                        L1PetInstance pet = (L1PetInstance)petNpc;
                        if (item.Id == pet.ItemObjId)
                        {
                            pc.sendPackets(new S_ServerMessage(1187)); // 寵物項鍊正在使用中。
                            return;
                        }
                    }
                }
                // 使用中的魔法娃娃 - 無法丟棄
                foreach (L1DollInstance doll in pc.DollList.Values)
                {
                    if (doll.ItemObjId == item.Id)
                    {
                        pc.sendPackets(new S_ServerMessage(1181)); // 這個魔法娃娃目前正在使用中。
                        return;

                    }
                }

                if (item.Equipped)
                {
                    // \f1你不能夠放棄此樣物品。
                    pc.sendPackets(new S_ServerMessage(125));
                    return;
                }
                if (item.Bless >= 128)
                { // 封印的裝備
                  // \f1%0%d是不可轉移的…
                    pc.sendPackets(new S_ServerMessage(210, item.Item.Name));
                    return;
                }
                //TODO 物品丟地上系統會自動刪除 by bill00148 
                if (Config.Drop_Item)
                {
                    if (pc.Level <= Config.DropItemMinLv)
                    {
                        pc.Inventory.removeItem(objectId, count);
                        pc.sendPackets(new S_SystemMessage("物品請勿丟地上，已遭系統刪除。"));
                        return;
                    }
                }
                //end
                // 交易紀錄
                if (Config.writeDropLog)
                {
                    logger.Info($"{pc.Name} drop {item.Id}");
                }

                pc.Inventory.tradeItem(item, count,
                    Container.Instance.Resolve<IGameWorld>().getInventory(x, y, pc.MapId));
                pc.turnOnOffLight();
            }
        }

        public override string Type
        {
            get
            {
                return C_DROP_ITEM;
            }
        }
    }

}