using LineageServer.Extensions;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Xml;

namespace LineageServer.Server.Model.Npc.Action
{
	abstract class L1NpcXmlAction : INpcAction
	{
		private string _name;

		private readonly int[] _npcIds;

		private readonly IntRange _level;

		private readonly int _questId;

		private readonly int _questStep;

		private readonly int[] _classes;

		public L1NpcXmlAction(XmlElement xmlElement)
		{
			_name = xmlElement.GetString("Name");
			_npcIds = parseNpcIds(xmlElement.GetString("NpcId"));
			_level = parseLevel(xmlElement);
			_questId = L1NpcXmlParser.parseQuestId(xmlElement.GetString("QuestId"));
			_questStep = L1NpcXmlParser.parseQuestStep(xmlElement.GetString("QuestStep"));

			_classes = parseClasses(xmlElement);
		}

		private int[] parseClasses(XmlNode xmlNode)
		{
			string classes = xmlNode.GetString("Class").ToUpper();
			int[] result = new int[classes.Length];
			int idx = 0;
			for (int i = 0; i < classes.Length; i++)
			{
				result[idx++] = _charTypes[classes[i]];
			}
			Array.Sort(result);
			return result;
		}

		private IntRange parseLevel(XmlNode xmlNode)
		{
			int level = xmlNode.GetInt("Level");
			int min = xmlNode.GetInt("LevelMin", 1);
			int max = xmlNode.GetInt("LevelMax", ExpTable.MAX_LEVEL);
			return level == 0 ? new IntRange(min, max) : new IntRange(level, level);
		}

		private static readonly IDictionary<char, int> _charTypes = MapFactory.NewMap<char, int>();
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
			if (string.IsNullOrEmpty(npcIds))
			{
				return new int[0];
			}
			StringTokenizer tok = new StringTokenizer(npcIds, ",");
			int[] result = new int[tok.countTokens()];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = int.Parse(tok.nextToken().Trim());
			}
			Array.Sort(result);
			return result;
		}

		private bool acceptsNpcId(GameObject obj)
		{
			if (0 < _npcIds.Length)
			{
				if (!( obj is L1NpcInstance ))
				{
					return false;
				}
				int npcId = ( (L1NpcInstance)obj ).NpcTemplate.get_npcId();

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

		public virtual bool AcceptsRequest(string actionName, L1PcInstance pc, GameObject obj)
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
		public virtual L1NpcHtml Execute(string actionName, L1PcInstance pc, GameObject obj, byte[] args)
		{
			return L1NpcHtml.HTML_CLOSE;
		}
		public virtual L1NpcHtml ExecuteWithAmount(string actionName, L1PcInstance pc, GameObject obj, int amount)
		{
			return L1NpcHtml.HTML_CLOSE;
		}
	}

}