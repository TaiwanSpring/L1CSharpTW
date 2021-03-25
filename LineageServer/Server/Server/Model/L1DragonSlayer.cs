using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.utils.collections;
using System;
using System.Collections.Generic;
using System.Threading;
namespace LineageServer.Server.Server.Model
{
    /// <summary>
    /// 安塔瑞斯、法利昂副本 </summary>
    class L1DragonSlayer
    {
        private static ILogger _log = Logger.getLogger(nameof(L1DragonSlayer));

        private static L1DragonSlayer _instance;

        public const int STATUS_DRAGONSLAYER_NONE = 0;
        public const int STATUS_DRAGONSLAYER_READY_1RD = 1;
        public const int STATUS_DRAGONSLAYER_READY_2RD = 2;
        public const int STATUS_DRAGONSLAYER_READY_3RD = 3;
        public const int STATUS_DRAGONSLAYER_READY_4RD = 4;
        public const int STATUS_DRAGONSLAYER_START_1RD = 5;
        public const int STATUS_DRAGONSLAYER_START_2RD = 6;
        public const int STATUS_DRAGONSLAYER_START_2RD_1 = 7;
        public const int STATUS_DRAGONSLAYER_START_2RD_2 = 8;
        public const int STATUS_DRAGONSLAYER_START_2RD_3 = 9;
        public const int STATUS_DRAGONSLAYER_START_2RD_4 = 10;
        public const int STATUS_DRAGONSLAYER_START_3RD = 11;
        public const int STATUS_DRAGONSLAYER_START_3RD_1 = 12;
        public const int STATUS_DRAGONSLAYER_START_3RD_2 = 13;
        public const int STATUS_DRAGONSLAYER_START_3RD_3 = 14;
        public const int STATUS_DRAGONSLAYER_END_1 = 15;
        public const int STATUS_DRAGONSLAYER_END_2 = 16;
        public const int STATUS_DRAGONSLAYER_END_3 = 17;
        public const int STATUS_DRAGONSLAYER_END_4 = 18;
        public const int STATUS_DRAGONSLAYER_END_5 = 19;
        public const int STATUS_DRAGONSLAYER_END = 20;

        public const int STATUS_NONE = 0;
        public const int STATUS_READY_SPAWN = 1;
        public const int STATUS_SPAWN = 2;

        private class DragonSlayer
        {
            internal List<L1PcInstance> _members = new List<L1PcInstance>();
        }

        private static readonly IDictionary<int, DragonSlayer> _dataMap = Maps.newMap<int, DragonSlayer>();

