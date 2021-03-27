using LineageServer.Enum;
using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Gametime;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
using System.Collections.Generic;
using System.Threading;
namespace LineageServer.william
{
    public class L1GameReStart
    {
        private static ILogger _log = Logger.GetLogger(nameof(L1GameReStart));

        private static L1GameReStart _instance;
        private volatile L1GameTime _currentTime = new L1GameTime();
        private L1GameTime _previousTime = null;

        private HashSet<IL1GameTimeListener> _listeners = new HashSet<IL1GameTimeListener>();

        private static int willRestartTime;
        public int _remnant;

        private class TimeUpdaterRestar : IRunnable
        {
            private readonly L1GameReStart outerInstance;

            public TimeUpdaterRestar(L1GameReStart outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public void run()
            {
                while (true)
                {
                    outerInstance._previousTime = outerInstance._currentTime;
                    outerInstance._currentTime = new L1GameTime();
                    outerInstance.notifyChanged();
                    int remnant = outerInstance.GetRestartTime() * 60;
                    Console.WriteLine("【讀取】 【自動重啟】 【設定】【完成】【" + outerInstance.GetRestartTime() + "】【分鐘】。");
                    while (remnant > 0)
                    {
                        for (int i = remnant; i >= 0; i--)
                        {
                            outerInstance.SetRemnant(i);
                            willRestartTime = i;
                            if (i % 60 == 0 && i <= 300 && i != 0)
                            {
                                outerInstance.BroadCastToAll("伺服器將於 " + i / 60 + " 分鐘後自動重啟,請至安全區域準備登出。");
                                Console.WriteLine("伺服器將於 " + i / 60 + " 分鐘後重新啟動");
                            } //TODO if (五分鐘內 一分鐘一次)
                            else if (i <= 30 && i != 0)
                            {
                                outerInstance.BroadCastToAll("伺服器將於 " + i + "秒後重新啟動,煩請儘速下線！");
                                Console.WriteLine("伺服器將於 " + i + " 秒後重新啟動");
                            } //TODO else if (30秒內 一秒一次)
                            else if (i == 0)
                            {
                                outerInstance.BroadCastToAll("伺服器自動重啟。");
                                Console.WriteLine("伺服器重新啟動。");
                                outerInstance.disconnectAllCharacters();
                                GameServer.Instance.shutdown(); //TODO 修正自動重開角色資料會回溯
                                Environment.Exit(1);
                            } //TODO if (1秒)

                            Thread.Sleep(1000);
                        }
                    }
                }
            }
        }

        public virtual void disconnectAllCharacters()
        {
            ICollection<L1PcInstance> players = L1World.Instance.AllPlayers;
            foreach (L1PcInstance pc in players)
            {
                pc.NetConnection.ActiveChar = null;
                pc.NetConnection.kick();
            }
            // 全員Kickした後に保存処理をする
            foreach (L1PcInstance pc in players)
            {
                ClientThread.quitGame(pc);
                L1World.Instance.removeObject(pc);
            }
        }

        private int GetRestartTime()
        {
            return Config.REST_TIME;
        }

        private void BroadCastToAll(string @string)
        {
            ICollection<L1PcInstance> allpc = L1World.Instance.AllPlayers;
            foreach (L1PcInstance pc in allpc)
            {
                pc.sendPackets(new S_SystemMessage(@string));
            }
        }

        public virtual void SetRemnant(int remnant)
        {
            _remnant = remnant;
        }

        public static int WillRestartTime
        {
            get
            {
                return willRestartTime;
            }
        }

        public virtual int GetRemnant()
        {
            return _remnant;
        }

        private bool isFieldChanged(DateTimeFieldTypeEnum dateTimeFieldType)
        {
            return _previousTime.GetValue(dateTimeFieldType) != _currentTime.GetValue(dateTimeFieldType);
        }

        private void notifyChanged()
        {
            if (isFieldChanged(DateTimeFieldTypeEnum.Month))
            {
                foreach (IL1GameTimeListener listener in _listeners)
                {
                    listener.OnMonthChanged(_currentTime);
                }
            }
            if (isFieldChanged(DateTimeFieldTypeEnum.Day))
            {
                foreach (IL1GameTimeListener listener in _listeners)
                {
                    listener.OnDayChanged(_currentTime);
                }
            }
            if (isFieldChanged(DateTimeFieldTypeEnum.Hour))
            {
                foreach (IL1GameTimeListener listener in _listeners)
                {
                    listener.OnHourChanged(_currentTime);
                }
            }
            if (isFieldChanged(DateTimeFieldTypeEnum.Minute))
            {
                foreach (IL1GameTimeListener listener in _listeners)
                {
                    listener.OnMinuteChanged(_currentTime);
                }
            }
        }

        private L1GameReStart()
        {
            RunnableExecuter.Instance.execute(new TimeUpdaterRestar(this));
        }

        public static void init()
        {
            _instance = new L1GameReStart();
        }

        public static L1GameReStart Instance
        {
            get
            {
                return _instance;
            }
        }

        public virtual L1GameTime GameTime
        {
            get
            {
                return _currentTime;
            }
        }

        public virtual void addListener(IL1GameTimeListener listener)
        {
            _listeners.Add(listener);
        }

        public virtual void removeListener(IL1GameTimeListener listener)
        {
            _listeners.Remove(listener);
        }
    }

}