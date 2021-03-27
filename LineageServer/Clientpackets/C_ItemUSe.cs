using LineageServer.Interfaces;
using LineageServer.Server.DataSources;
using LineageServer.Server.Model;
using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.item;
using LineageServer.Server.Model.item.Action;
using LineageServer.Server.Model.poison;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;
using LineageServer.Server.storage;
using LineageServer.Server.Templates;
using LineageServer.Server.Types;
using LineageServer.Utils;
using System;
using System.Text;
using LineageServer.Server;
using LineageServer.Models;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// TODO: 翻譯，太多了= = 處理收到由客戶端傳來使用道具的封包
    /// </summary>
    class C_ItemUSe : ClientBasePacket
    {

        private const string C_ITEM_USE = "[C] C_ItemUSe";
        private static ILogger _log = Logger.GetLogger(nameof(C_ItemUSe));
        public C_ItemUSe(byte[] abyte0, ClientThread client) : base(abyte0)
        {

            L1PcInstance pc = client.ActiveChar;
            L1PcInventory pcInventory = pc.Inventory as L1PcInventory;
            if ((pc == null) || pc.Ghost || pc.Dead)
            {
                return;
            }

            int itemObjid = ReadD();

            L1ItemInstance l1iteminstance = pc.Inventory.getItem(itemObjid);

            if (l1iteminstance == null)
            {
                return;
            }

            if (l1iteminstance.Item.UseType == -1)
            { // none:不能使用的道具
                pc.sendPackets(new S_ServerMessage(74, l1iteminstance.LogName)); // \f1%0は使用できません。
                return;
            }
            int pcObjid = pc.Id;
            if (pc.Teleport)
            { // 傳送中
                return;
            }
            if (!pc.Map.UsableItem)
            {
                pc.sendPackets(new S_ServerMessage(563)); // \f1ここでは使えません。
                return;
            }
            int itemId;
            try
            {
                itemId = l1iteminstance.Item.ItemId;
            }
            catch (Exception)
            {
                return;
            }
            int l = 0;

            string s = string.Empty;
            int bmapid = 0;
            int btele = 0;
            int blanksc_skillid = 0;
            int spellsc_objid = 0;
            int spellsc_x = 0;
            int spellsc_y = 0;
            int resid = 0;
            int letterCode = 0;
            string letterReceiver = "";
            byte[] letterText = null;
            int cookStatus = 0;
            int cookNo = 0;
            int fishX = 0;
            int fishY = 0;

            int use_type = l1iteminstance.Item.UseType;
            switch (itemId)
            {
                case 40088:
                case 40096:
                case 49308:
                case 140088: // 變形卷軸
                    s = ReadS();
                    break;
                case L1ItemId.SCROLL_OF_ENCHANT_ARMOR:
                case L1ItemId.SCROLL_OF_ENCHANT_WEAPON:
                case L1ItemId.SCROLL_OF_ENCHANT_QUEST_WEAPON:
                case 40077:
                case 40078:
                case 40126:
                case 40098:
                case 40129:
                case 40130:
                case 140129:
                case 140130:
                case L1ItemId.B_SCROLL_OF_ENCHANT_ARMOR:
                case L1ItemId.B_SCROLL_OF_ENCHANT_WEAPON:
                case L1ItemId.C_SCROLL_OF_ENCHANT_ARMOR:
                case L1ItemId.C_SCROLL_OF_ENCHANT_WEAPON:
                case 41029:
                case 40317:
                case 49188:
                case 41036:
                case 41245:
                case 40127:
                case 40128:
                case 41048:
                case 41049:
                case 41050:
                case 41051:
                case 41052:
                case 41053:
                case 41054:
                case 41055:
                case 41056:
                case 41057:
                case 40925:
                case 40926:
                case 40927:
                case 40928:
                case 40929:
                case 40931:
                case 40932:
                case 40933:
                case 40934:
                case 40935:
                case 40936:
                case 40937:
                case 40938:
                case 40939:
                case 40940:
                case 40941:
                case 40942:
                case 40943:
                case 40944:
                case 40945:
                case 40946:
                case 40947:
                case 40948:
                case 40949:
                case 40950:
                case 40951:
                case 40952:
                case 40953:
                case 40954:
                case 40955:
                case 40956:
                case 40957:
                case 40958:
                case 40964:
                case 49092:
                case 49094:
                case 49098:
                case 49317:
                case 49321:
                case 41426:
                case 41427:
                case 40075:
                case 49311:
                case 49312:
                case 49148:
                case 41429:
                case 41430:
                case 41431:
                case 41432:
                case 47041:
                case 47042:
                case 47043:
                case 47044:
                case 47045:
                case 47046:
                case 47048:
                case 47049:
                case 47050:
                case 47051:
                case 47052:
                case 49198:
                case 49199:
                    l = ReadD();
                    break;
                case 140100:
                case 40100:
                case 40099:
                case 40086:
                case 40863: // 瞬間移動卷軸
                    bmapid = ReadH();
                    btele = ReadD();
                    // pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_TELEPORT_UNLOCK, false));
                    break;
                case 40090:
                case 40091:
                case 40092:
                case 40093:
                case 40094: // 空的魔法卷軸(Lv1)～(Lv5)
                    blanksc_skillid = ReadC();
                    break;
                case 40870:
                case 40879: // spell_buff
                    spellsc_objid = ReadD();
                    break;
                case 40089:
                case 140089: // 復活卷軸
                    resid = ReadD();
                    break;
                case 40310:
                case 40311:
                case 40730:
                case 40731:
                case 40732: // 信紙
                    letterCode = ReadH();
                    letterReceiver = ReadS();
                    letterText = ReadByte();
                    break;
                case 41293:
                case 41294: // 釣竿
                    fishX = ReadH();
                    fishY = ReadH();
                    break;
                default:
                    if ((use_type == 30))
                    { // spell_buff
                        spellsc_objid = ReadD();
                    }
                    else if ((use_type == 5) || (use_type == 17))
                    { // spell_long、spell_short
                        spellsc_objid = ReadD();
                        spellsc_x = ReadH();
                        spellsc_y = ReadH();
                    }
                    else if ((itemId >= 41255) && (itemId <= 41259))
                    { // 料理書
                        cookStatus = ReadC();
                        cookNo = ReadC();
                    }
                    else
                    {
                        l = ReadC();
                    }
                    break;
            }

            if (pc.CurrentHp > 0)
            {
                int delay_id = 0;
                if (l1iteminstance.Item.Type2 == 0)
                { // 種別：その他のアイテム
                    delay_id = ((L1EtcItem)l1iteminstance.Item).get_delayid();
                }
                if (delay_id != 0)
                { // ディレイ設定あり
                    if (pc.hasItemDelay(delay_id) == true)
                    {
                        return;
                    }
                }

                // 再使用チェック
                bool isDelayEffect = false;
                if (l1iteminstance.Item.Type2 == 0)
                { // etcitem
                    int delayEffect = ((L1EtcItem)l1iteminstance.Item).get_delayEffect();
                    if (delayEffect > 0)
                    {
                        isDelayEffect = true;
                        DateTime lastUsed = l1iteminstance.LastUsed;
                        if (lastUsed != null)
                        {
                            DateTime cal = new DateTime();
                            if ((cal - lastUsed).TotalMilliseconds / 1000 <= delayEffect)
                            {
                                // \f1沒有任何事情發生。
                                pc.sendPackets(new S_ServerMessage(79));
                                return;
                            }
                        }
                    }
                }

                L1ItemInstance l1iteminstance1 = pc.Inventory.getItem(l);
                if ((itemId == 40077) || (itemId == L1ItemId.SCROLL_OF_ENCHANT_WEAPON) || (itemId == L1ItemId.SCROLL_OF_ENCHANT_QUEST_WEAPON) || (itemId == 40130) || (itemId == 140130) || (itemId == L1ItemId.B_SCROLL_OF_ENCHANT_WEAPON) || (itemId == L1ItemId.C_SCROLL_OF_ENCHANT_WEAPON) || (itemId == 40128))
                { // 對武器施法的卷軸
                    Enchant.scrollOfEnchantWeapon(pc, l1iteminstance, l1iteminstance1, client);
                }
                else if (itemId == 49312)
                { // 象牙塔對武器施法的卷軸
                    Enchant.scrollOfEnchantWeaponIvoryTower(pc, l1iteminstance, l1iteminstance1, client);
                }
                else if ((itemId == 41429) || (itemId == 41430) || (itemId == 41431) || (itemId == 41432))
                { // 武器屬性強化卷軸
                    Enchant.scrollOfEnchantWeaponAttr(pc, l1iteminstance, l1iteminstance1, client);
                }
                else if ((itemId == 40078) || (itemId == L1ItemId.SCROLL_OF_ENCHANT_ARMOR) || (itemId == 40129) || (itemId == 140129) || (itemId == L1ItemId.B_SCROLL_OF_ENCHANT_ARMOR) || (itemId == L1ItemId.C_SCROLL_OF_ENCHANT_ARMOR) || (itemId == 40127))
                { // 對盔甲施法的卷軸
                    Enchant.scrollOfEnchantArmor(pc, l1iteminstance, l1iteminstance1, client);
                }
                else if (itemId == 49311)
                { // 象牙塔對盔甲施法的卷軸
                    Enchant.scrollOfEnchantArmorIvoryTower(pc, l1iteminstance, l1iteminstance1, client);
                }
                else if (l1iteminstance.Item.Type2 == 0)
                { // 道具類
                    int item_minlvl = ((L1EtcItem)l1iteminstance.Item).MinLevel;
                    int item_maxlvl = ((L1EtcItem)l1iteminstance.Item).MaxLevel;
                    if ((item_minlvl != 0) && (item_minlvl > pc.Level) && !pc.Gm)
                    {
                        pc.sendPackets(new S_ServerMessage(318, item_minlvl.ToString())); // 等級 %0以上才可使用此道具。
                        return;
                    }
                    else if ((item_maxlvl != 0) && (item_maxlvl < pc.Level) && !pc.Gm)
                    {
                        pc.sendPackets(new S_PacketBox(S_PacketBox.MSG_LEVEL_OVER, item_maxlvl)); // 等級%d以下才能使用此道具。
                        return;
                    }

                    if (((itemId == 40576) && !pc.Elf) || ((itemId == 40577) && !pc.Wizard) || ((itemId == 40578) && !pc.Knight))
                    { // 魂の結晶の破片（赤）
                        pc.sendPackets(new S_ServerMessage(264)); // \f1你的職業無法使用此道具。
                        return;
                    }

                    if (l1iteminstance.Item.Type == 0)
                    { // アロー
                        pcInventory.Arrow = l1iteminstance.Item.ItemId;
                        pc.sendPackets(new S_ServerMessage(452, l1iteminstance.LogName)); // %0%s 被選擇了。
                    }
                    else if (l1iteminstance.Item.Type == 15)
                    { // スティング
                        pcInventory.Sting = l1iteminstance.Item.ItemId;
                        pc.sendPackets(new S_ServerMessage(452, l1iteminstance.LogName));
                    }
                    else if (l1iteminstance.Item.Type == 16)
                    { // treasure_box
                        L1TreasureBox box = L1TreasureBox.get(itemId);

                        if (box != null)
                        {
                            if (box.open(pc))
                            {
                                L1EtcItem temp = (L1EtcItem)l1iteminstance.Item;
                                if (temp.get_delayEffect() > 0)
                                {
                                    // 有限制再次使用的時間且可堆疊的道具
                                    if (l1iteminstance.Stackable)
                                    {
                                        if (l1iteminstance.Count > 1)
                                        {
                                            isDelayEffect = true;
                                        }
                                        pc.Inventory.removeItem(l1iteminstance.Id, 1);
                                    }
                                    else
                                    {
                                        isDelayEffect = true;
                                    }
                                }
                                else
                                {
                                    pc.Inventory.removeItem(l1iteminstance.Id, 1);
                                }
                            }
                        }
                    }
                    else if (l1iteminstance.Item.Type == 2)
                    { // light系アイテム
                        if ((l1iteminstance.RemainingTime <= 0) && (itemId != 40004))
                        {
                            return;
                        }
                        if (l1iteminstance.NowLighting)
                        {
                            l1iteminstance.NowLighting = false;
                            pc.turnOnOffLight();
                        }
                        else
                        {
                            l1iteminstance.NowLighting = true;
                            pc.turnOnOffLight();
                        }
                        pc.sendPackets(new S_ItemName(l1iteminstance));
                    }
                    else if (l1iteminstance.Item.Type == 17)
                    { // 魔法娃娃類
                        MagicDoll.useMagicDoll(pc, itemId, itemObjid);
                    }
                    else if (l1iteminstance.Item.Type == 18)
                    { // 家具類
                        if (itemId == 41401)
                        { // 移除家俱魔杖
                            FurnitureItem.useFurnitureRemovalWand(pc, spellsc_objid, l1iteminstance);
                        }
                        else
                        {
                            FurnitureItem.useFurnitureItem(pc, itemId, itemObjid);
                        }
                    }
                    else if (itemId == 47103)
                    { // 新鮮的餌
                        pc.sendPackets(new S_ServerMessage(452, l1iteminstance.LogName));
                    }
                    else if (itemId == 40003)
                    { // ランタン オイル
                        foreach (L1ItemInstance lightItem in pc.Inventory.Items)
                        {
                            if (lightItem.Item.ItemId == 40002)
                            {
                                lightItem.RemainingTime = l1iteminstance.Item.LightFuel;
                                pc.sendPackets(new S_ItemName(lightItem));
                                pc.sendPackets(new S_ServerMessage(230)); // ランタンにオイルを注ぎました。
                                break;
                            }
                        }
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if (itemId == 43000)
                    { // 復活のポーション（Lv99キャラのみが使用可能/Lv1に戻る効果）
                        pc.Exp = 1;
                        pc.resetLevel();
                        pc.BonusStats = 0;
                        pc.sendPackets(new S_SkillSound(pcObjid, 191));
                        pc.broadcastPacket(new S_SkillSound(pcObjid, 191));
                        pc.sendPackets(new S_OwnCharStatus(pc));
                        pc.Inventory.removeItem(l1iteminstance, 1);
                        pc.sendPackets(new S_ServerMessage(822)); // 独自アイテムですので、メッセージは適当です。
                        pc.Save(); // DBにキャラクター情報を書き込む

                        // 處理新手保護系統(遭遇的守護)狀態資料的變動
                        pc.checkNoviceType();
                    }
                    else if (itemId == 40033)
                    { // エリクサー:腕力
                        if (pc.BaseStr < Config.BONUS_STATS3 && pc.ElixirStats < Config.BONUS_STATS2)
                        { // 調整能力值上限
                            pc.addBaseStr((sbyte)1); // 素のSTR値に+1
                            pc.ElixirStats = pc.ElixirStats + 1;
                            pc.Inventory.removeItem(l1iteminstance, 1);
                            pc.sendPackets(new S_OwnCharStatus2(pc, 0));
                            pc.Save(); // DBにキャラクター情報を書き込む
                        }
                        else
                        {
                            if (pc.BaseStr >= Config.BONUS_STATS3)
                            {
                                pc.sendPackets(new S_SystemMessage("能力值Str" + Config.BONUS_STATS3 + "以後不能喝萬能藥! "));
                            }
                            if (pc.ElixirStats >= Config.BONUS_STATS2)
                            {
                                pc.sendPackets(new S_SystemMessage("萬能藥只能喝" + Config.BONUS_STATS2 + "瓶"));
                            }
                        }
                    }
                    else if (itemId == 40034)
                    { // エリクサー:体力
                        if (pc.BaseCon < Config.BONUS_STATS3 && pc.ElixirStats < Config.BONUS_STATS2)
                        { // 調整能力值上限
                            pc.addBaseCon((sbyte)1); // 素のCON値に+1
                            pc.ElixirStats = pc.ElixirStats + 1;
                            pc.Inventory.removeItem(l1iteminstance, 1);
                            pc.sendPackets(new S_OwnCharStatus2(pc, 0));
                            pc.Save(); // DBにキャラクター情報を書き込む
                        }
                        else
                        {
                            if (pc.BaseCon >= Config.BONUS_STATS3)
                            {
                                pc.sendPackets(new S_SystemMessage("Con能力值" + Config.BONUS_STATS3 + "以後不能喝萬能藥! "));
                            }
                            if (pc.ElixirStats >= Config.BONUS_STATS2)
                            {
                                pc.sendPackets(new S_SystemMessage("萬能藥只能喝" + Config.BONUS_STATS2 + "瓶"));
                            }
                        }
                    }
                    else if (itemId == 40035)
                    { // エリクサー:機敏
                        if (pc.BaseDex < Config.BONUS_STATS3 && pc.ElixirStats < Config.BONUS_STATS2)
                        { // 調整能力值上限
                            pc.addBaseDex((sbyte)1); // 素のDEX値に+1
                            pc.resetBaseAc();
                            pc.ElixirStats = pc.ElixirStats + 1;
                            pc.Inventory.removeItem(l1iteminstance, 1);
                            pc.sendPackets(new S_OwnCharStatus2(pc, 0));
                            pc.Save(); // DBにキャラクター情報を書き込む
                        }
                        else
                        {
                            if (pc.BaseInt >= Config.BONUS_STATS3)
                            {
                                pc.sendPackets(new S_SystemMessage("Int能力值" + Config.BONUS_STATS3 + "以後不能喝萬能藥! "));
                            }
                            if (pc.ElixirStats >= Config.BONUS_STATS2)
                            {
                                pc.sendPackets(new S_SystemMessage("萬能藥只能喝" + Config.BONUS_STATS2 + "瓶"));
                            }
                        }
                    }
                    else if (itemId == 40036)
                    { // エリクサー:知力
                        if (pc.BaseInt < Config.BONUS_STATS3 && pc.ElixirStats < Config.BONUS_STATS2)
                        { // 調整能力值上限
                            pc.addBaseInt((sbyte)1); // 素のINT値に+1
                            pc.ElixirStats = pc.ElixirStats + 1;
                            pc.Inventory.removeItem(l1iteminstance, 1);
                            pc.sendPackets(new S_OwnCharStatus2(pc, 0));
                            pc.Save(); // DBにキャラクター情報を書き込む
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(481)); // \f1一つの能力値の最大値は25です。他の能力値を選択してください。
                        }
                    }
                    else if (itemId == 40037)
                    { // エリクサー:精神
                        if (pc.BaseWis < Config.BONUS_STATS3 && pc.ElixirStats < Config.BONUS_STATS2)
                        { // 調整能力值上限
                            pc.addBaseWis((sbyte)1); // 素のWIS値に+1
                            pc.resetBaseMr();
                            pc.ElixirStats = pc.ElixirStats + 1;
                            pc.Inventory.removeItem(l1iteminstance, 1);
                            pc.sendPackets(new S_OwnCharStatus2(pc, 0));
                            pc.Save(); // DBにキャラクター情報を書き込む
                        }
                        else
                        {
                            if (pc.BaseWis >= Config.BONUS_STATS3)
                            {
                                pc.sendPackets(new S_SystemMessage("Wis能力值" + Config.BONUS_STATS3 + "以後不能喝萬能藥! "));
                            }
                            if (pc.ElixirStats >= Config.BONUS_STATS2)
                            {
                                pc.sendPackets(new S_SystemMessage("萬能藥只能喝" + Config.BONUS_STATS2 + "瓶"));
                            }
                        }
                    }
                    else if (itemId == 40038)
                    { // エリクサー:魅力
                        if (pc.BaseCha < Config.BONUS_STATS3 && pc.ElixirStats < Config.BONUS_STATS2)
                        { // 調整能力值上限
                            pc.addBaseCha((sbyte)1); // 素のCHA値に+1
                            pc.ElixirStats = pc.ElixirStats + 1;
                            pc.Inventory.removeItem(l1iteminstance, 1);
                            pc.sendPackets(new S_OwnCharStatus2(pc, 0));
                            pc.Save(); // DBにキャラクター情報を書き込む
                        }
                        else
                        {
                            if (pc.BaseCha >= Config.BONUS_STATS3)
                            {
                                pc.sendPackets(new S_SystemMessage("Cha能力值" + Config.BONUS_STATS3 + "以後不能喝萬能藥! "));
                            }
                            if (pc.ElixirStats >= Config.BONUS_STATS2)
                            {
                                pc.sendPackets(new S_SystemMessage("萬能藥只能喝" + Config.BONUS_STATS2 + "瓶"));
                            }
                        }
                    }
                    // 治癒藥水、濃縮體力恢復劑、象牙塔治癒藥水
                    else if ((itemId == L1ItemId.POTION_OF_HEALING) || (itemId == L1ItemId.CONDENSED_POTION_OF_HEALING) || (itemId == 40029))
                    {
                        Potion.UseHeallingPotion(pc, l1iteminstance, 15, 189);
                    }
                    else if (itemId == 40022)
                    { // 古代體力恢復劑
                        Potion.UseHeallingPotion(pc, l1iteminstance, 20, 189);
                    }
                    else if ((itemId == L1ItemId.POTION_OF_EXTRA_HEALING) || (itemId == L1ItemId.CONDENSED_POTION_OF_EXTRA_HEALING))
                    {
                        Potion.UseHeallingPotion(pc, l1iteminstance, 45, 194);
                    }
                    else if (itemId == 40023)
                    { // 古代強力體力恢復劑
                        Potion.UseHeallingPotion(pc, l1iteminstance, 30, 194);
                    }
                    else if ((itemId == L1ItemId.POTION_OF_GREATER_HEALING) || (itemId == L1ItemId.CONDENSED_POTION_OF_GREATER_HEALING) || (itemId == 47114) || (itemId == 49137) || (itemId == 41141))
                    {
                        Potion.UseHeallingPotion(pc, l1iteminstance, 75, 197);
                    }
                    else if (itemId == 40024)
                    { // 古代終極體力恢復劑
                        Potion.UseHeallingPotion(pc, l1iteminstance, 55, 197);
                    }
                    else if (itemId == 40506)
                    { // 安特的水果
                        Potion.UseHeallingPotion(pc, l1iteminstance, 70, 197);
                    }
                    else if ((itemId == 40026) || (itemId == 40027) || (itemId == 40028))
                    { // 香蕉汁、橘子汁、蘋果汁
                        Potion.UseHeallingPotion(pc, l1iteminstance, 25, 189);
                    }
                    else if ((itemId == 40058) || (itemId == 49268) || (itemId == 49269))
                    { // 煙燻的麵包屑、愛瑪伊的畫像、伊森之畫像
                        Potion.UseHeallingPotion(pc, l1iteminstance, 30, 189);
                    }
                    else if (itemId == 40071)
                    { // 烤焦的麵包屑
                        Potion.UseHeallingPotion(pc, l1iteminstance, 70, 197);
                    }
                    else if (itemId == 40734)
                    { // 信賴貨幣
                        Potion.UseHeallingPotion(pc, l1iteminstance, 50, 189);
                    }
                    else if (itemId == L1ItemId.B_POTION_OF_HEALING)
                    { // 受祝福的 治癒藥水
                        Potion.UseHeallingPotion(pc, l1iteminstance, 25, 189);
                    }
                    else if (itemId == L1ItemId.C_POTION_OF_HEALING)
                    { // 受咀咒的 治癒藥水
                        Potion.UseHeallingPotion(pc, l1iteminstance, 10, 189);
                    }
                    else if (itemId == L1ItemId.B_POTION_OF_EXTRA_HEALING)
                    { // 受祝福的 強力治癒藥水
                        Potion.UseHeallingPotion(pc, l1iteminstance, 55, 194);
                    }
                    else if (itemId == L1ItemId.B_POTION_OF_GREATER_HEALING)
                    { // 受祝福的 終極治癒藥水
                        Potion.UseHeallingPotion(pc, l1iteminstance, 85, 197);
                    }
                    else if (itemId == 140506)
                    { // 受祝福的 安特的水果
                        Potion.UseHeallingPotion(pc, l1iteminstance, 80, 197);
                    }
                    else if (itemId == 40043)
                    { // 兔子的肝
                        Potion.UseHeallingPotion(pc, l1iteminstance, 600, 189);
                    }
                    else if (itemId == 41403)
                    { // 庫傑的糧食
                        Potion.UseHeallingPotion(pc, l1iteminstance, 300, 189);
                    }
                    else if ((itemId >= 41417) && (itemId <= 41421))
                    { // 日本「亞丁的夏天」活動道具 - 刨冰
                        Potion.UseHeallingPotion(pc, l1iteminstance, 90, 197);
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if (itemId == 41337)
                    { // 受祝福的五穀麵包
                        Potion.UseHeallingPotion(pc, l1iteminstance, 85, 197);
                    }
                    else if (itemId == 40858)
                    { // liquor（酒）
                        pc.Drink = true;
                        pc.sendPackets(new S_Liquor(pc.Id, 1));
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if ((itemId == L1ItemId.POTION_OF_CURE_POISON) || (itemId == 40507))
                    { // 翡翠藥水
                        if (pc.hasSkillEffect(L1SkillId.DECAY_POTION))
                        { // 藥水霜化術狀態
                            pc.sendPackets(new S_ServerMessage(698)); // 喉嚨灼熱，無法喝東西。
                            return;
                        }
                        else
                        {
                            pc.sendPackets(new S_SkillSound(pc.Id, 192));
                            pc.broadcastPacket(new S_SkillSound(pc.Id, 192));
                            if (itemId == L1ItemId.POTION_OF_CURE_POISON)
                            {
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                            else if (itemId == 40507)
                            {
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }

                            pc.curePoison();
                        }
                    }
                    else if ((itemId == L1ItemId.POTION_OF_HASTE_SELF) || (itemId == L1ItemId.B_POTION_OF_HASTE_SELF) || (itemId == 40018) || (itemId == 140018) || (itemId == 40039) || (itemId == 40040) || (itemId == 40030) || (itemId == 41338) || (itemId == 41261) || (itemId == 41262) || (itemId == 41268) || (itemId == 41269) || (itemId == 41271) || (itemId == 41272) || (itemId == 41273) || (itemId == 41342) || (itemId == 49302) || (itemId == 49140))
                    {
                        Potion.useGreenPotion(pc, l1iteminstance, itemId);
                    }
                    else if ((itemId == L1ItemId.POTION_OF_EMOTION_BRAVERY) || (itemId == L1ItemId.B_POTION_OF_EMOTION_BRAVERY) || (itemId == L1ItemId.POTION_OF_REINFORCED_CASE) || (itemId == L1ItemId.W_POTION_OF_EMOTION_BRAVERY))
                    { // 福利勇敢藥水
                        if (pc.Knight)
                        { // 騎士
                            Potion.Brave(pc, l1iteminstance, itemId);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                    }
                    else if (itemId == L1ItemId.FORBIDDEN_FRUIT)
                    { // 生命之樹果實
                        if (pc.DragonKnight || pc.Illusionist)
                        { // 龍騎士、幻術師
                            Potion.Brave(pc, l1iteminstance, itemId);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                    }
                    else if ((itemId == L1ItemId.ELVEN_WAFER) || (itemId == L1ItemId.B_ELVEN_WAFER) || (itemId == L1ItemId.W_POTION_OF_FOREST))
                    { // 福利森林藥水
                        if (pc.Elf)
                        { // 妖精
                            Potion.Brave(pc, l1iteminstance, itemId);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                    }
                    else if (itemId == L1ItemId.DEVILS_BLOOD)
                    { // 惡魔之血
                        if (pc.Crown)
                        { // 王族
                            Potion.Brave(pc, l1iteminstance, itemId);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                    }
                    else if (itemId == L1ItemId.COIN_OF_REPUTATION)
                    { // 名譽貨幣
                        if (!pc.DragonKnight && !pc.Illusionist)
                        { // 龍騎士與幻術師無法使用
                            Potion.Brave(pc, l1iteminstance, itemId);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                    }
                    else if (itemId == L1ItemId.CHOCOLATE_CAKE)
                    { // 巧克力蛋糕
                        Potion.ThirdSpeed(pc, l1iteminstance, 600);
                    }
                    else if ((itemId >= L1ItemId.POTION_OF_EXP_150) && (itemId <= L1ItemId.SCROLL_FOR_ENCHANTING_BATTLE))
                    { // 150%神力藥水 ~ 強化戰鬥卷軸
                        Effect.useEffectItem(pc, l1iteminstance);
                    }
                    else if ((itemId == 40066) || (itemId == 41413))
                    { // 年糕、月餅
                        Potion.UseMpPotion(pc, l1iteminstance, 7, 6);
                    }
                    else if ((itemId == 40067) || (itemId == 41414))
                    { // 艾草年糕、福月餅
                        Potion.UseMpPotion(pc, l1iteminstance, 15, 16);
                    }
                    else if (itemId == 40735)
                    { // 勇氣貨幣
                        Potion.UseMpPotion(pc, l1iteminstance, 60, 0);
                    }
                    else if ((itemId == 40042) || (itemId == 41142))
                    { // 精神藥水、神秘的魔力藥水
                        Potion.UseMpPotion(pc, l1iteminstance, 50, 0);
                    }
                    else if (itemId == 41404)
                    { // 庫傑的靈藥
                        Potion.UseMpPotion(pc, l1iteminstance, 80, 21);
                    }
                    else if (itemId == 41412)
                    { // 金粽子
                        Potion.UseMpPotion(pc, l1iteminstance, 5, 16);
                    }
                    else if ((itemId == 40032) || (itemId == 40041) || (itemId == 41344) || (itemId == 49303))
                    { // 水中的水、福利呼吸藥水
                        Potion.useBlessOfEva(pc, l1iteminstance, itemId);
                    }
                    else if ((itemId == L1ItemId.POTION_OF_MANA) || (itemId == L1ItemId.B_POTION_OF_MANA || (itemId == 40736) || (itemId == 49306)))
                    { // 智慧貨幣、福利藍色藥水
                        Potion.useBluePotion(pc, l1iteminstance, itemId);
                    }
                    else if ((itemId == L1ItemId.POTION_OF_EMOTION_WISDOM) || (itemId == L1ItemId.B_POTION_OF_EMOTION_WISDOM) || (itemId == 49307))
                    { // 福利慎重藥水
                        if (pc.Wizard)
                        { // 法師
                            Potion.useWisdomPotion(pc, l1iteminstance, itemId);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79));
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                    }
                    else if (itemId == L1ItemId.POTION_OF_BLINDNESS)
                    { // 黑色藥水
                        Potion.useBlindPotion(pc, l1iteminstance);
                    }
                    else if ((itemId == 40088) || (itemId == 40096) || (itemId == 49308) || (itemId == 140088))
                    { // 變形卷軸、福利變形藥水
                        if (usePolyScroll(pc, itemId, s))
                        {
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(181)); // \f1無法變成你指定的怪物。
                        }
                    }
                    else if ((itemId == 41154) || (itemId == 41155) || (itemId == 41156) || (itemId == 41157) || (itemId == 49220))
                    { // 妖魔密使變形卷軸
                        usePolyScale(pc, itemId);
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if ((itemId == 41143) || (itemId == 41144) || (itemId == 41145) || (itemId == 49149) || (itemId == 49150) || (itemId == 49151) || (itemId == 49152) || (itemId == 49153) || (itemId == 49154) || (itemId == 49155) || (itemId == 49139))
                    {
                        usePolyPotion(pc, itemId);
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if (itemId == 40317)
                    { // 砥石
                      // 武器か防具の場合のみ
                        if ((l1iteminstance1.Item.Type2 != 0) && (l1iteminstance1.get_durability() > 0))
                        {
                            string msg0;
                            pc.Inventory.recoveryDamage(l1iteminstance1);
                            msg0 = l1iteminstance1.LogName;
                            if (l1iteminstance1.get_durability() == 0)
                            {
                                pc.sendPackets(new S_ServerMessage(464, msg0)); // %0%sは新品同様の状態になりました。
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(463, msg0)); // %0の状態が良くなりました。
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if (itemId >= 47017 && itemId <= 47023)
                    { // 龍之魔眼
                        Effect.useEffectItem(pc, l1iteminstance);
                    }
                    else if (itemId == 47041)
                    { // 地龍之精緻魔眼、水龍之精緻魔眼
                        if (l1iteminstance1.Item.ItemId == 47042)
                        { // 水龍之精緻魔眼
                            pc.Inventory.consumeItem(47041, 1);
                            pc.Inventory.consumeItem(47042, 1);
                            createNewItem(pc, 47021, 1); // 誕生之魔眼
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 47042)
                    { // 水龍之精緻魔眼
                        if (l1iteminstance1.Item.ItemId == 47041)
                        { // 地龍之精緻魔眼、水龍之精緻魔眼
                            pc.Inventory.consumeItem(47041, 1);
                            pc.Inventory.consumeItem(47042, 1);
                            createNewItem(pc, 47021, 1); // 誕生之魔眼
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 47043)
                    { // 風龍之精緻魔眼
                        if (l1iteminstance1.Item.ItemId == 47045)
                        { // 誕生之精緻魔眼
                            pc.Inventory.consumeItem(47043, 1);
                            pc.Inventory.consumeItem(47045, 1);
                            createNewItem(pc, 47022, 1); // 形象之魔眼
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 47045)
                    { // 誕生之精緻魔眼
                        if (l1iteminstance1.Item.ItemId == 47043)
                        { // 風龍之精緻魔眼
                            pc.Inventory.consumeItem(47043, 1);
                            pc.Inventory.consumeItem(47045, 1);
                            createNewItem(pc, 47022, 1); // 形象之魔眼
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 47044)
                    { // 火龍之精緻魔眼
                        if (l1iteminstance1.Item.ItemId == 47046)
                        { // 形象之精緻魔眼
                            pc.Inventory.consumeItem(47044, 1);
                            pc.Inventory.consumeItem(47046, 1);
                            createNewItem(pc, 47023, 1); // 生命之魔眼
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 47046)
                    { // 形象之精緻魔眼
                        if (l1iteminstance1.Item.ItemId == 47044)
                        { // 火龍之精緻魔眼
                            pc.Inventory.consumeItem(47044, 1);
                            pc.Inventory.consumeItem(47046, 1);
                            createNewItem(pc, 47023, 1); // 生命之魔眼
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId >= 47049 && itemId <= 47052)
                    { // XX附魔轉換卷軸
                        if (l1iteminstance1.ItemId >= 47053 && l1iteminstance1.ItemId <= 47062)
                        { // 附魔石 ~ 9階附魔石
                            int type = (itemId - 47048) * 10; // type = 10,20,30,40
                            int rnd = RandomHelper.Next(100) + 1;
                            if (Config.MAGIC_STONE_TYPE < rnd)
                            {
                                int newItem = l1iteminstance1.ItemId + type; // 附魔石(近戰) ~ 9階附魔石(近戰)
                                L1Item template = ItemTable.Instance.getTemplate(newItem);
                                if (template == null)
                                {
                                    pc.sendPackets(new S_ServerMessage(79));
                                }
                                createNewItem(pc, newItem, 1); // 獲得附魔石(XX)
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(1411, l1iteminstance1.Name)); // 對\f1%0附加魔法失敗。
                            }
                            pc.Inventory.removeItem(l1iteminstance1, 1); // 刪除 - 附魔石
                            pc.Inventory.removeItem(l1iteminstance, 1); // 刪除 - 附魔轉換卷軸
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79));
                        }
                    }
                    else if (itemId == 47048)
                    { // 附魔強化卷軸
                        int item_id = l1iteminstance1.ItemId;
                        if ((item_id < 47053) || (item_id > 47102) || (item_id == 47062) || (item_id == 47072) || (item_id == 47082) || (item_id == 47092) || (item_id == 47102))
                        {
                            pc.sendPackets(new S_ServerMessage(79));
                            return;
                        }

                        int rnd = RandomHelper.Next(100) + 1;
                        if (Config.MAGIC_STONE_LEVEL < rnd || (item_id >= 47053 && item_id <= 47056) || (item_id >= 47063 && item_id <= 47066) || (item_id >= 47073 && item_id <= 47076) || (item_id >= 47083 && item_id <= 47086) || (item_id >= 47093 && item_id <= 47096))
                        {
                            int newItem = l1iteminstance1.ItemId + 1; // X 階附魔石 -> X+1 階附魔石
                            L1Item template = ItemTable.Instance.getTemplate(newItem);
                            if (template == null)
                            {
                                pc.sendPackets(new S_ServerMessage(79));
                                return;
                            }
                            pc.sendPackets(new S_ServerMessage(1410, l1iteminstance1.Name)); // 對\f1%0附加強大的魔法力量成功。

                            l1iteminstance1.Item = template;
                            pc.Inventory.updateItem(l1iteminstance1, L1PcInventory.COL_ITEMID);
                            pcInventory.saveItem(l1iteminstance1, L1PcInventory.COL_ITEMID);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(1411, l1iteminstance1.Name)); // 對\f1%0附加魔法失敗。
                            pc.Inventory.removeItem(l1iteminstance1, 1);
                        }
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if ((itemId >= 47064 && itemId <= 47067) || (itemId >= 47074 && itemId <= 47077) || (itemId >= 47084 && itemId <= 47087) || (itemId >= 47094 && itemId <= 47097))
                    { // 1 ~ 4 階附魔石(近戰)(遠攻)(恢復)(防禦)
                        if (pc.Inventory.consumeItem(41246, 30))
                        {
                            Effect.useEffectItem(pc, l1iteminstance);
                        }
                        else
                        {
                            isDelayEffect = false;
                            pc.sendPackets(new S_ServerMessage(337, "$5240"));
                        }
                    }
                    else if ((itemId == 47068) || (itemId == 47069) || (itemId == 47078) || (itemId == 47079) || (itemId == 47088) || (itemId == 47089) || (itemId == 47098) || (itemId == 47099))
                    { // 5 ~ 6階附魔石(近戰)(遠攻)(恢復)(防禦)
                        if (pc.Inventory.consumeItem(41246, 60))
                        {
                            Effect.useEffectItem(pc, l1iteminstance);
                        }
                        else
                        {
                            isDelayEffect = false;
                            pc.sendPackets(new S_ServerMessage(337, "$5240"));
                        }
                    }
                    else if ((itemId == 47070) || (itemId == 47080) || (itemId == 47090) || (itemId == 47100))
                    { // 7階附魔石(近戰)(遠攻)(恢復)(防禦)
                        if (pc.Inventory.consumeItem(41246, 100))
                        {
                            Effect.useEffectItem(pc, l1iteminstance);
                        }
                        else
                        {
                            isDelayEffect = false;
                            pc.sendPackets(new S_ServerMessage(337, "$5240"));
                        }
                    }
                    else if ((itemId == 47071) || (itemId == 47081) || (itemId == 47091) || (itemId == 47101))
                    { // 8階附魔石(近戰)(遠攻)(恢復)(防禦)
                        if (pc.Inventory.consumeItem(41246, 200))
                        {
                            Effect.useEffectItem(pc, l1iteminstance);
                        }
                        else
                        {
                            isDelayEffect = false;
                            pc.sendPackets(new S_ServerMessage(337, "$5240"));
                        }
                    }
                    else if ((itemId == 47072) || (itemId == 47082) || (itemId == 47092) || (itemId == 47102))
                    { // 9階附魔石(近戰)(遠攻)(恢復)(防禦)
                        if (pc.Inventory.consumeItem(41246, 300))
                        {
                            Effect.useEffectItem(pc, l1iteminstance);
                        }
                        else
                        {
                            isDelayEffect = false;
                            pc.sendPackets(new S_ServerMessage(337, "$5240"));
                        }
                    }
                    else if ((itemId == 40097) || (itemId == 40119) || (itemId == 140119) || (itemId == 140329))
                    { // 解除咀咒的卷軸、原住民圖騰
                        foreach (L1ItemInstance eachItem in pc.Inventory.Items)
                        {
                            if ((eachItem.Item.Bless != 2) && (eachItem.Item.Bless != 130))
                            {
                                continue;
                            }
                            if (!eachItem.Equipped && ((itemId == 40119) || (itemId == 40097)))
                            {
                                // 裝備中才可解除咀咒
                                continue;
                            }
                            int id_normal = eachItem.ItemId - 200000;
                            L1Item template = ItemTable.Instance.getTemplate(id_normal);
                            if (template == null)
                            {
                                continue;
                            }
                            if (eachItem.Bless == 130)
                            { // 封印中的咀咒裝備
                                eachItem.Bless = 129;
                            }
                            else
                            { // 未封印的咀咒裝備
                                eachItem.Bless = 1;
                            }
                            if (pc.Inventory.checkItem(id_normal) && template.Stackable)
                            {
                                pc.Inventory.storeItem(id_normal, eachItem.Count);
                                pc.Inventory.removeItem(eachItem, eachItem.Count);
                            }
                            else
                            {
                                eachItem.Item = template;
                                pc.Inventory.updateItem(eachItem, L1PcInventory.COL_ITEMID);
                                pcInventory.saveItem(eachItem, L1PcInventory.COL_ITEMID);
                            }
                        }
                        pc.Inventory.removeItem(l1iteminstance, 1);
                        pc.sendPackets(new S_ServerMessage(155)); // \f1誰かが助けてくれたようです。
                    }
                    else if ((itemId == 40126) || (itemId == 40098))
                    { // 確認スクロール
                        if (!l1iteminstance1.Identified)
                        {
                            l1iteminstance1.Identified = true;
                            pc.Inventory.updateItem(l1iteminstance1, L1PcInventory.COL_IS_ID);
                        }
                        pc.sendPackets(new S_IdentifyDesc(l1iteminstance1));
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if (itemId == 41036)
                    { // 糊
                        int diaryId = l1iteminstance1.Item.ItemId;
                        if ((diaryId >= 41038) && (41047 >= diaryId))
                        {
                            if ((RandomHelper.Next(99) + 1) <= Config.CREATE_CHANCE_DIARY)
                            {
                                createNewItem(pc, diaryId + 10, 1);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(158, l1iteminstance1.Name)); // \f1%0が蒸発してなくなりました。
                            }
                            pc.Inventory.removeItem(l1iteminstance1, 1);
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if ((itemId >= 41048) && (41055 >= itemId))
                    {
                        // 糊付けされた航海日誌ページ：１～８ページ
                        int logbookId = l1iteminstance1.Item.ItemId;
                        if (logbookId == (itemId + 8034))
                        {
                            createNewItem(pc, logbookId + 2, 1);
                            pc.Inventory.removeItem(l1iteminstance1, 1);
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if ((itemId == 41056) || (itemId == 41057))
                    {
                        // 糊付けされた航海日誌ページ：９，１０ページ
                        int logbookId = l1iteminstance1.Item.ItemId;
                        if (logbookId == (itemId + 8034))
                        {
                            createNewItem(pc, 41058, 1);
                            pc.Inventory.removeItem(l1iteminstance1, 1);
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 40925)
                    { // 浄化のポーション
                        int earingId = l1iteminstance1.Item.ItemId;
                        if ((earingId >= 40987) && (40989 >= earingId))
                        { // 呪われたブラックイアリング
                            if (RandomHelper.Next(100) < Config.CREATE_CHANCE_RECOLLECTION)
                            {
                                createNewItem(pc, earingId + 186, 1);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(158, l1iteminstance1.Name)); // \f1%0が蒸発してなくなりました。
                            }
                            pc.Inventory.removeItem(l1iteminstance1, 1);
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if ((itemId >= 40926) && (40929 >= itemId))
                    { // 一～四階神秘藥水
                        int earing2Id = l1iteminstance1.Item.ItemId;
                        int potion1 = 0;
                        int potion2 = 0;
                        if ((earing2Id >= 41173) && (41184 >= earing2Id))
                        {
                            if (itemId == 40926)
                            {
                                potion1 = 247;
                                potion2 = 249;
                            }
                            else if (itemId == 40927)
                            {
                                potion1 = 249;
                                potion2 = 251;
                            }
                            else if (itemId == 40928)
                            {
                                potion1 = 251;
                                potion2 = 253;
                            }
                            else if (itemId == 40929)
                            {
                                potion1 = 253;
                                potion2 = 255;
                            }
                            if ((earing2Id >= (itemId + potion1)) && ((itemId + potion2) >= earing2Id))
                            {
                                if ((RandomHelper.Next(99) + 1) < Config.CREATE_CHANCE_MYSTERIOUS)
                                {
                                    createNewItem(pc, (earing2Id - 12), 1);
                                    pc.Inventory.removeItem(l1iteminstance1, 1);
                                    pc.Inventory.removeItem(l1iteminstance, 1);
                                }
                                else
                                {
                                    pc.sendPackets(new S_ServerMessage(160, l1iteminstance1.Name));
                                    // \f1%0%s %2 產生激烈的 %1 光芒，但是沒有任何事情發生。
                                    pc.Inventory.removeItem(l1iteminstance, 1);
                                }
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if ((itemId >= 40931) && (40942 >= itemId))
                    { // 精工的藍、綠、紅寶石
                        int earing3Id = l1iteminstance1.Item.ItemId;
                        if ((earing3Id >= 41161) && (41172 >= earing3Id))
                        {
                            // ミステリアスイアリング類
                            if (earing3Id == (itemId + 230))
                            {
                                if ((RandomHelper.Next(99) + 1) < Config.CREATE_CHANCE_PROCESSING)
                                {
                                    int[] earingId = new int[] { 41161, 41162, 41163, 41164, 41165, 41166, 41167, 41168, 41169, 41170, 41171, 41172 };
                                    int[] earinglevel = new int[] { 21014, 21006, 21007, 21015, 21009, 21008, 21016, 21012, 21010, 21017, 21013, 21011 };
                                    for (int i = 0; i < earingId.Length; i++)
                                    {
                                        if (earing3Id == earingId[i])
                                        {
                                            createNewItem(pc, earinglevel[i], 1);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    pc.sendPackets(new S_ServerMessage(158, l1iteminstance1.Name)); // \f1%0%s 消失。
                                }
                                pc.Inventory.removeItem(l1iteminstance1, 1);
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if ((itemId >= 40943) && (40958 >= itemId))
                    { // 精工的土、水、火、風之鑽
                        int ringId = l1iteminstance1.Item.ItemId;
                        int ringlevel = 0;
                        int gmas = 0;
                        int gmam = 0;
                        if ((ringId >= 41185) && (41200 >= ringId))
                        {
                            // 細工されたリング類
                            if ((itemId == 40943) || (itemId == 40947) || (itemId == 40951) || (itemId == 40955))
                            {
                                gmas = 443;
                                gmam = 447;
                            }
                            else if ((itemId == 40944) || (itemId == 40948) || (itemId == 40952) || (itemId == 40956))
                            {
                                gmas = 442;
                                gmam = 446;
                            }
                            else if ((itemId == 40945) || (itemId == 40949) || (itemId == 40953) || (itemId == 40957))
                            {
                                gmas = 441;
                                gmam = 445;
                            }
                            else if ((itemId == 40946) || (itemId == 40950) || (itemId == 40954) || (itemId == 40958))
                            {
                                gmas = 444;
                                gmam = 448;
                            }
                            if (ringId == (itemId + 242))
                            {
                                if ((RandomHelper.Next(99) + 1) < Config.CREATE_CHANCE_PROCESSING_DIAMOND)
                                {
                                    ringlevel = 20435 + (ringId - 41185);
                                    pc.sendPackets(new S_ServerMessage(gmas, l1iteminstance1.Name));
                                    createNewItem(pc, ringlevel, 1);
                                    pc.Inventory.removeItem(l1iteminstance1, 1);
                                    pc.Inventory.removeItem(l1iteminstance, 1);
                                }
                                else
                                {
                                    pc.sendPackets(new S_ServerMessage(gmam, l1iteminstance.Name));
                                    pc.Inventory.removeItem(l1iteminstance, 1);
                                }
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 41029)
                    { // 召喚球の欠片
                        int dantesId = l1iteminstance1.Item.ItemId;
                        if ((dantesId >= 41030) && (41034 >= dantesId))
                        { // 召喚球のコア・各段階
                            if ((RandomHelper.Next(99) + 1) < Config.CREATE_CHANCE_DANTES)
                            {
                                createNewItem(pc, dantesId + 1, 1);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(158, l1iteminstance1.Name)); // \f1%0が蒸発してなくなりました。
                            }
                            pc.Inventory.removeItem(l1iteminstance1, 1);
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 40964)
                    { // ダークマジックパウダー
                        int historybookId = l1iteminstance1.Item.ItemId;
                        if ((historybookId >= 41011) && (41018 >= historybookId))
                        {
                            if ((RandomHelper.Next(99) + 1) <= Config.CREATE_CHANCE_HISTORY_BOOK)
                            {
                                createNewItem(pc, historybookId + 8, 1);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(158, l1iteminstance1.Name)); // \f1%0が蒸発してなくなりました。
                            }
                            pc.Inventory.removeItem(l1iteminstance1, 1);
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if ((itemId == 40090) || (itemId == 40091) || (itemId == 40092) || (itemId == 40093) || (itemId == 40094))
                    { // ブランク
                      // スクロール(Lv1)～ブランク
                      // スクロール(Lv5)
                        if (pc.Wizard)
                        { // ウィザード
                            if (((itemId == 40090) && (blanksc_skillid <= 7)) || ((itemId == 40091) && (blanksc_skillid <= 15)) || ((itemId == 40092) && (blanksc_skillid <= 22)) || ((itemId == 40093) && (blanksc_skillid <= 31)) || ((itemId == 40094) && (blanksc_skillid <= 39)))
                            { // ブランク
                              // スクロール(Lv5)でレベル5以下の魔法
                                L1ItemInstance spellsc = ItemTable.Instance.createItem(40859 + blanksc_skillid);
                                if (spellsc != null)
                                {
                                    if (pc.Inventory.checkAddItem(spellsc, 1) == L1Inventory.OK)
                                    {
                                        L1Skills l1skills = SkillsTable.Instance.getTemplate(blanksc_skillid + 1); // blanksc_skillidは0始まり
                                        if (pc.CurrentHp + 1 < l1skills.HpConsume + 1)
                                        {
                                            pc.sendPackets(new S_ServerMessage(279)); // \f1HPが不足していて魔法を使うことができません。
                                            return;
                                        }
                                        if (pc.CurrentMp < l1skills.MpConsume)
                                        {
                                            pc.sendPackets(new S_ServerMessage(278)); // \f1MPが不足していて魔法を使うことができません。
                                            return;
                                        }
                                        if (l1skills.ItemConsumeId != 0)
                                        { // 材料が必要
                                            if (!pc.Inventory.checkItem(l1skills.ItemConsumeId, l1skills.ItemConsumeCount))
                                            { // 必要材料をチェック
                                                pc.sendPackets(new S_ServerMessage(299)); // \f1魔法を詠唱するための材料が足りません。
                                                return;
                                            }
                                        }
                                        pc.CurrentHp = pc.CurrentHp - l1skills.HpConsume;
                                        pc.CurrentMp = pc.CurrentMp - l1skills.MpConsume;
                                        int lawful = pc.Lawful + l1skills.Lawful;
                                        if (lawful > 32767)
                                        {
                                            lawful = 32767;
                                        }
                                        if (lawful < -32767)
                                        {
                                            lawful = -32767;
                                        }
                                        pc.Lawful = lawful;
                                        if (l1skills.ItemConsumeId != 0)
                                        { // 材料が必要
                                            pc.Inventory.consumeItem(l1skills.ItemConsumeId, l1skills.ItemConsumeCount);
                                        }
                                        pc.Inventory.removeItem(l1iteminstance, 1);
                                        pc.Inventory.storeItem(spellsc);
                                    }
                                }
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(591)); // \f1スクロールがそんな強い魔法を記録するにはあまりに弱いです。
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(264)); // \f1你的職業無法使用此道具。
                        }

                        // スペルスクロール
                    }
                    else if ((((itemId >= 40859) && (itemId <= 40898)) && (itemId != 40863)) || ((itemId >= 49281) && (itemId <= 49286)))
                    { // 40863はテレポートスクロールとして処理される
                        if ((spellsc_objid == pc.Id) && (l1iteminstance.Item.UseType != 30))
                        { // spell_buff
                            pc.sendPackets(new S_ServerMessage(281)); // \f1魔法が無効になりました。
                            return;
                        }
                        pc.Inventory.removeItem(l1iteminstance, 1);
                        if ((spellsc_objid == 0) && (l1iteminstance.Item.UseType != 51) && (l1iteminstance.Item.UseType != 26) && (l1iteminstance.Item.UseType != 27))
                        {
                            L1World.Instance.broadcastServerMessage("RETURN " + itemId);
                            return;
                            // ターゲットがいない場合にhandleCommandsを送るとぬるぽになるためここでreturn
                            // handleCommandsのほうで判断＆処理すべき部分かもしれない
                        }
                        int skillid = itemId - 40858;
                        if (itemId == 49281)
                        { // 魔法卷軸 (體魄強健術)
                            skillid = 42;
                        }
                        else if (itemId == 49282)
                        { // 魔法卷軸 (祝福魔法武器)
                            skillid = 48;
                        }
                        else if (itemId == 49283)
                        { // 魔法卷軸 (體力回復術)
                            skillid = 49;
                        }
                        else if (itemId == 49284)
                        { // 魔法卷軸 (神聖疾走)
                            skillid = 52;
                        }
                        else if (itemId == 49285)
                        { // 魔法卷軸 (強力加速術)
                            skillid = 54;
                        }
                        else if (itemId == 49286)
                        { // 魔法卷軸 (全部治癒術)
                            skillid = 57;
                        }
                        L1SkillUse l1skilluse = new L1SkillUse();
                        l1skilluse.handleCommands(client.ActiveChar, skillid, spellsc_objid, spellsc_x, spellsc_y, null, 0, L1SkillUse.TYPE_SPELLSC);

                    }
                    else if (((itemId >= 40373) && (itemId <= 40382)) || ((itemId >= 40385) && (itemId <= 40390)))
                    {
                        pc.sendPackets(new S_UseMap(pc, l1iteminstance.Id, l1iteminstance.Item.ItemId));
                    }
                    else if ((itemId == 40310) || (itemId == 40730) || (itemId == 40731) || (itemId == 40732))
                    { // 便箋(未使用)
                        if (writeLetter(itemId, pc, letterCode, letterReceiver, letterText))
                        {
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                    }
                    else if (itemId == 40311)
                    { // 血盟便箋(未使用)
                        if (writeClanLetter(itemId, pc, letterCode, letterReceiver, letterText))
                        {
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                    }
                    else if ((itemId == 49016) || (itemId == 49018) || (itemId == 49020) || (itemId == 49022) || (itemId == 49024))
                    { // 便箋(未開封)
                        pc.sendPackets(new S_Letter(l1iteminstance));
                        l1iteminstance.ItemId = itemId + 1;
                        pc.Inventory.updateItem(l1iteminstance, L1PcInventory.COL_ITEMID);
                        pcInventory.saveItem(l1iteminstance, L1PcInventory.COL_ITEMID);
                    }
                    else if ((itemId == 49017) || (itemId == 49019) || (itemId == 49021) || (itemId == 49023) || (itemId == 49025))
                    { // 便箋(開封済み)
                        pc.sendPackets(new S_Letter(l1iteminstance));
                    }
                    else if ((itemId == 40314) || (itemId == 40316))
                    { // ペットのアミュレット
                        if (pc.Inventory.checkItem(41160))
                        { // 召喚の笛
                            if (withdrawPet(pc, itemObjid))
                            {
                                pc.Inventory.consumeItem(41160, 1);
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 40315)
                    { // ペットの笛
                        pc.sendPackets(new S_Sound(437));
                        pc.broadcastPacket(new S_Sound(437));
                        foreach (L1NpcInstance petNpc in pc.PetList.Values)
                        {
                            if (petNpc is L1PetInstance)
                            { // ペット
                                L1PetInstance pet = (L1PetInstance)petNpc;
                                pet.call();
                            }
                        }
                    }
                    else if (itemId == 40493)
                    { // マジックフルート
                        pc.sendPackets(new S_Sound(165));
                        pc.broadcastPacket(new S_Sound(165));
                        foreach (GameObject visible in pc.KnownObjects)
                        {
                            if (visible is L1GuardianInstance)
                            {
                                L1GuardianInstance guardian = (L1GuardianInstance)visible;
                                if (guardian.NpcTemplate.get_npcId() == 70850)
                                { // パン
                                    if (createNewItem(pc, 88, 1))
                                    {
                                        pc.Inventory.removeItem(l1iteminstance, 1);
                                    }
                                }
                            }
                        }
                    }
                    else if (itemId == 40325)
                    { // 2面コイン
                        if (pc.Inventory.checkItem(40318, 1))
                        {
                            int gfxid = 3237 + RandomHelper.Next(2);
                            pc.sendPackets(new S_SkillSound(pc.Id, gfxid));
                            pc.broadcastPacket(new S_SkillSound(pc.Id, gfxid));
                            pc.Inventory.consumeItem(40318, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 40326)
                    { // 3方向ルーレット
                        if (pc.Inventory.checkItem(40318, 1))
                        {
                            int gfxid = 3229 + RandomHelper.Next(3);
                            pc.sendPackets(new S_SkillSound(pc.Id, gfxid));
                            pc.broadcastPacket(new S_SkillSound(pc.Id, gfxid));
                            pc.Inventory.consumeItem(40318, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 40327)
                    { // 4方向ルーレット
                        if (pc.Inventory.checkItem(40318, 1))
                        {
                            int gfxid = 3241 + RandomHelper.Next(4);
                            pc.sendPackets(new S_SkillSound(pc.Id, gfxid));
                            pc.broadcastPacket(new S_SkillSound(pc.Id, gfxid));
                            pc.Inventory.consumeItem(40318, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 40328)
                    { // 6面ダイス
                        if (pc.Inventory.checkItem(40318, 1))
                        {
                            int gfxid = 3204 + RandomHelper.Next(6);
                            pc.sendPackets(new S_SkillSound(pc.Id, gfxid));
                            pc.broadcastPacket(new S_SkillSound(pc.Id, gfxid));
                            pc.Inventory.consumeItem(40318, 1);
                        }
                        else
                        {
                            // \f1沒有任何事情發生。
                            pc.sendPackets(new S_ServerMessage(79));
                        }
                    }
                    else if ((itemId == 40089) || (itemId == 140089))
                    { // 復活スクロール、祝福された復活スクロール
                        L1Character resobject = (L1Character)L1World.Instance.findObject(resid);
                        if (resobject != null)
                        {
                            if (resobject is L1PcInstance)
                            {
                                L1PcInstance target = (L1PcInstance)resobject;
                                if (pc.Id == target.Id)
                                {
                                    return;
                                }
                                if (L1World.Instance.getVisiblePlayer(target, 0).Count > 0)
                                {
                                    foreach (L1PcInstance visiblePc in L1World.Instance.getVisiblePlayer(target, 0))
                                    {
                                        if (!visiblePc.Dead)
                                        {
                                            // \f1その場所に他の人が立っているので復活させることができません。
                                            pc.sendPackets(new S_ServerMessage(592));
                                            return;
                                        }
                                    }
                                }
                                if ((target.CurrentHp == 0) && (target.Dead == true))
                                {
                                    if (pc.Map.UseResurrection)
                                    {
                                        target.TempID = pc.Id;
                                        if (itemId == 40089)
                                        {
                                            // また復活したいですか？（Y/N）
                                            target.sendPackets(new S_Message_YN(321, ""));
                                        }
                                        else if (itemId == 140089)
                                        {
                                            // また復活したいですか？（Y/N）
                                            target.sendPackets(new S_Message_YN(322, ""));
                                        }
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                            }
                            else if (resobject is L1NpcInstance)
                            {
                                if (!(resobject is L1TowerInstance))
                                {
                                    L1NpcInstance npc = (L1NpcInstance)resobject;
                                    if (npc.NpcTemplate.CantResurrect && !(npc is L1PetInstance))
                                    {
                                        pc.Inventory.removeItem(l1iteminstance, 1);
                                        return;
                                    }
                                    if ((npc is L1PetInstance) && (L1World.Instance.getVisiblePlayer(npc, 0).Count > 0))
                                    {
                                        foreach (L1PcInstance visiblePc in L1World.Instance.getVisiblePlayer(npc, 0))
                                        {
                                            if (!visiblePc.Dead)
                                            {
                                                // \f1その場所に他の人が立っているので復活させることができません。
                                                pc.sendPackets(new S_ServerMessage(592));
                                                return;
                                            }
                                        }
                                    }
                                    if ((npc.CurrentHp == 0) && npc.Dead)
                                    {
                                        npc.resurrect(npc.MaxHp / 4);
                                        npc.Resurrect = true;
                                        if ((npc is L1PetInstance))
                                        {
                                            L1PetInstance pet = (L1PetInstance)npc;
                                            // 開始飽食度計時
                                            pet.startFoodTimer(pet);
                                            // 開始回血回魔
                                            pet.startHpRegeneration();
                                            pet.startMpRegeneration();
                                        }
                                    }
                                }
                            }
                        }
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if (((itemId > 40169) && (itemId < 40226)) || ((itemId >= 45000) && (itemId <= 45022)))
                    { // 魔法書
                        useSpellBook(pc, l1iteminstance, itemId);
                    }
                    else if ((itemId > 40225) && (itemId < 40232))
                    {
                        if (pc.Crown || pc.Gm)
                        {
                            if ((itemId == 40226) && (pc.Level >= 15))
                            {
                                SpellBook4(pc, l1iteminstance, client);
                            }
                            else if ((itemId == 40228) && (pc.Level >= 30))
                            {
                                SpellBook4(pc, l1iteminstance, client);
                            }
                            else if ((itemId == 40227) && (pc.Level >= 40))
                            {
                                SpellBook4(pc, l1iteminstance, client);
                            }
                            else if (((itemId == 40231) || (itemId == 40232)) && (pc.Level >= 45))
                            {
                                SpellBook4(pc, l1iteminstance, client);
                            }
                            else if ((itemId == 40230) && (pc.Level >= 50))
                            {
                                SpellBook4(pc, l1iteminstance, client);
                            }
                            else if ((itemId == 40229) && (pc.Level >= 55))
                            {
                                SpellBook4(pc, l1iteminstance, client);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(312)); // LVが低くて
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79));
                        }
                    }
                    else if (((itemId >= 40232) && (itemId <= 40264)) || ((itemId >= 41149) && (itemId <= 41153)))
                    {
                        useElfSpellBook(pc, l1iteminstance, itemId);
                    }
                    else if ((itemId > 40264) && (itemId < 40280))
                    { // 闇精霊の水晶
                        if (pc.Darkelf || pc.Gm)
                        {
                            if ((itemId >= 40265) && (itemId <= 40269) && (pc.Level >= 15))
                            {
                                SpellBook1(pc, l1iteminstance, client);
                            }
                            else if ((itemId >= 40270) && (itemId <= 40274) && (pc.Level >= 30))
                            {
                                SpellBook1(pc, l1iteminstance, client);
                            }
                            else if ((itemId >= 40275) && (itemId <= 40279) && (pc.Level >= 45))
                            {
                                SpellBook1(pc, l1iteminstance, client);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(312));
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // (原文:闇精霊の水晶はダークエルフのみが習得できます。)
                        }
                    }
                    else if (((itemId >= 40164) && (itemId <= 40166)) || ((itemId >= 41147) && (itemId <= 41148)))
                    {
                        if (pc.Knight || pc.Gm)
                        {
                            if ((itemId >= 40164) && (itemId <= 40165) && (pc.Level >= 50))
                            {
                                SpellBook3(pc, l1iteminstance, client);
                            }
                            else if ((itemId >= 41147) && (itemId <= 41148) && (pc.Level >= 50))
                            {
                                SpellBook3(pc, l1iteminstance, client);
                            }
                            else if ((itemId == 40166) && (pc.Level >= 60))
                            { // バウンスアタック
                                SpellBook3(pc, l1iteminstance, client);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(312));
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79));
                        }
                    }
                    else if ((itemId >= 49102) && (itemId <= 49116))
                    { // ドラゴンナイトの書板
                        if (pc.DragonKnight || pc.Gm)
                        {
                            if ((itemId >= 49102) && (itemId <= 49106) && (pc.Level >= 15))
                            {
                                SpellBook5(pc, l1iteminstance, client);
                            }
                            else if ((itemId >= 49107) && (itemId <= 49111) && (pc.Level >= 30))
                            {
                                SpellBook5(pc, l1iteminstance, client);
                            }
                            else if ((itemId >= 49112) && (itemId <= 49116) && (pc.Level >= 45))
                            {
                                SpellBook5(pc, l1iteminstance, client);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(312));
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79));
                        }
                    }
                    else if ((itemId >= 49117) && (itemId <= 49136))
                    { // 記憶の水晶
                        if (pc.Illusionist || pc.Gm)
                        {
                            if ((itemId >= 49117) && (itemId <= 49121) && (pc.Level >= 10))
                            {
                                SpellBook6(pc, l1iteminstance, client);
                            }
                            else if ((itemId >= 49122) && (itemId <= 49126) && (pc.Level >= 20))
                            {
                                SpellBook6(pc, l1iteminstance, client);
                            }
                            else if ((itemId >= 49127) && (itemId <= 49131) && (pc.Level >= 30))
                            {
                                SpellBook6(pc, l1iteminstance, client);
                            }
                            else if ((itemId >= 49132) && (itemId <= 49136) && (pc.Level >= 40))
                            {
                                SpellBook6(pc, l1iteminstance, client);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(312));
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79));
                        }
                    }
                    else if ((itemId == 40079) || (itemId == 40095) || (itemId == 40521))
                    { // 傳送回家的卷軸、象牙塔傳送回家的卷軸、精靈羽翼
                        if (pc.Map.Escapable || pc.Gm)
                        {
                            int[] loc = Getback.GetBack_Location(pc, true);
                            L1Teleport.teleport(pc, loc[0], loc[1], (short)loc[2], 5, true);
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(276)); // \f1在此無法使用傳送。
                        }
                    }
                    else if (itemId == 40124)
                    { // 血盟帰還スクロール
                        if (pc.Map.Escapable || pc.Gm)
                        {
                            int castle_id = 0;
                            int house_id = 0;
                            if (pc.Clanid != 0)
                            { // クラン所属
                                L1Clan clan = L1World.Instance.getClan(pc.Clanname);
                                if (clan != null)
                                {
                                    castle_id = clan.CastleId;
                                    house_id = clan.HouseId;
                                }
                            }
                            if (castle_id != 0)
                            { // 城主クラン員
                                if (pc.Map.Escapable || pc.Gm)
                                {
                                    int[] loc = new int[3];
                                    loc = L1CastleLocation.getCastleLoc(castle_id);
                                    int locx = loc[0];
                                    int locy = loc[1];
                                    short mapid = (short)(loc[2]);
                                    L1Teleport.teleport(pc, locx, locy, mapid, 5, true);
                                    pc.Inventory.removeItem(l1iteminstance, 1);
                                }
                                else
                                {
                                    pc.sendPackets(new S_ServerMessage(647));
                                }
                            }
                            else if (house_id != 0)
                            { // アジト所有クラン員
                                if (pc.Map.Escapable || pc.Gm)
                                {
                                    int[] loc = new int[3];
                                    loc = L1HouseLocation.getHouseLoc(house_id);
                                    int locx = loc[0];
                                    int locy = loc[1];
                                    short mapid = (short)(loc[2]);
                                    L1Teleport.teleport(pc, locx, locy, mapid, 5, true);
                                    pc.Inventory.removeItem(l1iteminstance, 1);
                                }
                                else
                                {
                                    pc.sendPackets(new S_ServerMessage(647));
                                }
                            }
                            else
                            {
                                if (pc.HomeTownId > 0)
                                {
                                    int[] loc = L1TownLocation.getGetBackLoc(pc.HomeTownId);
                                    int locx = loc[0];
                                    int locy = loc[1];
                                    short mapid = (short)(loc[2]);
                                    L1Teleport.teleport(pc, locx, locy, mapid, 5, true);
                                    pc.Inventory.removeItem(l1iteminstance, 1);
                                }
                                else
                                {
                                    int[] loc = Getback.GetBack_Location(pc, true);
                                    L1Teleport.teleport(pc, loc[0], loc[1], (short)loc[2], 5, true);
                                    pc.Inventory.removeItem(l1iteminstance, 1);
                                }
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(647));
                        }
                    }
                    else if ((itemId == 140100) || (itemId == 40100) || (itemId == 40099) || (itemId == 40086) || (itemId == 40863))
                    { // スペルスクロール(テレポート)
                        L1BookMark bookm = pc.getBookMark(btele);
                        if (bookm != null)
                        { // ブックマークを取得出来たらテレポート
                            if (pc.Map.Escapable || pc.Gm)
                            {
                                int newX = bookm.LocX;
                                int newY = bookm.LocY;
                                short mapId = bookm.MapId;

                                if (itemId == 40086)
                                { // マステレポートスクロール
                                    foreach (L1PcInstance member in L1World.Instance.getVisiblePlayer(pc))
                                    {
                                        if ((pc.Location.getTileLineDistance(member.Location) <= 3) && (member.Clanid == pc.Clanid) && (pc.Clanid != 0) && (member.Id != pc.Id))
                                        {
                                            L1Teleport.teleport(member, newX, newY, mapId, 5, true);
                                        }
                                    }
                                }

                                L1Teleport.teleport(pc, newX, newY, mapId, 5, true);
                                // 卷軸傳送後 使用物品延遲完才解開停止狀態
                                L1ItemDelay.teleportUnlock(pc, l1iteminstance);
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(79));
                                pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_TELEPORT_UNLOCK, true));
                            }
                        }
                        else
                        {
                            if (pc.Map.Teleportable || pc.Gm)
                            {
                                L1Location newLocation = pc.Location.randomLocation(200, true);
                                int newX = newLocation.X;
                                int newY = newLocation.Y;
                                short mapId = (short)newLocation.MapId;

                                if (itemId == 40086)
                                { // マステレポートスクロール
                                    foreach (L1PcInstance member in L1World.Instance.getVisiblePlayer(pc))
                                    {
                                        if ((pc.Location.getTileLineDistance(member.Location) <= 3) && (member.Clanid == pc.Clanid) && (pc.Clanid != 0) && (member.Id != pc.Id))
                                        {
                                            L1Teleport.teleport(member, newX, newY, mapId, 5, true);
                                        }
                                    }
                                }
                                L1Teleport.teleport(pc, newX, newY, mapId, 5, true);
                                // 卷軸傳送後 使用物品延遲完才解開停止狀態
                                L1ItemDelay.teleportUnlock(pc, l1iteminstance);
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(276));
                                pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_TELEPORT_UNLOCK, true));
                            }
                        }
                    }
                    else if (itemId == 240100)
                    { // 呪われたテレポートスクロール(オリジナルアイテム)
                        L1Teleport.teleport(pc, pc.X, pc.Y, pc.MapId, pc.Heading, true);
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if ((itemId >= 40901) && (itemId <= 40908))
                    { // 各種エンゲージリング
                        L1PcInstance partner = null;
                        bool partner_stat = false;
                        if (pc.PartnerId != 0)
                        { // 結婚中
                            partner = (L1PcInstance)L1World.Instance.findObject(pc.PartnerId);
                            if ((partner != null) && (partner.PartnerId != 0) && (pc.PartnerId == partner.Id) && (partner.PartnerId == pc.Id))
                            {
                                partner_stat = true;
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(662)); // \f1あなたは結婚していません。
                            return;
                        }

                        if (partner_stat)
                        {
                            bool castle_area = L1CastleLocation.checkInAllWarArea(partner.X, partner.Y, partner.MapId);
                            if (((partner.MapId == 0) || (partner.MapId == 4) || (partner.MapId == 304)) && (castle_area == false))
                            {
                                L1Teleport.teleport(pc, partner.X, partner.Y, partner.MapId, 5, true);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(547)); // \f1あなたのパートナーは今あなたが行けない所でプレイ中です。
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(546)); // \f1あなたのパートナーは今プレイをしていません。
                        }
                    }
                    else if (itemId == 40555)
                    { // 密室鑰匙
                        if (pc.Knight && ((pc.X >= 32806) && (pc.X <= 32814)) && ((pc.Y >= 32798) && (pc.Y <= 32807)) && (pc.MapId == 13))
                        {
                            short mapid = 13;
                            L1Teleport.teleport(pc, 32815, 32810, mapid, 5, false);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 40417)
                    { // 精靈結晶
                        if (((pc.X >= 32665) && (pc.X <= 32674)) && ((pc.Y >= 32976) && (pc.Y <= 32985)) && (pc.MapId == 440))
                        {
                            short mapid = 430;
                            L1Teleport.teleport(pc, 32922, 32812, mapid, 5, true);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 49202)
                    { // 時空裂痕邪念碎片
                        if ((pc.MapId != 2004) && (pc.Quest.get_step(L1Quest.QUEST_LEVEL50) > 1))
                        {
                            short mapid = 2004;
                            L1Teleport.teleport(pc, 32723, 32834, mapid, 5, true);
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 49178)
                    { // 希蓮恩的護身符
                        if ((pc.Illusionist) && (pc.MapId == 2004) && (pc.Quest.get_step(L1Quest.QUEST_LEVEL50) > 1))
                        {
                            short mapid = 1000;
                            L1Teleport.teleport(pc, 32772, 32812, mapid, 5, true);
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 49216)
                    { // 普洛凱爾的護身符
                        if ((pc.DragonKnight) && (pc.MapId == 2004) && (pc.Quest.get_step(L1Quest.QUEST_LEVEL50) > 1))
                        {
                            short mapid = 1001;
                            L1Teleport.teleport(pc, 32817, 32832, mapid, 5, true);
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 49165)
                    { // 聖殿2樓鑰匙
                        if (pc.Quest.get_step(L1Quest.QUEST_LEVEL50) > 0)
                        {
                            if (pc.Crown && (pc.MapId == 2000))
                            {
                                short mapid = 2000;
                                L1Teleport.teleport(pc, 32860, 32739, mapid, 5, true);
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                            else if (pc.Knight && (pc.MapId == 2001))
                            {
                                short mapid = 2001;
                                L1Teleport.teleport(pc, 32860, 32739, mapid, 5, true);
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                            else if (pc.Elf && (pc.MapId == 2002))
                            {
                                short mapid = 2002;
                                L1Teleport.teleport(pc, 32860, 32739, mapid, 5, true);
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                            else if (pc.Wizard && (pc.MapId == 2003))
                            {
                                short mapid = 2003;
                                L1Teleport.teleport(pc, 32860, 32739, mapid, 5, true);
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 49166)
                    { // 聖殿3樓鑰匙
                        if (pc.Quest.get_step(L1Quest.QUEST_LEVEL50) > 0)
                        {
                            if (pc.Crown && (pc.MapId == 2000))
                            {
                                short mapid = 2000;
                                L1Teleport.teleport(pc, 32735, 32748, mapid, 5, true);
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                            else if (pc.Knight && (pc.MapId == 2001))
                            {
                                short mapid = 2001;
                                L1Teleport.teleport(pc, 32735, 32748, mapid, 5, true);
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                            else if (pc.Elf && (pc.MapId == 2002))
                            {
                                short mapid = 2002;
                                L1Teleport.teleport(pc, 32735, 32748, mapid, 5, true);
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                            else if (pc.Wizard && (pc.MapId == 2003))
                            {
                                short mapid = 2003;
                                L1Teleport.teleport(pc, 32735, 32748, mapid, 5, true);
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 49239)
                    { // 消滅之意志
                        if (pc.Quest.get_step(L1Quest.QUEST_LEVEL50) > 0)
                        {
                            if (pc.Crown && (pc.MapId == 2000))
                            {
                                short mapid = 2000;
                                L1Teleport.teleport(pc, 32793, 32757, mapid, 5, true);
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                            else if (pc.Knight && (pc.MapId == 2001))
                            {
                                short mapid = 2001;
                                L1Teleport.teleport(pc, 32793, 32757, mapid, 5, true);
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                            else if (pc.Elf && (pc.MapId == 2002))
                            {
                                short mapid = 2002;
                                L1Teleport.teleport(pc, 32793, 32757, mapid, 5, true);
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                            else if (pc.Wizard && (pc.MapId == 2003))
                            {
                                short mapid = 2003;
                                L1Teleport.teleport(pc, 32793, 32757, mapid, 5, true);
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 40566)
                    { // ミステリアス シェル
                        if (pc.Elf && (pc.X >= 33971) && (pc.X <= 33975) && (pc.Y >= 32324) && (pc.Y <= 32328) && (pc.MapId == 4) &&
                            !pc.Inventory.checkItem(40548))
                        { // 亡霊の袋
                            bool found = false;
                            foreach (GameObject obj in L1World.Instance.Object)
                            {
                                if (obj is L1MonsterInstance)
                                {
                                    L1MonsterInstance mob = (L1MonsterInstance)obj;
                                    if (mob != null)
                                    {
                                        if (mob.NpcTemplate.get_npcId() == 45300)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (found)
                            {
                                pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                            }
                            else
                            {
                                L1SpawnUtil.spawn(pc, 45300, 0, 0); // 古代人の亡霊
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 40557)
                    { // 暗殺リスト(グルーディン)
                        if ((pc.X == 32620) && (pc.Y == 32641) && (pc.MapId == 4))
                        {
                            foreach (GameObject @object in L1World.Instance.Object)
                            {
                                if (@object is L1NpcInstance)
                                {
                                    L1NpcInstance npc = (L1NpcInstance)@object;
                                    if (npc.NpcTemplate.get_npcId() == 45883)
                                    {
                                        pc.sendPackets(new S_ServerMessage(79));
                                        return;
                                    }
                                }
                            }
                            L1SpawnUtil.spawn(pc, 45883, 0, 300000);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 40563)
                    { // 暗殺リスト(火田村)
                        if ((pc.X == 32730) && (pc.Y == 32426) && (pc.MapId == 4))
                        {
                            foreach (GameObject @object in L1World.Instance.Object)
                            {
                                if (@object is L1NpcInstance)
                                {
                                    L1NpcInstance npc = (L1NpcInstance)@object;
                                    if (npc.NpcTemplate.get_npcId() == 45884)
                                    {
                                        pc.sendPackets(new S_ServerMessage(79));
                                        return;
                                    }
                                }
                            }
                            L1SpawnUtil.spawn(pc, 45884, 0, 300000);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 40561)
                    { // 暗殺リスト(ケント)
                        if ((pc.X == 33046) && (pc.Y == 32806) && (pc.MapId == 4))
                        {
                            foreach (GameObject @object in L1World.Instance.Object)
                            {
                                if (@object is L1NpcInstance)
                                {
                                    L1NpcInstance npc = (L1NpcInstance)@object;
                                    if (npc.NpcTemplate.get_npcId() == 45885)
                                    {
                                        pc.sendPackets(new S_ServerMessage(79));
                                        return;
                                    }
                                }
                            }
                            L1SpawnUtil.spawn(pc, 45885, 0, 300000);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 40560)
                    { // 暗殺リスト(ウッドベック)
                        if ((pc.X == 32580) && (pc.Y == 33260) && (pc.MapId == 4))
                        {
                            foreach (GameObject @object in L1World.Instance.Object)
                            {
                                if (@object is L1NpcInstance)
                                {
                                    L1NpcInstance npc = (L1NpcInstance)@object;
                                    if (npc.NpcTemplate.get_npcId() == 45886)
                                    {
                                        pc.sendPackets(new S_ServerMessage(79));
                                        return;
                                    }
                                }
                            }
                            L1SpawnUtil.spawn(pc, 45886, 0, 300000);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 40562)
                    { // 暗殺リスト(ハイネ)
                        if ((pc.X == 33447) && (pc.Y == 33476) && (pc.MapId == 4))
                        {
                            foreach (GameObject @object in L1World.Instance.Object)
                            {
                                if (@object is L1NpcInstance)
                                {
                                    L1NpcInstance npc = (L1NpcInstance)@object;
                                    if (npc.NpcTemplate.get_npcId() == 45887)
                                    {
                                        pc.sendPackets(new S_ServerMessage(79));
                                        return;
                                    }
                                }
                            }
                            L1SpawnUtil.spawn(pc, 45887, 0, 300000);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 40559)
                    { // 暗殺リスト(アデン)
                        if ((pc.X == 34215) && (pc.Y == 33195) && (pc.MapId == 4))
                        {
                            foreach (GameObject @object in L1World.Instance.Object)
                            {
                                if (@object is L1NpcInstance)
                                {
                                    L1NpcInstance npc = (L1NpcInstance)@object;
                                    if (npc.NpcTemplate.get_npcId() == 45888)
                                    {
                                        pc.sendPackets(new S_ServerMessage(79));
                                        return;
                                    }
                                }
                            }
                            L1SpawnUtil.spawn(pc, 45888, 0, 300000);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 40558)
                    { // 暗殺リスト(ギラン)
                        if ((pc.X == 33513) && (pc.Y == 32890) && (pc.MapId == 4))
                        {
                            foreach (GameObject @object in L1World.Instance.Object)
                            {
                                if (@object is L1NpcInstance)
                                {
                                    L1NpcInstance npc = (L1NpcInstance)@object;
                                    if (npc.NpcTemplate.get_npcId() == 45889)
                                    {
                                        pc.sendPackets(new S_ServerMessage(79));
                                        return;
                                    }
                                }
                            }
                            L1SpawnUtil.spawn(pc, 45889, 0, 300000);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 40572)
                    { // 刺客之證
                        if ((pc.X == 32778) && (pc.Y == 32738) && (pc.MapId == 21))
                        {
                            L1Teleport.teleport(pc, 32781, 32728, (short)21, 5, true);
                        }
                        else if ((pc.X == 32781) && (pc.Y == 32728) && (pc.MapId == 21))
                        {
                            L1Teleport.teleport(pc, 32778, 32738, (short)21, 5, true);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79));
                        }
                    }
                    else if ((itemId == 40006) || (itemId == 40412) || (itemId == 140006))
                    { // 松木魔杖、黑暗安特的樹枝
                        if (pc.Map.UsePainwand)
                        {
                            S_AttackPacket s_attackPacket = new S_AttackPacket(pc, 0, ActionCodes.ACTION_Wand);
                            pc.sendPackets(s_attackPacket);
                            pc.broadcastPacket(s_attackPacket);
                            int[] mobArray = new int[] { 45008, 45140, 45016, 45021, 45025, 45033, 45099, 45147, 45123, 45130, 45046, 45092, 45138, 45098, 45127, 45143, 45149, 45171, 45040, 45155, 45192, 45173, 45213, 45079, 45144 };
                            // ゴブリン・ホブコブリン・コボルト・鹿・グレムリン
                            // インプ・インプエルダー・オウルベア・スケルトンアーチャー・スケルトンアックス
                            // ビーグル・ドワーフウォーリアー・オークスカウト・ガンジオーク・ロバオーク
                            // ドゥダーマラオーク・アトゥバオーク・ネルガオーク・ベアー・トロッグ
                            // ラットマン・ライカンスロープ・ガースト・ノール・リザードマン
                            /*
							 * 45005, 45008, 45009, 45016, 45019, 45043, 45060,
							 * 45066, 45068, 45082, 45093, 45101, 45107, 45126,
							 * 45129, 45136, 45144, 45157, 45161, 45173, 45184,
							 * 45223 }; // カエル、ゴブリン、オーク、コボルド、 // オーク
							 * アーチャー、ウルフ、スライム、ゾンビ、 // フローティングアイ、オーク ファイター、 // ウェア
							 * ウルフ、アリゲーター、スケルトン、 // ストーン ゴーレム、スケルトン アーチャー、 // ジャイアント
							 * スパイダー、リザードマン、グール、 // スパルトイ、ライカンスロープ、ドレッド スパイダー、 //
							 * バグベアー
							 */
                            int rnd = RandomHelper.Next(mobArray.Length);
                            L1SpawnUtil.spawn(pc, mobArray[rnd], 0, 300000);
                            if ((itemId == 40006) || (itemId == 140006))
                            {
                                l1iteminstance.ChargeCount = l1iteminstance.ChargeCount - 1;
                                pc.Inventory.updateItem(l1iteminstance, L1PcInventory.COL_CHARGE_COUNT);
                                if (l1iteminstance.ChargeCount <= 0)
                                { // 次數為 0時刪除
                                    pc.Inventory.removeItem(l1iteminstance, 1);
                                }
                            }
                            else
                            {
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                        }
                        else
                        {
                            // \f1沒有任何事情發生。
                            pc.sendPackets(new S_ServerMessage(79));
                        }
                    }
                    else if (itemId == 40007)
                    { // 閃電魔杖
                        int dmg = 0;
                        int[] data = null;
                        GameObject target = L1World.Instance.findObject(spellsc_objid);
                        if (target != null)
                        {
                            dmg = doWandAction(pc, target);
                        }
                        data = new int[] { ActionCodes.ACTION_Wand, dmg, 10, 6 }; // data = {actid, dmg, spellgfx, use_type}
                        pc.sendPackets(new S_UseAttackSkill(pc, spellsc_objid, spellsc_x, spellsc_y, data));
                        pc.broadcastPacket(new S_UseAttackSkill(pc, spellsc_objid, spellsc_x, spellsc_y, data));
                        l1iteminstance.ChargeCount = l1iteminstance.ChargeCount - 1;
                        pc.Inventory.updateItem(l1iteminstance, L1PcInventory.COL_CHARGE_COUNT);
                        if (l1iteminstance.ChargeCount <= 0)
                        { // 次數為 0時刪除
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                    }
                    else if ((itemId == 40008) || (itemId == 40410) || (itemId == 140008))
                    { // 楓木魔杖、黑暗安特的樹皮
                        if ((pc.MapId == 63) || (pc.MapId == 552) || (pc.MapId == 555) || (pc.MapId == 557) || (pc.MapId == 558) || (pc.MapId == 779))
                        { // 水中では使用不可
                            pc.sendPackets(new S_ServerMessage(563)); // \f1ここでは使えません。
                        }
                        else
                        {
                            S_AttackPacket s_attackPacket = new S_AttackPacket(pc, 0, ActionCodes.ACTION_Wand);
                            pc.sendPackets(s_attackPacket);
                            pc.broadcastPacket(s_attackPacket);
                            GameObject target = L1World.Instance.findObject(spellsc_objid);
                            if (target != null)
                            {
                                L1Character cha = (L1Character)target;
                                polyAction(pc, cha);
                                if ((itemId == 40008) || (itemId == 140008))
                                {
                                    l1iteminstance.ChargeCount = l1iteminstance.ChargeCount - 1;
                                    pc.Inventory.updateItem(l1iteminstance, L1PcInventory.COL_CHARGE_COUNT);
                                    if (l1iteminstance.ChargeCount <= 0)
                                    { // 次數為 0時刪除
                                        pc.Inventory.removeItem(l1iteminstance, 1);
                                    }
                                }
                                else
                                {
                                    pc.Inventory.removeItem(l1iteminstance, 1);
                                }
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                            }
                        }
                    }
                    else if ((itemId >= 40289) && (itemId <= 40297))
                    { // 傲慢の塔11~91階テレポートアミュレット
                        useToiTeleportAmulet(pc, itemId, l1iteminstance);
                    }
                    else if ((itemId >= 40280) && (itemId <= 40288))
                    {
                        // 封印された傲慢の塔11～91階テレポートアミュレット
                        pc.Inventory.removeItem(l1iteminstance, 1);
                        L1ItemInstance item = pc.Inventory.storeItem(itemId + 9, 1);
                        if (item != null)
                        {
                            pc.sendPackets(new S_ServerMessage(403, item.LogName));
                        }
                        // 肉類
                    }
                    else if ((itemId == 40056) || (itemId == 40057) || (itemId == 40059) || (itemId == 40060) || (itemId == 40061) || (itemId == 40062) || (itemId == 40063) || (itemId == 40064) || (itemId == 40065) || (itemId == 40069) || (itemId == 40072) || (itemId == 40073) || (itemId == 140061) || (itemId == 140062) || (itemId == 140065) || (itemId == 140069) || (itemId == 140072) || (itemId == 41296) || (itemId == 41297) || (itemId == 41266) || (itemId == 41267) || (itemId == 41274) || (itemId == 41275) || (itemId == 41276) || (itemId == 41252) || (itemId == 49040) || (itemId == 49041) || (itemId == 49042) || (itemId == 49043) || (itemId == 49044) || (itemId == 49045) || (itemId == 49046) || (itemId == 49047))
                    {
                        pc.Inventory.removeItem(l1iteminstance, 1);
                        // XXX 食べ物毎の満腹度(100単位で変動)
                        short foodvolume1 = (short)(l1iteminstance.Item.FoodVolume / 10);
                        short foodvolume2 = 0;
                        if (foodvolume1 <= 0)
                        {
                            foodvolume1 = 5;
                        }
                        if (pc.get_food() >= 225)
                        {
                            pc.sendPackets(new S_PacketBox(S_PacketBox.FOOD, (short)pc.get_food()));
                        }
                        else
                        {
                            foodvolume2 = (short)(pc.get_food() + foodvolume1);
                            if (foodvolume2 <= 225)
                            {
                                pc.set_food(foodvolume2);
                                pc.sendPackets(new S_PacketBox(S_PacketBox.FOOD, (short)pc.get_food()));
                            }
                            else
                            {
                                pc.set_food((short)225);
                                pc.sendPackets(new S_PacketBox(S_PacketBox.FOOD, (short)pc.get_food()));
                            }
                        }
                        if (itemId == 40057)
                        { // フローティングアイ肉
                            pc.setSkillEffect(L1SkillId.STATUS_FLOATING_EYE, 0);
                        }
                        pc.sendPackets(new S_ServerMessage(76, l1iteminstance.Item.IdentifiedNameId));
                    }
                    else if (itemId == 40070)
                    { // 進化果實
                        pc.sendPackets(new S_ServerMessage(76, l1iteminstance.LogName));
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if (itemId == 41298)
                    { // 鱈魚
                        Potion.UseHeallingPotion(pc, l1iteminstance, 4, 189);
                    }
                    else if (itemId == 41299)
                    { // 虎斑帶魚
                        Potion.UseHeallingPotion(pc, l1iteminstance, 15, 194);
                    }
                    else if (itemId == 41300)
                    { // 鮪魚
                        Potion.UseHeallingPotion(pc, l1iteminstance, 35, 197);
                    }
                    else if ((itemId >= 40136) && (itemId <= 40161))
                    { // 煙火
                        int soundid = 3198;
                        if (itemId == 40154)
                        {
                            soundid = 3198;
                        }
                        else if (itemId == 40152)
                        {
                            soundid = 2031;
                        }
                        else if (itemId == 40141)
                        {
                            soundid = 2028;
                        }
                        else if (itemId == 40160)
                        {
                            soundid = 2030;
                        }
                        else if (itemId == 40145)
                        {
                            soundid = 2029;
                        }
                        else if (itemId == 40159)
                        {
                            soundid = 2033;
                        }
                        else if (itemId == 40151)
                        {
                            soundid = 2032;
                        }
                        else if (itemId == 40161)
                        {
                            soundid = 2037;
                        }
                        else if (itemId == 40142)
                        {
                            soundid = 2036;
                        }
                        else if (itemId == 40146)
                        {
                            soundid = 2039;
                        }
                        else if (itemId == 40148)
                        {
                            soundid = 2043;
                        }
                        else if (itemId == 40143)
                        {
                            soundid = 2041;
                        }
                        else if (itemId == 40156)
                        {
                            soundid = 2042;
                        }
                        else if (itemId == 40139)
                        {
                            soundid = 2040;
                        }
                        else if (itemId == 40137)
                        {
                            soundid = 2047;
                        }
                        else if (itemId == 40136)
                        {
                            soundid = 2046;
                        }
                        else if (itemId == 40138)
                        {
                            soundid = 2048;
                        }
                        else if (itemId == 40140)
                        {
                            soundid = 2051;
                        }
                        else if (itemId == 40144)
                        {
                            soundid = 2053;
                        }
                        else if (itemId == 40147)
                        {
                            soundid = 2045;
                        }
                        else if (itemId == 40149)
                        {
                            soundid = 2034;
                        }
                        else if (itemId == 40150)
                        {
                            soundid = 2055;
                        }
                        else if (itemId == 40153)
                        {
                            soundid = 2038;
                        }
                        else if (itemId == 40155)
                        {
                            soundid = 2044;
                        }
                        else if (itemId == 40157)
                        {
                            soundid = 2035;
                        }
                        else if (itemId == 40158)
                        {
                            soundid = 2049;
                        }
                        else
                        {
                            soundid = 3198;
                        }

                        S_SkillSound s_skillsound = new S_SkillSound(pc.Id, soundid);
                        pc.sendPackets(s_skillsound);
                        pc.broadcastPacket(s_skillsound);
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if ((itemId >= 41357) && (itemId <= 41382))
                    { // アルファベット花火
                        int soundid = itemId - 34946;
                        S_SkillSound s_skillsound = new S_SkillSound(pc.Id, soundid);
                        pc.sendPackets(s_skillsound);
                        pc.broadcastPacket(s_skillsound);
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if (itemId == 40615)
                    { // 暗影神殿2樓鑰匙
                        if (((pc.X >= 32701) && (pc.X <= 32705)) && ((pc.Y >= 32894) && (pc.Y <= 32898)) && (pc.MapId == 522))
                        { // 影の神殿1F
                            L1Teleport.teleport(pc, ((L1EtcItem)l1iteminstance.Item).get_locx(), ((L1EtcItem)l1iteminstance.Item).get_locy(), ((L1EtcItem)l1iteminstance.Item).get_mapid(), 5, true);
                        }
                        else
                        {
                            // \f1沒有任何事情發生。
                            pc.sendPackets(new S_ServerMessage(79));
                        }
                    }
                    else if ((itemId == 40616) || (itemId == 40782) || (itemId == 40783))
                    { // 暗影神殿3樓鑰匙
                        if (((pc.X >= 32698) && (pc.X <= 32702)) && ((pc.Y >= 32894) && (pc.Y <= 32898)) && (pc.MapId == 523))
                        { // 影の神殿2階
                            L1Teleport.teleport(pc, ((L1EtcItem)l1iteminstance.Item).get_locx(), ((L1EtcItem)l1iteminstance.Item).get_locy(), ((L1EtcItem)l1iteminstance.Item).get_mapid(), 5, true);
                        }
                        else
                        {
                            // \f1沒有任何事情發生。
                            pc.sendPackets(new S_ServerMessage(79));
                        }
                    }
                    else if (itemId == 40692)
                    { // 完成的藏寶圖
                        if (pc.Inventory.checkItem(40621))
                        {
                            // \f1沒有任何事情發生。
                            pc.sendPackets(new S_ServerMessage(79));
                        }
                        else if (((pc.X >= 32856) && (pc.X <= 32858)) && ((pc.Y >= 32857) && (pc.Y <= 32858)) && (pc.MapId == 443))
                        { // 海賊島のダンジョン３階
                            L1Teleport.teleport(pc, ((L1EtcItem)l1iteminstance.Item).get_locx(), ((L1EtcItem)l1iteminstance.Item).get_locy(), ((L1EtcItem)l1iteminstance.Item).get_mapid(), 5, true);
                        }
                        else
                        {
                            // \f1沒有任何事情發生。
                            pc.sendPackets(new S_ServerMessage(79));
                        }
                    }
                    else if (itemId == 40101)
                    { // 指定傳送卷軸(隱藏之谷)
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei037"));
                    }
                    else if (itemId == 40383)
                    { // 地圖:歌唱之島
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei035"));
                    }
                    else if (itemId == 40384)
                    { // 地圖:隱藏之谷
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei036"));
                    }
                    else if (itemId == 40630)
                    { // 迪哥的舊日記
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "diegodiary"));
                    }
                    else if (itemId == 40641)
                    { // 說話卷軸
                        if (Config.ALT_TALKINGSCROLLQUEST == true)
                        {
                            if (pc.Quest.get_step(L1Quest.QUEST_TOSCROLL) == 0)
                            {
                                pc.sendPackets(new S_NPCTalkReturn(pc.Id, "tscrolla"));
                            }
                            else if (pc.Quest.get_step(L1Quest.QUEST_TOSCROLL) == 1)
                            {
                                pc.sendPackets(new S_NPCTalkReturn(pc.Id, "tscrollb"));
                            }
                            else if (pc.Quest.get_step(L1Quest.QUEST_TOSCROLL) == 2)
                            {
                                pc.sendPackets(new S_NPCTalkReturn(pc.Id, "tscrollc"));
                            }
                            else if (pc.Quest.get_step(L1Quest.QUEST_TOSCROLL) == 3)
                            {
                                pc.sendPackets(new S_NPCTalkReturn(pc.Id, "tscrolld"));
                            }
                            else if (pc.Quest.get_step(L1Quest.QUEST_TOSCROLL) == 4)
                            {
                                pc.sendPackets(new S_NPCTalkReturn(pc.Id, "tscrolle"));
                            }
                            else if (pc.Quest.get_step(L1Quest.QUEST_TOSCROLL) == 5)
                            {
                                pc.sendPackets(new S_NPCTalkReturn(pc.Id, "tscrollf"));
                            }
                            else if (pc.Quest.get_step(L1Quest.QUEST_TOSCROLL) == 6)
                            {
                                pc.sendPackets(new S_NPCTalkReturn(pc.Id, "tscrollg"));
                            }
                            else if (pc.Quest.get_step(L1Quest.QUEST_TOSCROLL) == 7)
                            {
                                pc.sendPackets(new S_NPCTalkReturn(pc.Id, "tscrollh"));
                            }
                            else if (pc.Quest.get_step(L1Quest.QUEST_TOSCROLL) == 8)
                            {
                                pc.sendPackets(new S_NPCTalkReturn(pc.Id, "tscrolli"));
                            }
                            else if (pc.Quest.get_step(L1Quest.QUEST_TOSCROLL) == 9)
                            {
                                pc.sendPackets(new S_NPCTalkReturn(pc.Id, "tscrollj"));
                            }
                            else if (pc.Quest.get_step(L1Quest.QUEST_TOSCROLL) == 10)
                            {
                                pc.sendPackets(new S_NPCTalkReturn(pc.Id, "tscrollk"));
                            }
                            else if (pc.Quest.get_step(L1Quest.QUEST_TOSCROLL) == 11)
                            {
                                pc.sendPackets(new S_NPCTalkReturn(pc.Id, "tscrolll"));
                            }
                            else if (pc.Quest.get_step(L1Quest.QUEST_TOSCROLL) == 12)
                            {
                                pc.sendPackets(new S_NPCTalkReturn(pc.Id, "tscrollm"));
                            }
                            else if (pc.Quest.get_step(L1Quest.QUEST_TOSCROLL) == 13)
                            {
                                pc.sendPackets(new S_NPCTalkReturn(pc.Id, "tscrolln"));
                            }
                            else if (pc.Quest.get_step(L1Quest.QUEST_TOSCROLL) == 255)
                            {
                                pc.sendPackets(new S_NPCTalkReturn(pc.Id, "tscrollo"));
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_NPCTalkReturn(pc.Id, "tscrollp"));
                        }
                    }
                    else if (itemId == 40663)
                    { // 兒子的信
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "sonsletter"));
                    }
                    else if (itemId == 40701)
                    { // 小藏寶圖
                        if (pc.Quest.get_step(L1Quest.QUEST_LUKEIN1) == 1)
                        {
                            pc.sendPackets(new S_NPCTalkReturn(pc.Id, "firsttmap"));
                        }
                        else if (pc.Quest.get_step(L1Quest.QUEST_LUKEIN1) == 2)
                        {
                            pc.sendPackets(new S_NPCTalkReturn(pc.Id, "secondtmapa"));
                        }
                        else if (pc.Quest.get_step(L1Quest.QUEST_LUKEIN1) == 3)
                        {
                            pc.sendPackets(new S_NPCTalkReturn(pc.Id, "secondtmapb"));
                        }
                        else if (pc.Quest.get_step(L1Quest.QUEST_LUKEIN1) == 4)
                        {
                            pc.sendPackets(new S_NPCTalkReturn(pc.Id, "secondtmapc"));
                        }
                        else if (pc.Quest.get_step(L1Quest.QUEST_LUKEIN1) == 5)
                        {
                            pc.sendPackets(new S_NPCTalkReturn(pc.Id, "thirdtmapd"));
                        }
                        else if (pc.Quest.get_step(L1Quest.QUEST_LUKEIN1) == 6)
                        {
                            pc.sendPackets(new S_NPCTalkReturn(pc.Id, "thirdtmape"));
                        }
                        else if (pc.Quest.get_step(L1Quest.QUEST_LUKEIN1) == 7)
                        {
                            pc.sendPackets(new S_NPCTalkReturn(pc.Id, "thirdtmapf"));
                        }
                        else if (pc.Quest.get_step(L1Quest.QUEST_LUKEIN1) == 8)
                        {
                            pc.sendPackets(new S_NPCTalkReturn(pc.Id, "thirdtmapg"));
                        }
                        else if (pc.Quest.get_step(L1Quest.QUEST_LUKEIN1) == 9)
                        {
                            pc.sendPackets(new S_NPCTalkReturn(pc.Id, "thirdtmaph"));
                        }
                        else if (pc.Quest.get_step(L1Quest.QUEST_LUKEIN1) == 10)
                        {
                            pc.sendPackets(new S_NPCTalkReturn(pc.Id, "thirdtmapi"));
                        }
                    }
                    else if (itemId == 41007)
                    { // 伊莉絲的命令書：靈魂之安息
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "erisscroll"));
                    }
                    else if (itemId == 41009)
                    { // 伊莉絲的命令書：同盟之意志
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "erisscroll2"));
                    }
                    else if (itemId == 41019)
                    { // 拉斯塔巴德歷史書第1頁
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "lashistory1"));
                    }
                    else if (itemId == 41020)
                    { // 拉斯塔巴德歷史書第2頁
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "lashistory2"));
                    }
                    else if (itemId == 41021)
                    { // 拉斯塔巴德歷史書第3頁
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "lashistory3"));
                    }
                    else if (itemId == 41022)
                    { // 拉斯塔巴德歷史書第4頁
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "lashistory4"));
                    }
                    else if (itemId == 41023)
                    { // 拉斯塔巴德歷史書第5頁
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "lashistory5"));
                    }
                    else if (itemId == 41024)
                    { // 拉斯塔巴德歷史書第6頁
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "lashistory6"));
                    }
                    else if (itemId == 41025)
                    { // 拉斯塔巴德歷史書第7頁
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "lashistory7"));
                    }
                    else if (itemId == 41026)
                    { // 拉斯塔巴德歷史書第8頁
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "lashistory8"));
                    }
                    else if (itemId == 41060)
                    { // 諾曼阿吐巴的信
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "nonames"));
                    }
                    else if (itemId == 41061)
                    { // 妖精調查書：卡麥都達瑪拉
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "kames"));
                    }
                    else if (itemId == 41062)
                    { // 人類調查書：巴庫摩那魯加
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "bakumos"));
                    }
                    else if (itemId == 41063)
                    { // 精靈調查書：可普都達瑪拉
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "bukas"));
                    }
                    else if (itemId == 41064)
                    { // 妖魔調查書：弧鄔牟那魯加
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "huwoomos"));
                    }
                    else if (itemId == 41065)
                    { // 死亡之樹調查書：諾亞阿吐巴
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "noas"));
                    }
                    else if (itemId == 41146)
                    { // ドロモンドの招待状
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei001"));
                    }
                    else if (itemId == 41209)
                    { // ポピレアの依頼書
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei002"));
                    }
                    else if (itemId == 41210)
                    { // 研磨材
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei003"));
                    }
                    else if (itemId == 41211)
                    { // ハーブ
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei004"));
                    }
                    else if (itemId == 41212)
                    { // 特製キャンディー
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei005"));
                    }
                    else if (itemId == 41213)
                    { // ティミーのバスケット
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei006"));
                    }
                    else if (itemId == 41214)
                    { // 運の証
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei012"));
                    }
                    else if (itemId == 41215)
                    { // 知の証
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei010"));
                    }
                    else if (itemId == 41216)
                    { // 力の証
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei011"));
                    }
                    else if (itemId == 41222)
                    { // マシュル
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei008"));
                    }
                    else if (itemId == 41223)
                    { // 武具の破片
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei007"));
                    }
                    else if (itemId == 41224)
                    { // バッジ
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei009"));
                    }
                    else if (itemId == 41225)
                    { // ケスキンの発注書
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei013"));
                    }
                    else if (itemId == 41226)
                    { // パゴの薬
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei014"));
                    }
                    else if (itemId == 41227)
                    { // アレックスの紹介状
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei033"));
                    }
                    else if (itemId == 41228)
                    { // ラビのお守り
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei034"));
                    }
                    else if (itemId == 41229)
                    { // スケルトンの頭
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei025"));
                    }
                    else if (itemId == 41230)
                    { // ジーナンへの手紙
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei020"));
                    }
                    else if (itemId == 41231)
                    { // マッティへの手紙
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei021"));
                    }
                    else if (itemId == 41233)
                    { // ケーイへの手紙
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei019"));
                    }
                    else if (itemId == 41234)
                    { // 骨の入った袋
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei023"));
                    }
                    else if (itemId == 41235)
                    { // 材料表
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei024"));
                    }
                    else if (itemId == 41236)
                    { // ボーンアーチャーの骨
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei026"));
                    }
                    else if (itemId == 41237)
                    { // スケルトンスパイクの骨
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei027"));
                    }
                    else if (itemId == 41239)
                    { // ヴートへの手紙
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei018"));
                    }
                    else if (itemId == 41240)
                    { // フェーダへの手紙
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "ei022"));
                    }
                    else if (itemId == 41356)
                    { // 波倫的資源清單
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "rparum3"));
                    }
                    else if (itemId == 41340)
                    { // 傭兵團長多文的推薦書
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "tion"));
                    }
                    else if (itemId == 41317)
                    { // 拉羅森的推薦書
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "rarson"));
                    }
                    else if (itemId == 41318)
                    { // 可恩的便條紙
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "kuen"));
                    }
                    else if (itemId == 41329)
                    { // 標本製作委託書
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "anirequest"));
                    }
                    else if (itemId == 41346)
                    { // 羅賓孫的便條紙1
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "robinscroll"));
                    }
                    else if (itemId == 41347)
                    { // 羅賓孫的便條紙2
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "robinscroll2"));
                    }
                    else if (itemId == 41348)
                    { // 羅賓孫的推薦書
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "robinhood"));
                    }
                    else if (itemId == 49172)
                    { // 希蓮恩的第一次信件
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "silrein1lt"));
                    }
                    else if (itemId == 49173)
                    { // 希蓮恩的第二次信件
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "silrein2lt"));
                    }
                    else if (itemId == 49174)
                    { // 希蓮恩的第三次信件
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "silrein3lt"));
                    }
                    else if (itemId == 49175)
                    { // 希蓮恩的第四次信件
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "silrein4lt"));
                    }
                    else if (itemId == 49176)
                    { // 希蓮恩的第五次信件
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "silrein5lt"));
                    }
                    else if (itemId == 49177)
                    { // 希蓮恩的第六次信件
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "silrein6lt"));
                    }
                    else if (itemId == 49202)
                    { // 時空裂痕邪念碎片
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "cot_ep1st"));
                    }
                    else if (itemId == 49206)
                    { // 塞維斯邪念碎片
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "bluesoul_p"));
                    }
                    else if (itemId == 49210)
                    { // 普洛凱爾的第一次指令書
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "first_p"));
                    }
                    else if (itemId == 49211)
                    { // 普洛凱爾的第二次指令書
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "second_p"));
                    }
                    else if (itemId == 49212)
                    { // 普洛凱爾的第三次指令書
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "third_p"));
                    }
                    else if (itemId == 49221)
                    { // 妖魔密使首領間諜書
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "spy_letter"));
                    }
                    else if (itemId == 49231)
                    { // 路西爾斯邪念碎片
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "redsoul_p"));
                    }
                    else if (itemId == 49287)
                    { // 普洛凱爾的第四次指令書
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "fourth_p"));
                    }
                    else if (itemId == 49288)
                    { // 普洛凱爾的第五次指令書
                        pc.sendPackets(new S_NPCTalkReturn(pc.Id, "fifth_p"));
                    }
                    else if (itemId == 41208)
                    { // 微弱的靈魂
                        if (((pc.X >= 32844) && (pc.X <= 32845)) && ((pc.Y >= 32693) && (pc.Y <= 32694)) && (pc.MapId == 550))
                        { // 船の墓場:地上層
                            L1Teleport.teleport(pc, ((L1EtcItem)l1iteminstance.Item).get_locx(), ((L1EtcItem)l1iteminstance.Item).get_locy(), ((L1EtcItem)l1iteminstance.Item).get_mapid(), 5, true);
                        }
                        else
                        {
                            // \f1沒有任何事情發生。
                            pc.sendPackets(new S_ServerMessage(79));
                        }
                    }
                    else if (itemId == 40700)
                    { // シルバーフルート
                        pc.sendPackets(new S_Sound(10));
                        pc.broadcastPacket(new S_Sound(10));
                        if (((pc.X >= 32619) && (pc.X <= 32623)) && ((pc.Y >= 33120) && (pc.Y <= 33124)) && (pc.MapId == 440))
                        { // 海賊島前半魔方陣座標
                            bool found = false;
                            foreach (GameObject obj in L1World.Instance.Object)
                            {
                                if (obj is L1MonsterInstance)
                                {
                                    L1MonsterInstance mob = (L1MonsterInstance)obj;
                                    if (mob != null)
                                    {
                                        if (mob.NpcTemplate.get_npcId() == 45875)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (found)
                            {
                            }
                            else
                            {
                                L1SpawnUtil.spawn(pc, 45875, 0, 0); // ラバーボーンヘッド
                            }
                        }
                    }
                    else if (itemId == 41121)
                    { // カヘルの契約書
                        if ((pc.Quest.get_step(L1Quest.QUEST_SHADOWS) == L1Quest.QUEST_END) || pc.Inventory.checkItem(41122, 1))
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                        else
                        {
                            createNewItem(pc, 41122, 1);
                        }
                    }
                    else if (itemId == 41130)
                    { // 血痕の契約書
                        if ((pc.Quest.get_step(L1Quest.QUEST_DESIRE) == L1Quest.QUEST_END) || pc.Inventory.checkItem(41131, 1))
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                        else
                        {
                            createNewItem(pc, 41131, 1);
                        }
                    }
                    else if (itemId == 42501)
                    { // 暴風疾走
                        if (pc.CurrentMp < 10)
                        {
                            pc.sendPackets(new S_ServerMessage(278)); // \f1MPが不足していて魔法を使うことができません。
                            return;
                        }
                        pc.CurrentMp = pc.CurrentMp - 10;
                        // pc.sendPackets(new S_CantMove()); // テレポート後に移動不可能になる場合がある
                        L1Teleport.teleport(pc, spellsc_x, spellsc_y, pc.MapId, pc.Heading, true, L1Teleport.CHANGE_POSITION);
                    }
                    else if ((itemId == 41293))
                    { // 魔法釣竿
                        startFishing(pc, itemId, fishX, fishY);
                    }
                    else if (itemId == 41245)
                    { // 溶解剤
                        useResolvent(pc, l1iteminstance1, l1iteminstance);
                    }
                    else if ((itemId >= 41255) && (itemId <= 41259))
                    { // 料理の本
                        if (cookStatus == 0)
                        {
                            pc.sendPackets(new S_PacketBox(S_PacketBox.COOK_WINDOW, (itemId - 41255)));
                        }
                        else
                        {
                            makeCooking(pc, cookNo);
                        }
                    }
                    else if (itemId == 41260)
                    { // 薪
                        foreach (GameObject @object in L1World.Instance.getVisibleObjects(pc, 3))
                        {
                            if (@object is L1EffectInstance)
                            {
                                if (((L1NpcInstance)@object).NpcTemplate.get_npcId() == 81170)
                                {
                                    // すでに周囲に焚き火があります。
                                    pc.sendPackets(new S_ServerMessage(1162));
                                    return;
                                }
                            }
                        }
                        int[] loc = new int[2];
                        loc = pc.FrontLoc;
                        L1EffectSpawn.Instance.spawnEffect(81170, 600000, loc[0], loc[1], pc.MapId);
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if (((itemId >= 41277) && (itemId <= 41292)) || ((itemId >= 49049) && (itemId <= 49064)) || ((itemId >= 49244) && (itemId <= 49259)) || itemId == L1ItemId.POTION_OF_WONDER_DRUG)
                    { // 魔法料理、象牙塔妙藥
                        L1Cooking.useCookingItem(pc, l1iteminstance);
                    }
                    else if (itemId == 41411)
                    { // 銀のチョンズ
                        Potion.UseHeallingPotion(pc, l1iteminstance, 10, 189);
                    }
                    else if (itemId == 41345)
                    { // 酸性の乳液
                        L1DamagePoison.doInfection(pc, pc, 3000, 5);
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if (itemId == 41315)
                    { // 聖水
                        if (pc.hasSkillEffect(L1SkillId.STATUS_HOLY_WATER_OF_EVA))
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                            return;
                        }
                        if (pc.hasSkillEffect(L1SkillId.STATUS_HOLY_MITHRIL_POWDER))
                        {
                            pc.removeSkillEffect(L1SkillId.STATUS_HOLY_MITHRIL_POWDER);
                        }
                        pc.setSkillEffect(L1SkillId.STATUS_HOLY_WATER, 900 * 1000);
                        pc.sendPackets(new S_SkillSound(pc.Id, 190));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 190));
                        pc.sendPackets(new S_ServerMessage(1141));
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if (itemId == 41316)
                    { // 神聖なミスリル パウダー
                        if (pc.hasSkillEffect(L1SkillId.STATUS_HOLY_WATER_OF_EVA))
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                            return;
                        }
                        if (pc.hasSkillEffect(L1SkillId.STATUS_HOLY_WATER))
                        {
                            pc.removeSkillEffect(L1SkillId.STATUS_HOLY_WATER);
                        }
                        pc.setSkillEffect(L1SkillId.STATUS_HOLY_MITHRIL_POWDER, 900 * 1000);
                        pc.sendPackets(new S_SkillSound(pc.Id, 190));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 190));
                        pc.sendPackets(new S_ServerMessage(1142));
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if (itemId == 41354)
                    { // 神聖なエヴァの水
                        if (pc.hasSkillEffect(L1SkillId.STATUS_HOLY_WATER) || pc.hasSkillEffect(L1SkillId.STATUS_HOLY_MITHRIL_POWDER))
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                            return;
                        }
                        pc.setSkillEffect(L1SkillId.STATUS_HOLY_WATER_OF_EVA, 900 * 1000);
                        pc.sendPackets(new S_SkillSound(pc.Id, 190));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 190));
                        pc.sendPackets(new S_ServerMessage(1140));
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if (itemId == 49168)
                    { // 破壞之密藥
                        pc.setSkillEffect(L1SkillId.SECRET_MEDICINE_OF_DESTRUCTION, 900 * 1000);
                        pc.sendPackets(new S_SkillSound(pc.Id, 190));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 190));
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if (itemId == 49092)
                    { // 龜裂之核
                        int targetItemId = l1iteminstance1.Item.ItemId;
                        // 上鎖的歐西里斯初級寶箱、上鎖的歐西里斯高級寶箱
                        if ((targetItemId == 49095) || (targetItemId == 49099) || (targetItemId == 49318) || (targetItemId == 49322))
                        {
                            createNewItem(pc, targetItemId + 1, 1);
                            pc.Inventory.consumeItem(targetItemId, 1);
                            pc.Inventory.consumeItem(49092, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 49094)
                    { // 歐西里斯初級寶箱碎片(下)
                        if (l1iteminstance1.Item.ItemId == 49093)
                        {
                            pc.Inventory.consumeItem(49093, 1);
                            pc.Inventory.consumeItem(49094, 1);
                            createNewItem(pc, 49095, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 49098)
                    { // 歐西里斯高級寶箱碎片(下)
                        if (l1iteminstance1.Item.ItemId == 49097)
                        {
                            pc.Inventory.consumeItem(49097, 1);
                            pc.Inventory.consumeItem(49098, 1);
                            createNewItem(pc, 49099, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 49317)
                    { // 庫庫爾坎初級寶箱碎片：下
                        if (l1iteminstance1.Item.ItemId == 49316)
                        {
                            pc.Inventory.consumeItem(49316, 1);
                            pc.Inventory.consumeItem(49317, 1);
                            createNewItem(pc, 49318, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 49321)
                    { // 庫庫爾坎高級寶箱碎片(下)
                        if (l1iteminstance1.Item.ItemId == 49320)
                        {
                            pc.Inventory.consumeItem(49320, 1);
                            pc.Inventory.consumeItem(49321, 1);
                            createNewItem(pc, 49322, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 49198)
                    { // 第二次邪念碎片
                        if (l1iteminstance1.Item.ItemId == 49197)
                        {
                            pc.Inventory.consumeItem(49197, 1);
                            pc.Inventory.consumeItem(49198, 1);
                            createNewItem(pc, 49200, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 49199)
                    { // 第三次邪念碎片
                        if (l1iteminstance1.Item.ItemId == 49200)
                        {
                            pc.Inventory.consumeItem(49199, 1);
                            pc.Inventory.consumeItem(49200, 1);
                            createNewItem(pc, 49201, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 49148)
                    { // 飾品強化卷軸
                        Enchant.scrollOfEnchantAccessory(pc, l1iteminstance, l1iteminstance1, client);
                    }
                    else if (itemId == 41426)
                    { // 封印スクロール
                        L1ItemInstance lockItem = pc.Inventory.getItem(l);
                        if (((lockItem != null) && (lockItem.Item.Type2 == 1)) || (lockItem.Item.Type2 == 2) || ((lockItem.Item.Type2 == 0) && lockItem.Item.CanSeal))
                        {
                            if ((lockItem.Bless == 0) || (lockItem.Bless == 1) || (lockItem.Bless == 2) || (lockItem.Bless == 3))
                            {
                                int bless = 1;
                                switch (lockItem.Bless)
                                {
                                    case 0:
                                        bless = 128;
                                        break;
                                    case 1:
                                        bless = 129;
                                        break;
                                    case 2:
                                        bless = 130;
                                        break;
                                    case 3:
                                        bless = 131;
                                        break;
                                }
                                lockItem.Bless = bless;
                                pc.Inventory.updateItem(lockItem, L1PcInventory.COL_BLESS);
                                pcInventory.saveItem(lockItem, L1PcInventory.COL_BLESS);
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 41427)
                    { // 封印解除スクロール
                        L1ItemInstance lockItem = pc.Inventory.getItem(l);
                        if (((lockItem != null) && (lockItem.Item.Type2 == 1)) || (lockItem.Item.Type2 == 2) || ((lockItem.Item.Type2 == 0) && lockItem.Item.CanSeal))
                        {
                            if ((lockItem.Bless == 128) || (lockItem.Bless == 129) || (lockItem.Bless == 130) || (lockItem.Bless == 131))
                            {
                                int bless = 1;
                                switch (lockItem.Bless)
                                {
                                    case 128:
                                        bless = 0;
                                        break;
                                    case 129:
                                        bless = 1;
                                        break;
                                    case 130:
                                        bless = 2;
                                        break;
                                    case 131:
                                        bless = 3;
                                        break;
                                }
                                lockItem.Bless = bless;
                                pc.Inventory.updateItem(lockItem, L1PcInventory.COL_BLESS);
                                pcInventory.saveItem(lockItem, L1PcInventory.COL_BLESS);
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 41428)
                    { // 太古の玉爾
                        if ((pc != null) && (l1iteminstance != null))
                        {
                            Account account = Account.Load(pc.AccountName);
                            if (account == null)
                            {
                                pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                                return;
                            }
                            int characterSlot = account.CharacterSlot;
                            int maxAmount = Config.DEFAULT_CHARACTER_SLOT + characterSlot;
                            if (maxAmount >= 8)
                            {
                                pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                                return;
                            }
                            if (characterSlot < 0)
                            {
                                characterSlot = 0;
                            }
                            else
                            {
                                characterSlot += 1;
                            }
                            account.CharacterSlot = characterSlot;
                            Account.UpdateCharacterSlot(account);
                            pc.Inventory.removeItem(l1iteminstance, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79));
                        }
                    }
                    else if (itemId == 40075)
                    { // 毀滅盔甲的卷軸
                        if (l1iteminstance1.Item.Type2 == 2)
                        {
                            int msg = 0;
                            switch (l1iteminstance1.Item.Type)
                            {
                                case 1: // helm
                                    msg = 171; // \f1你的鋼盔如爆炸般地破碎了。
                                    break;
                                case 2: // armor
                                    msg = 169; // \f1你的盔甲變成塵埃落地。
                                    break;
                                case 3: // T
                                    msg = 170; // \f1你的T恤破碎成線四散。
                                    break;
                                case 4: // cloak
                                    msg = 168; // \f1你的斗蓬破碎化為塵埃。
                                    break;
                                case 5: // glove
                                    msg = 172; // \f1你的手套消失。
                                    break;
                                case 6: // boots
                                    msg = 173; // \f1你的長靴分解。
                                    break;
                                case 7: // shield
                                    msg = 174; // \f1你的盾崩潰分散。
                                    break;
                                default:
                                    msg = 167; // \f1你的皮膚癢。
                                    break;
                            }
                            pc.sendPackets(new S_ServerMessage(msg));
                            pc.Inventory.removeItem(l1iteminstance1, 1);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(154)); // \f1這個卷軸散開了。
                        }
                        pc.Inventory.removeItem(l1iteminstance, 1);
                    }
                    else if (itemId == 49222)
                    { // 妖魔密使之笛子
                        if (pc.DragonKnight && (pc.MapId == 61))
                        {
                            bool found = false;
                            foreach (GameObject obj in L1World.Instance.Object)
                            {
                                if (obj is L1MonsterInstance)
                                {
                                    L1MonsterInstance mob = (L1MonsterInstance)obj;
                                    if (mob != null)
                                    {
                                        if (mob.NpcTemplate.get_npcId() == 46161)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (found)
                            {
                                pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                            }
                            else
                            {
                                L1SpawnUtil.spawn(pc, 46161, 0, 0); // オーク
                                                                    // 密使リーダー
                            }
                            pc.Inventory.consumeItem(49222, 1);
                        }
                    }
                    else if (itemId == 49188)
                    { // 索夏依卡靈魂之石
                        if (l1iteminstance1.Item.ItemId == 49186)
                        {
                            L1ItemInstance item1 = ItemTable.Instance.createItem(49189);
                            item1.Count = 1;
                            if (pc.Inventory.checkAddItem(item1, 1) == L1Inventory.OK)
                            {
                                pc.Inventory.storeItem(item1);
                                pc.sendPackets(new S_ServerMessage(403, item1.LogName));
                                pc.Inventory.removeItem(l1iteminstance, 1);
                                pc.Inventory.removeItem(l1iteminstance1, 1);
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                    }
                    else if (itemId == 49189)
                    { // 索夏依卡靈魂之笛
                        if (pc.Illusionist && (pc.MapId == 4))
                        { // 古魯丁祭壇
                            bool found = false;
                            foreach (GameObject obj in L1World.Instance.Object)
                            {
                                if (obj is L1MonsterInstance)
                                {
                                    L1MonsterInstance mob = (L1MonsterInstance)obj;
                                    if (mob != null)
                                    {
                                        if (mob.NpcTemplate.get_npcId() == 46163)
                                        { // 艾爾摩索夏依卡將軍的冤魂
                                            found = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (found)
                            {
                                pc.sendPackets(new S_ServerMessage(79));
                            }
                            else
                            {
                                L1SpawnUtil.spawn(pc, 46163, 0, 0);
                            }
                            pc.Inventory.consumeItem(49189, 1);
                        }
                    }
                    else if (itemId == 49201)
                    { // 完成的時間水晶球
                        if (pc.Illusionist && (pc.MapId == 4))
                        { // 火龍窟
                            bool found = false;
                            foreach (GameObject obj in L1World.Instance.Object)
                            {
                                if (obj is L1MonsterInstance)
                                {
                                    L1MonsterInstance mob = (L1MonsterInstance)obj;
                                    if (mob != null)
                                    {
                                        if (mob.NpcTemplate.get_npcId() == 81254)
                                        { // 時空裂痕
                                            found = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (found)
                            {
                                pc.sendPackets(new S_ServerMessage(79));
                            }
                            else
                            {
                                L1SpawnUtil.spawn(pc, 81254, 0, 0);
                            }
                            pc.Inventory.consumeItem(49201, 1);
                        }
                    }
                    else if (itemId == 49208)
                    { // 藍色之火碎片
                        if (pc.Illusionist && (pc.MapId == 2004))
                        { // 異界 奎斯特
                            bool found = false;
                            foreach (GameObject obj in L1World.Instance.Object)
                            {
                                if (obj is L1MonsterInstance)
                                {
                                    L1MonsterInstance mob = (L1MonsterInstance)obj;
                                    if (mob != null)
                                    {
                                        if (mob.NpcTemplate.get_npcId() == 81313)
                                        { // 塞維斯
                                            found = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (found)
                            {
                                pc.sendPackets(new S_ServerMessage(79));
                            }
                            else
                            {
                                L1SpawnUtil.spawn(pc, 81313, 0, 0);
                            }
                            pc.Inventory.consumeItem(49208, 1);
                        }
                    }
                    else if (itemId == 49227)
                    { // 紅色之火碎片
                        if (pc.DragonKnight && (pc.MapId == 2004))
                        { // 異界 奎斯特
                            bool found = false;
                            foreach (GameObject obj in L1World.Instance.Object)
                            {
                                if (obj is L1MonsterInstance)
                                {
                                    L1MonsterInstance mob = (L1MonsterInstance)obj;
                                    if (mob != null)
                                    {
                                        if (mob.NpcTemplate.get_npcId() == 81312)
                                        { // 路西爾斯
                                            found = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (found)
                            {
                                pc.sendPackets(new S_ServerMessage(79));
                            }
                            else
                            {
                                L1SpawnUtil.spawn(pc, 81312, 0, 0);
                            }
                            pc.Inventory.consumeItem(49227, 1);
                        }
                    }
                    else if (itemId == 49167)
                    { // 魔之角笛
                        if (pc.Crown && (pc.MapId == 2000) && (pc.X == 32807) && (pc.Y == 32773))
                        {
                            bool found = false;
                            foreach (GameObject obj in L1World.Instance.Object)
                            {
                                if (obj is L1MonsterInstance)
                                {
                                    L1MonsterInstance mob = (L1MonsterInstance)obj;
                                    if (mob != null)
                                    {
                                        if (mob.NpcTemplate.get_npcId() == 81323)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (found)
                            {
                                pc.sendPackets(new S_ServerMessage(79));
                            }
                            else
                            {
                                L1SpawnUtil.spawn(pc, 81323, 0, 0);
                            }
                        }
                        else if (pc.Knight && (pc.MapId == 2001) && (pc.X == 32807) && (pc.Y == 32773))
                        {
                            bool found = false;
                            foreach (GameObject obj in L1World.Instance.Object)
                            {
                                if (obj is L1MonsterInstance)
                                {
                                    L1MonsterInstance mob = (L1MonsterInstance)obj;
                                    if (mob != null)
                                    {
                                        if (mob.NpcTemplate.get_npcId() == 81324)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (found)
                            {
                                pc.sendPackets(new S_ServerMessage(79));
                            }
                            else
                            {
                                L1SpawnUtil.spawn(pc, 81324, 0, 0);
                            }
                        }
                        else if (pc.Elf && (pc.MapId == 2002) && (pc.X == 32807) && (pc.Y == 32773))
                        {
                            bool found = false;
                            foreach (GameObject obj in L1World.Instance.Object)
                            {
                                if (obj is L1MonsterInstance)
                                {
                                    L1MonsterInstance mob = (L1MonsterInstance)obj;
                                    if (mob != null)
                                    {
                                        if (mob.NpcTemplate.get_npcId() == 81325)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (found)
                            {
                                pc.sendPackets(new S_ServerMessage(79));
                            }
                            else
                            {
                                L1SpawnUtil.spawn(pc, 81325, 0, 0);
                            }
                        }
                        else if (pc.Wizard && (pc.MapId == 2003) && (pc.X == 32807) && (pc.Y == 32773))
                        {
                            bool found = false;
                            foreach (GameObject obj in L1World.Instance.Object)
                            {
                                if (obj is L1MonsterInstance)
                                {
                                    L1MonsterInstance mob = (L1MonsterInstance)obj;
                                    if (mob != null)
                                    {
                                        if (mob.NpcTemplate.get_npcId() == 81326)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (found)
                            {
                                pc.sendPackets(new S_ServerMessage(79));
                            }
                            else
                            {
                                L1SpawnUtil.spawn(pc, 81326, 0, 0);
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                        }
                        pc.Inventory.consumeItem(49167, 1);
                    }
                    else if (itemId == 47010)
                    { // 龍之鑰匙
                        if (!L1CastleLocation.checkInAllWarArea(pc.Location))
                        { // 檢查是否在城堡區域內
                            pc.sendPackets(new S_DragonGate(pc, L1DragonSlayer.Instance.checkDragonPortal()));
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(79)); // 沒有任何事情發生
                        }
                    }
                    else
                    {
                        int locX = ((L1EtcItem)l1iteminstance.Item).get_locx();
                        int locY = ((L1EtcItem)l1iteminstance.Item).get_locy();
                        short mapId = ((L1EtcItem)l1iteminstance.Item).get_mapid();
                        if ((locX != 0) && (locY != 0))
                        { // 各種テレポートスクロール
                            if (pc.Map.Escapable || pc.Gm)
                            {
                                L1Teleport.teleport(pc, locX, locY, mapId, pc.Heading, true);
                                pc.Inventory.removeItem(l1iteminstance, 1);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(647));
                            }
                        }
                        else
                        {
                            if (l1iteminstance.Count < 1)
                            { // あり得ない？
                                pc.sendPackets(new S_ServerMessage(329, l1iteminstance.LogName)); // \f1%0を持っていません。
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(74, l1iteminstance.LogName)); // \f1%0は使用できません。
                            }
                        }
                    }
                }
                else if (l1iteminstance.Item.Type2 == 1)
                {
                    // 種別：武器
                    int min = l1iteminstance.Item.MinLevel;
                    int max = l1iteminstance.Item.MaxLevel;
                    if ((min != 0) && (min > pc.Level))
                    {
                        // 等級 %0以上才可使用此道具。
                        pc.sendPackets(new S_ServerMessage(318, min.ToString()));
                    }
                    else if ((max != 0) && (max < pc.Level))
                    {
                        pc.sendPackets(new S_PacketBox(S_PacketBox.MSG_LEVEL_OVER, max)); // 等級%d以下才能使用此道具。
                    }
                    else
                    {
                        if ((pc.Crown && l1iteminstance.Item.UseRoyal) || (pc.Knight && l1iteminstance.Item.UseKnight) || (pc.Elf && l1iteminstance.Item.UseElf) || (pc.Wizard && l1iteminstance.Item.UseMage) || (pc.Darkelf && l1iteminstance.Item.UseDarkelf) || (pc.DragonKnight && l1iteminstance.Item.UseDragonknight) || (pc.Illusionist && l1iteminstance.Item.UseIllusionist))
                        {
                            UseWeapon(pc, l1iteminstance);
                        }
                        else
                        {
                            // \f1你的職業無法使用此道具。
                            pc.sendPackets(new S_ServerMessage(264));
                        }
                    }
                }
                else if (l1iteminstance.Item.Type2 == 2)
                { // 種別：防具
                    if ((pc.Crown && l1iteminstance.Item.UseRoyal) || (pc.Knight && l1iteminstance.Item.UseKnight) || (pc.Elf && l1iteminstance.Item.UseElf) || (pc.Wizard && l1iteminstance.Item.UseMage) || (pc.Darkelf && l1iteminstance.Item.UseDarkelf) || (pc.DragonKnight && l1iteminstance.Item.UseDragonknight) || (pc.Illusionist && l1iteminstance.Item.UseIllusionist))
                    {

                        int min = ((L1Armor)l1iteminstance.Item).MinLevel;
                        int max = ((L1Armor)l1iteminstance.Item).MaxLevel;
                        if ((min != 0) && (min > pc.Level))
                        {
                            // 等級 %0以上才可使用此道具。
                            pc.sendPackets(new S_ServerMessage(318, min.ToString()));
                        }
                        else if ((max != 0) && (max < pc.Level))
                        {
                            pc.sendPackets(new S_PacketBox(S_PacketBox.MSG_LEVEL_OVER, max)); // 等級%d以下才能使用此道具。
                        }
                        else
                        {
                            UseArmor(pc, l1iteminstance);
                        }
                    }
                    else
                    {
                        // \f1你的職業無法使用此道具。
                        pc.sendPackets(new S_ServerMessage(264));
                    }
                }

                // 効果ディレイがある場合は現在時間をセット
                if (isDelayEffect)
                {
                    l1iteminstance.LastUsed = DateTime.Now;
                    pc.Inventory.updateItem(l1iteminstance, L1PcInventory.COL_DELAY_EFFECT);
                    pcInventory.saveItem(l1iteminstance, L1PcInventory.COL_DELAY_EFFECT);
                }

                L1ItemDelay.onItemUse(client, l1iteminstance); // アイテムディレイ開始
            }
        }

        private bool usePolyScroll(L1PcInstance pc, int item_id, string s)
        {
            int awakeSkillId = pc.AwakeSkillId;
            if ((awakeSkillId == L1SkillId.AWAKEN_ANTHARAS) ||
                (awakeSkillId == L1SkillId.AWAKEN_FAFURION) ||
                (awakeSkillId == L1SkillId.AWAKEN_VALAKAS))
            {
                pc.sendPackets(new S_ServerMessage(1384)); // 目前狀態中無法變身。
                return false;
            }

            int time = 0;
            if ((item_id == 40088) || (item_id == 40096))
            { // 變形卷軸、象牙塔變形卷軸
                time = 1800;
            }
            else if (item_id == 49308)
            { // 福利變形藥水
                time = RandomHelper.Next(2401, 4800);
            }
            else if (item_id == 140088)
            { // 受祝福的 變形卷軸
                time = 2100;
            }

            L1PolyMorph poly = PolyTable.Instance.getTemplate(s);
            if ((poly != null) || s.Equals(""))
            {
                if (s.Equals(""))
                {
                    if ((pc.TempCharGfx == 6034) || (pc.TempCharGfx == 6035))
                    {
                        return true;
                    }
                    else
                    {
                        pc.removeSkillEffect(L1SkillId.SHAPE_CHANGE);
                        return true;
                    }
                }
                else if ((poly.MinLevel <= pc.Level) || pc.Gm)
                {
                    L1PolyMorph.doPoly(pc, poly.PolyId, time, L1PolyMorph.MORPH_BY_ITEMMAGIC);
                    return true;
                }
            }
            return false;
        }

        private void usePolyScale(L1PcInstance pc, int itemId)
        {
            int time = 900;
            int awakeSkillId = pc.AwakeSkillId;
            if ((awakeSkillId == L1SkillId.AWAKEN_ANTHARAS) ||
                (awakeSkillId == L1SkillId.AWAKEN_FAFURION) ||
                (awakeSkillId == L1SkillId.AWAKEN_VALAKAS))
            {
                pc.sendPackets(new S_ServerMessage(1384)); // 目前狀態中無法變身。
                return;
            }

            int polyId = 0;
            if (itemId == 41154)
            { // 暗之鱗
                polyId = 3101;
            }
            else if (itemId == 41155)
            { // 火之鱗
                polyId = 3126;
            }
            else if (itemId == 41156)
            { // 叛之鱗
                polyId = 3888;
            }
            else if (itemId == 41157)
            { // 恨之鱗
                polyId = 3784;
            }
            else if (itemId == 49220)
            { // 妖魔密使變形卷軸
                polyId = 6984;
                time = 1200;
            }
            L1PolyMorph.doPoly(pc, polyId, time, L1PolyMorph.MORPH_BY_ITEMMAGIC);
        }

        private void usePolyPotion(L1PcInstance pc, int itemId)
        {
            int time = 1800;
            int awakeSkillId = pc.AwakeSkillId;
            if ((awakeSkillId == L1SkillId.AWAKEN_ANTHARAS) ||
                (awakeSkillId == L1SkillId.AWAKEN_FAFURION) ||
                (awakeSkillId == L1SkillId.AWAKEN_VALAKAS))
            {
                pc.sendPackets(new S_ServerMessage(1384)); // 目前狀態中無法變身。
                return;
            }

            int polyId = 0;
            if (itemId == 41143)
            { // 海賊骷髏首領變身藥水
                polyId = 6086;
                time = 900;
            }
            else if (itemId == 41144)
            { // 海賊骷髏士兵變身藥水
                polyId = 6087;
                time = 900;
            }
            else if (itemId == 41145)
            { // 海賊骷髏刀手變身藥水
                polyId = 6088;
                time = 900;
            }
            else if ((itemId == 49149) && (pc.get_sex() == 0) && pc.Crown)
            { // 夏納的變身卷軸(等級30)
                polyId = 6822;
            }
            else if ((itemId == 49149) && (pc.get_sex() == 1) && pc.Crown)
            {
                polyId = 6823;
            }
            else if ((itemId == 49149) && (pc.get_sex() == 0) && pc.Knight)
            {
                polyId = 6824;
            }
            else if ((itemId == 49149) && (pc.get_sex() == 1) && pc.Knight)
            {
                polyId = 6825;
            }
            else if ((itemId == 49149) && (pc.get_sex() == 0) && pc.Elf)
            {
                polyId = 6826;
            }
            else if ((itemId == 49149) && (pc.get_sex() == 1) && pc.Elf)
            {
                polyId = 6827;
            }
            else if ((itemId == 49149) && (pc.get_sex() == 0) && pc.Wizard)
            {
                polyId = 6828;
            }
            else if ((itemId == 49149) && (pc.get_sex() == 1) && pc.Wizard)
            {
                polyId = 6829;
            }
            else if ((itemId == 49149) && (pc.get_sex() == 0) && pc.Darkelf)
            {
                polyId = 6830;
            }
            else if ((itemId == 49149) && (pc.get_sex() == 1) && pc.Darkelf)
            {
                polyId = 6831;
            }
            else if ((itemId == 49149) && (pc.get_sex() == 0) && pc.DragonKnight)
            {
                polyId = 7139;
            }
            else if ((itemId == 49149) && (pc.get_sex() == 1) && pc.DragonKnight)
            {
                polyId = 7140;
            }
            else if ((itemId == 49149) && (pc.get_sex() == 0) && pc.Illusionist)
            {
                polyId = 7141;
            }
            else if ((itemId == 49149) && (pc.get_sex() == 1) && pc.Illusionist)
            {
                polyId = 7142;
            }
            else if ((itemId == 49150) && (pc.get_sex() == 0) && pc.Crown)
            { // 夏納的變身卷軸(等級40)
                polyId = 6832;
            }
            else if ((itemId == 49150) && (pc.get_sex() == 1) && pc.Crown)
            {
                polyId = 6833;
            }
            else if ((itemId == 49150) && (pc.get_sex() == 0) && pc.Knight)
            {
                polyId = 6834;
            }
            else if ((itemId == 49150) && (pc.get_sex() == 1) && pc.Knight)
            {
                polyId = 6835;
            }
            else if ((itemId == 49150) && (pc.get_sex() == 0) && pc.Elf)
            {
                polyId = 6836;
            }
            else if ((itemId == 49150) && (pc.get_sex() == 1) && pc.Elf)
            {
                polyId = 6837;
            }
            else if ((itemId == 49150) && (pc.get_sex() == 0) && pc.Wizard)
            {
                polyId = 6838;
            }
            else if ((itemId == 49150) && (pc.get_sex() == 1) && pc.Wizard)
            {
                polyId = 6839;
            }
            else if ((itemId == 49150) && (pc.get_sex() == 0) && pc.Darkelf)
            {
                polyId = 6840;
            }
            else if ((itemId == 49150) && (pc.get_sex() == 1) && pc.Darkelf)
            {
                polyId = 6841;
            }
            else if ((itemId == 49150) && (pc.get_sex() == 0) && pc.DragonKnight)
            {
                polyId = 7143;
            }
            else if ((itemId == 49150) && (pc.get_sex() == 1) && pc.DragonKnight)
            {
                polyId = 7144;
            }
            else if ((itemId == 49150) && (pc.get_sex() == 0) && pc.Illusionist)
            {
                polyId = 7145;
            }
            else if ((itemId == 49150) && (pc.get_sex() == 1) && pc.Illusionist)
            {
                polyId = 7146;
            }
            else if ((itemId == 49151) && (pc.get_sex() == 0) && pc.Crown)
            { // 夏納的變身卷軸(等級52)
                polyId = 6842;
            }
            else if ((itemId == 49151) && (pc.get_sex() == 1) && pc.Crown)
            {
                polyId = 6843;
            }
            else if ((itemId == 49151) && (pc.get_sex() == 0) && pc.Knight)
            {
                polyId = 6844;
            }
            else if ((itemId == 49151) && (pc.get_sex() == 1) && pc.Knight)
            {
                polyId = 6845;
            }
            else if ((itemId == 49151) && (pc.get_sex() == 0) && pc.Elf)
            {
                polyId = 6846;
            }
            else if ((itemId == 49151) && (pc.get_sex() == 1) && pc.Elf)
            {
                polyId = 6847;
            }
            else if ((itemId == 49151) && (pc.get_sex() == 0) && pc.Wizard)
            {
                polyId = 6848;
            }
            else if ((itemId == 49151) && (pc.get_sex() == 1) && pc.Wizard)
            {
                polyId = 6849;
            }
            else if ((itemId == 49151) && (pc.get_sex() == 0) && pc.Darkelf)
            {
                polyId = 6850;
            }
            else if ((itemId == 49151) && (pc.get_sex() == 1) && pc.Darkelf)
            {
                polyId = 6851;
            }
            else if ((itemId == 49151) && (pc.get_sex() == 0) && pc.DragonKnight)
            {
                polyId = 7147;
            }
            else if ((itemId == 49151) && (pc.get_sex() == 1) && pc.DragonKnight)
            {
                polyId = 7148;
            }
            else if ((itemId == 49151) && (pc.get_sex() == 0) && pc.Illusionist)
            {
                polyId = 7149;
            }
            else if ((itemId == 49151) && (pc.get_sex() == 1) && pc.Illusionist)
            {
                polyId = 7150;
            }
            else if ((itemId == 49152) && (pc.get_sex() == 0) && pc.Crown)
            { // 夏納的變身卷軸(等級55)
                polyId = 6852;
            }
            else if ((itemId == 49152) && (pc.get_sex() == 1) && pc.Crown)
            {
                polyId = 6853;
            }
            else if ((itemId == 49152) && (pc.get_sex() == 0) && pc.Knight)
            {
                polyId = 6854;
            }
            else if ((itemId == 49152) && (pc.get_sex() == 1) && pc.Knight)
            {
                polyId = 6855;
            }
            else if ((itemId == 49152) && (pc.get_sex() == 0) && pc.Elf)
            {
                polyId = 6856;
            }
            else if ((itemId == 49152) && (pc.get_sex() == 1) && pc.Elf)
            {
                polyId = 6857;
            }
            else if ((itemId == 49152) && (pc.get_sex() == 0) && pc.Wizard)
            {
                polyId = 6858;
            }
            else if ((itemId == 49152) && (pc.get_sex() == 1) && pc.Wizard)
            {
                polyId = 6859;
            }
            else if ((itemId == 49152) && (pc.get_sex() == 0) && pc.Darkelf)
            {
                polyId = 6860;
            }
            else if ((itemId == 49152) && (pc.get_sex() == 1) && pc.Darkelf)
            {
                polyId = 6861;
            }
            else if ((itemId == 49152) && (pc.get_sex() == 0) && pc.DragonKnight)
            {
                polyId = 7151;
            }
            else if ((itemId == 49152) && (pc.get_sex() == 1) && pc.DragonKnight)
            {
                polyId = 7152;
            }
            else if ((itemId == 49152) && (pc.get_sex() == 0) && pc.Illusionist)
            {
                polyId = 7153;
            }
            else if ((itemId == 49152) && (pc.get_sex() == 1) && pc.Illusionist)
            {
                polyId = 7154;
            }
            else if ((itemId == 49153) && (pc.get_sex() == 0) && pc.Crown)
            { // 夏納的變身卷軸(等級60)
                polyId = 6862;
            }
            else if ((itemId == 49153) && (pc.get_sex() == 1) && pc.Crown)
            {
                polyId = 6863;
            }
            else if ((itemId == 49153) && (pc.get_sex() == 0) && pc.Knight)
            {
                polyId = 6864;
            }
            else if ((itemId == 49153) && (pc.get_sex() == 1) && pc.Knight)
            {
                polyId = 6865;
            }
            else if ((itemId == 49153) && (pc.get_sex() == 0) && pc.Elf)
            {
                polyId = 6866;
            }
            else if ((itemId == 49153) && (pc.get_sex() == 1) && pc.Elf)
            {
                polyId = 6867;
            }
            else if ((itemId == 49153) && (pc.get_sex() == 0) && pc.Wizard)
            {
                polyId = 6868;
            }
            else if ((itemId == 49153) && (pc.get_sex() == 1) && pc.Wizard)
            {
                polyId = 6869;
            }
            else if ((itemId == 49153) && (pc.get_sex() == 0) && pc.Darkelf)
            {
                polyId = 6870;
            }
            else if ((itemId == 49153) && (pc.get_sex() == 1) && pc.Darkelf)
            {
                polyId = 6871;
            }
            else if ((itemId == 49153) && (pc.get_sex() == 0) && pc.DragonKnight)
            {
                polyId = 7155;
            }
            else if ((itemId == 49153) && (pc.get_sex() == 1) && pc.DragonKnight)
            {
                polyId = 7156;
            }
            else if ((itemId == 49153) && (pc.get_sex() == 0) && pc.Illusionist)
            {
                polyId = 7157;
            }
            else if ((itemId == 49153) && (pc.get_sex() == 1) && pc.Illusionist)
            {
                polyId = 7158;
            }
            else if ((itemId == 49154) && (pc.get_sex() == 0) && pc.Crown)
            { // 夏納的變身卷軸(等級65)
                polyId = 6872;
            }
            else if ((itemId == 49154) && (pc.get_sex() == 1) && pc.Crown)
            {
                polyId = 6873;
            }
            else if ((itemId == 49154) && (pc.get_sex() == 0) && pc.Knight)
            {
                polyId = 6874;
            }
            else if ((itemId == 49154) && (pc.get_sex() == 1) && pc.Knight)
            {
                polyId = 6875;
            }
            else if ((itemId == 49154) && (pc.get_sex() == 0) && pc.Elf)
            {
                polyId = 6876;
            }
            else if ((itemId == 49154) && (pc.get_sex() == 1) && pc.Elf)
            {
                polyId = 6877;
            }
            else if ((itemId == 49154) && (pc.get_sex() == 0) && pc.Wizard)
            {
                polyId = 6878;
            }
            else if ((itemId == 49154) && (pc.get_sex() == 1) && pc.Wizard)
            {
                polyId = 6879;
            }
            else if ((itemId == 49154) && (pc.get_sex() == 0) && pc.Darkelf)
            {
                polyId = 6880;
            }
            else if ((itemId == 49154) && (pc.get_sex() == 1) && pc.Darkelf)
            {
                polyId = 6881;
            }
            else if ((itemId == 49154) && (pc.get_sex() == 0) && pc.DragonKnight)
            {
                polyId = 7159;
            }
            else if ((itemId == 49154) && (pc.get_sex() == 1) && pc.DragonKnight)
            {
                polyId = 7160;
            }
            else if ((itemId == 49154) && (pc.get_sex() == 0) && pc.Illusionist)
            {
                polyId = 7161;
            }
            else if ((itemId == 49154) && (pc.get_sex() == 1) && pc.Illusionist)
            {
                polyId = 7162;
            }
            else if ((itemId == 49155) && (pc.get_sex() == 0) && pc.Crown)
            { // 夏納的變身卷軸(等級70)
                polyId = 6882;
            }
            else if ((itemId == 49155) && (pc.get_sex() == 1) && pc.Crown)
            {
                polyId = 6883;
            }
            else if ((itemId == 49155) && (pc.get_sex() == 0) && pc.Knight)
            {
                polyId = 6884;
            }
            else if ((itemId == 49155) && (pc.get_sex() == 1) && pc.Knight)
            {
                polyId = 6885;
            }
            else if ((itemId == 49155) && (pc.get_sex() == 0) && pc.Elf)
            {
                polyId = 6886;
            }
            else if ((itemId == 49155) && (pc.get_sex() == 1) && pc.Elf)
            {
                polyId = 6887;
            }
            else if ((itemId == 49155) && (pc.get_sex() == 0) && pc.Wizard)
            {
                polyId = 6888;
            }
            else if ((itemId == 49155) && (pc.get_sex() == 1) && pc.Wizard)
            {
                polyId = 6889;
            }
            else if ((itemId == 49155) && (pc.get_sex() == 0) && pc.Darkelf)
            {
                polyId = 6890;
            }
            else if ((itemId == 49155) && (pc.get_sex() == 1) && pc.Darkelf)
            {
                polyId = 6891;
            }
            else if ((itemId == 49155) && (pc.get_sex() == 0) && pc.DragonKnight)
            {
                polyId = 7163;
            }
            else if ((itemId == 49155) && (pc.get_sex() == 1) && pc.DragonKnight)
            {
                polyId = 7164;
            }
            else if ((itemId == 49155) && (pc.get_sex() == 0) && pc.Illusionist)
            {
                polyId = 7165;
            }
            else if ((itemId == 49155) && (pc.get_sex() == 1) && pc.Illusionist)
            {
                polyId = 7166;
            }
            else if ((itemId == 49139))
            { // 起司蛋糕
                polyId = 6137; // 52級死亡騎士
                time = 900;
            }
            L1PolyMorph.doPoly(pc, polyId, time, L1PolyMorph.MORPH_BY_ITEMMAGIC);
        }

        private void UseArmor(L1PcInstance activeChar, L1ItemInstance armor)
        {
            int type = armor.Item.Type;
            L1PcInventory pcInventory = activeChar.Inventory as L1PcInventory;
            bool equipeSpace; // 装備する箇所が空いているか
            if (type == 9)
            { // リングの場合
                equipeSpace = pcInventory.getTypeEquipped(2, 9) <= 1;
            }
            else
            {
                equipeSpace = pcInventory.getTypeEquipped(2, type) <= 0;
            }

            if (equipeSpace && !armor.Equipped)
            { // 使用した防具を装備していなくて、その装備箇所が空いている場合（装着を試みる）
                int polyid = activeChar.TempCharGfx;

                if (!L1PolyMorph.isEquipableArmor(polyid, type))
                { // その変身では装備不可
                    return;
                }

                if (((type == 13) && (pcInventory.getTypeEquipped(2, 7) >= 1)) || ((type == 7) && (pcInventory.getTypeEquipped(2, 13) >= 1)))
                { // シールド、ガーダー同時裝備不可
                    activeChar.sendPackets(new S_ServerMessage(124)); // \f1已經裝備其他東西。
                    return;
                }
                if ((type == 7) && (activeChar.Weapon != null))
                { // シールドの場合、武器を装備していたら両手武器チェック
                    if (activeChar.Weapon.Item.TwohandedWeapon)
                    { // 両手武器
                        activeChar.sendPackets(new S_ServerMessage(129)); // \f1両手の武器を武装したままシールドを着用することはできません。
                        return;
                    }
                }

                if ((type == 3) && (pcInventory.getTypeEquipped(2, 4) >= 1))
                { // シャツの場合、マントを着てないか確認
                    activeChar.sendPackets(new S_ServerMessage(126, "$224", "$225")); // \f1%1上に%0を着ることはできません。
                    return;
                }
                else if ((type == 3) && (pcInventory.getTypeEquipped(2, 2) >= 1))
                { // シャツの場合、メイルを着てないか確認
                    activeChar.sendPackets(new S_ServerMessage(126, "$224", "$226")); // \f1%1上に%0を着ることはできません。
                    return;
                }
                else if ((type == 2) && (pcInventory.getTypeEquipped(2, 4) >= 1))
                { // メイルの場合、マントを着てないか確認
                    activeChar.sendPackets(new S_ServerMessage(126, "$226", "$225")); // \f1%1上に%0を着ることはできません。
                    return;
                }

                pcInventory.setEquipped(armor, true);
            }
            else if (armor.Equipped)
            { // 使用した防具を装備していた場合（脱着を試みる）
                if (armor.Item.Bless == 2)
                { // 呪われていた場合
                    activeChar.sendPackets(new S_ServerMessage(150)); // \f1はずすことができません。呪いをかけられているようです。
                    return;
                }
                if ((type == 3) && (pcInventory.getTypeEquipped(2, 2) >= 1))
                { // シャツの場合、メイルを着てないか確認
                    activeChar.sendPackets(new S_ServerMessage(127)); // \f1それは脱ぐことができません。
                    return;
                }
                else if (((type == 2) || (type == 3)) && (pcInventory.getTypeEquipped(2, 4) >= 1))
                { // シャツとメイルの場合、マントを着てないか確認
                    activeChar.sendPackets(new S_ServerMessage(127)); // \f1それは脱ぐことができません。
                    return;
                }
                if (type == 7)
                { // シールドの場合、ソリッドキャリッジの効果消失
                    if (activeChar.hasSkillEffect(L1SkillId.SOLID_CARRIAGE))
                    {
                        activeChar.removeSkillEffect(L1SkillId.SOLID_CARRIAGE);
                    }
                }
                pcInventory.setEquipped(armor, false);
            }
            else
            {
                activeChar.sendPackets(new S_ServerMessage(124)); // \f1已經裝備其他東西。
            }
            // セット装備用HP、MP、MR更新
            activeChar.CurrentHp = activeChar.CurrentHp;
            activeChar.CurrentMp = activeChar.CurrentMp;
            activeChar.sendPackets(new S_OwnCharAttrDef(activeChar));
            activeChar.sendPackets(new S_OwnCharStatus(activeChar));
            activeChar.sendPackets(new S_SPMR(activeChar));
        }

        private void UseWeapon(L1PcInstance activeChar, L1ItemInstance weapon)
        {
            L1PcInventory pcInventory = activeChar.Inventory as L1PcInventory;
            if ((activeChar.Weapon == null) || !activeChar.Weapon.Equals(weapon))
            { // 指定された武器が装備している武器と違う場合、装備できるか確認
                int weapon_type = weapon.Item.Type;
                int polyid = activeChar.TempCharGfx;

                if (!L1PolyMorph.isEquipableWeapon(polyid, weapon_type))
                { // その変身では装備不可
                    return;
                }
                if (weapon.Item.TwohandedWeapon && (pcInventory.getTypeEquipped(2, 7) >= 1))
                { // 両手武器の場合、シールド装備の確認
                    activeChar.sendPackets(new S_ServerMessage(128)); // \f1當你拿著一個盾時，你無法使用雙手武器。
                    return;
                }
            }

            if (activeChar.Weapon != null)
            { // 既に何かを装備している場合、前の装備をはずす
                if (activeChar.Weapon.Item.Bless == 2)
                { // 呪われていた場合
                    activeChar.sendPackets(new S_ServerMessage(150)); // \f1はずすことができません。呪いをかけられているようです。
                    return;
                }
                if (activeChar.Weapon.Equals(weapon))
                {
                    // 装備交換ではなく外すだけ
                    pcInventory.setEquipped(activeChar.Weapon, false, false, false);
                    return;
                }
                else
                {
                    pcInventory.setEquipped(activeChar.Weapon, false, false, true);
                }
            }

            if (weapon.ItemId == 200002)
            { // 呪われたダイスダガー
                activeChar.sendPackets(new S_ServerMessage(149, weapon.LogName)); // \f1%0が手にくっつきました。
            }
            pcInventory.setEquipped(weapon, true, false, false);
        }

        private void useSpellBook(L1PcInstance pc, L1ItemInstance item, int itemId)
        {
            int itemAttr = 0;
            int locAttr = 0; // 0:other 1:law 2:chaos
            bool isLawful = true;
            int pcX = pc.X;
            int pcY = pc.Y;
            int mapId = pc.MapId;
            int level = pc.Level;
            if ((itemId == 45000) || (itemId == 45008) || (itemId == 45018) || (itemId == 45021) || (itemId == 40171) || (itemId == 40179) || (itemId == 40180) || (itemId == 40182) || (itemId == 40194) || (itemId == 40197) || (itemId == 40202) || (itemId == 40206) || (itemId == 40213) || (itemId == 40220) || (itemId == 40222))
            {
                itemAttr = 1;
            }
            if ((itemId == 45009) || (itemId == 45010) || (itemId == 45019) || (itemId == 40172) || (itemId == 40173) || (itemId == 40178) || (itemId == 40185) || (itemId == 40186) || (itemId == 40192) || (itemId == 40196) || (itemId == 40201) || (itemId == 40204) || (itemId == 40211) || (itemId == 40221) || (itemId == 40225))
            {
                itemAttr = 2;
            }
            // ロウフルテンプル
            if (((pcX > 33116) && (pcX < 33128) && (pcY > 32930) && (pcY < 32942) && (mapId == 4)) || ((pcX > 33135) && (pcX < 33147) && (pcY > 32235) && (pcY < 32247) && (mapId == 4)) || ((pcX >= 32783) && (pcX <= 32803) && (pcY >= 32831) && (pcY <= 32851) && (mapId == 77)))
            {
                locAttr = 1;
                isLawful = true;
            }
            // カオティックテンプル
            if (((pcX > 32880) && (pcX < 32892) && (pcY > 32646) && (pcY < 32658) && (mapId == 4)) || ((pcX > 32662) && (pcX < 32674) && (pcY > 32297) && (pcY < 32309) && (mapId == 4)))
            {
                locAttr = 2;
                isLawful = false;
            }
            if (pc.Gm)
            {
                SpellBook(pc, item, isLawful);
            }
            else if (((itemAttr == locAttr) || (itemAttr == 0)) && (locAttr != 0))
            {
                if (pc.Knight)
                {
                    if ((itemId >= 45000) && (itemId <= 45007) && (level >= 50))
                    {
                        SpellBook(pc, item, isLawful);
                    }
                    else if ((itemId >= 45000) && (itemId <= 45007))
                    {
                        pc.sendPackets(new S_ServerMessage(312));
                    }
                    else
                    {
                        pc.sendPackets(new S_ServerMessage(79));
                    }
                }
                else if (pc.Crown || pc.Darkelf)
                {
                    if ((itemId >= 45000) && (itemId <= 45007) && (level >= 10))
                    {
                        SpellBook(pc, item, isLawful);
                    }
                    else if ((itemId >= 45008) && (itemId <= 45015) && (level >= 20))
                    {
                        SpellBook(pc, item, isLawful);
                    }
                    else if (((itemId >= 45008) && (itemId <= 45015)) || ((itemId >= 45000) && (itemId <= 45007)))
                    {
                        pc.sendPackets(new S_ServerMessage(312)); // レベルが低くてその魔法を覚えることができません。
                    }
                    else
                    {
                        pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                    }
                }
                else if (pc.Elf)
                {
                    if ((itemId >= 45000) && (itemId <= 45007) && (level >= 8))
                    {
                        SpellBook(pc, item, isLawful);
                    }
                    else if ((itemId >= 45008) && (itemId <= 45015) && (level >= 16))
                    {
                        SpellBook(pc, item, isLawful);
                    }
                    else if ((itemId >= 45016) && (itemId <= 45022) && (level >= 24))
                    {
                        SpellBook(pc, item, isLawful);
                    }
                    else if ((itemId >= 40170) && (itemId <= 40177) && (level >= 32))
                    {
                        SpellBook(pc, item, isLawful);
                    }
                    else if ((itemId >= 40178) && (itemId <= 40185) && (level >= 40))
                    {
                        SpellBook(pc, item, isLawful);
                    }
                    else if ((itemId >= 40186) && (itemId <= 40193) && (level >= 48))
                    {
                        SpellBook(pc, item, isLawful);
                    }
                    else if (((itemId >= 45000) && (itemId <= 45022)) || ((itemId >= 40170) && (itemId <= 40193)))
                    {
                        pc.sendPackets(new S_ServerMessage(312)); // レベルが低くてその魔法を覚えることができません。
                    }
                    else
                    {
                        pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                    }
                }
                else if (pc.Wizard)
                {
                    if ((itemId >= 45000) && (itemId <= 45007) && (level >= 4))
                    {
                        SpellBook(pc, item, isLawful);
                    }
                    else if ((itemId >= 45008) && (itemId <= 45015) && (level >= 8))
                    {
                        SpellBook(pc, item, isLawful);
                    }
                    else if ((itemId >= 45016) && (itemId <= 45022) && (level >= 12))
                    {
                        SpellBook(pc, item, isLawful);
                    }
                    else if ((itemId >= 40170) && (itemId <= 40177) && (level >= 16))
                    {
                        SpellBook(pc, item, isLawful);
                    }
                    else if ((itemId >= 40178) && (itemId <= 40185) && (level >= 20))
                    {
                        SpellBook(pc, item, isLawful);
                    }
                    else if ((itemId >= 40186) && (itemId <= 40193) && (level >= 24))
                    {
                        SpellBook(pc, item, isLawful);
                    }
                    else if ((itemId >= 40194) && (itemId <= 40201) && (level >= 28))
                    {
                        SpellBook(pc, item, isLawful);
                    }
                    else if ((itemId >= 40202) && (itemId <= 40209) && (level >= 32))
                    {
                        SpellBook(pc, item, isLawful);
                    }
                    else if ((itemId >= 40210) && (itemId <= 40217) && (level >= 36))
                    {
                        SpellBook(pc, item, isLawful);
                    }
                    else if ((itemId >= 40218) && (itemId <= 40225) && (level >= 40))
                    {
                        SpellBook(pc, item, isLawful);
                    }
                    else
                    {
                        pc.sendPackets(new S_ServerMessage(312)); // レベルが低くてその魔法を覚えることができません。
                    }
                }
            }
            else if ((itemAttr != locAttr) && (itemAttr != 0) && (locAttr != 0))
            {
                // 間違ったテンプルで読んだ場合雷が落ちる
                pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                S_SkillSound effect = new S_SkillSound(pc.Id, 10);
                pc.sendPackets(effect);
                pc.broadcastPacket(effect);
                // ダメージは適当
                pc.CurrentHp = Math.Max(pc.CurrentHp - 45, 0);
                if (pc.CurrentHp <= 0)
                {
                    pc.death(null);
                }
                pc.Inventory.removeItem(item, 1);
            }
            else
            {
                pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
            }
        }

        private void useElfSpellBook(L1PcInstance pc, L1ItemInstance item, int itemId)
        {
            int level = pc.Level;
            if ((pc.Elf || pc.Gm) && isLearnElfMagic(pc))
            {
                if ((itemId >= 40232) && (itemId <= 40234) && (level >= 10))
                {
                    SpellBook2(pc, item);
                }
                else if ((itemId >= 40235) && (itemId <= 40236) && (level >= 20))
                {
                    SpellBook2(pc, item);
                }
                if ((itemId >= 40237) && (itemId <= 40240) && (level >= 30))
                {
                    SpellBook2(pc, item);
                }
                else if ((itemId >= 40241) && (itemId <= 40243) && (level >= 40))
                {
                    SpellBook2(pc, item);
                }
                else if ((itemId >= 40244) && (itemId <= 40246) && (level >= 50))
                {
                    SpellBook2(pc, item);
                }
                else if ((itemId >= 40247) && (itemId <= 40248) && (level >= 30))
                {
                    SpellBook2(pc, item);
                }
                else if ((itemId >= 40249) && (itemId <= 40250) && (level >= 40))
                {
                    SpellBook2(pc, item);
                }
                else if ((itemId >= 40251) && (itemId <= 40252) && (level >= 50))
                {
                    SpellBook2(pc, item);
                }
                else if ((itemId == 40253) && (level >= 30))
                {
                    SpellBook2(pc, item);
                }
                else if ((itemId == 40254) && (level >= 40))
                {
                    SpellBook2(pc, item);
                }
                else if ((itemId == 40255) && (level >= 50))
                {
                    SpellBook2(pc, item);
                }
                else if ((itemId == 40256) && (level >= 30))
                {
                    SpellBook2(pc, item);
                }
                else if ((itemId == 40257) && (level >= 40))
                {
                    SpellBook2(pc, item);
                }
                else if ((itemId >= 40258) && (itemId <= 40259) && (level >= 50))
                {
                    SpellBook2(pc, item);
                }
                else if ((itemId >= 40260) && (itemId <= 40261) && (level >= 30))
                {
                    SpellBook2(pc, item);
                }
                else if ((itemId == 40262) && (level >= 40))
                {
                    SpellBook2(pc, item);
                }
                else if ((itemId >= 40263) && (itemId <= 40264) && (level >= 50))
                {
                    SpellBook2(pc, item);
                }
                else if ((itemId >= 41149) && (itemId <= 41150) && (level >= 50))
                {
                    SpellBook2(pc, item);
                }
                else if ((itemId == 41151) && (level >= 40))
                {
                    SpellBook2(pc, item);
                }
                else if ((itemId >= 41152) && (itemId <= 41153) && (level >= 50))
                {
                    SpellBook2(pc, item);
                }
            }
            else
            {
                pc.sendPackets(new S_ServerMessage(79)); // (原文:精霊の水晶はエルフのみが習得できます。)
            }
        }

        private bool isLearnElfMagic(L1PcInstance pc)
        {
            int pcX = pc.X;
            int pcY = pc.Y;
            int pcMapId = pc.MapId;
            if (((pcX >= 32786) && (pcX <= 32797) && (pcY >= 32842) && (pcY <= 32859) && (pcMapId == 75)) || (pc.Location.isInScreen(new Point(33055, 32336)) && (pcMapId == 4)))
            { // マザーツリー
                return true;
            }
            return false;
        }

        private void SpellBook(L1PcInstance pc, L1ItemInstance item, bool isLawful)
        {
            string s = "";
            int i = 0;
            int level1 = 0;
            int level2 = 0;
            int l = 0;
            int i1 = 0;
            int j1 = 0;
            int k1 = 0;
            int l1 = 0;
            int i2 = 0;
            int j2 = 0;
            int k2 = 0;
            int l2 = 0;
            int i3 = 0;
            int j3 = 0;
            int k3 = 0;
            int l3 = 0;
            int i4 = 0;
            int j4 = 0;
            int k4 = 0;
            int l4 = 0;
            int i5 = 0;
            int j5 = 0;
            int k5 = 0;
            int l5 = 0;
            int i6 = 0;
            for (int skillId = 1; skillId < 81; skillId++)
            {
                L1Skills l1skills = SkillsTable.Instance.getTemplate(skillId);
                string s1 = null;
                if (Config.CLIENT_LANGUAGE == 3)
                {
                    s1 = (new StringBuilder()).Append("魔法書(").Append(l1skills.Name).Append(")").ToString();
                }
                else if (Config.CLIENT_LANGUAGE == 5)
                {
                    s1 = (new StringBuilder()).Append("魔法书(").Append(l1skills.Name).Append(")").ToString();
                }
                else
                {
                    s1 = (new StringBuilder()).Append("魔法書(").Append(l1skills.Name).Append(")").ToString();
                }
                if (item.Item.Name == s1)
                {
                    int skillLevel = l1skills.SkillLevel;
                    int i7 = l1skills.Id;
                    s = l1skills.Name;
                    i = l1skills.SkillId;
                    switch (skillLevel)
                    {
                        case 1: // '\001'
                            level1 = i7;
                            break;

                        case 2: // '\002'
                            level2 = i7;
                            break;

                        case 3: // '\003'
                            l = i7;
                            break;

                        case 4: // '\004'
                            i1 = i7;
                            break;

                        case 5: // '\005'
                            j1 = i7;
                            break;

                        case 6: // '\006'
                            k1 = i7;
                            break;

                        case 7: // '\007'
                            l1 = i7;
                            break;

                        case 8: // '\b'
                            i2 = i7;
                            break;

                        case 9: // '\t'
                            j2 = i7;
                            break;

                        case 10: // '\n'
                            k2 = i7;
                            break;

                        case 11: // '\013'
                            l2 = i7;
                            break;

                        case 12: // '\f'
                            i3 = i7;
                            break;

                        case 13: // '\r'
                            j3 = i7;
                            break;

                        case 14: // '\016'
                            k3 = i7;
                            break;

                        case 15: // '\017'
                            l3 = i7;
                            break;

                        case 16: // '\020'
                            i4 = i7;
                            break;

                        case 17: // '\021'
                            j4 = i7;
                            break;

                        case 18: // '\022'
                            k4 = i7;
                            break;

                        case 19: // '\023'
                            l4 = i7;
                            break;

                        case 20: // '\024'
                            i5 = i7;
                            break;

                        case 21: // '\025'
                            j5 = i7;
                            break;

                        case 22: // '\026'
                            k5 = i7;
                            break;

                        case 23: // '\027'
                            l5 = i7;
                            break;

                        case 24: // '\030'
                            i6 = i7;
                            break;
                    }
                }
            }

            int objid = pc.Id;
            pc.sendPackets(new S_AddSkill(level1, level2, l, i1, j1, k1, l1, i2, j2, k2, l2, i3, j3, k3, l3, i4, j4, k4, l4, i5, j5, k5, l5, i6, 0, 0, 0, 0));
            S_SkillSound s_skillSound = new S_SkillSound(objid, isLawful ? 224 : 231);
            pc.sendPackets(s_skillSound);
            pc.broadcastPacket(s_skillSound);
            SkillsTable.Instance.spellMastery(objid, i, s, 0, 0);
            pc.Inventory.removeItem(item, 1);
        }

        private void SpellBook1(L1PcInstance pc, L1ItemInstance l1iteminstance, ClientThread clientthread)
        {
            string s = "";
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            int i1 = 0;
            int j1 = 0;
            int k1 = 0;
            int l1 = 0;
            int i2 = 0;
            int j2 = 0;
            int k2 = 0;
            int l2 = 0;
            int i3 = 0;
            int j3 = 0;
            int k3 = 0;
            int l3 = 0;
            int i4 = 0;
            int j4 = 0;
            int k4 = 0;
            int l4 = 0;
            int i5 = 0;
            int j5 = 0;
            int k5 = 0;
            int l5 = 0;
            int i6 = 0;
            for (int j6 = 97; j6 < 112; j6++)
            {
                L1Skills l1skills = SkillsTable.Instance.getTemplate(j6);
                string s1 = null;
                if (Config.CLIENT_LANGUAGE == 3)
                {
                    s1 = (new StringBuilder()).Append("黑暗精靈水晶(").Append(l1skills.Name).Append(")").ToString();
                }
                else if (Config.CLIENT_LANGUAGE == 5)
                {
                    s1 = (new StringBuilder()).Append("黑暗精灵水晶(").Append(l1skills.Name).Append(")").ToString();
                }
                else
                {
                    s1 = (new StringBuilder()).Append("闇精霊の水晶(").Append(l1skills.Name).Append(")").ToString();
                }
                if (l1iteminstance.Item.Name == s1)
                {
                    int l6 = l1skills.SkillLevel;
                    int i7 = l1skills.Id;
                    s = l1skills.Name;
                    i = l1skills.SkillId;
                    switch (l6)
                    {
                        case 1: // '\001'
                            j = i7;
                            break;

                        case 2: // '\002'
                            k = i7;
                            break;

                        case 3: // '\003'
                            l = i7;
                            break;

                        case 4: // '\004'
                            i1 = i7;
                            break;

                        case 5: // '\005'
                            j1 = i7;
                            break;

                        case 6: // '\006'
                            k1 = i7;
                            break;

                        case 7: // '\007'
                            l1 = i7;
                            break;

                        case 8: // '\b'
                            i2 = i7;
                            break;

                        case 9: // '\t'
                            j2 = i7;
                            break;

                        case 10: // '\n'
                            k2 = i7;
                            break;

                        case 11: // '\013'
                            l2 = i7;
                            break;

                        case 12: // '\f'
                            i3 = i7;
                            break;

                        case 13: // '\r'
                            j3 = i7;
                            break;

                        case 14: // '\016'
                            k3 = i7;
                            break;

                        case 15: // '\017'
                            l3 = i7;
                            break;

                        case 16: // '\020'
                            i4 = i7;
                            break;

                        case 17: // '\021'
                            j4 = i7;
                            break;

                        case 18: // '\022'
                            k4 = i7;
                            break;

                        case 19: // '\023'
                            l4 = i7;
                            break;

                        case 20: // '\024'
                            i5 = i7;
                            break;

                        case 21: // '\025'
                            j5 = i7;
                            break;

                        case 22: // '\026'
                            k5 = i7;
                            break;

                        case 23: // '\027'
                            l5 = i7;
                            break;

                        case 24: // '\030'
                            i6 = i7;
                            break;
                    }
                }
            }

            int k6 = pc.Id;
            pc.sendPackets(new S_AddSkill(j, k, l, i1, j1, k1, l1, i2, j2, k2, l2, i3, j3, k3, l3, i4, j4, k4, l4, i5, j5, k5, l5, i6, 0, 0, 0, 0));
            S_SkillSound s_skillSound = new S_SkillSound(k6, 231);
            pc.sendPackets(s_skillSound);
            pc.broadcastPacket(s_skillSound);
            SkillsTable.Instance.spellMastery(k6, i, s, 0, 0);
            pc.Inventory.removeItem(l1iteminstance, 1);
        }

        private void SpellBook2(L1PcInstance pc, L1ItemInstance l1iteminstance)
        {
            string s = "";
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            int i1 = 0;
            int j1 = 0;
            int k1 = 0;
            int l1 = 0;
            int i2 = 0;
            int j2 = 0;
            int k2 = 0;
            int l2 = 0;
            int i3 = 0;
            int j3 = 0;
            int k3 = 0;
            int l3 = 0;
            int i4 = 0;
            int j4 = 0;
            int k4 = 0;
            int l4 = 0;
            int i5 = 0;
            int j5 = 0;
            int k5 = 0;
            int l5 = 0;
            int i6 = 0;
            for (int j6 = 129; j6 <= 176; j6++)
            {
                L1Skills l1skills = SkillsTable.Instance.getTemplate(j6);
                string s1 = null;
                if (Config.CLIENT_LANGUAGE == 3)
                {
                    s1 = (new StringBuilder()).Append("精靈水晶(").Append(l1skills.Name).Append(")").ToString();
                }
                else if (Config.CLIENT_LANGUAGE == 5)
                {
                    s1 = (new StringBuilder()).Append("精灵水晶(").Append(l1skills.Name).Append(")").ToString();
                }
                else
                {
                    s1 = (new StringBuilder()).Append("精霊の水晶(").Append(l1skills.Name).Append(")").ToString();
                }
                if (l1iteminstance.Item.Name == s1)
                {
                    if (!pc.Gm && (l1skills.Attr != 0) && (pc.ElfAttr != l1skills.Attr))
                    {
                        if ((pc.ElfAttr == 0) || (pc.ElfAttr == 1) || (pc.ElfAttr == 2) || (pc.ElfAttr == 4) || (pc.ElfAttr == 8))
                        { // 属性値が異常な場合は全属性を覚えられるようにしておく
                            pc.sendPackets(new S_ServerMessage(79));
                            return;
                        }
                    }
                    int l6 = l1skills.SkillLevel;
                    int i7 = l1skills.Id;
                    s = l1skills.Name;
                    i = l1skills.SkillId;
                    switch (l6)
                    {
                        case 1: // '\001'
                            j = i7;
                            break;

                        case 2: // '\002'
                            k = i7;
                            break;

                        case 3: // '\003'
                            l = i7;
                            break;

                        case 4: // '\004'
                            i1 = i7;
                            break;

                        case 5: // '\005'
                            j1 = i7;
                            break;

                        case 6: // '\006'
                            k1 = i7;
                            break;

                        case 7: // '\007'
                            l1 = i7;
                            break;

                        case 8: // '\b'
                            i2 = i7;
                            break;

                        case 9: // '\t'
                            j2 = i7;
                            break;

                        case 10: // '\n'
                            k2 = i7;
                            break;

                        case 11: // '\013'
                            l2 = i7;
                            break;

                        case 12: // '\f'
                            i3 = i7;
                            break;

                        case 13: // '\r'
                            j3 = i7;
                            break;

                        case 14: // '\016'
                            k3 = i7;
                            break;

                        case 15: // '\017'
                            l3 = i7;
                            break;

                        case 16: // '\020'
                            i4 = i7;
                            break;

                        case 17: // '\021'
                            j4 = i7;
                            break;

                        case 18: // '\022'
                            k4 = i7;
                            break;

                        case 19: // '\023'
                            l4 = i7;
                            break;

                        case 20: // '\024'
                            i5 = i7;
                            break;

                        case 21: // '\025'
                            j5 = i7;
                            break;

                        case 22: // '\026'
                            k5 = i7;
                            break;

                        case 23: // '\027'
                            l5 = i7;
                            break;

                        case 24: // '\030'
                            i6 = i7;
                            break;
                    }
                }
            }

            int k6 = pc.Id;
            pc.sendPackets(new S_AddSkill(j, k, l, i1, j1, k1, l1, i2, j2, k2, l2, i3, j3, k3, l3, i4, j4, k4, l4, i5, j5, k5, l5, i6, 0, 0, 0, 0));
            S_SkillSound s_skillSound = new S_SkillSound(k6, 224);
            pc.sendPackets(s_skillSound);
            pc.broadcastPacket(s_skillSound);
            SkillsTable.Instance.spellMastery(k6, i, s, 0, 0);
            pc.Inventory.removeItem(l1iteminstance, 1);
        }

        private void SpellBook3(L1PcInstance pc, L1ItemInstance l1iteminstance, ClientThread clientthread)
        {
            string s = "";
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            int i1 = 0;
            int j1 = 0;
            int k1 = 0;
            int l1 = 0;
            int i2 = 0;
            int j2 = 0;
            int k2 = 0;
            int l2 = 0;
            int i3 = 0;
            int j3 = 0;
            int k3 = 0;
            int l3 = 0;
            int i4 = 0;
            int j4 = 0;
            int k4 = 0;
            int l4 = 0;
            int i5 = 0;
            int j5 = 0;
            int k5 = 0;
            int l5 = 0;
            int i6 = 0;
            for (int j6 = 87; j6 <= 91; j6++)
            {
                L1Skills l1skills = SkillsTable.Instance.getTemplate(j6);

                string s1 = null;
                if (Config.CLIENT_LANGUAGE == 3)
                {
                    s1 = (new StringBuilder()).Append("技術書(").Append(l1skills.Name).Append(")").ToString();
                }
                else if (Config.CLIENT_LANGUAGE == 5)
                {
                    s1 = (new StringBuilder()).Append("技术书(").Append(l1skills.Name).Append(")").ToString();
                }
                else
                {
                    s1 = (new StringBuilder()).Append("技術書(").Append(l1skills.Name).Append(")").ToString();
                }
                if (l1iteminstance.Item.Name == s1)
                {
                    int l6 = l1skills.SkillLevel;
                    int i7 = l1skills.Id;
                    s = l1skills.Name;
                    i = l1skills.SkillId;
                    switch (l6)
                    {
                        case 1: // '\001'
                            j = i7;
                            break;

                        case 2: // '\002'
                            k = i7;
                            break;

                        case 3: // '\003'
                            l = i7;
                            break;

                        case 4: // '\004'
                            i1 = i7;
                            break;

                        case 5: // '\005'
                            j1 = i7;
                            break;

                        case 6: // '\006'
                            k1 = i7;
                            break;

                        case 7: // '\007'
                            l1 = i7;
                            break;

                        case 8: // '\b'
                            i2 = i7;
                            break;

                        case 9: // '\t'
                            j2 = i7;
                            break;

                        case 10: // '\n'
                            k2 = i7;
                            break;

                        case 11: // '\013'
                            l2 = i7;
                            break;

                        case 12: // '\f'
                            i3 = i7;
                            break;

                        case 13: // '\r'
                            j3 = i7;
                            break;

                        case 14: // '\016'
                            k3 = i7;
                            break;

                        case 15: // '\017'
                            l3 = i7;
                            break;

                        case 16: // '\020'
                            i4 = i7;
                            break;

                        case 17: // '\021'
                            j4 = i7;
                            break;

                        case 18: // '\022'
                            k4 = i7;
                            break;

                        case 19: // '\023'
                            l4 = i7;
                            break;

                        case 20: // '\024'
                            i5 = i7;
                            break;

                        case 21: // '\025'
                            j5 = i7;
                            break;

                        case 22: // '\026'
                            k5 = i7;
                            break;

                        case 23: // '\027'
                            l5 = i7;
                            break;

                        case 24: // '\030'
                            i6 = i7;
                            break;
                    }
                }
            }

            int k6 = pc.Id;
            pc.sendPackets(new S_AddSkill(j, k, l, i1, j1, k1, l1, i2, j2, k2, l2, i3, j3, k3, l3, i4, j4, k4, l4, i5, j5, k5, l5, i6, 0, 0, 0, 0));
            S_SkillSound s_skillSound = new S_SkillSound(k6, 224);
            pc.sendPackets(s_skillSound);
            pc.broadcastPacket(s_skillSound);
            SkillsTable.Instance.spellMastery(k6, i, s, 0, 0);
            pc.Inventory.removeItem(l1iteminstance, 1);
        }

        private void SpellBook4(L1PcInstance pc, L1ItemInstance l1iteminstance, ClientThread clientthread)
        {
            string s = "";
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            int i1 = 0;
            int j1 = 0;
            int k1 = 0;
            int l1 = 0;
            int i2 = 0;
            int j2 = 0;
            int k2 = 0;
            int l2 = 0;
            int i3 = 0;
            int j3 = 0;
            int k3 = 0;
            int l3 = 0;
            int i4 = 0;
            int j4 = 0;
            int k4 = 0;
            int l4 = 0;
            int i5 = 0;
            int j5 = 0;
            int k5 = 0;
            int l5 = 0;
            int i6 = 0;
            for (int j6 = 113; j6 < 121; j6++)
            {
                L1Skills l1skills = SkillsTable.Instance.getTemplate(j6);
                string s1 = null;
                if (Config.CLIENT_LANGUAGE == 3)
                {
                    s1 = (new StringBuilder()).Append("魔法書(").Append(l1skills.Name).Append(")").ToString();
                }
                else if (Config.CLIENT_LANGUAGE == 5)
                {
                    s1 = (new StringBuilder()).Append("魔法书(").Append(l1skills.Name).Append(")").ToString();
                }
                else
                {
                    s1 = (new StringBuilder()).Append("魔法書(").Append(l1skills.Name).Append(")").ToString();
                }
                if (l1iteminstance.Item.Name == s1)
                {
                    int l6 = l1skills.SkillLevel;
                    int i7 = l1skills.Id;
                    s = l1skills.Name;
                    i = l1skills.SkillId;
                    switch (l6)
                    {
                        case 1: // '\001'
                            j = i7;
                            break;

                        case 2: // '\002'
                            k = i7;
                            break;

                        case 3: // '\003'
                            l = i7;
                            break;

                        case 4: // '\004'
                            i1 = i7;
                            break;

                        case 5: // '\005'
                            j1 = i7;
                            break;

                        case 6: // '\006'
                            k1 = i7;
                            break;

                        case 7: // '\007'
                            l1 = i7;
                            break;

                        case 8: // '\b'
                            i2 = i7;
                            break;

                        case 9: // '\t'
                            j2 = i7;
                            break;

                        case 10: // '\n'
                            k2 = i7;
                            break;

                        case 11: // '\013'
                            l2 = i7;
                            break;

                        case 12: // '\f'
                            i3 = i7;
                            break;

                        case 13: // '\r'
                            j3 = i7;
                            break;

                        case 14: // '\016'
                            k3 = i7;
                            break;

                        case 15: // '\017'
                            l3 = i7;
                            break;

                        case 16: // '\020'
                            i4 = i7;
                            break;

                        case 17: // '\021'
                            j4 = i7;
                            break;

                        case 18: // '\022'
                            k4 = i7;
                            break;

                        case 19: // '\023'
                            l4 = i7;
                            break;

                        case 20: // '\024'
                            i5 = i7;
                            break;

                        case 21: // '\025'
                            j5 = i7;
                            break;

                        case 22: // '\026'
                            k5 = i7;
                            break;

                        case 23: // '\027'
                            l5 = i7;
                            break;

                        case 24: // '\030'
                            i6 = i7;
                            break;
                    }
                }
            }

            int k6 = pc.Id;
            pc.sendPackets(new S_AddSkill(j, k, l, i1, j1, k1, l1, i2, j2, k2, l2, i3, j3, k3, l3, i4, j4, k4, l4, i5, j5, k5, l5, i6, 0, 0, 0, 0));
            S_SkillSound s_skillSound = new S_SkillSound(k6, 224);
            pc.sendPackets(s_skillSound);
            pc.broadcastPacket(s_skillSound);
            SkillsTable.Instance.spellMastery(k6, i, s, 0, 0);
            pc.Inventory.removeItem(l1iteminstance, 1);
        }

        private void SpellBook5(L1PcInstance pc, L1ItemInstance l1iteminstance, ClientThread clientthread)
        {
            string s = "";
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            int i1 = 0;
            int j1 = 0;
            int k1 = 0;
            int l1 = 0;
            int i2 = 0;
            int j2 = 0;
            int k2 = 0;
            int l2 = 0;
            int i3 = 0;
            int j3 = 0;
            int k3 = 0;
            int l3 = 0;
            int i4 = 0;
            int j4 = 0;
            int k4 = 0;
            int l4 = 0;
            int i5 = 0;
            int j5 = 0;
            int k5 = 0;
            int l5 = 0;
            int i6 = 0;
            int i8 = 0;
            int j8 = 0;
            int k8 = 0;
            int l8 = 0;
            for (int j6 = 181; j6 <= 195; j6++)
            {
                L1Skills l1skills = SkillsTable.Instance.getTemplate(j6);
                string s1 = null;
                if (Config.CLIENT_LANGUAGE == 3)
                {
                    s1 = (new StringBuilder()).Append("龍騎士書板(").Append(l1skills.Name).Append(")").ToString();
                }
                else if (Config.CLIENT_LANGUAGE == 5)
                {
                    s1 = (new StringBuilder()).Append("龙骑士书板(").Append(l1skills.Name).Append(")").ToString();
                }
                else
                {
                    s1 = (new StringBuilder()).Append("ドラゴンナイトの書板（").Append(l1skills.Name).Append("）").ToString();
                }
                if (l1iteminstance.Item.Name == s1)
                {
                    int l6 = l1skills.SkillLevel;
                    int i7 = l1skills.Id;
                    s = l1skills.Name;
                    i = l1skills.SkillId;
                    switch (l6)
                    {
                        case 1: // '\001'
                            j = i7;
                            break;

                        case 2: // '\002'
                            k = i7;
                            break;

                        case 3: // '\003'
                            l = i7;
                            break;

                        case 4: // '\004'
                            i1 = i7;
                            break;

                        case 5: // '\005'
                            j1 = i7;
                            break;

                        case 6: // '\006'
                            k1 = i7;
                            break;

                        case 7: // '\007'
                            l1 = i7;
                            break;

                        case 8: // '\b'
                            i2 = i7;
                            break;

                        case 9: // '\t'
                            j2 = i7;
                            break;

                        case 10: // '\n'
                            k2 = i7;
                            break;

                        case 11: // '\013'
                            l2 = i7;
                            break;

                        case 12: // '\f'
                            i3 = i7;
                            break;

                        case 13: // '\r'
                            j3 = i7;
                            break;

                        case 14: // '\016'
                            k3 = i7;
                            break;

                        case 15: // '\017'
                            l3 = i7;
                            break;

                        case 16: // '\020'
                            i4 = i7;
                            break;

                        case 17: // '\021'
                            j4 = i7;
                            break;

                        case 18: // '\022'
                            k4 = i7;
                            break;

                        case 19: // '\023'
                            l4 = i7;
                            break;

                        case 20: // '\024'
                            i5 = i7;
                            break;

                        case 21: // '\025'
                            j5 = i7;
                            break;

                        case 22: // '\026'
                            k5 = i7;
                            break;

                        case 23: // '\027'
                            l5 = i7;
                            break;

                        case 24: // '\030'
                            i6 = i7;
                            break;

                        case 25: // '\031'
                            j8 = i7;
                            break;

                        case 26: // '\032'
                            k8 = i7;
                            break;

                        case 27: // '\033'
                            l8 = i7;
                            break;
                        case 28: // '\034'
                            i8 = i7;
                            break;
                    }
                }
            }

            int k6 = pc.Id;
            pc.sendPackets(new S_AddSkill(j, k, l, i1, j1, k1, l1, i2, j2, k2, l2, i3, j3, k3, l3, i4, j4, k4, l4, i5, j5, k5, l5, i6, j8, k8, l8, i8));
            S_SkillSound s_skillSound = new S_SkillSound(k6, 224);
            pc.sendPackets(s_skillSound);
            pc.broadcastPacket(s_skillSound);
            SkillsTable.Instance.spellMastery(k6, i, s, 0, 0);
            pc.Inventory.removeItem(l1iteminstance, 1);
        }

        private void SpellBook6(L1PcInstance pc, L1ItemInstance l1iteminstance, ClientThread clientthread)
        {
            string s = "";
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            int i1 = 0;
            int j1 = 0;
            int k1 = 0;
            int l1 = 0;
            int i2 = 0;
            int j2 = 0;
            int k2 = 0;
            int l2 = 0;
            int i3 = 0;
            int j3 = 0;
            int k3 = 0;
            int l3 = 0;
            int i4 = 0;
            int j4 = 0;
            int k4 = 0;
            int l4 = 0;
            int i5 = 0;
            int j5 = 0;
            int k5 = 0;
            int l5 = 0;
            int i6 = 0;
            int i8 = 0;
            int j8 = 0;
            int k8 = 0;
            int l8 = 0;
            for (int j6 = 201; j6 <= 220; j6++)
            {
                L1Skills l1skills = SkillsTable.Instance.getTemplate(j6);
                string s1 = null;
                if (Config.CLIENT_LANGUAGE == 3)
                {
                    s1 = (new StringBuilder()).Append("記憶水晶(").Append(l1skills.Name).Append(")").ToString();
                }
                else if (Config.CLIENT_LANGUAGE == 5)
                {
                    s1 = (new StringBuilder()).Append("记忆水晶(").Append(l1skills.Name).Append(")").ToString();
                }
                else
                {
                    s1 = (new StringBuilder()).Append("記憶の水晶(").Append(l1skills.Name).Append("）").ToString();
                }
                if (l1iteminstance.Item.Name == s1)
                {
                    int l6 = l1skills.SkillLevel;
                    int i7 = l1skills.Id;
                    s = l1skills.Name;
                    i = l1skills.SkillId;
                    switch (l6)
                    {
                        case 1: // '\001'
                            j = i7;
                            break;

                        case 2: // '\002'
                            k = i7;
                            break;

                        case 3: // '\003'
                            l = i7;
                            break;

                        case 4: // '\004'
                            i1 = i7;
                            break;

                        case 5: // '\005'
                            j1 = i7;
                            break;

                        case 6: // '\006'
                            k1 = i7;
                            break;

                        case 7: // '\007'
                            l1 = i7;
                            break;

                        case 8: // '\b'
                            i2 = i7;
                            break;

                        case 9: // '\t'
                            j2 = i7;
                            break;

                        case 10: // '\n'
                            k2 = i7;
                            break;

                        case 11: // '\013'
                            l2 = i7;
                            break;

                        case 12: // '\f'
                            i3 = i7;
                            break;

                        case 13: // '\r'
                            j3 = i7;
                            break;

                        case 14: // '\016'
                            k3 = i7;
                            break;

                        case 15: // '\017'
                            l3 = i7;
                            break;

                        case 16: // '\020'
                            i4 = i7;
                            break;

                        case 17: // '\021'
                            j4 = i7;
                            break;

                        case 18: // '\022'
                            k4 = i7;
                            break;

                        case 19: // '\023'
                            l4 = i7;
                            break;

                        case 20: // '\024'
                            i5 = i7;
                            break;

                        case 21: // '\025'
                            j5 = i7;
                            break;

                        case 22: // '\026'
                            k5 = i7;
                            break;

                        case 23: // '\027'
                            l5 = i7;
                            break;

                        case 24: // '\030'
                            i6 = i7;
                            break;

                        case 25: // '\031'
                            j8 = i7;
                            break;

                        case 26: // '\032'
                            k8 = i7;
                            break;

                        case 27: // '\033'
                            l8 = i7;
                            break;
                        case 28: // '\034'
                            i8 = i7;
                            break;
                    }
                }
            }

            int k6 = pc.Id;
            pc.sendPackets(new S_AddSkill(j, k, l, i1, j1, k1, l1, i2, j2, k2, l2, i3, j3, k3, l3, i4, j4, k4, l4, i5, j5, k5, l5, i6, j8, k8, l8, i8));
            S_SkillSound s_skillSound = new S_SkillSound(k6, 224);
            pc.sendPackets(s_skillSound);
            pc.broadcastPacket(s_skillSound);
            SkillsTable.Instance.spellMastery(k6, i, s, 0, 0);
            pc.Inventory.removeItem(l1iteminstance, 1);
        }

        private int doWandAction(L1PcInstance user, GameObject target)
        {
            if (user.Id == target.Id)
            {
                return 0; // 目標為自身
            }
            if (user.glanceCheck(target.X, target.Y) == false)
            {
                return 0; // 有障礙物
            }

            // XXX 適当なダメージ計算、要修正
            int dmg = (RandomHelper.Next(11) - 5) + user.Str;
            dmg = Math.Max(1, dmg);

            if (target is L1PcInstance)
            {
                L1PcInstance pc = (L1PcInstance)target;
                if (pc.Map.isSafetyZone(pc.Location) || user.checkNonPvP(user, pc))
                {
                    return 0;
                }
                if (pc.hasSkillEffect(L1SkillId.ICE_LANCE) ||
                    pc.hasSkillEffect(L1SkillId.ABSOLUTE_BARRIER) ||
                    pc.hasSkillEffect(L1SkillId.EARTH_BIND))
                {
                    return 0;
                }

                int newHp = pc.CurrentHp - dmg;
                if (newHp > 0)
                {
                    pc.CurrentHp = newHp;
                }
                else if ((newHp <= 0) && pc.Gm)
                {
                    pc.CurrentHp = pc.MaxHp;
                }
                else if ((newHp <= 0) && !pc.Gm)
                {
                    pc.death(user);
                }
                return dmg;
            }
            else if (target is L1MonsterInstance)
            {
                L1MonsterInstance mob = (L1MonsterInstance)target;
                mob.receiveDamage(user, dmg);
                return dmg;
            }
            return 0;
        }

        private void polyAction(L1PcInstance attacker, L1Character cha)
        {
            bool isSameClan = false;
            if (cha is L1PcInstance)
            {
                L1PcInstance pc = (L1PcInstance)cha;
                if ((pc.Clanid != 0) && (attacker.Clanid == pc.Clanid))
                { // 目標為盟友
                    isSameClan = true;
                }
            }
            if ((attacker.Id != cha.Id) && !isSameClan)
            { // 非自身及盟友
                int probability = 3 * (attacker.Level - cha.getLevel()) + 100 - cha.Mr;
                int rnd = RandomHelper.Next(100) + 1;
                if (rnd > probability)
                {
                    attacker.sendPackets(new S_ServerMessage(79));
                    return;
                }
            }

            int[] polyArray = new int[] { 29, 945, 947, 979, 1037, 1039, 3860, 3861, 3862, 3863, 3864, 3865, 3904, 3906, 95, 146, 2374, 2376, 2377, 2378, 3866, 3867, 3868, 3869, 3870, 3871, 3872, 3873, 3874, 3875, 3876 };

            int pid = RandomHelper.Next(polyArray.Length);
            int polyId = polyArray[pid];

            if (cha is L1PcInstance)
            {
                L1PcInstance pc = (L1PcInstance)cha;
                int awakeSkillId = pc.AwakeSkillId;
                if ((awakeSkillId == L1SkillId.AWAKEN_ANTHARAS) ||
                    (awakeSkillId == L1SkillId.AWAKEN_FAFURION) ||
                    (awakeSkillId == L1SkillId.AWAKEN_VALAKAS))
                {
                    if (attacker.Id == pc.Id)
                    {
                        attacker.sendPackets(new S_ServerMessage(1384)); // 目前狀態中無法變身。
                    }
                    else
                    {
                        attacker.sendPackets(new S_ServerMessage(79));
                    }
                    return;
                }

                if (pc.Inventory is L1PcInventory pcInventory && pcInventory.checkEquipped(20281))
                { // 裝備變形控制戒指
                    pc.sendPackets(new S_ShowPolyList(pc.Id));
                    if (!pc.ShapeChange)
                    {
                        pc.ShapeChange = true;
                    }
                }
                else
                {
                    if (attacker.Id != pc.Id)
                    {
                        pc.sendPackets(new S_ServerMessage(241, attacker.Name)); // %0%s 把你變身。
                    }
                    L1Skills skillTemp = SkillsTable.Instance.getTemplate(L1SkillId.SHAPE_CHANGE);
                    L1PolyMorph.doPoly(pc, polyId, skillTemp.BuffDuration, L1PolyMorph.MORPH_BY_ITEMMAGIC, false);
                }
            }
            else if (cha is L1MonsterInstance)
            {
                L1MonsterInstance mob = (L1MonsterInstance)cha;
                if (mob.Level < 50)
                {
                    int npcId = mob.NpcTemplate.get_npcId();
                    if ((npcId != 45338) && (npcId != 45370) && (npcId != 45456) && (npcId != 45464) && (npcId != 45473) && (npcId != 45488) && (npcId != 45497) && (npcId != 45516) && (npcId != 45529) && (npcId != 45458))
                    { // ドレイク(船長)
                        L1Skills skillTemp = SkillsTable.Instance.getTemplate(L1SkillId.SHAPE_CHANGE);
                        L1PolyMorph.doPoly(mob, polyId, skillTemp.BuffDuration, L1PolyMorph.MORPH_BY_ITEMMAGIC);
                    }
                }
            }
        }

        private bool createNewItem(L1PcInstance pc, int item_id, int count)
        {
            L1ItemInstance item = ItemTable.Instance.createItem(item_id);
            if (item != null)
            {
                item.Count = count;
                if (pc.Inventory.checkAddItem(item, count) == L1Inventory.OK)
                {
                    pc.Inventory.storeItem(item);
                }
                else
                { // 持てない場合は地面に落とす 処理のキャンセルはしない（不正防止）
                    L1World.Instance.getInventory(pc.X, pc.Y, pc.MapId).storeItem(item);
                }
                pc.sendPackets(new S_ServerMessage(403, item.LogName)); // %0を手に入れました。
                return true;
            }
            else
            {
                return false;
            }
        }

        private void useToiTeleportAmulet(L1PcInstance pc, int itemId, L1ItemInstance item)
        {
            bool isTeleport = false;
            if ((itemId == 40289) || (itemId == 40293))
            { // 11,51Famulet
                if ((pc.X >= 32816) && (pc.X <= 32821) && (pc.Y >= 32778) && (pc.Y <= 32783) && (pc.MapId == 101))
                {
                    isTeleport = true;
                }
            }
            else if ((itemId == 40290) || (itemId == 40294))
            { // 21,61Famulet
                if ((pc.X >= 32815) && (pc.X <= 32820) && (pc.Y >= 32815) && (pc.Y <= 32820) && (pc.MapId == 101))
                {
                    isTeleport = true;
                }
            }
            else if ((itemId == 40291) || (itemId == 40295))
            { // 31,71Famulet
                if ((pc.X >= 32779) && (pc.X <= 32784) && (pc.Y >= 32778) && (pc.Y <= 32783) && (pc.MapId == 101))
                {
                    isTeleport = true;
                }
            }
            else if ((itemId == 40292) || (itemId == 40296))
            { // 41,81Famulet
                if ((pc.X >= 32779) && (pc.X <= 32784) && (pc.Y >= 32815) && (pc.Y <= 32820) && (pc.MapId == 101))
                {
                    isTeleport = true;
                }
            }
            else if (itemId == 40297)
            { // 91Famulet
                if ((pc.X >= 32706) && (pc.X <= 32710) && (pc.Y >= 32909) && (pc.Y <= 32913) && (pc.MapId == 190))
                {
                    isTeleport = true;
                }
            }

            if (isTeleport)
            {
                L1Teleport.teleport(pc, item.Item.get_locx(), item.Item.get_locy(), item.Item.get_mapid(), 5, true);
            }
            else
            {
                pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
            }
        }

        private bool writeLetter(int itemId, L1PcInstance pc, int letterCode, string letterReceiver, byte[] letterText)
        {

            int newItemId = 0;
            if (itemId == 40310)
            {
                newItemId = 49016;
            }
            else if (itemId == 40730)
            {
                newItemId = 49020;
            }
            else if (itemId == 40731)
            {
                newItemId = 49022;
            }
            else if (itemId == 40732)
            {
                newItemId = 49024;
            }
            L1ItemInstance item = ItemTable.Instance.createItem(newItemId);
            if (item == null)
            {
                return false;
            }
            item.Count = 1;

            if (sendLetter(pc, letterReceiver, item, true))
            {
                saveLetter(item.Id, letterCode, pc.Name, letterReceiver, letterText);
            }
            else
            {
                return false;
            }
            return true;
        }

        private bool writeClanLetter(int itemId, L1PcInstance pc, int letterCode, string letterReceiver, byte[] letterText)
        {
            L1Clan targetClan = null;
            foreach (L1Clan clan in L1World.Instance.AllClans)
            {
                if (clan.ClanName.ToLower().Equals(letterReceiver.ToLower()))
                {
                    targetClan = clan;
                    break;
                }
            }
            if (targetClan == null)
            {
                pc.sendPackets(new S_ServerMessage(434)); // 受信者がいません。
                return false;
            }

            string[] memberName = targetClan.AllMembers;
            foreach (string element in memberName)
            {
                L1ItemInstance item = ItemTable.Instance.createItem(49016);
                if (item == null)
                {
                    return false;
                }
                item.Count = 1;
                if (sendLetter(pc, element, item, false))
                {
                    saveLetter(item.Id, letterCode, pc.Name, element, letterText);
                }
            }
            return true;
        }

        private bool sendLetter(L1PcInstance pc, string name, L1ItemInstance item, bool isFailureMessage)
        {
            L1PcInstance target = L1World.Instance.getPlayer(name);
            if (target != null)
            {
                if (target.Inventory.checkAddItem(item, 1) == L1Inventory.OK)
                {
                    target.Inventory.storeItem(item);
                    target.sendPackets(new S_SkillSound(target.Id, 1091));
                    target.sendPackets(new S_ServerMessage(428)); // 手紙が届きました。
                }
                else
                {
                    if (isFailureMessage)
                    {
                        // 相手のアイテムが重すぎるため、これ以上あげられません。
                        pc.sendPackets(new S_ServerMessage(942));
                    }
                    return false;
                }
            }
            else
            {
                if (CharacterTable.doesCharNameExist(name))
                {
                    try
                    {
                        int targetId = CharacterTable.Instance.restoreCharacter(name).Id;
                        CharactersItemStorage storage = CharactersItemStorage.create();
                        if (storage.getItemCount(targetId) < 180)
                        {
                            storage.storeItem(targetId, item);
                        }
                        else
                        {
                            if (isFailureMessage)
                            {
                                // 相手のアイテムが重すぎるため、これ以上あげられません。
                                pc.sendPackets(new S_ServerMessage(942));
                            }
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        _log.Error(e);
                    }
                }
                else
                {
                    if (isFailureMessage)
                    {
                        pc.sendPackets(new S_ServerMessage(109, name)); // %0という名前の人はいません。
                    }
                    return false;
                }
            }
            return true;
        }

        private void saveLetter(int itemObjectId, int code, string sender, string receiver, byte[] text)
        {
            // 日付を取得する
            //SimpleDateFormat sdf = new SimpleDateFormat("yy/MM/dd");
            //TimeZone tz = TimeZone.getTimeZone(Config.TIME_ZONE);
            //string date = sdf.format(DateTime.getInstance(tz));
            string date = DateTime.Now.ToString("yy/MM/dd");
            // subjectとcontentの区切り(0x00 0x00)位置を見つける
            int spacePosition1 = 0;
            int spacePosition2 = 0;
            for (int i = 0; i < text.Length; i += 2)
            {
                if ((text[i] == 0) && (text[i + 1] == 0))
                {
                    if (spacePosition1 == 0)
                    {
                        spacePosition1 = i;
                    }
                    else if ((spacePosition1 != 0) && (spacePosition2 == 0))
                    {
                        spacePosition2 = i;
                        break;
                    }
                }
            }

            // letterテーブルに書き込む
            int subjectLength = spacePosition1 + 2;
            int contentLength = spacePosition2 - spacePosition1;
            if (contentLength <= 0)
            {
                contentLength = 1;
            }
            sbyte[] subject = new sbyte[subjectLength];
            sbyte[] content = new sbyte[contentLength];
            Array.Copy(text, 0, subject, 0, subjectLength);
            Array.Copy(text, subjectLength, content, 0, contentLength);
            LetterTable.Instance.writeLetter(itemObjectId, code, sender, receiver, date, 0, subject, content);
        }

        private bool withdrawPet(L1PcInstance pc, int itemObjectId)
        {
            if (!pc.Map.TakePets)
            {
                pc.sendPackets(new S_ServerMessage(563)); // \f1ここでは使えません。
                return false;
            }

            int petCost = 0;
            foreach (L1NpcInstance petNpc in pc.PetList.Values)
            {
                if (petNpc is L1PetInstance)
                {
                    if (((L1PetInstance)petNpc).ItemObjId == itemObjectId)
                    { // 既に引き出しているペット
                        return false;
                    }
                }
                petCost += petNpc.Petcost;
            }
            int charisma = pc.Cha;
            if (pc.Crown)
            { // 君主
                charisma += 6;
            }
            else if (pc.Elf)
            { // エルフ
                charisma += 12;
            }
            else if (pc.Wizard)
            { // WIZ
                charisma += 6;
            }
            else if (pc.Darkelf)
            { // DE
                charisma += 6;
            }
            else if (pc.DragonKnight)
            { // ドラゴンナイト
                charisma += 6;
            }
            else if (pc.Illusionist)
            { // イリュージョニスト
                charisma += 6;
            }
            charisma -= petCost;
            int petCount = charisma / 6;
            if (petCount <= 0)
            {
                pc.sendPackets(new S_ServerMessage(489)); // 引き取ろうとするペットが多すぎます。
                return false;
            }

            L1Pet l1pet = PetTable.Instance.getTemplate(itemObjectId);
            if (l1pet != null)
            {
                L1Npc npcTemp = NpcTable.Instance.getTemplate(l1pet.get_npcid());
                L1PetInstance pet = new L1PetInstance(npcTemp, pc, l1pet);
                pet.Petcost = 6;
            }
            return true;
        }

        private void startFishing(L1PcInstance pc, int itemId, int fishX, int fishY)
        {
            if ((pc.MapId != 5300) && (pc.MapId != 5301))
            {
                // 無法在這個地區使用釣竿。
                pc.sendPackets(new S_ServerMessage(1138));
                return;
            }
            if (pc.TempCharGfx != pc.ClassId)
            {
                // 這裡不可以變身。
                pc.sendPackets(new S_ServerMessage(1170));
                return;
            }

            int rodLength = 6;

            if (pc.Map.isFishingZone(fishX, fishY))
            {
                if (pc.Map.isFishingZone(fishX + 1, fishY) && pc.Map.isFishingZone(fishX - 1, fishY) && pc.Map.isFishingZone(fishX, fishY + 1) && pc.Map.isFishingZone(fishX, fishY - 1))
                {
                    if ((fishX > pc.X + rodLength) || (fishX < pc.X - rodLength))
                    {
                        pc.sendPackets(new S_ServerMessage(1138));
                    }
                    else if ((fishY > pc.Y + rodLength) || (fishY < pc.Y - rodLength))
                    {
                        pc.sendPackets(new S_ServerMessage(1138));
                    }
                    else if (pc.Inventory.consumeItem(47103, 1))
                    { // 新鮮的餌
                        pc.FishX = fishX;
                        pc.FishY = fishY;
                        pc.sendPackets(new S_Fishing(pc.Id, ActionCodes.ACTION_Fishing, fishX, fishY));
                        pc.broadcastPacket(new S_Fishing(pc.Id, ActionCodes.ACTION_Fishing, fishX, fishY));
                        pc.Fishing = true;
                        long time = DateTime.Now.Ticks + 10000 + (RandomHelper.Next(5) * 1000);
                        pc.FishingTime = time;
                        FishingTimeController.Instance.addMember(pc);
                    }
                    else
                    {
                        // 釣魚需要有餌。
                        pc.sendPackets(new S_ServerMessage(1137));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1138));
                }
            }
            else
            {
                pc.sendPackets(new S_ServerMessage(1138));
            }
        }

        private void useResolvent(L1PcInstance pc, L1ItemInstance item, L1ItemInstance resolvent)
        {
            if ((item == null) || (resolvent == null))
            {
                pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                return;
            }
            if ((item.Item.Type2 == 1) || (item.Item.Type2 == 2))
            { // 武器・防具
                if (item.EnchantLevel != 0)
                { // 強化済み
                    pc.sendPackets(new S_ServerMessage(1161)); // 溶解できません。
                    return;
                }
                if (item.Equipped)
                { // 装備中
                    pc.sendPackets(new S_ServerMessage(1161)); // 溶解できません。
                    return;
                }
            }
            int crystalCount = ResolventTable.Instance.getCrystalCount(item.Item.ItemId);
            if (crystalCount == 0)
            {
                pc.sendPackets(new S_ServerMessage(1161)); // 溶解できません。
                return;
            }

            int rnd = RandomHelper.Next(100) + 1;
            if ((rnd >= 1) && (rnd <= 50))
            {
                crystalCount = 0;
                pc.sendPackets(new S_ServerMessage(158, item.Name)); // \f1%0が蒸発してなくなりました。
            }
            else if ((rnd >= 51) && (rnd <= 90))
            {
                crystalCount *= 1;
            }
            else if ((rnd >= 91) && (rnd <= 100))
            {
                crystalCount = (int)(crystalCount * 1.5);
                pc.Inventory.storeItem(41246, crystalCount);
            }
            if (crystalCount != 0)
            {
                L1ItemInstance crystal = ItemTable.Instance.createItem(41246);
                crystal.Count = crystalCount;
                if (pc.Inventory.checkAddItem(crystal, 1) == L1Inventory.OK)
                {
                    pc.Inventory.storeItem(crystal);
                    pc.sendPackets(new S_ServerMessage(403, crystal.LogName)); // %0を手に入れました。
                }
                else
                { // 持てない場合は地面に落とす 處理のキャンセルはしない（不正防止）
                    L1World.Instance.getInventory(pc.X, pc.Y, pc.MapId).storeItem(crystal);
                }
            }
            pc.Inventory.removeItem(item, 1);
            pc.Inventory.removeItem(resolvent, 1);
        }

        private void makeCooking(L1PcInstance pc, int cookNo)
        {
            bool isNearFire = false;
            foreach (GameObject obj in L1World.Instance.getVisibleObjects(pc, 3))
            {
                if (obj is L1EffectInstance)
                {
                    L1EffectInstance effect = (L1EffectInstance)obj;
                    if (effect.GfxId == 5943)
                    {
                        isNearFire = true;
                        break;
                    }
                }
            }
            if (!isNearFire)
            {
                pc.sendPackets(new S_ServerMessage(1160)); // 料理には焚き火が必要です。
                return;
            }
            if (pc.MaxWeight <= pc.Inventory.Weight)
            {
                pc.sendPackets(new S_ServerMessage(1103)); // アイテムが重すぎて、料理できません。
                return;
            }
            if (pc.hasSkillEffect(L1SkillId.COOKING_NOW))
            {
                return;
            }
            pc.setSkillEffect(L1SkillId.COOKING_NOW, 3 * 1000);

            int chance = RandomHelper.Next(100) + 1;
            if (cookNo == 0)
            { // フローティングアイステーキ
                if (pc.Inventory.checkItem(40057, 1))
                {
                    pc.Inventory.consumeItem(40057, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 41277, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 41285, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 1)
            { // ベアーステーキ
                if (pc.Inventory.checkItem(41275, 1))
                {
                    pc.Inventory.consumeItem(41275, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 41278, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 41286, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 2)
            { // ナッツ餅
                if (pc.Inventory.checkItem(41263, 1) && pc.Inventory.checkItem(41265, 1))
                {
                    pc.Inventory.consumeItem(41263, 1);
                    pc.Inventory.consumeItem(41265, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 41279, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 41287, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 3)
            { // 蟻脚のチーズ焼き
                if (pc.Inventory.checkItem(41274, 1) && pc.Inventory.checkItem(41267, 1))
                {
                    pc.Inventory.consumeItem(41274, 1);
                    pc.Inventory.consumeItem(41267, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 41280, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 41288, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 4)
            { // フルーツサラダ
                if (pc.Inventory.checkItem(40062, 1) && pc.Inventory.checkItem(40069, 1) && pc.Inventory.checkItem(40064, 1))
                {
                    pc.Inventory.consumeItem(40062, 1);
                    pc.Inventory.consumeItem(40069, 1);
                    pc.Inventory.consumeItem(40064, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 41281, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 41289, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 5)
            { // フルーツ甘酢あんかけ
                if (pc.Inventory.checkItem(40056, 1) && pc.Inventory.checkItem(40060, 1) && pc.Inventory.checkItem(40061, 1))
                {
                    pc.Inventory.consumeItem(40056, 1);
                    pc.Inventory.consumeItem(40060, 1);
                    pc.Inventory.consumeItem(40061, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 41282, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 41290, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 6)
            { // 猪肉の串焼き
                if (pc.Inventory.checkItem(41276, 1))
                {
                    pc.Inventory.consumeItem(41276, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 41283, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 41291, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 7)
            { // キノコスープ
                if (pc.Inventory.checkItem(40499, 1) && pc.Inventory.checkItem(40060, 1))
                {
                    pc.Inventory.consumeItem(40499, 1);
                    pc.Inventory.consumeItem(40060, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 41284, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 41292, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 8)
            { // キャビアカナッペ
                if (pc.Inventory.checkItem(49040, 1) && pc.Inventory.checkItem(49048, 1))
                {
                    pc.Inventory.consumeItem(49040, 1);
                    pc.Inventory.consumeItem(49048, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 49049, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 49057, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 9)
            { // アリゲーターステーキ
                if (pc.Inventory.checkItem(49041, 1) && pc.Inventory.checkItem(49048, 1))
                {
                    pc.Inventory.consumeItem(49041, 1);
                    pc.Inventory.consumeItem(49048, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 49050, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 49058, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 10)
            { // タートルドラゴンの菓子
                if (pc.Inventory.checkItem(49042, 1) && pc.Inventory.checkItem(41265, 1) && pc.Inventory.checkItem(49048, 1))
                {
                    pc.Inventory.consumeItem(49042, 1);
                    pc.Inventory.consumeItem(41265, 1);
                    pc.Inventory.consumeItem(49048, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 49051, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 49059, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 11)
            { // キウィパロット焼き
                if (pc.Inventory.checkItem(49043, 1) && pc.Inventory.checkItem(49048, 1))
                {
                    pc.Inventory.consumeItem(49043, 1);
                    pc.Inventory.consumeItem(49048, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 49052, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 49060, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 12)
            { // スコーピオン焼き
                if (pc.Inventory.checkItem(49044, 1) && pc.Inventory.checkItem(49048, 1))
                {
                    pc.Inventory.consumeItem(49044, 1);
                    pc.Inventory.consumeItem(49048, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 49053, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 49061, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 13)
            { // イレッカドムシチュー
                if (pc.Inventory.checkItem(49045, 1) && pc.Inventory.checkItem(49048, 1))
                {
                    pc.Inventory.consumeItem(49045, 1);
                    pc.Inventory.consumeItem(49048, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 49054, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 49062, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 14)
            { // クモ脚の串焼き
                if (pc.Inventory.checkItem(49046, 1) && pc.Inventory.checkItem(49048, 1))
                {
                    pc.Inventory.consumeItem(49046, 1);
                    pc.Inventory.consumeItem(49048, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 49055, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 49063, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 15)
            { // 蟹肉湯
                if (pc.Inventory.checkItem(49047, 1) && pc.Inventory.checkItem(40499, 1) && pc.Inventory.checkItem(49048, 1))
                {
                    pc.Inventory.consumeItem(49047, 1); // 蟹肉
                    pc.Inventory.consumeItem(40499, 1); // 蘑菇汁
                    pc.Inventory.consumeItem(49048, 1); // 綜合烤肉醬
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 49056, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 49064, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 16)
            { // クラスタシアンのハサミ焼き
                if (pc.Inventory.checkItem(49048, 1) && pc.Inventory.checkItem(49243, 1) && pc.Inventory.checkItem(49260, 1))
                {
                    pc.Inventory.consumeItem(49048, 1);
                    pc.Inventory.consumeItem(49243, 1);
                    pc.Inventory.consumeItem(49260, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 49244, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 49252, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 17)
            { // グリフォン焼き
                if (pc.Inventory.checkItem(49048, 1) && pc.Inventory.checkItem(49243, 1) && pc.Inventory.checkItem(49261, 1))
                {
                    pc.Inventory.consumeItem(49048, 1);
                    pc.Inventory.consumeItem(49243, 1);
                    pc.Inventory.consumeItem(49261, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 49245, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 49253, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 18)
            { // コカトリスステーキ
                if (pc.Inventory.checkItem(49048, 1) && pc.Inventory.checkItem(49243, 1) && pc.Inventory.checkItem(49262, 1))
                {
                    pc.Inventory.consumeItem(49048, 1);
                    pc.Inventory.consumeItem(49243, 1);
                    pc.Inventory.consumeItem(49262, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 49246, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 49254, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 19)
            { // タートルドラゴン焼き
                if (pc.Inventory.checkItem(49048, 1) && pc.Inventory.checkItem(49243, 1) && pc.Inventory.checkItem(49263, 1))
                {
                    pc.Inventory.consumeItem(49048, 1);
                    pc.Inventory.consumeItem(49243, 1);
                    pc.Inventory.consumeItem(49263, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 49247, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 49255, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 20)
            { // レッサードラゴンの手羽先
                if (pc.Inventory.checkItem(49048, 1) && pc.Inventory.checkItem(49243, 1) && pc.Inventory.checkItem(49264, 1))
                {
                    pc.Inventory.consumeItem(49048, 1);
                    pc.Inventory.consumeItem(49243, 1);
                    pc.Inventory.consumeItem(49264, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 49248, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 49256, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 21)
            { // ドレイク焼き
                if (pc.Inventory.checkItem(49048, 1) && pc.Inventory.checkItem(49243, 1) && pc.Inventory.checkItem(49265, 1))
                {
                    pc.Inventory.consumeItem(49048, 1);
                    pc.Inventory.consumeItem(49243, 1);
                    pc.Inventory.consumeItem(49265, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 49249, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 49257, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 22)
            { // 深海魚のシチュー
                if (pc.Inventory.checkItem(49048, 1) && pc.Inventory.checkItem(49243, 1) && pc.Inventory.checkItem(49266, 1))
                {
                    pc.Inventory.consumeItem(49048, 1);
                    pc.Inventory.consumeItem(49243, 1);
                    pc.Inventory.consumeItem(49266, 1);
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 49250, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 49258, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
            else if (cookNo == 23)
            { // 邪惡蜥蜴蛋湯
                if (pc.Inventory.checkItem(49048, 1) && pc.Inventory.checkItem(49243, 1) && pc.Inventory.checkItem(49267, 1) && pc.Inventory.checkItem(40499, 1))
                {
                    pc.Inventory.consumeItem(49048, 1); // 綜合烤肉醬
                    pc.Inventory.consumeItem(49243, 1); // 香菜
                    pc.Inventory.consumeItem(49267, 1); // 邪惡蜥蜴蛋
                    pc.Inventory.consumeItem(40499, 1); // 蘑菇汁
                    if ((chance >= 1) && (chance <= 90))
                    {
                        createNewItem(pc, 49251, 1);
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6392));
                    }
                    else if ((chance >= 91) && (chance <= 95))
                    {
                        createNewItem(pc, 49259, 1);
                        pc.sendPackets(new S_SkillSound(pc.Id, 6390));
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6390));
                    }
                    else if ((chance >= 96) && (chance <= 100))
                    {
                        pc.sendPackets(new S_ServerMessage(1101)); // 料理が失敗しました。
                        pc.broadcastPacket(new S_SkillSound(pc.Id, 6394));
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1102)); // 料理の材料が足りません。
                }
            }
        }

        public override string Type
        {
            get
            {
                return C_ITEM_USE;
            }
        }
    }

}