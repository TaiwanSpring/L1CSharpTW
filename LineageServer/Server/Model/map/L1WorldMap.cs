using LineageServer.Interfaces;
using LineageServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LineageServer.Server.Model.Map
{
    class L1WorldMap : IGameComponent, IWorldMap
    {
        private static ILogger _log = Logger.GetLogger(nameof(L1WorldMap));

        private IDictionary<int, L1Map> _maps;

        /// <summary>
        /// 指定されたマップの情報を保持するL1Mapを返す。
        /// </summary>
        /// <param name="mapId">
        ///            マップID </param>
        /// <returns> マップ情報を保持する、L1Mapオブジェクト。 </returns>
        public virtual L1Map getMap(short mapId)
        {
            if (_maps.ContainsKey(mapId))
            {
                return _maps[mapId];
            }
            else
            {
                return L1Map.NullMap;
            }
        }

        public void Initialize()
        {
            Stopwatch timer = Stopwatch.StartNew();
            System.Console.Write("【讀取】 【遊戲地圖 】【設定】");
            try
            {
                _maps = MapReader.DefaultReader.read();
                if (_maps == null)
                {
                    throw new Exception("地圖檔案讀取失敗...");
                }
            }
            catch (Exception e)
            {
                // 復帰不能
                _log.Error(e);
                throw e;
            }
            timer.Stop();
            System.Console.WriteLine("【完成】【" + timer.ElapsedMilliseconds + "】【毫秒】。");
        }
    }

}