using LineageServer.Server.Model.Instance;
using System.Collections.Generic;
using System.Xml;

namespace LineageServer.Server.Model.Npc.Action
{
	class L1NpcListedAction : L1NpcXmlAction
	{
		private IList<INpcAction> _actions;

		public L1NpcListedAction(XmlElement xmlElement) : base(xmlElement)
		{
			_actions = L1NpcXmlParser.listActions(xmlElement);
		}

		public override L1NpcHtml Execute(string actionName, L1PcInstance pc, GameObject obj, byte[] args)
		{
			L1NpcHtml result = null;
			foreach (INpcAction action in _actions)
			{
				if (!action.AcceptsRequest(actionName, pc, obj))
				{
					continue;
				}
				L1NpcHtml r = action.Execute(actionName, pc, obj, args);
				if (r != null)
				{
					result = r;
				}
			}
			return result;
		}

		public override L1NpcHtml ExecuteWithAmount(string actionName, L1PcInstance pc, GameObject obj, int amount)
		{
			L1NpcHtml result = null;
			foreach (INpcAction action in _actions)
			{
				if (!action.AcceptsRequest(actionName, pc, obj))
				{
					continue;
				}
				L1NpcHtml r = action.ExecuteWithAmount(actionName, pc, obj, amount);
				if (r != null)
				{
					result = r;
				}
			}
			return result;
		}
	}

}