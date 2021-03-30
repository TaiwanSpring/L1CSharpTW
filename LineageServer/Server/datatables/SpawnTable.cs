using System;
using System.Collections.Generic;

/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.DataTables
{

    using Config = LineageServer.Server.Config;
    using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
    using L1Spawn = LineageServer.Server.Model.L1Spawn;
    using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
    using L1Npc = LineageServer.Server.Templates.L1Npc;
    using NumberUtil = LineageServer.Utils.NumberUtil;
    using PerformanceTimer = LineageServer.Utils.PerformanceTimer;
    using SQLUtil = LineageServer.Utils.SQLUtil;
    using MapFactory = LineageServer.Utils.MapFactory;

    public class SpawnTable
    {
        //JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
        private static Logger _log = Logger.GetLogger(typeof(SpawnTable).FullName);

        private static SpawnTable _instance;

        private IDictionary<int, L1Spawn> _spawntable = MapFactory.NewMap();

        private int _highestId;

        public static SpawnTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SpawnTable();
                }
                return _instance;
            }
        }

        private SpawnTable()
        {
            PerformanceTimer timer = new PerformanceTimer();
            System.Console.Write("【讀取】 【spawn mob】【設定】");
            fillSpawnTable();
            _log.config("モンスター配置リスト " + _spawntable.Count + "件ロード");
            System.Console.WriteLine("【完成】【" + timer.get() + "】【毫秒】。");
        }

        private void fillSpawnTable()
        {

            int spawnCount = 0;
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            ResultSet rs = null;
            try
            {

                con = L1DatabaseFactory.Instance.Connection;
                pstm = con.prepareStatement("SELECT * FROM spawnlist");
                rs = pstm.executeQuery();

                L1Spawn spawnDat;
                L1Npc template1;
                while (rs.next())
                {
                    if (Config.ALT_HALLOWEENIVENT == false)
                    {
                        int npcid = dataSourceRow.getInt("id");
                        if ((npcid >= 26656) && (npcid <= 26734))
                        {
                            continue;
                        }
                    }
                    int npcTemplateId = dataSourceRow.getInt("npc_templateid");
                    template1 = NpcTable.Instance.getTemplate(npcTemplateId);
                    int count;

                    if (template1 == null)
                    {
                        _log.Warning("mob data for id:" + npcTemplateId + " missing in npc table");
                        spawnDat = null;
                    }
                    else
                    {
                        if (dataSourceRow.getInt("count") == 0)
                        {
                            continue;
                        }
                        double amount_rate = MapsTable.Instance.getMonsterAmount(dataSourceRow.getShort("mapid"));
                        count = calcCount(template1, dataSourceRow.getInt("count"), amount_rate);
                        if (count == 0)
                        {
                            continue;
                        }

                        spawnDat = new L1Spawn(template1);
                        spawnDat.Id = dataSourceRow.getInt("id");
                        spawnDat.Amount = count;
                        spawnDat.GroupId = dataSourceRow.getInt("group_id");
                        spawnDat.LocX = dataSourceRow.getInt("locx");
                        spawnDat.LocY = dataSourceRow.getInt("locy");
                        spawnDat.Randomx = dataSourceRow.getInt("randomx");
                        spawnDat.Randomy = dataSourceRow.getInt("randomy");
                        spawnDat.LocX1 = dataSourceRow.getInt("locx1");
                        spawnDat.LocY1 = dataSourceRow.getInt("locy1");
                        spawnDat.LocX2 = dataSourceRow.getInt("locx2");
                        spawnDat.LocY2 = dataSourceRow.getInt("locy2");
                        spawnDat.Heading = dataSourceRow.getInt("heading");
                        spawnDat.MinRespawnDelay = dataSourceRow.getInt("min_respawn_delay");
                        spawnDat.MaxRespawnDelay = dataSourceRow.getInt("max_respawn_delay");
                        spawnDat.MapId = dataSourceRow.getShort("mapid");
                        spawnDat.RespawnScreen = dataSourceRow.getBoolean("respawn_screen");
                        spawnDat.MovementDistance = dataSourceRow.getInt("movement_distance");
                        spawnDat.Rest = dataSourceRow.getBoolean("rest");
                        spawnDat.SpawnType = dataSourceRow.getInt("near_spawn");
                        spawnDat.Time = SpawnTimeTable.Instance.get(spawnDat.Id);

                        spawnDat.Name = template1.get_name();

                        if ((count > 1) && (spawnDat.LocX1 == 0))
                        {
                            // 複数かつ固定spawnの場合は、個体数 * 6 の範囲spawnに変える。
                            // ただし範囲が30を超えないようにする
                            int range = Math.Min(count * 6, 30);
                            spawnDat.LocX1 = spawnDat.LocX - range;
                            spawnDat.LocY1 = spawnDat.LocY - range;
                            spawnDat.LocX2 = spawnDat.LocX + range;
                            spawnDat.LocY2 = spawnDat.LocY + range;
                        }

                        // start the spawning
                        spawnDat.init();
                        spawnCount += spawnDat.Amount;
                    }

                    _spawntable[spawnDat.Id] = spawnDat;
                    if (spawnDat.Id > _highestId)
                    {
                        _highestId = spawnDat.Id;
                    }
                }

            }
            catch (SQLException e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
            finally
            {
                SQLUtil.close(rs);
                SQLUtil.close(pstm);
                SQLUtil.close(con);
            }
            _log.fine("総モンスター数 " + spawnCount + "匹");
        }

        public virtual L1Spawn getTemplate(int Id)
        {
            return _spawntable[Id];
        }

        public virtual void addNewSpawn(L1Spawn spawn)
        {
            _highestId++;
            spawn.Id = _highestId;
            _spawntable[spawn.Id] = spawn;
        }

        public static void storeSpawn(L1PcInstance pc, L1Npc npc)
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            try
            {
                int count = 1;
                int randomXY = 12;
                int minRespawnDelay = 60;
                int maxRespawnDelay = 120;
                string note = npc.get_name();

                con = L1DatabaseFactory.Instance.Connection;
                pstm = con.prepareStatement("INSERT INTO spawnlist SET location=?,count=?,npc_templateid=?,group_id=?,locx=?,locy=?,randomx=?,randomy=?,heading=?,min_respawn_delay=?,max_respawn_delay=?,mapid=?");
                pstm.setString(1, note);
                pstm.setInt(2, count);
                pstm.setInt(3, npc.get_npcId());
                pstm.setInt(4, 0);
                pstm.setInt(5, pc.X);
                pstm.setInt(6, pc.Y);
                pstm.setInt(7, randomXY);
                pstm.setInt(8, randomXY);
                pstm.setInt(9, pc.Heading);
                pstm.setInt(10, minRespawnDelay);
                pstm.setInt(11, maxRespawnDelay);
                pstm.setInt(12, pc.MapId);
                pstm.execute();

            }
            catch (Exception e)
            {
                NpcTable._log.Error(e);
            }
            finally
            {
                SQLUtil.close(pstm);
                SQLUtil.close(con);
            }
        }

        private static int calcCount(L1Npc npc, int count, double rate)
        {
            if (rate == 0)
            {
                return 0;
            }
            if ((rate == 1) || npc.AmountFixed)
            {
                return count;
            }
            else
            {
                return randomRound((count * rate));
            }
        }

        /// <summary>
        /// 少数を小数点第二位までの確率で上か下に丸めた整数を返す。
        /// 例えば1.3は30%の確率で切り捨て、70%の確率で切り上げられる。
        /// </summary>
        /// <param name="number"> - もとの少数 </param>
        /// <returns> 丸められた整数 </returns>
        public static int randomRound(double number)
        {
            double percentage = (number - Math.Floor(number)) * 100;

            if (percentage == 0)
            {
                return ((int)number);
            }
            else
            {
                int r = RandomHelper.Next(100);

                if (r < percentage)
                {
                    return ((int)number + 1);
                }
                else
                {
                    return ((int)number);
                }
            }
        }
    }

}