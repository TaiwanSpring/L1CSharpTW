using LineageServer.Interfaces;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.Map;
using LineageServer.Server.Types;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
namespace LineageServer.Server.Model
{
	class ElementalStoneGenerator : IRunnable
	{
		private const int ELVEN_FOREST_MAPID = 4;
		private static readonly int MAX_COUNT = Config.ELEMENTAL_STONE_AMOUNT; // 設置個数
		private const int INTERVAL = 3; // 設置間隔 秒
		private const int SLEEP_TIME = 300; // 設置終了後、再設置までのスリープ時間 秒
		private const int FIRST_X = 32911;
		private const int FIRST_Y = 32210;
		private const int LAST_X = 33141;
		private const int LAST_Y = 32500;
		private const int ELEMENTAL_STONE_ID = 40515; // 精霊の石

		private List<L1GroundInventory> _itemList = new List<L1GroundInventory>(MAX_COUNT);

		private readonly GameObject _dummy = new GameObject();

		/// <summary>
		/// 指定された位置に石を置けるかを返す。
		/// </summary>
		private bool canPut(L1Location loc)
		{
			_dummy.MapId = (short)loc.MapId;
			_dummy.X = loc.X;
			_dummy.Y = loc.Y;

			// 可視範囲のプレイヤーチェック
			if (Container.Instance.Resolve<IGameWorld>().getVisiblePlayer(_dummy).Count > 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// 次の設置ポイントを決める。
		/// </summary>
		private Point nextPoint()
		{
			int newX = RandomHelper.Next(LAST_X - FIRST_X) + FIRST_X;
			int newY = RandomHelper.Next(LAST_Y - FIRST_Y) + FIRST_Y;

			return new Point(newX, newY);
		}

		/// <summary>
		/// 拾われた石をリストから削除する。
		/// </summary>
		private void removeItemsPickedUp()
		{
			for (int i = 0; i < _itemList.Count; i++)
			{
				L1GroundInventory gInventory = _itemList[i];
				if (!gInventory.checkItem(ELEMENTAL_STONE_ID))
				{
					_itemList.RemoveAt(i);
					i--;
				}
			}
		}

		/// <summary>
		/// 指定された位置へ石を置く。
		/// </summary>
		private void putElementalStone(L1Location loc)
		{
			L1GroundInventory gInventory = Container.Instance.Resolve<IGameWorld>().getInventory(loc);

			L1ItemInstance item = ItemTable.Instance.createItem(ELEMENTAL_STONE_ID);
			item.EnchantLevel = 0;
			item.Count = 1;
			gInventory.storeItem(item);
			_itemList.Add(gInventory);
		}

		public void run()
		{
			L1Map map = Container.Instance.Resolve<IWorldMap>().getMap((short)ELVEN_FOREST_MAPID);
			while (true)
			{
				removeItemsPickedUp();

				while (_itemList.Count < MAX_COUNT)
				{
					// 減っている場合セット
					L1Location loc = new L1Location(nextPoint(), map);

					if (!canPut(loc))
					{
						// XXX 設置範囲内全てにPCが居た場合無限ループになるが…
						continue;
					}

					putElementalStone(loc);

					Thread.Sleep(INTERVAL * 1000); // 一定時間毎に設置
				}

				Thread.Sleep(SLEEP_TIME * 1000); // maxまで設置終了後一定時間は再設置しない
			}
		}
	}

}