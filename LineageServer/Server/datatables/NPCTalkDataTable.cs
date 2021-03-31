using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Utils;
using System.Collections.Generic;

namespace LineageServer.Server.DataTables
{
	class NPCTalkDataTable
	{
		private readonly static IDataSource dataSource =
			 Container.Instance.Resolve<IDataSourceFactory>()
			 .Factory(Enum.DataSourceTypeEnum.Npcaction);

		private static NPCTalkDataTable _instance;

		private IDictionary<int, L1NpcTalkData> _datatable = MapFactory.NewMap<int, L1NpcTalkData>();

		public static NPCTalkDataTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new NPCTalkDataTable();
				}
				return _instance;
			}
		}

		private NPCTalkDataTable()
		{
			parseList();
		}

		private void parseList()
		{
			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();
			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				L1NpcTalkData l1npctalkdata = new L1NpcTalkData();
				l1npctalkdata.NpcID = dataSourceRow.getInt(Npcaction.Column_npcid);
				l1npctalkdata.NormalAction = dataSourceRow.getString(Npcaction.Column_normal_action);
				l1npctalkdata.CaoticAction = dataSourceRow.getString(Npcaction.Column_caotic_action);
				l1npctalkdata.TeleportURL = dataSourceRow.getString(Npcaction.Column_teleport_url);
				l1npctalkdata.TeleportURLA = dataSourceRow.getString(Npcaction.Column_teleport_urla);
				_datatable[l1npctalkdata.NpcID] = l1npctalkdata;
			}
		}

		public virtual L1NpcTalkData getTemplate(int i)
		{
			return _datatable[i];
		}
	}
}