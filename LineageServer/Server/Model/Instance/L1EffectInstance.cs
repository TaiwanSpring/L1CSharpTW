using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataSources;
using LineageServer.Server.Model.poison;
using LineageServer.Server.Model.skill;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using System;
using System.Threading;
namespace LineageServer.Server.Model.Instance
{
    class L1EffectInstance : L1NpcInstance
    {
        private const int FW_DAMAGE_INTERVAL = 1000;

        private const int CUBE_INTERVAL = 500; // キューブ範囲内に居るキャラクターをチェックする間隔

        private const int CUBE_TIME = 8000; // 効果時間8秒?

        private const int POISON_INTERVAL = 1000;

        public L1EffectInstance(L1Npc template) : base(template)
        {

            int npcId = NpcTemplate.get_npcId();
            if (npcId == 81157)
            { // FW
                RunnableExecuter.Instance.execute(new FwDamageTimer(this, this));
            }
            else if ((npcId == 80149) || (npcId == 80150) || (npcId == 80151) || (npcId == 80152))
            { // キューブ[バランス]
                RunnableExecuter.Instance.execute(new CubeTimer(this, this));
            }
            else if (npcId == 93002)
            { // 毒霧
                RunnableExecuter.Instance.execute(new PoisonTimer(this, this));
            }
        }

        public override void onAction(L1PcInstance pc)
        {
        }

