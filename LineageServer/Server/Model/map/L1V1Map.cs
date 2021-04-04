using LineageServer.Interfaces;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Types;
using System;

namespace LineageServer.Server.Model.Map
{
	class L1V1Map : L1Map
	{
		private int _mapId;

		private int _worldTopLeftX;

		private int _worldTopLeftY;

		private int _worldBottomRightX;

		private int _worldBottomRightY;

		private byte[][] _map;

		private bool _isUnderwater;

		private bool _isMarkable;

		private bool _isTeleportable;

		private bool _isEscapable;

		private bool _isUseResurrection;

		private bool _isUsePainwand;

		private bool _isEnabledDeathPenalty;

		private bool _isTakePets;

		private bool _isRecallPets;

		private bool _isUsableItem;

		private bool _isUsableSkill;

		/*
		 * マップ情報を1面で保持するために仕方なくビットフラグ。 可読性が大きく下がるので良い子は真似しない。
		 */
		/// <summary>
		/// Mobなどの通行不可能になるオブジェクトがタイル上に存在するかを示すビットフラグ
		/// </summary>
		private static readonly byte BITFLAG_IS_IMPASSABLE = 128; // 1000 0000

		protected internal L1V1Map()
		{

		}

		public L1V1Map(int mapId, byte[][] map, int worldTopLeftX, int worldTopLeftY, bool underwater, bool markable, bool teleportable, bool escapable, bool useResurrection, bool usePainwand, bool enabledDeathPenalty, bool takePets, bool recallPets, bool usableItem, bool usableSkill)
		{
			_mapId = mapId;
			_map = map;
			_worldTopLeftX = worldTopLeftX;
			_worldTopLeftY = worldTopLeftY;

			_worldBottomRightX = worldTopLeftX + map.Length - 1;
			_worldBottomRightY = worldTopLeftY + map[0].Length - 1;

			_isUnderwater = underwater;
			_isMarkable = markable;
			_isTeleportable = teleportable;
			_isEscapable = escapable;
			_isUseResurrection = useResurrection;
			_isUsePainwand = usePainwand;
			_isEnabledDeathPenalty = enabledDeathPenalty;
			_isTakePets = takePets;
			_isRecallPets = recallPets;
			_isUsableItem = usableItem;
			_isUsableSkill = usableSkill;
		}

		public L1V1Map(L1V1Map map)
		{
			_mapId = map._mapId;

			// _mapをコピー
			_map = new byte[map._map.Length][];
			for (int i = 0; i < map._map.Length; i++)
			{
				_map[i] = new byte[map._map[i].Length];
				Buffer.BlockCopy(map._map[i], 0, _map[i], 0, map._map[i].Length);
			}

			_worldTopLeftX = map._worldTopLeftX;
			_worldTopLeftY = map._worldTopLeftY;
			_worldBottomRightX = map._worldBottomRightX;
			_worldBottomRightY = map._worldBottomRightY;

		}

		private int accessTile(int x, int y)
		{
			if (!isInMap(x, y))
			{ // XXX とりあえずチェックする。これは良くない。
				return 0;
			}

			return _map[x - _worldTopLeftX][y - _worldTopLeftY];
		}

		private int accessOriginalTile(int x, int y)
		{
			return accessTile(x, y) & ( ~BITFLAG_IS_IMPASSABLE );
		}

		private void setTile(int x, int y, int tile)
		{
			if (!isInMap(x, y))
			{ // XXX とりあえずチェックする。これは良くない。
				return;
			}
			_map[x - _worldTopLeftX][y - _worldTopLeftY] = (byte)tile;
		}

		/*
		 * ものすごく良くない気がする
		 */
		public virtual byte[][] RawTiles
		{
			get
			{
				return _map;
			}
		}

		public override int Id
		{
			get
			{
				return _mapId;
			}
		}

		public override int X
		{
			get
			{
				return _worldTopLeftX;
			}
		}

		public override int Y
		{
			get
			{
				return _worldTopLeftY;
			}
		}

		public override int Width
		{
			get
			{
				return _worldBottomRightX - _worldTopLeftX + 1;
			}
		}

		public override int Height
		{
			get
			{
				// TODO Auto-generated method stub
				return _worldBottomRightY - _worldTopLeftY + 1;
			}
		}

		public override int getTile(int x, int y)
		{
			short tile = _map[x - _worldTopLeftX][y - _worldTopLeftY];
			if (0 != ( tile & BITFLAG_IS_IMPASSABLE ))
			{
				return 300;
			}
			return accessOriginalTile(x, y);
		}

		public override int getOriginalTile(int x, int y)
		{
			return accessOriginalTile(x, y);
		}

		public override bool isInMap(Point pt)
		{
			return isInMap(pt.X, pt.Y);
		}

		public override bool isInMap(int x, int y)
		{
			// フィールドの茶色エリアの判定
			if (( _mapId == 4 ) && ( ( x < 32520 ) || ( y < 32070 ) || ( ( y < 32190 ) && ( x < 33950 ) ) ))
			{
				return false;
			}
			return ( ( _worldTopLeftX <= x ) && ( x <= _worldBottomRightX ) && ( _worldTopLeftY <= y ) && ( y <= _worldBottomRightY ) );
		}

