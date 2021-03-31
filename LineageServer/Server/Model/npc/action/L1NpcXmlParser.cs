using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Xml;

namespace LineageServer.Server.Model.Npc.Action
{
	class L1NpcXmlParser
	{
		public static IList<INpcAction> listActions(XmlElement element)
		{
			IList<INpcAction> result = ListFactory.NewList<INpcAction>();
			XmlNodeList list = element.ChildNodes;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].NodeType == XmlNodeType.Element
					&& list[i] is XmlElement childElement)
				{
					INpcAction action = L1NpcActionFactory.newAction(childElement);
					if (action != null)
					{
						result.Add(action);
					}
				}
			}
			return result;
		}
		private static readonly IDictionary<string, int> _questIds = MapFactory.NewMap<string, int>();
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
			if (string.IsNullOrEmpty(questId))
			{
				return -1;
			}
			string key = questId.ToLower();
			if (_questIds.ContainsKey(key))
			{
				return _questIds[key];
			}
			else
			{
				return -1;
			}
		}
		public static int parseQuestStep(string questStep)
		{
			if (string.IsNullOrEmpty(questStep))
			{
				return -1;
			}
			if (questStep == "End")
			{
				return L1Quest.QUEST_END;
			}
			return int.Parse(questStep);
		}
	}

}