        public override void deleteMe()
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
            foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(this))
            {
                pc.removeKnownObject(this);
                pc.sendPackets(new S_RemoveObject(this));
            }
            removeAllKnownObjects();
        }

        internal class FwDamageTimer : IRunnable
        {
            private readonly L1EffectInstance outerInstance;

            internal L1EffectInstance _effect;

            public FwDamageTimer(L1EffectInstance outerInstance, L1EffectInstance effect)
            {
                this.outerInstance = outerInstance;
                _effect = effect;
            }

            public void run()
            {
                while (!outerInstance._destroyed)
                {
                    try
                    {
                        foreach (GameObject objects in L1World.Instance.getVisibleObjects(_effect, 0))
                        {
                            if (objects is L1PcInstance)
                            {
                                L1PcInstance pc = (L1PcInstance)objects;
                                if (pc.Dead)
                                {
                                    continue;
                                }
                                if (pc.ZoneType == 1)
                                {
                                    bool isNowWar = false;
                                    int castleId = L1CastleLocation.getCastleIdByArea(pc);
                                    if (castleId > 0)
                                    {
                                        isNowWar = WarTimeController.Instance.isNowWar(castleId);
                                    }
                                    if (!isNowWar)
                                    {
                                        continue;
                                    }
                                }
                                L1Magic magic = new L1Magic(_effect, pc);
                                int damage = magic.calcPcFireWallDamage();
                                if (damage == 0)
                                {
                                    continue;
                                }
                                pc.sendPackets(new S_DoActionGFX(pc.Id, ActionCodes.ACTION_Damage));
                                pc.broadcastPacket(new S_DoActionGFX(pc.Id, ActionCodes.ACTION_Damage));
                                pc.receiveDamage(_effect, damage, false);
                            }
                            else if (objects is L1MonsterInstance)
                            {
                                L1MonsterInstance mob = (L1MonsterInstance)objects;
                                if (mob.Dead)
                                {
                                    continue;
                                }
                                L1Magic magic = new L1Magic(_effect, mob);
                                int damage = magic.calcNpcFireWallDamage();
                                if (damage == 0)
                                {
                                    continue;
                                }
                                mob.broadcastPacket(new S_DoActionGFX(mob.Id, ActionCodes.ACTION_Damage));
                                mob.receiveDamage(_effect, damage);
                            }
                        }
                        Thread.Sleep(FW_DAMAGE_INTERVAL);
                    }
                    catch (Exception e)
                    {
                        // ignore
                    }
                }
            }
        }

        internal class CubeTimer : IRunnable
        {
            private readonly L1EffectInstance outerInstance;

            internal L1EffectInstance _effect;

            public CubeTimer(L1EffectInstance outerInstance, L1EffectInstance effect)
            {
                this.outerInstance = outerInstance;
                _effect = effect;
            }

            public void run()
            {
                while (!outerInstance._destroyed)
                {
                    try
                    {
                        foreach (GameObject objects in L1World.Instance.getVisibleObjects(_effect, 3))
                        {
                            if (objects is L1PcInstance)
                            {
                                L1PcInstance pc = (L1PcInstance)objects;
                                if (pc.Dead)
                                {
                                    continue;
                                }
                                L1PcInstance user = outerInstance.User; // Cube使用者
                                if (pc.Id == user.Id)
                                {
                                    outerInstance.cubeToAlly(pc, _effect);
                                    continue;
                                }
                                if ((pc.Clanid != 0) && (user.Clanid == pc.Clanid))
                                {
                                    outerInstance.cubeToAlly(pc, _effect);
                                    continue;
                                }
                                if (pc.InParty && pc.Party.isMember(user))
                                {
                                    outerInstance.cubeToAlly(pc, _effect);
                                    continue;
                                }
                                if (pc.ZoneType == 1)
                                { // セーフティーゾーンでは戦争中を除き敵には無効
                                    bool isNowWar = false;
                                    int castleId = L1CastleLocation.getCastleIdByArea(pc);
                                    if (castleId > 0)
                                    {
                                        isNowWar = WarTimeController.Instance.isNowWar(castleId);
                                    }
                                    if (!isNowWar)
                                    {
                                        continue;
                                    }
                                    outerInstance.cubeToEnemy(pc, _effect);
                                }
                                else
                                {
                                    outerInstance.cubeToEnemy(pc, _effect);
                                }
                            }
                            else if (objects is L1MonsterInstance)
                            {
                                L1MonsterInstance mob = (L1MonsterInstance)objects;
                                if (mob.Dead)
                                {
                                    continue;
                                }
                                outerInstance.cubeToEnemy(mob, _effect);
                            }
                        }
                        Thread.Sleep(CUBE_INTERVAL);
                    }
                    catch (Exception e)
                    {
                        // ignore
                    }
                }
            }
        }

        internal class PoisonTimer : IRunnable
        {
            private readonly L1EffectInstance outerInstance;

            internal L1EffectInstance _effect;


            public PoisonTimer(L1EffectInstance outerInstance, L1EffectInstance effect)
            {
                this.outerInstance = outerInstance;
                _effect = effect;
            }

            public void run()
            {
                while (!outerInstance._destroyed)
                {
                    try
                    {
                        foreach (GameObject objects in L1World.Instance.getVisibleObjects(_effect, 0))
                        {
                            if (!(objects is L1MonsterInstance))
                            {
                                L1Character cha = (L1Character)objects;
                                L1DamagePoison.doInfection(_effect, cha, 3000, 20);
                            }
                        }
                        Thread.Sleep(POISON_INTERVAL);
                    }
                    catch (Exception e)
                    {
                        // ignore
                    }
                }
            }
        }

        private void cubeToAlly(L1Character cha, L1Character effect)
        {
            int npcId = NpcTemplate.get_npcId();
            int castGfx = SkillsTable.Instance.getTemplate(SkillId).CastGfx;
            L1PcInstance pc = null;

            if (npcId == 80149)
            { // キューブ[イグニション]
                if (!cha.hasSkillEffect(L1SkillId.STATUS_CUBE_IGNITION_TO_ALLY))
                {
                    cha.addFire(30);
                    if (cha is L1PcInstance)
                    {
                        pc = (L1PcInstance)cha;
                        pc.sendPackets(new S_OwnCharAttrDef(pc));
                        pc.sendPackets(new S_SkillSound(pc.Id, castGfx));
                    }
                    cha.broadcastPacket(new S_SkillSound(cha.Id, castGfx));
                    cha.setSkillEffect(L1SkillId.STATUS_CUBE_IGNITION_TO_ALLY, CUBE_TIME);
                }
            }
            else if (npcId == 80150)
            { // キューブ[クエイク]
                if (!cha.hasSkillEffect(L1SkillId.STATUS_CUBE_QUAKE_TO_ALLY))
                {
                    cha.addEarth(30);
                    if (cha is L1PcInstance)
                    {
                        pc = (L1PcInstance)cha;
                        pc.sendPackets(new S_OwnCharAttrDef(pc));
                        pc.sendPackets(new S_SkillSound(pc.Id, castGfx));
                    }
                    cha.broadcastPacket(new S_SkillSound(cha.Id, castGfx));
                    cha.setSkillEffect(L1SkillId.STATUS_CUBE_QUAKE_TO_ALLY, CUBE_TIME);
                }
            }
            else if (npcId == 80151)
            { // キューブ[ショック]
                if (!cha.hasSkillEffect(L1SkillId.STATUS_CUBE_SHOCK_TO_ALLY))
                {
                    cha.addWind(30);
                    if (cha is L1PcInstance)
                    {
                        pc = (L1PcInstance)cha;
                        pc.sendPackets(new S_OwnCharAttrDef(pc));
                        pc.sendPackets(new S_SkillSound(pc.Id, castGfx));
                    }
                    cha.broadcastPacket(new S_SkillSound(cha.Id, castGfx));
                    cha.setSkillEffect(L1SkillId.STATUS_CUBE_SHOCK_TO_ALLY, CUBE_TIME);
                }
            }
            else if (npcId == 80152)
            { // キューブ[バランス]
                if (!cha.hasSkillEffect(L1SkillId.STATUS_CUBE_BALANCE))
                {
                    if (cha is L1PcInstance)
                    {
                        pc = (L1PcInstance)cha;
                        pc.sendPackets(new S_SkillSound(pc.Id, castGfx));
                    }
                    cha.broadcastPacket(new S_SkillSound(cha.Id, castGfx));
                    cha.setSkillEffect(L1SkillId.STATUS_CUBE_BALANCE, CUBE_TIME);
                    L1Cube cube = new L1Cube(effect, cha, L1SkillId.STATUS_CUBE_BALANCE);
                    cube.begin();
                }
            }
        }

        private void cubeToEnemy(L1Character cha, L1Character effect)
        {
            int npcId = NpcTemplate.get_npcId();
            int castGfx2 = SkillsTable.Instance.getTemplate(SkillId).CastGfx2;
            L1PcInstance pc = null;
            if (npcId == 80149)
            { // キューブ[イグニション]
                if (!cha.hasSkillEffect(L1SkillId.STATUS_CUBE_IGNITION_TO_ENEMY))
                {
                    if (cha is L1PcInstance)
                    {
                        pc = (L1PcInstance)cha;
                        pc.sendPackets(new S_SkillSound(pc.Id, castGfx2));
                    }
                    cha.broadcastPacket(new S_SkillSound(cha.Id, castGfx2));
                    cha.setSkillEffect(L1SkillId.STATUS_CUBE_IGNITION_TO_ENEMY, CUBE_TIME);
                    L1Cube cube = new L1Cube(effect, cha, L1SkillId.STATUS_CUBE_IGNITION_TO_ENEMY);
                    cube.begin();
                }
            }
            else if (npcId == 80150)
            { // キューブ[クエイク]
                if (!cha.hasSkillEffect(L1SkillId.STATUS_CUBE_QUAKE_TO_ENEMY))
                {
                    if (cha is L1PcInstance)
                    {
                        pc = (L1PcInstance)cha;
                        pc.sendPackets(new S_SkillSound(pc.Id, castGfx2));
                    }
                    cha.broadcastPacket(new S_SkillSound(cha.Id, castGfx2));
                    cha.setSkillEffect(L1SkillId.STATUS_CUBE_QUAKE_TO_ENEMY, CUBE_TIME);
                    L1Cube cube = new L1Cube(effect, cha, L1SkillId.STATUS_CUBE_QUAKE_TO_ENEMY);
                    cube.begin();
                }
            }
            else if (npcId == 80151)
            { // キューブ[ショック]
                if (!cha.hasSkillEffect(L1SkillId.STATUS_CUBE_SHOCK_TO_ENEMY))
                {
                    if (cha is L1PcInstance)
                    {
                        pc = (L1PcInstance)cha;
                        pc.sendPackets(new S_SkillSound(pc.Id, castGfx2));
                    }
                    cha.broadcastPacket(new S_SkillSound(cha.Id, castGfx2));
                    cha.setSkillEffect(L1SkillId.STATUS_CUBE_SHOCK_TO_ENEMY, CUBE_TIME);
                    L1Cube cube = new L1Cube(effect, cha, L1SkillId.STATUS_CUBE_SHOCK_TO_ENEMY);
                    cube.begin();
                }
            }
            else if (npcId == 80152)
            { // キューブ[バランス]
                if (!cha.hasSkillEffect(L1SkillId.STATUS_CUBE_BALANCE))
                {
                    if (cha is L1PcInstance)
                    {
                        pc = (L1PcInstance)cha;
                        pc.sendPackets(new S_SkillSound(pc.Id, castGfx2));
                    }
                    cha.broadcastPacket(new S_SkillSound(cha.Id, castGfx2));
                    cha.setSkillEffect(L1SkillId.STATUS_CUBE_BALANCE, CUBE_TIME);
                    L1Cube cube = new L1Cube(effect, cha, L1SkillId.STATUS_CUBE_BALANCE);
                    cube.begin();
                }
            }
        }

        private L1PcInstance _pc;

        public virtual L1PcInstance User
        {
            set
            {
                _pc = value;
            }
            get
            {
                return _pc;
            }
        }


        private int _skillId;

        public virtual int SkillId
        {
            set
            {
                _skillId = value;
            }
            get
            {
                return _skillId;
            }
        }


    }

}