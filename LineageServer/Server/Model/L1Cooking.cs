﻿using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;

namespace LineageServer.Server.Model
{
    class L1Cooking
    {
        public static void useCookingItem(L1PcInstance pc, L1ItemInstance item)
        {
            int itemId = item.Item.ItemId;
            int cookingId = 0;
            if ((itemId == 41284) || (itemId == 41292) || (itemId == 49056) || (itemId == 49064) || (itemId == 49251) || (itemId == 49259))
            { // デザート
                if (pc.get_food() != 225)
                {
                    pc.sendPackets(new S_ServerMessage(74, item.getNumberedName(1))); // \f1%0は使用できません。
                    return;
                }
            }

            // 料理 LV1、特別的料理 LV1、料理 LV2、特別的料理 LV2、料理 LV3、特別的料理 LV3 - 不可重複
            if (((itemId >= 41277) && (itemId <= 41283)) || ((itemId >= 41285) && (itemId <= 41291)) || ((itemId >= 49049) && (itemId <= 49055)) || ((itemId >= 49057) && (itemId <= 49063)) || ((itemId >= 49244) && (itemId <= 49250)) || ((itemId >= 49252) && (itemId <= 49258)))
            {
                cookingId = pc.CookingId;
                if (cookingId != 0)
                {
                    pc.removeSkillEffect(cookingId);
                }
            }

            // 蘑菇湯、特別的蘑菇湯、蟹肉湯、特別的蟹肉湯、邪惡蜥蜴蛋湯、特別的邪惡蜥蜴蛋湯 - 不可重複
            if ((itemId == 41284) || (itemId == 41292) || (itemId == 49056) || (itemId == 49064) || (itemId == 49251) || (itemId == 49259))
            {
                int dessertId = pc.DessertId;
                if (dessertId != 0)
                {
                    pc.removeSkillEffect(dessertId);
                }
            }

            int time = 900;
            if ((itemId == 41277) || (itemId == 41285))
            { // フローティングアイステーキ
                if (itemId == 41277)
                {
                    cookingId = L1SkillId.COOKING_1_0_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_1_0_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 41278) || (itemId == 41286))
            { // ベアーステーキ
                if (itemId == 41278)
                {
                    cookingId = L1SkillId.COOKING_1_1_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_1_1_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 41279) || (itemId == 41287))
            { // ナッツ餅
                if (itemId == 41279)
                {
                    cookingId = L1SkillId.COOKING_1_2_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_1_2_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 41280) || (itemId == 41288))
            { // 蟻脚のチーズ焼き
                if (itemId == 41280)
                {
                    cookingId = L1SkillId.COOKING_1_3_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_1_3_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 41281) || (itemId == 41289))
            { // フルーツサラダ
                if (itemId == 41281)
                {
                    cookingId = L1SkillId.COOKING_1_4_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_1_4_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 41282) || (itemId == 41290))
            { // フルーツ甘酢あんかけ
                if (itemId == 41282)
                {
                    cookingId = L1SkillId.COOKING_1_5_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_1_5_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 41283) || (itemId == 41291))
            { // 猪肉の串焼き
                if (itemId == 41283)
                {
                    cookingId = L1SkillId.COOKING_1_6_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_1_6_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 41284) || (itemId == 41292))
            { // キノコスープ
                if (itemId == 41284)
                {
                    cookingId = L1SkillId.COOKING_1_7_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_1_7_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 49049) || (itemId == 49057))
            { // キャビアカナッペ
                if (itemId == 49049)
                {
                    cookingId = L1SkillId.COOKING_2_0_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_2_0_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 49050) || (itemId == 49058))
            { // アリゲーターステーキ
                if (itemId == 49050)
                {
                    cookingId = L1SkillId.COOKING_2_1_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_2_1_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 49051) || (itemId == 49059))
            { // タートルドラゴンの菓子
                if (itemId == 49051)
                {
                    cookingId = L1SkillId.COOKING_2_2_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_2_2_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 49052) || (itemId == 49060))
            { // キウィパロット焼き
                if (itemId == 49052)
                {
                    cookingId = L1SkillId.COOKING_2_3_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_2_3_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 49053) || (itemId == 49061))
            { // スコーピオン焼き
                if (itemId == 49053)
                {
                    cookingId = L1SkillId.COOKING_2_4_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_2_4_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 49054) || (itemId == 49062))
            { // イレッカドムシチュー
                if (itemId == 49054)
                {
                    cookingId = L1SkillId.COOKING_2_5_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_2_5_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 49055) || (itemId == 49063))
            { // クモ脚の串焼き
                if (itemId == 49055)
                {
                    cookingId = L1SkillId.COOKING_2_6_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_2_6_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 49056) || (itemId == 49064))
            { // クラブスープ
                if (itemId == 49056)
                {
                    cookingId = L1SkillId.COOKING_2_7_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_2_7_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 49244) || (itemId == 49252))
            { // クラスタシアンのハサミ焼き
                if (itemId == 49244)
                {
                    cookingId = L1SkillId.COOKING_3_0_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_3_0_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 49245) || (itemId == 49253))
            { // グリフォン焼き
                if (itemId == 49245)
                {
                    cookingId = L1SkillId.COOKING_3_1_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_3_1_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 49246) || (itemId == 49254))
            { // コカトリスステーキ
                if (itemId == 49246)
                {
                    cookingId = L1SkillId.COOKING_3_2_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_3_2_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 49247) || (itemId == 49255))
            { // タートルドラゴン焼き
                if (itemId == 49247)
                {
                    cookingId = L1SkillId.COOKING_3_3_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_3_3_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 49248) || (itemId == 49256))
            { // レッサードラゴンの手羽先
                if (itemId == 49248)
                {
                    cookingId = L1SkillId.COOKING_3_4_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_3_4_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 49249) || (itemId == 49257))
            { // ドレイク焼き
                if (itemId == 49249)
                {
                    cookingId = L1SkillId.COOKING_3_5_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_3_5_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 49250) || (itemId == 49258))
            { // 深海魚のシチュー
                if (itemId == 49250)
                {
                    cookingId = L1SkillId.COOKING_3_6_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_3_6_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if ((itemId == 49251) || (itemId == 49259))
            { // バシリスクの卵スープ
                if (itemId == 49251)
                {
                    cookingId = L1SkillId.COOKING_3_7_N;
                }
                else
                {
                    cookingId = L1SkillId.COOKING_3_7_S;
                }
                eatCooking(pc, cookingId, time);
            }
            else if (itemId == L1ItemId.POTION_OF_WONDER_DRUG)
            { // 象牙塔妙藥
                cookingId = L1SkillId.COOKING_WONDER_DRUG;
                eatCooking(pc, cookingId, time);
            }
            pc.sendPackets(new S_ServerMessage(76, item.getNumberedName(1))); // \f1吃 %0%o。
            pc.Inventory.removeItem(item, 1);
        }

        public static void eatCooking(L1PcInstance pc, int cookingId, int time)
        {
            int cookingType = 0;
            if ((cookingId == L1SkillId.COOKING_1_0_N) || (cookingId == L1SkillId.COOKING_1_0_S))
            { // フローティングアイステーキ
                cookingType = 0;
                pc.addWind(10);
                pc.addWater(10);
                pc.addFire(10);
                pc.addEarth(10);
                pc.sendPackets(new S_OwnCharAttrDef(pc));
            }
            else if ((cookingId == L1SkillId.COOKING_1_1_N) || (cookingId == L1SkillId.COOKING_1_1_S))
            { // ベアーステーキ
                cookingType = 1;
                pc.addMaxHp(30);
                pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.getMaxHp()));
                if (pc.InParty)
                { // パーティー中
                    pc.Party.updateMiniHP(pc);
                }
            }
            else if ((cookingId == L1SkillId.COOKING_1_2_N) || (cookingId == L1SkillId.COOKING_1_2_S))
            { // ナッツ餅
                cookingType = 2;
            }
            else if ((cookingId == L1SkillId.COOKING_1_3_N) || (cookingId == L1SkillId.COOKING_1_3_S))
            { // 蟻脚のチーズ焼き
                cookingType = 3;
                pc.addAc(-1);
                pc.sendPackets(new S_OwnCharStatus(pc));
            }
            else if ((cookingId == L1SkillId.COOKING_1_4_N) || (cookingId == L1SkillId.COOKING_1_4_S))
            { // フルーツサラダ
                cookingType = 4;
                pc.addMaxMp(20);
                pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.getMaxMp()));
            }
            else if ((cookingId == L1SkillId.COOKING_1_5_N) || (cookingId == L1SkillId.COOKING_1_5_S))
            { // フルーツ甘酢あんかけ
                cookingType = 5;
            }
            else if ((cookingId == L1SkillId.COOKING_1_6_N) || (cookingId == L1SkillId.COOKING_1_6_S))
            { // 猪肉の串焼き
                cookingType = 6;
                pc.addMr(5);
                pc.sendPackets(new S_SPMR(pc));
            }
            else if ((cookingId == L1SkillId.COOKING_1_7_N) || (cookingId == L1SkillId.COOKING_1_7_S))
            { // キノコスープ
                cookingType = 7;
            }
            else if ((cookingId == L1SkillId.COOKING_2_0_N) || (cookingId == L1SkillId.COOKING_2_0_S))
            { // キャビアカナッペ
                cookingType = 8;
            }
            else if ((cookingId == L1SkillId.COOKING_2_1_N) || (cookingId == L1SkillId.COOKING_2_1_S))
            { // アリゲーターステーキ
                cookingType = 9;
                pc.addMaxHp(30);
                pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.getMaxHp()));
                if (pc.InParty)
                { // パーティー中
                    pc.Party.updateMiniHP(pc);
                }
                pc.addMaxMp(30);
                pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.getMaxMp()));
            }
            else if ((cookingId == L1SkillId.COOKING_2_2_N) || (cookingId == L1SkillId.COOKING_2_2_S))
            { // タートルドラゴンの菓子
                cookingType = 10;
                pc.addAc(-2);
                pc.sendPackets(new S_OwnCharStatus(pc));
            }
            else if ((cookingId == L1SkillId.COOKING_2_3_N) || (cookingId == L1SkillId.COOKING_2_3_S))
            { // キウィパロット焼き
                cookingType = 11;
            }
            else if ((cookingId == L1SkillId.COOKING_2_4_N) || (cookingId == L1SkillId.COOKING_2_4_S))
            { // スコーピオン焼き
                cookingType = 12;
            }
            else if ((cookingId == L1SkillId.COOKING_2_5_N) || (cookingId == L1SkillId.COOKING_2_5_S))
            { // イレッカドムシチュー
                cookingType = 13;
                pc.addMr(10);
                pc.sendPackets(new S_SPMR(pc));
            }
            else if ((cookingId == L1SkillId.COOKING_2_6_N) || (cookingId == L1SkillId.COOKING_2_6_S))
            { // クモ脚の串焼き
                cookingType = 14;
                pc.addSp(1);
                pc.sendPackets(new S_SPMR(pc));
            }
            else if ((cookingId == L1SkillId.COOKING_2_7_N) || (cookingId == L1SkillId.COOKING_2_7_S))
            { // クラブスープ
                cookingType = 15;
            }
            else if ((cookingId == L1SkillId.COOKING_3_0_N) || (cookingId == L1SkillId.COOKING_3_0_S))
            { // クラスタシアンのハサミ焼き
                cookingType = 16;
            }
            else if ((cookingId == L1SkillId.COOKING_3_1_N) || (cookingId == L1SkillId.COOKING_3_1_S))
            { // グリフォン焼き
                cookingType = 17;
                pc.addMaxHp(50);
                pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.getMaxHp()));
                if (pc.InParty)
                { // パーティー中
                    pc.Party.updateMiniHP(pc);
                }
                pc.addMaxMp(50);
                pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.getMaxMp()));
            }
            else if ((cookingId == L1SkillId.COOKING_3_2_N) || (cookingId == L1SkillId.COOKING_3_2_S))
            { // コカトリスステーキ
                cookingType = 18;
            }
            else if ((cookingId == L1SkillId.COOKING_3_3_N) || (cookingId == L1SkillId.COOKING_3_3_S))
            { // タートルドラゴン焼き
                cookingType = 19;
                pc.addAc(-3);
                pc.sendPackets(new S_OwnCharStatus(pc));
            }
            else if ((cookingId == L1SkillId.COOKING_3_4_N) || (cookingId == L1SkillId.COOKING_3_4_S))
            { // レッサードラゴンの手羽先
                cookingType = 20;
                pc.addMr(15);
                pc.sendPackets(new S_SPMR(pc));
                pc.addWind(10);
                pc.addWater(10);
                pc.addFire(10);
                pc.addEarth(10);
                pc.sendPackets(new S_OwnCharAttrDef(pc));
            }
            else if ((cookingId == L1SkillId.COOKING_3_5_N) || (cookingId == L1SkillId.COOKING_3_5_S))
            { // ドレイク焼き
                cookingType = 21;
                pc.addSp(2);
                pc.sendPackets(new S_SPMR(pc));
            }
            else if ((cookingId == L1SkillId.COOKING_3_6_N) || (cookingId == L1SkillId.COOKING_3_6_S))
            { // 深海魚のシチュー
                cookingType = 22;
                pc.addMaxHp(30);
                pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.getMaxHp()));
                if (pc.InParty)
                { // パーティー中
                    pc.Party.updateMiniHP(pc);
                }
            }
            else if ((cookingId == L1SkillId.COOKING_3_7_N) || (cookingId == L1SkillId.COOKING_3_7_S))
            { // バシリスクの卵スープ
                cookingType = 23;
            }
            else if (cookingId == L1SkillId.COOKING_WONDER_DRUG)
            { // 象牙塔妙藥
                cookingType = 54;
                pc.addHpr(10);
                pc.addMpr(2);
            }
            pc.sendPackets(new S_PacketBox(53, cookingType, time));
            pc.setSkillEffect(cookingId, time * 1000);
            if (((cookingId >= L1SkillId.COOKING_1_0_N) && (cookingId <= L1SkillId.COOKING_1_6_N)) || ((cookingId >= L1SkillId.COOKING_1_0_S) && (cookingId <= L1SkillId.COOKING_1_6_S)) || ((cookingId >= L1SkillId.COOKING_2_0_N) && (cookingId <= L1SkillId.COOKING_2_6_N)) || ((cookingId >= L1SkillId.COOKING_2_0_S) && (cookingId <= L1SkillId.COOKING_2_6_S)) || ((cookingId >= L1SkillId.COOKING_3_0_N) && (cookingId <= L1SkillId.COOKING_3_6_N)) || ((cookingId >= L1SkillId.COOKING_3_0_S) && (cookingId <= L1SkillId.COOKING_3_6_S)))
            {
                pc.CookingId = cookingId;
            }
            else if ((cookingId == L1SkillId.COOKING_1_7_N) || (cookingId == L1SkillId.COOKING_1_7_S) || (cookingId == L1SkillId.COOKING_2_7_N) || (cookingId == L1SkillId.COOKING_2_7_S) || (cookingId == L1SkillId.COOKING_3_7_N) || (cookingId == L1SkillId.COOKING_3_7_S))
            {
                pc.DessertId = cookingId;
            }

            // XXX 空腹ゲージが17%になるため再送信。S_PacketBoxに空腹ゲージ更新のコードが含まれている？
            pc.sendPackets(new S_OwnCharStatus(pc));
        }

    }

}