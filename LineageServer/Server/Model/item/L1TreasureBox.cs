using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace LineageServer.Server.Model.item
{
    [XmlRoot(ElementName = nameof(TreasureBoxList))]
    class TreasureBoxList
    {
        [XmlElement(ElementName = "TreasureBox")]
        public List<L1TreasureBox> Items { get; set; }
    }

    [XmlRoot(ElementName = nameof(Item))]
    class Item
    {
        [XmlAttribute(AttributeName = nameof(ItemId))]
        public int ItemId { get; set; }
        [XmlAttribute(AttributeName = nameof(Count))]
        public int Count { get; set; }
        [XmlAttribute(AttributeName = nameof(Chance))]
        public double Chance { get; set; }
        [XmlAttribute(AttributeName = nameof(Enchant))]
        public int Enchant { get; set; }
        //[XmlAttribute(AttributeName = nameof(Attr))]
        //public string Attr { get; set; }
        //[XmlAttribute(AttributeName = nameof(Identi))]
        //public string Identi { get; set; }
        //[XmlAttribute(AttributeName = nameof(Bless))]
        //public string Bless { get; set; }
    }

    public enum TYPE
    {
        [XmlEnum("")]
        Unknow,
        [XmlEnum(nameof(RANDOM))]
        RANDOM,
        [XmlEnum(nameof(SPECIFIC))]
        SPECIFIC,
        [XmlEnum(nameof(RANDOM_SPECIFIC))]
        RANDOM_SPECIFIC
    }

    [XmlRoot(ElementName = "TreasureBox")]
    class L1TreasureBox
    {
        private static readonly ILogger _log = Logger.GetLogger(nameof(L1TreasureBox));

        private static readonly IDictionary<int, L1TreasureBox> _dataMap = MapFactory.NewMap<int, L1TreasureBox>();

        const int chanegDenominator = 10000;

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

        [XmlElement(ElementName = "ItemId")]
        public int BoxId { get; set; }

        [XmlElement(ElementName = "Type")]
        public TYPE Type { get; set; }

        [XmlElement(ElementName = "Item")]
        public List<Item> Items { get; set; }

        public int TotalChance { get; } = 100;

        private bool Init()
        {
            double totalChance = 0d;
            foreach (Item each in Items)
            {
                if (ItemTable.Instance.getTemplate(each.ItemId) == null)
                {
                    _log.Warning("item ID " + each.ItemId + " is not found。");
                    return false;
                }
                totalChance += each.Chance;
            }

            if (totalChance != 0 && totalChance != 100d)
            {
                _log.Warning("ID " + BoxId + " 的總機率不等於100%。");
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void load()
        {
            System.Console.Write("【讀取】 【TreasureBox】【設定】");
            const string PATH = "./data/xml/Item/TreasureBox.xml";
            FileInfo fileInfo = new FileInfo(PATH);
            if (fileInfo.Exists)
            {
                Stopwatch timer = Stopwatch.StartNew();
                TreasureBoxList treasureBoxList;
                try
                {
                    IXmlDeserialize xmlDeserialize = Container.Instance.Resolve<IXmlDeserialize>();
                    treasureBoxList = xmlDeserialize.Deserialize<TreasureBoxList>(File.ReadAllText(fileInfo.FullName));
                }
                catch (Exception e)
                {
                    _log.Error(e);
                    return;
                }

                foreach (L1TreasureBox each in treasureBoxList.Items)
                {
                    if (each.Init())
                    {
                        _dataMap[each.BoxId] = each;
                    }
                }

                timer.Stop();

                System.Console.WriteLine("【完成】【" + timer.ElapsedMilliseconds + "】【毫秒】。");
            }
            else
            {
                System.Console.Write($"【讀取】 【TreasureBox】【設定】{fileInfo} 檔案不存在");
            }
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