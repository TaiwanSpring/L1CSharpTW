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
	using BinaryInputStream = LineageServer.Server.Server.utils.BinaryInputStream;
	using FileUtil = LineageServer.Server.Server.utils.FileUtil;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	/// <summary>
	/// 地圖 (v2maps/\d*.txt)讀取 (測試用)
	/// </summary>
	public class V2MapReader : MapReader
	{

		/// <summary>
		/// 地圖的路徑 </summary>
		private const string MAP_DIR = "./v2maps/";

		/// <summary>
		/// 傳回所有地圖的編號
		/// </summary>
		/// <returns> ArraryList </returns>
		private IList<int> listMapIds()
		{
			IList<int> ids = Lists.newList();

			File mapDir = new File(MAP_DIR);
			foreach (string name in mapDir.list())
			{
				File mapFile = new File(mapDir, name);
				if (!mapFile.exists())
				{
					continue;
				}
				if (!FileUtil.getExtension(mapFile).ToLower().Equals("md"))
				{
					continue;
				}
				int id = 0;
				try
				{
					string idStr = FileUtil.getNameWithoutExtension(mapFile);
					id = int.Parse(idStr);
				}
				catch (System.FormatException)
				{
					continue;
				}
				ids.Add(id);
			}
			return ids;
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
			foreach (int id in listMapIds())
			{
				maps[id] = read(id);
			}
			return maps;
		}

		/// <summary>
		/// 從地圖中讀取特定編號的地圖
		/// </summary>
		/// <param name="mapId">
		///            地圖編號 </param>
		/// <returns> L1Map </returns>
		/// <exception cref="IOException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public L1Map read(final int mapId) throws java.io.IOException
		public override L1Map read(in int mapId)
		{
			File file = new File(MAP_DIR + mapId + ".md");
			if (!file.exists())
			{
				throw new FileNotFoundException("MapId: " + mapId);
			}

			BinaryInputStream @in = new BinaryInputStream(new BufferedInputStream(new InflaterInputStream(new FileStream(file, FileMode.Open, FileAccess.Read))));

			int id = @in.readInt();
			if (mapId != id)
			{
				throw new FileNotFoundException("MapId: " + mapId);
			}

			int xLoc = @in.readInt();
			int yLoc = @in.readInt();
			int width = @in.readInt();
			int height = @in.readInt();

			sbyte[] tiles = new sbyte[width * height * 2];
			for (int i = 0; i < width * height * 2; i++)
			{
				tiles[i] = (sbyte) @in.readByte();
			}
			@in.Close();

			L1V2Map map = new L1V2Map(id, tiles, xLoc, yLoc, width, height, MapsTable.Instance.isUnderwater(mapId), MapsTable.Instance.isMarkable(mapId), MapsTable.Instance.isTeleportable(mapId), MapsTable.Instance.isEscapable(mapId), MapsTable.Instance.isUseResurrection(mapId), MapsTable.Instance.isUsePainwand(mapId), MapsTable.Instance.isEnabledDeathPenalty(mapId), MapsTable.Instance.isTakePets(mapId), MapsTable.Instance.isRecallPets(mapId), MapsTable.Instance.isUsableItem(mapId), MapsTable.Instance.isUsableSkill(mapId));
			return map;
		}
	}

}