using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using System;
using System.Threading;
namespace LineageServer.Server.Model.skill
{
    public interface IL1SkillTimer
    {
        int RemainingTime { get; }

        void begin();

        void end();

        void kill();
    }

    /*
	 * XXX 2008/02/13 vala 本来、このクラスはあるべきではないが暫定処置。
	 */
    internal class L1SkillStop
    {
        public static void stopSkill(L1Character cha, int skillId)
        {
            if (skillId == L1SkillId.LIGHT)
            { // ライト
                if (cha is L1PcInstance)
                {
                    if (!cha.Invisble)
                    {
                        L1PcInstance pc = (L1PcInstance)cha;
                        pc.turnOnOffLight();
                    }
                }
            }
            else if (skillId == L1SkillId.GLOWING_AURA)
            { // グローウィング オーラ
                cha.addHitup(-5);
                cha.addBowHitup(-5);
                cha.addMr(-20);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SPMR(pc));
                    pc.sendPackets(new S_SkillIconAura(113, 0));
                }
            }
            else if (skillId == L1SkillId.SHINING_AURA)
            { // シャイニング オーラ
                cha.addAc(8);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillIconAura(114, 0));
                }
            }
            else if (skillId == L1SkillId.BRAVE_AURA)
            { // ブレイブ オーラ
                cha.addDmgup(-5);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillIconAura(116, 0));
                }
            }
            else if (skillId == L1SkillId.SHIELD)
            { // シールド
                cha.addAc(2);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillIconShield(2, 0));
                }
            }
            else if (skillId == L1SkillId.BLIND_HIDING)
            { // ブラインドハイディング
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.delBlindHiding();
                }
            }
            else if (skillId == L1SkillId.SHADOW_ARMOR)
            { // シャドウ アーマー
                cha.addAc(3);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillIconShield(3, 0));
                }
            }
            else if (skillId == L1SkillId.DRESS_DEXTERITY)
            { // ドレス デクスタリティー
                cha.addDex((sbyte)-2);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_Dexup(pc, 2, 0));
                }
            }
            else if (skillId == L1SkillId.DRESS_MIGHTY)
            { // ドレス マイティー
                cha.addStr((sbyte)-2);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_Strup(pc, 2, 0));
                }
            }
            else if (skillId == L1SkillId.SHADOW_FANG)
            { // シャドウ ファング
                cha.addDmgup(-5);
            }
            else if (skillId == L1SkillId.ENCHANT_WEAPON)
            { // エンチャント ウェポン
                cha.addDmgup(-2);
            }
            else if (skillId == L1SkillId.BLESSED_ARMOR)
            { // ブレスド アーマー
                cha.addAc(3);
            }
            else if (skillId == L1SkillId.EARTH_BLESS)
            { // アース ブレス
                cha.addAc(7);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillIconShield(7, 0));
                }
            }
            else if (skillId == L1SkillId.RESIST_MAGIC)
            { // レジスト マジック
                cha.addMr(-10);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SPMR(pc));
                }
            }
            else if (skillId == L1SkillId.CLEAR_MIND)
            { // クリアー マインド
                cha.addWis((sbyte)-3);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.resetBaseMr();
                }
            }
            else if (skillId == L1SkillId.RESIST_ELEMENTAL)
            { // レジスト エレメント
                cha.addWind(-10);
                cha.addWater(-10);
                cha.addFire(-10);
                cha.addEarth(-10);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_OwnCharAttrDef(pc));
                }
            }
            else if (skillId == L1SkillId.ELEMENTAL_PROTECTION)
            { // エレメンタルプロテクション
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    int attr = pc.ElfAttr;
                    if (attr == 1)
                    {
                        cha.addEarth(-50);
                    }
                    else if (attr == 2)
                    {
                        cha.addFire(-50);
                    }
                    else if (attr == 4)
                    {
                        cha.addWater(-50);
                    }
                    else if (attr == 8)
                    {
                        cha.addWind(-50);
                    }
                    pc.sendPackets(new S_OwnCharAttrDef(pc));
                }
            }
            else if (skillId == L1SkillId.ELEMENTAL_FALL_DOWN)
            { // 弱化屬性
                int attr = cha.AddAttrKind;
                int i = 50;
                switch (attr)
                {
                    case 1:
                        cha.addEarth(i);
                        break;
                    case 2:
                        cha.addFire(i);
                        break;
                    case 4:
                        cha.addWater(i);
                        break;
                    case 8:
                        cha.addWind(i);
                        break;
                    default:
                        break;
                }
                cha.AddAttrKind = 0;
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_OwnCharAttrDef(pc));
                }
            }
            else if (skillId == L1SkillId.IRON_SKIN)
            { // アイアン スキン
                cha.addAc(10);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillIconShield(10, 0));
                }
            }
            else if (skillId == L1SkillId.EARTH_SKIN)
            { // アース スキン
                cha.addAc(6);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillIconShield(6, 0));
                }
            }
            else if (skillId == L1SkillId.PHYSICAL_ENCHANT_STR)
            { // フィジカル エンチャント：STR
                cha.addStr((sbyte)-5);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_Strup(pc, 5, 0));
                }
            }
            else if (skillId == L1SkillId.PHYSICAL_ENCHANT_DEX)
            { // フィジカル エンチャント：DEX
                cha.addDex((sbyte)-5);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_Dexup(pc, 5, 0));
                }
            }
            else if (skillId == L1SkillId.FIRE_WEAPON)
            { // ファイアー ウェポン
                cha.addDmgup(-4);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillIconAura(147, 0));
                }
            }
            else if (skillId == L1SkillId.FIRE_BLESS)
            { // ファイアー ブレス
                cha.addDmgup(-4);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillIconAura(154, 0));
                }
            }
            else if (skillId == L1SkillId.BURNING_WEAPON)
            { // バーニング ウェポン
                cha.addDmgup(-6);
                cha.addHitup(-3);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillIconAura(162, 0));
                }
            }
            else if (skillId == L1SkillId.BLESS_WEAPON)
            { // ブレス ウェポン
                cha.addDmgup(-2);
                cha.addHitup(-2);
                cha.addBowHitup(-2);
            }
            else if (skillId == L1SkillId.WIND_SHOT)
            { // ウィンド ショット
                cha.addBowHitup(-6);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillIconAura(148, 0));
                }
            }
            else if (skillId == L1SkillId.STORM_EYE)
            { // ストーム アイ
                cha.addBowHitup(-2);
                cha.addBowDmgup(-3);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillIconAura(155, 0));
                }
            }
            else if (skillId == L1SkillId.STORM_SHOT)
            { // ストーム ショット
                cha.addBowDmgup(-5);
                cha.addBowHitup(1);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillIconAura(165, 0));
                }
            }
            else if (skillId == L1SkillId.BERSERKERS)
            { // バーサーカー
                cha.addAc(-10);
                cha.addDmgup(-5);
                cha.addHitup(-2);
            }
            else if (skillId == L1SkillId.SHAPE_CHANGE)
            { // シェイプ チェンジ
                L1PolyMorph.undoPoly(cha);
            }
            else if (skillId == L1SkillId.ADVANCE_SPIRIT)
            { // アドバンスド スピリッツ
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-pc.AdvenHp);
                    pc.addMaxMp(-pc.AdvenMp);
                    pc.AdvenHp = 0;
                    pc.AdvenMp = 0;
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    if (pc.InParty)
                    { // パーティー中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                }
            }
            else if ((skillId == L1SkillId.HASTE) || (skillId == L1SkillId.GREATER_HASTE))
            { // ヘイスト、グレーターヘイスト
                cha.MoveSpeed = 0;
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillHaste(pc.Id, 0, 0));
                    pc.broadcastPacket(new S_SkillHaste(pc.Id, 0, 0));
                }
            }
            else if ((skillId == L1SkillId.HOLY_WALK) || (skillId == L1SkillId.MOVING_ACCELERATION) || (skillId == L1SkillId.WIND_WALK) || (skillId == L1SkillId.BLOODLUST))
            { // ホーリーウォーク、ムービングアクセレーション、ウィンドウォーク、ブラッドラスト
                cha.BraveSpeed = 0;
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillBrave(pc.Id, 0, 0));
                    pc.broadcastPacket(new S_SkillBrave(pc.Id, 0, 0));
                }
            }
            else if (skillId == L1SkillId.ILLUSION_OGRE)
            { // 幻覺：歐吉
                cha.addDmgup(-4);
                cha.addHitup(-4);
                cha.addBowDmgup(-4);
                cha.addBowHitup(-4);
            }
            else if (skillId == L1SkillId.ILLUSION_LICH)
            { // イリュージョン：リッチ
                cha.addSp(-2);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SPMR(pc));
                }
            }
            else if (skillId == L1SkillId.ILLUSION_DIA_GOLEM)
            { // イリュージョン：ダイアモンドゴーレム
                cha.addAc(20);
            }
            else if (skillId == L1SkillId.ILLUSION_AVATAR)
            { // イリュージョン：アバター
                cha.addDmgup(-10);
                cha.addBowDmgup(-10);
            }
            else if (skillId == L1SkillId.INSIGHT)
            { // 洞察
                cha.addStr((sbyte)-1);
                cha.addCon((sbyte)-1);
                cha.addDex((sbyte)-1);
                cha.addWis((sbyte)-1);
                cha.addInt((sbyte)-1);
            }
            else if (skillId == L1SkillId.PANIC)
            { // 恐慌
                cha.addStr((sbyte)1);
                cha.addCon((sbyte)1);
                cha.addDex((sbyte)1);
                cha.addWis((sbyte)1);
                cha.addInt((sbyte)1);
            }

            // ****** 状態変化が解けた場合
            else if ((skillId == L1SkillId.CURSE_BLIND) || (skillId == L1SkillId.DARKNESS))
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_CurseBlind(0));
                }
            }
            else if (skillId == L1SkillId.CURSE_PARALYZE)
            { // カーズ パラライズ
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_Poison(pc.Id, 0));
                    pc.broadcastPacket(new S_Poison(pc.Id, 0));
                    pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_PARALYSIS, false));
                }
            }
            else if (skillId == L1SkillId.WEAKNESS)
            { // 弱化術
                cha.addDmgup(5);
                cha.addHitup(1);
            }
            else if (skillId == L1SkillId.DISEASE)
            { // 疾病術
                cha.addDmgup(6);
                cha.addAc(-12);
            }
            else if ((skillId == L1SkillId.ICE_LANCE) || (skillId == L1SkillId.FREEZING_BLIZZARD) || (skillId == L1SkillId.FREEZING_BREATH) || (skillId == L1SkillId.ICE_LANCE_COCKATRICE) || (skillId == L1SkillId.ICE_LANCE_BASILISK))
            { // 邪惡蜥蜴冰矛圍籬
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_Poison(pc.Id, 0));
                    pc.broadcastPacket(new S_Poison(pc.Id, 0));
                    pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_FREEZE, false));
                }
                else if ((cha is L1MonsterInstance) || (cha is L1SummonInstance) || (cha is L1PetInstance))
                {
                    L1NpcInstance npc = (L1NpcInstance)cha;
                    npc.broadcastPacket(new S_Poison(npc.Id, 0));
                    npc.Paralyzed = false;
                }
            }
            else if (skillId == L1SkillId.EARTH_BIND)
            { // アースバインド
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_Poison(pc.Id, 0));
                    pc.broadcastPacket(new S_Poison(pc.Id, 0));
                    pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_FREEZE, false));
                }
                else if ((cha is L1MonsterInstance) || (cha is L1SummonInstance) || (cha is L1PetInstance))
                {
                    L1NpcInstance npc = (L1NpcInstance)cha;
                    npc.broadcastPacket(new S_Poison(npc.Id, 0));
                    npc.Paralyzed = false;
                }
            }
            else if (skillId == L1SkillId.SHOCK_STUN)
            { // 衝擊之暈
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_STUN, false));
                }
                else if ((cha is L1MonsterInstance) || (cha is L1SummonInstance) || (cha is L1PetInstance))
                {
                    L1NpcInstance npc = (L1NpcInstance)cha;
                    npc.Paralyzed = false;
                }
            }
            else if (skillId == L1SkillId.BONE_BREAK_START)
            { // 骷髏毀壞 (發動)
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_STUN, true));
                    pc.setSkillEffect(L1SkillId.BONE_BREAK_END, 1 * 1000);
                }
                else if ((cha is L1MonsterInstance) || (cha is L1SummonInstance) || (cha is L1PetInstance))
                {
                    L1NpcInstance npc = (L1NpcInstance)cha;
                    npc.Paralyzed = true;
                    npc.setSkillEffect(L1SkillId.BONE_BREAK_END, 1 * 1000);
                }
            }
            else if (skillId == L1SkillId.BONE_BREAK_END)
            { // 骷髏毀壞 (結束)
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_STUN, false));
                }
                else if ((cha is L1MonsterInstance) || (cha is L1SummonInstance) || (cha is L1PetInstance))
                {
                    L1NpcInstance npc = (L1NpcInstance)cha;
                    npc.Paralyzed = false;
                }
            }
            else if (skillId == L1SkillId.FOG_OF_SLEEPING)
            { // フォグ オブ スリーピング
                cha.Sleeped = false;
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_SLEEP, false));
                    pc.sendPackets(new S_OwnCharStatus(pc));
                }
            }
            else if (skillId == L1SkillId.ABSOLUTE_BARRIER)
            { // 絕對屏障
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.startHpRegeneration();
                    pc.startMpRegeneration();
                    pc.startHpRegenerationByDoll();
                    pc.startMpRegenerationByDoll();
                }
            }
            else if (skillId == L1SkillId.MEDITATION)
            { // 冥想術
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMpr(-5);
                }
            }
            else if (skillId == L1SkillId.CONCENTRATION)
            { // 專注
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMpr(-2);
                }
            }
            else if (skillId == L1SkillId.WIND_SHACKLE)
            { // 風之枷鎖
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillIconWindShackle(pc.Id, 0));
                    pc.broadcastPacket(new S_SkillIconWindShackle(pc.Id, 0));
                }
            }
            else if ((skillId == L1SkillId.SLOW) || (skillId == L1SkillId.ENTANGLE) || (skillId == L1SkillId.MASS_SLOW))
            { // スロー、エンタングル、マススロー
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillHaste(pc.Id, 0, 0));
                    pc.broadcastPacket(new S_SkillHaste(pc.Id, 0, 0));
                }
                cha.MoveSpeed = 0;
            }
            else if (skillId == L1SkillId.STATUS_FREEZE)
            { // 束縛
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_BIND, false));
                }
                else if ((cha is L1MonsterInstance) || (cha is L1SummonInstance) || (cha is L1PetInstance))
                {
                    L1NpcInstance npc = (L1NpcInstance)cha;
                    npc.Paralyzed = false;
                }
            }
            else if (skillId == L1SkillId.THUNDER_GRAB_START)
            {
                L1Skills _skill = SkillsTable.Instance.getTemplate(L1SkillId.THUNDER_GRAB); // 奪命之雷
                int _fetterDuration = _skill.BuffDuration * 1000;
                cha.setSkillEffect(L1SkillId.STATUS_FREEZE, _fetterDuration);
                L1EffectSpawn.Instance.spawnEffect(81182, _fetterDuration, cha.X, cha.Y, cha.MapId);
            }
            else if (skillId == L1SkillId.GUARD_BRAKE)
            { // 護衛毀滅
                cha.addAc(-15);
            }
            else if (skillId == L1SkillId.HORROR_OF_DEATH)
            { // 驚悚死神
                cha.addStr(5);
                cha.addInt(5);
            }
            else if (skillId == L1SkillId.STATUS_CUBE_IGNITION_TO_ALLY)
            { // キューブ[イグニション]：味方
                cha.addFire(-30);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_OwnCharAttrDef(pc));
                }
            }
            else if (skillId == L1SkillId.STATUS_CUBE_QUAKE_TO_ALLY)
            { // キューブ[クエイク]：味方
                cha.addEarth(-30);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_OwnCharAttrDef(pc));
                }
            }
            else if (skillId == L1SkillId.STATUS_CUBE_SHOCK_TO_ALLY)
            { // キューブ[ショック]：味方
                cha.addWind(-30);
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_OwnCharAttrDef(pc));
                }
            }
            else if (skillId == L1SkillId.STATUS_CUBE_IGNITION_TO_ENEMY)
            { // キューブ[イグニション]：敵
            }
            else if (skillId == L1SkillId.STATUS_CUBE_QUAKE_TO_ENEMY)
            { // キューブ[クエイク]：敵
            }
            else if (skillId == L1SkillId.STATUS_CUBE_SHOCK_TO_ENEMY)
            { // キューブ[ショック]：敵
            }
            else if (skillId == L1SkillId.STATUS_MR_REDUCTION_BY_CUBE_SHOCK)
            { // キューブ[ショック]によるMR減少
              // cha.addMr(10);
              // if (cha instanceof L1PcInstance) {
              // L1PcInstance pc = (L1PcInstance) cha;
              // pc.sendPackets(new S_SPMR(pc));
              // }
            }
            else if (skillId == L1SkillId.STATUS_CUBE_BALANCE)
            { // キューブ[バランス]
            }

            // ****** アイテム関係
            else if ((skillId == L1SkillId.STATUS_BRAVE) || (skillId == L1SkillId.STATUS_ELFBRAVE) || (skillId == L1SkillId.STATUS_BRAVE2))
            { // 二段加速
                cha.BraveSpeed = 0;
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillBrave(pc.Id, 0, 0));
                    pc.broadcastPacket(new S_SkillBrave(pc.Id, 0, 0));
                }
            }
            else if (skillId == L1SkillId.STATUS_THIRD_SPEED)
            { // 三段加速
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_Liquor(pc.Id, 0)); // 人物 * 1.15
                    pc.broadcastPacket(new S_Liquor(pc.Id, 0)); // 人物 * 1.15
                }
            }
            /// <summary>
            /// 生命之樹果實 </summary>
            /*else if (skillId == L1SkillId.STATUS_RIBRAVE) { // ユグドラの実
				// XXX ユグドラの実のアイコンを消す方法が不明
				cha.setBraveSpeed(0);
			}*/
            else if (skillId == L1SkillId.STATUS_HASTE)
            { // グリーン ポーション
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillHaste(pc.Id, 0, 0));
                    pc.broadcastPacket(new S_SkillHaste(pc.Id, 0, 0));
                }
                cha.MoveSpeed = 0;
            }
            else if (skillId == L1SkillId.STATUS_BLUE_POTION)
            { // ブルー ポーション
            }
            else if (skillId == L1SkillId.STATUS_UNDERWATER_BREATH)
            { // エヴァの祝福＆マーメイドの鱗
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillIconBlessOfEva(pc.Id, 0));
                }
            }
            else if (skillId == L1SkillId.STATUS_WISDOM_POTION)
            { // ウィズダム ポーション
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    cha.addSp(-2);
                    pc.sendPackets(new S_SkillIconWisdomPotion(0));
                }
            }
            else if (skillId == L1SkillId.STATUS_CHAT_PROHIBITED)
            { // チャット禁止
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_ServerMessage(288)); // チャットができるようになりました。
                }
            }

            // ****** 毒関係
            else if (skillId == L1SkillId.STATUS_POISON)
            { // ダメージ毒
                cha.curePoison();
            }

            // ****** 料理関係
            else if ((skillId == L1SkillId.COOKING_1_0_N) || (skillId == L1SkillId.COOKING_1_0_S))
            { // フローティングアイステーキ
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addWind(-10);
                    pc.addWater(-10);
                    pc.addFire(-10);
                    pc.addEarth(-10);
                    pc.sendPackets(new S_OwnCharAttrDef(pc));
                    pc.sendPackets(new S_PacketBox(53, 0, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_1_1_N) || (skillId == L1SkillId.COOKING_1_1_S))
            { // ベアーステーキ
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-30);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    if (pc.InParty)
                    { // パーティー中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.sendPackets(new S_PacketBox(53, 1, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_1_2_N) || (skillId == L1SkillId.COOKING_1_2_S))
            { // ナッツ餅
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_PacketBox(53, 2, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_1_3_N) || (skillId == L1SkillId.COOKING_1_3_S))
            { // 蟻脚のチーズ焼き
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addAc(1);
                    pc.sendPackets(new S_PacketBox(53, 3, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_1_4_N) || (skillId == L1SkillId.COOKING_1_4_S))
            { // フルーツサラダ
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxMp(-20);
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    pc.sendPackets(new S_PacketBox(53, 4, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_1_5_N) || (skillId == L1SkillId.COOKING_1_5_S))
            { // フルーツ甘酢あんかけ
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_PacketBox(53, 5, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_1_6_N) || (skillId == L1SkillId.COOKING_1_6_S))
            { // 猪肉の串焼き
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMr(-5);
                    pc.sendPackets(new S_SPMR(pc));
                    pc.sendPackets(new S_PacketBox(53, 6, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_1_7_N) || (skillId == L1SkillId.COOKING_1_7_S))
            { // キノコスープ
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_PacketBox(53, 7, 0));
                    pc.DessertId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_2_0_N) || (skillId == L1SkillId.COOKING_2_0_S))
            { // キャビアカナッペ
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_PacketBox(53, 8, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_2_1_N) || (skillId == L1SkillId.COOKING_2_1_S))
            { // アリゲーターステーキ
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-30);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    if (pc.InParty)
                    { // パーティー中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.addMaxMp(-30);
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    pc.sendPackets(new S_PacketBox(53, 9, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_2_2_N) || (skillId == L1SkillId.COOKING_2_2_S))
            { // タートルドラゴンの菓子
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addAc(2);
                    pc.sendPackets(new S_PacketBox(53, 10, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_2_3_N) || (skillId == L1SkillId.COOKING_2_3_S))
            { // キウィパロット焼き
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_PacketBox(53, 11, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_2_4_N) || (skillId == L1SkillId.COOKING_2_4_S))
            { // スコーピオン焼き
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_PacketBox(53, 12, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_2_5_N) || (skillId == L1SkillId.COOKING_2_5_S))
            { // イレッカドムシチュー
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMr(-10);
                    pc.sendPackets(new S_SPMR(pc));
                    pc.sendPackets(new S_PacketBox(53, 13, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_2_6_N) || (skillId == L1SkillId.COOKING_2_6_S))
            { // クモ脚の串焼き
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addSp(-1);
                    pc.sendPackets(new S_SPMR(pc));
                    pc.sendPackets(new S_PacketBox(53, 14, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_2_7_N) || (skillId == L1SkillId.COOKING_2_7_S))
            { // クラブスープ
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_PacketBox(53, 15, 0));
                    pc.DessertId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_3_0_N) || (skillId == L1SkillId.COOKING_3_0_S))
            { // クラスタシアンのハサミ焼き
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_PacketBox(53, 16, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_3_1_N) || (skillId == L1SkillId.COOKING_3_1_S))
            { // グリフォン焼き
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-50);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    if (pc.InParty)
                    { // パーティー中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.addMaxMp(-50);
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    pc.sendPackets(new S_PacketBox(53, 17, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_3_2_N) || (skillId == L1SkillId.COOKING_3_2_S))
            { // コカトリスステーキ
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_PacketBox(53, 18, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_3_3_N) || (skillId == L1SkillId.COOKING_3_3_S))
            { // タートルドラゴン焼き
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addAc(3);
                    pc.sendPackets(new S_PacketBox(53, 19, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_3_4_N) || (skillId == L1SkillId.COOKING_3_4_S))
            { // レッサードラゴンの手羽先
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMr(-15);
                    pc.sendPackets(new S_SPMR(pc));
                    pc.addWind(-10);
                    pc.addWater(-10);
                    pc.addFire(-10);
                    pc.addEarth(-10);
                    pc.sendPackets(new S_OwnCharAttrDef(pc));
                    pc.sendPackets(new S_PacketBox(53, 20, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_3_5_N) || (skillId == L1SkillId.COOKING_3_5_S))
            { // ドレイク焼き
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addSp(-2);
                    pc.sendPackets(new S_SPMR(pc));
                    pc.sendPackets(new S_PacketBox(53, 21, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_3_6_N) || (skillId == L1SkillId.COOKING_3_6_S))
            { // 深海魚のシチュー
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-30);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    if (pc.InParty)
                    { // パーティー中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.sendPackets(new S_PacketBox(53, 22, 0));
                    pc.CookingId = 0;
                }
            }
            else if ((skillId == L1SkillId.COOKING_3_7_N) || (skillId == L1SkillId.COOKING_3_7_S))
            { // バシリスクの卵スープ
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_PacketBox(53, 23, 0));
                    pc.DessertId = 0;
                }
            }
            else if (skillId == L1SkillId.COOKING_WONDER_DRUG)
            { // 象牙塔妙藥
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addHpr(-10);
                    pc.addMpr(-2);
                }
            }
            // ****** 
            else if (skillId == L1SkillId.EFFECT_BLESS_OF_MAZU)
            { // 媽祖的祝福
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addHitup(-3);
                    pc.addDmgup(-3);
                    pc.addMpr(-2);
                }
            }
            else if (skillId == L1SkillId.EFFECT_STRENGTHENING_HP)
            { // 體力增強卷軸
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-50);
                    pc.addHpr(-4);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                }
            }
            else if (skillId == L1SkillId.EFFECT_STRENGTHENING_MP)
            { // 魔力增強卷軸
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxMp(-40);
                    pc.addMpr(-4);
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                }
            }
            else if (skillId == L1SkillId.EFFECT_ENCHANTING_BATTLE)
            { // 強化戰鬥卷軸
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addHitup(-3);
                    pc.addDmgup(-3);
                    pc.addBowHitup(-3);
                    pc.addBowDmgup(-3);
                    pc.addSp(-3);
                    pc.sendPackets(new S_SPMR(pc));
                }
            }
            else if (skillId == L1SkillId.MIRROR_IMAGE || skillId == L1SkillId.UNCANNY_DODGE)
            { // 鏡像、暗影閃避
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addDodge((sbyte)-5); // 閃避率 - 50%
                                            // 更新閃避率顯示
                    pc.sendPackets(new S_PacketBox(88, pc.Dodge));
                }
            }
            else if (skillId == L1SkillId.RESIST_FEAR)
            { // 恐懼無助
                cha.addNdodge((sbyte)-5); // 閃避率 + 50%
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    // 更新閃避率顯示
                    pc.sendPackets(new S_PacketBox(101, pc.Ndodge));
                }
            }
            else if (skillId == L1SkillId.EFFECT_BLOODSTAIN_OF_ANTHARAS)
            { // 安塔瑞斯的血痕
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addAc(2);
                    pc.addWater(-50);
                    pc.sendPackets(new S_SkillIconBloodstain(82, 0));
                }
            }
            else if (skillId == L1SkillId.EFFECT_BLOODSTAIN_OF_FAFURION)
            { // 法利昂的血痕
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addWind(-50);
                    pc.sendPackets(new S_SkillIconBloodstain(85, 0));
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_A_1)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-10);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_A_2)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-20);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_A_3)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-30);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_A_4)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-40);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_A_5)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-50);
                    pc.addHpr(-1);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_A_6)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-60);
                    pc.addHpr(-2);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_A_7)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-70);
                    pc.addHpr(-3);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_A_8)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-80);
                    pc.addHpr(-4);
                    pc.addHitup(-1);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_A_9)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-100);
                    pc.addHpr(-5);
                    pc.addHitup(-2);
                    pc.addDmgup(-2);
                    pc.addStr((sbyte)-1);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_B_1)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-5);
                    pc.addMaxMp(-3);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_B_2)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-10);
                    pc.addMaxMp(-6);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_B_3)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-15);
                    pc.addMaxMp(-10);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_B_4)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-20);
                    pc.addMaxMp(-15);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_B_5)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-25);
                    pc.addMaxMp(-20);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_B_6)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-30);
                    pc.addMaxMp(-20);
                    pc.addHpr(-1);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_B_7)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-35);
                    pc.addMaxMp(-20);
                    pc.addHpr(-1);
                    pc.addMpr(-1);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_B_8)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-40);
                    pc.addMaxMp(-25);
                    pc.addHpr(-2);
                    pc.addMpr(-1);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_B_9)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-50);
                    pc.addMaxMp(-30);
                    pc.addHpr(-2);
                    pc.addMpr(-2);
                    pc.addBowDmgup(-2);
                    pc.addBowHitup(-2);
                    pc.addDex((sbyte)-1);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_C_1)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxMp(-5);
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_C_2)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxMp(-10);
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_C_3)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxMp(-15);
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_C_4)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxMp(-20);
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_C_5)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxMp(-25);
                    pc.addMpr(-1);
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_C_6)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxMp(-30);
                    pc.addMpr(-2);
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_C_7)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxMp(-35);
                    pc.addMpr(-3);
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_C_8)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxMp(-40);
                    pc.addMpr(-4);
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_C_9)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxMp(-50);
                    pc.addMpr(-5);
                    pc.addInt((sbyte)-1);
                    pc.addSp(-1);
                    pc.sendPackets(new S_SPMR(pc));
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_D_1)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMr(-2);
                    pc.sendPackets(new S_SPMR(pc));
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_D_2)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMr(-4);
                    pc.sendPackets(new S_SPMR(pc));
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_D_3)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMr(-6);
                    pc.sendPackets(new S_SPMR(pc));
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_D_4)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMr(-8);
                    pc.sendPackets(new S_SPMR(pc));
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_D_5)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMr(-10);
                    pc.addAc(1);
                    pc.sendPackets(new S_SPMR(pc));
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_D_6)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMr(-10);
                    pc.addAc(2);
                    pc.sendPackets(new S_SPMR(pc));
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_D_7)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMr(-10);
                    pc.addAc(3);
                    pc.sendPackets(new S_SPMR(pc));
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_D_8)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMr(-15);
                    pc.addAc(4);
                    pc.addDamageReductionByArmor(-1);
                    pc.sendPackets(new S_SPMR(pc));
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_STONE_D_9)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMr(-20);
                    pc.addAc(5);
                    pc.addCon((sbyte)-1);
                    pc.addDamageReductionByArmor(-3);
                    pc.sendPackets(new S_SPMR(pc));
                    pc.MagicStoneLevel = (sbyte)0;
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_EYE_OF_AHTHARTS)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addRegistStone(-3); // 石化耐性

                    pc.addDodge((sbyte)-1); // 閃避率 - 10%
                                            // 更新閃避率顯示
                    pc.sendPackets(new S_PacketBox(88, pc.Dodge));
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_EYE_OF_FAFURION)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.add_regist_freeze(-3); // 寒冰耐性
                                              // 魔法傷害減免
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_EYE_OF_LINDVIOR)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addRegistSleep(-3); // 睡眠耐性
                                           // 魔法暴擊率
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_EYE_OF_VALAKAS)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addRegistStun(-3); // 昏迷耐性
                    pc.addDmgup(-2); // 額外攻擊點數
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_EYE_OF_BIRTH)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addRegistBlind(-3); // 闇黑耐性
                                           // 魔法傷害減免

                    pc.addDodge((sbyte)-1); // 閃避率 - 10%
                                            // 更新閃避率顯示
                    pc.sendPackets(new S_PacketBox(88, pc.Dodge));
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_EYE_OF_FIGURE)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addRegistSustain(-3); // 支撐耐性
                                             // 魔法傷害減免
                                             // 魔法暴擊率

                    pc.addDodge((sbyte)-1); // 閃避率 - 10%
                                            // 更新閃避率顯示
                    pc.sendPackets(new S_PacketBox(88, pc.Dodge));
                }
            }
            else if (skillId == L1SkillId.EFFECT_MAGIC_EYE_OF_LIFE)
            {
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addDmgup(2); // 額外攻擊點數
                                    // 魔法傷害減免
                                    // 魔法暴擊率
                                    // 防護中毒狀態

                    pc.addDodge((sbyte)-1); // 閃避率 - 10%
                                            // 更新閃避率顯示
                    pc.sendPackets(new S_PacketBox(88, pc.Dodge));
                }
            }
            else if (skillId == L1SkillId.EFFECT_BLESS_OF_CRAY)
            { // 卡瑞的祝福
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-100);
                    pc.addMaxMp(-50);
                    pc.addHpr(-3);
                    pc.addMpr(-3);
                    pc.addEarth(-30);
                    pc.addDmgup(-1);
                    pc.addHitup(-5);
                    pc.addWeightReduction(-40);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    if (pc.InParty)
                    {
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                }
            }
            else if (skillId == L1SkillId.EFFECT_BLESS_OF_SAELL)
            { // 莎爾的祝福
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.addMaxHp(-80);
                    pc.addMaxMp(-10);
                    pc.addWater(-30);
                    pc.addAc(8);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    if (pc.InParty)
                    {
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                }
            }
            else if (skillId == L1SkillId.ERASE_MAGIC)
            { // 魔法消除
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillIconAura(152, 0));
                }
            }
            else if (skillId == L1SkillId.STATUS_CURSE_YAHEE)
            { // 炎魔的烙印
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillIconAura(221, 0, 1));
                }
            }
            else if (skillId == L1SkillId.STATUS_CURSE_BARLOG)
            { // 火焰之影的烙印
                if (cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)cha;
                    pc.sendPackets(new S_SkillIconAura(221, 0, 2));
                }
            }

            if (cha is L1PcInstance)
            {
                L1PcInstance pc = (L1PcInstance)cha;
                sendStopMessage(pc, skillId);
                pc.sendPackets(new S_OwnCharStatus(pc));
            }
        }

        // メッセージの表示（終了するとき）
        private static void sendStopMessage(L1PcInstance charaPc, int skillid)
        {
            L1Skills l1skills = SkillsTable.Instance.getTemplate(skillid);
            if ((l1skills == null) || (charaPc == null))
            {
                return;
            }

            int msgID = l1skills.SysmsgIdStop;
            if (msgID > 0)
            {
                charaPc.sendPackets(new S_ServerMessage(msgID));
            }
        }
    }
    //之後看
    /*
	internal class L1SkillTimerThreadImpl : IRunnable, IL1SkillTimer
	{
		public L1SkillTimerThreadImpl(L1Character cha, int skillId, int timeMillis)
		{
			_cha = cha;
			_skillId = skillId;
			_timeMillis = timeMillis;
		}

		public void run()
		{
			for (int timeCount = _timeMillis / 1000; timeCount > 0; timeCount--)
			{
				Thread.Sleep(1000);
				_remainingTime = timeCount;
			}
			_cha.removeSkillEffect(L1SkillId._skillId);
		}

		public virtual int RemainingTime
		{
			get
			{
				return _remainingTime;
			}
		}

		public virtual void begin()
		{
			Container.Instance.Resolve<ITaskController>().execute(this);
		}

		public virtual void end()
		{
			base.Interrupt();
			L1SkillStop.stopSkill(_cha, _skillId);
		}

		public virtual void kill()
		{
			if (Thread.CurrentThread.Id == base.Id)
			{
				return; // 呼び出し元スレッドが自分であれば止めない
			}
			base.Interrupt();
		}

		private readonly L1Character _cha;

		private readonly int _timeMillis;

		private readonly int _skillId;

		private int _remainingTime;
	}
	*/
    internal class L1SkillTimerTimerImpl : Models.TimerTask, IL1SkillTimer
    {
        public L1SkillTimerTimerImpl(L1Character cha, int skillId, int timeMillis)
        {
            _cha = cha;
            _skillId = skillId;
            _timeMillis = timeMillis;

            _remainingTime = _timeMillis / 1000;
        }

        public override void run()
        {
            _remainingTime--;
            if (_remainingTime <= 0)
            {
                _cha.removeSkillEffect(_skillId);
            }
        }

        public virtual void begin()
        {
            Container.Instance.Resolve<ITaskController>().execute(this, 1000, 1000);
        }

        public virtual void end()
        {
            kill();
            L1SkillStop.stopSkill(_cha, _skillId);
        }

        public virtual void kill()
        {
            cancel();
        }

        public virtual int RemainingTime
        {
            get
            {
                return _remainingTime;
            }
        }

        private readonly L1Character _cha;

        private readonly int _timeMillis;

        private readonly int _skillId;

        private int _remainingTime;
    }
}