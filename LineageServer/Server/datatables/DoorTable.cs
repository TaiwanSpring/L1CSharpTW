using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;
using System.Linq;
namespace LineageServer.Server.DataTables
{
	class DoorTable
	{
		private static DoorTable _instance;

		private readonly IDictionary<L1Location, L1DoorInstance> _doors = MapFactory.NewConcurrentMap<L1Location, L1DoorInstance>();
		private readonly IDictionary<L1Location, L1DoorInstance> _doorDirections = MapFactory.NewConcurrentMap<L1Location, L1DoorInstance>();

		public static void initialize()
		{
			_instance = new DoorTable();
		}

		public static DoorTable Instance
		{
			get
			{
				return _instance;
			}
		}

		private DoorTable()
		{
			loadDoors();
		}

		private void loadDoors()
		{
			foreach (L1DoorSpawn spawn in L1DoorSpawn.all())
			{
				L1Location loc = spawn.Location;
				if (_doors.ContainsKey(loc))
				{
					continue;
				}
				createDoor(spawn.Id, spawn.Gfx, loc, spawn.Hp, spawn.Keeper, spawn.Opening);
			}
		}

		private void putDirections(L1DoorInstance door)
		{
			foreach (L1Location key in makeDirectionsKeys(door))
			{
				_doorDirections[key] = door;
			}
		}

		private void removeDirections(L1DoorInstance door)
		{
			foreach (L1Location key in makeDirectionsKeys(door))
			{
				_doorDirections.Remove(key);
			}
		}

		private IList<L1Location> makeDirectionsKeys(L1DoorInstance door)
		{
			IList<L1Location> keys = ListFactory.NewList<L1Location>();
			int left = door.LeftEdgeLocation;
			int right = door.RightEdgeLocation;
			if (door.Direction == 0)
			{
				for (int x = left; x <= right; x++)
				{
					keys.Add(new L1Location(x, door.Y, door.MapId));
				}
			}
			else
			{
				for (int y = left; y <= right; y++)
				{
					keys.Add(new L1Location(door.X, y, door.MapId));
				}
			}
			return keys;
		}

		public virtual L1DoorInstance createDoor(int doorId, L1DoorGfx gfx, L1Location loc, int hp, int keeper, bool isOpening)
		{
			if (_doors.ContainsKey(loc))
			{
				return null;
			}
			L1DoorInstance door = new L1DoorInstance(doorId, gfx, loc, hp, keeper, isOpening);

			door.Id = IdFactory.Instance.nextId();

			L1World.Instance.storeObject(door);
			L1World.Instance.addVisibleObject(door);

			_doors[door.Location] = door;
			putDirections(door);
			return door;
		}

		public virtual void deleteDoorByLocation(L1Location loc)
		{
			if (_doors.ContainsKey(loc))
			{
				L1DoorInstance door = _doors[loc];
				if (_doors.Remove(loc))
				{
					removeDirections(door);
					door.deleteMe();
				}
			}
		}

		public virtual int getDoorDirection(L1Location loc)
		{
			L1DoorInstance door = _doorDirections[loc];
			if (door == null || door.OpenStatus == ActionCodes.ACTION_Open)
			{
				return -1;
			}
			return door.Direction;
		}

		public virtual L1DoorInstance findByDoorId(int doorId)
		{
			foreach (L1DoorInstance door in _doors.Values)
			{
				if (door.DoorId == doorId)
				{
					return door;
				}
			}
			return null;
		}

		public virtual L1DoorInstance[] DoorList
		{
			get
			{
				return _doors.Values.ToArray();
			}
		}
	}

}