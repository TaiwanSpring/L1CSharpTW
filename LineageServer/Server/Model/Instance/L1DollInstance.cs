using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using LineageServer.Utils;

namespace LineageServer.Server.Model.Instance
{
    class L1DollInstance : L1NpcInstance
    {
        public const int DOLL_TIME = 1800000;
        private int _itemId;
        private int _itemObjId;
        private int run;
        private bool _isDelete = false;

        // ターゲットがいない場合の処理
        public override bool noTarget()
        {
            if ((_master != null) && !_master.Dead && (_master.MapId == MapId))
            {
                if (Location.getTileLineDistance(_master.Location) > 2)
                {
                    int dir = moveDirection(_master.X, _master.Y);
                    DirectionMove = dir;
                    SleepTime = calcSleepTime(Passispeed, MOVE_SPEED);
                }
                else
                {
                    // 魔法娃娃 - 特殊動作
                    dollAction();
                }
            }
            else
            {
                _isDelete = true;
                deleteDoll();
                return true;
            }
            return false;
        }

        // 時間計測用
        internal class DollTimer : IRunnable
        {
            private readonly L1DollInstance outerInstance;

            public DollTimer(L1DollInstance outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public void run()
            {
                if (outerInstance._destroyed)
                { // 既に破棄されていないかチェック
                    return;
                }
                outerInstance.deleteDoll();
            }
        }

        public L1DollInstance(L1Npc template, L1PcInstance master, int itemId, int itemObjId) : base(template)
        {
            Id = IdFactory.Instance.nextId();

            ItemId = itemId;
            ItemObjId = itemObjId;
            RunnableExecuter.Instance.execute(new DollTimer(this), DOLL_TIME);

            Master = master;
            X = master.X + RandomHelper.Next(5) - 2;
            Y = master.Y + RandomHelper.Next(5) - 2;
            MapId = master.MapId;
            Heading = 5;
            LightSize = template.LightSize;
            MoveSpeed = 1;
            BraveSpeed = 1;

            L1World.Instance.storeObject(this);
            L1World.Instance.addVisibleObject(this);
            foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(this))
            {
                onPerceive(pc);
            }
            master.addDoll(this);
            if (!AiRunning)
            {
                startAI();
            }
            if (L1MagicDoll.isHpRegeneration(_master))
            {
                master.startHpRegenerationByDoll();
            }
            if (L1MagicDoll.isMpRegeneration(_master))
            {
                master.startMpRegenerationByDoll();
            }
            if (L1MagicDoll.isItemMake(_master))
            {
                master.startItemMakeByDoll();
            }
        }

        public virtual void deleteDoll()
        {
            broadcastPacket(new S_SkillSound(Id, 5936));
            if (_master != null && _isDelete)
            {
                L1PcInstance pc = (L1PcInstance)_master;
                pc.sendPackets(new S_SkillIconGFX(56, 0));
                pc.sendPackets(new S_OwnCharStatus(pc));
            }
            if (L1MagicDoll.isHpRegeneration(_master))
            {
                ((L1PcInstance)_master).stopHpRegenerationByDoll();
            }
            if (L1MagicDoll.isMpRegeneration(_master))
            {
                ((L1PcInstance)_master).stopMpRegenerationByDoll();
            }
            if (L1MagicDoll.isItemMake(_master))
            {
                ((L1PcInstance)_master).stopItemMakeByDoll();
            }
            _master.DollList.Remove(Id);
            deleteMe();
        }

        public override void onPerceive(L1PcInstance perceivedFrom)
        {
            // 判斷旅館內是否使用相同鑰匙
            if (perceivedFrom.MapId >= 16384 && perceivedFrom.MapId <= 25088 && perceivedFrom.InnKeyId != _master.InnKeyId)
            {
                return;
            }
            perceivedFrom.addKnownObject(this);
            perceivedFrom.sendPackets(new S_DollPack(this));
        }

        public override void onItemUse()
        {
        }

        public override void onGetItem(L1ItemInstance item)
        {
        }

        public virtual int ItemObjId
        {
            get
            {
                return _itemObjId;
            }
            set
            {
                _itemObjId = value;
            }
        }


        public virtual int ItemId
        {
            get
            {
                return _itemId;
            }
            set
            {
                _itemId = value;
            }
        }


        // 表情動作
        private void dollAction()
        {
            run = RandomHelper.Next(100) + 1;
            if (run <= 10)
            {
                int actionCode = ActionCodes.ACTION_Aggress; // 67
                if (run <= 5)
                {
                    actionCode = ActionCodes.ACTION_Think; // 66
                }

                broadcastPacket(new S_DoActionGFX(Id, actionCode));
                SleepTime = calcSleepTime(SprTable.Instance.getSprSpeed(TempCharGfx, actionCode), MOVE_SPEED);
            }
        }
    }

}