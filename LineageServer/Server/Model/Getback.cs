using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.Model
{
    class GetbackController
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.Getback);
        private static ILogger _log = Logger.GetLogger(nameof(GetbackController));

        private static IDictionary<int, IList<GetbackController>> _getback = MapFactory.NewMap<int, IList<GetbackController>>();

        private int _areaX1;

        private int _areaY1;

        private int _areaX2;

        private int _areaY2;

        private int _areaMapId;

        private int _getbackX1;

        private int _getbackY1;

        private int _getbackX2;

        private int _getbackY2;

        private int _getbackX3;

        private int _getbackY3;

        private int _getbackMapId;

        private int _getbackTownId;

        private int _getbackTownIdForElf;

        private int _getbackTownIdForDarkelf;

        private GetbackController()
        {
        }

        private bool SpecifyArea
        {
            get
            {
                return ((_areaX1 != 0) && (_areaY1 != 0) && (_areaX2 != 0) && (_areaY2 != 0));
            }
        }

        public static void loadGetBack()
        {
            _getback.Clear();
            IList<IDataSourceRow> dataSourceRows = dataSource.Select()
               .OrderByDesc(Getback.Column_area_x1).Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                GetbackController getback = new GetbackController();
                getback._areaX1 = dataSourceRow.getInt(Getback.Column_area_x1);
                getback._areaY1 = dataSourceRow.getInt(Getback.Column_area_y1);
                getback._areaX2 = dataSourceRow.getInt(Getback.Column_area_x2);
                getback._areaY2 = dataSourceRow.getInt(Getback.Column_area_y2);
                getback._areaMapId = dataSourceRow.getInt(Getback.Column_area_mapid);
                getback._getbackX1 = dataSourceRow.getInt(Getback.Column_getback_x1);
                getback._getbackY1 = dataSourceRow.getInt(Getback.Column_getback_y1);
                getback._getbackX2 = dataSourceRow.getInt(Getback.Column_getback_x2);
                getback._getbackY2 = dataSourceRow.getInt(Getback.Column_getback_y2);
                getback._getbackX3 = dataSourceRow.getInt(Getback.Column_getback_x3);
                getback._getbackY3 = dataSourceRow.getInt(Getback.Column_getback_y3);
                getback._getbackMapId = dataSourceRow.getInt(Getback.Column_getback_mapid);
                getback._getbackTownId = dataSourceRow.getInt(Getback.Column_getback_townid);
                getback._getbackTownIdForElf = dataSourceRow.getInt(Getback.Column_getback_townid_elf);
                getback._getbackTownIdForDarkelf = dataSourceRow.getInt(Getback.Column_getback_townid_darkelf);
                dataSourceRow.getBoolean(Getback.Column_scrollescape);

                if (_getback.ContainsKey(getback._areaMapId))
                {

                }
                else
                {
                    _getback.Add(getback._areaMapId, ListFactory.NewList<GetbackController>());
                }
                _getback[getback._areaMapId].Add(getback);
            }
        }

        /// <summary>
        /// pcの現在地から帰還ポイントを取得する。
        /// </summary>
        /// <param name="pc"> </param>
        /// <param name="bScroll_Escape">
        ///            (未使用) </param>
        /// <returns> locx,locy,mapidの順に格納されている配列 </returns>
        public static int[] GetBack_Location(L1PcInstance pc, bool bScroll_Escape)
        {

            int[] loc = new int[3];

            int nPosition = RandomHelper.Next(3);

            int pcLocX = pc.X;
            int pcLocY = pc.Y;
            int pcMapId = pc.MapId;
            IList<GetbackController> getbackList = _getback[pcMapId];

            if (getbackList != null)
            {
                GetbackController getback = null;
                foreach (GetbackController gb in getbackList)
                {
                    if (gb.SpecifyArea)
                    {
                        if ((gb._areaX1 <= pcLocX) && (pcLocX <= gb._areaX2) && (gb._areaY1 <= pcLocY) && (pcLocY <= gb._areaY2))
                        {
                            getback = gb;
                            break;
                        }
                    }
                    else
                    {
                        getback = gb;
                        break;
                    }
                }

                loc = ReadGetbackInfo(getback, nPosition);

                // town_idが指定されている場合はそこへ帰還させる
                if (pc.Elf && (getback._getbackTownIdForElf > 0))
                {
                    loc = L1TownLocation.getGetBackLoc(getback._getbackTownIdForElf);
                }
                else if (pc.Darkelf && (getback._getbackTownIdForDarkelf > 0))
                {
                    loc = L1TownLocation.getGetBackLoc(getback._getbackTownIdForDarkelf);
                }
                else if (getback._getbackTownId > 0)
                {
                    loc = L1TownLocation.getGetBackLoc(getback._getbackTownId);
                }
            }
            // getbackテーブルにデータがない場合、SKTに帰還
            else
            {
                loc[0] = 33089;
                loc[1] = 33397;
                loc[2] = 4;
            }
            return loc;
        }

        private static int[] ReadGetbackInfo(GetbackController getback, int nPosition)
        {
            int[] loc = new int[3];
            switch (nPosition)
            {
                case 0:
                    loc[0] = getback._getbackX1;
                    loc[1] = getback._getbackY1;
                    break;

                case 1:
                    loc[0] = getback._getbackX2;
                    loc[1] = getback._getbackY2;
                    break;

                case 2:
                    loc[0] = getback._getbackX3;
                    loc[1] = getback._getbackY3;
                    break;
            }
            loc[2] = getback._getbackMapId;

            return loc;
        }
    }

}