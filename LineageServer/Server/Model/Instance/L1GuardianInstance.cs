using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataSources;
using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.skill;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
namespace LineageServer.Server.Model.Instance
{
    class L1GuardianInstance : L1NpcInstance
    {
        private bool InstanceFieldsInitialized = false;

        private void InitializeInstanceFields()
        {
            _npc = this;
        }

        private static ILogger _log = Logger.GetLogger(nameof(L1GuardianInstance));

        private L1GuardianInstance _npc;

        private int GDROPITEM_TIME = Config.GDROPITEM_TIME;

        /// <param name="template"> </param>
        public L1GuardianInstance(L1Npc template) : base(template)
        {
            if (!InstanceFieldsInitialized)
            {
                InitializeInstanceFields();
                InstanceFieldsInitialized = true;
            }
            if (!Dropitems)
            {
                doGDropItem(0);
            }
        }

        public override void searchTarget()
        {
            // ターゲット検索
            L1PcInstance targetPlayer = null;

            foreach (L1PcInstance pc in L1World.Instance.getVisiblePlayer(this))
            {
                if ((pc.CurrentHp <= 0) || pc.Dead || pc.Gm || pc.Ghost)
                {
                    continue;
                }
                if (!pc.Invisble || NpcTemplate.is_agrocoi())
                { // インビジチェック
                    if (!pc.Elf)
                    { // エルフ以外
                        targetPlayer = pc;
                        wideBroadcastPacket(new S_NpcChatPacket(this, "$804", 2)); // エルフ以外の者よ、命が惜しければ早くここから去れ。ここは神聖な場所だ。
                        break;
                    }
                    else if (pc.Elf && pc.WantedForElf)
                    {
                        targetPlayer = pc;
                        wideBroadcastPacket(new S_NpcChatPacket(this, "$815", 1)); // 同族を殺したものは、己の血でその罪をあがなうことになるだろう。
                        break;
                    }
                }
            }
            if (targetPlayer != null)
            {
                _hateList.add(targetPlayer, 0);
                _target = targetPlayer;
            }
        }

        // リンクの設定
        public override L1Character Link
        {
            set
            {
                if ((value != null) && _hateList.Empty)
                { // ターゲットがいない場合のみ追加
                    _hateList.add(value, 0);
                    checkTarget();
                }
            }
        }

        public override void onNpcAI()
        {
            if (AiRunning)
            {
                return;
            }
            Actived = false;
            startAI();
        }

        public override void onAction(L1PcInstance pc)
        {
            onAction(pc, 0);
        }

        public virtual void doGDropItem(int timer)
        {
            GDropItemTask task = new GDropItemTask(this);
            RunnableExecuter.Instance.execute(task, timer * 60000);
        }

        private class GDropItemTask : IRunnable
        {
            internal bool InstanceFieldsInitialized = false;

            internal virtual void InitializeInstanceFields()
            {
                npcId = outerInstance.NpcTemplate.get_npcId();
            }

            private readonly L1GuardianInstance outerInstance;

            internal int npcId;

            internal GDropItemTask(L1GuardianInstance outerInstance)
            {
                this.outerInstance = outerInstance;

                if (!InstanceFieldsInitialized)
                {
                    InitializeInstanceFields();
                    InstanceFieldsInitialized = true;
                }
            }

            public void run()
            {
                try
                {
                    if (outerInstance.GDROPITEM_TIME > 0 && !outerInstance.Dropitems)
                    {
                        if (npcId == 70848)
                        { // 安特
                            if (!outerInstance._inventory.checkItem(40505) && !outerInstance._inventory.checkItem(40506) && !outerInstance._inventory.checkItem(40507))
                            {
                                outerInstance._inventory.storeItem(40506, 1);
                                outerInstance._inventory.storeItem(40507, 66);
                                outerInstance._inventory.storeItem(40505, 8);
                            }
                        }
                        if (npcId == 70850)
                        { // 潘
                            if (!outerInstance._inventory.checkItem(40519))
                            {
                                outerInstance._inventory.storeItem(40519, 30);
                            }
                        }
                        outerInstance.DropItems = true;
                        outerInstance.giveDropItems(true);
                        outerInstance.doGDropItem(outerInstance.GDROPITEM_TIME);
                    }
                    else
                    {
                        outerInstance.giveDropItems(false);
                    }
                }
                catch (Exception e)
                {
                    _log.Error(e);
                }
            }
        }

