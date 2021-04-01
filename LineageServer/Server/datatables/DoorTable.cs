using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;
using System.Linq;
using LineageServer.Interfaces;
using LineageServer.DataBase.DataSources;

namespace LineageServer.Server.DataTables
{
    class DoorTable : IGameComponent, IDoorController
    {
        private readonly IDictionary<L1Location, L1DoorInstance> _doors = MapFactory.NewConcurrentMap<L1Location, L1DoorInstance>();
        private readonly IDictionary<L1Location, L1DoorInstance> _doorDirections = MapFactory.NewConcurrentMap<L1Location, L1DoorInstance>();

        public void Initialize()
        {
            loadDoors();
        }
        private void loadDoors()
        {
            foreach (L1DoorSpawn spawn in all())
            {
                if (_doors.ContainsKey(spawn.Location))
                {

                }
                else
                {
                    L1DoorInstance door = new L1DoorInstance(spawn.Id, spawn.Gfx, spawn.Location, spawn.Hp, spawn.Keeper, spawn.Opening);
                    door.Id = Container.Instance.Resolve<IIdFactory>().nextId();
                    Container.Instance.Resolve<IGameWorld>().storeObject(door);
                    Container.Instance.Resolve<IGameWorld>().addVisibleObject(door);
                    putDirections(door);
                    _doors[door.Location] = door;
                }
            }
        }
        private IList<L1DoorSpawn> all()
        {
            IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>()
                .Factory(Enum.DataSourceTypeEnum.SpawnlistDoor);
            IList<L1DoorSpawn> result = ListFactory.NewList<L1DoorSpawn>();
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();
            IDictionary<int, L1DoorGfx> doorGfxMapping = GetDoorGfxMapping();
            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                int gfxId = dataSourceRow.getInt(SpawnlistDoor.Column_gfxid);
                if (doorGfxMapping.ContainsKey(gfxId))
                {
                    int id = dataSourceRow.getInt(SpawnlistDoor.Column_id);
                    int x = dataSourceRow.getInt(SpawnlistDoor.Column_locx);
                    int y = dataSourceRow.getInt(SpawnlistDoor.Column_locy);
                    int mapId = dataSourceRow.getInt(SpawnlistDoor.Column_mapid);
                    int hp = dataSourceRow.getInt(SpawnlistDoor.Column_hp);
                    int keeper = dataSourceRow.getInt(SpawnlistDoor.Column_keeper);
                    bool isOpening = dataSourceRow.getBoolean(SpawnlistDoor.Column_isOpening);
                    L1DoorGfx gfx = doorGfxMapping[gfxId];
                    L1DoorSpawn spawn = new L1DoorSpawn(id, gfx, x, y, mapId, hp, keeper, isOpening);
                    result.Add(spawn);
                }
            }
            return result;
        }
        /// <summary>
        /// door_gfxsテーブルから指定されたgfxidを主キーとする行を返します。<br>
        /// このメソッドは常に最新のデータをテーブルから返します。
        /// </summary>
        /// <param name="gfxId">
        /// @return </param>
        private IDictionary<int, L1DoorGfx> GetDoorGfxMapping()
        {
            IDictionary<int, L1DoorGfx> result = MapFactory.NewMap<int, L1DoorGfx>();
            IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>()
                .Factory(Enum.DataSourceTypeEnum.DoorGfxs);

            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                int id = dataSourceRow.getInt(DoorGfxs.Column_gfxid);
                int dir = dataSourceRow.getInt(DoorGfxs.Column_direction);
                int rEdge = dataSourceRow.getInt(DoorGfxs.Column_right_edge_offset);
                int lEdge = dataSourceRow.getInt(DoorGfxs.Column_left_edge_offset);
                result.Add(id, new L1DoorGfx(id, dir, rEdge, lEdge));
            }
            return result;
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

        public void deleteDoorByLocation(L1Location loc)
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

        public int getDoorDirection(L1Location loc)
        {
            L1DoorInstance door = _doorDirections[loc];
            if (door == null || door.OpenStatus == ActionCodes.ACTION_Open)
            {
                return -1;
            }
            return door.Direction;
        }

        public L1DoorInstance findByDoorId(int doorId)
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

        public L1DoorInstance[] DoorList
        {
            get
            {
                return _doors.Values.ToArray();
            }
        }
    }

}