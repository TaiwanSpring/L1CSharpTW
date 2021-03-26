﻿using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Templates;

namespace LineageServer.Server.Server.Model.shop
{
	public class L1AssessedItem
	{
		private readonly int _targetId;
		private int _assessedPrice;

		internal L1AssessedItem(int targetId, int assessedPrice)
		{
			_targetId = targetId;
			L1ItemInstance item = (L1ItemInstance)L1World.Instance.findObject(TargetId);
			if (item.ItemId == 40309)
			{ // Race Tickets
				L1RaceTicket ticket = RaceTicketTable.Instance.getTemplate(_targetId);
				int price = 0;
				if (ticket != null)
				{
					price = (int)( assessedPrice * ticket.get_allotment_percentage() * ticket.get_victory() );
				}
				_assessedPrice = price;
			}
			else
			{
				_assessedPrice = assessedPrice;
			}
		}

		public virtual int TargetId
		{
			get
			{
				return _targetId;
			}
		}

		public virtual int AssessedPrice
		{
			get
			{
				return _assessedPrice;
			}
		}
	}

}