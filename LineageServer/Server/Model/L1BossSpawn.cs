using LineageServer.Interfaces;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Text;
namespace LineageServer.Server.Model
{
    class L1BossSpawn : L1Spawn
    {
        private class SpawnTask : IRunnable
        {
            private readonly L1BossSpawn outerInstance;

            internal int _spawnNumber;
            internal int _objectId;

            internal SpawnTask(L1BossSpawn outerInstance, int spawnNumber, int objectId)
            {
                this.outerInstance = outerInstance;
                _spawnNumber = spawnNumber;
                _objectId = objectId;
            }

            public void run()
            {
                outerInstance.doSpawn(_spawnNumber, _objectId);
            }
        }

        public L1BossSpawn(L1Npc mobTemplate) : base(mobTemplate)
        {
        }

        /// <summary>
        /// SpawnTaskを起動する。
        /// </summary>
        /// <param name="spawnNumber">
        ///            L1Spawnで管理されている番号。ホームポイントが無ければ何を指定しても良い。 </param>
        public void executeSpawnTask(int spawnNumber, int objectId)
        {
            // countをデクリメントして全部死んだかチェック
            if (subAndGetCount() != 0)
            {
                return; // 全部死んでいない
            }
            // 前回出現時間に対して、次の出現時間を算出
            DateTime spawnTime;
            DateTime now = new DateTime(); // 現時刻
            DateTime latestStart = _cycle.getLatestStartTime(now); // 現時刻に対する最近の周期の開始時間

            DateTime activeStart = _cycle.getSpawnStartTime(_activeSpawnTime); // アクティブだった周期の開始時間
                                                                               // アクティブだった周期の開始時間 >= 最近の周期開始時間の場合、次の出現
            if (activeStart >= latestStart)
            {
                spawnTime = calcNextSpawnTime(activeStart);
            }
            else
            {
                // アクティブだった周期の開始時間 < 最近の周期開始時間の場合は、最近の周期で出現
                // わかりづらいが確率計算する為に、無理やりcalcNextSpawnTimeを通している。
                latestStart.AddSeconds(-1);
                spawnTime = calcNextSpawnTime(_cycle.getLatestStartTime(latestStart));
            }
            spawnBoss(spawnTime, objectId);
        }

        private int _spawnCount;

        private int subAndGetCount()
        {
            lock (this)
            {
                return --_spawnCount;
            }
        }

        private string _cycleType;

        public virtual string CycleType
        {
            set
            {
                _cycleType = value;
            }
        }

        private int _percentage;

        public virtual int Percentage
        {
            set
            {
                _percentage = value;
            }
        }

        private L1BossCycle _cycle;

        private DateTime _activeSpawnTime;

        public override void init()
        {
            if (_percentage <= 0)
            {
                return;
            }
            //_cycle = L1BossCycle.getBossCycle(_cycleType);
            if (_cycle == null)
            {
                throw new Exception(_cycleType + " not found");
            }
            DateTime now = new DateTime();
            // 出現時間
            DateTime spawnTime;
            if (Config.INIT_BOSS_SPAWN && _percentage > RandomHelper.Next(100))
            {
                spawnTime = _cycle.calcSpawnTime(now);

            }
            else
            {
                spawnTime = calcNextSpawnTime(now);
            }
            spawnBoss(spawnTime, 0);
        }

        // 確率計算して次の出現時間を算出
        private DateTime calcNextSpawnTime(DateTime cal)
        {
            do
            {
                cal = _cycle.nextSpawnTime(cal);
            } while (!(_percentage > RandomHelper.Next(100)));
            return cal;
        }

        // 指定された時間でボス出現をスケジュール
        private void spawnBoss(DateTime spawnTime, int objectId)
        {
            // 今回の出現時間を保存しておく。再出現時に使用。
            _activeSpawnTime = spawnTime;
            int delay = (int)(spawnTime - DateTime.Now).TotalMilliseconds;

            int cnt = _spawnCount;
            _spawnCount = Amount;
            while (cnt < Amount)
            {
                cnt++;
                Container.Instance.Resolve<ITaskController>().execute(new SpawnTask(this, cnt, objectId), delay);
            }
        }

        /// <summary>
        /// 現在アクティブなボスに対する周期と出現時間を表す。
        /// </summary>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[MOB]npcid:" + NpcId);
            builder.Append(" name:" + Name);
            builder.Append("[Type]" + _cycle.Name);
            builder.Append("[現在の周期]");
            builder.Append(_cycle.getSpawnStartTime(_activeSpawnTime));
            builder.Append(" - ");
            builder.Append(_cycle.getSpawnEndTime(_activeSpawnTime));
            builder.Append("[出現時間]");
            builder.Append(_activeSpawnTime);
            return builder.ToString();
        }
    }

}