        public override void onAction(L1PcInstance pc, int skillId)
        {
            if ((pc.Type == 2) && (pc.CurrentWeapon == 0) && pc.Elf)
            {
                L1Attack attack = new L1Attack(pc, this, skillId);

                if (attack.calcHit())
                {
                    try
                    {
                        int chance = 0;
                        int npcId = NpcTemplate.get_npcId();
                        string npcName = NpcTemplate.get_name();
                        string itemName = "";
                        int itemCount = 0;
                        L1Item item40499 = ItemTable.Instance.getTemplate(40499);
                        L1Item item40503 = ItemTable.Instance.getTemplate(40503);
                        L1Item item40505 = ItemTable.Instance.getTemplate(40505);
                        L1Item item40506 = ItemTable.Instance.getTemplate(40506);
                        L1Item item40507 = ItemTable.Instance.getTemplate(40507);
                        L1Item item40519 = ItemTable.Instance.getTemplate(40519);
                        if (npcId == 70848)
                        { // 安特
                            if (_inventory.checkItem(40499) && !_inventory.checkItem(40505))
                            { // 蘑菇汁 換
                              // 安特之樹皮
                                itemName = item40505.Name;
                                itemCount = _inventory.countItems(40499);
                                if (itemCount > 1)
                                {
                                    itemName += " (" + itemCount + ")";
                                }
                                _inventory.consumeItem(40499, itemCount);
                                pc.Inventory.storeItem(40505, itemCount);
                                pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message143, npcName, itemName));
                                if (!Dropitems)
                                {
                                    doGDropItem(3);
                                }
                            }
                            if (_inventory.checkItem(40505))
                            { // 安特之樹皮
                                chance = RandomHelper.Next(100) + 1;
                                if (chance <= 60 && chance >= 50)
                                {
                                    itemName = item40505.Name;
                                    _inventory.consumeItem(40505, 1);
                                    pc.Inventory.storeItem(40505, 1);
                                    pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message143, npcName, itemName));
                                }
                                else
                                {
                                    itemName = item40499.Name;
                                    pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message337, itemName));
                                }
                            }
                            else if (_inventory.checkItem(40507) && !_inventory.checkItem(40505))
                            { // 安特之樹枝
                                chance = RandomHelper.Next(100) + 1;
                                if (chance <= 40 && chance >= 25)
                                {
                                    itemName = item40507.Name;
                                    itemName += " (6)";
                                    _inventory.consumeItem(40507, 6);
                                    pc.Inventory.storeItem(40507, 6);
                                    pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message143, npcName, itemName));
                                }
                                else
                                {
                                    itemName = item40499.Name;
                                    pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message337, itemName));
                                }
                            }
                            else if (_inventory.checkItem(40506) && !_inventory.checkItem(40507))
                            { // 安特的水果
                                chance = RandomHelper.Next(100) + 1;
                                if (chance <= 90 && chance >= 85)
                                {
                                    itemName = item40506.Name;
                                    _inventory.consumeItem(40506, 1);
                                    pc.Inventory.storeItem(40506, 1);
                                    pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message143, npcName, itemName));
                                }
                                else
                                {
                                    itemName = item40499.Name;
                                    pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message337, itemName));
                                }
                            }
                            else
                            {
                                if (!forDropitems())
                                {
                                    DropItems = false;
                                    doGDropItem(GDROPITEM_TIME);
                                }
                                chance = RandomHelper.Next(100) + 1;
                                if (chance <= 80 && chance >= 40)
                                {
                                    broadcastPacket(new S_NpcChatPacket(_npc, "$822", 0));
                                }
                                else
                                {
                                    itemName = item40499.Name;
                                    pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message337, itemName));
                                }
                            }
                        }
                        if (npcId == 70850)
                        { // 潘
                            if (_inventory.checkItem(40519))
                            { // 潘的鬃毛
                                chance = RandomHelper.Next(100) + 1;
                                if (chance <= 25)
                                {
                                    itemName = item40519.Name;
                                    itemName += " (5)";
                                    _inventory.consumeItem(40519, 5);
                                    pc.Inventory.storeItem(40519, 5);
                                    pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message143, npcName, itemName));
                                }
                            }
                            else
                            {
                                if (!forDropitems())
                                {
                                    DropItems = false;
                                    doGDropItem(GDROPITEM_TIME);
                                }
                                chance = RandomHelper.Next(100) + 1;
                                if (chance <= 80 && chance >= 40)
                                {
                                    broadcastPacket(new S_NpcChatPacket(_npc, "$824", 0));
                                }
                            }
                        }
                        if (npcId == 70846)
                        { // 芮克妮
                            if (_inventory.checkItem(40507))
                            { // 安特之樹枝 換 芮克妮的網
                                itemName = item40503.Name;
                                itemCount = _inventory.countItems(40507);
                                if (itemCount > 1)
                                {
                                    itemName += " (" + itemCount + ")";
                                }
                                _inventory.consumeItem(40507, itemCount);
                                pc.Inventory.storeItem(40503, itemCount);
                                pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message143, npcName, itemName));
                            }
                            else
                            {
                                itemName = item40507.Name;
                                pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message337, itemName)); // \\f1%0不足%s。
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _log.Error(e);
                    }
                    attack.calcDamage();
                    attack.calcStaffOfMana();
                    attack.addPcPoisonAttack(pc, this);
                    attack.addChaserAttack();
                }
                attack.action();
                attack.commit();
            }
            else if ((CurrentHp > 0) && !Dead)
            {
                L1Attack attack = new L1Attack(pc, this, skillId);
                if (attack.calcHit())
                {
                    attack.calcDamage();
                    attack.calcStaffOfMana();
                    attack.addPcPoisonAttack(pc, this);
                    attack.addChaserAttack();
                }
                attack.action();
                attack.commit();
            }
        }

        public override void onTalkAction(L1PcInstance player)
        {
            int objid = Id;
            L1NpcTalkData talking = NPCTalkDataTable.Instance.getTemplate(NpcTemplate.get_npcId());
            GameObject @object = L1World.Instance.findObject(Id);
            L1NpcInstance target = (L1NpcInstance)@object;

            if (talking != null)
            {
                int pcx = player.X; // PCのX座標
                int pcy = player.Y; // PCのY座標
                int npcx = target.X; // NPCのX座標
                int npcy = target.Y; // NPCのY座標

                if ((pcx == npcx) && (pcy < npcy))
                {
                    Heading = 0;
                }
                else if ((pcx > npcx) && (pcy < npcy))
                {
                    Heading = 1;
                }
                else if ((pcx > npcx) && (pcy == npcy))
                {
                    Heading = 2;
                }
                else if ((pcx > npcx) && (pcy > npcy))
                {
                    Heading = 3;
                }
                else if ((pcx == npcx) && (pcy > npcy))
                {
                    Heading = 4;
                }
                else if ((pcx < npcx) && (pcy > npcy))
                {
                    Heading = 5;
                }
                else if ((pcx < npcx) && (pcy == npcy))
                {
                    Heading = 6;
                }
                else if ((pcx < npcx) && (pcy < npcy))
                {
                    Heading = 7;
                }
                broadcastPacket(new S_ChangeHeading(this));

                // html表示パケット送信
                if (player.Lawful < -1000)
                { // プレイヤーがカオティック
                    player.sendPackets(new S_NPCTalkReturn(talking, objid, 2));
                }
                else
                {
                    player.sendPackets(new S_NPCTalkReturn(talking, objid, 1));
                }

                // 動かないようにする
                lock (this)
                {
                    if (_monitor != null)
                    {
                        _monitor.cancel();
                    }
                    Rest = true;
                    _monitor = new RestMonitor(this);
                    RunnableExecuter.Instance.execute(_monitor, REST_MILLISEC);
                }
            }
        }

        public override void receiveDamage(L1Character attacker, int damage)
        { // 攻撃でＨＰを減らすときはここを使用
            if ((attacker is L1PcInstance) && (damage > 0))
            {
                L1PcInstance pc = (L1PcInstance)attacker;
                if ((pc.Type == 2) && (pc.CurrentWeapon == 0))
                {
                }
                else
                {
                    if ((CurrentHp > 0) && !Dead)
                    {
                        if (damage >= 0)
                        {
                            setHate(attacker, damage);
                        }
                        if (damage > 0)
                        {
                            removeSkillEffect(L1SkillId.FOG_OF_SLEEPING);
                        }
                        onNpcAI();
                        // 仲間意識をもつモンスターのターゲットに設定
                        serchLink(pc, NpcTemplate.get_family());
                        if (damage > 0)
                        {
                            pc.PetTarget = this;
                        }

                        int newHp = CurrentHp - damage;
                        if ((newHp <= 0) && !Dead)
                        {
                            CurrentHpDirect = 0;
                            Dead = true;
                            Status = ActionCodes.ACTION_Die;
                            _lastattacker = attacker;
                            Death death = new Death(this);
                            RunnableExecuter.Instance.execute(death);
                        }
                        if (newHp > 0)
                        {
                            CurrentHp = newHp;
                        }
                    }
                    else if (!Dead)
                    { // 念のため
                        Dead = true;
                        Status = ActionCodes.ACTION_Die;
                        _lastattacker = attacker;
                        Death death = new Death(this);
                        RunnableExecuter.Instance.execute(death);
                    }
                }
            }
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

        private L1Character _lastattacker;

        internal class Death : IRunnable
        {
            internal bool InstanceFieldsInitialized = false;

            internal virtual void InitializeInstanceFields()
            {
                lastAttacker = outerInstance._lastattacker;
            }

            private readonly L1GuardianInstance outerInstance;

            public Death(L1GuardianInstance outerInstance)
            {
                this.outerInstance = outerInstance;

                if (!InstanceFieldsInitialized)
                {
                    InitializeInstanceFields();
                    InstanceFieldsInitialized = true;
                }
            }

            internal L1Character lastAttacker;

            public void run()
            {
                outerInstance.DeathProcessing = true;
                outerInstance.CurrentHpDirect = 0;
                outerInstance.Dead = true;
                outerInstance.Status = ActionCodes.ACTION_Die;
                int targetobjid = outerInstance.Id;
                outerInstance.Map.setPassable(outerInstance.Location, true);
                outerInstance.broadcastPacket(new S_DoActionGFX(targetobjid, ActionCodes.ACTION_Die));

                L1PcInstance player = null;
                if (lastAttacker is L1PcInstance)
                {
                    player = (L1PcInstance)lastAttacker;
                }
                else if (lastAttacker is L1PetInstance)
                {
                    player = (L1PcInstance)((L1PetInstance)lastAttacker).Master;
                }
                else if (lastAttacker is L1SummonInstance)
                {
                    player = (L1PcInstance)((L1SummonInstance)lastAttacker).Master;
                }
                if (player != null)
                {
                    IList<L1Character> targetList = outerInstance._hateList.toTargetArrayList();
                    IList<int> hateList = outerInstance._hateList.toHateArrayList();
                    long exp = outerInstance.Exp;
                    CalcExp.calcExp(player, targetobjid, targetList, hateList, exp);

                    IList<L1Character> dropTargetList = outerInstance._dropHateList.toTargetArrayList();
                    IList<int> dropHateList = outerInstance._dropHateList.toHateArrayList();
                    try
                    {
                        DropTable.Instance.dropShare(outerInstance._npc, dropTargetList, dropHateList);
                    }
                    catch (Exception e)
                    {
                        _log.Error(e);
                    }
                    // カルマは止めを刺したプレイヤーに設定。ペットorサモンで倒した場合も入る。
                    player.addKarma((int)(outerInstance.Karma * Config.RATE_KARMA));
                }
                outerInstance.DeathProcessing = false;

                outerInstance.Karma = 0;
                outerInstance.Exp = 0;
                outerInstance.allTargetClear();

                outerInstance.startDeleteTimer();
            }
        }

        public override void onFinalAction(L1PcInstance player, string action)
        {
        }

        public virtual void doFinalAction(L1PcInstance player)
        {
        }

        private const int REST_MILLISEC = 10000;

        private RestMonitor _monitor;

        public class RestMonitor : TimerTask
        {
            private readonly L1GuardianInstance outerInstance;

            public RestMonitor(L1GuardianInstance outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public override void run()
            {
                outerInstance.Rest = false;
            }
        }
    }

}