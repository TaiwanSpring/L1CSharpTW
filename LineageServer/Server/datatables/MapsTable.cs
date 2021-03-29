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

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using SQLUtil = LineageServer.Utils.SQLUtil;
	using MapFactory = LineageServer.Utils.MapFactory;

	public sealed class MapsTable
	{
		private class MapData
		{
			private readonly MapsTable outerInstance;

			public MapData(MapsTable outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public int startX = 0;

			public int endX = 0;

			public int startY = 0;

			public int endY = 0;

			public double monster_amount = 1;

			public double dropRate = 1;

			public bool isUnderwater = false;

			public bool markable = false;

			public bool teleportable = false;

			public bool escapable = false;

			public bool isUseResurrection = false;

			public bool isUsePainwand = false;

			public bool isEnabledDeathPenalty = false;

			public bool isTakePets = false;

			public bool isRecallPets = false;

			public bool isUsableItem = false;

			public bool isUsableSkill = false;
		}

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(MapsTable).FullName);

		private static MapsTable _instance;

		/// <summary>
		/// KeyにマップID、Valueにテレポート可否フラグが格納されるHashMap
		/// </summary>
		private readonly IDictionary<int, MapData> _maps = MapFactory.newMap();

		/// <summary>
		/// 新しくMapsTableオブジェクトを生成し、マップのテレポート可否フラグを読み込む。
		/// </summary>
		private MapsTable()
		{
			loadMapsFromDatabase();
		}

		/// <summary>
		/// マップのテレポート可否フラグをデータベースから読み込み、HashMap _mapsに格納する。
		/// </summary>
		private void loadMapsFromDatabase()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM mapids");

				for (rs = pstm.executeQuery(); rs.next();)
				{
					MapData data = new MapData(this);
					int mapId = dataSourceRow.getInt("mapid");
					// dataSourceRow.getString("locationname");
					data.startX = dataSourceRow.getInt("startX");
					data.endX = dataSourceRow.getInt("endX");
					data.startY = dataSourceRow.getInt("startY");
					data.endY = dataSourceRow.getInt("endY");
					data.monster_amount = dataSourceRow.getDouble("monster_amount");
					data.dropRate = dataSourceRow.getDouble("drop_rate");
					data.isUnderwater = dataSourceRow.getBoolean("underwater");
					data.markable = dataSourceRow.getBoolean("markable");
					data.teleportable = dataSourceRow.getBoolean("teleportable");
					data.escapable = dataSourceRow.getBoolean("escapable");
					data.isUseResurrection = dataSourceRow.getBoolean("resurrection");
					data.isUsePainwand = dataSourceRow.getBoolean("painwand");
					data.isEnabledDeathPenalty = dataSourceRow.getBoolean("penalty");
					data.isTakePets = dataSourceRow.getBoolean("take_pets");
					data.isRecallPets = dataSourceRow.getBoolean("recall_pets");
					data.isUsableItem = dataSourceRow.getBoolean("usable_item");
					data.isUsableSkill = dataSourceRow.getBoolean("usable_skill");

					_maps[mapId] = data;
				}

				_log.config("Maps " + _maps.Count);
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
		}

		/// <summary>
		/// MapsTableのインスタンスを返す。
		/// </summary>
		/// <returns> MapsTableのインスタンス </returns>
		public static MapsTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new MapsTable();
				}
				return _instance;
			}
		}

		/// <summary>
		/// マップがのX開始座標を返す。
		/// </summary>
		/// <param name="mapId">
		///            調べるマップのマップID </param>
		/// <returns> X開始座標 </returns>
		public int getStartX(int mapId)
		{
			MapData map = _maps[mapId];
			if (map == null)
			{
				return 0;
			}
			return _maps[mapId].startX;
		}

		/// <summary>
		/// マップがのX終了座標を返す。
		/// </summary>
		/// <param name="mapId">
		///            調べるマップのマップID </param>
		/// <returns> X終了座標 </returns>
		public int getEndX(int mapId)
		{
			MapData map = _maps[mapId];
			if (map == null)
			{
				return 0;
			}
			return _maps[mapId].endX;
		}

		/// <summary>
		/// マップがのY開始座標を返す。
		/// </summary>
		/// <param name="mapId">
		///            調べるマップのマップID </param>
		/// <returns> Y開始座標 </returns>
		public int getStartY(int mapId)
		{
			MapData map = _maps[mapId];
			if (map == null)
			{
				return 0;
			}
			return _maps[mapId].startY;
		}

		/// <summary>
		/// マップがのY終了座標を返す。
		/// </summary>
		/// <param name="mapId">
		///            調べるマップのマップID </param>
		/// <returns> Y終了座標 </returns>
		public int getEndY(int mapId)
		{
			MapData map = _maps[mapId];
			if (map == null)
			{
				return 0;
			}
			return _maps[mapId].endY;
		}

		/// <summary>
		/// マップのモンスター量倍率を返す
		/// </summary>
		/// <param name="mapId">
		///            調べるマップのマップID </param>
		/// <returns> モンスター量の倍率 </returns>
		public double getMonsterAmount(int mapId)
		{
			MapData map = _maps[mapId];
			if (map == null)
			{
				return 0;
			}
			return map.monster_amount;
		}

		/// <summary>
		/// マップのドロップ倍率を返す
		/// </summary>
		/// <param name="mapId">
		///            調べるマップのマップID </param>
		/// <returns> ドロップ倍率 </returns>
		public double getDropRate(int mapId)
		{
			MapData map = _maps[mapId];
			if (map == null)
			{
				return 0;
			}
			return map.dropRate;
		}

		/// <summary>
		/// マップが、水中であるかを返す。
		/// </summary>
		/// <param name="mapId">
		///            調べるマップのマップID
		/// </param>
		/// <returns> 水中であればtrue </returns>
		public bool isUnderwater(int mapId)
		{
			MapData map = _maps[mapId];
			if (map == null)
			{
				return false;
			}
			return _maps[mapId].isUnderwater;
		}

		/// <summary>
		/// マップが、ブックマーク可能であるかを返す。
		/// </summary>
		/// <param name="mapId">
		///            調べるマップのマップID </param>
		/// <returns> ブックマーク可能であればtrue </returns>
		public bool isMarkable(int mapId)
		{
			MapData map = _maps[mapId];
			if (map == null)
			{
				return false;
			}
			return _maps[mapId].markable;
		}

		/// <summary>
		/// マップが、ランダムテレポート可能であるかを返す。
		/// </summary>
		/// <param name="mapId">
		///            調べるマップのマップID </param>
		/// <returns> 可能であればtrue </returns>
		public bool isTeleportable(int mapId)
		{
			MapData map = _maps[mapId];
			if (map == null)
			{
				return false;
			}
			return _maps[mapId].teleportable;
		}

		/// <summary>
		/// マップが、MAPを超えたテレポート可能であるかを返す。
		/// </summary>
		/// <param name="mapId">
		///            調べるマップのマップID </param>
		/// <returns> 可能であればtrue </returns>
		public bool isEscapable(int mapId)
		{
			MapData map = _maps[mapId];
			if (map == null)
			{
				return false;
			}
			return _maps[mapId].escapable;
		}

		/// <summary>
		/// マップが、復活可能であるかを返す。
		/// </summary>
		/// <param name="mapId">
		///            調べるマップのマップID
		/// </param>
		/// <returns> 復活可能であればtrue </returns>
		public bool isUseResurrection(int mapId)
		{
			MapData map = _maps[mapId];
			if (map == null)
			{
				return false;
			}
			return _maps[mapId].isUseResurrection;
		}

		/// <summary>
		/// マップが、パインワンド使用可能であるかを返す。
		/// </summary>
		/// <param name="mapId">
		///            調べるマップのマップID
		/// </param>
		/// <returns> パインワンド使用可能であればtrue </returns>
		public bool isUsePainwand(int mapId)
		{
			MapData map = _maps[mapId];
			if (map == null)
			{
				return false;
			}
			return _maps[mapId].isUsePainwand;
		}

		/// <summary>
		/// マップが、デスペナルティがあるかを返す。
		/// </summary>
		/// <param name="mapId">
		///            調べるマップのマップID
		/// </param>
		/// <returns> デスペナルティであればtrue </returns>
		public bool isEnabledDeathPenalty(int mapId)
		{
			MapData map = _maps[mapId];
			if (map == null)
			{
				return false;
			}
			return _maps[mapId].isEnabledDeathPenalty;
		}

		/// <summary>
		/// マップが、ペット・サモンを連れて行けるかを返す。
		/// </summary>
		/// <param name="mapId">
		///            調べるマップのマップID
		/// </param>
		/// <returns> ペット・サモンを連れて行けるならばtrue </returns>
		public bool isTakePets(int mapId)
		{
			MapData map = _maps[mapId];
			if (map == null)
			{
				return false;
			}
			return _maps[mapId].isTakePets;
		}

		/// <summary>
		/// マップが、ペット・サモンを呼び出せるかを返す。
		/// </summary>
		/// <param name="mapId">
		///            調べるマップのマップID
		/// </param>
		/// <returns> ペット・サモンを呼び出せるならばtrue </returns>
		public bool isRecallPets(int mapId)
		{
			MapData map = _maps[mapId];
			if (map == null)
			{
				return false;
			}
			return _maps[mapId].isRecallPets;
		}

		/// <summary>
		/// マップが、アイテムを使用できるかを返す。
		/// </summary>
		/// <param name="mapId">
		///            調べるマップのマップID
		/// </param>
		/// <returns> アイテムを使用できるならばtrue </returns>
		public bool isUsableItem(int mapId)
		{
			MapData map = _maps[mapId];
			if (map == null)
			{
				return false;
			}
			return _maps[mapId].isUsableItem;
		}

		/// <summary>
		/// マップが、スキルを使用できるかを返す。
		/// </summary>
		/// <param name="mapId">
		///            調べるマップのマップID
		/// </param>
		/// <returns> スキルを使用できるならばtrue </returns>
		public bool isUsableSkill(int mapId)
		{
			MapData map = _maps[mapId];
			if (map == null)
			{
				return false;
			}
			return _maps[mapId].isUsableSkill;
		}

	}

}