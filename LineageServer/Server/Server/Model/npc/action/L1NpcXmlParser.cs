using System;
using System.Collections.Generic;
namespace LineageServer.Server.Server.Model.Npc.Action
{

	using L1Quest = LineageServer.Server.Server.Model.L1Quest;
	using IterableElementList = LineageServer.Server.Server.Utils.IterableElementList;
	using Lists = LineageServer.Server.Server.Utils.collections.Lists;
	using Maps = LineageServer.Server.Server.Utils.collections.Maps;

	using Element = org.w3c.dom.Element;
	using NodeList = org.w3c.dom.NodeList;

	public class L1NpcXmlParser
	{
		public static IList<INpcAction> listActions(Element element)
		{
			IList<INpcAction> result = Lists.newList<INpcAction>();
			NodeList list = element.ChildNodes;
			foreach (Element elem in new IterableElementList(list))
			{
				INpcAction action = L1NpcActionFactory.newAction(elem);
				if (action != null)
				{
					result.Add(action);
				}
			}
			return result;
		}

		public static Element getFirstChildElementByTagName(Element element, string tagName)
		{
			IterableElementList list = new IterableElementList(element.getElementsByTagName(tagName));
			foreach (Element elem in list)
			{
				return elem;
			}
			return null;
		}

		public static int getIntAttribute(Element element, string name, int defaultValue)
		{
			int result = defaultValue;
			try
			{
				result = Convert.ToInt32(element.getAttribute(name));
			}
			catch (System.FormatException)
			{
			}
			return result;
		}

		public static bool getBoolAttribute(Element element, string name, bool defaultValue)
		{
			bool result = defaultValue;
			string value = element.getAttribute(name);
			if (!value.Equals(""))
			{
				result = Convert.ToBoolean(value);
			}
			return result;
		}

		private static readonly IDictionary<string, int> _questIds = Maps.newMap();
		static L1NpcXmlParser()
		{
			_questIds["level15"] = L1Quest.QUEST_LEVEL15;
			_questIds["level30"] = L1Quest.QUEST_LEVEL30;
			_questIds["level45"] = L1Quest.QUEST_LEVEL45;
			_questIds["level50"] = L1Quest.QUEST_LEVEL50;
			_questIds["lyra"] = L1Quest.QUEST_LYRA;
			_questIds["oilskinmant"] = L1Quest.QUEST_OILSKINMANT;
			_questIds["doromond"] = L1Quest.QUEST_DOROMOND;
			_questIds["ruba"] = L1Quest.QUEST_RUBA;
			_questIds["lukein"] = L1Quest.QUEST_LUKEIN1;
			_questIds["tbox1"] = L1Quest.QUEST_TBOX1;
			_questIds["tbox2"] = L1Quest.QUEST_TBOX2;
			_questIds["tbox3"] = L1Quest.QUEST_TBOX3;
			_questIds["cadmus"] = L1Quest.QUEST_CADMUS;
			_questIds["resta"] = L1Quest.QUEST_RESTA;
			_questIds["kamyla"] = L1Quest.QUEST_KAMYLA;
			_questIds["lizard"] = L1Quest.QUEST_LIZARD;
			_questIds["desire"] = L1Quest.QUEST_DESIRE;
			_questIds["shadows"] = L1Quest.QUEST_SHADOWS;
			_questIds["toscroll"] = L1Quest.QUEST_TOSCROLL;
			_questIds["moonoflongbow"] = L1Quest.QUEST_MOONOFLONGBOW;
			_questIds["Generalhamelofresentment"] = L1Quest.QUEST_GENERALHAMELOFRESENTMENT;
		}

		public static int parseQuestId(string questId)
		{
			if (questId.Equals(""))
			{
				return -1;
			}
			int? result = _questIds[questId.ToLower()];
			if (result == null)
			{
				throw new System.ArgumentException();
			}
			return result.Value;
		}

		public static int parseQuestStep(string questStep)
		{
			if (questStep.Equals(""))
			{
				return -1;
			}
			if (questStep.Equals("End", StringComparison.OrdinalIgnoreCase))
			{
				return L1Quest.QUEST_END;
			}
			return int.Parse(questStep);
		}
	}

}