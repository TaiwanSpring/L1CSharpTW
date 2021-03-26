using LineageServer.Enum;
using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model.map;
using LineageServer.Server.Server.Model.npc.action;
using LineageServer.Server.Server.Model.skill;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Templates;
using LineageServer.Server.Server.Utils.collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace LineageServer.Server.Server.Model.Instance
{
    [Serializable]
    class L1NpcInstance : L1Character
    {
        private const long serialVersionUID = 1L;

        public const int MOVE_SPEED = 0;

        public const int ATTACK_SPEED = 1;

        public const int MAGIC_SPEED = 2;

        public const int HIDDEN_STATUS_NONE = 0;

        public const int HIDDEN_STATUS_SINK = 1;

        public const int HIDDEN_STATUS_FLY = 2;

        public const int HIDDEN_STATUS_ICE = 3;

        public const int CHAT_TIMING_APPEARANCE = 0;

        public const int CHAT_TIMING_DEAD = 1;

        public const int CHAT_TIMING_HIDE = 2;

        public const int CHAT_TIMING_GAME_TIME = 3;

        //JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
        private static ILogger _log = Logger.getLogger(nameof(L1NpcInstance));

        private L1Npc _npcTemplate;

        private L1Spawn _spawn;

        private int _spawnNumber; // L1Spawnで管理されているナンバー

        private int _petcost; // ペットになったときのコスト

        public L1Inventory _inventory = new L1Inventory();

        private L1MobSkillUse mobSkill;

        // 対象を初めて発見したとき。（テレポート用）
        private bool firstFound = true;

        // 経路探索範囲（半径） ※上げすぎ注意！！
        public static int courceRange = 15;

        // 吸われたMP
        private int _drainedMana = 0;

        // 休憩
        private bool _rest = false;

        // ランダム移動時の距離と方向
        private int _randomMoveDistance = 0;

        private int _randomMoveDirection = 0;

        // ■■■■■■■■■■■■■ ＡＩ関連 ■■■■■■■■■■■

        internal interface NpcAI
        {
            void start();
        }

        protected internal virtual void startAI()
        {
            if (Config.NPCAI_IMPLTYPE == 1)
            {
                (new NpcAITimerImpl(this)).start();
            }
            else if (Config.NPCAI_IMPLTYPE == 2)
            {
                (new NpcAIThreadImpl(this)).start();
            }
            else
            {
                (new NpcAITimerImpl(this)).start();
            }
        }

        /// <summary>
        /// マルチ(コア)プロセッサをサポートする為のタイマープール。 AIの実装タイプがタイマーの場合に使用される。
        /// </summary>
        //private static readonly TimerPool _timerPool = new TimerPool(4);

        internal class NpcAITimerImpl : TimerTask, NpcAI
        {
            private readonly L1NpcInstance outerInstance;

            public NpcAITimerImpl(L1NpcInstance outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            /// <summary>
            /// 死亡処理の終了を待つタイマー
            /// </summary>
            private class DeathSyncTimer : TimerTask
            {
                private readonly L1NpcInstance.NpcAITimerImpl outerInstance;

                public DeathSyncTimer(L1NpcInstance.NpcAITimerImpl outerInstance)
                {
                    this.outerInstance = outerInstance;
                }

                internal virtual void schedule(int delay)
                {
                    RunnableExecuter.Instance.execute(new DeathSyncTimer(outerInstance), delay);
                }

                public override void run()
                {
                    if (outerInstance.outerInstance.DeathProcessing)
                    {
                        schedule(outerInstance.outerInstance.SleepTime);
                        return;
                    }
                    outerInstance.outerInstance.allTargetClear();
                    outerInstance.outerInstance.AiRunning = false;
                }
            }

            public virtual void start()
            {
                outerInstance.AiRunning = true;
                RunnableExecuter.Instance.execute(this, 0);
            }

            internal virtual void stop()
            {
                outerInstance.mobSkill.resetAllSkillUseCount();
                RunnableExecuter.Instance.execute(new DeathSyncTimer(this), 0); // 死亡同期を開始
            }

            // 同じインスタンスをTimerへ登録できない為、苦肉の策。
            internal virtual void schedule(int delay)
            {
                RunnableExecuter.Instance.execute(new NpcAITimerImpl(outerInstance), delay);
            }

            public void run()
            {
                try
                {
                    if (notContinued())
                    {
                        stop();
                        return;
                    }

                    // XXX 同期がとても怪しげな麻痺判定
                    if (0 < outerInstance._paralysisTime)
                    {
                        schedule(outerInstance._paralysisTime);
                        outerInstance._paralysisTime = 0;
                        outerInstance.Paralyzed = false;
                        return;
                    }
                    else if (outerInstance.Paralyzed || outerInstance.Sleeped)
                    {
                        schedule(200);
                        return;
                    }

                    if (!outerInstance.AIProcess())
                    { // AIを続けるべきであれば、次の実行をスケジュールし、終了
                        schedule(outerInstance.SleepTime);
                        return;
                    }
                    stop();
                }
                catch (Exception e)
                {
                    _log.log(Level.WARNING, "NpcAIで例外が発生しました。", e);
                }
            }

            internal virtual bool notContinued()
            {
                return outerInstance._destroyed || outerInstance.Dead || (outerInstance.CurrentHp <= 0) || (outerInstance.HiddenStatus != HIDDEN_STATUS_NONE);
            }
        }

        internal class NpcAIThreadImpl : IRunnable, NpcAI
        {
            private readonly L1NpcInstance outerInstance;

            public NpcAIThreadImpl(L1NpcInstance outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public virtual void start()
            {
                RunnableExecuter.Instance.execute(this);
            }

            public void run()
            {
                try
                {
                    outerInstance.AiRunning = true;
                    while (!outerInstance._destroyed && !outerInstance.Dead && (outerInstance.CurrentHp > 0) && (outerInstance.HiddenStatus == HIDDEN_STATUS_NONE))
                    {
                        /*
						 * if (_paralysisTime > 0) { try {
						 * Thread.sleep(_paralysisTime); } catch (Exception
						 * exception) { break; } finally { setParalyzed(false);
						 * _paralysisTime = 0; } }
						 */
                        while (outerInstance.Paralyzed || outerInstance.Sleeped)
                        {
                            Thread.Sleep(200);
                        }

                        if (outerInstance.AIProcess())
                        {
                            break;
                        }
                        try
                        {
                            // 指定時間分スレッド停止
                            Thread.Sleep(outerInstance.SleepTime);
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                    outerInstance.mobSkill.resetAllSkillUseCount();
                    do
                    {
                        try
                        {
                            Thread.Sleep(outerInstance.SleepTime);
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    } while (outerInstance.DeathProcessing);
                    outerInstance.allTargetClear();
                    outerInstance.AiRunning = false;
                }
                catch (Exception e)
                {
                    _log.log(Level.WARNING, "NpcAIで例外が発生しました。", e);
                }
            }
        }

        // ＡＩの処理 (返り値はＡＩ処理を終了するかどうか)
        private bool AIProcess()
        {
            SleepTime = 300;

            checkTarget();
            if ((_target == null) && (_master == null))
            {
                // 空っぽの場合はターゲットを探してみる
                // （主人がいる場合は自分でターゲットを探さない）
                searchTarget();
            }

            onDoppel(true);
            onItemUse();

            if (_target == null)
            {
                // ターゲットがいない場合
                checkTargetItem();
                if (PickupItem && (_targetItem == null))
                {
                    // アイテム拾う子の場合はアイテムを探してみる
                    searchTargetItem();
                }

                if (_targetItem == null)
                {
                    if (noTarget())
                    {
                        return true;
                    }
                }
                else
                {
                    // onTargetItem();
                    L1Inventory groundInventory = L1World.Instance.getInventory(_targetItem.X, _targetItem.Y, _targetItem.MapId);
                    if (groundInventory.checkItem(_targetItem.ItemId))
                    {
                        onTargetItem();
                    }
                    else
                    {
                        _targetItemList.Remove(_targetItem);
                        _targetItem = null;
                        SleepTime = 1000;
                        return false;
                    }
                }
            }
            else
            { // ターゲットがいる場合
                if (HiddenStatus == HIDDEN_STATUS_NONE)
                {
                    onTarget();
                }
                else
                {
                    return true;
                }
            }

            return false; // ＡＩ処理続行
        }

        // 變形怪變形
        public virtual void onDoppel(bool isChangeShape)
        {
        }

        // アイテム使用処理（Ｔｙｐｅによって結構違うのでオーバライドで実装）
        public virtual void onItemUse()
        {
        }

        // ターゲットを探す（Ｔｙｐｅによって結構違うのでオーバライドで実装）
        public virtual void searchTarget()
        {
            tagertClear();
        }

        // 有効なターゲットか確認及び次のターゲットを設定
        public virtual void checkTarget()
        {
            if ((_target == null) || (_target.MapId != MapId) || (_target.CurrentHp <= 0) || _target.Dead || (_target.Invisble && !NpcTemplate.is_agrocoi() && !_hateList.containsKey(_target)) || _target.getTileLineDistance(this) > 30)
            {
                if (_target != null)
                {
                    tagertClear();
                }
                if (!_hateList.Empty)
                {
                    _target = _hateList.MaxHateCharacter;
                    checkTarget();
                }
            }
        }

        // ヘイトの設定
        public virtual void setHate(L1Character cha, int hate)
        {
            if ((cha != null) && (cha.Id != Id))
            {
                if (!FirstAttack && (hate != 0))
                {
                    // hate += 20; // ＦＡヘイト
                    hate += MaxHp / 10; // ＦＡヘイト
                    FirstAttack = true;
                }

                _hateList.add(cha, hate);
                _dropHateList.add(cha, hate);
                _target = _hateList.MaxHateCharacter;
                checkTarget();
            }
        }

        // リンクの設定
        public virtual L1Character Link
        {
            set
            {
            }
        }

        // 仲間意識によりアクティブになるＮＰＣの検索（攻撃者がプレイヤーのみ有効）
        public virtual void serchLink(L1PcInstance targetPlayer, int family)
        {
            IList<L1Object> targetKnownObjects = targetPlayer.KnownObjects;
            foreach (object knownObject in targetKnownObjects)
            {
                if (knownObject is L1NpcInstance)
                {
                    L1NpcInstance npc = (L1NpcInstance)knownObject;
                    if (npc.NpcTemplate.get_agrofamily() > 0)
                    {
                        // 仲間に対してアクティブになる場合
                        if (npc.NpcTemplate.get_agrofamily() == 1)
                        {
                            // 同種族に対してのみ仲間意識
                            if (npc.NpcTemplate.get_family() == family)
                            {
                                npc.Link = targetPlayer;
                            }
                        }
                        else
                        {
                            // 全てのＮＰＣに対して仲間意識
                            npc.Link = targetPlayer;
                        }
                    }
                    L1MobGroupInfo mobGroupInfo = MobGroupInfo;
                    if (mobGroupInfo != null)
                    {
                        if ((MobGroupId != 0) && (MobGroupId == npc.MobGroupId))
                        { // 同じグループ
                            npc.Link = targetPlayer;
                        }
                    }
                }
            }
        }

        // ターゲットがいる場合の処理
        public virtual void onTarget()
        {
            Actived = true;
            _targetItemList.Clear();
            _targetItem = null;
            L1Character target = _target; // ここから先は_targetが変わると影響出るので別領域に参照確保
            if (Atkspeed == 0)
            { // 逃げるキャラ
                if (Passispeed > 0)
                { // 移動できるキャラ
                    int escapeDistance = 15;
                    if (hasSkillEffect(L1SkillId.DARKNESS) == true)
                    {
                        escapeDistance = 1;
                    }
                    if (Location.getTileLineDistance(target.Location) > escapeDistance)
                    { // ターゲットから逃げるの終了
                        tagertClear();
                    }
                    else
                    { // ターゲットから逃げる
                        int dir = targetReverseDirection(target.X, target.Y);
                        dir = checkObject(X, Y, MapId, dir);
                        DirectionMove = dir;
                        SleepTime = calcSleepTime(Passispeed, MOVE_SPEED);
                    }
                }
            }
            else
            { // 逃げないキャラ
                if (isAttackPosition(target.X, target.Y, AtkRanged))
                { // 攻撃可能位置
                    if (mobSkill.isSkillTrigger(target))
                    { // トリガの条件に合うスキルがある
                        if (mobSkill.skillUse(target, true))
                        { // スキル使用(mobskill.sqlのTriRndに従う)
                            SleepTime = calcSleepTime(mobSkill.SleepTime, MAGIC_SPEED);
                        }
                        else
                        { // スキル使用が失敗したら物理攻撃
                            Heading = targetDirection(target.X, target.Y);
                            attackTarget(target);
                        }
                    }
                    else
                    {
                        Heading = targetDirection(target.X, target.Y);
                        attackTarget(target);
                    }
                }
                else
                { // 攻撃不可能位置
                    if (mobSkill.skillUse(target, false))
                    { // スキル使用(mobskill.sqlのTriRndに従わず、発動確率は100%。ただしサモン、強制変身は常にTriRndに従う。)
                        SleepTime = calcSleepTime(mobSkill.SleepTime, MAGIC_SPEED);
                        return;
                    }

                    if (Passispeed > 0)
                    {
                        // 移動できるキャラ
                        int distance = Location.getTileDistance(target.Location);
                        if ((firstFound == true) && NpcTemplate.is_teleport() && (distance > 3) && (distance < 15))
                        {
                            if (nearTeleport(target.X, target.Y) == true)
                            {
                                firstFound = false;
                                return;
                            }
                        }

                        if (NpcTemplate.is_teleport() && (20 > RandomHelper.Next(100)) && (CurrentMp >= 10) && (distance > 6) && (distance < 15))
                        { // テレポート移動
                            if (nearTeleport(target.X, target.Y) == true)
                            {
                                return;
                            }
                        }
                        int dir = moveDirection(target.X, target.Y);
                        if (dir == -1)
                        {
                            // 假如怪物走不過去  就找附近下一個玩家攻擊
                            searchTarget();
                        }
                        else
                        {
                            DirectionMove = dir;
                            SleepTime = calcSleepTime(Passispeed, MOVE_SPEED);
                        }
                    }
                    else
                    {
                        // 移動できないキャラ（ターゲットから排除、ＰＴのときドロップチャンスがリセットされるけどまぁ自業自得）
                        tagertClear();
                    }
                }
            }
        }

        // 目標を指定のスキルで攻撃
        public virtual void attackTarget(L1Character target)
        {
            if (target is L1PcInstance)
            {
                L1PcInstance player = (L1PcInstance)target;
                if (player.Teleport)
                { // テレポート処理中
                    return;
                }
            }
            else if (target is L1PetInstance)
            {
                L1PetInstance pet = (L1PetInstance)target;
                L1Character cha = pet.Master;
                if (cha is L1PcInstance)
                {
                    L1PcInstance player = (L1PcInstance)cha;
                    if (player.Teleport)
                    { // テレポート処理中
                        return;
                    }
                }
            }
            else if (target is L1SummonInstance)
            {
                L1SummonInstance summon = (L1SummonInstance)target;
                L1Character cha = summon.Master;
                if (cha is L1PcInstance)
                {
                    L1PcInstance player = (L1PcInstance)cha;
                    if (player.Teleport)
                    { // テレポート処理中
                        return;
                    }
                }
            }
            if (this is L1PetInstance)
            {
                L1PetInstance pet = (L1PetInstance)this;
                L1Character cha = pet.Master;
                if (cha is L1PcInstance)
                {
                    L1PcInstance player = (L1PcInstance)cha;
                    if (player.Teleport)
                    { // テレポート処理中
                        return;
                    }
                }
            }
            else if (this is L1SummonInstance)
            {
                L1SummonInstance summon = (L1SummonInstance)this;
                L1Character cha = summon.Master;
                if (cha is L1PcInstance)
                {
                    L1PcInstance player = (L1PcInstance)cha;
                    if (player.Teleport)
                    { // テレポート処理中
                        return;
                    }
                }
            }

            if (target is L1NpcInstance)
            {
                L1NpcInstance npc = (L1NpcInstance)target;
                if (npc.HiddenStatus != HIDDEN_STATUS_NONE)
                { // 地中に潜っているか、飛んでいる
                    allTargetClear();
                    return;
                }
            }

            bool isCounterBarrier = false;
            L1Attack attack = new L1Attack(this, target);
            if (attack.calcHit())
            {
                if (target.hasSkillEffect(L1SkillId.COUNTER_BARRIER))
                {
                    L1Magic magic = new L1Magic(target, this);
                    bool isProbability = magic.calcProbabilityMagic(L1SkillId.COUNTER_BARRIER);
                    bool isShortDistance = attack.ShortDistance;
                    if (isProbability && isShortDistance)
                    {
                        isCounterBarrier = true;
                    }
                }
                if (!isCounterBarrier)
                {
                    attack.calcDamage();
                }
            }
            if (isCounterBarrier)
            {
                attack.actionCounterBarrier();
                attack.commitCounterBarrier();
            }
            else
            {
                attack.action();
                attack.commit();
            }
            SleepTime = calcSleepTime(Atkspeed, ATTACK_SPEED);
        }

        // ターゲットアイテムを探す
        public virtual void searchTargetItem()
        {
            IList<L1GroundInventory> gInventorys = Lists.newList<L1GroundInventory>();

            foreach (L1Object obj in L1World.Instance.getVisibleObjects(this))
            {
                if ((obj != null) && (obj is L1GroundInventory))
                {
                    gInventorys.Add((L1GroundInventory)obj);
                }
            }
            if (gInventorys.Count == 0)
            {
                return;
            }

            // 拾うアイテム(のインベントリ)をランダムで選定
            int pickupIndex = RandomHelper.Next(gInventorys.Count);
            L1GroundInventory inventory = gInventorys[pickupIndex];
            foreach (L1ItemInstance item in inventory.Items)
            {
                if (Inventory.checkAddItem(item, item.Count) == L1Inventory.OK)
                { // 持てるならターゲットアイテムに加える
                    _targetItem = item;
                    _targetItemList.Add(_targetItem);
                }
            }
        }

        public virtual void searchItemFromAir()
        { // 怪物飛天中，發現特定道具時解除飛天撿拾道具
            IList<L1GroundInventory> gInventorys = Lists.newList<L1GroundInventory>();

            foreach (L1Object obj in L1World.Instance.getVisibleObjects(this))
            {
                if ((obj != null) && (obj is L1GroundInventory))
                {
                    gInventorys.Add((L1GroundInventory)obj);
                }
            }
            if (gInventorys.Count == 0)
            {
                return;
            }

            int pickupIndex = RandomHelper.Next(gInventorys.Count);
            L1GroundInventory inventory = gInventorys[pickupIndex];
            foreach (L1ItemInstance item in inventory.Items)
            {
                if ((item.Item.Type == 6) || (item.Item.Type == 7))
                { // food
                    if (Inventory.checkAddItem(item, item.Count) == L1Inventory.OK)
                    {
                        if (HiddenStatus == HIDDEN_STATUS_FLY)
                        {
                            HiddenStatus = HIDDEN_STATUS_NONE;
                            Status = L1NpcDefaultAction.Instance.getStatus(TempCharGfx);
                            broadcastPacket(new S_DoActionGFX(Id, ActionCodes.ACTION_Movedown));
                            onNpcAI();
                            startChat(CHAT_TIMING_HIDE);
                            _targetItem = item;
                            _targetItemList.Add(_targetItem);
                        }
                    }
                }
            }
        }

        public static void shuffle(L1Object[] arr)
        {
            for (int i = arr.Length - 1; i > 0; i--)
            {
                int t = RandomHelper.Next(i);

                // 選ばれた値と交換する
                L1Object tmp = arr[i];
                arr[i] = arr[t];
                arr[t] = tmp;
            }
        }

        // 有効なターゲットアイテムか確認及び次のターゲットアイテムを設定
        public virtual void checkTargetItem()
        {
            if ((_targetItem == null) || (_targetItem.MapId != MapId) || (Location.getTileDistance(_targetItem.Location) > 15))
            {
                if (_targetItemList.Count > 0)
                {
                    _targetItem = _targetItemList[0];
                    _targetItemList.RemoveAt(0);
                    checkTargetItem();
                }
                else
                {
                    _targetItem = null;
                }
            }
        }

        // ターゲットアイテムがある場合の処理
        public virtual void onTargetItem()
        {
            if (Location.getTileLineDistance(_targetItem.Location) == 0)
            { // ピックアップ可能位置
                pickupTargetItem(_targetItem);
            }
            else
            { // ピックアップ不可能位置
                int dir = moveDirection(_targetItem.X, _targetItem.Y);
                if (dir == -1)
                { // 拾うの諦め
                    _targetItemList.Remove(_targetItem);
                    _targetItem = null;
                }
                else
                { // ターゲットアイテムへ移動
                    DirectionMove = dir;
                    SleepTime = calcSleepTime(Passispeed, MOVE_SPEED);
                }
            }
        }

        // アイテムを拾う
        public virtual void pickupTargetItem(L1ItemInstance targetItem)
        {
            L1Inventory groundInventory = L1World.Instance.getInventory(targetItem.X, targetItem.Y, targetItem.MapId);
            L1ItemInstance item = groundInventory.tradeItem(targetItem, targetItem.Count, Inventory);
            turnOnOffLight();
            onGetItem(item);
            _targetItemList.Remove(_targetItem);
            _targetItem = null;
            SleepTime = 1000;
        }

        // ターゲットがいない場合の処理 (返り値はＡＩ処理を終了するかどうか)
        public virtual bool noTarget()
        {
            if ((_master != null) && (_master.MapId == MapId) && (Location.getTileLineDistance(_master.Location) > 2))
            { // 主人が同じマップにいて離れてる場合は追尾
                int dir = moveDirection(_master.X, _master.Y);
                if (dir != -1)
                {
                    DirectionMove = dir;
                    SleepTime = calcSleepTime(Passispeed, MOVE_SPEED);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (L1World.Instance.getRecognizePlayer(this).Count == 0)
                {
                    return true; // 周りにプレイヤーがいなくなったらＡＩ処理終了
                }
                // 移動できるキャラはランダムに動いておく
                if ((_master == null) && (Passispeed > 0) && !Rest)
                {
                    // グループに属していないorグループに属していてリーダーの場合、ランダムに動いておく
                    L1MobGroupInfo mobGroupInfo = MobGroupInfo;
                    if ((mobGroupInfo == null) || ((mobGroupInfo != null) && mobGroupInfo.isLeader(this)))
                    {
                        // 移動する予定の距離を移動し終えたら、新たに距離と方向を決める
                        // そうでないなら、移動する予定の距離をデクリメント
                        if (_randomMoveDistance == 0)
                        {
                            _randomMoveDistance = RandomHelper.Next(5) + 1;
                            _randomMoveDirection = RandomHelper.Next(20);
                            // ホームポイントから離れすぎないように、一定の確率でホームポイントの方向に補正
                            if ((HomeX != 0) && (HomeY != 0) && (_randomMoveDirection < 8) && (RandomHelper.Next(3) == 0))
                            {
                                _randomMoveDirection = moveDirection(HomeX, HomeY);
                            }
                        }
                        else
                        {
                            _randomMoveDistance--;
                        }
                        int dir = checkObject(X, Y, MapId, _randomMoveDirection);
                        if (dir != -1)
                        {
                            DirectionMove = dir;
                            SleepTime = calcSleepTime(Passispeed, MOVE_SPEED);
                        }
                    }
                    else
                    { // リーダーを追尾
                        L1NpcInstance leader = mobGroupInfo.Leader;
                        if (Location.getTileLineDistance(leader.Location) > 2)
                        {
                            int dir = moveDirection(leader.X, leader.Y);
                            if (dir == -1)
                            {
                                return true;
                            }
                            else
                            {
                                DirectionMove = dir;
                                SleepTime = calcSleepTime(Passispeed, MOVE_SPEED);
                            }
                        }
                    }
                }
            }
            return false;
        }

        public virtual void onFinalAction(L1PcInstance pc, string s)
        {
        }

        // 現在のターゲットを削除
        public virtual void tagertClear()
        {
            if (_target == null)
            {
                return;
            }
            _hateList.remove(_target);
            _target = null;
        }

        // 指定されたターゲットを削除
        public virtual void targetRemove(L1Character target)
        {
            _hateList.remove(target);
            if ((_target != null) && _target.Equals(target))
            {
                _target = null;
            }
        }

        // 全てのターゲットを削除
        public virtual void allTargetClear()
        {
            _hateList.clear();
            _dropHateList.clear();
            _target = null;
            _targetItemList.Clear();
            _targetItem = null;
        }

        // マスターの設定
        public virtual L1Character Master
        {
            set
            {
                _master = value;
            }
            get
            {
                return _master;
            }
        }


        // ＡＩトリガ
        public virtual void onNpcAI()
        {
        }

        // アイテム精製
        public virtual void refineItem()
        {

            int[] materials = null;
            int[] counts = null;
            int[] createitem = null;
            int[] createcount = null;

            if (_npcTemplate.get_npcId() == 45032)
            { // ブロッブ
              // オリハルコンソードの刀身
                if ((Exp != 0) && !_inventory.checkItem(20))
                {
                    materials = new int[] { 40508, 40521, 40045 };
                    counts = new int[] { 150, 3, 3 };
                    createitem = new int[] { 20 };
                    createcount = new int[] { 1 };
                    if (_inventory.checkItem(materials, counts))
                    {
                        for (int i = 0; i < materials.Length; i++)
                        {
                            _inventory.consumeItem(materials[i], counts[i]);
                        }
                        for (int j = 0; j < createitem.Length; j++)
                        {
                            _inventory.storeItem(createitem[j], createcount[j]);
                        }
                    }
                }
                // ロングソードの刀身
                if ((Exp != 0) && !_inventory.checkItem(19))
                {
                    materials = new int[] { 40494, 40521 };
                    counts = new int[] { 150, 3 };
                    createitem = new int[] { 19 };
                    createcount = new int[] { 1 };
                    if (_inventory.checkItem(materials, counts))
                    {
                        for (int i = 0; i < materials.Length; i++)
                        {
                            _inventory.consumeItem(materials[i], counts[i]);
                        }
                        for (int j = 0; j < createitem.Length; j++)
                        {
                            _inventory.storeItem(createitem[j], createcount[j]);
                        }
                    }
                }
                // ショートソードの刀身
                if ((Exp != 0) && !_inventory.checkItem(3))
                {
                    materials = new int[] { 40494, 40521 };
                    counts = new int[] { 50, 1 };
                    createitem = new int[] { 3 };
                    createcount = new int[] { 1 };
                    if (_inventory.checkItem(materials, counts))
                    {
                        for (int i = 0; i < materials.Length; i++)
                        {
                            _inventory.consumeItem(materials[i], counts[i]);
                        }
                        for (int j = 0; j < createitem.Length; j++)
                        {
                            _inventory.storeItem(createitem[j], createcount[j]);
                        }
                    }
                }
                // オリハルコンホーン
                if ((Exp != 0) && !_inventory.checkItem(100))
                {
                    materials = new int[] { 88, 40508, 40045 };
                    counts = new int[] { 4, 80, 3 };
                    createitem = new int[] { 100 };
                    createcount = new int[] { 1 };
                    if (_inventory.checkItem(materials, counts))
                    {
                        for (int i = 0; i < materials.Length; i++)
                        {
                            _inventory.consumeItem(materials[i], counts[i]);
                        }
                        for (int j = 0; j < createitem.Length; j++)
                        {
                            _inventory.storeItem(createitem[j], createcount[j]);
                        }
                    }
                }
                // ミスリルホーン
                if ((Exp != 0) && !_inventory.checkItem(89))
                {
                    materials = new int[] { 88, 40494 };
                    counts = new int[] { 2, 80 };
                    createitem = new int[] { 89 };
                    createcount = new int[] { 1 };
                    if (_inventory.checkItem(materials, counts))
                    {
                        for (int i = 0; i < materials.Length; i++)
                        {
                            _inventory.consumeItem(materials[i], counts[i]);
                        }
                        for (int j = 0; j < createitem.Length; j++)
                        {
                            L1ItemInstance item = _inventory.storeItem(createitem[j], createcount[j]);
                            if (NpcTemplate.get_digestitem() > 0)
                            {
                                DigestItem = item;
                            }
                        }
                    }
                }
            }
            else if (_npcTemplate.get_npcId() == 81069)
            { // ドッペルゲンガー（クエスト）
              // ドッペルゲンガーの体液
                if ((Exp != 0) && !_inventory.checkItem(40542))
                {
                    materials = new int[] { 40032 };
                    counts = new int[] { 1 };
                    createitem = new int[] { 40542 };
                    createcount = new int[] { 1 };
                    if (_inventory.checkItem(materials, counts))
                    {
                        for (int i = 0; i < materials.Length; i++)
                        {
                            _inventory.consumeItem(materials[i], counts[i]);
                        }
                        for (int j = 0; j < createitem.Length; j++)
                        {
                            _inventory.storeItem(createitem[j], createcount[j]);
                        }
                    }
                }
            }
            else if ((_npcTemplate.get_npcId() == 45166) || (_npcTemplate.get_npcId() == 45167))
            {
                // パンプキンの種
                if ((Exp != 0) && !_inventory.checkItem(40726))
                {
                    materials = new int[] { 40725 };
                    counts = new int[] { 1 };
                    createitem = new int[] { 40726 };
                    createcount = new int[] { 1 };
                    if (_inventory.checkItem(materials, counts))
                    {
                        for (int i = 0; i < materials.Length; i++)
                        {
                            _inventory.consumeItem(materials[i], counts[i]);
                        }
                        for (int j = 0; j < createitem.Length; j++)
                        {
                            _inventory.storeItem(createitem[j], createcount[j]);
                        }
                    }
                }
            }
        }

        private bool _aiRunning = false; // ＡＩが実行中か

        // ※ＡＩをスタートさせる時にすでに実行されてないか確認する時に使用
        private bool _actived = false; // ＮＰＣがアクティブか

        // ※この値がfalseで_targetがいる場合、アクティブになって初行動とみなしヘイストポーション等を使わせる判定で使用
        private bool _firstAttack = false; // ファーストアッタクされたか

        private int _sleep_time; // ＡＩを停止する時間(ms) ※行動を起こした場合に所要する時間をセット

        protected internal L1HateList _hateList = new L1HateList();

        protected internal L1HateList _dropHateList = new L1HateList();

        // ※攻撃するターゲットの判定とＰＴ時のドロップ判定で使用
        protected internal IList<L1ItemInstance> _targetItemList = Lists.newList<L1ItemInstance>(); // ダーゲットアイテム一覧

        protected internal L1Character _target = null; // 現在のターゲット

        protected internal L1ItemInstance _targetItem = null; // 現在のターゲットアイテム

        protected internal L1Character _master = null; // 主人orグループリーダー

        private bool _deathProcessing = false; // 死亡処理中か

        // EXP、Drop分配中はターゲットリスト、ヘイトリストをクリアしない

        private int _paralysisTime = 0; // Paralysis RestTime

        public virtual int ParalysisTime
        {
            set
            {
                _paralysisTime = value;
            }
            get
            {
                return _paralysisTime;
            }
        }

        public virtual L1HateList HateList
        {
            get
            {
                return _hateList;
            }
        }


        // HP自然回復
        public void startHpRegeneration()
        {
            int hprInterval = NpcTemplate.get_hprinterval();
            int hpr = NpcTemplate.get_hpr();
            if (!_hprRunning && (hprInterval > 0) && (hpr > 0))
            {
                _hprTimer = new HprTimer(this, hpr);
                RunnableExecuter.Instance.scheduleAtFixedRate(_hprTimer, hprInterval, hprInterval);
                _hprRunning = true;
            }
        }

        public void stopHpRegeneration()
        {
            if (_hprRunning)
            {
                _hprTimer.cancel();
                _hprRunning = false;
            }
        }

        // MP自然回復
        public void startMpRegeneration()
        {
            int mprInterval = NpcTemplate.get_mprinterval();
            int mpr = NpcTemplate.get_mpr();
            if (!_mprRunning && (mprInterval > 0) && (mpr > 0))
            {
                _mprTimer = new MprTimer(this, mpr);
                RunnableExecuter.Instance.scheduleAtFixedRate(_mprTimer, mprInterval, mprInterval);
                _mprRunning = true;
            }
        }

        public void stopMpRegeneration()
        {
            if (_mprRunning)
            {
                _mprTimer.cancel();
                _mprRunning = false;
            }
        }

        // ■■■■■■■■■■■■ タイマー関連 ■■■■■■■■■■

        // ＨＰ自然回復
        private bool _hprRunning = false;

        private HprTimer _hprTimer;

        internal class HprTimer : TimerTask
        {
            private readonly L1NpcInstance outerInstance;

            public override void run()
            {
                try
                {
                    if ((!outerInstance._destroyed && !outerInstance.Dead) && ((outerInstance.CurrentHp > 0) && (outerInstance.CurrentHp < outerInstance.MaxHp)))
                    {
                        outerInstance.CurrentHp = outerInstance.CurrentHp + _point;
                    }
                    else
                    {
                        cancel();
                        outerInstance._hprRunning = false;
                    }
                }
                catch (Exception e)
                {
                    _log.log(Enum.Level.Server, e.Message, e);
                }
            }

            public HprTimer(L1NpcInstance outerInstance, int point)
            {
                this.outerInstance = outerInstance;
                if (point < 1)
                {
                    point = 1;
                }
                _point = point;
            }

            internal readonly int _point;
        }

        // ＭＰ自然回復
        private bool _mprRunning = false;

        private MprTimer _mprTimer;

        internal class MprTimer : TimerTask
        {
            private readonly L1NpcInstance outerInstance;

            public override void run()
            {
                try
                {
                    if ((!outerInstance._destroyed && !outerInstance.Dead) &&
                        ((outerInstance.CurrentHp > 0) && (outerInstance.CurrentMp < outerInstance.MaxMp)))
                    {
                        outerInstance.CurrentMp = outerInstance.CurrentMp + _point;
                    }
                    else
                    {
                        cancel();
                        outerInstance._mprRunning = false;
                    }
                }
                catch (Exception e)
                {
                    _log.log(Enum.Level.Server, e.Message, e);
                }
            }

            public MprTimer(L1NpcInstance outerInstance, int point)
            {
                this.outerInstance = outerInstance;
                if (point < 1)
                {
                    point = 1;
                }
                _point = point;
            }

            internal readonly int _point;
        }

        // アイテム消化
        private IDictionary<int, int> _digestItems;

        public bool _digestItemRunning = false;

        internal class DigestItemTimer : IRunnable
        {
            private readonly L1NpcInstance outerInstance;

            public DigestItemTimer(L1NpcInstance outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public void run()
            {
                outerInstance._digestItemRunning = true;
                while (!outerInstance._destroyed && (outerInstance._digestItems.Count > 0))
                {
                    try
                    {
                        Thread.Sleep(1000);
                    }
                    catch (Exception)
                    {
                        break;
                    }

                    int[] keys = outerInstance._digestItems.Keys.ToArray();
                    foreach (int key in keys)
                    {
                        int digestCounter = outerInstance._digestItems[key];
                        digestCounter -= 1;
                        if (digestCounter <= 0)
                        {
                            outerInstance._digestItems.Remove(key);
                            L1ItemInstance digestItem = outerInstance.Inventory.getItem(key);
                            if (digestItem != null)
                            {
                                outerInstance.Inventory.removeItem(digestItem, digestItem.Count);
                            }
                        }
                        else
                        {
                            outerInstance._digestItems[key] = digestCounter;
                        }
                    }
                }
                outerInstance._digestItemRunning = false;
            }
        }

        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        public L1NpcInstance(L1Npc template)
        {
            Status = 0;
            MoveSpeed = 0;
            Dead = false;
            setreSpawn(false);

            if (template != null)
            {
                setting_template(template);
            }
        }

        // 指定のテンプレートで各種値を初期化
        public virtual void setting_template(L1Npc template)
        {
            _npcTemplate = template;
            int randomlevel = 0;
            double rate = 0;
            double diff = 0;
            Name = template.get_name();
            NameId = template.get_nameid();
            if (template.get_randomlevel() == 0)
            { // ランダムLv指定なし
                Level = template.get_level();
            }
            else
            { // ランダムLv指定あり（最小値:get_level(),最大値:get_randomlevel()）
                randomlevel = RandomHelper.Next(template.get_randomlevel() - template.get_level() + 1);
                diff = template.get_randomlevel() - template.get_level();
                rate = randomlevel / diff;
                randomlevel += template.get_level();
                Level = randomlevel;
            }
            if (template.get_randomhp() == 0)
            {
                MaxHp = template.get_hp();
                CurrentHpDirect = template.get_hp();
            }
            else
            {
                double randomhp = rate * (template.get_randomhp() - template.get_hp());
                int hp = (int)(template.get_hp() + randomhp);
                MaxHp = hp;
                CurrentHpDirect = hp;
            }
            if (template.get_randommp() == 0)
            {
                MaxMp = template.get_mp();
                CurrentMpDirect = template.get_mp();
            }
            else
            {
                double randommp = rate * (template.get_randommp() - template.get_mp());
                int mp = (int)(template.get_mp() + randommp);
                MaxMp = mp;
                CurrentMpDirect = mp;
            }
            if (template.get_randomac() == 0)
            {
                Ac = template.get_ac();
            }
            else
            {
                double randomac = rate * (template.get_randomac() - template.get_ac());
                int ac = (int)(template.get_ac() + randomac);
                Ac = ac;
            }
            if (template.get_randomlevel() == 0)
            {
                Str = template.get_str();
                Con = template.get_con();
                Dex = template.get_dex();
                Int = template.get_int();
                Wis = template.get_wis();
                Mr = template.get_mr();
            }
            else
            {
                Str = (sbyte)Math.Min(template.get_str() + diff, 127);
                Con = (sbyte)Math.Min(template.get_con() + diff, 127);
                Dex = (sbyte)Math.Min(template.get_dex() + diff, 127);
                Int = (sbyte)Math.Min(template.get_int() + diff, 127);
                Wis = (sbyte)Math.Min(template.get_wis() + diff, 127);
                Mr = (sbyte)Math.Min(template.get_mr() + diff, 127);

                addHitup((int)diff * 2);
                addDmgup((int)diff * 2);
            }
            Agro = template.is_agro();
            Agrocoi = template.is_agrocoi();
            Agrososc = template.is_agrososc();
            TempCharGfx = template.get_gfxid();
            GfxId = template.get_gfxid();
            Status = L1NpcDefaultAction.Instance.getStatus(TempCharGfx);
            PolyAtkRanged = template.get_ranged();
            PolyArrowGfx = template.BowActId;

            // 移動
            if (template.get_passispeed() != 0)
            {
                Passispeed = SprTable.Instance.getSprSpeed(TempCharGfx, Status);
            }
            else
            {
                Passispeed = 0;
            }
            // 攻擊
            if (template.get_atkspeed() != 0)
            {
                int actid = (Status + 1);
                if (L1NpcDefaultAction.Instance.getDefaultAttack(TempCharGfx) != actid)
                {
                    actid = L1NpcDefaultAction.Instance.getDefaultAttack(TempCharGfx);
                }
                Atkspeed = SprTable.Instance.getSprSpeed(TempCharGfx, actid);
            }
            else
            {
                Atkspeed = 0;
            }

            if (template.get_randomexp() == 0)
            {
                Exp = template.get_exp();
            }
            else
            {
                int level = Level;
                int exp = level * level;
                exp += 1;
                Exp = exp;
            }
            if (template.get_randomlawful() == 0)
            {
                Lawful = template.get_lawful();
                TempLawful = template.get_lawful();
            }
            else
            {
                double randomlawful = rate * (template.get_randomlawful() - template.get_lawful());
                int lawful = (int)(template.get_lawful() + randomlawful);
                Lawful = lawful;
                TempLawful = lawful;
            }
            PickupItem = template.is_picupitem();
            if (template.is_bravespeed())
            {
                BraveSpeed = 1;
            }
            else
            {
                BraveSpeed = 0;
            }
            if (template.get_digestitem() > 0)
            {
                _digestItems = Maps.newMap<int, int>();
            }
            Karma = template.Karma;
            LightSize = template.LightSize;

            mobSkill = new L1MobSkillUse(this);
        }

        // 延遲時間
        public virtual void npcSleepTime(int i, int type)
        {
            SleepTime = calcSleepTime(SprTable.Instance.getSprSpeed(TempCharGfx, i), type);
        }

        private int _passispeed;

        public virtual int Passispeed
        {
            get
            {
                return _passispeed;
            }
            set
            {
                _passispeed = value;
            }
        }


        private int _atkspeed;

        public virtual int Atkspeed
        {
            get
            {
                return _atkspeed;
            }
            set
            {
                _atkspeed = value;
            }
        }


        private bool _pickupItem;

        public virtual bool PickupItem
        {
            get
            {
                return _pickupItem;
            }
            set
            {
                _pickupItem = value;
            }
        }


        public override L1Inventory Inventory
        {
            get
            {
                return _inventory;
            }
            set
            {
                _inventory = value;
            }
        }


        public virtual L1Npc NpcTemplate
        {
            get
            {
                return _npcTemplate;
            }
        }

        public virtual int NpcId
        {
            get
            {
                return _npcTemplate.get_npcId();
            }
        }

        public virtual int Petcost
        {
            set
            {
                _petcost = value;
            }
            get
            {
                return _petcost;
            }
        }


        public virtual L1Spawn Spawn
        {
            set
            {
                _spawn = value;
            }
            get
            {
                return _spawn;
            }
        }


        public virtual int SpawnNumber
        {
            set
            {
                _spawnNumber = value;
            }
            get
            {
                return _spawnNumber;
            }
        }


        // オブジェクトIDをSpawnTaskに渡し再利用する
        // グループモンスターは複雑になるので再利用しない
        public virtual void onDecay(bool isReuseId)
        {
            int id = 0;
            if (isReuseId)
            {
                id = Id;
            }
            else
            {
                id = 0;
            }
            _spawn.executeSpawnTask(_spawnNumber, id);
        }

        public override void onPerceive(L1PcInstance perceivedFrom)
        {
            perceivedFrom.addKnownObject(this);
            perceivedFrom.sendPackets(new S_NPCPack(this));
            onNpcAI();
        }

        public virtual void deleteMe()
        {
            _destroyed = true;
            if (Inventory != null)
            {
                Inventory.clearItems();
            }
            allTargetClear();
            _master = null;
            L1World.Instance.removeVisibleObject(this);
            L1World.Instance.removeObject(this);
            IList<L1PcInstance> players = L1World.Instance.getRecognizePlayer(this);
            if (players.Count > 0)
            {
                S_RemoveObject s_deleteNewObject = new S_RemoveObject(this);
                foreach (L1PcInstance pc in players)
                {
                    if (pc != null)
                    {
                        pc.removeKnownObject(this);
                        // if(!L1Character.distancepc(user, this))
                        pc.sendPackets(s_deleteNewObject);
                    }
                }
            }
            removeAllKnownObjects();

            // リスパウン設定
            L1MobGroupInfo mobGroupInfo = MobGroupInfo;
            if (mobGroupInfo == null)
            {
                if (ReSpawn)
                {
                    onDecay(true);
                }
            }
            else
            {
                if (mobGroupInfo.removeMember(this) == 0)
                { // グループメンバー全滅
                    MobGroupInfo = null;
                    if (ReSpawn)
                    {
                        onDecay(false);
                    }
                }
            }
        }

        public virtual void ReceiveManaDamage(L1Character attacker, int damageMp)
        {
        }

        public virtual void receiveDamage(L1Character attacker, int damage)
        {
        }

        public virtual L1ItemInstance DigestItem
        {
            set
            {
                _digestItems[value.Id] = NpcTemplate.get_digestitem();
                if (!_digestItemRunning)
                {
                    DigestItemTimer digestItemTimer = new DigestItemTimer(this);
                    RunnableExecuter.Instance.execute(digestItemTimer);
                }
            }
        }

        public virtual void onGetItem(L1ItemInstance item)
        {
            refineItem();
            Inventory.shuffle();
            if (NpcTemplate.get_digestitem() > 0)
            {
                DigestItem = item;
            }
        }

        public virtual void approachPlayer(L1PcInstance pc)
        {
            if (pc.hasSkillEffect(L1SkillId.L1SkillId.60) || pc.hasSkillEffect(L1SkillId.L1SkillId.97))
            { // インビジビリティ、ブラインドハイディング中
                return;
            }
            if (HiddenStatus == HIDDEN_STATUS_SINK)
            {
                if (CurrentHp == MaxHp)
                {
                    if (pc.Location.getTileLineDistance(Location) <= 2)
                    {
                        appearOnGround(pc);
                    }
                }
            }
            else if (HiddenStatus == HIDDEN_STATUS_FLY)
            {
                if (CurrentHp == MaxHp)
                {
                    if (pc.Location.getTileLineDistance(Location) <= 1)
                    {
                        appearOnGround(pc);
                    }
                }
                else
                {
                    // if (getNpcTemplate().get_npcId() != 45681) { // リンドビオル以外
                    searchItemFromAir();
                    // }
                }
            }
            else if (HiddenStatus == HIDDEN_STATUS_ICE)
            {
                if (CurrentHp < MaxHp)
                {
                    appearOnGround(pc);
                }
            }
        }

        public virtual void appearOnGround(L1PcInstance pc)
        { // 怪物解除遁地、飛天、冰凍
            if (HiddenStatus == HIDDEN_STATUS_SINK)
            {
                HiddenStatus = HIDDEN_STATUS_NONE;
                Status = L1NpcDefaultAction.Instance.getStatus(TempCharGfx);
                broadcastPacket(new S_DoActionGFX(Id, ActionCodes.ACTION_Appear));
                broadcastPacket(new S_CharVisualUpdate(this, Status));
                if (!pc.hasSkillEffect(L1SkillId.L1SkillId.60) && !pc.hasSkillEffect(L1SkillId.L1SkillId.97) && !pc.Gm)
                {
                    _hateList.add(pc, 0);
                    _target = pc;
                }
                onNpcAI(); // モンスターのＡＩを開始
                startChat(CHAT_TIMING_HIDE);
            }
            else if (HiddenStatus == HIDDEN_STATUS_FLY)
            {
                HiddenStatus = HIDDEN_STATUS_NONE;
                Status = L1NpcDefaultAction.Instance.getStatus(TempCharGfx);
                broadcastPacket(new S_DoActionGFX(Id, ActionCodes.ACTION_Movedown));
                if (!pc.hasSkillEffect(L1SkillId.L1SkillId.60) && !pc.hasSkillEffect(L1SkillId.L1SkillId.97) && !pc.Gm)
                {
                    _hateList.add(pc, 0);
                    _target = pc;
                }
                onNpcAI(); // モンスターのＡＩを開始
                startChat(CHAT_TIMING_HIDE);
            }
            else if (HiddenStatus == HIDDEN_STATUS_ICE)
            {
                HiddenStatus = HIDDEN_STATUS_NONE;
                Status = L1NpcDefaultAction.Instance.getStatus(TempCharGfx);
                broadcastPacket(new S_DoActionGFX(Id, ActionCodes.ACTION_AxeWalk));
                broadcastPacket(new S_CharVisualUpdate(this, Status));
                if (!pc.hasSkillEffect(L1SkillId.L1SkillId.60) && !pc.hasSkillEffect(L1SkillId.L1SkillId.97) && !pc.Gm)
                {
                    _hateList.add(pc, 0);
                    _target = pc;
                }
                onNpcAI(); // モンスターのＡＩを開始
                startChat(CHAT_TIMING_HIDE);
            }
        }

        // ■■■■■■■■■■■■■ 移動関連 ■■■■■■■■■■■

        // 指定された方向に移動させる
        public virtual int DirectionMove
        {
            set
            {
                if (value >= 0)
                {
                    int nx = 0;
                    int ny = 0;

                    switch (value)
                    {
                        case 1:
                            nx = 1;
                            ny = -1;
                            Heading = 1;
                            break;

                        case 2:
                            nx = 1;
                            ny = 0;
                            Heading = 2;
                            break;

                        case 3:
                            nx = 1;
                            ny = 1;
                            Heading = 3;
                            break;

                        case 4:
                            nx = 0;
                            ny = 1;
                            Heading = 4;
                            break;

                        case 5:
                            nx = -1;
                            ny = 1;
                            Heading = 5;
                            break;

                        case 6:
                            nx = -1;
                            ny = 0;
                            Heading = 6;
                            break;

                        case 7:
                            nx = -1;
                            ny = -1;
                            Heading = 7;
                            break;

                        case 0:
                            nx = 0;
                            ny = -1;
                            Heading = 0;
                            break;

                        default:
                            break;

                    }

                    System.Collections.IDictionary.setPassable(Location, true);

                    int nnx = X + nx;
                    int nny = Y + ny;
                    X = nnx;
                    Y = nny;

                    System.Collections.IDictionary.setPassable(Location, false);

                    broadcastPacket(new S_MoveCharPacket(this));

                    // movement_distanceマス以上離れたらホームポイントへテレポート
                    if (MovementDistance > 0)
                    {
                        if ((this is L1GuardInstance) || (this is L1MerchantInstance) || (this is L1MonsterInstance))
                        {
                            if (Location.getLineDistance(new Point(HomeX, HomeY)) > MovementDistance)
                            {
                                teleport(HomeX, HomeY, Heading);
                            }
                        }
                    }
                    // 判斷士兵的怨靈、怨靈、哈蒙將軍的怨靈離開墓園範圍時傳送回墓園！
                    if ((NpcTemplate.get_npcId() >= 45912) && (NpcTemplate.get_npcId() <= 45916))
                    {
                        if (!((X >= 32591) && (X <= 32644) && (Y >= 32643) && (Y <= 32688) && (MapId == 4)))
                        {
                            teleport(HomeX, HomeY, Heading);
                        }
                    }
                }
            }
        }

        public virtual int moveDirection(int x, int y)
        { // 目標点Ｘ 目標点Ｙ
            return moveDirection(x, y, Location.getLineDistance(new Point(x, y)));
        }

        // 目標までの距離に応じて最適と思われるルーチンで進む方向を返す
        public virtual int moveDirection(int x, int y, double d)
        { // 目標点Ｘ 目標点Ｙ 目標までの距離
            int dir = 0;
            if ((hasSkillEffect(L1SkillId.40) == true) && (d >= 2D))
            { // ダークネスが掛かっていて、距離が2以上の場合追跡終了
                return -1;
            }
            else if (d > 30D)
            { // 距離が激しく遠い場合は追跡終了
                return -1;
            }
            else if (d > courceRange)
            { // 距離が遠い場合は単純計算
                dir = targetDirection(x, y);
                dir = checkObject(X, Y, MapId, dir);
            }
            else
            { // 目標までの最短経路を探索
                dir = _serchCource(x, y);
                if (dir == -1)
                { // 目標までの経路がなっかた場合はとりあえず近づいておく
                    dir = targetDirection(x, y);
                    if (!isExsistCharacterBetweenTarget(dir))
                    {
                        dir = checkObject(X, Y, MapId, dir);
                    }
                }
            }
            return dir;
        }

        private bool isExsistCharacterBetweenTarget(int dir)
        {
            if (!(this is L1MonsterInstance))
            { // モンスター以外は対象外
                return false;
            }
            if (_target == null)
            { // ターゲットがいない場合
                return false;
            }

            int locX = X;
            int locY = Y;
            int targetX = locX;
            int targetY = locY;

            if (dir == 1)
            {
                targetX = locX + 1;
                targetY = locY - 1;
            }
            else if (dir == 2)
            {
                targetX = locX + 1;
            }
            else if (dir == 3)
            {
                targetX = locX + 1;
                targetY = locY + 1;
            }
            else if (dir == 4)
            {
                targetY = locY + 1;
            }
            else if (dir == 5)
            {
                targetX = locX - 1;
                targetY = locY + 1;
            }
            else if (dir == 6)
            {
                targetX = locX - 1;
            }
            else if (dir == 7)
            {
                targetX = locX - 1;
                targetY = locY - 1;
            }
            else if (dir == 0)
            {
                targetY = locY - 1;
            }

            foreach (L1Object @object in L1World.Instance.getVisibleObjects(this, 1))
            {
                // PC, Summon, Petがいる場合
                if ((@object is L1PcInstance) || (@object is L1SummonInstance) || (@object is L1PetInstance))
                {
                    L1Character cha = (L1Character)@object;
                    // 進行方向に立ちふさがっている場合、ターゲットリストに加える
                    if ((cha.X == targetX) && (cha.Y == targetY) && (cha.MapId == MapId))
                    {
                        if (@object is L1PcInstance)
                        {
                            L1PcInstance pc = (L1PcInstance)@object;
                            if (pc.Ghost)
                            { // UB観戦中のPCは除く
                                continue;
                            }
                        }
                        _hateList.add(cha, 0);
                        _target = cha;
                        return true;
                    }
                }
            }
            return false;
        }

        // 目標の逆方向を返す
        public virtual int targetReverseDirection(int tx, int ty)
        { // 目標点Ｘ 目標点Ｙ
            int dir = targetDirection(tx, ty);
            dir += 4;
            if (dir > 7)
            {
                dir -= 8;
            }
            return dir;
        }

        // 進みたい方向に障害物がないか確認、ある場合は前方斜め左右も確認後進める方向を返す
        // ※従来あった処理に、バックできない仕様を省いて、目標の反対（左右含む）には進まないようにしたもの
        public static int checkObject(int x, int y, short m, int d)
        { // 起点Ｘ 起点Ｙ
          // マップＩＤ
          // 進行方向
            L1Map map = L1WorldMap.Instance.getMap(m);
            if (d == 1)
            {
                if (map.isPassable(x, y, 1))
                {
                    return 1;
                }
                else if (map.isPassable(x, y, 0))
                {
                    return 0;
                }
                else if (map.isPassable(x, y, 2))
                {
                    return 2;
                }
            }
            else if (d == 2)
            {
                if (map.isPassable(x, y, 2))
                {
                    return 2;
                }
                else if (map.isPassable(x, y, 1))
                {
                    return 1;
                }
                else if (map.isPassable(x, y, 3))
                {
                    return 3;
                }
            }
            else if (d == 3)
            {
                if (map.isPassable(x, y, 3))
                {
                    return 3;
                }
                else if (map.isPassable(x, y, 2))
                {
                    return 2;
                }
                else if (map.isPassable(x, y, 4))
                {
                    return 4;
                }
            }
            else if (d == 4)
            {
                if (map.isPassable(x, y, 4))
                {
                    return 4;
                }
                else if (map.isPassable(x, y, 3))
                {
                    return 3;
                }
                else if (map.isPassable(x, y, 5))
                {
                    return 5;
                }
            }
            else if (d == 5)
            {
                if (map.isPassable(x, y, 5))
                {
                    return 5;
                }
                else if (map.isPassable(x, y, 4))
                {
                    return 4;
                }
                else if (map.isPassable(x, y, 6))
                {
                    return 6;
                }
            }
            else if (d == 6)
            {
                if (map.isPassable(x, y, 6))
                {
                    return 6;
                }
                else if (map.isPassable(x, y, 5))
                {
                    return 5;
                }
                else if (map.isPassable(x, y, 7))
                {
                    return 7;
                }
            }
            else if (d == 7)
            {
                if (map.isPassable(x, y, 7))
                {
                    return 7;
                }
                else if (map.isPassable(x, y, 6))
                {
                    return 6;
                }
                else if (map.isPassable(x, y, 0))
                {
                    return 0;
                }
            }
            else if (d == 0)
            {
                if (map.isPassable(x, y, 0))
                {
                    return 0;
                }
                else if (map.isPassable(x, y, 7))
                {
                    return 7;
                }
                else if (map.isPassable(x, y, 1))
                {
                    return 1;
                }
            }
            return -1;
        }

        // 目標までの最短経路の方向を返す
        // ※目標を中心とした探索範囲のマップで探索
        private int _serchCource(int x, int y) // 目標点Ｘ 目標点Ｙ
        {
            int i;
            int locCenter = courceRange + 1;
            int diff_x = x - locCenter; // Ｘの実際のロケーションとの差
            int diff_y = y - locCenter; // Ｙの実際のロケーションとの差
            int[] locBace = new int[] { X - diff_x, Y - diff_y, 0, 0 }; // Ｘ Ｙ
                                                                        // 方向
                                                                        // 初期方向
            int[] locNext = new int[4];
            int[] locCopy;
            int[] dirFront = new int[5];
            //JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
            //ORIGINAL LINE: bool[][] serchMap = new bool[locCenter * 2 + 1][locCenter * 2 + 1];
            bool[][] serchMap = RectangularArrays.RectangularBoolArray(locCenter * 2 + 1, locCenter * 2 + 1);
            LinkedList<int[]> queueSerch = new LinkedList<int[]>();

            // 探索用マップの設定
            for (int j = courceRange * 2 + 1; j > 0; j--)
            {
                for (i = courceRange - Math.Abs(locCenter - j); i >= 0; i--)
                {
                    serchMap[j][locCenter + i] = true;
                    serchMap[j][locCenter - i] = true;
                }
            }

            // 初期方向の設置
            int[] firstCource = new int[] { 2, 4, 6, 0, 1, 3, 5, 7 };
            for (i = 0; i < 8; i++)
            {
                Array.Copy(locBace, 0, locNext, 0, 4);
                _moveLocation(locNext, firstCource[i]);
                if ((locNext[0] - locCenter == 0) && (locNext[1] - locCenter == 0))
                {
                    // 最短経路が見つかった場合:隣
                    return firstCource[i];
                }
                if (serchMap[locNext[0]][locNext[1]])
                {
                    int tmpX = locNext[0] + diff_x;
                    int tmpY = locNext[1] + diff_y;
                    bool found = false;
                    if (i == 0)
                    {
                        found = System.Collections.IDictionary.isPassable(tmpX, tmpY + 1, i);
                    }
                    else if (i == 1)
                    {
                        found = System.Collections.IDictionary.isPassable(tmpX - 1, tmpY + 1, i);
                    }
                    else if (i == 2)
                    {
                        found = System.Collections.IDictionary.isPassable(tmpX - 1, tmpY, i);
                    }
                    else if (i == 3)
                    {
                        found = System.Collections.IDictionary.isPassable(tmpX - 1, tmpY - 1, i);
                    }
                    else if (i == 4)
                    {
                        found = System.Collections.IDictionary.isPassable(tmpX, tmpY - 1, i);
                    }
                    else if (i == 5)
                    {
                        found = System.Collections.IDictionary.isPassable(tmpX + 1, tmpY - 1, i);
                    }
                    else if (i == 6)
                    {
                        found = System.Collections.IDictionary.isPassable(tmpX + 1, tmpY, i);
                    }
                    else if (i == 7)
                    {
                        found = System.Collections.IDictionary.isPassable(tmpX + 1, tmpY + 1, i);
                    }
                    if (found) // 移動経路があった場合
                    {
                        locCopy = new int[4];
                        Array.Copy(locNext, 0, locCopy, 0, 4);
                        locCopy[2] = firstCource[i];
                        locCopy[3] = firstCource[i];
                        queueSerch.AddLast(locCopy);
                    }
                    serchMap[locNext[0]][locNext[1]] = false;
                }
            }
            locBace = null;

            // 最短経路を探索
            while (queueSerch.Count > 0)
            {
                locBace = queueSerch.RemoveFirst();
                _getFront(dirFront, locBace[2]);
                for (i = 4; i >= 0; i--)
                {
                    Array.Copy(locBace, 0, locNext, 0, 4);
                    _moveLocation(locNext, dirFront[i]);
                    if ((locNext[0] - locCenter == 0) && (locNext[1] - locCenter == 0))
                    {
                        return locNext[3];
                    }
                    if (serchMap[locNext[0]][locNext[1]])
                    {
                        int tmpX = locNext[0] + diff_x;
                        int tmpY = locNext[1] + diff_y;
                        bool found = false;
                        if (i == 0)
                        {
                            found = System.Collections.IDictionary.isPassable(tmpX, tmpY + 1, i);
                        }
                        else if (i == 1)
                        {
                            found = System.Collections.IDictionary.isPassable(tmpX - 1, tmpY + 1, i);
                        }
                        else if (i == 2)
                        {
                            found = System.Collections.IDictionary.isPassable(tmpX - 1, tmpY, i);
                        }
                        else if (i == 3)
                        {
                            found = System.Collections.IDictionary.isPassable(tmpX - 1, tmpY - 1, i);
                        }
                        else if (i == 4)
                        {
                            found = System.Collections.IDictionary.isPassable(tmpX, tmpY - 1, i);
                        }
                        if (found) // 移動経路があった場合
                        {
                            locCopy = new int[4];
                            Array.Copy(locNext, 0, locCopy, 0, 4);
                            locCopy[2] = dirFront[i];
                            queueSerch.AddLast(locCopy);
                        }
                        serchMap[locNext[0]][locNext[1]] = false;
                    }
                }
                locBace = null;
            }
            return -1; // 目標までの経路がない場合
        }

        private void _moveLocation(int[] ary, int d)
        {
            if (d == 1)
            {
                ary[0] = ary[0] + 1;
                ary[1] = ary[1] - 1;
            }
            else if (d == 2)
            {
                ary[0] = ary[0] + 1;
            }
            else if (d == 3)
            {
                ary[0] = ary[0] + 1;
                ary[1] = ary[1] + 1;
            }
            else if (d == 4)
            {
                ary[1] = ary[1] + 1;
            }
            else if (d == 5)
            {
                ary[0] = ary[0] - 1;
                ary[1] = ary[1] + 1;
            }
            else if (d == 6)
            {
                ary[0] = ary[0] - 1;
            }
            else if (d == 7)
            {
                ary[0] = ary[0] - 1;
                ary[1] = ary[1] - 1;
            }
            else if (d == 0)
            {
                ary[1] = ary[1] - 1;
            }
            ary[2] = d;
        }

        private void _getFront(int[] ary, int d)
        {
            if (d == 1)
            {
                ary[4] = 2;
                ary[3] = 0;
                ary[2] = 1;
                ary[1] = 3;
                ary[0] = 7;
            }
            else if (d == 2)
            {
                ary[4] = 2;
                ary[3] = 4;
                ary[2] = 0;
                ary[1] = 1;
                ary[0] = 3;
            }
            else if (d == 3)
            {
                ary[4] = 2;
                ary[3] = 4;
                ary[2] = 1;
                ary[1] = 3;
                ary[0] = 5;
            }
            else if (d == 4)
            {
                ary[4] = 2;
                ary[3] = 4;
                ary[2] = 6;
                ary[1] = 3;
                ary[0] = 5;
            }
            else if (d == 5)
            {
                ary[4] = 4;
                ary[3] = 6;
                ary[2] = 3;
                ary[1] = 5;
                ary[0] = 7;
            }
            else if (d == 6)
            {
                ary[4] = 4;
                ary[3] = 6;
                ary[2] = 0;
                ary[1] = 5;
                ary[0] = 7;
            }
            else if (d == 7)
            {
                ary[4] = 6;
                ary[3] = 0;
                ary[2] = 1;
                ary[1] = 5;
                ary[0] = 7;
            }
            else if (d == 0)
            {
                ary[4] = 2;
                ary[3] = 6;
                ary[2] = 0;
                ary[1] = 1;
                ary[0] = 7;
            }
        }

        // ■■■■■■■■■■■■ アイテム関連 ■■■■■■■■■■

        private void useHealPotion(int healHp, int effectId)
        {
            broadcastPacket(new S_SkillSound(Id, effectId));
            if (hasSkillEffect(L1SkillId.POLLUTE_WATER))
            { // ポルートウォーター中は回復量1/2倍
                healHp /= 2;
            }
            if (this is L1PetInstance)
            {
                ((L1PetInstance)this).CurrentHp = CurrentHp + healHp;
            }
            else if (this is L1SummonInstance)
            {
                ((L1SummonInstance)this).CurrentHp = CurrentHp + healHp;
            }
            else
            {
                CurrentHpDirect = CurrentHp + healHp;
            }
        }

        private void useHastePotion(int time)
        {
            broadcastPacket(new S_SkillHaste(Id, 1, time));
            broadcastPacket(new S_SkillSound(Id, 191));
            MoveSpeed = 1;
            setSkillEffect(STATUS_HASTE, time * 1000);
        }

        // アイテムの使用判定及び使用
        public const int USEITEM_HEAL = 0;

        public const int USEITEM_HASTE = 1;

        public static int[] healPotions = new int[] { POTION_OF_GREATER_HEALING, POTION_OF_EXTRA_HEALING, POTION_OF_HEALING };

        public static int[] haestPotions = new int[] { B_POTION_OF_GREATER_HASTE_SELF, POTION_OF_GREATER_HASTE_SELF, B_POTION_OF_HASTE_SELF, POTION_OF_HASTE_SELF };

        public virtual void useItem(int type, int chance)
        { // 使用する種類 使用する可能性(％)
            if (hasSkillEffect(L1SkillId.71))
            {
                return; // ディケイ ポーション状態かチェック
            }

            if (RandomHelper.Next(100) > chance)
            {
                return; // 使用する可能性
            }

            if (type == USEITEM_HEAL)
            { // 回復系ポーション
              // 回復量の大きい順
                if (Inventory.consumeItem(POTION_OF_GREATER_HEALING, 1))
                {
                    useHealPotion(75, 197);
                }
                else if (Inventory.consumeItem(POTION_OF_EXTRA_HEALING, 1))
                {
                    useHealPotion(45, 194);
                }
                else if (Inventory.consumeItem(POTION_OF_HEALING, 1))
                {
                    useHealPotion(15, 189);
                }
            }
            else if (type == USEITEM_HASTE)
            { // ヘイスト系ポーション
                if (hasSkillEffect(L1SkillId.1001))
                {
                    return; // ヘイスト状態チェック
                }

                // 効果の長い順
                if (Inventory.consumeItem(B_POTION_OF_GREATER_HASTE_SELF, 1))
                {
                    useHastePotion(2100);
                }
                else if (Inventory.consumeItem(POTION_OF_GREATER_HASTE_SELF, 1))
                {
                    useHastePotion(1800);
                }
                else if (Inventory.consumeItem(B_POTION_OF_HASTE_SELF, 1))
                {
                    useHastePotion(350);
                }
                else if (Inventory.consumeItem(POTION_OF_HASTE_SELF, 1))
                {
                    useHastePotion(300);
                }
            }
        }

        // ■■■■■■■■■■■■■ スキル関連(npcskillsテーブル実装されたら消すかも) ■■■■■■■■■■■

        // 目標の隣へテレポート
        public virtual bool nearTeleport(int nx, int ny)
        {
            int rdir = RandomHelper.Next(8);
            int dir;
            for (int i = 0; i < 8; i++)
            {
                dir = rdir + i;
                if (dir > 7)
                {
                    dir -= 8;
                }
                if (dir == 1)
                {
                    nx++;
                    ny--;
                }
                else if (dir == 2)
                {
                    nx++;
                }
                else if (dir == 3)
                {
                    nx++;
                    ny++;
                }
                else if (dir == 4)
                {
                    ny++;
                }
                else if (dir == 5)
                {
                    nx--;
                    ny++;
                }
                else if (dir == 6)
                {
                    nx--;
                }
                else if (dir == 7)
                {
                    nx--;
                    ny--;
                }
                else if (dir == 0)
                {
                    ny--;
                }
                if (System.Collections.IDictionary.isPassable(nx, ny))
                {
                    dir += 4;
                    if (dir > 7)
                    {
                        dir -= 8;
                    }
                    teleport(nx, ny, dir);
                    CurrentMp = CurrentMp - 10;
                    return true;
                }
            }
            return false;
        }

        // 目標へテレポート
        public virtual void teleport(int nx, int ny, int dir)
        {
            foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(this))
            {
                pc.sendPackets(new S_SkillSound(Id, 169));
                pc.sendPackets(new S_RemoveObject(this));
                pc.removeKnownObject(this);
            }
            X = nx;
            Y = ny;
            Heading = dir;
        }

        // ----------From L1Character-------------
        private string _nameId; // ● ネームＩＤ

        public virtual string NameId
        {
            get
            {
                return _nameId;
            }
            set
            {
                _nameId = value;
            }
        }


        private bool _Agro; // ● アクティブか

        public virtual bool Agro
        {
            get
            {
                return _Agro;
            }
            set
            {
                _Agro = value;
            }
        }


        private bool _Agrocoi; // ● インビジアクティブか

        public virtual bool Agrocoi
        {
            get
            {
                return _Agrocoi;
            }
            set
            {
                _Agrocoi = value;
            }
        }


        private bool _Agrososc; // ● 変身アクティブか

        public virtual bool Agrososc
        {
            get
            {
                return _Agrososc;
            }
            set
            {
                _Agrososc = value;
            }
        }


        private int _homeX; // ● ホームポイントＸ（モンスターの戻る位置とかペットの警戒位置）

        public virtual int HomeX
        {
            get
            {
                return _homeX;
            }
            set
            {
                _homeX = value;
            }
        }


        private int _homeY; // ● ホームポイントＹ（モンスターの戻る位置とかペットの警戒位置）

        public virtual int HomeY
        {
            get
            {
                return _homeY;
            }
            set
            {
                _homeY = value;
            }
        }


        private bool _reSpawn; // ● 再ポップするかどうか

        public virtual bool ReSpawn
        {
            get
            {
                return _reSpawn;
            }
        }

        public virtual void setreSpawn(bool flag)
        {
            _reSpawn = flag;
        }

        private int _lightSize; // ● ライト ０．なし １～１４．大きさ

        public virtual int LightSize
        {
            get
            {
                return _lightSize;
            }
            set
            {
                _lightSize = value;
            }
        }


        private bool _weaponBreaked; // ● ウェポンブレイク中かどうか

        public virtual bool WeaponBreaked
        {
            get
            {
                return _weaponBreaked;
            }
            set
            {
                _weaponBreaked = value;
            }
        }


        private int _hiddenStatus; // ● 地中に潜ったり、空を飛んでいる状態

        public virtual int HiddenStatus
        {
            get
            {
                return _hiddenStatus;
            }
            set
            {
                _hiddenStatus = value;
            }
        }


        // 行動距離
        private int _movementDistance = 0;

        public virtual int MovementDistance
        {
            get
            {
                return _movementDistance;
            }
            set
            {
                _movementDistance = value;
            }
        }


        // 表示用ロウフル
        private int _tempLawful = 0;

        public virtual int TempLawful
        {
            get
            {
                return _tempLawful;
            }
            set
            {
                _tempLawful = value;
            }
        }


        protected internal virtual int calcSleepTime(int sleepTime, int type)
        {
            switch (MoveSpeed)
            {
                case 0: // 通常
                    break;
                case 1: // ヘイスト
                    sleepTime -= (int)(sleepTime * 0.25);
                    break;
                case 2: // スロー
                    sleepTime *= 2;
                    break;
            }
            if (BraveSpeed == 1)
            {
                sleepTime -= (int)(sleepTime * 0.25);
            }
            if (hasSkillEffect(L1SkillId.WIND_SHACKLE))
            {
                if ((type == ATTACK_SPEED) || (type == MAGIC_SPEED))
                {
                    sleepTime += (int)(sleepTime * 0.25);
                }
            }
            return sleepTime;
        }

        protected internal virtual bool AiRunning
        {
            set
            {
                _aiRunning = value;
            }
            get
            {
                return _aiRunning;
            }
        }


        protected internal virtual bool Actived
        {
            set
            {
                _actived = value;
            }
            get
            {
                return _actived;
            }
        }


        protected internal virtual bool FirstAttack
        {
            set
            {
                _firstAttack = value;
            }
            get
            {
                return _firstAttack;
            }
        }


        protected internal virtual int SleepTime
        {
            set
            {
                _sleep_time = value;
            }
            get
            {
                return _sleep_time;
            }
        }


        protected internal virtual bool DeathProcessing
        {
            set
            {
                _deathProcessing = value;
            }
            get
            {
                return _deathProcessing;
            }
        }


        public virtual int drainMana(int drain)
        {
            if (_drainedMana >= Config.MANA_DRAIN_LIMIT_PER_NPC)
            {
                return 0;
            }
            int result = Math.Min(drain, CurrentMp);
            if (_drainedMana + result > Config.MANA_DRAIN_LIMIT_PER_NPC)
            {
                result = Config.MANA_DRAIN_LIMIT_PER_NPC - _drainedMana;
            }
            _drainedMana += result;
            return result;
        }

        public bool _destroyed = false; // このインスタンスが破棄されているか

        // ※破棄後に動かないよう強制的にＡＩ等のスレッド処理中止（念のため）

        // NPCが別のNPCに変わる場合の処理
        protected internal virtual void transform(int transformId)
        {
            stopHpRegeneration();
            stopMpRegeneration();
            int transformGfxId = NpcTemplate.TransformGfxId;
            if (transformGfxId != 0)
            {
                broadcastPacket(new S_SkillSound(Id, transformGfxId));
            }
            L1Npc npcTemplate = NpcTable.Instance.getTemplate(transformId);
            setting_template(npcTemplate);

            broadcastPacket(new S_NpcChangeShape(Id, TempCharGfx, Lawful, Status));
            foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(this))
            {
                onPerceive(pc);
            }

        }

        public virtual bool Rest
        {
            set
            {
                this._rest = value;
            }
            get
            {
                return _rest;
            }
        }


        private bool _isResurrect;

        public virtual bool Resurrect
        {
            get
            {
                return _isResurrect;
            }
            set
            {
                _isResurrect = value;
            }
        }


        /// <summary>
        /// 妖精森林 物品掉落 </summary>
        private bool _isDropitems = false;

        public virtual bool Dropitems
        {
            get
            {
                return _isDropitems;
            }
        }

        public virtual bool DropItems
        {
            set
            {
                _isDropitems = value;
            }
        }

        private bool _forDropitems = false;

        public virtual bool forDropitems()
        {
            return _forDropitems;
        }

        public virtual void giveDropItems(bool i)
        {
            _forDropitems = i;
        }

        public override void resurrect(int hp)
        {
            lock (this)
            {
                if (_destroyed)
                {
                    return;
                }
                if (_deleteTask != null)
                {
                    if (!_future.cancel(false))
                    { // キャンセルできない
                        return;
                    }
                    _deleteTask = null;
                    _future = null;
                }
                base.resurrect(hp);

                // キャンセレーションをエフェクトなしでかける
                // 本来は死亡時に行うべきだが、負荷が大きくなるため復活時に行う
                L1SkillUse skill = new L1SkillUse();
                skill.handleCommands(null, CANCELLATION, Id, X, Y, null, 0, L1SkillUse.TYPE_LOGIN, this);
            }
        }

        // 死んでから消えるまでの時間計測用
        private DeleteTimer _deleteTask;

        //JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
        //ORIGINAL LINE: private java.util.concurrent.ScheduledFuture<?> _future = null;
        private ScheduledFuture<object> _future = null;

        protected internal virtual void startDeleteTimer()
        {
            lock (this)
            {
                if (_deleteTask != null)
                {
                    return;
                }
                _deleteTask = new DeleteTimer(Id);
                _future = RunnableExecuter.Instance.schedule(_deleteTask, Config.NPC_DELETION_TIME * 1000);
            }
        }

        protected internal class DeleteTimer : TimerTask
        {
            internal int _id;

            protected internal DeleteTimer(int oId)
            {
                _id = oId;
                if (!(L1World.Instance.findObject(_id) is L1NpcInstance))
                {
                    throw new System.ArgumentException("allowed only L1NpcInstance");
                }
            }

            public override void run()
            {
                L1NpcInstance npc = (L1NpcInstance)L1World.Instance.findObject(_id);
                if ((npc == null) || !npc.Dead || npc._destroyed)
                {
                    return; // 復活してるか、既に破棄済みだったら抜け
                }
                try
                {
                    npc.deleteMe();
                }
                catch (Exception e)
                { // 絶対例外を投げないように
                    System.Console.WriteLine(e.ToString());
                    System.Console.Write(e.StackTrace);
                }
            }
        }

        private L1MobGroupInfo _mobGroupInfo = null;

        public virtual bool InMobGroup
        {
            get
            {
                return MobGroupInfo != null;
            }
        }

        public virtual L1MobGroupInfo MobGroupInfo
        {
            get
            {
                return _mobGroupInfo;
            }
            set
            {
                _mobGroupInfo = value;
            }
        }


        private int _mobGroupId = 0;

        public virtual int MobGroupId
        {
            get
            {
                return _mobGroupId;
            }
            set
            {
                _mobGroupId = value;
            }
        }


        public virtual void startChat(int chatTiming)
        {
            // 出現時のチャットにも関わらず死亡中、死亡時のチャットにも関わらず生存中
            if ((chatTiming == CHAT_TIMING_APPEARANCE) && Dead)
            {
                return;
            }
            if ((chatTiming == CHAT_TIMING_DEAD) && !Dead)
            {
                return;
            }
            if ((chatTiming == CHAT_TIMING_HIDE) && Dead)
            {
                return;
            }
            if ((chatTiming == CHAT_TIMING_GAME_TIME) && Dead)
            {
                return;
            }

            int npcId = NpcTemplate.get_npcId();
            L1NpcChat npcChat = null;
            if (chatTiming == CHAT_TIMING_APPEARANCE)
            {
                npcChat = NpcChatTable.Instance.getTemplateAppearance(npcId);
            }
            else if (chatTiming == CHAT_TIMING_DEAD)
            {
                npcChat = NpcChatTable.Instance.getTemplateDead(npcId);
            }
            else if (chatTiming == CHAT_TIMING_HIDE)
            {
                npcChat = NpcChatTable.Instance.getTemplateHide(npcId);
            }
            else if (chatTiming == CHAT_TIMING_GAME_TIME)
            {
                npcChat = NpcChatTable.Instance.getTemplateGameTime(npcId);
            }
            if (npcChat == null)
            {
                return;
            }

            Timer timer = new Timer(true);
            L1NpcChatTimer npcChatTimer = new L1NpcChatTimer(this, npcChat);
            if (!npcChat.Repeat)
            {
                timer.schedule(npcChatTimer, npcChat.StartDelayTime);
            }
            else
            {
                timer.scheduleAtFixedRate(npcChatTimer, npcChat.StartDelayTime, npcChat.RepeatInterval);
            }
        }

        public virtual int AtkRanged
        {
            get
            {
                if (_polyAtkRanged == -1)
                {
                    return NpcTemplate.get_ranged();
                }
                return _polyAtkRanged;
            }
        }

        private int _polyAtkRanged = -1;

        public virtual int PolyAtkRanged
        {
            get
            {
                return _polyAtkRanged;
            }
            set
            {
                _polyAtkRanged = value;
            }
        }


        private int _polyArrowGfx = 0;

        public virtual int PolyArrowGfx
        {
            get
            {
                return _polyArrowGfx;
            }
            set
            {
                _polyArrowGfx = value;
            }
        }

        public int MaxHp { get; internal set; }
        public L1Map Map { get; internal set; }
        public int MaxMp { get; private set; }
        public int Level { get; private set; }
        public sbyte Str { get; private set; }
        public sbyte Con { get; private set; }
        public sbyte Dex { get; private set; }
        public sbyte Int { get; private set; }
        public sbyte Wis { get; private set; }
    }

}