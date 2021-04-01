using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.skill;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LineageServer.Server.Model.Instance
{
    class L1SummonInstance : L1NpcInstance
    {
        private const int SUMMON_TIME = 3600000;

        private int _currentPetStatus;

        private bool _tamed;

        private bool _isReturnToNature = false;

        private int _dir;

        // ターゲットがいない場合の処理
        public override bool noTarget()
        {
            switch (_currentPetStatus)
            {
                case 3: // 休息
                    return true;
                case 4: // 散開
                    if ((_master != null) && (_master.MapId == MapId) && (Location.getTileLineDistance(_master.Location) < 5))
                    {
                        _dir = targetReverseDirection(_master.X, _master.Y);
                        _dir = checkObject(X, Y, MapId, _dir);
                        DirectionMove = _dir;
                        SleepTime = calcSleepTime(Passispeed, MOVE_SPEED);
                    }
                    else
                    {
                        _currentPetStatus = 3;
                        return true;
                    }
                    return false;
                case 5:
                    if ((Math.Abs(HomeX - X) > 1) || (Math.Abs(HomeY - Y) > 1))
                    {
                        _dir = moveDirection(HomeX, HomeY);
                        if (_dir == -1)
                        {
                            HomeX = X;
                            HomeY = Y;
                        }
                        else
                        {
                            DirectionMove = _dir;
                            SleepTime = calcSleepTime(Passispeed, MOVE_SPEED);
                        }
                    }
                    return false;
                default:
                    if ((_master != null) && (_master.MapId == MapId))
                    {
                        if (Location.getTileLineDistance(_master.Location) > 2)
                        {
                            _dir = moveDirection(_master.X, _master.Y);
                            DirectionMove = _dir;
                            SleepTime = calcSleepTime(Passispeed, MOVE_SPEED);
                        }
                    }
                    else
                    {
                        _currentPetStatus = 3;
                        return true;
                    }
                    return false;
            }
        }

        // １時間計測用
        internal class SummonTimer : TimerTask
        {
            private readonly L1SummonInstance outerInstance;

            public SummonTimer(L1SummonInstance outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public void run()
            {
                if (IsCancel)
                {
                    return;
                }
                if (outerInstance._destroyed)
                { // 既に破棄されていないかチェック
                    return;
                }
                if (outerInstance._tamed)
                {
                    // テイミングモンスター、クリエイトゾンビの解放
                    outerInstance.liberate();
                }
                else
                {
                    // サモンの解散
                    outerInstance.Death(null);
                }
            }
        }
        private ITimerTask _summonFuture;
        // サモンモンスター用
        public L1SummonInstance(L1Npc template, L1Character master) : base(template)
        {
            Id = Container.Instance.Resolve<IIdFactory>().nextId();

            _summonFuture = new SummonTimer(this);
			Container.Instance.Resolve<ITaskController>().execute((IRunnable)this._summonFuture, SUMMON_TIME);

            Master = master;
            X = master.X + RandomHelper.Next(5) - 2;
            Y = master.Y + RandomHelper.Next(5) - 2;
            MapId = master.MapId;
            Heading = 5;
            LightSize = template.LightSize;

            _currentPetStatus = 3;
            _tamed = false;

            Container.Instance.Resolve<IGameWorld>().storeObject(this);
            Container.Instance.Resolve<IGameWorld>().addVisibleObject(this);
            foreach (L1PcInstance pc in Container.Instance.Resolve<IGameWorld>().getRecognizePlayer(this))
            {
                onPerceive(pc);
            }
            master.addPet(this);
        }

        // 造屍術處理
        public L1SummonInstance(L1NpcInstance target, L1Character master, bool isCreateZombie) : base(null)
        {
            Id = Container.Instance.Resolve<IIdFactory>().nextId();

            if (isCreateZombie)
            { // クリエイトゾンビ
                int npcId = 45065;
                L1PcInstance pc = (L1PcInstance)master;
                int level = pc.Level;
                if (pc.Wizard)
                {
                    if ((level >= 24) && (level <= 31))
                    {
                        npcId = 81183;
                    }
                    else if ((level >= 32) && (level <= 39))
                    {
                        npcId = 81184;
                    }
                    else if ((level >= 40) && (level <= 43))
                    {
                        npcId = 81185;
                    }
                    else if ((level >= 44) && (level <= 47))
                    {
                        npcId = 81186;
                    }
                    else if ((level >= 48) && (level <= 51))
                    {
                        npcId = 81187;
                    }
                    else if (level >= 52)
                    {
                        npcId = 81188;
                    }
                }
                else if (pc.Elf)
                {
                    if (level >= 48)
                    {
                        npcId = 81183;
                    }
                }
                L1Npc template = Container.Instance.Resolve<INpcController>().getTemplate(npcId).clone();
                setting_template(template);
            }
            else
            { // テイミングモンスター
                setting_template(target.NpcTemplate);
                CurrentHpDirect = target.CurrentHp;
                CurrentMpDirect = target.CurrentMp;
            }

            _summonFuture = new SummonTimer(this);
			Container.Instance.Resolve<ITaskController>().execute((IRunnable)this._summonFuture, SUMMON_TIME);

            Master = master;
            X = target.X;
            Y = target.Y;
            MapId = target.MapId;
            Heading = target.Heading;
            LightSize = target.LightSize;
            Petcost = 6;

            if ((target is L1MonsterInstance) && !((L1MonsterInstance)target).is_storeDroped())
            {
                DropTable.Instance.setDrop(target, target.Inventory);
            }
            Inventory = target.Inventory;
            target.Inventory = null;

            _currentPetStatus = 3;
            _tamed = true;

            // ペットが攻撃中だった場合止めさせる
            foreach (L1NpcInstance each in master.PetList.Values)
            {
                each.targetRemove(target);
            }

            target.deleteMe();
            Container.Instance.Resolve<IGameWorld>().storeObject(this);
            Container.Instance.Resolve<IGameWorld>().addVisibleObject(this);
            foreach (L1PcInstance pc in Container.Instance.Resolve<IGameWorld>().getRecognizePlayer(this))
            {
                onPerceive(pc);
            }
            master.addPet(this);
        }

        public override void receiveDamage(L1Character attacker, int damage)
        { // 攻撃でＨＰを減らすときはここを使用
            if (CurrentHp > 0)
            {
                if (damage > 0)
                {
                    setHate(attacker, 0); // サモンはヘイト無し
                    removeSkillEffect(L1SkillId.FOG_OF_SLEEPING);
                    if (!ExsistMaster)
                    {
                        _currentPetStatus = 1;
                        Target = attacker;
                    }
                }

                if ((attacker is L1PcInstance) && (damage > 0))
                {
                    L1PcInstance player = (L1PcInstance)attacker;
                    player.PetTarget = this;
                }

                if (attacker is L1PetInstance)
                {
                    L1PetInstance pet = (L1PetInstance)attacker;
                    // 目標在安區、攻擊者在安區、NOPVP
                    if ((ZoneType == 1) || (pet.ZoneType == 1))
                    {
                        damage = 0;
                    }
                }
                else if (attacker is L1SummonInstance)
                {
                    L1SummonInstance summon = (L1SummonInstance)attacker;
                    // 目標在安區、攻擊者在安區、NOPVP
                    if ((ZoneType == 1) || (summon.ZoneType == 1))
                    {
                        damage = 0;
                    }
                }

                int newHp = CurrentHp - damage;
                if (newHp <= 0)
                {
                    Death(attacker);
                }
                else
                {
                    CurrentHp = newHp;
                }
            }
            else if (!Dead) // 念のため
            {
                System.Console.WriteLine("警告：サモンのＨＰ減少処理が正しく行われていない箇所があります。※もしくは最初からＨＰ０");
                Death(attacker);
            }
        }

        public virtual void Death(L1Character lastAttacker)
        {
            lock (this)
            {
                if (!Dead)
                {
                    Dead = true;
                    CurrentHp = 0;
                    Status = ActionCodes.ACTION_Die;

                    Map.setPassable(Location, true);

                    // 死亡時物品給予主人或掉落地面
                    L1Inventory targetInventory = _master.Inventory;
                    IList<L1ItemInstance> items = _inventory.Items;
                    foreach (L1ItemInstance item in items)
                    {
                        if (_master.Inventory.checkAddItem(item, item.Count) == L1Inventory.OK)
                        {
                            _inventory.tradeItem(item, item.Count, targetInventory);
                            // \f1%0が%1をくれました。
                            ((L1PcInstance)_master).sendPackets(new S_ServerMessage(143, Name, item.LogName));
                        }
                        else
                        { // 持てないので足元に落とす
                            targetInventory = Container.Instance.Resolve<IGameWorld>().getInventory(X, Y, MapId);
                            _inventory.tradeItem(item, item.Count, targetInventory);
                        }
                    }

                    if (_tamed)
                    {
                        broadcastPacket(new S_DoActionGFX(Id, ActionCodes.ACTION_Die));
                        startDeleteTimer();
                    }
                    else
                    {
                        deleteMe();
                    }
                }
            }
        }

        public virtual void returnToNature()
        {
            lock (this)
            {
                _isReturnToNature = true;
                if (!_tamed)
                {
                    Map.setPassable(Location, true);
                    // アイテム解放処理
                    L1Inventory targetInventory = _master.Inventory;
                    IList<L1ItemInstance> items = _inventory.Items;
                    foreach (L1ItemInstance item in items)
                    {
                        if (_master.Inventory.checkAddItem(item, item.Count) == L1Inventory.OK)
                        {
                            _inventory.tradeItem(item, item.Count, targetInventory);
                            // \f1%0が%1をくれました。
                            ((L1PcInstance)_master).sendPackets(new S_ServerMessage(143, Name, item.LogName));
                        }
                        else
                        { // 持てないので足元に落とす
                            targetInventory = Container.Instance.Resolve<IGameWorld>().getInventory(X, Y, MapId);
                            _inventory.tradeItem(item, item.Count, targetInventory);
                        }
                    }
                    deleteMe();
                }
                else
                {
                    liberate();
                }
            }
        }

        // オブジェクト消去処理
        public override void deleteMe()
        {
            lock (this)
            {
                if (_destroyed)
                {
                    return;
                }
                if (!_tamed && !_isReturnToNature)
                {
                    broadcastPacket(new S_SkillSound(Id, 169));
                }
                //if (_master.getPetList().isEmpty()) {
                L1PcInstance pc = (L1PcInstance)_master;
                if (pc is L1PcInstance)
                {
                    pc.sendPackets(new S_PetCtrlMenu(_master, this, false)); // 關閉寵物控制圖形介面
                }
                //}
                _master.PetList.Remove(Id);
                base.deleteMe();

                if (_summonFuture != null)
                {
                    _summonFuture.cancel();
                    _summonFuture = null;
                }
            }
        }

        // 迷魅的怪物解散處理
        public virtual void liberate()
        {
            L1MonsterInstance monster = new L1MonsterInstance(NpcTemplate);
            monster.Id = Container.Instance.Resolve<IIdFactory>().nextId();

            monster.X = X;
            monster.Y = Y;
            monster.MapId = MapId;
            monster.Heading = Heading;
            monster.set_storeDroped(true);
            monster.Inventory = Inventory;
            Inventory.clearItems();
            monster.CurrentHpDirect = CurrentHp;
            monster.CurrentMpDirect = CurrentMp;
            monster.Exp = 0;

            if (!Dead)
            { // 原迷魅怪解散時死亡
                Dead = true;
                CurrentHp = 0;
                Map.setPassable(Location, true);
            }
            deleteMe();
            Container.Instance.Resolve<IGameWorld>().storeObject(monster);
            Container.Instance.Resolve<IGameWorld>().addVisibleObject(monster);
        }

        public virtual L1Character Target
        {
            set
            {
                if ((value != null) && ((_currentPetStatus == 1) || (_currentPetStatus == 2) || (_currentPetStatus == 5)))
                {
                    setHate(value, 0);
                    if (!AiRunning)
                    {
                        startAI();
                    }
                }
            }
        }

        public virtual L1Character MasterTarget
        {
            set
            {
                if ((value != null) && ((_currentPetStatus == 1) || (_currentPetStatus == 5)))
                {
                    setHate(value, 0);
                    if (!AiRunning)
                    {
                        startAI();
                    }
                }
            }
        }

        public override void onAction(L1PcInstance attacker)
        {
            onAction(attacker, 0);
        }

        public override void onAction(L1PcInstance attacker, int skillId)
        {
            // XXX:NullPointerException回避。onActionの引数の型はL1Characterのほうが良い？
            if (attacker == null)
            {
                return;
            }
            L1Character cha = Master;
            if (cha == null)
            {
                return;
            }
            L1PcInstance master = (L1PcInstance)cha;
            if (master.Teleport)
            {
                // テレポート処理中
                return;
            }
            if (((ZoneType == 1) || (attacker.ZoneType == 1)) && ExsistMaster)
            {
                // 攻撃される側がセーフティーゾーン
                // 攻撃モーション送信
                L1Attack attack_mortion = new L1Attack(attacker, this, skillId);
                attack_mortion.action();
                return;
            }

            if (attacker.checkNonPvP(attacker, this))
            {
                return;
            }

            L1Attack attack = new L1Attack(attacker, this, skillId);
            if (attack.calcHit())
            {
                attack.calcDamage();
            }
            attack.action();
            attack.commit();
        }

        public override void onTalkAction(L1PcInstance player)
        {
            if (Dead)
            {
                return;
            }
            if (_master == player)
            {
                player.sendPackets(new S_PetMenuPacket(this, 0));
            }
        }

        public override void onFinalAction(L1PcInstance player, string action)
        {
            int status = ActionType(action);
            if (status == 0)
            {
                return;
            }
            if (status == 6)
            {
                L1PcInstance petMaster = (L1PcInstance)_master;
                if (_tamed)
                {
                    // テイミングモンスター、クリエイトゾンビの解放
                    liberate();
                }
                else
                {
                    // サモンの解散
                    Death(null);
                }
                // 更新寵物控制介面
                var petList = petMaster.PetList.Values.ToArray();
                foreach (var petObject in petList)
                {
                    if (petObject is L1SummonInstance)
                    {
                        L1SummonInstance summon = (L1SummonInstance)petObject;
                        petMaster.sendPackets(new S_SummonPack(summon, petMaster));
                        return;
                    }
                    else if (petObject is L1PetInstance)
                    {
                        L1PetInstance pet = (L1PetInstance)petObject;
                        petMaster.sendPackets(new S_PetPack(pet, petMaster));
                        return;
                    }
                }
            }
            else
            {
                // 同じ主人のペットの状態をすべて更新
                var petList = _master.PetList.Values.ToArray();
                foreach (var petObject in petList)
                {
                    if (petObject is L1SummonInstance)
                    {
                        // サモンモンスター
                        L1SummonInstance summon = (L1SummonInstance)petObject;
                        summon.set_currentPetStatus(status);
                    }
                    else if (petObject is L1PetInstance)
                    { // ペット
                        L1PetInstance pet = (L1PetInstance)petObject;
                        if ((player != null) && (player.Level >= pet.Level) && pet.get_food() > 0)
                        {
                            pet.CurrentPetStatus = status;
                        }
                        else
                        {
                            if (!pet.Dead)
                            {
                                L1PetType type = PetTypeTable.Instance.get(pet.NpcTemplate.get_npcId());
                                int id = type.DefyMessageId;
                                if (id != 0)
                                {
                                    pet.broadcastPacket(new S_NpcChatPacket(pet, "$" + id, 0));
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void onPerceive(L1PcInstance perceivedFrom)
        {
            perceivedFrom.addKnownObject(this);
            perceivedFrom.sendPackets(new S_SummonPack(this, perceivedFrom));
        }

        public override void onItemUse()
        {
            if (!Actived)
            {
                // １００％の確率でヘイストポーション使用
                useItem(USEITEM_HASTE, 100);
            }
            if (CurrentHp * 100 / MaxHp < 40)
            {
                // ＨＰが４０％きったら
                // １００％の確率で回復ポーション使用
                useItem(USEITEM_HEAL, 100);
            }
        }

        public override void onGetItem(L1ItemInstance item)
        {
            if (NpcTemplate.get_digestitem() > 0)
            {
                DigestItem = item;
            }
            Array.Sort(healPotions);
            Array.Sort(haestPotions);
            if (Array.BinarySearch(healPotions, item.Item.ItemId) >= 0)
            {
                if (CurrentHp != MaxHp)
                {
                    useItem(USEITEM_HEAL, 100);
                }
            }
            else if (Array.BinarySearch(haestPotions, item.Item.ItemId) >= 0)
            {
                useItem(USEITEM_HASTE, 100);
            }
        }

        private int ActionType(string action)
        {
            int status = 0;
            if (action == "aggressive")
            { // 攻撃態勢
                status = 1;
            }
            else if (action == "defensive")
            { // 防御態勢
                status = 2;
            }
            else if (action == "stay")
            { // 休憩
                status = 3;
            }
            else if (action == "extend")
            { // 配備
                status = 4;
            }
            else if (action == "alert")
            { // 警戒
                status = 5;
            }
            else if (action == "dismiss")
            { // 解散
                status = 6;
            }
            return status;
        }

        public override int CurrentHp
        {
            set
            {
                int currentHp = value;
                if (currentHp >= MaxHp)
                {
                    currentHp = MaxHp;
                }
                CurrentHpDirect = currentHp;

                if (MaxHp > CurrentHp)
                {
                    startHpRegeneration();
                }

                if (_master is L1PcInstance)
                {
                    int HpRatio = 100 * currentHp / MaxHp;
                    L1PcInstance Master = (L1PcInstance)_master;
                    Master.sendPackets(new S_HPMeter(Id, HpRatio));
                }
            }
        }

        public override int CurrentMp
        {
            set
            {
                int currentMp = value;
                if (currentMp >= MaxMp)
                {
                    currentMp = MaxMp;
                }
                CurrentMpDirect = currentMp;

                if (MaxMp > CurrentMp)
                {
                    startMpRegeneration();
                }
            }
        }

        public virtual void set_currentPetStatus(int i)
        {
            _currentPetStatus = i;
            if (_currentPetStatus == 5)
            {
                HomeX = X;
                HomeY = Y;
            }

            if (_currentPetStatus == 3)
            {
                allTargetClear();
            }
            else
            {
                if (!AiRunning)
                {
                    startAI();
                }
            }
        }

        public virtual int get_currentPetStatus()
        {
            return _currentPetStatus;
        }

        public virtual bool ExsistMaster
        {
            get
            {
                bool isExsistMaster = true;
                if (Master != null)
                {
                    string masterName = Master.Name;
                    if (Container.Instance.Resolve<IGameWorld>().getPlayer(masterName) == null)
                    {
                        isExsistMaster = false;
                    }
                }
                return isExsistMaster;
            }
        }

    }

}