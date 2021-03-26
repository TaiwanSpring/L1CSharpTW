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
namespace LineageServer.Server.Server.Model
{

	using CastleTable = LineageServer.Server.Server.DataSources.CastleTable;
	using L1GameTime = LineageServer.Server.Server.Model.Gametime.L1GameTime;
	using IL1GameTimeListener = LineageServer.Server.Server.Model.Gametime.IL1GameTimeListener;
	using L1GameTimeClock = LineageServer.Server.Server.Model.Gametime.L1GameTimeClock;
	using L1Castle = LineageServer.Server.Server.Templates.L1Castle;
	using Random = LineageServer.Server.Server.utils.Random;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	// Referenced classes of package l1j.server.server.model:
	// L1CastleLocation

	public class L1CastleLocation
	{
		// castle_id
		public const int KENT_CASTLE_ID = 1;

		public const int OT_CASTLE_ID = 2;

		public const int WW_CASTLE_ID = 3;

		public const int GIRAN_CASTLE_ID = 4;

		public const int HEINE_CASTLE_ID = 5;

		public const int DOWA_CASTLE_ID = 6;

		public const int ADEN_CASTLE_ID = 7;

		public const int DIAD_CASTLE_ID = 8;

		// →↑がX軸、→↓がY軸
		// 肯特城  Update to 3.3C
		private const int KENT_TOWER_X = 33139;

		private const int KENT_TOWER_Y = 32768;

		private const short KENT_TOWER_MAP = 4;

		private const int KENT_X1 = 33089;

		private const int KENT_X2 = 33219;

		private const int KENT_Y1 = 32717;

		private const int KENT_Y2 = 32827;

		private const short KENT_MAP = 4;

		private const short KENT_INNER_CASTLE_MAP = 15;

		// 妖魔城堡  Update to 3.3C
		private const int OT_TOWER_X = 32798;

		private const int OT_TOWER_Y = 32291;

		private const short OT_TOWER_MAP = 4;

		private const int OT_X1 = 32750;

		private const int OT_X2 = 32850;

		private const int OT_Y1 = 32250;

		private const int OT_Y2 = 32350;

		private const short OT_MAP = 4;

		// 風木城  Update to 3.3C
		private const int WW_TOWER_X = 32623;

		private const int WW_TOWER_Y = 33379;

		private const short WW_TOWER_MAP = 4;

		private const int WW_X1 = 32571;

		private const int WW_X2 = 32721;

		private const int WW_Y1 = 33350;

		private const int WW_Y2 = 33460;

		private const short WW_MAP = 4;

		private const short WW_INNER_CASTLE_MAP = 29;

		// 奇岩城
		private const int GIRAN_TOWER_X = 33631;

		private const int GIRAN_TOWER_Y = 32678;

		private const short GIRAN_TOWER_MAP = 4;

		private const int GIRAN_X1 = 33559;

		private const int GIRAN_X2 = 33686;

		private const int GIRAN_Y1 = 32615;

		private const int GIRAN_Y2 = 32755;

		private const short GIRAN_MAP = 4;

		private const short GIRAN_INNER_CASTLE_MAP = 52;

		// 海音城
		private const int HEINE_TOWER_X = 33524;

		private const int HEINE_TOWER_Y = 33396;

		private const short HEINE_TOWER_MAP = 4;

		private const int HEINE_X1 = 33458;

		private const int HEINE_X2 = 33583;

		private const int HEINE_Y1 = 33315;

		private const int HEINE_Y2 = 33490;

		private const short HEINE_MAP = 4;

		private const short HEINE_INNER_CASTLE_MAP = 64;

		// 侏儒城
		private const int DOWA_TOWER_X = 32828;

		private const int DOWA_TOWER_Y = 32818;

		private const short DOWA_TOWER_MAP = 66;

		private const int DOWA_X1 = 32755;

		private const int DOWA_X2 = 32870;

		private const int DOWA_Y1 = 32790;

		private const int DOWA_Y2 = 32920;

		private const short DOWA_MAP = 66;

		// 亞丁城
		private const int ADEN_TOWER_X = 34090;

		private const int ADEN_TOWER_Y = 33260;

		private const short ADEN_TOWER_MAP = 4;

		private const int ADEN_X1 = 34007;

		private const int ADEN_X2 = 34162;

		private const int ADEN_Y1 = 33172;

		private const int ADEN_Y2 = 33332;

		private const short ADEN_MAP = 4;

		private const short ADEN_INNER_CASTLE_MAP = 300;

		private const int ADEN_SUB_TOWER1_X = 34057; // 青

