using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.poison;
using LineageServer.Server.Model.skill;
using LineageServer.Server.Templates;
using LineageServer.Server.Types;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.Model
{
    class L1Character : GameObject
    {

        private const long serialVersionUID = 1L;

        private L1Poison _poison = null;

        private bool _paralyzed;

        private bool _sleeped;

        private readonly IDictionary<int, L1NpcInstance> _petlist = MapFactory.NewMap<int, L1NpcInstance>();

        private readonly IDictionary<int, L1DollInstance> _dolllist = MapFactory.NewMap<int, L1DollInstance>();

        private readonly IDictionary<int, IL1SkillTimer> _skillEffect = MapFactory.NewMap<int, IL1SkillTimer>();

        private readonly IDictionary<int, L1ItemDelay.ItemDelayTimer> _itemdelay = MapFactory.NewMap<int, L1ItemDelay.ItemDelayTimer>();

        private readonly IDictionary<int, L1FollowerInstance> _followerlist = MapFactory.NewMap<int, L1FollowerInstance>();

        public L1Character()
        {
            _level = 1;
        }

        /// <summary>
        /// キャラクターを復活させる。
        /// </summary>
        /// <param name="hp">
        ///            復活後のHP </param>
        public virtual void resurrect(int hp)
        {
            if (!Dead)
            {
                return;
            }
            if (hp <= 0)
            {
                hp = 1;
            }
            CurrentHp = hp;
            Dead = false;
            Status = 0;
            L1PolyMorph.undoPoly(this);
            foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(this))
            {
                pc.sendPackets(new S_RemoveObject(this));
                pc.removeKnownObject(this);
                pc.updateObject();
            }
        }

        private int _currentHp;

        /// <summary>
        /// キャラクターの現在のHPを返す。
        /// </summary>
        /// <returns> 現在のHP </returns>
        public virtual int CurrentHp
        {
            get
            {
                return _currentHp;
            }
            set
            {
                _currentHp = value;
                if (_currentHp >= getMaxHp())
                {
                    _currentHp = getMaxHp();
                }
            }
        }


        /// <summary>
        /// キャラクターのHPを設定する。
        /// </summary>
        /// <param name="i">
        ///            キャラクターの新しいHP </param>
        public virtual int CurrentHpDirect
        {
            set
            {
                _currentHp = value;
            }
        }

        private int _currentMp;

        /// <summary>
        /// キャラクターの現在のMPを返す。
        /// </summary>
        /// <returns> 現在のMP </returns>
        public virtual int CurrentMp
        {
            get
            {
                return _currentMp;
            }
            set
            {
                _currentMp = value;
                if (_currentMp >= getMaxMp())
                {
                    _currentMp = getMaxMp();
                }
            }
        }


        /// <summary>
        /// キャラクターのMPを設定する。
        /// </summary>
        /// <param name="i">
        ///            キャラクターの新しいMP </param>
        public virtual int CurrentMpDirect
        {
            set
            {
                _currentMp = value;
            }
        }

        /// <summary>
        /// キャラクターの眠り状態を返す。
        /// </summary>
        /// <returns> 眠り状態を表す値。眠り状態であればtrue。 </returns>
        public virtual bool Sleeped
        {
            get
            {
                return _sleeped;
            }
            set
            {
                _sleeped = value;
            }
        }


        /// <summary>
        /// キャラクターの麻痺状態を返す。
        /// </summary>
        /// <returns> 麻痺状態を表す値。麻痺状態であればtrue。 </returns>
        public virtual bool Paralyzed
        {
            get
            {
                return _paralyzed;
            }
            set
            {
                _paralyzed = value;
            }
        }


        internal L1Paralysis _paralysis;

        public virtual L1Paralysis Paralysis
        {
            get
            {
                return _paralysis;
            }
        }

        public virtual L1Paralysis Paralaysis
        {
            set
            {
                _paralysis = value;
            }
        }

        public virtual void cureParalaysis()
        {
            if (_paralysis != null)
            {
                _paralysis.cure();
            }
        }

        /// <summary>
        /// キャラクターの可視範囲に居るプレイヤーへ、パケットを送信する。
        /// </summary>
        /// <param name="packet">
        ///            送信するパケットを表すServerBasePacketオブジェクト。 </param>
        public virtual void broadcastPacket(ServerBasePacket packet)
        {
            foreach (L1PcInstance pc in L1World.Instance.getVisiblePlayer(this))
            {
                // 旅館內判斷
                if (pc.MapId < 16384 || pc.MapId > 25088 || pc.InnKeyId == InnKeyId)
                {
                    pc.sendPackets(packet);
                }
            }
        }

        /// <summary>
        /// キャラクターの可視範囲に居るプレイヤーへ、パケットを送信する。 ただしターゲットの画面内には送信しない。
        /// </summary>
        /// <param name="packet">
        ///            送信するパケットを表すServerBasePacketオブジェクト。 </param>
        public virtual void broadcastPacketExceptTargetSight(ServerBasePacket packet, L1Character target)
        {
            foreach (L1PcInstance pc in L1World.Instance.getVisiblePlayerExceptTargetSight(this, target))
            {
                pc.sendPackets(packet);
            }
        }

        /// <summary>
        /// キャラクターの可視範囲でインビジを見破れるor見破れないプレイヤーを区別して、パケットを送信する。
        /// </summary>
        /// <param name="packet">
        ///            送信するパケットを表すServerBasePacketオブジェクト。 </param>
        /// <param name="isFindInvis">
        ///            true : 見破れるプレイヤーにだけパケットを送信する。 false : 見破れないプレイヤーにだけパケットを送信する。 </param>
        public virtual void broadcastPacketForFindInvis(ServerBasePacket packet, bool isFindInvis)
        {
            foreach (L1PcInstance pc in L1World.Instance.getVisiblePlayer(this))
            {
                if (isFindInvis)
                {
                    if (pc.hasSkillEffect(L1SkillId.GMSTATUS_FINDINVIS))
                    {
                        pc.sendPackets(packet);
                    }
                }
                else
                {
                    if (!pc.hasSkillEffect(L1SkillId.GMSTATUS_FINDINVIS))
                    {
                        pc.sendPackets(packet);
                    }
                }
            }
        }

        /// <summary>
        /// キャラクターの50マス以内に居るプレイヤーへ、パケットを送信する。
        /// </summary>
        /// <param name="packet">
        ///            送信するパケットを表すServerBasePacketオブジェクト。 </param>
        public virtual void wideBroadcastPacket(ServerBasePacket packet)
        {
            foreach (L1PcInstance pc in L1World.Instance.getVisiblePlayer(this, 50))
            {
                pc.sendPackets(packet);
            }
        }

        /// <summary>
        /// キャラクターの正面の座標を返す。
        /// </summary>
        /// <returns> 正面の座標 </returns>
        public virtual int[] FrontLoc
        {
            get
            {
                int[] loc = new int[2];
                int x = X;
                int y = Y;
                int heading = Heading;
                if (heading == 0)
                {
                    y--;
                }
                else if (heading == 1)
                {
                    x++;
                    y--;
                }
                else if (heading == 2)
                {
                    x++;
                }
                else if (heading == 3)
                {
                    x++;
                    y++;
                }
                else if (heading == 4)
                {
                    y++;
                }
                else if (heading == 5)
                {
                    x--;
                    y++;
                }
                else if (heading == 6)
                {
                    x--;
                }
                else if (heading == 7)
                {
                    x--;
                    y--;
                }
                loc[0] = x;
                loc[1] = y;
                return loc;
            }
        }

        /// <summary>
        /// 指定された座標に対する方向を返す。
        /// </summary>
        /// <param name="tx">
        ///            座標のX値 </param>
        /// <param name="ty">
        ///            座標のY値 </param>
        /// <returns> 指定された座標に対する方向 </returns>
        public virtual int targetDirection(int tx, int ty)
        {
            float dis_x = Math.Abs(X - tx); // Ｘ方向のターゲットまでの距離
            float dis_y = Math.Abs(Y - ty); // Ｙ方向のターゲットまでの距離
            float dis = Math.Max(dis_x, dis_y); // ターゲットまでの距離
            if (dis == 0)
            {
                return Heading; // 同じ位置ならいま向いてる方向を返しとく
            }
            int avg_x = (int)Math.Floor((dis_x / dis) + 0.59f); // 上下左右がちょっと優先な丸め
            int avg_y = (int)Math.Floor((dis_y / dis) + 0.59f); // 上下左右がちょっと優先な丸め

            int dir_x = 0;
            int dir_y = 0;
            if (X < tx)
            {
                dir_x = 1;
            }
            if (X > tx)
            {
                dir_x = -1;
            }
            if (Y < ty)
            {
                dir_y = 1;
            }
            if (Y > ty)
            {
                dir_y = -1;
            }

            if (avg_x == 0)
            {
                dir_x = 0;
            }
            if (avg_y == 0)
            {
                dir_y = 0;
            }

            if ((dir_x == 1) && (dir_y == -1))
            {
                return 1; // 上
            }
            if ((dir_x == 1) && (dir_y == 0))
            {
                return 2; // 右上
            }
            if ((dir_x == 1) && (dir_y == 1))
            {
                return 3; // 右
            }
            if ((dir_x == 0) && (dir_y == 1))
            {
                return 4; // 右下
            }
            if ((dir_x == -1) && (dir_y == 1))
            {
                return 5; // 下
            }
            if ((dir_x == -1) && (dir_y == 0))
            {
                return 6; // 左下
            }
            if ((dir_x == -1) && (dir_y == -1))
            {
                return 7; // 左
            }
            if ((dir_x == 0) && (dir_y == -1))
            {
                return 0; // 左上
            }
            return Heading; // ここにはこない。はず
        }

        /// <summary>
        /// 指定された座標までの直線上に、障害物が存在*しないか*を返す。
        /// </summary>
        /// <param name="tx">
        ///            座標のX値 </param>
        /// <param name="ty">
        ///            座標のY値 </param>
        /// <returns> 障害物が無ければtrue、あればfalseを返す。 </returns>
        public virtual bool glanceCheck(int tx, int ty)
        {
            int chx = X;
            int chy = Y;
            for (int i = 0; i < 15; i++)
            {
                if (chx == tx && chy == ty)
                {
                    break;
                }

                if (!System.Collections.IDictionary.isArrowPassable(chx, chy, targetDirection(tx, ty)))
                {
                    return false;
                }

                // Targetへ1タイル進める
                chx += Math.Max(-1, Math.Min(1, tx - chx));
                chy += Math.Max(-1, Math.Min(1, ty - chy));
            }
            return true;
        }

        /// <summary>
        /// 指定された座標へ攻撃可能であるかを返す。
        /// </summary>
        /// <param name="x">
        ///            座標のX値。 </param>
        /// <param name="y">
        ///            座標のY値。 </param>
        /// <param name="range">
        ///            攻撃可能な範囲(タイル数) </param>
        /// <returns> 攻撃可能であればtrue,不可能であればfalse </returns>
        public virtual bool isAttackPosition(int x, int y, int range)
        {
            if (range >= 7) // 遠隔武器（７以上の場合斜めを考慮すると画面外に出る)
            {
                if (Location.getTileDistance(new Point(x, y)) > range)
                {
                    return false;
                }
            }
            else // 近接武器
            {
                if (Location.getTileLineDistance(new Point(x, y)) > range)
                {
                    return false;
                }
            }
            return glanceCheck(x, y);
        }

        /// <summary>
        /// キャラクターのインベントリを返す。
        /// </summary>
        /// <returns> キャラクターのインベントリを表す、L1Inventoryオブジェクト。 </returns>
        public virtual L1Inventory Inventory
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// キャラクターへ、新たにスキル効果を追加する。
        /// </summary>
        /// <param name="skillId">
        ///            追加する効果のスキルID。 </param>
        /// <param name="timeMillis">
        ///            追加する効果の持続時間。無限の場合は0。 </param>
        private void addSkillEffect(int skillId, int timeMillis)
        {
            IL1SkillTimer timer = null;
            if (0 < timeMillis)
            {
                timer = L1SkillTimerCreator.create(this, skillId, timeMillis);
                timer.begin();
            }
            _skillEffect[skillId] = timer;
        }

        /// <summary>
        /// キャラクターへ、スキル効果を設定する。<br>
        /// 重複するスキルがない場合は、新たにスキル効果を追加する。<br>
        /// 重複するスキルがある場合は、残り効果時間とパラメータの効果時間の長い方を優先して設定する。
        /// </summary>
        /// <param name="skillId">
        ///            設定する効果のスキルID。 </param>
        /// <param name="timeMillis">
        ///            設定する効果の持続時間。無限の場合は0。 </param>
        public virtual void setSkillEffect(int skillId, int timeMillis)
        {
            if (hasSkillEffect(skillId))
            {
                int remainingTimeMills = getSkillEffectTimeSec(skillId) * 1000;

                // 残り時間が有限で、パラメータの効果時間の方が長いか無限の場合は上書きする。
                if ((remainingTimeMills >= 0) && ((remainingTimeMills < timeMillis) || (timeMillis == 0)))
                {
                    killSkillEffectTimer(skillId);
                    addSkillEffect(skillId, timeMillis);
                }
            }
            else
            {
                addSkillEffect(skillId, timeMillis);
            }
        }

        /// <summary>
        /// キャラクターから、スキル効果を削除する。
        /// </summary>
        /// <param name="skillId">
        ///            削除する効果のスキルID </param>
        public virtual void removeSkillEffect(int skillId)
        {
            if (_skillEffect.Remove(skillId, out IL1SkillTimer timer))
            {
                timer.end();
            }
        }

        /// <summary>
        /// キャラクターから、スキル効果のタイマーを削除する。 スキル効果は削除されない。
        /// </summary>
        /// <param name="skillId">
        ///            削除するタイマーのスキルＩＤ </param>
        public virtual void killSkillEffectTimer(int skillId)
        {
            if (_skillEffect.Remove(skillId, out IL1SkillTimer timer))
            {
                timer.kill();
            }
        }

        /// <summary>
        /// キャラクターから、全てのスキル効果タイマーを削除する。スキル効果は削除されない。
        /// </summary>
        public virtual void clearSkillEffectTimer()
        {
            foreach (IL1SkillTimer timer in _skillEffect.Values)
            {
                if (timer != null)
                {
                    timer.kill();
                }
            }
            _skillEffect.Clear();
        }

        /// <summary>
        /// キャラクターに、スキル効果が掛かっているかを返す。
        /// </summary>
        /// <param name="skillId">
        ///            調べる効果のスキルID。 </param>
        /// <returns> 魔法効果があればtrue、なければfalse。 </returns>
        public virtual bool hasSkillEffect(int skillId)
        {
            return _skillEffect.ContainsKey(skillId);
        }

        /// <summary>
        /// キャラクターのスキル効果の持続時間を返す。
        /// </summary>
        /// <param name="skillId">
        ///            調べる効果のスキルID </param>
        /// <returns> スキル効果の残り時間(秒)。スキルがかかっていないか効果時間が無限の場合、-1。 </returns>
        public virtual int getSkillEffectTimeSec(int skillId)
        {
            IL1SkillTimer timer = _skillEffect[skillId];
            if (timer == null)
            {
                return -1;
            }
            return timer.RemainingTime;
        }

        private bool _isSkillDelay = false;

        /// <summary>
        /// キャラクターへ、スキルディレイを追加する。
        /// </summary>
        /// <param name="flag"> </param>
        public virtual bool SkillDelay
        {
            set
            {
                _isSkillDelay = value;
            }
            get
            {
                return _isSkillDelay;
            }
        }


        /// <summary>
        /// キャラクターへ、アイテムディレイを追加する。
        /// </summary>
        /// <param name="delayId">
        ///            アイテムディレイID。 通常のアイテムであれば0、インビジビリティ クローク、バルログ ブラッディ クロークであれば1。 </param>
        /// <param name="timer">
        ///            ディレイ時間を表す、L1ItemDelay.ItemDelayTimerオブジェクト。 </param>
        public virtual void addItemDelay(int delayId, L1ItemDelay.ItemDelayTimer timer)
        {
            _itemdelay[delayId] = timer;
        }

        /// <summary>
        /// キャラクターから、アイテムディレイを削除する。
        /// </summary>
        /// <param name="delayId">
        ///            アイテムディレイID。 通常のアイテムであれば0、インビジビリティ クローク、バルログ ブラッディ クロークであれば1。 </param>
        public virtual void removeItemDelay(int delayId)
        {
            _itemdelay.Remove(delayId);
        }

        /// <summary>
        /// キャラクターに、アイテムディレイがあるかを返す。
        /// </summary>
        /// <param name="delayId">
        ///            調べるアイテムディレイID。 通常のアイテムであれば0、インビジビリティ クローク、バルログ ブラッディ
        ///            クロークであれば1。 </param>
        /// <returns> アイテムディレイがあればtrue、なければfalse。 </returns>
        public virtual bool hasItemDelay(int delayId)
        {
            return _itemdelay.ContainsKey(delayId);
        }

        /// <summary>
        /// キャラクターのアイテムディレイ時間を表す、L1ItemDelay.ItemDelayTimerを返す。
        /// </summary>
        /// <param name="delayId">
        ///            調べるアイテムディレイID。 通常のアイテムであれば0、インビジビリティ クローク、バルログ ブラッディ
        ///            クロークであれば1。 </param>
        /// <returns> アイテムディレイ時間を表す、L1ItemDelay.ItemDelayTimer。 </returns>
        public virtual L1ItemDelay.ItemDelayTimer getItemDelayTimer(int delayId)
        {
            return _itemdelay[delayId];
        }

        /// <summary>
        /// キャラクターへ、新たにペット、サモンモンスター、テイミングモンスター、あるいはクリエイトゾンビを追加する。
        /// </summary>
        /// <param name="npc">
        ///            追加するNpcを表す、L1NpcInstanceオブジェクト。 </param>
        public virtual void addPet(L1NpcInstance npc)
        {
            _petlist[npc.Id] = npc;
            sendPetCtrlMenu(npc, true); // 顯示寵物控制圖形介面
        }

        /// <summary>
        /// キャラクターから、ペット、サモンモンスター、テイミングモンスター、あるいはクリエイトゾンビを削除する。
        /// </summary>
        /// <param name="npc">
        ///            削除するNpcを表す、L1NpcInstanceオブジェクト。 </param>
        public virtual void removePet(L1NpcInstance npc)
        {
            _petlist.Remove(npc.Id);
            sendPetCtrlMenu(npc, false); // 關閉寵物控制圖形介面
        }

        /// <summary>
        /// 3.3C PetMenu 控制
        /// </summary>
        /// <param name="npc"> </param>
        /// <param name="type">
        ///            1:顯示 0:關閉 </param>
        public virtual void sendPetCtrlMenu(L1NpcInstance npc, bool type)
        {
            if (npc is L1PetInstance)
            {
                L1PetInstance pet = (L1PetInstance)npc;
                L1Character cha = pet.Master;
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_PetCtrlMenu(cha, npc, type));
                    // 處理寵物控制圖形介面
                }
            }
            else if (npc is L1SummonInstance)
            {
                L1SummonInstance summon = (L1SummonInstance)npc;
                L1Character cha = summon.Master;
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_PetCtrlMenu(cha, npc, type));
                }
            }
        }

        /// <summary>
        /// キャラクターのペットリストを返す。
        /// </summary>
        /// <returns> 
        ///         キャラクターのペットリストを表す、HashMapオブジェクト。このオブジェクトのKeyはオブジェクトID、ValueはL1NpcInstance
        ///         。 </returns>
        public virtual IDictionary<int, L1NpcInstance> PetList
        {
            get
            {
                return _petlist;
            }
        }

        /// <summary>
        /// キャラクターへマジックドールを追加する。
        /// </summary>
        /// <param name="doll">
        ///            追加するdollを表す、L1DollInstanceオブジェクト。 </param>
        public virtual void addDoll(L1DollInstance doll)
        {
            _dolllist[doll.Id] = doll;
        }

        /// <summary>
        /// キャラクターからマジックドールを削除する。
        /// </summary>
        /// <param name="doll">
        ///            削除するdollを表す、L1DollInstanceオブジェクト。 </param>
        public virtual void removeDoll(L1DollInstance doll)
        {
            _dolllist.Remove(doll.Id);
        }

        /// <summary>
        /// キャラクターのマジックドールリストを返す。
        /// </summary>
        /// <returns> キャラクターの魔法人形リストを表す、HashMapオブジェクト。このオブジェクトのKeyはオブジェクトID、
        ///         ValueはL1DollInstance。 </returns>
        public virtual IDictionary<int, L1DollInstance> DollList
        {
            get
            {
                return _dolllist;
            }
        }

        /// <summary>
        /// キャラクターへ従者を追加する。
        /// </summary>
        /// <param name="follower">
        ///            追加するfollowerを表す、L1FollowerInstanceオブジェクト。 </param>
        public virtual void addFollower(L1FollowerInstance follower)
        {
            _followerlist[follower.Id] = follower;
        }

        /// <summary>
        /// キャラクターから従者を削除する。
        /// </summary>
        /// <param name="follower">
        ///            削除するfollowerを表す、L1FollowerInstanceオブジェクト。 </param>
        public virtual void removeFollower(L1FollowerInstance follower)
        {
            _followerlist.Remove(follower.Id);
        }

        /// <summary>
        /// キャラクターの従者リストを返す。
        /// </summary>
        /// <returns> キャラクターの従者リストを表す、HashMapオブジェクト。このオブジェクトのKeyはオブジェクトID、
        ///         ValueはL1FollowerInstance。 </returns>
        public virtual IDictionary<int, L1FollowerInstance> FollowerList
        {
            get
            {
                return _followerlist;
            }
        }

        /// <summary>
        /// キャラクターへ、毒を追加する。
        /// </summary>
        /// <param name="poison">
        ///            毒を表す、L1Poisonオブジェクト。 </param>
        public virtual L1Poison Poison
        {
            set
            {
                _poison = value;
            }
            get
            {
                return _poison;
            }
        }

        /// <summary>
        /// キャラクターの毒を治療する。
        /// </summary>
        public virtual void curePoison()
        {
            if (_poison == null)
            {
                return;
            }
            _poison.cure();
        }


        /// <summary>
        /// キャラクターへ毒のエフェクトを付加する
        /// </summary>
        /// <param name="effectId"> </param>
        /// <seealso cref= S_Poison#S_Poison(int, int) </seealso>
        public virtual int PoisonEffect
        {
            set
            {
                broadcastPacket(new S_Poison(Id, value));
            }
        }

        /// <summary>
        /// キャラクターが存在する座標が、どのゾーンに属しているかを返す。
        /// </summary>
        /// <returns> 座標のゾーンを表す値。セーフティーゾーンであれば1、コンバットゾーンであれば-1、ノーマルゾーンであれば0。 </returns>
        public virtual int ZoneType
        {
            get
            {
                if (System.Collections.IDictionary.isSafetyZone(Location))
                {
                    return 1;
                }
                else if (System.Collections.IDictionary.isCombatZone(Location))
                {
                    return -1;
                }
                else
                { // ノーマルゾーン
                    return 0;
                }
            }
        }

        private long _exp; // ● 経験値

        /// <summary>
        /// キャラクターが保持している経験値を返す。
        /// </summary>
        /// <returns> 経験値。 </returns>
        public virtual long Exp
        {
            get
            {
                return _exp;
            }
            set
            {
                _exp = value;
            }
        }


        // ■■■■■■■■■■ L1PcInstanceへ移動するプロパティ ■■■■■■■■■■
        private readonly IList<GameObject> _knownObjects = ListFactory.NewConcurrentList<GameObject>();

        private readonly IList<L1PcInstance> _knownPlayer = ListFactory.NewConcurrentList<L1PcInstance>();

        /// <summary>
        /// 指定されたオブジェクトを、キャラクターが認識しているかを返す。
        /// </summary>
        /// <param name="obj">
        ///            調べるオブジェクト。 </param>
        /// <returns> オブジェクトをキャラクターが認識していればtrue、していなければfalse。 自分自身に対してはfalseを返す。 </returns>
        public virtual bool knownsObject(GameObject obj)
        {
            return _knownObjects.Contains(obj);
        }

        /// <summary>
        /// キャラクターが認識している全てのオブジェクトを返す。
        /// </summary>
        /// <returns> キャラクターが認識しているオブジェクトを表すL1Objectが格納されたArrayList。 </returns>
        public virtual IList<GameObject> KnownObjects
        {
            get
            {
                return _knownObjects;
            }
        }

        /// <summary>
        /// キャラクターが認識している全てのプレイヤーを返す。
        /// </summary>
        /// <returns> キャラクターが認識しているオブジェクトを表すL1PcInstanceが格納されたArrayList。 </returns>
        public virtual IList<L1PcInstance> KnownPlayers
        {
            get
            {
                return _knownPlayer;
            }
        }

        /// <summary>
        /// キャラクターに、新たに認識するオブジェクトを追加する。
        /// </summary>
        /// <param name="obj">
        ///            新たに認識するオブジェクト。 </param>
        public virtual void addKnownObject(GameObject obj)
        {
            if (!_knownObjects.Contains(obj))
            {
                _knownObjects.Add(obj);
                if (obj is L1PcInstance)
                {
                    _knownPlayer.Add((L1PcInstance)obj);
                }
            }
        }

        /// <summary>
        /// キャラクターから、認識しているオブジェクトを削除する。
        /// </summary>
        /// <param name="obj">
        ///            削除するオブジェクト。 </param>
        public virtual void removeKnownObject(GameObject obj)
        {
            _knownObjects.Remove(obj);
            if (obj is L1PcInstance l1PcInstance)
            {
                _knownPlayer.Remove(l1PcInstance);
            }
        }

        /// <summary>
        /// キャラクターから、全ての認識しているオブジェクトを削除する。
        /// </summary>
        public virtual void removeAllKnownObjects()
        {
            _knownObjects.Clear();
            _knownPlayer.Clear();
        }

        // ■■■■■■■■■■ プロパティ ■■■■■■■■■■

        private string _name; // ● 名前

        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }


        private int _level; // ● レベル

        public virtual int getLevel()
        {
            lock (this)
            {
                return _level;
            }
        }

        public virtual void setLevel(long level)
        {
            lock (this)
            {
                _level = (int)level;
            }
        }

        private short _maxHp = 0; // ● ＭＡＸＨＰ（1～32767）

        private int _trueMaxHp = 0; // ● 本当のＭＡＸＨＰ

        public virtual short getMaxHp()
        {
            return _maxHp;
        }

        public virtual void setMaxHp(int hp)
        {
            _trueMaxHp = hp;
            _maxHp = (short)IntRange.ensure(_trueMaxHp, 1, 32767);
            _currentHp = Math.Min(_currentHp, _maxHp);
        }

        public virtual void addMaxHp(int i)
        {
            setMaxHp(_trueMaxHp + i);
        }

        private short _maxMp = 0; // ● ＭＡＸＭＰ（0～32767）

        private int _trueMaxMp = 0; // ● 本当のＭＡＸＭＰ

        public virtual short getMaxMp()
        {
            return _maxMp;
        }

        public virtual void setMaxMp(int mp)
        {
            _trueMaxMp = mp;
            _maxMp = (short)IntRange.ensure(_trueMaxMp, 0, 32767);
            _currentMp = Math.Min(_currentMp, _maxMp);
        }

        public virtual void addMaxMp(int i)
        {
            setMaxMp(_trueMaxMp + i);
        }

        private int _ac = 0; // ● ＡＣ（-128～127）

        private int _trueAc = 0; // ● 本当のＡＣ

        public virtual int Ac
        {
            get
            {
                return _ac + L1MagicDoll.getAcByDoll(this); // TODO 魔法娃娃效果 - 防禦增加
            }
            set
            {
                _trueAc = value;
                _ac = IntRange.ensure(value, -211, 44);
            }
        }


        public virtual void addAc(int i)
        {
            Ac = _trueAc + i;
        }

        private sbyte _str = 0; // ● ＳＴＲ（1～127）

        private short _trueStr = 0; // ● 本当のＳＴＲ

        public virtual sbyte getStr()
        {
            return _str;
        }

        public virtual void setStr(int i)
        {
            _trueStr = (short)i;
            _str = (sbyte)IntRange.ensure(i, 1, 127);
        }

        public virtual void addStr(int i)
        {
            setStr(_trueStr + i);
        }

        private sbyte _con = 0; // ● ＣＯＮ（1～127）

        private short _trueCon = 0; // ● 本当のＣＯＮ

        public virtual sbyte getCon()
        {
            return _con;
        }

        public virtual void setCon(int i)
        {
            _trueCon = (short)i;
            _con = (sbyte)IntRange.ensure(i, 1, 127);
        }

        public virtual void addCon(int i)
        {
            setCon(_trueCon + i);
        }

        private sbyte _dex = 0; // ● ＤＥＸ（1～127）

        private short _trueDex = 0; // ● 本当のＤＥＸ

        public virtual sbyte getDex()
        {
            return _dex;
        }

        public virtual void setDex(int i)
        {
            _trueDex = (short)i;
            _dex = (sbyte)IntRange.ensure(i, 1, 127);
        }

        public virtual void addDex(int i)
        {
            setDex(_trueDex + i);
        }

        private sbyte _cha = 0; // ● ＣＨＡ（1～127）

        private short _trueCha = 0; // ● 本当のＣＨＡ

        public virtual sbyte getCha()
        {
            return _cha;
        }

        public virtual void setCha(int i)
        {
            _trueCha = (short)i;
            _cha = (sbyte)IntRange.ensure(i, 1, 127);
        }

        public virtual void addCha(int i)
        {
            setCha(_trueCha + i);
        }

        private sbyte _int = 0; // ● ＩＮＴ（1～127）

        private short _trueInt = 0; // ● 本当のＩＮＴ

        public virtual sbyte getInt()
        {
            return _int;
        }

        public virtual void setInt(int i)
        {
            _trueInt = (short)i;
            _int = (sbyte)IntRange.ensure(i, 1, 127);
        }

        public virtual void addInt(int i)
        {
            setInt(_trueInt + i);
        }

        private sbyte _wis = 0; // ● ＷＩＳ（1～127）

        private short _trueWis = 0; // ● 本当のＷＩＳ

        public virtual sbyte getWis()
        {
            return _wis;
        }

        public virtual void setWis(int i)
        {
            _trueWis = (short)i;
            _wis = (sbyte)IntRange.ensure(i, 1, 127);
        }

        public virtual void addWis(int i)
        {
            setWis(_trueWis + i);
        }

        private int _wind = 0; // ● 風防御（-128～127）

        private int _trueWind = 0; // ● 本当の風防御

        public virtual int Wind
        {
            get
            {
                return _wind;
            }
        } // 使用するとき

        public virtual void addWind(int i)
        {
            _trueWind += i;
            if (_trueWind >= 127)
            {
                _wind = 127;
            }
            else if (_trueWind <= -128)
            {
                _wind = -128;
            }
            else
            {
                _wind = _trueWind;
            }
        }

        private int _water = 0; // ● 水防御（-128～127）

        private int _trueWater = 0; // ● 本当の水防御

        public virtual int Water
        {
            get
            {
                return _water;
            }
        } // 使用するとき

        public virtual void addWater(int i)
        {
            _trueWater += i;
            if (_trueWater >= 127)
            {
                _water = 127;
            }
            else if (_trueWater <= -128)
            {
                _water = -128;
            }
            else
            {
                _water = _trueWater;
            }
        }

        private int _fire = 0; // ● 火防御（-128～127）

        private int _trueFire = 0; // ● 本当の火防御

        public virtual int Fire
        {
            get
            {
                return _fire;
            }
        } // 使用するとき

        public virtual void addFire(int i)
        {
            _trueFire += i;
            if (_trueFire >= 127)
            {
                _fire = 127;
            }
            else if (_trueFire <= -128)
            {
                _fire = -128;
            }
            else
            {
                _fire = _trueFire;
            }
        }

        private int _earth = 0; // ● 地防御（-128～127）

        private int _trueEarth = 0; // ● 本当の地防御

        public virtual int Earth
        {
            get
            {
                return _earth;
            }
        } // 使用するとき

        public virtual void addEarth(int i)
        {
            _trueEarth += i;
            if (_trueEarth >= 127)
            {
                _earth = 127;
            }
            else if (_trueEarth <= -128)
            {
                _earth = -128;
            }
            else
            {
                _earth = _trueEarth;
            }
        }

        private int _addAttrKind; // エレメンタルフォールダウンで減少した属性の種類

        public virtual int AddAttrKind
        {
            get
            {
                return _addAttrKind;
            }
            set
            {
                _addAttrKind = value;
            }
        }


        // 昏迷耐性
        private int _registStun = 0;

        private int _trueRegistStun = 0;

        public virtual int RegistStun
        {
            get
            {
                return (_registStun + L1MagicDoll.getRegistStunByDoll(this));
            }
        }

        public virtual void addRegistStun(int i)
        {
            _trueRegistStun += i;
            if (_trueRegistStun > 127)
            {
                _registStun = 127;
            }
            else if (_trueRegistStun < -128)
            {
                _registStun = -128;
            }
            else
            {
                _registStun = _trueRegistStun;
            }
        }

        // 石化耐性
        private int _registStone = 0;

        private int _trueRegistStone = 0;

        public virtual int RegistStone
        {
            get
            {
                return (_registStone + L1MagicDoll.getRegistStoneByDoll(this));
            }
        }

        public virtual void addRegistStone(int i)
        {
            _trueRegistStone += i;
            if (_trueRegistStone > 127)
            {
                _registStone = 127;
            }
            else if (_trueRegistStone < -128)
            {
                _registStone = -128;
            }
            else
            {
                _registStone = _trueRegistStone;
            }
        }

        // 睡眠耐性
        private int _registSleep = 0;

        private int _trueRegistSleep = 0;

        public virtual int RegistSleep
        {
            get
            {
                return (_registSleep + L1MagicDoll.getRegistSleepByDoll(this));
            }
        }

        public virtual void addRegistSleep(int i)
        {
            _trueRegistSleep += i;
            if (_trueRegistSleep > 127)
            {
                _registSleep = 127;
            }
            else if (_trueRegistSleep < -128)
            {
                _registSleep = -128;
            }
            else
            {
                _registSleep = _trueRegistSleep;
            }
        }

        // 寒冰耐性
        private int _registFreeze = 0;

        private int _trueRegistFreeze = 0;

        public virtual int RegistFreeze
        {
            get
            {
                return (_registFreeze + L1MagicDoll.getRegistFreezeByDoll(this)); // TODO 魔法娃娃效果 - 寒冰耐性增加
            }
        }

        public virtual void add_regist_freeze(int i)
        {
            _trueRegistFreeze += i;
            if (_trueRegistFreeze > 127)
            {
                _registFreeze = 127;
            }
            else if (_trueRegistFreeze < -128)
            {
                _registFreeze = -128;
            }
            else
            {
                _registFreeze = _trueRegistFreeze;
            }
        }

        // 支撐耐性
        private int _registSustain = 0;

        private int _trueRegistSustain = 0;

        public virtual int RegistSustain
        {
            get
            {
                return (_registSustain + L1MagicDoll.getRegistSustainByDoll(this));
            }
        }

        public virtual void addRegistSustain(int i)
        {
            _trueRegistSustain += i;
            if (_trueRegistSustain > 127)
            {
                _registSustain = 127;
            }
            else if (_trueRegistSustain < -128)
            {
                _registSustain = -128;
            }
            else
            {
                _registSustain = _trueRegistSustain;
            }
        }

        // 闇黑耐性
        private int _registBlind = 0;

        private int _trueRegistBlind = 0;

        public virtual int RegistBlind
        {
            get
            {
                return (_registBlind + L1MagicDoll.getRegistBlindByDoll(this));
            }
        }

        public virtual void addRegistBlind(int i)
        {
            _trueRegistBlind += i;
            if (_trueRegistBlind > 127)
            {
                _registBlind = 127;
            }
            else if (_trueRegistBlind < -128)
            {
                _registBlind = -128;
            }
            else
            {
                _registBlind = _trueRegistBlind;
            }
        }

        private int _dmgup = 0; // ● 近距離傷害補正（-128～127）

        private int _trueDmgup = 0; // ● 本当のダメージ補正

        public virtual int Dmgup
        {
            get
            {
                return _dmgup + L1MagicDoll.getDamageAddByDoll(this); // 魔法娃娃效果 - 近距離的攻擊力增加
            }
        } // 使用するとき

        public virtual void addDmgup(int i)
        {
            _trueDmgup += i;
            if (_trueDmgup >= 127)
            {
                _dmgup = 127;
            }
            else if (_trueDmgup <= -128)
            {
                _dmgup = -128;
            }
            else
            {
                _dmgup = _trueDmgup;
            }
        }

        private int _bowDmgup = 0; // ● 弓傷害補正（-128～127）

        private int _trueBowDmgup = 0; // ● 本当の弓ダメージ補正

        public virtual int BowDmgup
        {
            get
            {
                return _bowDmgup + L1MagicDoll.getBowDamageByDoll(this); // 魔法娃娃效果 - 遠距離的攻擊力增加
            }
        }

        public virtual void addBowDmgup(int i)
        {
            _trueBowDmgup += i;
            if (_trueBowDmgup >= 127)
            {
                _bowDmgup = 127;
            }
            else if (_trueBowDmgup <= -128)
            {
                _bowDmgup = -128;
            }
            else
            {
                _bowDmgup = _trueBowDmgup;
            }
        }

        private int _hitup = 0; // ● 命中補正（-128～127）

        private int _trueHitup = 0; // ● 本当の命中補正

        public virtual int Hitup
        {
            get
            {
                return (_hitup + L1MagicDoll.getHitAddByDoll(this)); // TODO 魔法娃娃效果 - 近距離的命中力增加
            }
        }

        public virtual void addHitup(int i)
        {
            _trueHitup += i;
            if (_trueHitup >= 127)
            {
                _hitup = 127;
            }
            else if (_trueHitup <= -128)
            {
                _hitup = -128;
            }
            else
            {
                _hitup = _trueHitup;
            }
        }

        private int _bowHitup = 0; // ● 弓命中補正（-128～127）

        private int _trueBowHitup = 0; // ● 本当の弓命中補正

        public virtual int BowHitup
        {
            get
            {
                return (_bowHitup + L1MagicDoll.getBowHitAddByDoll(this)); // TODO 魔法娃娃效果 - 弓的命中力增加
            }
        }

        public virtual void addBowHitup(int i)
        {
            _trueBowHitup += i;
            if (_trueBowHitup >= 127)
            {
                _bowHitup = 127;
            }
            else if (_trueBowHitup <= -128)
            {
                _bowHitup = -128;
            }
            else
            {
                _bowHitup = _trueBowHitup;
            }
        }

        private int _mr = 0; // ● 魔法防御（0～）

        private int _trueMr = 0; // ● 本当の魔法防御

        public virtual int Mr
        {
            get
            {
                if (hasSkillEffect(153) == true)
                {
                    return _mr / 4;
                }
                else
                {
                    return _mr;
                }
            }
            set
            {
                _trueMr = value;
                if (_trueMr <= 0)
                {
                    _mr = 0;
                }
                else
                {
                    _mr = _trueMr;
                }
            }
        } // 使用するとき

        public virtual int TrueMr
        {
            get
            {
                return _trueMr;
            }
        } // セットするとき

        public virtual void addMr(int i)
        {
            _trueMr += i;
            if (_trueMr <= 0)
            {
                _mr = 0;
            }
            else
            {
                _mr = _trueMr;
            }
        }

        private int _sp = 0; // ● 増加したＳＰ

        public virtual int Sp
        {
            get
            {
                return TrueSp + _sp;
            }
        }

        public virtual int TrueSp
        {
            get
            {
                return MagicLevel + MagicBonus;
            }
        }

        public virtual void addSp(int i)
        {
            _sp += i;
        }

        private bool _isDead; // ● 死亡状態

        public virtual bool Dead
        {
            get
            {
                return _isDead;
            }
            set
            {
                _isDead = value;
            }
        }


        private int _status; // ● 状態？

        public virtual int Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }


        private string _title; // ● 頭銜

        public virtual string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }


        private int _lawful; // ● 正義值

        public virtual int Lawful
        {
            get
            {
                return _lawful;
            }
            set
            {
                _lawful = value;
            }
        }


        public virtual void addLawful(int i)
        {
            lock (this)
            {
                _lawful += i;
                if (_lawful > 32767)
                {
                    _lawful = 32767;
                }
                else if (_lawful < -32768)
                {
                    _lawful = -32768;
                }
            }
        }

        private int _heading; // ● 面向 0.左上 1.上 2.右上 3.右 4.右下 5.下 6.左下 7.左

        public virtual int Heading
        {
            get
            {
                return _heading;
            }
            set
            {
                _heading = value;
            }
        }


        private int _moveSpeed; // ● 速度 0.通常 1.加速 2.緩速

        public virtual int MoveSpeed
        {
            get
            {
                return _moveSpeed;
            }
            set
            {
                _moveSpeed = value;
            }
        }


        private int _braveSpeed; // ● 勇敢狀態 0，通常1。勇敢

        public virtual int BraveSpeed
        {
            get
            {
                return _braveSpeed;
            }
            set
            {
                _braveSpeed = value;
            }
        }


        private int _tempCharGfx; // ● 暫時變身的ID

        public virtual int TempCharGfx
        {
            get
            {
                return _tempCharGfx;
            }
            set
            {
                _tempCharGfx = value;
            }
        }


        private int _gfxid; // ● 原本圖型的ＩＤ

        public virtual int GfxId
        {
            get
            {
                return _gfxid;
            }
            set
            {
                _gfxid = value;
            }
        }


        public virtual int MagicLevel
        {
            get
            {
                return getLevel() / 4;
            }
        }

        public virtual int MagicBonus
        {
            get
            {
                int i = getInt();
                if (i <= 5)
                {
                    return -2;
                }
                else if (i <= 8)
                {
                    return -1;
                }
                else if (i <= 11)
                {
                    return 0;
                }
                else if (i <= 14)
                {
                    return 1;
                }
                else if (i <= 17)
                {
                    return 2;
                }
                else if (i <= 24)
                {
                    return i - 15;
                }
                else if (i <= 35)
                {
                    return 10;
                }
                else if (i <= 42)
                {
                    return 11;
                }
                else if (i <= 49)
                {
                    return 12;
                }
                else if (i <= 50)
                {
                    return 13;
                }
                else
                {
                    return i - 25;
                }
            }
        }

        public virtual bool Invisble
        {
            get
            {
                return (hasSkillEffect(L1SkillId.INVISIBILITY) || hasSkillEffect(L1SkillId.BLIND_HIDING));
            }
        }

        public virtual void healHp(int pt)
        {
            CurrentHp = CurrentHp + pt;
        }

        private int _karma;

        /// <summary>
        /// キャラクターが保持しているカルマを返す。
        /// </summary>
        /// <returns> カルマ。 </returns>
        public virtual int Karma
        {
            get
            {
                return _karma;
            }
            set
            {
                _karma = value;
            }
        }



        public virtual void turnOnOffLight()
        {
            int lightSize = 0;
            if (this is L1NpcInstance)
            {
                L1NpcInstance npc = (L1NpcInstance)this;
                lightSize = npc.LightSize; // npc.sqlのライトサイズ
            }
            if (hasSkillEffect(L1SkillId.LIGHT))
            {
                lightSize = 14;
            }

            foreach (L1ItemInstance item in Inventory.Items)
            {
                if ((item.Item.Type2 == 0) && (item.Item.Type == 2))
                { // light系アイテム
                    int itemlightSize = item.Item.LightRange;
                    if ((itemlightSize != 0) && item.NowLighting)
                    {
                        if (itemlightSize > lightSize)
                        {
                            lightSize = itemlightSize;
                        }
                    }
                }
            }

            if (this is L1PcInstance)
            {
                L1PcInstance pc = (L1PcInstance)this;
                pc.sendPackets(new S_Light(pc.Id, lightSize));
            }
            if (!Invisble)
            {
                broadcastPacket(new S_Light(Id, lightSize));
            }

            OwnLightSize = lightSize; // S_OwnCharPackのライト範囲
            ChaLightSize = lightSize; // S_OtherCharPack, S_NPCPackなどのライト範囲
        }

        private int _chaLightSize; // ● ライトの範囲

        public virtual int ChaLightSize
        {
            get
            {
                if (Invisble)
                {
                    return 0;
                }
                return _chaLightSize;
            }
            set
            {
                _chaLightSize = value;
            }
        }


        private int _ownLightSize; // ● ライトの範囲(S_OwnCharPack用)

        public virtual int OwnLightSize
        {
            get
            {
                return _ownLightSize;
            }
            set
            {
                _ownLightSize = value;
            }
        }


        private int _portalNumber = -1; // 龍之門扉編號

        public virtual int PortalNumber
        {
            get
            {
                return _portalNumber;
            }
            set
            {
                _portalNumber = value;
            }
        }


        // 飽食度
        private int _food;

        public virtual int get_food()
        {
            return _food;
        }

        public virtual void set_food(int i)
        {
            _food = i;
        }

        // 附魔石階級
        private sbyte _magicStoneLevel;

        public virtual sbyte MagicStoneLevel
        {
            get
            {
                return _magicStoneLevel;
            }
            set
            {
                _magicStoneLevel = value;
            }
        }


        // 閃避率 +
        private sbyte _dodge = 0;

        public virtual sbyte Dodge
        {
            get
            {
                return _dodge;
            }
        }

        public virtual void addDodge(sbyte i)
        {
            _dodge += i;
            if (_dodge >= 10)
            {
                _dodge = 10;
            }
            else if (_dodge <= 0)
            {
                _dodge = 0;
            }
        }

        // 閃避率 -
        private sbyte _nDodge = 0;

        public virtual sbyte Ndodge
        {
            get
            {
                return _nDodge;
            }
        }

        public virtual void addNdodge(sbyte i)
        {
            _nDodge += i;
            if (_nDodge >= 10)
            {
                _nDodge = 10;
            }
            else if (_nDodge <= 0)
            {
                _nDodge = 0;
            }
        }

        // 旅館
        private int _innRoomNumber;

        public virtual int InnRoomNumber
        {
            get
            {
                return _innRoomNumber;
            }
            set
            {
                _innRoomNumber = value;
            }
        }


        private int _innKeyId;

        public virtual int InnKeyId
        {
            get
            {
                return _innKeyId;
            }
            set
            {
                _innKeyId = value;
            }
        }


        private bool _isHall;

        public virtual bool checkRoomOrHall()
        {
            return _isHall;
        }

        public virtual bool Hall
        {
            set
            {
                _isHall = value;
            }
        }
        // 怪物血條判斷功能 語法來源99NETS网游模拟娱乐社区
        private L1Character _MobHPBar = null;

        public virtual L1Character MobHPBar
        {
            get
            {
                return _MobHPBar;
            }
            set
            {
                _MobHPBar = value;
            }
        }

        // end
        // 判斷特定狀態下才可攻擊 NPC
        public virtual bool isAttackMiss(L1Character cha, int npcId)
        {
            switch (npcId)
            {
                case 45912: // 士兵的怨靈
                case 45913: // 士兵的怨靈
                case 45914: // 怨靈
                case 45915: // 怨靈
                    if (!cha.hasSkillEffect(L1SkillId.STATUS_HOLY_WATER))
                    {
                        return true;
                    }
                    return false;
                case 45916: // 哈蒙將軍的怨靈
                    if (!cha.hasSkillEffect(L1SkillId.STATUS_HOLY_MITHRIL_POWDER))
                    {
                        return true;
                    }
                    return false;
                case 45941: // 受詛咒的巫女莎爾
                    if (!cha.hasSkillEffect(L1SkillId.STATUS_HOLY_WATER_OF_EVA))
                    {
                        return true;
                    }
                    return false;
                case 45752: // 炎魔(變身前)
                    if (!cha.hasSkillEffect(L1SkillId.STATUS_CURSE_BARLOG))
                    {
                        return true;
                    }
                    return false;
                case 45753: // 炎魔(變身後)
                    if (!cha.hasSkillEffect(L1SkillId.STATUS_CURSE_BARLOG))
                    {
                        return true;
                    }
                    return false;
                case 45675: // 火焰之影(變身前)
                    if (!cha.hasSkillEffect(L1SkillId.STATUS_CURSE_YAHEE))
                    {
                        return true;
                    }
                    return false;
                case 81082: // 火焰之影(變身後)
                    if (!cha.hasSkillEffect(L1SkillId.STATUS_CURSE_YAHEE))
                    {
                        return true;
                    }
                    return false;
                case 45625: // 混沌
                    if (!cha.hasSkillEffect(L1SkillId.STATUS_CURSE_YAHEE))
                    {
                        return true;
                    }
                    return false;
                case 45674: // 死亡
                    if (!cha.hasSkillEffect(L1SkillId.STATUS_CURSE_YAHEE))
                    {
                        return true;
                    }
                    return false;
                case 45685: // 墮落
                    if (!cha.hasSkillEffect(L1SkillId.STATUS_CURSE_YAHEE))
                    {
                        return true;
                    }
                    return false;
                case 81341: // 再生之祭壇
                    if (!cha.hasSkillEffect(L1SkillId.SECRET_MEDICINE_OF_DESTRUCTION))
                    {
                        return true;
                    }
                    return false;
                default:
                    if ((npcId >= 46068) && (npcId <= 46091) && (cha.TempCharGfx == 6035))
                    {
                        return true;
                    }
                    else if ((npcId >= 46092) && (npcId <= 46106) && (cha.TempCharGfx == 6034))
                    {
                        return true;
                    }
                    return false;
            }
        }
    }

}