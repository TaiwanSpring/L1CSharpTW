using System;
using System.Collections.Generic;

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
namespace LineageServer.Server.Model
{

	using Config = LineageServer.Server.Config;
	using L1ItemInstance = LineageServer.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using S_DropItem = LineageServer.Serverpackets.S_DropItem;
	using S_RemoveObject = LineageServer.Serverpackets.S_RemoveObject;
	using MapFactory = LineageServer.Utils.MapFactory;

	[Serializable]
	public class L1GroundInventory : L1Inventory
	{
		/// 
		private const long serialVersionUID = 1L;

		private static readonly Timer _timer = new Timer();

		private IDictionary<int, DeletionTimer> _reservedTimers = MapFactory.newMap();

		private class DeletionTimer : TimerTask
		{
			private readonly L1GroundInventory outerInstance;

			internal readonly L1ItemInstance _item;

			public DeletionTimer(L1GroundInventory outerInstance, L1ItemInstance item)
			{
				this.outerInstance = outerInstance;
				_item = item;
			}

			public override void run()
			{
				try
				{
					lock (outerInstance)
					{
						if (!outerInstance._items.Contains(_item))
						{ // 拾われたタイミングによってはこの条件を満たし得る
							return; // 既に拾われている
						}
						outerInstance.removeItem(_item);
					}
				}
				catch (Exception t)
				{
					_log.log(Enum.Level.Server, t.LocalizedMessage, t);
				}
			}
		}

		private L1ItemInstance Timer
		{
			set
			{
				if (!Config.ALT_ITEM_DELETION_TYPE.Equals("std", StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
				if (value.ItemId == 40515)
				{ // 精霊の石
					return;
				}
    
				_timer.schedule(new DeletionTimer(this, value), Config.ALT_ITEM_DELETION_TIME * 60 * 1000);
			}
		}

		private void cancelTimer(L1ItemInstance item)
		{
			DeletionTimer timer = _reservedTimers[item.Id];
			if (timer == null)
			{
				return;
			}
			timer.cancel();
		}

		public L1GroundInventory(int objectId, int x, int y, short map)
		{
			Id = objectId;
			X = x;
			Y = y;
			Map = map;
			L1World.Instance.addVisibleObject(this);
		}

		public override void onPerceive(L1PcInstance perceivedFrom)
		{
			foreach (L1ItemInstance item in Items)
			{
				if (!perceivedFrom.knownsObject(item))
				{
					perceivedFrom.addKnownObject(item);
					perceivedFrom.sendPackets(new S_DropItem(item)); // プレイヤーへDROPITEM情報を通知
				}
			}
		}

		// 認識範囲内にいるプレイヤーへオブジェクト送信
		public override void insertItem(L1ItemInstance item)
		{
			Timer = item;

			foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(item))
			{
				pc.sendPackets(new S_DropItem(item));
				pc.addKnownObject(item);
			}
		}

		// 見える範囲内にいるプレイヤーのオブジェクト更新
		public override void updateItem(L1ItemInstance item)
		{
			foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(item))
			{
				pc.sendPackets(new S_DropItem(item));
			}
		}

		// 空インベントリ破棄及び見える範囲内にいるプレイヤーのオブジェクト削除
		public override void deleteItem(L1ItemInstance item)
		{
			cancelTimer(item);
			foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(item))
			{
				pc.sendPackets(new S_RemoveObject(item));
				pc.removeKnownObject(item);
			}

			_items.Remove(item);
			if (_items.Count == 0)
			{
				L1World.Instance.removeVisibleObject(this);
			}
		}

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(L1PcInventory).FullName);
	}

}