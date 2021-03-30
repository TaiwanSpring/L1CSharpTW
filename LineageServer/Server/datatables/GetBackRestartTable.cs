using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;
using System.Linq;
namespace LineageServer.Server.DataTables
{
    class GetBackRestartTable
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.GetbackRestart);
        private static GetBackRestartTable _instance;

        private readonly IDictionary<int, L1GetBackRestart> _getbackrestart = MapFactory.NewMap<int, L1GetBackRestart>();

        public static GetBackRestartTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GetBackRestartTable();
                }
                return _instance;
            }
        }

        public GetBackRestartTable()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();
            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                L1GetBackRestart gbr = new L1GetBackRestart();
                gbr.Area = dataSourceRow.getInt(GetbackRestart.Column_area);
                gbr.LocX = dataSourceRow.getInt(GetbackRestart.Column_locx);
                gbr.LocY = dataSourceRow.getInt(GetbackRestart.Column_locy);
                gbr.MapId = dataSourceRow.getShort(GetbackRestart.Column_mapid);

                _getbackrestart[gbr.Area] = gbr;
            }
        }

        public virtual L1GetBackRestart[] GetBackRestartTableList
        {
            get
            {
                return _getbackrestart.Values.ToArray();
            }
        }

    }

}