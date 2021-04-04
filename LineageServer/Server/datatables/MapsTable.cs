using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
    sealed class MapsTable : IGameComponent
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.Mapids);
        private static MapsTable _instance;

        /// <summary>
        /// KeyにマップID、Valueにテレポート可否フラグが格納されるHashMap
        /// </summary>
        private readonly IDictionary<int, MapData> _maps = MapFactory.NewMap<int, MapData>();

        /// <summary>
        /// 新しくMapsTableオブジェクトを生成し、マップのテレポート可否フラグを読み込む。
        /// </summary>
        private MapsTable()
        {

        }
        public void Initialize()
        {
            loadMapsFromDatabase();
        }
        /// <summary>
        /// マップのテレポート可否フラグをデータベースから読み込み、HashMap _mapsに格納する。
        /// </summary>
        private void loadMapsFromDatabase()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                MapData data = new MapData();
                int mapId = dataSourceRow.getInt(Mapids.Column_mapid);
                data.startX = dataSourceRow.getInt(Mapids.Column_startX);
                data.endX = dataSourceRow.getInt(Mapids.Column_endX);
                data.startY = dataSourceRow.getInt(Mapids.Column_startY);
                data.endY = dataSourceRow.getInt(Mapids.Column_endY);
                data.monster_amount = dataSourceRow.getDouble(Mapids.Column_monster_amount);
                data.dropRate = dataSourceRow.getDouble(Mapids.Column_drop_rate);
                data.isUnderwater = dataSourceRow.getBoolean(Mapids.Column_underwater);
                data.markable = dataSourceRow.getBoolean(Mapids.Column_markable);
                data.teleportable = dataSourceRow.getBoolean(Mapids.Column_teleportable);
                data.escapable = dataSourceRow.getBoolean(Mapids.Column_escapable);
                data.isUseResurrection = dataSourceRow.getBoolean(Mapids.Column_resurrection);
                data.isUsePainwand = dataSourceRow.getBoolean(Mapids.Column_painwand);
                data.isEnabledDeathPenalty = dataSourceRow.getBoolean(Mapids.Column_penalty);
                data.isTakePets = dataSourceRow.getBoolean(Mapids.Column_take_pets);
                data.isRecallPets = dataSourceRow.getBoolean(Mapids.Column_recall_pets);
                data.isUsableItem = dataSourceRow.getBoolean(Mapids.Column_usable_item);
                data.isUsableSkill = dataSourceRow.getBoolean(Mapids.Column_usable_skill);

                _maps[mapId] = data;
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
            if (_maps.ContainsKey(mapId))
            {
                return _maps[mapId].startX;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// マップがのX終了座標を返す。
        /// </summary>
        /// <param name="mapId">
        ///            調べるマップのマップID </param>
        /// <returns> X終了座標 </returns>
        public int getEndX(int mapId)
        {
            if (_maps.ContainsKey(mapId))
            {
                return _maps[mapId].endX;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// マップがのY開始座標を返す。
        /// </summary>
        /// <param name="mapId">
        ///            調べるマップのマップID </param>
        /// <returns> Y開始座標 </returns>
        public int getStartY(int mapId)
        {
            if (_maps.ContainsKey(mapId))
            {
                return _maps[mapId].startY;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// マップがのY終了座標を返す。
        /// </summary>
        /// <param name="mapId">
        ///            調べるマップのマップID </param>
        /// <returns> Y終了座標 </returns>
        public int getEndY(int mapId)
        {
            if (_maps.ContainsKey(mapId))
            {
                return _maps[mapId].endY;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// マップのモンスター量倍率を返す
        /// </summary>
        /// <param name="mapId">
        ///            調べるマップのマップID </param>
        /// <returns> モンスター量の倍率 </returns>
        public double getMonsterAmount(int mapId)
        {
            if (_maps.ContainsKey(mapId))
            {
                return _maps[mapId].monster_amount;
            }
            else
            {
                return 0d;
            }
        }

        /// <summary>
        /// マップのドロップ倍率を返す
        /// </summary>
        /// <param name="mapId">
        ///            調べるマップのマップID </param>
        /// <returns> ドロップ倍率 </returns>
        public double getDropRate(int mapId)
        {
            if (_maps.ContainsKey(mapId))
            {
                return _maps[mapId].dropRate;
            }
            else
            {
                return 0d;
            }
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
            if (_maps.ContainsKey(mapId))
            {
                return _maps[mapId].isUnderwater;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// マップが、ブックマーク可能であるかを返す。
        /// </summary>
        /// <param name="mapId">
        ///            調べるマップのマップID </param>
        /// <returns> ブックマーク可能であればtrue </returns>
        public bool isMarkable(int mapId)
        {
            if (_maps.ContainsKey(mapId))
            {
                return _maps[mapId].markable;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// マップが、ランダムテレポート可能であるかを返す。
        /// </summary>
        /// <param name="mapId">
        ///            調べるマップのマップID </param>
        /// <returns> 可能であればtrue </returns>
        public bool isTeleportable(int mapId)
        {
            if (_maps.ContainsKey(mapId))
            {
                return _maps[mapId].teleportable;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// マップが、MAPを超えたテレポート可能であるかを返す。
        /// </summary>
        /// <param name="mapId">
        ///            調べるマップのマップID </param>
        /// <returns> 可能であればtrue </returns>
        public bool isEscapable(int mapId)
        {
            if (_maps.ContainsKey(mapId))
            {
                return _maps[mapId].escapable;
            }
            else
            {
                return false;
            }
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
            if (_maps.ContainsKey(mapId))
            {
                return _maps[mapId].isUseResurrection;
            }
            else
            {
                return false;
            }
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
            if (_maps.ContainsKey(mapId))
            {
                return _maps[mapId].isUsePainwand;
            }
            else
            {
                return false;
            }
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
            if (_maps.ContainsKey(mapId))
            {
                return _maps[mapId].isEnabledDeathPenalty;
            }
            else
            {
                return false;
            }
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
            if (_maps.ContainsKey(mapId))
            {
                return _maps[mapId].isTakePets;
            }
            else
            {
                return false;
            }
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
            if (_maps.ContainsKey(mapId))
            {
                return _maps[mapId].isRecallPets;
            }
            else
            {
                return false;
            }
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
            if (_maps.ContainsKey(mapId))
            {
                return _maps[mapId].isUsableItem;
            }
            else
            {
                return false;
            }
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
            if (_maps.ContainsKey(mapId))
            {
                return _maps[mapId].isUsableSkill;
            }
            else
            {
                return false;
            }
        }

        public bool HasMap(int mapId)
        {
            return _maps.ContainsKey(mapId);
        }
        class MapData
        {
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
    }

}