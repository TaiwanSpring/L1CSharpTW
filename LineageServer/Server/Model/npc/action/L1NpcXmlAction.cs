using System;
using System.Collections.Generic;
using System.Xml;
namespace LineageServer.Server.Model.Npc.Action
{
	public abstract class L1NpcXmlAction : INpcAction
	{
		private string _name;

		private readonly int[] _npcIds;

		private readonly IntRange _level;

		private readonly int _questId;

		private readonly int _questStep;

		private readonly int[] _classes;

		public L1NpcXmlAction(Element element)
		{
			_name = element.getAttribute("Name");
			_name = _name.Equals("") ? null : _name;
			_npcIds = parseNpcIds(element.getAttribute("NpcId"));
			_level = parseLevel(element);
			_questId = L1NpcXmlParser.parseQuestId(element.getAttribute("QuestId"));
			_questStep = L1NpcXmlParser.parseQuestStep(element.getAttribute("QuestStep"));

			_classes = parseClasses(element);
		}

		private int[] parseClasses(Element element)
		{
			string classes = element.getAttribute("Class").ToUpper();
			int[] result = new int[classes.Length];
			int idx = 0;
			foreach (char? cha in classes.ToCharArray())
			{
				result[idx++] = _charTypes[cha];
			}
			Array.Sort(result);
			return result;
		}

		private IntRange parseLevel(Element element)
		{
			int level = L1NpcXmlParser.getIntAttribute(element, "Level", 0);
			int min = L1NpcXmlParser.getIntAttribute(element, "LevelMin", 1);
			int max = L1NpcXmlParser.getIntAttribute(element, "LevelMax", ExpTable.MAX_LEVEL);
			return level == 0 ? new IntRange(min, max) : new IntRange(level, level);
		}

		private static readonly IDictionary<char, int> _charTypes = Maps.NewMap();
		static L1NpcXmlAction()
		{
			_charTypes['P'] = 0;
			_charTypes['K'] = 1;
			_charTypes['E'] = 2;
			_charTypes['W'] = 3;
			_charTypes['D'] = 4;
			_charTypes['R'] = 5;
			_charTypes['I'] = 6;
		}

		private int[] parseNpcIds(string npcIds)
		{
			StringTokenizer tok = new StringTokenizer(npcIds.Replace(" ", ""), ",");
			int[] result = new int[tok.countTokens()];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = int.Parse(tok.nextToken());
			}
			Array.Sort(result);
			return result;
		}

		private bool acceptsNpcId(GameObject obj)
		{
			if (0 < _npcIds.Length)
			{
				if (!(obj is L1NpcInstance))
				{
					return false;
				}
				int npcId = ((L1NpcInstance) obj).NpcTemplate.get_npcId();

				if (Array.BinarySearch(_npcIds, npcId) < 0)
				{
					return false;
				}
			}
			return true;
		}

		private bool acceptsLevel(int level)
		{
			return _level.includes(level);
		}

		private bool acceptsCharType(int type)
		{
			if (0 < _classes.Length)
			{
				if (Array.BinarySearch(_classes, type) < 0)
				{
					return false;
				}
			}
			return true;
		}

		private bool acceptsActionName(string name)
		{
			if (string.ReferenceEquals(_name, null))
			{
				return true;
			}
			return name.Equals(_name);
		}

		private bool acceptsQuest(L1PcInstance pc)
		{
			if (_questId == -1)
			{
				return true;
			}
			if (_questStep == -1)
			{
				return 0 < pc.Quest.get_step(_questId);
			}
			return pc.Quest.get_step(_questId) == _questStep;
		}

		public virtual bool acceptsRequest(string actionName, L1PcInstance pc, GameObject obj)
		{
			if (!acceptsNpcId(obj))
			{
				return false;
			}
			if (!acceptsLevel(pc.Level))
			{
				return false;
			}
			if (!acceptsQuest(pc))
			{
				return false;
			}
			if (!acceptsCharType(pc.Type))
			{
				return false;
			}
			if (!acceptsActionName(actionName))
			{
				return false;
			}
			return true;
		}

		public override abstract L1NpcHtml execute(string actionName, L1PcInstance pc, GameObject obj, sbyte[] args);

		public virtual L1NpcHtml executeWithAmount(string actionName, L1PcInstance pc, GameObject obj, int amount)
		{
			return null;
		}
	}

}