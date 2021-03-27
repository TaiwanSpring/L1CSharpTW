using LineageServer.Interfaces;
using LineageServer.Server.DataSources;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace LineageServer.Server
{
    /// <summary>
    /// 攻城戰
    /// </summary>
    class WarTimeController : IRunnable
    {
        private static WarTimeController _instance;

        private L1Castle[] _l1castle = new L1Castle[8];

        private DateTime[] _war_start_time = new DateTime[8];

        private DateTime[] _war_end_time = new DateTime[8];

        private bool[] _is_now_war = new bool[8];

        private WarTimeController()
        {
            for (int i = 0; i < _l1castle.Length; i++)
            {
                _l1castle[i] = CastleTable.Instance.getCastleTable(i + 1);
                _war_start_time[i] = _l1castle[i].WarTime;
                _war_end_time[i] = _l1castle[i].WarTime.Add(Config.ALT_WAR_TIME);
            }
        }

        public static WarTimeController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WarTimeController();
                }
                return _instance;
            }
        }

        public void run()
        {
            try
            {
                while (true)
                {
                    checkWarTime(); // 檢查攻城時間
                    Thread.Sleep(1000);
                }
            }
            catch (Exception)
            {
            }
        }

        public virtual bool isNowWar(int castle_id)
        {
            return _is_now_war[castle_id - 1];
        }

        // TODO 
        public virtual void checkCastleWar(L1PcInstance player)
        {
            IList<string> castle = new List<string>();
            for (int i = 0; i < 8; i++)
            {
                if (_is_now_war[i])
                {
                    castle.Add(CastleTable.Instance.getCastleTable(i + 1).Name);
                    // 攻城戰進行中。
                    player.sendPackets(new S_PacketBox(S_PacketBox.MSG_WAR_IS_GOING_ALL, castle.ToArray()));
                }
            }
        }

        private void checkWarTime()
        {
            DateTime dateTimeNow = DateTime.Now;
            for (int i = 0; i < 8; i++)
            {
                if (_war_start_time[i] < dateTimeNow && _war_end_time[i] > dateTimeNow)
                {
                    if (_is_now_war[i] == false)
                    {
                        _is_now_war[i] = true;
                        // 招出攻城的旗子
                        L1WarSpawn warspawn = new L1WarSpawn();
                        warspawn.SpawnFlag(i + 1);
                        // 修理城門並設定為關閉
                        foreach (L1DoorInstance door in DoorTable.Instance.DoorList)
                        {
                            if (L1CastleLocation.checkInWarArea(i + 1, door))
                            {
                                door.repairGate();
                            }
                        }

                        L1World.Instance.broadcastPacketToAll(new S_PacketBox(S_PacketBox.MSG_WAR_BEGIN, i + 1)); // %sの攻城戦が始まりました。
                        int[] loc = new int[3];
                        foreach (L1PcInstance pc in L1World.Instance.AllPlayers)
                        {
                            int castleId = i + 1;
                            if (L1CastleLocation.checkInWarArea(castleId, pc) && !pc.Gm)
                            { // 剛好在攻城範圍內
                                L1Clan clan = L1World.Instance.getClan(pc.Clanname);
                                if (clan != null)
                                {
                                    if (clan.CastleId == castleId)
                                    { // 如果是城血盟
                                        continue;
                                    }
                                }
                                loc = L1CastleLocation.getGetBackLoc(castleId);
                                L1Teleport.teleport(pc, loc[0], loc[1], (short)loc[2], 5, true);
                            }
                        }
                    }
                }
                else if (_war_end_time[i] < dateTimeNow)
                { // 攻城結束
                    if (_is_now_war[i] == true)
                    {
                        _is_now_war[i] = false;
                        L1World.Instance.broadcastPacketToAll(new S_PacketBox(S_PacketBox.MSG_WAR_END, i + 1)); // %sの攻城戦が終了しました。
                                                                                                                // 更新攻城時間
                        WarUpdate(i);

                        int castle_id = i + 1;
                        foreach (GameObject l1object in L1World.Instance.Object)
                        {
                            // 取消攻城的旗子
                            if (l1object is L1FieldObjectInstance)
                            {
                                L1FieldObjectInstance flag = (L1FieldObjectInstance)l1object;
                                if (L1CastleLocation.checkInWarArea(castle_id, flag))
                                {
                                    flag.deleteMe();
                                }
                            }
                            // 移除皇冠
                            if (l1object is L1CrownInstance)
                            {
                                L1CrownInstance crown = (L1CrownInstance)l1object;
                                if (L1CastleLocation.checkInWarArea(castle_id, crown))
                                {
                                    crown.deleteMe();
                                }
                            }
                            // 移除守護塔
                            if (l1object is L1TowerInstance)
                            {
                                L1TowerInstance tower = (L1TowerInstance)l1object;
                                if (L1CastleLocation.checkInWarArea(castle_id, tower))
                                {
                                    tower.deleteMe();
                                }
                            }
                        }
                        // 塔重新出現
                        L1WarSpawn warspawn = new L1WarSpawn();
                        warspawn.SpawnTower(castle_id);

                        // 移除城門
                        foreach (L1DoorInstance door in DoorTable.Instance.DoorList)
                        {
                            if (L1CastleLocation.checkInWarArea(castle_id, door))
                            {
                                door.repairGate();
                            }
                        }
                    }
                    else
                    { // 更新過期的攻城時間
                        _war_start_time[i] = dateTimeNow;
                        _war_end_time[i] = _war_start_time[i];
                        WarUpdate(i);
                    }
                }

            }
        }

        private void WarUpdate(int i)
        {
            _war_start_time[i] = _war_start_time[i].Add(Config.ALT_WAR_INTERVAL);
            _war_end_time[i] = _war_end_time[i].Add(Config.ALT_WAR_INTERVAL);

            _l1castle[i].WarTime = _war_start_time[i];
            _l1castle[i].TaxRate = 10; // 稅率10%
            _l1castle[i].PublicMoney = 0; // 清除城堡稅收
            CastleTable.Instance.updateCastle(_l1castle[i]);
        }
    }

}