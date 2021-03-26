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
namespace LineageServer.Server.Server
{


	using L1Location = LineageServer.Server.Server.Model.L1Location;
	using L1ItemSetItem = LineageServer.Server.Server.Templates.L1ItemSetItem;
	using IterableElementList = LineageServer.Server.Server.Utils.IterableElementList;
	using Lists = LineageServer.Server.Server.Utils.collections.Lists;
	using Maps = LineageServer.Server.Server.Utils.collections.Maps;

	using Document = org.w3c.dom.Document;
	using Element = org.w3c.dom.Element;
	using NodeList = org.w3c.dom.NodeList;
	using SAXException = org.xml.sax.SAXException;

	public class GMCommandsConfig
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(GMCommandsConfig).FullName);

		private interface ConfigLoader
		{
			void load(Element element);
		}

		private abstract class ListLoaderAdapter : ConfigLoader
		{
			private readonly GMCommandsConfig outerInstance;

			internal readonly string _listName;

			public ListLoaderAdapter(GMCommandsConfig outerInstance, string listName)
			{
				this.outerInstance = outerInstance;
				_listName = listName;
			}

			public void load(Element element)
			{
				NodeList nodes = element.ChildNodes;
				foreach (Element elem in new IterableElementList(nodes))
				{
					if (elem.NodeName.equalsIgnoreCase(_listName))
					{
						loadElement(elem);
					}
				}
			}

			public abstract void loadElement(Element element);
		}

		private class RoomLoader : ListLoaderAdapter
		{
			private readonly GMCommandsConfig outerInstance;

			public RoomLoader(GMCommandsConfig outerInstance) : base(outerInstance, "Room")
			{
				this.outerInstance = outerInstance;
			}

			public override void loadElement(Element element)
			{
				string name = element.getAttribute("Name");
				int locX = Convert.ToInt32(element.getAttribute("LocX"));
				int locY = Convert.ToInt32(element.getAttribute("LocY"));
				int mapId = Convert.ToInt32(element.getAttribute("MapId"));
				ROOMS[name.ToLower()] = new L1Location(locX, locY, mapId);
			}
		}

		private class ItemSetLoader : ListLoaderAdapter
		{
			private readonly GMCommandsConfig outerInstance;

			public ItemSetLoader(GMCommandsConfig outerInstance) : base(outerInstance, "ItemSet")
			{
				this.outerInstance = outerInstance;
			}

			public virtual L1ItemSetItem loadItem(Element element)
			{
				int id = Convert.ToInt32(element.getAttribute("Id"));
				int amount = Convert.ToInt32(element.getAttribute("Amount"));
				int enchant = Convert.ToInt32(element.getAttribute("Enchant"));
				return new L1ItemSetItem(id, amount, enchant);
			}

			public override void loadElement(Element element)
			{
				IList<L1ItemSetItem> list = Lists.newList();
				NodeList nodes = element.ChildNodes;
				foreach (Element elem in new IterableElementList(nodes))
				{
					if (elem.NodeName.equalsIgnoreCase("Item"))
					{
						list.Add(loadItem(elem));
					}
				}
				string name = element.getAttribute("Name");
				ITEM_SETS[name.ToLower()] = list;
			}
		}

		private static IDictionary<string, ConfigLoader> _loaders = Maps.newMap();
		static GMCommandsConfig()
		{
			GMCommandsConfig instance = new GMCommandsConfig();
			_loaders["roomlist"] = new LineageServer.Server.Server.GMCommandsConfig.RoomLoader(instance);
			_loaders["itemsetlist"] = new LineageServer.Server.Server.GMCommandsConfig.ItemSetLoader(instance);
		}

		public static IDictionary<string, L1Location> ROOMS = Maps.newMap();

		public static IDictionary<string, IList<L1ItemSetItem>> ITEM_SETS = Maps.newMap();

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private static org.w3c.dom.Document loadXml(String file) throws javax.xml.parsers.ParserConfigurationException, org.xml.sax.SAXException, java.io.IOException
		private static Document loadXml(string file)
		{
			DocumentBuilder builder = DocumentBuilderFactory.newInstance().newDocumentBuilder();
			return builder.parse(file);
		}

		public static void load()
		{
			try
			{
				Document doc = loadXml("./data/xml/GmCommands/GMCommands.xml");
				NodeList nodes = doc.DocumentElement.ChildNodes;
				for (int i = 0; i < nodes.Length; i++)
				{
					ConfigLoader loader = _loaders[nodes.item(i).NodeName.ToLower()];
					if (loader != null)
					{
						loader.load((Element) nodes.item(i));
					}
				}
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, "讀取 GMCommands.xml 失敗", e);
			}
		}
	}

}