        public static L1DragonSlayer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new L1DragonSlayer();
                }
                return _instance;
            }
        }

        // 判斷龍之門扉是否開啟 ,最多12個龍門
        private bool[] _portalNumber = new bool[12];

        public virtual bool[] PortalNumber
        {
            get
            {
                return _portalNumber;
            }
        }

        public virtual void setPortalNumber(int number, bool i)
        {
            _portalNumber[number] = i;
        }

        // 判斷龍之鑰匙顯示可開啟的龍門
        private bool[] _checkDragonPortal = new bool[4];

        public virtual bool[] checkDragonPortal()
        {
            _checkDragonPortal[0] = false; // 安塔瑞斯
            _checkDragonPortal[1] = false; // 法利昂
            _checkDragonPortal[2] = false; // 林德拜爾
            _checkDragonPortal[3] = false; // 巴拉卡斯

            for (int i = 0; i < 12; i++)
            {
                if (!PortalNumber[i])
                {
                    if (i < 6)
                    { // 前6個安塔瑞斯
                        _checkDragonPortal[0] = true;
                    }
                    else
                    { // 後6個法利昂
                        _checkDragonPortal[1] = true;
                    }
                }
            }
            return _checkDragonPortal;
        }

        // 龍之門扉物件
        private L1NpcInstance[] _portal = new L1NpcInstance[12];

        public virtual L1NpcInstance[] portalPack()
        {
            return _portal;
        }

        public virtual void setPortalPack(int number, L1NpcInstance portal)
        {
            _portal[number] = portal;
        }

        // 副本目前狀態
        private int[] _DragonSlayerStatus = new int[12];

        public virtual int[] DragonSlayerStatus
        {
            get
            {
                return _DragonSlayerStatus;
            }
        }

        public virtual void setDragonSlayerStatus(int portalNum, int i)
        {
            _DragonSlayerStatus[portalNum] = i;
        }

        // 判斷隱匿的巨龍谷入口是否已出現
        private int _hiddenDragonValleyStstus = 0;

        public virtual int checkHiddenDragonValleyStstus()
        {
            return _hiddenDragonValleyStstus;
        }

        public virtual int HiddenDragonValleyStstus
        {
            set
            {
                _hiddenDragonValleyStstus = value;
            }
        }

        // 加入玩家
        public virtual void addPlayerList(L1PcInstance pc, int portalNum)
        {
            if (_dataMap.ContainsKey(portalNum))
            {
                if (!_dataMap[portalNum]._members.Contains(pc))
                {
                    _dataMap[portalNum]._members.Add(pc);
                }
            }
        }

        // 移除玩家
        public virtual void removePlayer(L1PcInstance pc, int portalNum)
        {
            if (_dataMap.ContainsKey(portalNum))
            {
                if (_dataMap[portalNum]._members.Contains(pc))
                {
                    _dataMap[portalNum]._members.Remove(pc);
                }
            }
        }

        // 清除玩家
        private void clearPlayerList(int portalNum)
        {
            if (_dataMap.ContainsKey(portalNum))
            {
                _dataMap[portalNum]._members.Clear();
            }
        }

        // 取得參加人數
        public virtual int getPlayersCount(int num)
        {
            DragonSlayer _DragonSlayer = null;
            if (!_dataMap.ContainsKey(num))
            {
                _DragonSlayer = new DragonSlayer();
                _dataMap[num] = _DragonSlayer;
            }
            return _dataMap[num]._members.Count;
        }

        private L1PcInstance[] getPlayersArray(int num)
        {
            return _dataMap[num]._members.ToArray();
        }

        // 開始第一階段
        public virtual void startDragonSlayer(int portalNum)
        {
            if (DragonSlayerStatus[portalNum] == STATUS_DRAGONSLAYER_NONE)
            {
                setDragonSlayerStatus(portalNum, STATUS_DRAGONSLAYER_READY_1RD);
                DragonSlayerTimer timer = new DragonSlayerTimer(this, portalNum, STATUS_DRAGONSLAYER_READY_1RD, 150000);
                timer.begin();
            }
        }

        // 開始第二階段
        public virtual void startDragonSlayer2rd(int portalNum)
        {
            if (DragonSlayerStatus[portalNum] == STATUS_DRAGONSLAYER_START_1RD)
            {
                if (portalNum >= 6 && portalNum <= 11)
                {
                    sendMessage(portalNum, 1661, null); // 法利昂：可憐啊！他們就是和你一樣，註定要當我的祭品！
                }
                else
                {
                    sendMessage(portalNum, 1573, null); // 安塔瑞斯：你這頑固的傢伙！你又激起我的憤怒了！
                }
                setDragonSlayerStatus(portalNum, STATUS_DRAGONSLAYER_START_2RD);
                DragonSlayerTimer timer = new DragonSlayerTimer(this, portalNum, STATUS_DRAGONSLAYER_START_2RD, 10000);
                timer.begin();
            }
        }

        // 開始第三階段
        public virtual void startDragonSlayer3rd(int portalNum)
        {
            if (DragonSlayerStatus[portalNum] == STATUS_DRAGONSLAYER_START_2RD_4)
            {
                if (portalNum >= 6 && portalNum <= 11)
                {
                    sendMessage(portalNum, 1665, null); // 巫女莎爾：法利昂的力量好像削弱了不少！ 勇士們啊，再接再厲吧！
                }
                else
                {
                    sendMessage(portalNum, 1577, null); // 卡瑞：嗚啊！你有聽到那些冤魂的慘叫聲嗎！受死吧！！
                }
                setDragonSlayerStatus(portalNum, STATUS_DRAGONSLAYER_START_3RD);
                DragonSlayerTimer timer = new DragonSlayerTimer(this, portalNum, STATUS_DRAGONSLAYER_START_3RD, 10000);
                timer.begin();
            }
        }

        // 副本完成
        public virtual void endDragonSlayer(int portalNum)
        {
            if (DragonSlayerStatus[portalNum] == STATUS_DRAGONSLAYER_START_3RD_3)
            {
                setDragonSlayerStatus(portalNum, STATUS_DRAGONSLAYER_END_1);
                DragonSlayerTimer timer = new DragonSlayerTimer(this, portalNum, STATUS_DRAGONSLAYER_END_1, 10000);
                timer.begin();
            }
        }

        // 門扉存在時間結束
        public virtual void endDragonPortal(int portalNum)
        {
            if (DragonSlayerStatus[portalNum] != STATUS_DRAGONSLAYER_END_5)
            {
                setDragonSlayerStatus(portalNum, STATUS_DRAGONSLAYER_END_5);
                DragonSlayerTimer timer = new DragonSlayerTimer(this, portalNum, STATUS_DRAGONSLAYER_END_5, 5000);
                timer.begin();
            }
        }

        // 計時器
        public class DragonSlayerTimer : TimerTask
        {
            private readonly L1DragonSlayer outerInstance;


            internal readonly int _num;
            internal readonly int _status;
            internal readonly int _time;

            public DragonSlayerTimer(L1DragonSlayer outerInstance, int num, int status, int time)
            {
                this.outerInstance = outerInstance;
                _num = num;
                _status = status;
                _time = time;
            }

            public override void run()
            {
                short mapId = (short)(1005 + _num);
                int[] msg = new int[] { 1570, 1571, 1572, 1574, 1575, 1576, 1578, 1579, 1581 };
                if (_num >= 6 && _num <= 11)
                {
                    msg = new int[] { 1657, 1658, 1659, 1662, 1663, 1664, 1666, 1667, 1669 };
                }
                switch (_status)
                {
                    // 階段一
                    case STATUS_DRAGONSLAYER_READY_1RD:
                        outerInstance.setDragonSlayerStatus(_num, STATUS_DRAGONSLAYER_READY_2RD);
                        outerInstance.sendMessage(_num, msg[0], null); // 安塔瑞斯：到底是誰把我吵醒了？
                                                                       // 法利昂：竟敢闖入我的領域...勇氣可嘉啊...
                        DragonSlayerTimer timer_1rd = new DragonSlayerTimer(outerInstance, _num, STATUS_DRAGONSLAYER_READY_2RD, 10000);
                        timer_1rd.begin();
                        break;
                    case STATUS_DRAGONSLAYER_READY_2RD:
                        outerInstance.setDragonSlayerStatus(_num, STATUS_DRAGONSLAYER_READY_3RD);
                        outerInstance.sendMessage(_num, msg[1], null); // 卡瑞：安塔瑞斯！我不停追逐你，結果追到這黑暗的地方來！
                                                                       // 巫女莎爾：你這卑劣的法利昂！你會付出欺騙我的代價！
                        DragonSlayerTimer timer_2rd = new DragonSlayerTimer(outerInstance, _num, STATUS_DRAGONSLAYER_READY_3RD, 10000);
                        timer_2rd.begin();
                        break;
                    case STATUS_DRAGONSLAYER_READY_3RD:
                        outerInstance.setDragonSlayerStatus(_num, STATUS_DRAGONSLAYER_READY_4RD);
                        outerInstance.sendMessage(_num, msg[2], null); // 安塔瑞斯：真可憐，就讓我把你解決掉，受死吧！卡瑞！
                                                                       // 法利昂：雖然在解除封印時你幫了很大的忙...但現在我不會再仁慈了！！
                        DragonSlayerTimer timer_3rd = new DragonSlayerTimer(outerInstance, _num, STATUS_DRAGONSLAYER_READY_4RD, 10000);
                        timer_3rd.begin();
                        break;
                    case STATUS_DRAGONSLAYER_READY_4RD:
                        outerInstance.setDragonSlayerStatus(_num, STATUS_DRAGONSLAYER_START_1RD);
                        // 召喚龍
                        if (_num >= 0 && _num <= 5)
                        {
                            outerInstance.spawn(97006, _num, 32783, 32693, mapId, 10, 0); // 地龍 - 階段一
                        }
                        else
                        {
                            outerInstance.spawn(97044, _num, 32955, 32839, mapId, 10, 0); // 水龍 - 階段一
                        }
                        break;
                    // 階段二
                    case STATUS_DRAGONSLAYER_START_2RD:
                        outerInstance.setDragonSlayerStatus(_num, STATUS_DRAGONSLAYER_START_2RD_1);
                        outerInstance.sendMessage(_num, msg[3], null); // 卡瑞：勇士們！亞丁的命運就掌握在你們的武器上了， 能夠讓安塔瑞斯窒息的人就是你們了！
                                                                       // 巫女莎爾：勇士們！請消滅邪惡的法利昂，解除伊娃王國的血之詛咒吧！
                        DragonSlayerTimer timer_4rd = new DragonSlayerTimer(outerInstance, _num, STATUS_DRAGONSLAYER_START_2RD_1, 10000);
                        timer_4rd.begin();
                        break;
                    case STATUS_DRAGONSLAYER_START_2RD_1:
                        outerInstance.setDragonSlayerStatus(_num, STATUS_DRAGONSLAYER_START_2RD_2);
                        outerInstance.sendMessage(_num, msg[4], null); // 安塔瑞斯：像這種蝦兵蟹將也想要贏我！噗哈哈哈…
                                                                       // 法利昂：你們只夠格當我的玩具！！
                        DragonSlayerTimer timer_5rd = new DragonSlayerTimer(outerInstance, _num, STATUS_DRAGONSLAYER_START_2RD_2, 30000);
                        timer_5rd.begin();
                        break;
                    case STATUS_DRAGONSLAYER_START_2RD_2:
                        outerInstance.setDragonSlayerStatus(_num, STATUS_DRAGONSLAYER_START_2RD_3);
                        outerInstance.sendMessage(_num, msg[5], null); // 安塔瑞斯：我今天又可以飽餐一頓了？你們的血激起我的鬥志。
                                                                       // 法利昂：刻骨的恐懼到底是什麼，就讓你們嘗一下吧！
                        DragonSlayerTimer timer_6rd = new DragonSlayerTimer(outerInstance, _num, STATUS_DRAGONSLAYER_START_2RD_3, 10000);
                        timer_6rd.begin();
                        break;
                    case STATUS_DRAGONSLAYER_START_2RD_3:
                        outerInstance.setDragonSlayerStatus(_num, STATUS_DRAGONSLAYER_START_2RD_4);
                        // 召喚龍
                        if (_num >= 0 && _num <= 5)
                        {
                            outerInstance.spawn(97007, _num, 32783, 32693, mapId, 10, 0); // 地龍 - 階段二
                        }
                        else
                        {
                            outerInstance.spawn(97045, _num, 32955, 32839, mapId, 10, 0); // 水龍 - 階段二
                        }
                        break;
                    // 階段三
                    case STATUS_DRAGONSLAYER_START_3RD:
                        outerInstance.setDragonSlayerStatus(_num, STATUS_DRAGONSLAYER_START_3RD_1);
                        outerInstance.sendMessage(_num, msg[6], null); // 安塔瑞斯：你竟然敢對付我...我看你們是不想活了？
                                                                       // 法利昂：我要讓你們知道你們所謂的希望，只不過是妄想！
                        DragonSlayerTimer timer_7rd = new DragonSlayerTimer(outerInstance, _num, STATUS_DRAGONSLAYER_START_3RD_1, 40000);
                        timer_7rd.begin();
                        break;
                    case STATUS_DRAGONSLAYER_START_3RD_1:
                        outerInstance.setDragonSlayerStatus(_num, STATUS_DRAGONSLAYER_START_3RD_2);
                        outerInstance.sendMessage(_num, msg[7], null); // 安塔瑞斯：我的憤怒值已經衝上天了，我的父親格蘭肯將會賜我力量。
                                                                       // 法利昂：你們會後悔跟了莎爾！ 可笑的愚民…
                        DragonSlayerTimer timer_8rd = new DragonSlayerTimer(outerInstance, _num, STATUS_DRAGONSLAYER_START_3RD_2, 10000);
                        timer_8rd.begin();
                        break;
                    case STATUS_DRAGONSLAYER_START_3RD_2:
                        outerInstance.setDragonSlayerStatus(_num, STATUS_DRAGONSLAYER_START_3RD_3);
                        // 召喚龍
                        if (_num >= 0 && _num <= 5)
                        {
                            outerInstance.spawn(97008, _num, 32783, 32693, mapId, 10, 0); // 地龍 - 階段三
                        }
                        else
                        {
                            outerInstance.spawn(97046, _num, 32955, 32839, mapId, 10, 0); // 水龍 - 階段三
                        }
                        break;
                    case STATUS_DRAGONSLAYER_END_1:
                        outerInstance.setDragonSlayerStatus(_num, STATUS_DRAGONSLAYER_END_2);
                        outerInstance.sendMessage(_num, msg[8], null); // 卡瑞：喔... 頂尖的勇士們！你們經歷了多少的失敗才有今天的成就，你們擊敗了安塔瑞斯！
                                                                       //我終於復仇了嗚哈哈哈！！ 謝謝你們，你們是最頂尖的戰士！
                        if (outerInstance.checkHiddenDragonValleyStstus() == STATUS_NONE)
                        { // 準備開啟隱匿的巨龍谷入口
                            outerInstance.HiddenDragonValleyStstus = STATUS_READY_SPAWN;
                            DragonSlayerTimer timer_9rd = new DragonSlayerTimer(outerInstance, _num, STATUS_DRAGONSLAYER_END_2, 10000);
                            timer_9rd.begin();
                        }
                        else
                        { // 直接結束
                            if (outerInstance.DragonSlayerStatus[_num] != STATUS_DRAGONSLAYER_END_5)
                            {
                                outerInstance.setDragonSlayerStatus(_num, STATUS_DRAGONSLAYER_END_5);
                                DragonSlayerTimer timer = new DragonSlayerTimer(outerInstance, _num, STATUS_DRAGONSLAYER_END_5, 5000);
                                timer.begin();
                            }
                        }
                        break;
                    case STATUS_DRAGONSLAYER_END_2:
                        outerInstance.setDragonSlayerStatus(_num, STATUS_DRAGONSLAYER_END_3);
                        outerInstance.sendMessage(_num, 1582, null); // 侏儒的呼喚：威頓村莊出現了通往隱匿的巨龍谷入口。
                        if (outerInstance.checkHiddenDragonValleyStstus() == STATUS_READY_SPAWN)
                        { // 開啟隱匿的巨龍谷入口
                            outerInstance.HiddenDragonValleyStstus = STATUS_SPAWN;
                            outerInstance.spawn(81277, -1, 33726, 32506, (short)4, 0, 86400000); // 24小時
                        }
                        DragonSlayerTimer timer_10rd = new DragonSlayerTimer(outerInstance, _num, STATUS_DRAGONSLAYER_END_3, 5000);
                        timer_10rd.begin();
                        break;
                    case STATUS_DRAGONSLAYER_END_3:
                        outerInstance.setDragonSlayerStatus(_num, STATUS_DRAGONSLAYER_END_4);
                        outerInstance.sendMessage(_num, 1583, null); // 侏儒的呼喚：威頓村莊通往隱匿的巨龍谷入口已經開啟了。
                        DragonSlayerTimer timer_11rd = new DragonSlayerTimer(outerInstance, _num, STATUS_DRAGONSLAYER_END_4, 5000);
                        timer_11rd.begin();
                        break;
                    case STATUS_DRAGONSLAYER_END_4:
                        outerInstance.setDragonSlayerStatus(_num, STATUS_DRAGONSLAYER_END_5);
                        outerInstance.sendMessage(_num, 1584, null); // 侏儒的呼喚：快離開這裡吧，門就快要關了。
                        DragonSlayerTimer timer_12rd = new DragonSlayerTimer(outerInstance, _num, STATUS_DRAGONSLAYER_END_5, 5000);
                        timer_12rd.begin();
                        break;
                    case STATUS_DRAGONSLAYER_END_5:
                        // 刪除龍之門扉
                        if (outerInstance.portalPack()[_num] != null)
                        {
                            outerInstance.portalPack()[_num].Status = ActionCodes.ACTION_Die;
                            outerInstance.portalPack()[_num].broadcastPacket(new S_DoActionGFX(outerInstance.portalPack()[_num].Id, ActionCodes.ACTION_Die));
                            outerInstance.portalPack()[_num].deleteMe();
                        }
                        // 龍之門扉重置
                        outerInstance.resetDragonSlayer(_num);
                        DragonSlayerTimer timer_13rd = new DragonSlayerTimer(outerInstance, _num, STATUS_DRAGONSLAYER_END, 300000); // 下次可重新開啟同編號龍門的等候時間
                        timer_13rd.begin();
                        break;
                    case STATUS_DRAGONSLAYER_END:
                        outerInstance.setPortalNumber(_num, false);
                        break;
                }
                cancel();
            }

            public virtual void begin()
            {
                RunnableExecuter.Instance.execute(this, _time); // 延遲時間
            }
        }

        // 訊息發送
        public virtual void sendMessage(int portalNum, int type, string msg)
        {
            short mapId = (short)(1005 + portalNum);
            L1PcInstance[] temp = getPlayersArray(portalNum);
            foreach (L1PcInstance element in temp)
            {
                if ((element.MapId == mapId) && (element.X >= 32740 && element.X <= 32827) && (element.Y >= 32652 && element.Y <= 32727) && (portalNum >= 0 && portalNum <= 5))
                { // 安塔瑞斯棲息地
                    element.sendPackets(new S_ServerMessage(type, msg));
                }
                else if ((element.MapId == mapId) && (element.X >= 32921 && element.X <= 33009) && (element.Y >= 32799 && element.Y <= 32869) && (portalNum >= 6 && portalNum <= 11))
                { // 法利昂棲息地
                    element.sendPackets(new S_ServerMessage(type, msg));
                }
            }
        }

        // 重置副本
        public virtual void resetDragonSlayer(int portalNumber)
        {
            short mapId = (short)(1005 + portalNumber); // MapId 判斷

            foreach (object obj in L1World.Instance.getVisibleObjects(mapId).Values)
            {
                // 將玩家傳出副本地圖
                if (obj is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)obj;
                    if (pc != null)
                    {
                        if (pc.Dead)
                        {
                            reStartPlayer(pc);
                        }
                        else
                        {
                            // 傳送至威頓村
                            pc.PortalNumber = -1;
                            L1Teleport.teleport(pc, 33710, 32521, (short)4, pc.Heading, true);
                        }
                    }
                }
                // 門關閉
                else if (obj is L1DoorInstance)
                {
                    L1DoorInstance door = (L1DoorInstance)obj;
                    door.close();
                }
                // 刪除副本內的怪物
                else if (obj is L1NpcInstance)
                {
                    L1NpcInstance npc = (L1NpcInstance)obj;
                    if ((npc.Master == null) && (npc.NpcTemplate.get_npcId() < 81301 && npc.NpcTemplate.get_npcId() > 81306))
                    {
                        npc.deleteMe();
                    }
                }
                // 刪除副本內的物品
                else if (obj is L1Inventory)
                {
                    L1Inventory inventory = (L1Inventory)obj;
                    inventory.clearItems();
                }
            }
            setPortalPack(portalNumber, null);
            setDragonSlayerStatus(portalNumber, STATUS_DRAGONSLAYER_NONE);
            clearPlayerList(portalNumber);
        }

        // 副本內死亡的玩家重新開始
        private void reStartPlayer(L1PcInstance pc)
        {
            pc.stopPcDeleteTimer();

            int[] loc = Getback.GetBack_Location(pc, true);

            pc.removeAllKnownObjects();
            pc.broadcastPacket(new S_RemoveObject(pc));

            pc.CurrentHp = pc.Level;
            pc.set_food(40);
            pc.Dead = false;
            pc.Status = 0;
            L1World.Instance.moveVisibleObject(pc, loc[2]);
            pc.X = loc[0];
            pc.Y = loc[1];
            pc.MapId = (short)loc[2];
            pc.sendPackets(new S_MapID(pc.MapId, pc.Map.Underwater));
            pc.broadcastPacket(new S_OtherCharPacks(pc));
            pc.sendPackets(new S_OwnCharPack(pc));
            pc.sendPackets(new S_CharVisualUpdate(pc));
            pc.startHpRegeneration();
            pc.startMpRegeneration();
            pc.sendPackets(new S_Weather(L1World.Instance.Weather));
            if (pc.HellTime > 0)
            {
                pc.beginHell(false);
            }
        }

        // 召喚用
        public virtual void spawn(int npcId, int portalNumber, int X, int Y, short mapId, int randomRange, int timeMillisToDelete)
        {
            try
            {
                L1NpcInstance npc = NpcTable.Instance.newNpcInstance(npcId);
                npc.Id = IdFactory.Instance.nextId();
                npc.MapId = mapId;

                if (randomRange == 0)
                {
                    npc.X = X;
                    npc.Y = Y;
                }
                else
                {
                    int tryCount = 0;
                    do
                    {
                        tryCount++;
                        npc.X = X + RandomHelper.Next(randomRange) - RandomHelper.Next(randomRange);
                        npc.Y = Y + RandomHelper.Next(randomRange) - RandomHelper.Next(randomRange);
                        if (npc.Map.isInMap(npc.Location) && npc.Map.isPassable(npc.Location))
                        {
                            break;
                        }
                        Thread.Sleep(1);
                    } while (tryCount < 50);

                    if (tryCount >= 50)
                    {
                        npc.X = X;
                        npc.Y = Y;
                    }
                }

                npc.HomeX = npc.X;
                npc.HomeY = npc.Y;
                npc.Heading = RandomHelper.Next(8);
                npc.PortalNumber = portalNumber;

                L1World.Instance.storeObject(npc);
                L1World.Instance.addVisibleObject(npc);

                if (npc.GfxId == 7548 || npc.GfxId == 7550 || npc.GfxId == 7552 || npc.GfxId == 7554 || npc.GfxId == 7585 || npc.GfxId == 7539 || npc.GfxId == 7557 || npc.GfxId == 7558 || npc.GfxId == 7864 || npc.GfxId == 7869 || npc.GfxId == 7870)
                {
                    npc.npcSleepTime(ActionCodes.ACTION_AxeWalk, L1NpcInstance.ATTACK_SPEED);
                    foreach (L1PcInstance pc in L1World.Instance.getVisiblePlayer(npc))
                    {
                        npc.onPerceive(pc);
                        S_DoActionGFX gfx = new S_DoActionGFX(npc.Id, ActionCodes.ACTION_AxeWalk);
                        pc.sendPackets(gfx);
                    }
                }

                npc.turnOnOffLight();
                npc.startChat(L1NpcInstance.CHAT_TIMING_APPEARANCE); // チャット開始

                if (0 < timeMillisToDelete)
                {
                    L1NpcDeleteTimer timer = new L1NpcDeleteTimer(npc, timeMillisToDelete);
                    timer.begin();
                }
            }
            catch (Exception e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
        }
    }

}