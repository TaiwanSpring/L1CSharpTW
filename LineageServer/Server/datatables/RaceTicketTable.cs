using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
namespace LineageServer.Server.DataTables
{
	class RaceTicketTable
	{
		private readonly static IDataSource dataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.RaceTicket);

		private static RaceTicketTable _instance;

		private readonly IDictionary<int, L1RaceTicket> _tickets = MapFactory.NewMap<int, L1RaceTicket>();

		private int _maxRoundNumber;

		public static RaceTicketTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new RaceTicketTable();
				}
				return _instance;
			}
		}

		private RaceTicketTable()
		{
			load();
		}

		private void load()
		{
			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

			int temp = 0;

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				L1RaceTicket ticket = new L1RaceTicket();
				int itemobjid = dataSourceRow.getInt(RaceTicket.Column_item_obj_id);
				ticket.set_itemobjid(itemobjid);
				ticket.set_round(dataSourceRow.getInt(RaceTicket.Column_round));
				ticket.set_allotment_percentage(dataSourceRow.getInt(RaceTicket.Column_allotment_percentage));
				ticket.set_victory(dataSourceRow.getInt(RaceTicket.Column_victory));
				ticket.set_runner_num(dataSourceRow.getInt(RaceTicket.Column_runner_num));

				if (ticket.get_round() > temp)
				{
					temp = ticket.get_round();
				}
				_tickets[itemobjid] = ticket;
			}

			_maxRoundNumber = temp;
		}

		public virtual void storeNewTiket(L1RaceTicket ticket)
		{
			// PCのインベントリーが増える場合に実行
			// XXX 呼ばれる前と処理の重複
			if (!_tickets.ContainsKey(ticket.get_itemobjid()))
			{
				IDataSourceRow dataSourceRow = dataSource.NewRow();
				dataSourceRow.Insert()
				.Set(RaceTicket.Column_item_obj_id, ticket.get_itemobjid())
				.Set(RaceTicket.Column_round, ticket.get_round())
				.Set(RaceTicket.Column_allotment_percentage, ticket.get_allotment_percentage())
				.Set(RaceTicket.Column_victory, ticket.get_victory())
				.Set(RaceTicket.Column_runner_num, ticket.get_runner_num())
				.Execute();
				_tickets.Add(ticket.get_itemobjid(), ticket);
			}
		}

		public virtual void deleteTicket(int itemobjid)
		{
			// PCのインベントリーが減少する再に使用
			if (_tickets.ContainsKey(itemobjid))
			{
				IDataSourceRow dataSourceRow = dataSource.NewRow();
				dataSourceRow.Delete()
				.Where(RaceTicket.Column_item_obj_id, itemobjid)
				.Execute();

				_tickets.Remove(itemobjid);
			}
		}

		public virtual void oldTicketDelete(int round)
		{
			IDataSourceRow dataSourceRow = dataSource.NewRow();
			dataSourceRow.Delete()
			.Where(RaceTicket.Column_item_obj_id, 0)
			.WhereNot(RaceTicket.Column_round, round)
			.Execute();
		}

		public virtual void updateTicket(int round, int num, double allotment_percentage)
		{
			foreach (L1RaceTicket ticket in RaceTicketTableList)
			{
				if (ticket.get_round() == round && ticket.get_runner_num() == num)
				{
					ticket.set_victory(1);
					ticket.set_allotment_percentage(allotment_percentage);
				}
			}

			IDataSourceRow dataSourceRow = dataSource.NewRow();
			dataSourceRow.Update()
			.Where(RaceTicket.Column_round, round)
			.Set(RaceTicket.Column_allotment_percentage, allotment_percentage)
			.Set(RaceTicket.Column_victory, 1)
			.Set(RaceTicket.Column_runner_num, num)
			.Execute();
		}

		public virtual L1RaceTicket getTemplate(int itemobjid)
		{
			if (_tickets.ContainsKey(itemobjid))
			{
				return _tickets[itemobjid];
			}
			return null;
		}

		public virtual L1RaceTicket[] RaceTicketTableList
		{
			get
			{
				return _tickets.Values.ToArray();
			}
		}

		public virtual int RoundNumOfMax
		{
			get
			{
				return _maxRoundNumber;
			}
		}
	}

}