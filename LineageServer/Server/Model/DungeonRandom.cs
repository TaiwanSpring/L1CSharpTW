using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Utils;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Server.Model
{
    class DungeonRandomController
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.DungeonRandom);
        private static DungeonRandomController _instance = null;

        private static IDictionary<string, NewDungeonRandom> _dungeonMap = MapFactory.NewMap<string, NewDungeonRandom>();

        public static DungeonRandomController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DungeonRandomController();
                }
                return _instance;
            }
        }

        private DungeonRandomController()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                int srcMapId = dataSourceRow.getInt(DungeonRandom.Column_src_mapid);
                int srcX = dataSourceRow.getInt(DungeonRandom.Column_src_x);
                int srcY = dataSourceRow.getInt(DungeonRandom.Column_src_y);
                string key = (new StringBuilder()).Append(srcMapId).Append(srcX).Append(srcY).ToString();
                int[] newX = new int[5];
                int[] newY = new int[5];
                short[] NewMapId = new short[5];
                newX[0] = dataSourceRow.getInt(DungeonRandom.Column_new_x1);
                newY[0] = dataSourceRow.getInt(DungeonRandom.Column_new_y1);
                NewMapId[0] = dataSourceRow.getShort(DungeonRandom.Column_new_mapid1);
                newX[1] = dataSourceRow.getInt(DungeonRandom.Column_new_x2);
                newY[1] = dataSourceRow.getInt(DungeonRandom.Column_new_y2);
                NewMapId[1] = dataSourceRow.getShort(DungeonRandom.Column_new_mapid2);
                newX[2] = dataSourceRow.getInt(DungeonRandom.Column_new_x3);
                newY[2] = dataSourceRow.getInt(DungeonRandom.Column_new_y3);
                NewMapId[2] = dataSourceRow.getShort(DungeonRandom.Column_new_mapid3);
                newX[3] = dataSourceRow.getInt(DungeonRandom.Column_new_x4);
                newY[3] = dataSourceRow.getInt(DungeonRandom.Column_new_y4);
                NewMapId[3] = dataSourceRow.getShort(DungeonRandom.Column_new_mapid4);
                newX[4] = dataSourceRow.getInt(DungeonRandom.Column_new_x5);
                newY[4] = dataSourceRow.getInt(DungeonRandom.Column_new_y5);
                NewMapId[4] = dataSourceRow.getShort(DungeonRandom.Column_new_mapid5);
                int heading = dataSourceRow.getInt(DungeonRandom.Column_new_heading);
                NewDungeonRandom newDungeonRandom = new NewDungeonRandom(newX, newY, NewMapId, heading);

                _dungeonMap[key] = newDungeonRandom;
            }
        }

        private class NewDungeonRandom
        {
            internal int[] _newX = new int[5];

            internal int[] _newY = new int[5];

            internal short[] _NewMapId = new short[5];

            internal int _heading;

            internal NewDungeonRandom(int[] newX, int[] newY, short[] NewMapId, int heading)
            {
                for (int i = 0; i < 5; i++)
                {
                    _newX[i] = newX[i];
                    _newY[i] = newY[i];
                    _NewMapId[i] = NewMapId[i];
                }
                _heading = heading;
            }
        }

        public virtual bool dg(int locX, int locY, int mapId, L1PcInstance pc)
        {
            string key = (new StringBuilder()).Append(mapId).Append(locX).Append(locY).ToString();
            if (_dungeonMap.ContainsKey(key))
            {
                int rnd = RandomHelper.Next(5);
                NewDungeonRandom newDungeonRandom = _dungeonMap[key];
                short NewMap = newDungeonRandom._NewMapId[rnd];
                int newX = newDungeonRandom._newX[rnd];
                int newY = newDungeonRandom._newY[rnd];
                int heading = newDungeonRandom._heading;

                // 2秒無敵狀態。
                pc.setSkillEffect(L1SkillId.ABSOLUTE_BARRIER, 2000);
                pc.stopHpRegeneration();
                pc.stopMpRegeneration();
                pc.stopHpRegenerationByDoll();
                pc.stopMpRegenerationByDoll();
                L1Teleport.teleport(pc, newX, newY, NewMap, heading, true);
                return true;
            }
            return false;
        }
    }

}