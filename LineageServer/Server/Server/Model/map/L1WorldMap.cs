using LineageServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LineageServer.Server.Server.Model.map
{
	public class L1WorldMap
	{
		private static ILogger _log = Logger.getLogger(nameof(L1WorldMap));

		private static L1WorldMap _instance;
		private IDictionary<int, L1Map> _maps;

		public static L1WorldMap Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new L1WorldMap();
				}
				return _instance;
			}
		}

		private L1WorldMap()
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
				_log.log(Enum.Level.Server, e.Message, e);
				Environment.Exit(0);
			}
			timer.Stop();
			System.Console.WriteLine("【完成】【" + timer.ElapsedMilliseconds + "】【毫秒】。");
		}

		/// <summary>
		/// 指定されたマップの情報を保持するL1Mapを返す。
		/// </summary>
		/// <param name="mapId">
		///            マップID </param>
		/// <returns> マップ情報を保持する、L1Mapオブジェクト。 </returns>
		public virtual L1Map getMap(short mapId)
		{
			L1Map map = _maps[(int)mapId];
			if (map == null)
			{ // マップ情報が無い
				map = L1Map.NullMap; // 何もしないMapを返す。
			}
			return map;
		}
	}

}