		public override bool isPassable(Point pt)
		{
			return isPassable(pt.X, pt.Y);
		}

		public override bool isPassable(int x, int y)
		{
			return isPassable(x, y - 1, 4) || isPassable(x + 1, y, 6) || isPassable(x, y + 1, 0) || isPassable(x - 1, y, 2);
		}

		public override bool isPassable(Point pt, int heading)
		{
			return isPassable(pt.X, pt.Y, heading);
		}

		public override bool isPassable(int x, int y, int heading)
		{
			// 現在のタイル
			int tile1 = accessTile(x, y);
			// 移動予定のタイル
			int tile2;

			if (heading == 0)
			{
				tile2 = accessTile(x, y - 1);
			}
			else if (heading == 1)
			{
				tile2 = accessTile(x + 1, y - 1);
			}
			else if (heading == 2)
			{
				tile2 = accessTile(x + 1, y);
			}
			else if (heading == 3)
			{
				tile2 = accessTile(x + 1, y + 1);
			}
			else if (heading == 4)
			{
				tile2 = accessTile(x, y + 1);
			}
			else if (heading == 5)
			{
				tile2 = accessTile(x - 1, y + 1);
			}
			else if (heading == 6)
			{
				tile2 = accessTile(x - 1, y);
			}
			else if (heading == 7)
			{
				tile2 = accessTile(x - 1, y - 1);
			}
			else
			{
				return false;
			}

			if (( tile2 & BITFLAG_IS_IMPASSABLE ) == BITFLAG_IS_IMPASSABLE)
			{
				return false;
			}

			if (!( ( tile2 & 0x02 ) == 0x02 || ( tile2 & 0x01 ) == 0x01 ))
			{
				return false;
			}

			if (heading == 0)
			{
				return ( tile1 & 0x02 ) == 0x02;
			}
			else if (heading == 1)
			{
				int tile3 = accessTile(x, y - 1);
				int tile4 = accessTile(x + 1, y);
				return ( ( tile3 & 0x01 ) == 0x01 ) || ( ( tile4 & 0x02 ) == 0x02 );
			}
			else if (heading == 2)
			{
				return ( tile1 & 0x01 ) == 0x01;
			}
			else if (heading == 3)
			{
				int tile3 = accessTile(x, y + 1);
				return ( tile3 & 0x01 ) == 0x01;
			}
			else if (heading == 4)
			{
				return ( tile2 & 0x02 ) == 0x02;
			}
			else if (heading == 5)
			{
				return ( ( tile2 & 0x01 ) == 0x01 ) || ( ( tile2 & 0x02 ) == 0x02 );
			}
			else if (heading == 6)
			{
				return ( tile2 & 0x01 ) == 0x01;
			}
			else if (heading == 7)
			{
				int tile3 = accessTile(x - 1, y);
				return ( tile3 & 0x02 ) == 0x02;
			}

			return false;
		}

		public override void setPassable(Point pt, bool isPassable)
		{
			setPassable(pt.X, pt.Y, isPassable);
		}

		public override void setPassable(int x, int y, bool isPassable)
		{
			if (isPassable)
			{
				setTile(x, y, (short)( accessTile(x, y) & ( ~BITFLAG_IS_IMPASSABLE ) ));
			}
			else
			{
				setTile(x, y, (short)( accessTile(x, y) | BITFLAG_IS_IMPASSABLE ));
			}
		}

		public override bool isSafetyZone(Point pt)
		{
			return isSafetyZone(pt.X, pt.Y);
		}

		public override bool isSafetyZone(int x, int y)
		{
			int tile = accessOriginalTile(x, y);

			return ( tile & 0x30 ) == 0x10;
		}

		public override bool isCombatZone(Point pt)
		{
			return isCombatZone(pt.X, pt.Y);
		}

		public override bool isCombatZone(int x, int y)
		{
			int tile = accessOriginalTile(x, y);

			return ( tile & 0x30 ) == 0x20;
		}

		public override bool isNormalZone(Point pt)
		{
			return isNormalZone(pt.X, pt.Y);
		}

		public override bool isNormalZone(int x, int y)
		{
			int tile = accessOriginalTile(x, y);
			return ( tile & 0x30 ) == 0x00;
		}

		public override bool isArrowPassable(Point pt)
		{
			return isArrowPassable(pt.X, pt.Y);
		}

		public override bool isArrowPassable(int x, int y)
		{
			return ( accessOriginalTile(x, y) & 0x0e ) != 0;
		}

		public override bool isArrowPassable(Point pt, int heading)
		{
			return isArrowPassable(pt.X, pt.Y, heading);
		}

