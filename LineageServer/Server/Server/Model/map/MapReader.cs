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

	using Config = LineageServer.Server.Config;

	/// <summary>
	/// 讀取地圖的 abstract 類別
	/// </summary>
	public abstract class MapReader
	{
		/// <summary>
		/// 取得所有地圖與編號的 Mapping (abstract 方法).
		/// </summary>
		/// <returns> Map </returns>
		/// <exception cref="IOException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract java.util.Map<int, L1Map> read() throws java.io.IOException;
		public abstract IDictionary<int, L1Map> read();

		/// <summary>
		/// 從快取地圖中讀取特定編號的地圖 (abstract 方法).
		/// </summary>
		/// <param name="id">
		///            地圖編號 </param>
		/// <returns> L1Map </returns>
		/// <exception cref="IOException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract L1Map read(int id) throws java.io.IOException;
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
					return new V2MapReader();
				}
				if (Config.CACHE_MAP_FILES)
				{
					return new CachedMapReader();
				}
				return new TextMapReader();
			}
		}
	}

}