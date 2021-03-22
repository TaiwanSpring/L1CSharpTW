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
namespace LineageServer.Server.Server.Model.item
{


	using ItemTable = LineageServer.Server.Server.DataSources.ItemTable;
	using L1Inventory = LineageServer.Server.Server.Model.L1Inventory;
	using L1PcInventory = LineageServer.Server.Server.Model.L1PcInventory;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using PerformanceTimer = LineageServer.Server.Server.utils.PerformanceTimer;
	using Random = LineageServer.Server.Server.utils.Random;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAccessorType(XmlAccessType.FIELD) public class L1TreasureBox
	public class L1TreasureBox
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1TreasureBox).FullName);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAccessorType(XmlAccessType.FIELD) @XmlRootElement(name = "TreasureBoxList") private static class TreasureBoxList implements Iterable<L1TreasureBox>
		private class TreasureBoxList : IEnumerable<L1TreasureBox>
		{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlElement(name = "TreasureBox") private java.util.List<L1TreasureBox> _list;
			internal IList<L1TreasureBox> _list;

			public virtual IEnumerator<L1TreasureBox> GetEnumerator()
			{
				return _list.GetEnumerator();
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAccessorType(XmlAccessType.FIELD) private static class Item
		private class Item
		{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAttribute(name = "ItemId") private int _itemId;
			internal int _itemId;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAttribute(name = "Count") private int _count;
			internal int _count;

			internal int _chance;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unused") @XmlAttribute(name = "Chance") private void setChance(double chance)
			internal virtual double Chance
			{
				set
				{
					_chance = (int)(value * 10000);
				}
				get
				{
					return _chance;
				}
			}

			public virtual int ItemId
			{
				get
				{
					return _itemId;
				}
			}

			public virtual int Count
			{
				get
				{
					return _count;
				}
			}


//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAttribute(name = "Enchant") private int _enchant;
			internal int _enchant;

			public virtual int Enchant
			{
				get
				{
					return _enchant;
				}
			}
		}

		private enum TYPE
		{
			RANDOM,
			SPECIFIC
		}

		private const string PATH = "./data/xml/Item/TreasureBox.xml";

		private static readonly IDictionary<int, L1TreasureBox> _dataMap = Maps.newMap();

		/// <summary>
		/// 指定されたIDのTreasureBoxを返す。
		/// </summary>
		/// <param name="id">
		///            - TreasureBoxのID。普通はアイテムのItemIdになる。 </param>
		/// <returns> 指定されたIDのTreasureBox。見つからなかった場合はnull。 </returns>
		public static L1TreasureBox get(int id)
		{
			return _dataMap[id];
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAttribute(name = "ItemId") private int _boxId;
		private int _boxId;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAttribute(name = "Type") private TYPE _type;
		private TYPE _type;

		private int BoxId
		{
			get
			{
				return _boxId;
			}
		}

		private TYPE Type
		{
			get
			{
				return _type;
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlElement(name = "Item") private java.util.concurrent.CopyOnWriteArrayList<Item> _items;
		private CopyOnWriteArrayList<Item> _items;

		private IList<Item> Items
		{
			get
			{
				return _items;
			}
		}

		private int _totalChance;

		private int TotalChance
		{
			get
			{
				return _totalChance;
			}
		}

		private void init()
		{
			foreach (Item each in Items)
			{
				_totalChance += (int)each.Chance;
				if (ItemTable.Instance.getTemplate(each.ItemId) == null)
				{
					Items.Remove(each);
					_log.warning("item ID " + each.ItemId + " is not found。");
				}
			}
			if ((TotalChance != 0) && (TotalChance != 1000000))
			{
				_log.warning("ID " + BoxId + " 的總機率不等於100%。");
			}
		}

		public static void load()
		{
			PerformanceTimer timer = new PerformanceTimer();
            System.Console.Write("【讀取】 【TreasureBox】【設定】");
			try
			{
				JAXBContext context = JAXBContext.newInstance(typeof(L1TreasureBox.TreasureBoxList));

				Unmarshaller um = context.createUnmarshaller();

				File file = new File(PATH);
				TreasureBoxList list = (TreasureBoxList) um.unmarshal(file);

				foreach (L1TreasureBox each in list)
				{
					each.init();
					_dataMap[each.BoxId] = each;
				}
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, PATH + "載入失敗。", e);
				Environment.Exit(0);
			}
            System.Console.WriteLine("【完成】【" + timer.get() + "】【毫秒】。");
		}

		/// <summary>
		/// TreasureBoxを開けるPCにアイテムを入手させる。PCがアイテムを持ちきれなかった場合は アイテムは地面に落ちる。
		/// </summary>
		/// <param name="pc">
		///            - TreasureBoxを開けるPC </param>
		/// <returns> 開封した結果何らかのアイテムが出てきた場合はtrueを返す。 持ちきれず地面に落ちた場合もtrueになる。 </returns>
		public virtual bool open(L1PcInstance pc)
		{
			L1ItemInstance item = null;

			if (Type.Equals(TYPE.SPECIFIC))
			{
				// 出るアイテムが決まっているもの
				foreach (Item each in Items)
				{
					item = ItemTable.Instance.createItem(each.ItemId);
					item.EnchantLevel = each.Enchant; // Enchant Feature for treasure_box
					if (item != null)
					{
						item.Count = each.Count;
						storeItem(pc, item);
					}
				}

			}
			else if (Type.Equals(TYPE.RANDOM))
			{
				// 出るアイテムがランダムに決まるもの
				int chance = 0;

				int r = RandomHelper.Next(TotalChance);

				foreach (Item each in Items)
				{
					chance += (int)each.Chance;

					if (r < chance)
					{
						item = ItemTable.Instance.createItem(each.ItemId);
						item.EnchantLevel = each.Enchant; // Enchant Feature for treasure_box
						if (item != null)
						{
							item.Count = each.Count;
							storeItem(pc, item);
						}
						break;
					}
				}
			}

			if (item == null)
			{
				return false;
			}
			else
			{
				int itemId = BoxId;

				// 魂の結晶の破片、魔族のスクロール、ブラックエントの実
				if ((itemId == 40576) || (itemId == 40577) || (itemId == 40578) || (itemId == 40411) || (itemId == 49013))
				{
					pc.death(null); // キャラクターを死亡させる
				}

				// 多魯嘉之袋
				if ((itemId == 46000))
				{
					L1ItemInstance box = pc.Inventory.findItemId(itemId);
					box.ChargeCount = box.ChargeCount - 1;
					pc.Inventory.updateItem(box, L1PcInventory.COL_CHARGE_COUNT);
					if (box.ChargeCount < 1)
					{
						pc.Inventory.removeItem(box, 1);
					}
				}

				return true;
			}
		}

		private static void storeItem(L1PcInstance pc, L1ItemInstance item)
		{
			L1Inventory inventory;

			if (pc.Inventory.checkAddItem(item, item.Count) == L1Inventory.OK)
			{
				inventory = pc.Inventory;
			}
			else
			{
				// 持てない場合は地面に落とす 処理のキャンセルはしない（不正防止）
				inventory = L1World.Instance.getInventory(pc.Location);
			}
			inventory.storeItem(item);
			pc.sendPackets(new S_ServerMessage(403, item.LogName)); // %0を手に入れました。
		}
	}

}