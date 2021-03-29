using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LineageServer.Server.Model
{
    /// <summary>
    /// 檢查加速器使用的類別。
    /// </summary>
    class AcceleratorChecker
    {

        //JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
        private static readonly ILogger _log = Logger.GetLogger(nameof(AcceleratorChecker));

        private readonly L1PcInstance _pc;

        private int _injusticeCount;

        private int _justiceCount;

        private static readonly int INJUSTICE_COUNT_LIMIT = Config.INJUSTICE_COUNT;

        private static readonly int JUSTICE_COUNT_LIMIT = Config.JUSTICE_COUNT;

        // 実際には移動と攻撃のパケット間隔はsprの理論値より5%ほど遅い。
        // それを考慮して-5としている。
        private static readonly double CHECK_STRICTNESS = (Config.CHECK_STRICTNESS - 5) / 100D;

        private const double HASTE_RATE = 0.75; // 速度 * 1.33

        private const double WAFFLE_RATE = 0.87; // 速度 * 1.15

        private const double DOUBLE_HASTE_RATE = 0.375; // 速度 * 2.66

        private readonly Dictionary<ACT_TYPE, long> _actTimers = new Dictionary<ACT_TYPE, long>();

        private readonly Dictionary<ACT_TYPE, long> _checkTimers = new Dictionary<ACT_TYPE, long>();

        public enum ACT_TYPE
        {
            MOVE,
            ATTACK,
            SPELL_DIR,
            SPELL_NODIR
        }

        // 檢查結果
        public const int R_OK = 0;

        public const int R_DETECTED = 1;

        public const int R_DISPOSED = 2;

        public AcceleratorChecker(L1PcInstance pc)
        {
            _pc = pc;
            _injusticeCount = 0;
            _justiceCount = 0;
            long now = DateTime.Now.Ticks;
            foreach (object item in System.Enum.GetValues(typeof(ACT_TYPE)))
            {
                if (item is ACT_TYPE act)
                {
                    _actTimers[act] = now;
                    _checkTimers[act] = now;
                }
            }
        }

        /// <summary>
        /// アクションの間隔が不正でないかチェックし、適宜処理を行う。
        /// </summary>
        /// <param name="type">
        ///            - チェックするアクションのタイプ </param>
        /// <returns> 問題がなかった場合は0、不正であった場合は1、不正動作が一定回数に達した ためプレイヤーを切断した場合は2を返す。 </returns>
        public virtual int checkInterval(ACT_TYPE type)
        {
            int result = R_OK;
            long now = DateTime.Now.Ticks;
            long interval = now - _actTimers[type];
            int rightInterval = getRightInterval(type);

            interval *= (long)CHECK_STRICTNESS;

            if (0 < interval && interval < rightInterval)
            {
                _injusticeCount++;
                _justiceCount = 0;
                if (_injusticeCount >= INJUSTICE_COUNT_LIMIT)
                {
                    doPunishment(Config.ILLEGAL_SPEEDUP_PUNISHMENT);
                    return R_DISPOSED;
                }
                result = R_DETECTED;
            }
            else if (interval >= rightInterval)
            {
                _justiceCount++;
                if (_justiceCount >= JUSTICE_COUNT_LIMIT)
                {
                    _injusticeCount = 0;
                    _justiceCount = 0;
                }
            }

            // 検証用
            // double rate = (double) interval / rightInterval;
            // System.out.println(String.format("%s: %d / %d = %.2f (o-%d x-%d)",
            // type.toString(), interval, rightInterval, rate,
            // _justiceCount, _injusticeCount));

            _actTimers[type] = now;
            return result;
        }

        /// <summary>
        /// 加速檢測處罰 </summary>
        /// <param name="punishmaent"> 處罰模式 </param>
        private void doPunishment(int punishmaent)
        {
            if (!_pc.Gm)
            { // 如果不是GM才執行處罰
                int x = _pc.X, y = _pc.Y, mapid = _pc.MapId; // 紀錄座標
                _log.Info($"檢測到 {_pc.Name} 正在使用加速器");
                switch (punishmaent)
                {
                    case 0: // 剔除
                        _pc.sendPackets(new S_ServerMessage(945));
                        _pc.sendPackets(new S_Disconnect());
                        break;
                    case 1: // 鎖定人物10秒
                        _pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_BIND, true));
                        try
                        {
                            Thread.Sleep(10000); // 暫停十秒
                        }
                        catch (Exception e)
                        {
                            System.Console.WriteLine(e.Message);
                        }
                        _pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_BIND, false));
                        break;
                    case 2: // 傳到地域
                        L1Teleport.teleport(_pc, 32737, 32796, (short)99, 5, false);
                        _pc.sendPackets(new S_SystemMessage("因為你使用加速器，被傳送到了地獄，10秒後傳回。"));
                        try
                        {
                            Thread.Sleep(10000); // 暫停十秒
                        }
                        catch (Exception e)
                        {
                            System.Console.WriteLine(e.Message);
                        }
                        Task.Delay(10 * 1000).ContinueWith(task => L1Teleport.teleport(_pc, x, y, (short)mapid, 5, false));
                        break;
                    case 3: // 傳到GM房，30秒後傳回
                        L1Teleport.teleport(_pc, 32737, 32796, (short)99, 5, false);
                        _pc.sendPackets(new S_SystemMessage("因為你使用加速器，被傳送到了GM房，30秒後傳回。"));
                        Task.Delay(3 * 10 * 1000).ContinueWith(task => L1Teleport.teleport(_pc, x, y, (short)mapid, 5, false));
                        break;
                }

                if (Config.writeRobotsLog)
                {
                    _log.Log($"{_pc.Name}使用加速器"); // 加速器紀錄
                }
            }
            else
            {
                // GM不需要斷線
                _pc.sendPackets(new S_SystemMessage("遊戲管理員在遊戲中使用加速器檢測中。"));

                _injusticeCount = 0;
            }
        }

        /// <summary>
        /// PCの状態から指定された種類のアクションの正しいインターバル(ms)を計算し、返す。
        /// </summary>
        /// <param name="type">
        ///            - アクションの種類 </param>
        /// <param name="_pc">
        ///            - 調べるPC </param>
        /// <returns> 正しいインターバル(ms) </returns>
        private int getRightInterval(ACT_TYPE type)
        {
            int interval;

            // 動作判斷
            switch (type)
            {
                case LineageServer.Server.Model.AcceleratorChecker.ACT_TYPE.ATTACK:
                    interval = SprTable.Instance.getAttackSpeed(_pc.TempCharGfx, _pc.CurrentWeapon + 1);
                    break;
                case LineageServer.Server.Model.AcceleratorChecker.ACT_TYPE.MOVE:
                    interval = SprTable.Instance.getMoveSpeed(_pc.TempCharGfx, _pc.CurrentWeapon);
                    break;
                case LineageServer.Server.Model.AcceleratorChecker.ACT_TYPE.SPELL_DIR:
                    interval = SprTable.Instance.getDirSpellSpeed(_pc.TempCharGfx);
                    break;
                case LineageServer.Server.Model.AcceleratorChecker.ACT_TYPE.SPELL_NODIR:
                    interval = SprTable.Instance.getNodirSpellSpeed(_pc.TempCharGfx);
                    break;
                default:
                    return 0;
            }

            // 一段加速
            switch (_pc.MoveSpeed)
            {
                case 1: // 加速術
                    interval *= (int)HASTE_RATE;
                    break;
                case 2: // 緩速術
                    interval /= (int)HASTE_RATE;
                    break;
                default:
                    break;
            }

            // 二段加速
            switch (_pc.BraveSpeed)
            {
                case 1: // 勇水
                    interval *= (int)HASTE_RATE; // 攻速、移速 * 1.33倍
                    break;
                case 3: // 精餅
                    interval *= (int)WAFFLE_RATE; // 攻速、移速 * 1.15倍
                    break;
                case 4: // 神疾、風走、行走
                    if (type.Equals(ACT_TYPE.MOVE))
                    {
                        interval *= (int)HASTE_RATE; // 移速 * 1.33倍
                    }
                    break;
                case 5: // 超級加速
                    interval *= (int)DOUBLE_HASTE_RATE; // 攻速、移速 * 2.66倍
                    break;
                case 6: // 血之渴望
                    if (type.Equals(ACT_TYPE.ATTACK))
                    {
                        interval *= (int)HASTE_RATE; // 攻速 * 1.33倍
                    }
                    break;
                default:
                    break;
            }

            // 生命之樹果實
            if (_pc.Ribrave && type.Equals(ACT_TYPE.MOVE))
            { // 移速 * 1.15倍
                interval *= (int)WAFFLE_RATE;
            }
            // 三段加速
            if (_pc.ThirdSpeed)
            { // 攻速、移速 * 1.15倍
                interval *= (int)WAFFLE_RATE;
            }
            // 風之枷鎖
            if (_pc.WindShackle && !type.Equals(ACT_TYPE.MOVE))
            { // 攻速or施法速度 / 2倍
                interval /= 2;
            }
            if (_pc.MapId == 5143)
            { // 寵物競速例外
                interval = (int)(interval * 0.1);
            }
            return interval;
        }
    }

}