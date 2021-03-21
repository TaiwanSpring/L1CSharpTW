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
namespace LineageServer.Server.Server.Model.shop
{
	using RaceTicketTable = LineageServer.Server.Server.datatables.RaceTicketTable;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1RaceTicket = LineageServer.Server.Server.Templates.L1RaceTicket;

	public class L1AssessedItem
	{
		private readonly int _targetId;
		private int _assessedPrice;

		internal L1AssessedItem(int targetId, int assessedPrice)
		{
			_targetId = targetId;
			L1ItemInstance item = (L1ItemInstance) L1World.Instance.findObject(TargetId);
			if (item.ItemId == 40309)
			{ // Race Tickets
				L1RaceTicket ticket = RaceTicketTable.Instance.getTemplate(_targetId);
				int price = 0;
				if (ticket != null)
				{
					price = (int)(assessedPrice * ticket.get_allotment_percentage() * ticket.get_victory());
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