		public override bool isArrowPassable(int x, int y, int heading)
		{
			// 現在のタイル
			int tile1 = accessTile(x, y);
			// 移動予定のタイル
			int tile2;
			// 移動予定の座標
			int newX;
			int newY;

			if (heading == 0)
			{
				tile2 = accessTile(x, y - 1);
				newX = x;
				newY = y - 1;
			}
			else if (heading == 1)
			{
				tile2 = accessTile(x + 1, y - 1);
				newX = x + 1;
				newY = y - 1;
			}
			else if (heading == 2)
			{
				tile2 = accessTile(x + 1, y);
				newX = x + 1;
				newY = y;
			}
			else if (heading == 3)
			{
				tile2 = accessTile(x + 1, y + 1);
				newX = x + 1;
				newY = y + 1;
			}
			else if (heading == 4)
			{
				tile2 = accessTile(x, y + 1);
				newX = x;
				newY = y + 1;
			}
			else if (heading == 5)
			{
				tile2 = accessTile(x - 1, y + 1);
				newX = x - 1;
				newY = y + 1;
			}
			else if (heading == 6)
			{
				tile2 = accessTile(x - 1, y);
				newX = x - 1;
				newY = y;
			}
			else if (heading == 7)
			{
				tile2 = accessTile(x - 1, y - 1);
				newX = x - 1;
				newY = y - 1;
			}
			else
			{
				return false;
			}

			if (isExistDoor(newX, newY))
			{
				return false;
			}

			// if (Config.ARROW_PASS_FLOWER_BED) {
			// if (tile2 == 0x00 || (tile2 & 0x10) == 0x10) { // 花壇
			// if (tile2 == 0x00) { // 花壇
			// return true;
			// }
			// }

			if (heading == 0)
			{
				return ( tile1 & 0x08 ) == 0x08;
			}
			else if (heading == 1)
			{
				int tile3 = accessTile(x, y - 1);
				int tile4 = accessTile(x + 1, y);
				return ( ( tile3 & 0x04 ) == 0x04 ) || ( ( tile4 & 0x08 ) == 0x08 );
			}
			else if (heading == 2)
			{
				return ( tile1 & 0x04 ) == 0x04;
			}
			else if (heading == 3)
			{
				int tile3 = accessTile(x, y + 1);
				return ( tile3 & 0x04 ) == 0x04;
			}
			else if (heading == 4)
			{
				return ( tile2 & 0x08 ) == 0x08;
			}
			else if (heading == 5)
			{
				return ( ( tile2 & 0x04 ) == 0x04 ) || ( ( tile2 & 0x08 ) == 0x08 );
			}
			else if (heading == 6)
			{
				return ( tile2 & 0x04 ) == 0x04;
			}
			else if (heading == 7)
			{
				int tile3 = accessTile(x - 1, y);
				return ( tile3 & 0x08 ) == 0x08;
			}

			return false;
		}

		public override bool Underwater
		{
			get
			{
				return _isUnderwater;
			}
		}

		public override bool Markable
		{
			get
			{
				return _isMarkable;
			}
		}

		public override bool Teleportable
		{
			get
			{
				return _isTeleportable;
			}
		}

		public override bool Escapable
		{
			get
			{
				return _isEscapable;
			}
		}

		public override bool UseResurrection
		{
			get
			{
				return _isUseResurrection;
			}
		}

		public override bool UsePainwand
		{
			get
			{
				return _isUsePainwand;
			}
		}

		public override bool EnabledDeathPenalty
		{
			get
			{
				return _isEnabledDeathPenalty;
			}
		}

		public override bool TakePets
		{
			get
			{
				return _isTakePets;
			}
		}

		public override bool RecallPets
		{
			get
			{
				return _isRecallPets;
			}
		}

		public override bool UsableItem
		{
			get
			{
				return _isUsableItem;
			}
		}

		public override bool UsableSkill
		{
			get
			{
				return _isUsableSkill;
			}
		}

		public override bool isFishingZone(int x, int y)
		{
			return accessOriginalTile(x, y) == 28; // 3.3C 釣魚池可釣魚區域
		}

		public override bool isExistDoor(int x, int y)
		{
			foreach (L1DoorInstance door in Container.Instance.Resolve<IDoorController>().DoorList)
			{
				if (_mapId != door.MapId)
				{
					continue;
				}
				if (door.OpenStatus == ActionCodes.ACTION_Open)
				{
					continue;
				}
				if (door.Dead)
				{
					continue;
				}
				int leftEdgeLocation = door.LeftEdgeLocation;
				int rightEdgeLocation = door.RightEdgeLocation;
				int size = rightEdgeLocation - leftEdgeLocation;
				if (size == 0)
				{ // 1マス分の幅のドア
					if (( x == door.X ) && ( y == door.Y ))
					{
						return true;
					}
				}
				else
				{ // 2マス分以上の幅があるドア
					if (door.Direction == 0)
					{ // ／向き
						for (int doorX = leftEdgeLocation; doorX <= rightEdgeLocation; doorX++)
						{
							if (( x == doorX ) && ( y == door.Y ))
							{
								return true;
							}
						}
					}
					else
					{ // ＼向き
						for (int doorY = leftEdgeLocation; doorY <= rightEdgeLocation; doorY++)
						{
							if (( x == door.X ) && ( y == doorY ))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public override string toString(Point pt)
		{
			int tile = getOriginalTile(pt.X, pt.Y);

			return ( tile & 0xFF ) + " " + ( ( tile >> 8 ) & 0xFF );
		}
	}
}