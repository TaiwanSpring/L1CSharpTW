using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
using System.Collections.Generic;
using System.Threading;

namespace LineageServer.Server.Model
{
    class L1DeleteItemOnGround : IGameComponent
    {
        private DeleteTimer _deleteTimer;

        private static readonly ILogger _log = Logger.GetLogger(nameof(L1DeleteItemOnGround));

        private class DeleteTimer : IRunnable
        {
            private readonly L1DeleteItemOnGround outerInstance;

            public DeleteTimer(L1DeleteItemOnGround outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public void run()
            {
                if (Config.ALT_ITEM_DELETION_TIME <= 0)
                {

                }
                else
                {
                    TimeSpan time = TimeSpan.FromMinutes(Config.ALT_ITEM_DELETION_TIME);

                    while (true)
                    {
                        Thread.Sleep(time);

                        Container.Instance.Resolve<IGameWorld>().broadcastPacketToAll(new S_ServerMessage(166, L1Message.onGroundItem, L1Message.secondsDelete + "。"));

                        Thread.Sleep(10 * 1000);

                        outerInstance.deleteItem();

                        Container.Instance.Resolve<IGameWorld>().broadcastPacketToAll(new S_ServerMessage(166, L1Message.onGroundItem, L1Message.deleted + "。"));
                    }
                }
            }
        }

        public virtual void Initialize()
        {
            if (Config.ALT_ITEM_DELETION_TYPE != "auto")
            {
                return;
            }

            _deleteTimer = new DeleteTimer(this);
            Container.Instance.Resolve<ITaskController>().execute(_deleteTimer); // タイマー開始
        }

        private void deleteItem()
        {
            int numOfDeleted = 0;
            foreach (GameObject obj in Container.Instance.Resolve<IGameWorld>().Object)
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

                    IList<L1PcInstance> players = Container.Instance.Resolve<IGameWorld>().getVisiblePlayer(item, Config.ALT_ITEM_DELETION_RANGE);
                    if (players.Count == 0)
                    { // 指定範囲内にプレイヤーが居なければ削除
                        L1Inventory groundInventory = Container.Instance.Resolve<IGameWorld>().getInventory(item.X, item.Y, item.MapId);
                        groundInventory.removeItem(item);
                        numOfDeleted++;
                    }
                }
            }
            _log.Log("ワールドマップ上のアイテムを自動削除。削除数: " + numOfDeleted);
        }
    }

}