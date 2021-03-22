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
namespace LineageServer.Server.Server.Model.npc.action
{

	using ExpTable = LineageServer.Server.Server.DataSources.ExpTable;
	using L1Object = LineageServer.Server.Server.Model.L1Object;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1NpcHtml = LineageServer.Server.Server.Model.npc.L1NpcHtml;
	using IntRange = LineageServer.Server.Server.utils.IntRange;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	using Element = org.w3c.dom.Element;

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

		private static readonly IDictionary<char, int> _charTypes = Maps.newMap();
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

		private bool acceptsNpcId(L1Object obj)
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

		public virtual bool acceptsRequest(string actionName, L1PcInstance pc, L1Object obj)
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

		public override abstract L1NpcHtml execute(string actionName, L1PcInstance pc, L1Object obj, sbyte[] args);

		public virtual L1NpcHtml executeWithAmount(string actionName, L1PcInstance pc, L1Object obj, int amount)
		{
			return null;
		}
	}

}