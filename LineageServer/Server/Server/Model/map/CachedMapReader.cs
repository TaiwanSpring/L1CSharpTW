using System.Collections.Generic;
using System.IO;

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

	using MapsTable = LineageServer.Server.Server.datatables.MapsTable;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	/// <summary>
	/// 將地圖做快取的動作以減少讀取的時間。
	/// </summary>
	public class CachedMapReader : MapReader
	{

		/// <summary>
		/// 地圖檔的路徑 </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unused") private static final String MAP_DIR = "./maps/";
		private const string MAP_DIR = "./maps/";

		/// <summary>
		/// cache 後地圖檔的路徑 </summary>
		private const string CACHE_DIR = "./data/mapcache/";

		/// <summary>
		/// 將指定編號的地圖轉成快取的地圖格式
		/// </summary>
		/// <param name="mapId">
		///            地圖編號 </param>
		/// <returns> L1V1Map </returns>
		/// <exception cref="IOException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private L1V1Map cacheMap(final int mapId) throws java.io.IOException
		private L1V1Map cacheMap(in int mapId)
		{
			File file = new File(CACHE_DIR);
			if (!file.exists())
			{
				file.mkdir();
			}

			L1V1Map map = (L1V1Map) (new TextMapReader()).read(mapId);

			DataOutputStream @out = new DataOutputStream(new BufferedOutputStream(new FileStream(CACHE_DIR + mapId + ".map", FileMode.Create, FileAccess.Write)));

			@out.writeInt(map.Id);
			@out.writeInt(map.X);
			@out.writeInt(map.Y);
			@out.writeInt(map.Width);
			@out.writeInt(map.Height);

			foreach (sbyte[] line in map.RawTiles)
			{
				foreach (sbyte tile in line)
				{
					@out.writeByte(tile);
				}
			}
			@out.flush();
			@out.close();

			return map;
		}

		/// <summary>
		/// 從快取地圖中讀取特定編號的地圖
		/// </summary>
		/// <param name="mapId">
		///            地圖編號 </param>
		/// <returns> L1Map </returns>
		/// <exception cref="IOException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public L1Map read(final int mapId) throws java.io.IOException
		public override L1Map read(in int mapId)
		{
			File file = new File(CACHE_DIR + mapId + ".map");
			if (!file.exists())
			{
				return cacheMap(mapId);
			}

			DataInputStream @in = new DataInputStream(new BufferedInputStream(new FileStream(CACHE_DIR + mapId + ".map", FileMode.Open, FileAccess.Read)));

			int id = @in.readInt();
			if (mapId != id)
			{
				@in.close();
				throw new FileNotFoundException();
			}

			int xLoc = @in.readInt();
			int yLoc = @in.readInt();
			int width = @in.readInt();
			int height = @in.readInt();

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: sbyte[][] tiles = new sbyte[width][height];
			sbyte[][] tiles = RectangularArrays.RectangularSbyteArray(width, height);
			foreach (sbyte[] line in tiles)
			{
				@in.read(line);
			}

			@in.close();
			L1V1Map map = new L1V1Map(id, tiles, xLoc, yLoc, MapsTable.Instance.isUnderwater(mapId), MapsTable.Instance.isMarkable(mapId), MapsTable.Instance.isTeleportable(mapId), MapsTable.Instance.isEscapable(mapId), MapsTable.Instance.isUseResurrection(mapId), MapsTable.Instance.isUsePainwand(mapId), MapsTable.Instance.isEnabledDeathPenalty(mapId), MapsTable.Instance.isTakePets(mapId), MapsTable.Instance.isRecallPets(mapId), MapsTable.Instance.isUsableItem(mapId), MapsTable.Instance.isUsableSkill(mapId));
			return map;
		}

		/// <summary>
		/// 取得所有地圖與編號的 Mapping
		/// </summary>
		/// <returns> Map </returns>
		/// <exception cref="IOException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public java.util.Map<int, L1Map> read() throws java.io.IOException
		public override IDictionary<int, L1Map> read()
		{
			IDictionary<int, L1Map> maps = Maps.newMap();
			foreach (int id in TextMapReader.listMapIds())
			{
				maps[id] = read(id);
			}
			return maps;
		}
	}

}