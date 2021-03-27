using LineageServer.Server.Model.skill;
using System;
using System.Collections.Generic;
using System.Threading;

/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Model.Instance
{
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static l1j.server.server.model.skill.L1SkillId.EFFECT_BLOODSTAIN_OF_ANTHARAS;
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static l1j.server.server.model.skill.L1SkillId.FOG_OF_SLEEPING;


    using Config = LineageServer.Server.Config;
    using ActionCodes = LineageServer.Server.ActionCodes;
    using RunnableExecuter = LineageServer.Server.RunnableExecuter;
    using IdFactory = LineageServer.Server.IdFactory;
    using DropTable = LineageServer.Server.DataSources.DropTable;
    using NPCTalkDataTable = LineageServer.Server.DataSources.NPCTalkDataTable;
    using NpcTable = LineageServer.Server.DataSources.NpcTable;
    using SprTable = LineageServer.Server.DataSources.SprTable;
    using UBTable = LineageServer.Server.DataSources.UBTable;
    using L1Attack = LineageServer.Server.Model.L1Attack;
    using L1Character = LineageServer.Server.Model.L1Character;
    using L1DragonSlayer = LineageServer.Server.Model.L1DragonSlayer;
    using L1Location = LineageServer.Server.Model.L1Location;
    using L1NpcTalkData = LineageServer.Server.Model.L1NpcTalkData;
    using GameObject = LineageServer.Server.Model.GameObject;
    using L1UltimateBattle = LineageServer.Server.Model.L1UltimateBattle;
    using L1World = LineageServer.Server.Model.L1World;
    using L1BuffUtil = LineageServer.Server.Model.skill.L1BuffUtil;
    using S_ChangeName = LineageServer.Serverpackets.S_ChangeName;
    using S_CharVisualUpdate = LineageServer.Serverpackets.S_CharVisualUpdate;
    using S_DoActionGFX = LineageServer.Serverpackets.S_DoActionGFX;
    using S_NPCPack = LineageServer.Serverpackets.S_NPCPack;
    using S_NPCTalkReturn = LineageServer.Serverpackets.S_NPCTalkReturn;
    using S_NpcChangeShape = LineageServer.Serverpackets.S_NpcChangeShape;
    using S_ServerMessage = LineageServer.Serverpackets.S_ServerMessage;
    using S_SkillBrave = LineageServer.Serverpackets.S_SkillBrave;
    using L1Npc = LineageServer.Server.Templates.L1Npc;
    using CalcExp = LineageServer.Utils.CalcExp;
    using Random = LineageServer.Utils.Random;
    using S_HPMeter = LineageServer.Serverpackets.S_HPMeter;

    [Serializable]
    public class L1MonsterInstance : L1NpcInstance
    {

        /// 
        private const long serialVersionUID = 1L;

        //JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
        private static Logger _log = Logger.GetLogger(typeof(L1MonsterInstance).FullName);

        private bool _storeDroped; // ドロップアイテムの読込が完了したか

        private bool isDoppel;

        // アイテム使用処理
        public override void onItemUse()
        {
            if (!Actived && (_target != null))
            {
                useItem(USEITEM_HASTE, 40); // ４０％使用加速藥水
                                            // 變形判斷
                onDoppel(true);
            }
            if (CurrentHp * 100 / MaxHp < 40)
            { // ＨＰが４０％きったら
                useItem(USEITEM_HEAL, 50); // ５０％の確率で回復ポーション使用
            }
        }

        // 變形怪變成玩家判斷
        public override void onDoppel(bool isChangeShape)
        {
            if (NpcTemplate.is_doppel())
            {
                bool updateObject = false;

                if (!isChangeShape)
                { // 復原
                    updateObject = true;
                    // setName(getNpcTemplate().get_name());
                    // setNameId(getNpcTemplate().get_nameid());
                    TempLawful = NpcTemplate.get_lawful();
                    GfxId = NpcTemplate.get_gfxid();
                    TempCharGfx = NpcTemplate.get_gfxid();
                }
                else if (!isDoppel && (_target is L1PcInstance))
                { // 未變形
                    SleepTime = 300;
                    L1PcInstance targetPc = (L1PcInstance)_target;
                    isDoppel = true;
                    updateObject = true;
                    Name = targetPc.Name;
                    NameId = targetPc.Name;
                    TempLawful = targetPc.Lawful;
                    GfxId = targetPc.ClassId;
                    TempCharGfx = targetPc.ClassId;

                    if (targetPc.ClassId != 6671)
                    { // 非幻術師拿劍
                        Status = 4;
                    }
                    else
                    { // 幻術師拿斧頭
                        Status = 11;
                    }
                }
                // 移動、攻擊速度
                Passispeed = SprTable.Instance.getMoveSpeed(TempCharGfx, Status);
                Atkspeed = SprTable.Instance.getAttackSpeed(TempCharGfx, Status + 1);
                // 變形
                if (updateObject)
                {
                    foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(this))
                    {
                        if (!isChangeShape)
                        {
                            pc.sendPackets(new S_ChangeName(Id, NpcTemplate.get_nameid()));
                        }
                        else
                        {
                            pc.sendPackets(new S_ChangeName(Id, NameId));
                        }
                        pc.sendPackets(new S_NpcChangeShape(Id, GfxId, TempLawful, Status));
                    }
                }
            }
        }

        public override void onPerceive(L1PcInstance perceivedFrom)
        {
            perceivedFrom.addKnownObject(this);
            if (0 < CurrentHp)
            {
                perceivedFrom.sendPackets(new S_NPCPack(this));
                onNpcAI(); // モンスターのＡＩを開始
                if (BraveSpeed == 1)
                { // 二段加速狀態
                    perceivedFrom.sendPackets(new S_SkillBrave(Id, 1, 600000));
                    BraveSpeed = 1;
                }
            }
            else
            {
                // 水龍 階段一、二 死亡隱形
                if (GfxId != 7864 && GfxId != 7869)
                {
                    perceivedFrom.sendPackets(new S_NPCPack(this));
                }
            }
        }

        // ターゲットを探す
        public static int[][] _classGfxId = new int[][]
        {
            new int[] {0, 1},
            new int[] {48, 61},
            new int[] {37, 138},
            new int[] {734, 1186},
            new int[] {2786, 2796}
        };

        public override void searchTarget()
        {
            // 目標捜索
            L1PcInstance lastTarget = null;
            L1PcInstance targetPlayer = null;

            if (_target != null && _target is L1PcInstance)
            {
                lastTarget = (L1PcInstance)_target;
                tagertClear();
            }

            foreach (L1PcInstance pc in L1World.Instance.getVisiblePlayer(this))
            {

                if (pc == lastTarget || (pc.CurrentHp <= 0) || pc.Dead || pc.Gm || pc.Monitor || pc.Ghost)
                {
                    continue;
                }

                // 闘技場内は変身／未変身に限らず全てアクティブ
                int mapId = MapId;
                if ((mapId == 88) || (mapId == 98) || (mapId == 92) || (mapId == 91) || (mapId == 95))
                {
                    if (!pc.Invisble || NpcTemplate.is_agrocoi())
                    { // インビジチェック
                        targetPlayer = pc;
                        break;
                    }
                }

                if (NpcId == 45600)
                { // カーツ
                    if (pc.Crown || pc.Darkelf || (pc.TempCharGfx != pc.ClassId))
                    { // 未変身の君主、DEにはアクティブ
                        targetPlayer = pc;
                        break;
                    }
                }

                // どちらかの条件を満たす場合、友好と見なされ先制攻撃されない。
                // ・モンスターのカルマがマイナス値（バルログ側モンスター）でPCのカルマレベルが1以上（バルログ友好）
                // ・モンスターのカルマがプラス値（ヤヒ側モンスター）でPCのカルマレベルが-1以下（ヤヒ友好）
                if (((NpcTemplate.Karma < 0) && (pc.KarmaLevel >= 1)) || ((NpcTemplate.Karma > 0) && (pc.KarmaLevel <= -1)))
                {
                    continue;
                }
                // 見棄てられた者たちの地 カルマクエストの変身中は、各陣営のモンスターから先制攻撃されない
                if (((pc.TempCharGfx == 6034) && (NpcTemplate.Karma < 0)) || ((pc.TempCharGfx == 6035) && (NpcTemplate.Karma > 0)) || ((pc.TempCharGfx == 6035) && (NpcTemplate.get_npcId() == 46070)) || ((pc.TempCharGfx == 6035) && (NpcTemplate.get_npcId() == 46072)))
                {
                    continue;
                }

                if (!NpcTemplate.is_agro() && !NpcTemplate.is_agrososc() && (NpcTemplate.is_agrogfxid1() < 0) && (NpcTemplate.is_agrogfxid2() < 0))
                { // 完全なノンアクティブモンスター
                    if (pc.Lawful < -1000)
                    { // プレイヤーがカオティック
                        targetPlayer = pc;
                        break;
                    }
                    continue;
                }

                if (!pc.Invisble || NpcTemplate.is_agrocoi())
                { // インビジチェック
                    if (pc.hasSkillEffect(L1SkillId.SHAPE_CHANGE))
                    { // 変身してる
                        if (NpcTemplate.is_agrososc())
                        { // 変身に対してアクティブ
                            targetPlayer = pc;
                            break;
                        }
                    }
                    else if (NpcTemplate.is_agro())
                    { // アクティブモンスター
                        targetPlayer = pc;
                        break;
                    }

                    // 特定のクラスorグラフィックＩＤにアクティブ
                    if ((NpcTemplate.is_agrogfxid1() >= 0) && (NpcTemplate.is_agrogfxid1() <= 4))
                    { // クラス指定
                        if ((_classGfxId[NpcTemplate.is_agrogfxid1()][0] == pc.TempCharGfx) || (_classGfxId[NpcTemplate.is_agrogfxid1()][1] == pc.TempCharGfx))
                        {
                            targetPlayer = pc;
                            break;
                        }
                    }
                    else if (pc.TempCharGfx == NpcTemplate.is_agrogfxid1())
                    { // グラフィックＩＤ指定
                        targetPlayer = pc;
                        break;
                    }

                    if ((NpcTemplate.is_agrogfxid2() >= 0) && (NpcTemplate.is_agrogfxid2() <= 4))
                    { // クラス指定
                        if ((_classGfxId[NpcTemplate.is_agrogfxid2()][0] == pc.TempCharGfx) || (_classGfxId[NpcTemplate.is_agrogfxid2()][1] == pc.TempCharGfx))
                        {
                            targetPlayer = pc;
                            break;
                        }
                    }
                    else if (pc.TempCharGfx == NpcTemplate.is_agrogfxid2())
                    { // グラフィックＩＤ指定
                        targetPlayer = pc;
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

        public L1MonsterInstance(L1Npc template) : base(template)
        {
            _storeDroped = false;
        }

        public override void onNpcAI()
        {
            if (AiRunning)
            {
                return;
            }
            if (!_storeDroped) // 無駄なオブジェクトＩＤを発行しないようにここでセット
            {
                DropTable.Instance.setDrop(this, Inventory);
                Inventory.shuffle();
                _storeDroped = true;
            }
            Actived = false;
            startAI();
        }

        public override void onTalkAction(L1PcInstance pc)
        {
            int objid = Id;
            L1NpcTalkData talking = NPCTalkDataTable.Instance.getTemplate(NpcTemplate.get_npcId());

            // html表示パケット送信
            if (pc.Lawful < -1000)
            { // プレイヤーがカオティック
                pc.sendPackets(new S_NPCTalkReturn(talking, objid, 2));
            }
            else
            {
                pc.sendPackets(new S_NPCTalkReturn(talking, objid, 1));
            }
        }

        public override void onAction(L1PcInstance pc)
        {
            onAction(pc, 0);
        }

        public override void onAction(L1PcInstance pc, int skillId)
        {
            if ((CurrentHp > 0) && !Dead)
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

        public override void ReceiveManaDamage(L1Character attacker, int mpDamage)
        { // 攻撃でＭＰを減らすときはここを使用
            if ((mpDamage > 0) && !Dead)
            {
                // int Hate = mpDamage / 10 + 10; // 注意！計算適当 ダメージの１０分の１＋ヒットヘイト１０
                // setHate(attacker, Hate);
                setHate(attacker, mpDamage);

                onNpcAI();

                if (attacker is L1PcInstance)
                { // 仲間意識をもつモンスターのターゲットに設定
                    serchLink((L1PcInstance)attacker, NpcTemplate.get_family());
                }

                int newMp = CurrentMp - mpDamage;
                if (newMp < 0)
                {
                    newMp = 0;
                }
                CurrentMp = newMp;
            }
        }

        public override void receiveDamage(L1Character attacker, int damage)
        { // 攻撃でＨＰを減らすときはここを使用
            if ((CurrentHp > 0) && !Dead)
            {
                if ((HiddenStatus == HIDDEN_STATUS_SINK) || (HiddenStatus == HIDDEN_STATUS_FLY))
                {
                    return;
                }
                if (damage >= 0)
                {
                    if (!(attacker is L1EffectInstance))
                    { // FWはヘイトなし
                        setHate(attacker, damage);
                    }
                }
                if (damage > 0)
                {
                    removeSkillEffect(FOG_OF_SLEEPING);
                }

                onNpcAI();

                if (attacker is L1PcInstance)
                { // 仲間意識をもつモンスターのターゲットに設定
                    serchLink((L1PcInstance)attacker, NpcTemplate.get_family());
                }
                // 怪物血條判斷功能 語法來源99NETS网游模拟娱乐社区
                if (Config.Attack_Mob_HP_Bar)
                {
                    if (attacker is L1PcInstance)
                    {
                        L1PcInstance pc = (L1PcInstance)attacker;
                        pc.sendPackets(new S_HPMeter(this, true));
                        L1Character MobHPBar = pc.MobHPBar;
                        if (MobHPBar != null && MobHPBar != this)
                        {
                            pc.sendPackets(new S_HPMeter(MobHPBar, false));
                        }
                        pc.MobHPBar = this;
                    }
                }
                // 怪物血條判斷功能 end
                // 血痕相剋傷害增加 1.5倍
                if ((NpcTemplate.get_npcId() == 97044 || NpcTemplate.get_npcId() == 97045 || NpcTemplate.get_npcId() == 97046) && (attacker.hasSkillEffect(EFFECT_BLOODSTAIN_OF_ANTHARAS)))
                { // 有安塔瑞斯的血痕時對法利昂增傷
                    damage *= 1.5;
                }

                if ((attacker is L1PcInstance) && (damage > 0))
                {
                    L1PcInstance player = (L1PcInstance)attacker;
                    player.PetTarget = this;
                }

                int newHp = CurrentHp - damage;
                if ((newHp <= 0) && !Dead)
                {
                    int transformId = NpcTemplate.TransformId;
                    // 変身しないモンスター
                    if (transformId == -1)
                    {
                        if (PortalNumber != -1)
                        {
                            if (NpcTemplate.get_npcId() == 97006 || NpcTemplate.get_npcId() == 97044)
                            {
                                // 準備階段二
                                L1DragonSlayer.Instance.startDragonSlayer2rd(PortalNumber);
                            }
                            else if (NpcTemplate.get_npcId() == 97007 || NpcTemplate.get_npcId() == 97045)
                            {
                                // 準備階段三
                                L1DragonSlayer.Instance.startDragonSlayer3rd(PortalNumber);
                            }
                            else if (NpcTemplate.get_npcId() == 97008 || NpcTemplate.get_npcId() == 97046)
                            {
                                bloodstain();
                                // 結束屠龍副本
                                L1DragonSlayer.Instance.endDragonSlayer(PortalNumber);
                            }
                        }
                        CurrentHpDirect = 0;
                        Dead = true;
                        Status = ActionCodes.ACTION_Die;
                        openDoorWhenNpcDied(this);
                        Death death = new Death(this, attacker);
                        RunnableExecuter.Instance.execute(death);
                        // Death(attacker);
                        if (PortalNumber == -1 && (NpcTemplate.get_npcId() == 97006 || NpcTemplate.get_npcId() == 97007 || NpcTemplate.get_npcId() == 97044 || NpcTemplate.get_npcId() == 97045))
                        {
                            doNextDragonStep(attacker, NpcTemplate.get_npcId());
                        }
                    }
                    else
                    { // 変身するモンスター
                      // distributeExpDropKarma(attacker);
                        transform(transformId);
                    }
                }
                if (newHp > 0)
                {
                    CurrentHp = newHp;
                    hide();
                }
            }
            else if (!Dead)
            { // 念のため
                Dead = true;
                Status = ActionCodes.ACTION_Die;
                Death death = new Death(this, attacker);
                RunnableExecuter.Instance.execute(death);
                // Death(attacker);
                if (PortalNumber == -1 && (NpcTemplate.get_npcId() == 97006 || NpcTemplate.get_npcId() == 97007 || NpcTemplate.get_npcId() == 97044 || NpcTemplate.get_npcId() == 97045))
                {
                    doNextDragonStep(attacker, NpcTemplate.get_npcId());
                }
            }
        }

        private static void openDoorWhenNpcDied(L1NpcInstance npc)
        {
            int[] npcId = new int[] { 46143, 46144, 46145, 46146, 46147, 46148, 46149, 46150, 46151, 46152 };
            int[] doorId = new int[] { 5001, 5002, 5003, 5004, 5005, 5006, 5007, 5008, 5009, 5010 };

            for (int i = 0; i < npcId.Length; i++)
            {
                if (npc.NpcTemplate.get_npcId() == npcId[i])
                {
                    openDoorInCrystalCave(doorId[i]);
                    break;
                }
            }
        }

        private static void openDoorInCrystalCave(int doorId)
        {
            foreach (GameObject @object in L1World.Instance.Object)
            {
                if (@object is L1DoorInstance)
                {
                    L1DoorInstance door = (L1DoorInstance)@object;
                    if (door.DoorId == doorId)
                    {
                        door.open();
                    }
                }
            }
        }

        /// <summary>
        /// 距離が5以上離れているpcを距離3～4の位置に引き寄せる。
        /// </summary>
        /// <param name="pc"> </param>
        /*
		 * private void recall(L1PcInstance pc) { if (getMapId() != pc.getMapId()) {
		 * return; } if (getLocation().getTileLineDistance(pc.getLocation()) > 4) {
		 * for (int count = 0; count < 10; count++) { L1Location newLoc =
		 * getLocation().randomLocation(3, 4, false); if (glanceCheck(newLoc.getX(),
		 * newLoc.getY())) { L1Teleport.teleport(pc, newLoc.getX(), newLoc.getY(),
		 * getMapId(), 5, true); break; } } } }
		 */

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

        internal class Death : IRunnableStart
        {
            private readonly L1MonsterInstance outerInstance;

            internal L1Character _lastAttacker;

            public Death(L1MonsterInstance outerInstance, L1Character lastAttacker)
            {
                this.outerInstance = outerInstance;
                _lastAttacker = lastAttacker;
            }

            public override void run()
            {
                outerInstance.DeathProcessing = true;
                outerInstance.CurrentHpDirect = 0;
                outerInstance.Dead = true;
                outerInstance.Status = ActionCodes.ACTION_Die;

                outerInstance.Map.setPassable(outerInstance.Location, true);

                outerInstance.broadcastPacket(new S_DoActionGFX(outerInstance.Id, ActionCodes.ACTION_Die));
                // 變形判斷
                outerInstance.onDoppel(false);

                outerInstance.startChat(CHAT_TIMING_DEAD);

                outerInstance.distributeExpDropKarma(_lastAttacker);
                outerInstance.giveUbSeal();

                outerInstance.DeathProcessing = false;

                outerInstance.Exp = 0;
                outerInstance.Karma = 0;
                outerInstance.allTargetClear();

                outerInstance.startDeleteTimer();
            }
        }

        private void distributeExpDropKarma(L1Character lastAttacker)
        {
            if (lastAttacker == null)
            {
                return;
            }
            L1PcInstance pc = null;
            if (lastAttacker is L1PcInstance)
            {
                pc = (L1PcInstance)lastAttacker;
            }
            else if (lastAttacker is L1PetInstance)
            {
                pc = (L1PcInstance)((L1PetInstance)lastAttacker).Master;
            }
            else if (lastAttacker is L1SummonInstance)
            {
                pc = (L1PcInstance)((L1SummonInstance)lastAttacker).Master;
            }

            if (pc != null)
            {
                List<L1Character> targetList = _hateList.toTargetArrayList();
                List<int> hateList = _hateList.toHateArrayList();
                long exp = Exp;
                CalcExp.calcExp(pc, Id, targetList, hateList, exp);
                // 死亡した場合はドロップとカルマも分配、死亡せず変身した場合はEXPのみ
                if (Dead)
                {
                    distributeDrop();
                    giveKarma(pc);
                }
            }
            else if (lastAttacker is L1EffectInstance)
            { // FWが倒した場合
                List<L1Character> targetList = _hateList.toTargetArrayList();
                List<int> hateList = _hateList.toHateArrayList();
                // ヘイトリストにキャラクターが存在する
                if (hateList.Count > 0)
                {
                    // 最大ヘイトを持つキャラクターが倒したものとする
                    int maxHate = 0;
                    for (int i = hateList.Count - 1; i >= 0; i--)
                    {
                        if (maxHate < (hateList[i]))
                        {
                            maxHate = (hateList[i]);
                            lastAttacker = targetList[i];
                        }
                    }
                    if (lastAttacker is L1PcInstance)
                    {
                        pc = (L1PcInstance)lastAttacker;
                    }
                    else if (lastAttacker is L1PetInstance)
                    {
                        pc = (L1PcInstance)((L1PetInstance)lastAttacker).Master;
                    }
                    else if (lastAttacker is L1SummonInstance)
                    {
                        pc = (L1PcInstance)((L1SummonInstance)lastAttacker).Master;
                    }
                    if (pc != null)
                    {
                        long exp = Exp;
                        CalcExp.calcExp(pc, Id, targetList, hateList, exp);
                        // 死亡した場合はドロップとカルマも分配、死亡せず変身した場合はEXPのみ
                        if (Dead)
                        {
                            distributeDrop();
                            giveKarma(pc);
                        }
                    }
                }
            }
        }

        private void distributeDrop()
        {
            List<L1Character> dropTargetList = _dropHateList.toTargetArrayList();
            List<int> dropHateList = _dropHateList.toHateArrayList();
            try
            {
                int npcId = NpcTemplate.get_npcId();
                if ((npcId != 45640) || ((npcId == 45640) && (TempCharGfx == 2332)))
                {
                    DropTable.Instance.dropShare(L1MonsterInstance.this, dropTargetList, dropHateList);
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
        }

        private void giveKarma(L1PcInstance pc)
        {
            int karma = Karma;
            if (karma != 0)
            {
                int karmaSign = Integer.signum(karma);
                int pcKarmaLevel = pc.KarmaLevel;
                int pcKarmaLevelSign = Integer.signum(pcKarmaLevel);
                // カルマ背信行為は5倍
                if ((pcKarmaLevelSign != 0) && (karmaSign != pcKarmaLevelSign))
                {
                    karma *= 5;
                }
                // カルマは止めを刺したプレイヤーに設定。ペットorサモンで倒した場合も入る。
                pc.addKarma((int)(karma * Config.RATE_KARMA));
            }
        }

        private void giveUbSeal()
        {
            if (UbSealCount != 0)
            { // UBの勇者の証
                L1UltimateBattle ub = UBTable.Instance.getUb(UbId);
                if (ub != null)
                {
                    foreach (L1PcInstance pc in ub.MembersArray)
                    {
                        if ((pc != null) && !pc.Dead && !pc.Ghost)
                        {
                            L1ItemInstance item = pc.Inventory.storeItem(41402, UbSealCount);
                            pc.sendPackets(new S_ServerMessage(403, item.LogName)); // %0を手に入れました。
                        }
                    }
                }
            }
        }

        public virtual bool is_storeDroped()
        {
            return _storeDroped;
        }

        public virtual void set_storeDroped(bool flag)
        {
            _storeDroped = flag;
        }

        private int _ubSealCount = 0; // UBで倒された時、参加者に与えられる勇者の証の個数

        public virtual int UbSealCount
        {
            get
            {
                return _ubSealCount;
            }
            set
            {
                _ubSealCount = value;
            }
        }


        private int _ubId = 0; // UBID

        public virtual int UbId
        {
            get
            {
                return _ubId;
            }
            set
            {
                _ubId = value;
            }
        }


        private void hide()
        {
            int npcid = NpcTemplate.get_npcId();
            if ((npcid == 45061) || (npcid == 45161) || (npcid == 45181) || (npcid == 45455))
            { // デッドリースパルトイ
                if (MaxHp / 3 > CurrentHp)
                {
                    int rnd = RandomHelper.Next(10);
                    if (2 > rnd)
                    {
                        allTargetClear();
                        HiddenStatus = HIDDEN_STATUS_SINK;
                        broadcastPacket(new S_DoActionGFX(Id, ActionCodes.ACTION_Hide));
                        Status = 11;
                        broadcastPacket(new S_CharVisualUpdate(this, Status));
                    }
                }
            }
            else if (npcid == 45682)
            { // アンタラス
                if (MaxHp / 3 > CurrentHp)
                {
                    int rnd = RandomHelper.Next(50);
                    if (1 > rnd)
                    {
                        allTargetClear();
                        HiddenStatus = HIDDEN_STATUS_SINK;
                        broadcastPacket(new S_DoActionGFX(Id, ActionCodes.ACTION_AntharasHide));
                        Status = 20;
                        broadcastPacket(new S_CharVisualUpdate(this, Status));
                    }
                }
            }
            else if ((npcid == 45067) || (npcid == 45264) || (npcid == 45452) || (npcid == 45090) || (npcid == 45321) || (npcid == 45445))
            { // グリフォン
                if (MaxHp / 3 > CurrentHp)
                {
                    int rnd = RandomHelper.Next(10);
                    if (2 > rnd)
                    {
                        allTargetClear();
                        HiddenStatus = HIDDEN_STATUS_FLY;
                        broadcastPacket(new S_DoActionGFX(Id, ActionCodes.ACTION_Moveup));
                    }
                }
            }
            else if (npcid == 45681)
            { // リンドビオル
                if (MaxHp / 3 > CurrentHp)
                {
                    int rnd = RandomHelper.Next(50);
                    if (1 > rnd)
                    {
                        allTargetClear();
                        HiddenStatus = HIDDEN_STATUS_FLY;
                        broadcastPacket(new S_DoActionGFX(Id, ActionCodes.ACTION_Moveup));
                    }
                }
            }
            else if ((npcid == 46107) || (npcid == 46108))
            { // テーベ マンドラゴラ(黒)
                if (MaxHp / 4 > CurrentHp)
                {
                    int rnd = RandomHelper.Next(10);
                    if (2 > rnd)
                    {
                        allTargetClear();
                        HiddenStatus = HIDDEN_STATUS_SINK;
                        broadcastPacket(new S_DoActionGFX(Id, ActionCodes.ACTION_Hide));
                        Status = 11;
                        broadcastPacket(new S_CharVisualUpdate(this, Status));
                    }
                }
            }
        }

        public virtual void initHide()
        {
            // 出現直後の隠れる動作
            // 潜るMOBは一定の確率で地中に潜った状態に、
            // 飛ぶMOBは飛んだ状態にしておく
            int npcid = NpcTemplate.get_npcId();
            if ((npcid == 45061) || (npcid == 45161) || (npcid == 45181) || (npcid == 45455))
            { // デッドリースパルトイ
                int rnd = RandomHelper.Next(3);
                if (1 > rnd)
                {
                    HiddenStatus = HIDDEN_STATUS_SINK;
                    Status = 11;
                }
            }
            else if ((npcid == 45045) || (npcid == 45126) || (npcid == 45134) || (npcid == 45281))
            { // ギランストーンゴーレム
                int rnd = RandomHelper.Next(3);
                if (1 > rnd)
                {
                    HiddenStatus = HIDDEN_STATUS_SINK;
                    Status = 4;
                }
            }
            else if ((npcid == 45067) || (npcid == 45264) || (npcid == 45452) || (npcid == 45090) || (npcid == 45321) || (npcid == 45445))
            { // グリフォン
                HiddenStatus = HIDDEN_STATUS_FLY;
            }
            else if (npcid == 45681)
            { // リンドビオル
                HiddenStatus = HIDDEN_STATUS_FLY;
            }
            else if ((npcid == 46107) || (npcid == 46108))
            { // テーベ マンドラゴラ(黒)
                int rnd = RandomHelper.Next(3);
                if (1 > rnd)
                {
                    HiddenStatus = HIDDEN_STATUS_SINK;
                    Status = 11;
                }
            }
            else if ((npcid >= 46125) && (npcid <= 46128))
            {
                HiddenStatus = L1NpcInstance.HIDDEN_STATUS_ICE;
                Status = 4;
            }
        }

        public virtual void initHideForMinion(L1NpcInstance leader)
        {
            // グループに属するモンスターの出現直後の隠れる動作（リーダーと同じ動作にする）
            int npcid = NpcTemplate.get_npcId();
            if (leader.HiddenStatus == HIDDEN_STATUS_SINK)
            {
                if ((npcid == 45061) || (npcid == 45161) || (npcid == 45181) || (npcid == 45455))
                { // デッドリースパルトイ
                    HiddenStatus = HIDDEN_STATUS_SINK;
                    Status = 11;
                }
                else if ((npcid == 45045) || (npcid == 45126) || (npcid == 45134) || (npcid == 45281))
                { // ギランストーンゴーレム
                    HiddenStatus = HIDDEN_STATUS_SINK;
                    Status = 4;
                }
                else if ((npcid == 46107) || (npcid == 46108))
                { // テーベ マンドラゴラ(黒)
                    HiddenStatus = HIDDEN_STATUS_SINK;
                    Status = 11;
                }
            }
            else if (leader.HiddenStatus == HIDDEN_STATUS_FLY)
            {
                if ((npcid == 45067) || (npcid == 45264) || (npcid == 45452) || (npcid == 45090) || (npcid == 45321) || (npcid == 45445))
                { // グリフォン
                    HiddenStatus = HIDDEN_STATUS_FLY;
                    Status = 4;
                }
                else if (npcid == 45681)
                { // リンドビオル
                    HiddenStatus = HIDDEN_STATUS_FLY;
                    Status = 11;
                }
            }
            else if ((npcid >= 46125) && (npcid <= 46128))
            {
                HiddenStatus = L1NpcInstance.HIDDEN_STATUS_ICE;
                Status = 4;
            }
        }

        protected internal override void transform(int transformId)
        {
            base.transform(transformId);

            // DROPの再設定
            Inventory.clearItems();
            DropTable.Instance.setDrop(this, Inventory);
            Inventory.shuffle();
        }

        private bool _nextDragonStepRunning = false;

        protected internal virtual bool NextDragonStepRunning
        {
            set
            {
                _nextDragonStepRunning = value;
            }
            get
            {
                return _nextDragonStepRunning;
            }
        }

        public int Level { get; internal set; }


        // 龍之血痕
        private void bloodstain()
        {
            foreach (L1PcInstance pc in L1World.Instance.getVisiblePlayer(this, 50))
            {
                if (NpcTemplate.get_npcId() == 97008)
                {
                    pc.sendPackets(new S_ServerMessage(1580)); // 安塔瑞斯：黑暗的詛咒將會降臨到你們身上！席琳，
                                                               // 我的母親，請讓我安息吧...
                    L1BuffUtil.bloodstain(pc, (sbyte)0, 4320, true);
                }
                else if (NpcTemplate.get_npcId() == 97046)
                {
                    pc.sendPackets(new S_ServerMessage(1668)); // 法利昂：莎爾...你這個傢伙...怎麼...對得起我的母親...席琳啊...請拿走...我的生命吧...
                    L1BuffUtil.bloodstain(pc, (sbyte)1, 4320, true);
                }
            }
        }

        private void doNextDragonStep(L1Character attacker, int npcid)
        {
            if (!NextDragonStepRunning)
            {
                int[] dragonId = new int[] { 97006, 97007, 97044, 97045 };
                int[] nextStepId = new int[] { 97007, 97008, 97045, 97046 };
                int nextSpawnId = 0;
                for (int i = 0; i < dragonId.Length; i++)
                {
                    if (npcid == dragonId[i])
                    {
                        nextSpawnId = nextStepId[i];
                        break;
                    }
                }
                if (attacker != null && nextSpawnId > 0)
                {
                    L1PcInstance _pc = null;
                    if (attacker is L1PcInstance)
                    {
                        _pc = (L1PcInstance)attacker;
                    }
                    else if (attacker is L1PetInstance)
                    {
                        L1PetInstance pet = (L1PetInstance)attacker;
                        L1Character cha = pet.Master;
                        if (cha is L1PcInstance)
                        {
                            _pc = (L1PcInstance)cha;
                        }
                    }
                    else if (attacker is L1SummonInstance)
                    {
                        L1SummonInstance summon = (L1SummonInstance)attacker;
                        L1Character cha = summon.Master;
                        if (cha is L1PcInstance)
                        {
                            _pc = (L1PcInstance)cha;
                        }
                    }
                    if (_pc != null)
                    {
                        NextDragonStep nextDragonStep = new NextDragonStep(this, _pc, this, nextSpawnId);
                        RunnableExecuter.Instance.execute(nextDragonStep);
                    }
                }
            }
        }

        internal class NextDragonStep : IRunnableStart
        {
            private readonly L1MonsterInstance outerInstance;

            internal L1PcInstance _pc;
            internal L1MonsterInstance _mob;
            internal int _npcid;
            internal int _transformId;
            internal int _x;
            internal int _y;
            internal int _h;
            internal short _m;
            internal L1Location _loc = new L1Location();

            public NextDragonStep(L1MonsterInstance outerInstance, L1PcInstance pc, L1MonsterInstance mob, int transformId)
            {
                this.outerInstance = outerInstance;
                _pc = pc;
                _mob = mob;
                _transformId = transformId;
                _x = mob.X;
                _y = mob.Y;
                _h = mob.Heading;
                _m = mob.MapId;
                _loc = mob.Location;
            }

            public override void run()
            {
                outerInstance.NextDragonStepRunning = true;
                try
                {
                    Thread.Sleep(10500);
                    L1NpcInstance npc = NpcTable.Instance.newNpcInstance(_transformId);
                    npc.Id = IdFactory.Instance.nextId();
                    npc.Map = (short)_m;
                    npc.HomeX = _x;
                    npc.HomeY = _y;
                    npc.Heading = _h;
                    npc.Location.set(_loc);
                    npc.Location.forward(_h);
                    npc.PortalNumber = outerInstance.PortalNumber;

                    outerInstance.broadcastPacket(new S_NPCPack(npc));
                    outerInstance.broadcastPacket(new S_DoActionGFX(npc.Id, ActionCodes.ACTION_Hide));

                    L1World.Instance.storeObject(npc);
                    L1World.Instance.addVisibleObject(npc);
                    npc.turnOnOffLight();
                    npc.startChat(L1NpcInstance.CHAT_TIMING_APPEARANCE); // チャット開始
                    outerInstance.NextDragonStepRunning = false;
                }
                catch (InterruptedException)
                {
                }
            }
        }
    }

}