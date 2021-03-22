using System;
using System.Collections.Generic;
using System.Threading;

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
namespace LineageServer.Server.Server.Model
{
	using Random = LineageServer.Server.Server.utils.Random;

	using Config = LineageServer.Server.Config;
	using ItemTable = LineageServer.Server.Server.DataSources.ItemTable;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1Map = LineageServer.Server.Server.Model.map.L1Map;
	using L1WorldMap = LineageServer.Server.Server.Model.map.L1WorldMap;
	using Point = LineageServer.Server.Server.Types.Point;

	public class ElementalStoneGenerator : IRunnableStart
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(ElementalStoneGenerator).FullName);

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

		private static ElementalStoneGenerator _instance = null;

		private ElementalStoneGenerator()
		{
		}

		public static ElementalStoneGenerator Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ElementalStoneGenerator();
				}
				return _instance;
			}
		}

		private readonly L1Object _dummy = new L1Object();

		/// <summary>
		/// 指定された位置に石を置けるかを返す。
		/// </summary>
		private bool canPut(L1Location loc)
		{
			_dummy.setMap(loc.getMap());
			_dummy.X = loc.X;
			_dummy.Y = loc.Y;

			// 可視範囲のプレイヤーチェック
			if (L1World.Instance.getVisiblePlayer(_dummy).Count > 0)
			{
				return false;
			}
			return true;
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
			L1GroundInventory gInventory = L1World.Instance.getInventory(loc);

			L1ItemInstance item = ItemTable.Instance.createItem(ELEMENTAL_STONE_ID);
			item.EnchantLevel = 0;
			item.Count = 1;
			gInventory.storeItem(item);
			_itemList.Add(gInventory);
		}

		public override void run()
		{
			try
			{
				L1Map map = L1WorldMap.Instance.getMap((short) ELVEN_FOREST_MAPID);
				while (true)
				{
					removeItemsPickedUp();

					while (_itemList.Count < MAX_COUNT)
					{ // 減っている場合セット
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
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
		}
	}

}