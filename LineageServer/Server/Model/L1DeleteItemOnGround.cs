using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
using System.Collections.Generic;
using System.Threading;

namespace LineageServer.Server.Model
{
    class L1DeleteItemOnGround
    {
        private DeleteTimer _deleteTimer;

        private static readonly ILogger _log = Logger.GetLogger(nameof(L1DeleteItemOnGround));

        public L1DeleteItemOnGround()
        {
        }

        private class DeleteTimer : IRunnable
        {
            private readonly L1DeleteItemOnGround outerInstance;

            public DeleteTimer(L1DeleteItemOnGround outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public void run()
            {
                int time = Config.ALT_ITEM_DELETION_TIME * 60 * 1000 - 10 * 1000;
                for (; ; )
                {
                    try
                    {
                        Thread.Sleep(time);
                    }
                    catch (Exception exception)
                    {
                        _log.Warning("L1DeleteItemOnGround error: " + exception);
                        break;
                    }
                    L1World.Instance.broadcastPacketToAll(new S_ServerMessage(166, L1Message.onGroundItem, L1Message.secondsDelete + "。"));
                    try
                    {
                        Thread.Sleep(10000);
                    }
                    catch (Exception exception)
                    {
                        _log.Warning("L1DeleteItemOnGround error: " + exception);
                        break;
                    }
                    outerInstance.deleteItem();
                    L1World.Instance.broadcastPacketToAll(new S_ServerMessage(166, L1Message.onGroundItem, L1Message.deleted + "。"));
                }
            }
        }

        public virtual void initialize()
        {
            if (Config.ALT_ITEM_DELETION_TYPE != "auto")
            {
                return;
            }

            _deleteTimer = new DeleteTimer(this);
            RunnableExecuter.Instance.execute(_deleteTimer); // タイマー開始
        }

        private void deleteItem()
        {
            int numOfDeleted = 0;
            foreach (GameObject obj in L1World.Instance.Object)
            {
                if (obj is L1ItemInstance item)
                {
                    if (item.X == 0 && item.Y == 0)
                    { // 地面上のアイテムではなく、誰かの所有物
                        continue;
                    }
                    if (item.Item.ItemId == 40515)
                    { // 精霊の石
                        continue;
                    }
                    if (L1HouseLocation.isInHouse(item.X, item.Y, item.MapId))
                    { // アジト内
                        continue;
                    }

                    IList<L1PcInstance> players = L1World.Instance.getVisiblePlayer(item, Config.ALT_ITEM_DELETION_RANGE);
                    if (players.Count == 0)
                    { // 指定範囲内にプレイヤーが居なければ削除
                        L1Inventory groundInventory = L1World.Instance.getInventory(item.X, item.Y, item.MapId);
                        groundInventory.removeItem(item);
                        numOfDeleted++;
                    }
                }
            }
            _log.Log("ワールドマップ上のアイテムを自動削除。削除数: " + numOfDeleted);
        }
    }

}