﻿/// <summary>
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
namespace LineageServer.Server.Server.Model
{

	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;

	public class L1ItemOwnerTimer : TimerTask
	{
		public L1ItemOwnerTimer(L1ItemInstance item, int timeMillis)
		{
			_item = item;
			_timeMillis = timeMillis;
		}

		public override void run()
		{
			_item.ItemOwnerId = 0;
			cancel();
		}

		public virtual void begin()
		{
			Timer timer = new Timer();
			timer.schedule(this, _timeMillis);
		}

		private readonly L1ItemInstance _item;

		private readonly int _timeMillis;
	}

}