		private const int ADEN_SUB_TOWER1_Y = 33291;

		private const int ADEN_SUB_TOWER2_X = 34123; // 赤

		private const int ADEN_SUB_TOWER2_Y = 33291;

		private const int ADEN_SUB_TOWER3_X = 34057; // 緑

		private const int ADEN_SUB_TOWER3_Y = 33230;

		private const int ADEN_SUB_TOWER4_X = 34123; // 白

		private const int ADEN_SUB_TOWER4_Y = 33230;

		// 狄亞得要塞
		private const int DIAD_TOWER_X = 33033;

		private const int DIAD_TOWER_Y = 32895;

		private const short DIAD_TOWER_MAP = 320;

		private const int DIAD_X1 = 32888;

		private const int DIAD_X2 = 33070;

		private const int DIAD_Y1 = 32839;

		private const int DIAD_Y2 = 32953;

		private const short DIAD_MAP = 320;

		private const short DIAD_INNER_CASTLE_MAP = 330;

		private static readonly IDictionary<int, L1Location> _towers = Maps.newMap();

		static L1CastleLocation()
		{
			_towers[KENT_CASTLE_ID] = new L1Location(KENT_TOWER_X, KENT_TOWER_Y, KENT_TOWER_MAP);
			_towers[OT_CASTLE_ID] = new L1Location(OT_TOWER_X, OT_TOWER_Y, OT_TOWER_MAP);
			_towers[WW_CASTLE_ID] = new L1Location(WW_TOWER_X, WW_TOWER_Y, WW_TOWER_MAP);
			_towers[GIRAN_CASTLE_ID] = new L1Location(GIRAN_TOWER_X, GIRAN_TOWER_Y, GIRAN_TOWER_MAP);
			_towers[HEINE_CASTLE_ID] = new L1Location(HEINE_TOWER_X, HEINE_TOWER_Y, HEINE_TOWER_MAP);
			_towers[DOWA_CASTLE_ID] = new L1Location(DOWA_TOWER_X, DOWA_TOWER_Y, DOWA_TOWER_MAP);
			_towers[ADEN_CASTLE_ID] = new L1Location(ADEN_TOWER_X, ADEN_TOWER_Y, ADEN_TOWER_MAP);
			_towers[DIAD_CASTLE_ID] = new L1Location(DIAD_TOWER_X, DIAD_TOWER_Y, DIAD_TOWER_MAP);
			_areas[KENT_CASTLE_ID] = new L1MapArea(KENT_X1, KENT_Y1, KENT_X2, KENT_Y2, KENT_MAP);
			_areas[OT_CASTLE_ID] = new L1MapArea(OT_X1, OT_Y1, OT_X2, OT_Y2, OT_MAP);
			_areas[WW_CASTLE_ID] = new L1MapArea(WW_X1, WW_Y1, WW_X2, WW_Y2, WW_MAP);
			_areas[GIRAN_CASTLE_ID] = new L1MapArea(GIRAN_X1, GIRAN_Y1, GIRAN_X2, GIRAN_Y2, GIRAN_MAP);
			_areas[HEINE_CASTLE_ID] = new L1MapArea(HEINE_X1, HEINE_Y1, HEINE_X2, HEINE_Y2, HEINE_MAP);
			_areas[DOWA_CASTLE_ID] = new L1MapArea(DOWA_X1, DOWA_Y1, DOWA_X2, DOWA_Y2, DOWA_MAP);
			_areas[ADEN_CASTLE_ID] = new L1MapArea(ADEN_X1, ADEN_Y1, ADEN_X2, ADEN_Y2, ADEN_MAP);
			_areas[DIAD_CASTLE_ID] = new L1MapArea(DIAD_X1, DIAD_Y1, DIAD_X2, DIAD_Y2, DIAD_MAP);
			_innerTowerMaps[KENT_CASTLE_ID] = (int) KENT_INNER_CASTLE_MAP;
			_innerTowerMaps[WW_CASTLE_ID] = (int) WW_INNER_CASTLE_MAP;
			_innerTowerMaps[GIRAN_CASTLE_ID] = (int) GIRAN_INNER_CASTLE_MAP;
			_innerTowerMaps[HEINE_CASTLE_ID] = (int) HEINE_INNER_CASTLE_MAP;
			_innerTowerMaps[ADEN_CASTLE_ID] = (int) ADEN_INNER_CASTLE_MAP;
			_innerTowerMaps[DIAD_CASTLE_ID] = (int) DIAD_INNER_CASTLE_MAP;
			_subTowers[1] = new L1Location(ADEN_SUB_TOWER1_X, ADEN_SUB_TOWER1_Y, ADEN_TOWER_MAP);
			_subTowers[2] = new L1Location(ADEN_SUB_TOWER2_X, ADEN_SUB_TOWER2_Y, ADEN_TOWER_MAP);
			_subTowers[3] = new L1Location(ADEN_SUB_TOWER3_X, ADEN_SUB_TOWER3_Y, ADEN_TOWER_MAP);
			_subTowers[4] = new L1Location(ADEN_SUB_TOWER4_X, ADEN_SUB_TOWER4_Y, ADEN_TOWER_MAP);
		}

