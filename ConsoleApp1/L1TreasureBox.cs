/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace ConsoleApp1
{
	[XmlRoot(ElementName = nameof(Item))]
	public class Item
	{
		[XmlAttribute(AttributeName = nameof(ItemId))]
		public string ItemId { get; set; }
		[XmlAttribute(AttributeName = nameof(Count))]
		public string Count { get; set; }
		[XmlAttribute(AttributeName = nameof(Chance))]
		public double Chance { get; set; }
		[XmlAttribute(AttributeName = nameof(Enchant))]
		public string Enchant { get; set; }
		[XmlAttribute(AttributeName = nameof(Attr))]
		public string Attr { get; set; }
		[XmlAttribute(AttributeName = nameof(Identi))]
		public string Identi { get; set; }
		[XmlAttribute(AttributeName = nameof(Bless))]
		public string Bless { get; set; }
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
	public class L1TreasureBox
	{
		[XmlElement(ElementName = nameof(Item))]
		public List<Item> Item { get; set; }
		[XmlAttribute(AttributeName = nameof(Type))]
		public TYPE Type { get; set; }
		[XmlAttribute(AttributeName = "ItemId")]
		public int BoxId { get; set; }
	}

	[XmlRoot(ElementName = nameof(TreasureBoxList))]
	public class TreasureBoxList
	{
		[XmlElement(ElementName = nameof(TreasureBox))]
		public List<L1TreasureBox> TreasureBox { get; set; }
	}

}
