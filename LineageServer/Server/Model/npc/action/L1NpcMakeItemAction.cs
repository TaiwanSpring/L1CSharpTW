﻿using System;
using System.Collections.Generic;
namespace LineageServer.Server.Model.Npc.Action
{

    using ItemTable = LineageServer.Server.DataSources.ItemTable;
    using GameObject = LineageServer.Server.Model.GameObject;
    using LineageServer.Server.Model;
    using L1PcInventory = LineageServer.Server.Model.L1PcInventory;
    using L1ItemInstance = LineageServer.Server.Model.Instance.L1ItemInstance;
    using L1NpcInstance = LineageServer.Server.Model.Instance.L1NpcInstance;
    using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
    using L1NpcHtml = LineageServer.Server.Model.Npc.L1NpcHtml;
    using S_HowManyMake = LineageServer.Serverpackets.S_HowManyMake;
    using S_ServerMessage = LineageServer.Serverpackets.S_ServerMessage;
    using L1Item = LineageServer.Server.Templates.L1Item;
    using IterableElementList = LineageServer.Utils.IterableElementList;
    using ListFactory = LineageServer.Utils.ListFactory;

    using Element = org.w3c.dom.Element;
    using NodeList = org.w3c.dom.NodeList;

    class L1NpcMakeItemAction : L1NpcXmlAction
    {
        private readonly IList<L1ObjectAmount<int>> _materials = ListFactory.newList();

        private readonly IList<L1ObjectAmount<int>> _items = ListFactory.newList();

        private readonly bool _isAmountInputable;

        private readonly INpcAction _actionOnSucceed;

        private readonly INpcAction _actionOnFail;

        public L1NpcMakeItemAction(Element element) : base(element)
        {

            _isAmountInputable = L1NpcXmlParser.getBoolAttribute(element, "AmountInputable", true);
            NodeList list = element.ChildNodes;
            foreach (Element elem in new IterableElementList(list))
            {
                if (elem.NodeName.equalsIgnoreCase("Material"))
                {
                    int id = Convert.ToInt32(elem.getAttribute("ItemId"));
                    int amount = Convert.ToInt32(elem.getAttribute("Amount"));
                    _materials.Add(new L1ObjectAmount<int>(id, amount));
                    continue;
                }
                if (elem.NodeName.equalsIgnoreCase("Item"))
                {
                    int id = Convert.ToInt32(elem.getAttribute("ItemId"));
                    int amount = Convert.ToInt32(elem.getAttribute("Amount"));
                    _items.Add(new L1ObjectAmount<int>(id, amount));
                    continue;
                }
            }

            if (_items.Count == 0 || _materials.Count == 0)
            {
                throw new System.ArgumentException();
            }

            Element elem = L1NpcXmlParser.getFirstChildElementByTagName(element, "Succeed");
            _actionOnSucceed = elem == null ? null : new L1NpcListedAction(elem);
            elem = L1NpcXmlParser.getFirstChildElementByTagName(element, "Fail");
            _actionOnFail = elem == null ? null : new L1NpcListedAction(elem);
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
                    pc.sendPackets(new S_ServerMessage(337, temp.Name + "(" + ((material.Amount * amount) - pc.Inventory.countItems(temp.ItemId)) + ")")); // \f1%0が不足しています。
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
                weight += temp.Weight * (makingItem.Amount * amount) / 1000;
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
            int count = int.MaxValue;
            foreach (L1ObjectAmount<int> material in _materials)
            {
                int numOfSet = inv.countItems(material.Object) / material.Amount;
                count = Math.Min(count, numOfSet);
            }
            return count;
        }

        public override L1NpcHtml execute(string actionName, L1PcInstance pc, GameObject obj, sbyte[] args)
        {
            int numOfMaterials = countNumOfMaterials(pc.Inventory);
            if ((1 < numOfMaterials) && _isAmountInputable)
            {
                pc.sendPackets(new S_HowManyMake(obj.Id, numOfMaterials, actionName));
                return null;
            }
            return executeWithAmount(actionName, pc, obj, 1);
        }

        public override L1NpcHtml executeWithAmount(string actionName, L1PcInstance pc, GameObject obj, int amount)
        {
            L1NpcInstance npc = (L1NpcInstance)obj;
            L1NpcHtml result = null;
            if (makeItems(pc, npc.NpcTemplate.get_name(), amount))
            {
                if (_actionOnSucceed != null)
                {
                    result = _actionOnSucceed.execute(actionName, pc, obj, new sbyte[0]);
                }
            }
            else
            {
                if (_actionOnFail != null)
                {
                    result = _actionOnFail.execute(actionName, pc, obj, new sbyte[0]);
                }
            }
            return result == null ? L1NpcHtml.HTML_CLOSE : result;
        }

    }

}