using System.Collections.Generic;
using System.Linq;

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
namespace LineageServer.Server.Server.DataSources
{

	using ActionCodes = LineageServer.Server.Server.ActionCodes;
	using IdFactory = LineageServer.Server.Server.IdFactory;
	using L1Location = LineageServer.Server.Server.Model.L1Location;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1DoorInstance = LineageServer.Server.Server.Model.Instance.L1DoorInstance;
	using L1DoorGfx = LineageServer.Server.Server.Templates.L1DoorGfx;
	using L1DoorSpawn = LineageServer.Server.Server.Templates.L1DoorSpawn;
	using Lists = LineageServer.Server.Server.Utils.collections.Lists;
	using Maps = LineageServer.Server.Server.Utils.collections.Maps;

	public class DoorTable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(DoorTable).FullName);
		private static DoorTable _instance;

		private readonly IDictionary<L1Location, L1DoorInstance> _doors = Maps.newConcurrentHashMap();
		private readonly IDictionary<L1Location, L1DoorInstance> _doorDirections = Maps.newConcurrentHashMap();

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
					_log.log(Level.WARNING, string.Format("Duplicate door location: id = {0:D}", spawn.Id));
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
			IList<L1Location> keys = Lists.newArrayList();
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
			L1DoorInstance door = _doors.Remove(loc);
			if (door != null)
			{
				removeDirections(door);
				door.deleteMe();
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