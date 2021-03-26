//何苦用1維陣列各種Offset

/*
using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Types;

namespace LineageServer.Server.Server.Model.map
{
	class L1V2Map : L1Map
	{
		private readonly int _id;
		private readonly int _xLoc;
		private readonly int _yLoc;
		private readonly int _width;
		private readonly int _height;
		private readonly sbyte[] _map;
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


		/// <summary>
		/// Mobなどの通行不可能になるオブジェクトがタイル上に存在するかを示すビットフラグ
		/// </summary>
		private static readonly sbyte BITFLAG_IS_IMPASSABLE = unchecked((sbyte)128); // 1000 0000

		private int offset(int x, int y)
		{
			return ( ( y - _yLoc ) * _width * 2 ) + ( ( x - _xLoc ) * 2 );
		}

		private int accessOriginalTile(int x, int y)
		{
			return _map[offset(x, y)] & ( ~BITFLAG_IS_IMPASSABLE );
		}

		public L1V2Map(int id, sbyte[] map, int xLoc, int yLoc, int width, int height, bool underwater, bool markable, bool teleportable, bool escapable, bool useResurrection, bool usePainwand, bool enabledDeathPenalty, bool takePets, bool recallPets, bool usableItem, bool usableSkill)
		{
			_id = id;
			_map = map;
			_xLoc = xLoc;
			_yLoc = yLoc;
			_width = width;
			_height = height;

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

		public override int Height
		{
			get
			{
				return _height;
			}
		}

		public override int Id
		{
			get
			{
				return _id;
			}
		}

		public override int getOriginalTile(int x, int y)
		{
			int lo = _map[offset(x, y)];
			int hi = _map[offset(x, y) + 1];
			return ( lo | ( ( hi << 8 ) & 0xFF00 ) );
		}

		public override int getTile(int x, int y)
		{
			return _map[offset(x, y)];
		}

		public override int Width
		{
			get
			{
				return _width;
			}
		}

		public override int X
		{
			get
			{
				return _xLoc;
			}
		}

		public override int Y
		{
			get
			{
				return _yLoc;
			}
		}

		public override bool isArrowPassable(Point pt)
		{
			return isArrowPassable(pt.X, pt.Y);
		}

		public override bool isArrowPassable(int x, int y)
		{
			return ( accessOriginalTile(x, y) != 1 );
		}

		public override bool isArrowPassable(Point pt, int heading)
		{
			return isArrowPassable(pt.X, pt.Y, heading);
		}

		public override bool isArrowPassable(int x, int y, int heading)
		{
			int tile;
			// 移動予定の座標
			int newX;
			int newY;

			if (heading == 0)
			{
				tile = accessOriginalTile(x, y - 1);
				newX = x;
				newY = y - 1;
			}
			else if (heading == 1)
			{
				tile = accessOriginalTile(x + 1, y - 1);
				newX = x + 1;
				newY = y - 1;
			}
			else if (heading == 2)
			{
				tile = accessOriginalTile(x + 1, y);
				newX = x + 1;
				newY = y;
			}
			else if (heading == 3)
			{
				tile = accessOriginalTile(x + 1, y + 1);
				newX = x + 1;
				newY = y + 1;
			}
			else if (heading == 4)
			{
				tile = accessOriginalTile(x, y + 1);
				newX = x;
				newY = y + 1;
			}
			else if (heading == 5)
			{
				tile = accessOriginalTile(x - 1, y + 1);
				newX = x - 1;
				newY = y + 1;
			}
			else if (heading == 6)
			{
				tile = accessOriginalTile(x - 1, y);
				newX = x - 1;
				newY = y;
			}
			else if (heading == 7)
			{
				tile = accessOriginalTile(x - 1, y - 1);
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

			return ( tile != 1 );
		}

		public override bool isCombatZone(Point pt)
		{
			return isCombatZone(pt.X, pt.Y);
		}

		public override bool isCombatZone(int x, int y)
		{
			return ( accessOriginalTile(x, y) == 8 );
		}

		public override bool isInMap(Point pt)
		{
			return isInMap(pt.X, pt.Y);
		}

		public override bool isInMap(int x, int y)
		{
			return ( _xLoc <= x && x < _xLoc + _width && _yLoc <= y && y < _yLoc + _height );
		}

		public override bool isNormalZone(Point pt)
		{
			return isNormalZone(pt.X, pt.Y);
		}

		public override bool isNormalZone(int x, int y)
		{
			return ( !isCombatZone(x, y) && !isSafetyZone(x, y) );
		}

		public override bool isPassable(Point pt)
		{
			return isPassable(pt.X, pt.Y);
		}

		public override bool isPassable(int x, int y)
		{
			int tile = accessOriginalTile(x, y);
			if (tile == 1 || tile == 9 || tile == 65 || tile == 69 || tile == 73)
			{
				return false;
			}
			if (0 != ( _map[offset(x, y)] & BITFLAG_IS_IMPASSABLE ))
			{
				return false;
			}
			return true;
		}

		public override bool isPassable(Point pt, int heading)
		{
			return isPassable(pt.X, pt.Y, heading);
		}

		public override bool isPassable(int x, int y, int heading)
		{
			int tile;
			if (heading == 0)
			{
				tile = accessOriginalTile(x, y - 1);
			}
			else if (heading == 1)
			{
				tile = accessOriginalTile(x + 1, y - 1);
			}
			else if (heading == 2)
			{
				tile = accessOriginalTile(x + 1, y);
			}
			else if (heading == 3)
			{
				tile = accessOriginalTile(x + 1, y + 1);
			}
			else if (heading == 4)
			{
				tile = accessOriginalTile(x, y + 1);
			}
			else if (heading == 5)
			{
				tile = accessOriginalTile(x - 1, y + 1);
			}
			else if (heading == 6)
			{
				tile = accessOriginalTile(x - 1, y);
			}
			else if (heading == 7)
			{
				tile = accessOriginalTile(x - 1, y - 1);
			}
			else
			{
				return false;
			}

			if (tile == 1 || tile == 9 || tile == 65 || tile == 69 || tile == 73)
			{
				return false;
			}
			if (0 != ( _map[offset(x, y)] & BITFLAG_IS_IMPASSABLE ))
			{
				return false;
			}
			return true;
		}

		public override bool isSafetyZone(Point pt)
		{
			return isSafetyZone(pt.X, pt.Y);
		}

		public override bool isSafetyZone(int x, int y)
		{
			return accessOriginalTile(x, y) == 4;
		}

		public override void setPassable(Point pt, bool isPassable)
		{
			setPassable(pt.X, pt.Y, isPassable);
		}

		public override void setPassable(int x, int y, bool isPassable)
		{
			if (isPassable)
			{
				_map[offset(x, y)] &= (sbyte)( ~BITFLAG_IS_IMPASSABLE );
			}
			else
			{
				_map[offset(x, y)] |= BITFLAG_IS_IMPASSABLE;
			}
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
			foreach (L1DoorInstance door in DoorTable.Instance.DoorList)
			{
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
					if (x == door.X && y == door.Y)
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
							if (x == doorX && y == door.Y)
							{
								return true;
							}
						}
					}
					else
					{ // ＼向き
						for (int doorY = leftEdgeLocation; doorY <= rightEdgeLocation; doorY++)
						{
							if (x == door.X && y == doorY)
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
*/