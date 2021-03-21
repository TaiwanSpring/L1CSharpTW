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
namespace LineageServer.Server.Server.Model.map
{

	using PerformanceTimer = LineageServer.Server.Server.utils.PerformanceTimer;

	public class L1WorldMap
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1WorldMap).FullName);

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
			PerformanceTimer timer = new PerformanceTimer();
            System.Console.Write("【讀取】 【遊戲地圖 】【設定】");

			try
			{
				_maps = MapReader.DefaultReader.read();
				if (_maps == null)
				{
					throw new Exception("地圖檔案讀取失敗...");
				}
			}
			catch (FileNotFoundException)
			{
                System.Console.WriteLine("提示: 地圖檔案缺失，請檢查330_maps.zip是否尚未解壓縮。");
				Environment.Exit(0);
			}
			catch (Exception e)
			{
				// 復帰不能
				_log.log(Enum.Level.Server, e.Message, e);
				Environment.Exit(0);
			}

            System.Console.WriteLine("【完成】【" + timer.get() + "】【毫秒】。");
		}

		/// <summary>
		/// 指定されたマップの情報を保持するL1Mapを返す。
		/// </summary>
		/// <param name="mapId">
		///            マップID </param>
		/// <returns> マップ情報を保持する、L1Mapオブジェクト。 </returns>
		public virtual L1Map getMap(short mapId)
		{
			L1Map map = _maps[(int) mapId];
			if (map == null)
			{ // マップ情報が無い
				map = L1Map.newNull(); // 何もしないMapを返す。
			}
			return map;
		}
	}

}