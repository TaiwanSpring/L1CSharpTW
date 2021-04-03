
using LineageServer.Interfaces;
using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來增加交易物品的封包
    /// </summary>
    class C_TradeAddItem : ClientBasePacket
    {
        private const string C_TRADE_ADD_ITEM = "[C] C_TradeAddItem";
        public C_TradeAddItem(byte[] abyte0, ClientThread client) : base(abyte0)
        {

            L1PcInstance pc = client.ActiveChar;
            if (pc == null)
            {
                return;
            }

            int itemid = ReadD();
            int itemcount = ReadD();

            L1Trade trade = new L1Trade();
            L1ItemInstance item = pc.Inventory.getItem(itemid);
            if (!item.Item.Tradable)
            {
                pc.sendPackets(new S_ServerMessage(210, item.Item.Name)); // \f1%0は捨てたりまたは他人に讓ることができません。
                return;
            }
            if (item.Bless >= 128)
            { // 封印的裝備
              // \f1%0は捨てたりまたは他人に讓ることができません。
                pc.sendPackets(new S_ServerMessage(210, item.Item.Name));
                return;
            }
            // 使用中的寵物項鍊 - 無法交易
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
            // 使用中的魔法娃娃 - 無法交易
            foreach (L1DollInstance doll in pc.DollList.Values)
            {
                if (doll.ItemObjId == item.Id)
                {
                    pc.sendPackets(new S_ServerMessage(1181)); // 這個魔法娃娃目前正在使用中。
                    return;
                }
            }

            L1PcInstance tradingPartner = (L1PcInstance)Container.Instance.Resolve<IGameWorld>().findObject(pc.TradeID);
            if (tradingPartner == null)
            {
                return;
            }
            if (pc.TradeOk)
            {
                return;
            }
            if (tradingPartner.Inventory.checkAddItem(item, itemcount) != L1Inventory.OK)
            { // 檢查容量與重量
                tradingPartner.sendPackets(new S_ServerMessage(270)); // \f1持っているものが重くて取引できません。
                pc.sendPackets(new S_ServerMessage(271)); // \f1相手が物を持ちすぎていて取引できません。
                return;
            }

            trade.TradeAddItem(pc, itemid, itemcount);
        }

        public override string Type
        {
            get
            {
                return C_TRADE_ADD_ITEM;
            }
        }
    }

}