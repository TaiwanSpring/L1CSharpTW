using LineageServer.Server.Model.Map;
using LineageServer.Server.Types;
using LineageServer.Utils;
using System;

namespace LineageServer.Server.Model
{
	public class L1Location : Point
	{
		protected internal L1Map _map = L1Map.NullMap;

		public L1Location() : base()
		{
		}

		public L1Location(L1Location loc) : this(loc.X, loc.Y, loc._map)
		{
		}

		public L1Location(int x, int y, int mapId) : base(x, y)
		{
			setMap(mapId);
		}

		public L1Location(int x, int y, L1Map map) : base(x, y)
		{
			_map = map;
		}

		public L1Location(Point pt, int mapId) : base(pt)
		{
			setMap(mapId);
		}

		public L1Location(Point pt, L1Map map) : base(pt)
		{
			_map = map;
		}

		public virtual void set(L1Location loc)
		{
			_map = loc._map;
			X = loc.X;
			Y = loc.Y;
		}

		public virtual void set(int x, int y, int mapId)
		{
			set(x, y);
			setMap(mapId);
		}

		public virtual void set(int x, int y, L1Map map)
		{
			set(x, y);
			_map = map;
		}

		public virtual void set(Point pt, int mapId)
		{
			set(pt);
			setMap(mapId);
		}

		public virtual void set(Point pt, L1Map map)
		{
			set(pt);
			_map = map;
		}

		public virtual L1Map getMap()
		{
			return _map;
		}

		public virtual int MapId
		{
			get
			{
				return _map.Id;
			}
		}

		public virtual void setMap(L1Map map)
		{
			_map = map;
		}

		public virtual void setMap(int mapId)
		{
			_map = L1WorldMap.Instance.getMap((short)mapId);
		}

		public override bool Equals(object obj)
		{
			if (obj is L1Location other)
			{
				return this == other;
			}
			else
			{
				return false;
			}
		}

		public static bool operator ==(L1Location a, L1Location b)
		{
			if (a != null && b != null)
			{
				return ( a.getMap() == b.getMap() ) && ( a.X == b.X ) && ( a.Y == b.Y );
			}
			else
			{
				return false;
			}
		}

		public static bool operator !=(L1Location a, L1Location b)
		{
			if (a != null && b != null)
			{
				return !( a == b );
			}
			else
			{
				return true;
			}
		}
		public override int GetHashCode()
		{
			return 7 * _map.Id + base.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("({0:D}, {1:D}) on {2:D}", X, Y, _map.Id);
		}

		/// <summary>
		/// このLocationに対する、移動可能なランダム範囲のLocationを返す。
		/// ランダムテレポートの場合は、城エリア、アジト内のLocationは返却されない。
		/// </summary>
		/// <param name="max">
		///            ランダム範囲の最大値 </param>
		/// <param name="isRandomTeleport">
		///            ランダムテレポートか </param>
		/// <returns> 新しいLocation </returns>
		public virtual L1Location randomLocation(int max, bool isRandomTeleport)
		{
			return randomLocation(0, max, isRandomTeleport);
		}

		/// <summary>
		/// このLocationに対する、移動可能なランダム範囲のLocationを返す。
		/// ランダムテレポートの場合は、城エリア、アジト内のLocationは返却されない。
		/// </summary>
		/// <param name="min">
		///            ランダム範囲の最小値(0で自身の座標を含む) </param>
		/// <param name="max">
		///            ランダム範囲の最大値 </param>
		/// <param name="isRandomTeleport">
		///            ランダムテレポートか </param>
		/// <returns> 新しいLocation </returns>
		public virtual L1Location randomLocation(int min, int max, bool isRandomTeleport)
		{
			return L1Location.randomLocation(this, min, max, isRandomTeleport);
		}

		/// <summary>
		/// 引数のLocationに対して、移動可能なランダム範囲のLocationを返す。
		/// ランダムテレポートの場合は、城エリア、アジト内のLocationは返却されない。
		/// </summary>
		/// <param name="baseLocation">
		///            ランダム範囲の元になるLocation </param>
		/// <param name="min">
		///            ランダム範囲の最小値(0で自身の座標を含む) </param>
		/// <param name="max">
		///            ランダム範囲の最大値 </param>
		/// <param name="isRandomTeleport">
		///            ランダムテレポートか </param>
		/// <returns> 新しいLocation </returns>
		public static L1Location randomLocation(L1Location baseLocation, int min, int max, bool isRandomTeleport)
		{
			if (min > max)
			{
				throw new System.ArgumentException("min > maxとなる引数は無効");
			}
			if (max <= 0)
			{
				return new L1Location(baseLocation);
			}
			if (min < 0)
			{
				min = 0;
			}

			L1Location newLocation = new L1Location();
			int newX = 0;
			int newY = 0;
			int locX = baseLocation.X;
			int locY = baseLocation.Y;
			short mapId = (short)baseLocation.MapId;
			L1Map map = baseLocation.getMap();

			newLocation.setMap(map);

			int locX1 = locX - max;
			int locX2 = locX + max;
			int locY1 = locY - max;
			int locY2 = locY + max;

			// map範囲
			int mapX1 = map.X;
			int mapX2 = mapX1 + map.Width;
			int mapY1 = map.Y;
			int mapY2 = mapY1 + map.Height;

			// 最大でもマップの範囲内までに補正
			if (locX1 < mapX1)
			{
				locX1 = mapX1;
			}
			if (locX2 > mapX2)
			{
				locX2 = mapX2;
			}
			if (locY1 < mapY1)
			{
				locY1 = mapY1;
			}
			if (locY2 > mapY2)
			{
				locY2 = mapY2;
			}

			int diffX = locX2 - locX1; // x方向
			int diffY = locY2 - locY1; // y方向

			int trial = 0;
			// 試行回数を範囲最小値によってあげる為の計算
			int amax = (int)Math.Pow(1 + ( max * 2 ), 2);
			int amin = ( min == 0 ) ? 0 : (int)Math.Pow(1 + ( ( min - 1 ) * 2 ), 2);
			int trialLimit = 40 * amax / ( amax - amin );
			Random random = new Random(DateTime.Now.Millisecond);
			while (true)
			{
				if (trial >= trialLimit)
				{
					newLocation.set(locX, locY);
					break;
				}
				trial++;

				newX = locX1 + RandomHelper.Next(diffX + 1);
				newY = locY1 + RandomHelper.Next(diffY + 1);

				newLocation.set(newX, newY);

				if (baseLocation.getTileLineDistance(newLocation) < min)
				{
					continue;

				}
				if (isRandomTeleport)
				{ // ランダムテレポートの場合
					if (L1CastleLocation.checkInAllWarArea(newX, newY, mapId))
					{ // いずれかの城エリア
						continue;
					}

					// いずれかのアジト内
					if (L1HouseLocation.isInHouse(newX, newY, mapId))
					{
						continue;
					}
				}

				if (map.isInMap(newX, newY) && map.isPassable(newX, newY))
				{
					break;
				}
			}
			return newLocation;
		}
	}

}