		private static readonly IDictionary<int, L1MapArea> _areas = Maps.newMap();


		private static readonly IDictionary<int, int> _innerTowerMaps = Maps.newMap();


		private static readonly IDictionary<int, L1Location> _subTowers = Maps.newMap();


		private L1CastleLocation()
		{
		}

		public static int getCastleId(L1Location loc)
		{
			foreach (KeyValuePair<int, L1Location> entry in _towers.SetOfKeyValuePairs())
			{
				if (entry.Value.Equals(loc))
				{
					return entry.Key;
				}
			}
			return 0;
		}

		/// <summary>
		/// ガーディアンタワー、クラウンの座標からcastle_idを返す
		/// </summary>
		public static int getCastleId(int locx, int locy, short mapid)
		{
			return getCastleId(new L1Location(locx, locy, mapid));
		}

		public static int getCastleIdByArea(L1Location loc)
		{
			foreach (KeyValuePair<int, L1MapArea> entry in _areas.SetOfKeyValuePairs())
			{
				if (entry.Value.contains(loc))
				{
					return entry.Key;
				}
			}
			foreach (KeyValuePair<int, int> entry in _innerTowerMaps.SetOfKeyValuePairs())
			{
				if (entry.Value == loc.MapId)
				{
					return entry.Key;
				}
			}
			return 0;
		}

		/// <summary>
		/// 戦争エリア（旗内）の座標からcastle_idを返す
		/// </summary>
		public static int getCastleIdByArea(L1Character cha)
		{
			return getCastleIdByArea(cha.Location);
		}

		public static bool checkInWarArea(int castleId, L1Location loc)
		{
			return castleId == getCastleIdByArea(loc);
		}

		/// <summary>
		/// 指定した城の戦争エリア（旗内）にいるか返す
		/// </summary>
		public static bool checkInWarArea(int castleId, L1Character cha)
		{
			return checkInWarArea(castleId, cha.Location);
		}

		public static bool checkInAllWarArea(L1Location loc)
		{
			return 0 != getCastleIdByArea(loc);
		}

		/// <summary>
		/// いずれかの戦争エリア（旗内）かどうかチェック
		/// </summary>
		public static bool checkInAllWarArea(int locx, int locy, short mapid)
		{
			return checkInAllWarArea(new L1Location(locx, locy, mapid));
		}

		/// <summary>
		/// castleIdからガーディアンタワーの座標を返す
		/// </summary>
		public static int[] getTowerLoc(int castleId)
		{
			int[] result = new int[3];
			L1Location loc = _towers[castleId];
			if (loc != null)
			{
				result[0] = loc.X;
				result[1] = loc.Y;
				result[2] = loc.MapId;
			}
			return result;
		}

