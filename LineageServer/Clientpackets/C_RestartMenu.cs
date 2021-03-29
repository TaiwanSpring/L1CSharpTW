using LineageServer.Server;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
using System.Collections.Generic;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理接收由客戶端傳來的開啟重新開始選單請求
    /// </summary>
    class C_RestartMenu : ClientBasePacket
    {
        private const string C_RESTARTMENU = "[C] C_RestartMenu";

        public C_RestartMenu(byte[] abyte0, ClientThread clientthread) : base(abyte0)
        {
            L1PcInstance pc = clientthread.ActiveChar;
            if (pc == null)
            {
                return;
            }

            int data = ReadC();

            if (data == 1)
            { // 請求授予血盟RANK
                int rank = ReadC();
                string name = ReadS();
                L1PcInstance targetPc = L1World.Instance.getPlayer(name);

                L1Clan clan = L1World.Instance.getClan(pc.Clanname);
                if (clan == null)
                {
                    return;
                }

                int userRank = pc.ClanRank; // 授予者的階級

                if (userRank == 0 || userRank > 10)
                {
                    pc.sendPackets(new S_ServerMessage(518)); // 血盟君主才可使用此命令。
                    return;
                }

                if (name == pc.Name)
                {
                    pc.sendPackets(new S_ServerMessage(2068));
                    return;
                }

                if (!(rank > 1 && rank < 11))
                {
                    // 請輸入想要變更階級的人的名稱與階級。[階級 = 實習→ㄧ般→守護→副主]
                    pc.sendPackets(new S_ServerMessage(781));
                    return;
                }

                /// <summary>
                /// 可以使用的權限清單 </summary>
                IList<int> rankList = new List<int>();
                switch (userRank)
                { // 各階級限制
                    case 3:
                        rankList.Add(2);
                        rankList.Add(5);
                        rankList.Add(6);
                        break;
                    case 4:
                        rankList.Add(2);
                        rankList.Add(3);
                        rankList.Add(5);
                        rankList.Add(6);
                        break;
                    case 6:
                        rankList.Add(2);
                        rankList.Add(5);
                        break;
                    case 9:
                        rankList.Add(7);
                        rankList.Add(8);
                        break;
                    case 10:
                        rankList.Add(7);
                        rankList.Add(8);
                        rankList.Add(9);
                        break;

                }

                if (rank == 3)
                { // 如果授予副王階級給其他職業
                    if (!rankList.Contains(3))
                    {
                        return;
                    }
                    else if (!targetPc.Crown)
                    {
                        pc.sendPackets(new S_ServerMessage(2064));
                        return;
                    }
                }
                else if (rank == 2 || rank == 5)
                { // 授予 聯盟一般/ 聯盟見習
                    if (!rankList.Contains(2) && !rankList.Contains(5))
                    {
                        return;
                    }
                    else if (userRank == 4 || userRank == 10 || userRank == 3)
                    {
                    }
                    else if (targetPc.ClanRank == 6 || targetPc.ClanRank == 9 || targetPc.ClanRank == 3 || targetPc.ClanRank == 4 || targetPc.ClanRank == 10)
                    {
                        pc.sendPackets(new S_ServerMessage(2065));
                        return;
                    }
                    else if (pc.Level < 25)
                    {
                        pc.sendPackets(new S_ServerMessage(2471));
                        return;
                    }
                }
                else if (rank == 6 || rank == 9)
                { // 授予守護騎士
                    if (!rankList.Contains(6) && !rankList.Contains(9))
                    {
                        return;
                    }
                    else if (targetPc.ClanRank == 6 || targetPc.ClanRank == 9 || targetPc.ClanRank == 3 || targetPc.ClanRank == 4 || targetPc.ClanRank == 10)
                    {
                        pc.sendPackets(new S_ServerMessage(2065));
                        return;
                    }
                    else if (pc.Level < 40 && userRank != 10 && userRank != 4 && userRank != 3)
                    {
                        pc.sendPackets(new S_ServerMessage(2472));
                        return;
                    }
                    if (targetPc.Level < 40)
                    {
                        pc.sendPackets(new S_ServerMessage(2473));
                        return;
                    }
                }
                else if (rank == 7 || rank == 8)
                { // 授予 一般/ 見習
                    if ((targetPc.ClanRank == 9 || targetPc.ClanRank == 10) && userRank != 10)
                    {
                        pc.sendPackets(new S_ServerMessage(2065));
                        return;
                    }
                }


                if (targetPc != null)
                { // 玩家在線上
                    if (pc.Clanid == targetPc.Clanid)
                    { // 同血盟
                        targetPc.ClanRank = rank;
                        if (targetPc.Save()) // 儲存玩家的資料到資料庫中
                        {
                            targetPc.sendPackets(new S_PacketBox(S_PacketBox.MSG_RANK_CHANGED, rank, name)); // 你的階級變更為%s
                            pc.sendPackets(new S_PacketBox(S_PacketBox.MSG_RANK_CHANGED, rank, name)); // 你的階級變更為%s
                        }
                    }
                    else
                    {
                        pc.sendPackets(new S_ServerMessage(414)); // 您只能邀請您血盟中的成員。
                        return;
                    }
                }
                else
                { // 離線盟友
                    L1PcInstance restorePc = CharacterTable.Instance.restoreCharacter(name);
                    if ((restorePc != null) && (restorePc.Clanid == pc.Clanid))
                    { // 同じ血盟
                        pc.sendPackets(new S_ServerMessage(2069));
                        return;
                    }
                    else
                    {
                        pc.sendPackets(new S_ServerMessage(109, name)); // %0という名前の人はいません。
                        return;
                    }
                }
            }
            else if (data == 2)
            {
                pc.sendPackets(new S_ServerMessage(74, "同盟目錄"));
            }
            else if (data == 3)
            {
                pc.sendPackets(new S_ServerMessage(74, "加入同盟"));
            }
            else if (data == 4)
            {
                pc.sendPackets(new S_ServerMessage(74, "退出同盟"));
            }
            else if (data == 5)
            { // 請求施放 生存吶喊 (CTRL+E)
                if (pc.Weapon == null)
                {
                    pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message1973));
                    return;
                }
                if (pc.CurrentHp >= pc.MaxHp)
                {
                    pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message1974));
                    return;
                }
                if (pc.get_food() >= 225)
                {
                    int addHp = 0;
                    int gfxId1 = 8683;
                    int gfxId2 = 829;
                    long curTime = DateTime.Now.Millisecond / 1000; // 現在時間
                    int fullTime = (int)((curTime - pc.CryOfSurvivalTime) / 60); // 飽食經過時間(分)
                    if (fullTime <= 0)
                    {
                        pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message1974));
                        return;
                    }
                    else if (fullTime >= 1 && fullTime <= 29)
                    {
                        addHp = (int)(pc.MaxHp * (fullTime / 100.0D));
                    }
                    else if (fullTime >= 30)
                    {
                        int weaponEnchantLv = pc.Weapon.EnchantLevel;
                        if (weaponEnchantLv <= 6)
                        {
                            gfxId1 = 8684;
                            gfxId2 = 8907;
                            addHp = (int)(pc.MaxHp * (20 + RandomHelper.Next(20) / 100.0D));
                        }
                        else if (weaponEnchantLv == 7 || weaponEnchantLv == 8)
                        {
                            gfxId1 = 8685;
                            gfxId2 = 8909;
                            addHp = (int)(pc.MaxHp * ((30 + RandomHelper.Next(20)) / 100.0D));
                        }
                        else if (weaponEnchantLv == 9 || weaponEnchantLv == 10)
                        {
                            gfxId1 = 8773;
                            gfxId2 = 8910;
                            addHp = (int)(pc.MaxHp * ((50 + RandomHelper.Next(10)) / 100.0D));
                        }
                        else if (weaponEnchantLv >= 11)
                        {
                            gfxId1 = 8686;
                            gfxId2 = 8908;
                            addHp = (int)(pc.MaxHp * (0.7));
                        }
                    }

                    S_SkillSound sound = new S_SkillSound(pc.Id, gfxId1);
                    pc.sendPackets(sound);
                    pc.broadcastPacket(sound);

                    sound = new S_SkillSound(pc.Id, gfxId2);
                    pc.sendPackets(sound);
                    pc.broadcastPacket(sound);

                    if (addHp != 0)
                    {
                        pc.set_food(0);
                        pc.sendPackets(new S_PacketBox(S_PacketBox.FOOD, 0));
                        pc.CurrentHp = pc.CurrentHp + addHp;
                    }
                }
            }
            else if (data == 6)
            { // 請求顯示 生存吶喊 頭頂動畫(ALT+0)
                int gfxId = 8683;
                long curTime = DateTime.Now.Millisecond / 1000; // 現在時間
                int fullTime = (int)((curTime - pc.CryOfSurvivalTime) / 60); // 飽食經過時間(分)
                if (pc.Weapon == null)
                {
                    pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message1973));
                    return;
                }
                if (fullTime >= 30)
                {
                    int weaponEnchantLv = pc.Weapon.EnchantLevel;
                    if (weaponEnchantLv <= 6)
                    {
                        gfxId = 8684;
                    }
                    else if (weaponEnchantLv >= 7 && weaponEnchantLv <= 8)
                    {
                        gfxId = 8685;
                    }
                    else if (weaponEnchantLv >= 9 && weaponEnchantLv <= 10)
                    {
                        gfxId = 8773;
                    }
                    else if (weaponEnchantLv >= 11)
                    {
                        gfxId = 8686;
                    }
                }
                S_SkillSound sound = new S_SkillSound(pc.Id, gfxId);
                pc.sendPackets(sound);
                pc.broadcastPacket(sound);
            }
            else if (data == 9)
            {
                // TODO 未來完成地圖得計時器後，改成動態即時讀取。
                string[] map = new string[] { "$12125", "$6081", "$14250", "$12126" };
                int[] time = new int[] { 180, 60, 120, 120 };
                pc.sendPackets(new S_PacketBox(S_PacketBox.MAP_TIME, map, time));
            }
        }

        public override string Type
        {
            get
            {
                return C_RESTARTMENU;
            }
        }
    }

}