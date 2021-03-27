using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.map;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace LineageServer.Server
{
    /// <summary>
    /// 還不知道是衝尛的
    /// </summary>
    public class GMCommandsConfig
    {
        public static IDictionary<string, L1Location> ROOMS { get; } = MapFactory.NewMap<string, L1Location>();

        public static IDictionary<string, IList<L1ItemSetItem>> ITEM_SETS { get; } = MapFactory.NewMap<string, IList<L1ItemSetItem>>();

        public static void Load()
        {
            FileInfo fileInfo = new FileInfo("./data/xml/GmCommands/GMCommands.xml");

            if (fileInfo.Exists)
            {
                string xml = File.ReadAllText(fileInfo.FullName);

                IXmlDeserialize xmlDeserialize = Container.Instance.Resolve<IXmlDeserialize>();

                GMCommands commands = xmlDeserialize.Deserialize<GMCommands>(xml);

                for (int i = 0; i < commands.ItemSetList.ItemSet.Count; i++)
                {
                    ItemSet itemSet = commands.ItemSetList.ItemSet[i];

                    if (!ITEM_SETS.ContainsKey(itemSet.Name))
                    {
                        ITEM_SETS.Add(itemSet.Name, new List<L1ItemSetItem>());
                    }

                    for (int j = 0; j < itemSet.Items.Count; j++)
                    {
                        Item item = itemSet.Items[j];

                        ITEM_SETS[itemSet.Name].Add(new L1ItemSetItem(item.Id, item.Amount, item.Enchant));
                    }
                }

                for (int i = 0; i < commands.RoomList.Rooms.Count; i++)
                {
                    Room room = commands.RoomList.Rooms[i];

                    if (!ROOMS.ContainsKey(room.Name))
                    {
                        ROOMS.Add(room.Name, new L1Location(room.LocX, room.LocY, L1WorldMap.Instance.getMap((short)room.MapId)));
                    }
                }
            }
        }
        [XmlRoot(ElementName = "Item")]
        public class Item
        {

            [XmlAttribute(AttributeName = "Id")]
            public int Id { get; set; }

            [XmlAttribute(AttributeName = "Amount")]
            public int Amount { get; set; }

            [XmlAttribute(AttributeName = "Enchant")]
            public int Enchant { get; set; }
        }

        [XmlRoot(ElementName = "ItemSet")]
        public class ItemSet
        {

            [XmlElement(ElementName = "Item")]
            public List<Item> Items { get; set; }

            [XmlAttribute(AttributeName = "Name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "ItemSetList")]
        public class ItemSetList
        {

            [XmlElement(ElementName = "ItemSet")]
            public List<ItemSet> ItemSet { get; set; }
        }

        [XmlRoot(ElementName = "Room")]
        public class Room
        {

            [XmlAttribute(AttributeName = "Name")]
            public string Name { get; set; }

            [XmlAttribute(AttributeName = "LocX")]
            public int LocX { get; set; }

            [XmlAttribute(AttributeName = "LocY")]
            public int LocY { get; set; }

            [XmlAttribute(AttributeName = "MapId")]
            public int MapId { get; set; }
        }

        [XmlRoot(ElementName = "RoomList")]
        public class RoomList
        {

            [XmlElement(ElementName = "Room")]
            public List<Room> Rooms { get; set; }
        }

        [XmlRoot(ElementName = "GMCommands")]
        public class GMCommands
        {

            [XmlElement(ElementName = "ItemSetList")]
            public ItemSetList ItemSetList { get; set; }

            [XmlElement(ElementName = "RoomList")]
            public RoomList RoomList { get; set; }
        }
    }
}