		/// <summary>
		/// castleIdから戦争エリア（旗内）の座標を返す
		/// </summary>
		public static int[] getWarArea(int castleId)
		{
			int[] loc = new int[5];
			if (castleId == KENT_CASTLE_ID)
			{ // ケント城
				loc[0] = KENT_X1;
				loc[1] = KENT_X2;
				loc[2] = KENT_Y1;
				loc[3] = KENT_Y2;
				loc[4] = KENT_MAP;
			}
			else if (castleId == OT_CASTLE_ID)
			{ // オークの森
				loc[0] = OT_X1;
				loc[1] = OT_X2;
				loc[2] = OT_Y1;
				loc[3] = OT_Y2;
				loc[4] = OT_MAP;
			}
			else if (castleId == WW_CASTLE_ID)
			{ // ウィンダウッド城
				loc[0] = WW_X1;
				loc[1] = WW_X2;
				loc[2] = WW_Y1;
				loc[3] = WW_Y2;
				loc[4] = WW_MAP;
			}
			else if (castleId == GIRAN_CASTLE_ID)
			{ // ギラン城
				loc[0] = GIRAN_X1;
				loc[1] = GIRAN_X2;
				loc[2] = GIRAN_Y1;
				loc[3] = GIRAN_Y2;
				loc[4] = GIRAN_MAP;
			}
			else if (castleId == HEINE_CASTLE_ID)
			{ // ハイネ城
				loc[0] = HEINE_X1;
				loc[1] = HEINE_X2;
				loc[2] = HEINE_Y1;
				loc[3] = HEINE_Y2;
				loc[4] = HEINE_MAP;
			}
			else if (castleId == DOWA_CASTLE_ID)
			{ // ドワーフ城
				loc[0] = DOWA_X1;
				loc[1] = DOWA_X2;
				loc[2] = DOWA_Y1;
				loc[3] = DOWA_Y2;
				loc[4] = DOWA_MAP;
			}
			else if (castleId == ADEN_CASTLE_ID)
			{ // アデン城
				loc[0] = ADEN_X1;
				loc[1] = ADEN_X2;
				loc[2] = ADEN_Y1;
				loc[3] = ADEN_Y2;
				loc[4] = ADEN_MAP;
			}
			else if (castleId == DIAD_CASTLE_ID)
			{ // ディアド要塞
				loc[0] = DIAD_X1;
				loc[1] = DIAD_X2;
				loc[2] = DIAD_Y1;
				loc[3] = DIAD_Y2;
				loc[4] = DIAD_MAP;
			}
			return loc;
		}

		public static int[] getCastleLoc(int castle_id)
		{ // castle_idから城内の座標を返す
			int[] loc = new int[3];
			if (castle_id == KENT_CASTLE_ID)
			{ // ケント城
				loc[0] = 32731;
				loc[1] = 32810;
				loc[2] = 15;
			}
			else if (castle_id == OT_CASTLE_ID)
			{ // オークの森
				loc[0] = 32800;
				loc[1] = 32277;
				loc[2] = 4;
			}
			else if (castle_id == WW_CASTLE_ID)
			{ // ウィンダウッド城
				loc[0] = 32730;
				loc[1] = 32814;
				loc[2] = 29;
			}
			else if (castle_id == GIRAN_CASTLE_ID)
			{ // ギラン城
				loc[0] = 32724;
				loc[1] = 32827;
				loc[2] = 52;
			}
			else if (castle_id == HEINE_CASTLE_ID)
			{ // ハイネ城
				loc[0] = 32568;
				loc[1] = 32855;
				loc[2] = 64;
			}
			else if (castle_id == DOWA_CASTLE_ID)
			{ // ドワーフ城
				loc[0] = 32853;
				loc[1] = 32810;
				loc[2] = 66;
			}
			else if (castle_id == ADEN_CASTLE_ID)
			{ // アデン城
				loc[0] = 32892;
				loc[1] = 32572;
				loc[2] = 300;
			}
			else if (castle_id == DIAD_CASTLE_ID)
			{ // ディアド要塞
				loc[0] = 32733;
				loc[1] = 32985;
				loc[2] = 330;
			}
			return loc;
		}

		/*
		 * castle_idから帰還先の座標をランダムに返す
		 */
		public static int[] getGetBackLoc(int castle_id)
		{
			int[] loc;
			if (castle_id == KENT_CASTLE_ID)
			{ // ケント城
				loc = L1TownLocation.getGetBackLoc(L1TownLocation.TOWNID_KENT);
			}
			else if (castle_id == OT_CASTLE_ID)
			{ // オークの森
				loc = L1TownLocation.getGetBackLoc(L1TownLocation.TOWNID_ORCISH_FOREST);
			}
			else if (castle_id == WW_CASTLE_ID)
			{ // ウィンダウッド城
				loc = L1TownLocation.getGetBackLoc(L1TownLocation.TOWNID_WINDAWOOD);
			}
			else if (castle_id == GIRAN_CASTLE_ID)
			{ // ギラン城
				loc = L1TownLocation.getGetBackLoc(L1TownLocation.TOWNID_GIRAN);
			}
			else if (castle_id == HEINE_CASTLE_ID)
			{ // ハイネ城
				loc = L1TownLocation.getGetBackLoc(L1TownLocation.TOWNID_HEINE);
			}
			else if (castle_id == DOWA_CASTLE_ID)
			{ // ドワーフ城
				loc = L1TownLocation.getGetBackLoc(L1TownLocation.TOWNID_WERLDAN);
			}
			else if (castle_id == ADEN_CASTLE_ID)
			{ // アデン城
				loc = L1TownLocation.getGetBackLoc(L1TownLocation.TOWNID_ADEN);
			}
			else if (castle_id == DIAD_CASTLE_ID)
			{ // ディアド要塞
				// ディアド要塞の帰還先は未調査
				int rnd = RandomHelper.Next(3);
				loc = new int[3];
				if (rnd == 0)
				{
					loc[0] = 32792;
					loc[1] = 32807;
					loc[2] = 310;
				}
				else if (rnd == 1)
				{
					loc[0] = 32816;
					loc[1] = 32820;
					loc[2] = 310;
				}
				else if (rnd == 2)
				{
					loc[0] = 32823;
					loc[1] = 32797;
					loc[2] = 310;
				}
			}
			else
			{ // 存在しないcastle_idが指定された場合はSKT
				loc = L1TownLocation.getGetBackLoc(L1TownLocation.TOWNID_SILVER_KNIGHT_TOWN);
			}
			return loc;
		}

