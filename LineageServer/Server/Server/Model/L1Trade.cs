using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.utils;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.Server.Model
{
    class L1Trade
    {
        private static L1Trade _instance;

        public L1Trade()
        {
        }

        public static L1Trade Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new L1Trade();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 加入交易物品 </summary>
        /// <param name="player">
        ///          自己 </param>
        /// <param name="itemid">
        ///          欲交易出去的物品 </param>
        /// <param name="itemcount">
        ///          物品數量 </param>
        public virtual void TradeAddItem(L1PcInstance player, int itemid, int itemcount)
        {
            //未在交易狀態中
            if (player.TradeID == 0)
            {
                return;
            }

            // 交易對象
            L1PcInstance trading_partner = (L1PcInstance)L1World.Instance.findObject(player.TradeID);
            if (trading_partner == null)
            {
                TradeCancel(player);
                return;
            }

            L1ItemInstance l1iteminstance = player.Inventory.getItem(itemid);
            if (l1iteminstance == null)
            {
                return;
            }
            // 裝備中, 不可交易
            if (l1iteminstance.Equipped)
            {
                return;
            }

            itemcount = Math.Abs(itemcount); // 確保物品數量為正
            itemcount = Math.Min(itemcount, l1iteminstance.Count); // 確保交易的物品小於身上所有

            if (itemcount < 0 || itemcount > 2000000000 || l1iteminstance.Count < 0 || l1iteminstance.Count < itemcount)
            {
                return;
            }

            if ((l1iteminstance.Count < itemcount) || (0 > itemcount))
            {
                player.sendPackets(new S_TradeStatus(1));
                trading_partner.sendPackets(new S_TradeStatus(1));
                player.TradeOk = false;
                trading_partner.TradeOk = false;
                player.TradeID = 0;
                trading_partner.TradeID = 0;
                return;
            }
            player.Inventory.tradeItem(l1iteminstance, itemcount, player.TradeWindowInventory);
            player.sendPackets(new S_TradeAddItem(l1iteminstance, itemcount, 0));
            trading_partner.sendPackets(new S_TradeAddItem(l1iteminstance, itemcount, 1));
        }

        /// <summary>
        /// 交易完成 </summary>
        /// <param name="player"> </param>
        public virtual void TradeOK(L1PcInstance player)
        {
            //未在交易狀態中
            if (player.TradeID == 0)
            {
                return;
            }

            // 交易對象
            L1PcInstance trading_partner = (L1PcInstance)L1World.Instance.findObject(player.TradeID);
            if (trading_partner == null)
            {
                TradeCancel(player);
                return;
            }

            IList<L1ItemInstance> player_tradelist = player.TradeWindowInventory.Items;
            int player_tradecount = player.TradeWindowInventory.Size;

            IList<L1ItemInstance> trading_partner_tradelist = trading_partner.TradeWindowInventory.Items;
            int trading_partner_tradecount = trading_partner.TradeWindowInventory.Size;

            for (int cnt = 0; cnt < player_tradecount; cnt++)
            {
                L1ItemInstance l1iteminstance1 = player_tradelist[0];
                player.TradeWindowInventory.tradeItem(l1iteminstance1, l1iteminstance1.Count, trading_partner.Inventory);
                // 交易紀錄
                if (Config.writeTradeLog)
                {
                    LogRecorder.writeTradeLog(player, trading_partner, l1iteminstance1);
                }
            }
            for (int cnt = 0; cnt < trading_partner_tradecount; cnt++)
            {
                L1ItemInstance l1iteminstance2 = trading_partner_tradelist[0];
                trading_partner.TradeWindowInventory.tradeItem(l1iteminstance2, l1iteminstance2.Count, player.Inventory);
                // 交易紀錄
                if (Config.writeTradeLog)
                {
                    LogRecorder.writeTradeLog(trading_partner, player, l1iteminstance2);
                }
            }

            player.sendPackets(new S_TradeStatus(0));
            trading_partner.sendPackets(new S_TradeStatus(0));
            player.TradeOk = false;
            trading_partner.TradeOk = false;
            player.TradeID = 0;
            trading_partner.TradeID = 0;
            player.turnOnOffLight();
            trading_partner.turnOnOffLight();
        }

        /// <summary>
        /// 取消交易 </summary>
        /// <param name="player"> </param>
        public virtual void TradeCancel(L1PcInstance player)
        {
            //未在交易狀態中
            if (player.TradeID == 0)
            {
                return;
            }

            // 交易對象
            L1PcInstance trading_partner = (L1PcInstance)L1World.Instance.findObject(player.TradeID);

            //自己的欲交易的物品, 放回自己的包包
            IList<L1ItemInstance> player_tradelist = player.TradeWindowInventory.Items;
            int player_tradecount = player.TradeWindowInventory.Size;

            for (int cnt = 0; cnt < player_tradecount; cnt++)
            {
                L1ItemInstance l1iteminstance1 = player_tradelist[0];
                player.TradeWindowInventory.tradeItem(l1iteminstance1, l1iteminstance1.Count, player.Inventory);
            }
            player.sendPackets(new S_TradeStatus(1));
            player.TradeOk = false;
            player.TradeID = 0;


            //交易對象的欲交易的物品, 放回他自己的包包
            if (trading_partner != null)
            {
                IList<L1ItemInstance> trading_partner_tradelist = trading_partner.TradeWindowInventory.Items;
                int trading_partner_tradecount = trading_partner.TradeWindowInventory.Size;

                for (int cnt = 0; cnt < trading_partner_tradecount; cnt++)
                {
                    L1ItemInstance l1iteminstance2 = trading_partner_tradelist[0];
                    trading_partner.TradeWindowInventory.tradeItem(l1iteminstance2, l1iteminstance2.Count, trading_partner.Inventory);
                }

                trading_partner.sendPackets(new S_TradeStatus(1));
                trading_partner.TradeOk = false;
                trading_partner.TradeID = 0;
            }
        }
    }

}