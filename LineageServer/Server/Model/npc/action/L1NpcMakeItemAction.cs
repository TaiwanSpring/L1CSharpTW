using LineageServer.Extensions;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Xml;

namespace LineageServer.Server.Model.Npc.Action
{
	class L1NpcMakeItemAction : L1NpcXmlAction
	{
		private readonly IList<L1ObjectAmount<int>> _materials = ListFactory.NewList<L1ObjectAmount<int>>();

		private readonly IList<L1ObjectAmount<int>> _items = ListFactory.NewList<L1ObjectAmount<int>>();

		private readonly bool _isAmountInputable;

		private readonly INpcAction _actionOnSucceed;

		private readonly INpcAction _actionOnFail;

		public L1NpcMakeItemAction(XmlElement xmlElement) : base(xmlElement)
		{
			_isAmountInputable = xmlElement.GetBool("AmountInputable", true);

			for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
			{
				if (xmlElement.ChildNodes[i].NodeType == XmlNodeType.Element
					&& xmlElement.ChildNodes[i] is XmlElement element)
				{
					if (element.Name == "Succeed")
					{
						_actionOnSucceed = new L1NpcListedAction(element);
					}
					else if (element.Name == "Fail")
					{
						_actionOnFail = new L1NpcListedAction(element);
					}
					else
					{
						int id = element.GetInt("ItemId", -1);
						if (id == -1)
						{
							continue;
						}

						IList<L1ObjectAmount<int>> list = null;
						if (element.Name == "Material")
						{
							list = _materials;
						}
						else if (element.Name == "Item")
						{
							list = _items;
						}

						if (list == null)
						{

						}
						else
						{
							int amount = element.GetInt("Amount");

							list.Add(new L1ObjectAmount<int>(id, amount));
						}
					}
				}
			}

			if (_items.Count == 0 || _materials.Count == 0)
			{
				throw new System.ArgumentException();
			}
		}

		private bool makeItems(L1PcInstance pc, string npcName, int amount)
		{
			if (amount <= 0)
			{
				return false;
			}

			bool isEnoughMaterials = true;
			foreach (L1ObjectAmount<int> material in _materials)
			{
				if (!pc.Inventory.checkItemNotEquipped(material.Object, material.Amount * amount))
				{
					L1Item temp = ItemTable.Instance.getTemplate(material.Object);
					pc.sendPackets(new S_ServerMessage(337, temp.Name + "(" + ( ( material.Amount * amount ) - pc.Inventory.countItems(temp.ItemId) ) + ")")); // \f1%0が不足しています。
					isEnoughMaterials = false;
				}
			}
			if (!isEnoughMaterials)
			{
				return false;
			}

			// 容量と重量の計算
			int countToCreate = 0; // アイテムの個数（纏まる物は1個）
			int weight = 0;

			foreach (L1ObjectAmount<int> makingItem in _items)
			{
				L1Item temp = ItemTable.Instance.getTemplate(makingItem.Object);
				if (temp.Stackable)
				{
					if (!pc.Inventory.checkItem(makingItem.Object))
					{
						countToCreate += 1;
					}
				}
				else
				{
					countToCreate += makingItem.Amount * amount;
				}
				weight += temp.Weight * ( makingItem.Amount * amount ) / 1000;
			}
			// 容量確認
			if (pc.Inventory.Size + countToCreate > 180)
			{
				pc.sendPackets(new S_ServerMessage(263)); // \f1一人のキャラクターが持って歩けるアイテムは最大180個までです。
				return false;
			}
			// 重量確認
			if (pc.MaxWeight < pc.Inventory.Weight + weight)
			{
				pc.sendPackets(new S_ServerMessage(82)); // アイテムが重すぎて、これ以上持てません。
				return false;
			}

			foreach (L1ObjectAmount<int> material in _materials)
			{
				// 材料消費
				pc.Inventory.consumeItem(material.Object, material.Amount * amount);
			}

			foreach (L1ObjectAmount<int> makingItem in _items)
			{
				L1ItemInstance item = pc.Inventory.storeItem(makingItem.Object, makingItem.Amount * amount);
				if (item != null)
				{
					string itemName = ItemTable.Instance.getTemplate(makingItem.Object).Name;
					if (makingItem.Amount * amount > 1)
					{
						itemName = itemName + " (" + makingItem.Amount * amount + ")";
					}
					pc.sendPackets(new S_ServerMessage(143, npcName, itemName)); // \f1%0が%1をくれました。
				}
			}
			return true;
		}

		/// <summary>
		/// 指定されたインベントリ内に、素材が何セットあるか数える
		/// </summary>
		private int countNumOfMaterials(L1PcInventory inv)
		{
			if (inv == null)
			{
				return 0;
			}
			int count = int.MaxValue;
			foreach (L1ObjectAmount<int> material in _materials)
			{
				int numOfSet = inv.countItems(material.Object) / material.Amount;
				count = Math.Min(count, numOfSet);
			}
			return count;
		}

		public override L1NpcHtml Execute(string actionName, L1PcInstance pc, GameObject obj, byte[] args)
		{
			int numOfMaterials = countNumOfMaterials(pc.Inventory as L1PcInventory);
			if (( 1 < numOfMaterials ) && _isAmountInputable)
			{
				pc.sendPackets(new S_HowManyMake(obj.Id, numOfMaterials, actionName));
				return null;
			}
			return ExecuteWithAmount(actionName, pc, obj, 1);
		}

		public override L1NpcHtml ExecuteWithAmount(string actionName, L1PcInstance pc, GameObject obj, int amount)
		{
			L1NpcInstance npc = (L1NpcInstance)obj;
			L1NpcHtml result = null;
			if (makeItems(pc, npc.NpcTemplate.get_name(), amount))
			{
				if (_actionOnSucceed != null)
				{
					result = _actionOnSucceed.Execute(actionName, pc, obj, new byte[0]);
				}
			}
			else
			{
				if (_actionOnFail != null)
				{
					result = _actionOnFail.Execute(actionName, pc, obj, new byte[0]);
				}
			}
			return result == null ? L1NpcHtml.HTML_CLOSE : result;
		}

	}

}