		/// <summary>
		/// npcidからcastle_idを返す
		/// </summary>
		/// <param name="npcid">
		/// @return </param>
		public static int getCastleIdByNpcid(int npcid)
		{
			// アデン城：アデン王国全域
			// ケント城：ケント、グルーディン
			// ウィンダウッド城：ウッドベック、オアシス、シルバーナイトタウン
			// ギラン城：ギラン、話せる島
			// ハイネ城：ハイネ
			// ドワーフ城：ウェルダン、象牙の塔、象牙の塔の村
			// オーク砦：火田村
			// ディアド要塞：戦争税の一部

			int castle_id = 0;

			int town_id = L1TownLocation.getTownIdByNpcid(npcid);

			switch (town_id)
			{
				case L1TownLocation.TOWNID_KENT:
				case L1TownLocation.TOWNID_GLUDIO:
					castle_id = KENT_CASTLE_ID; // ケント城
					break;

				case L1TownLocation.TOWNID_ORCISH_FOREST:
					castle_id = OT_CASTLE_ID; // オークの森
					break;

				case L1TownLocation.TOWNID_SILVER_KNIGHT_TOWN:
				case L1TownLocation.TOWNID_WINDAWOOD:
					castle_id = WW_CASTLE_ID; // ウィンダウッド城
					break;

				case L1TownLocation.TOWNID_TALKING_ISLAND:
				case L1TownLocation.TOWNID_GIRAN:
					castle_id = GIRAN_CASTLE_ID; // ギラン城
					break;

				case L1TownLocation.TOWNID_HEINE:
					castle_id = HEINE_CASTLE_ID; // ハイネ城
					break;

				case L1TownLocation.TOWNID_WERLDAN:
				case L1TownLocation.TOWNID_OREN:
					castle_id = DOWA_CASTLE_ID; // ドワーフ城
					break;

				case L1TownLocation.TOWNID_ADEN:
					castle_id = ADEN_CASTLE_ID; // アデン城
					break;

				case L1TownLocation.TOWNID_OUM_DUNGEON:
					castle_id = DIAD_CASTLE_ID; // ディアド要塞
					break;

				default:
					break;
			}
			return castle_id;
		}

		// このメソッドはアデン時間で一日毎に更新される税率を返却する。(リアルタイムな税率ではない)
		public static int getCastleTaxRateByNpcId(int npcId)
		{
			int castleId = getCastleIdByNpcid(npcId);
			if (castleId != 0)
			{
				return _castleTaxRate[castleId];
			}
			return 0;
		}

		// 各城の税率を保管しておくHashMap(ショップ用)
		private static IDictionary<int, int> _castleTaxRate = Maps.newMap();

		private static L1CastleTaxRateListener _listener;

		// GameServer#initialize,L1CastleTaxRateListener#onDayChangedだけに呼び出される予定。
		public static void setCastleTaxRate()
		{
			foreach (L1Castle castle in CastleTable.Instance.CastleTableList)
			{
				_castleTaxRate[castle.Id] = castle.TaxRate;
			}
			if (_listener == null)
			{
				_listener = new L1CastleTaxRateListener();
				L1GameTimeClock.Instance.AddListener(_listener);
			}
		}

		private class L1CastleTaxRateListener : IL1GameTimeListener
		{
			public override void OnDayChanged(L1GameTime time)
			{
				L1CastleLocation.setCastleTaxRate();
			}
		}

		/// <summary>
		/// サブタワー番号からサブタワーの座標を返す
		/// </summary>
		public static int[] getSubTowerLoc(int no)
		{
			int[] result = new int[3];
			L1Location loc = _subTowers[no];
			if (loc != null)
			{
				result[0] = loc.X;
				result[1] = loc.Y;
				result[2] = loc.MapId;
			}
			return result;
		}

	}

}