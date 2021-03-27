using LineageServer.Utils;
using System.Collections.Generic;
using System.IO;

namespace LineageServer.Server.Model.map
{
	/// <summary>
	/// 讀取地圖的 abstract 類別
	/// </summary>
	public abstract class MapReader
	{
		/// <summary>
		/// 取得所有地圖與編號的 Mapping (abstract 方法).
		/// </summary>
		/// <returns> Map </returns>
		public abstract IDictionary<int, L1Map> read();

		/// <summary>
		/// 從快取地圖中讀取特定編號的地圖 (abstract 方法).
		/// </summary>
		/// <param name="id">
		///            地圖編號 </param>
		/// <returns> L1Map </returns>
		public abstract L1Map read(int id);
		/// <summary>
		/// 依照設定檔中的設定來選擇讀取地圖的方法(使用V2MapReader 或 快取地圖).
		/// </summary>
		/// <returns> MapReader </returns>
		public static MapReader DefaultReader
		{
			get
			{
				if (Config.LOAD_V2_MAP_FILES)
				{
					return new TextMapReader();
				}
				if (Config.CACHE_MAP_FILES)
				{
					return new TextMapReader();
				}
				return new TextMapReader();
			}
		}
	}

}