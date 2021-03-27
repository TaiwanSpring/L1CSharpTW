using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
using System.Collections.Generic;

namespace LineageServer.Server.Model
{
    class L1GroundInventory : L1Inventory
    {
        /// 
        private const long serialVersionUID = 1L;

        private IDictionary<int, DeletionTimer> _reservedTimers = MapFactory.NewMap<int, DeletionTimer>();

        private class DeletionTimer : TimerTask
        {
            private readonly L1GroundInventory outerInstance;

            internal readonly L1ItemInstance _item;

            public DeletionTimer(L1GroundInventory outerInstance, L1ItemInstance item)
            {
                this.outerInstance = outerInstance;
                _item = item;
            }

            public override void run()
            {
                try
                {
                    lock (outerInstance)
                    {
                        if (!outerInstance._items.Contains(_item))
                        { // 拾われたタイミングによってはこの条件を満たし得る
                            return; // 既に拾われている
                        }
                        outerInstance.removeItem(_item);
                    }
                }
                catch (Exception e)
                {
                    _log.Error(e);
                }
            }
        }

        private L1ItemInstance Timer
        {
            set
            {
                if (Config.ALT_ITEM_DELETION_TYPE != "std")
                {
                    return;
                }

                if (value.ItemId == 40515)
                { // 精霊の石
                    return;
                }

                RunnableExecuter.Instance.execute(new DeletionTimer(this, value), Config.ALT_ITEM_DELETION_TIME * 60 * 1000);
            }
        }

        private void cancelTimer(L1ItemInstance item)
        {
            DeletionTimer timer = _reservedTimers[item.Id];
            if (timer == null)
            {
                return;
            }
            timer.cancel();
        }

        public L1GroundInventory(int objectId, int x, int y, short map)
        {
            Id = objectId;
            X = x;
            Y = y;
            MapId = map;
            L1World.Instance.addVisibleObject(this);
        }

        public override void onPerceive(L1PcInstance perceivedFrom)
        {
            foreach (L1ItemInstance item in Items)
            {
                if (!perceivedFrom.knownsObject(item))
                {
                    perceivedFrom.addKnownObject(item);
                    perceivedFrom.sendPackets(new S_DropItem(item)); // プレイヤーへDROPITEM情報を通知
                }
            }
        }

        // 認識範囲内にいるプレイヤーへオブジェクト送信
        public override void insertItem(L1ItemInstance item)
        {
            Timer = item;

            foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(item))
            {
                pc.sendPackets(new S_DropItem(item));
                pc.addKnownObject(item);
            }
        }

        // 見える範囲内にいるプレイヤーのオブジェクト更新
        public override void updateItem(L1ItemInstance item)
        {
            foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(item))
            {
                pc.sendPackets(new S_DropItem(item));
            }
        }

        // 空インベントリ破棄及び見える範囲内にいるプレイヤーのオブジェクト削除
        public override void deleteItem(L1ItemInstance item)
        {
            cancelTimer(item);
            foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(item))
            {
                pc.sendPackets(new S_RemoveObject(item));
                pc.removeKnownObject(item);
            }

            _items.Remove(item);
            if (_items.Count == 0)
            {
                L1World.Instance.removeVisibleObject(this);
            }
        }

        //JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
        private static ILogger _log = Logger.GetLogger(nameof(L1PcInventory));
    }

}