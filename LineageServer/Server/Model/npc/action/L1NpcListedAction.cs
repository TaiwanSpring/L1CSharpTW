using System.Collections.Generic;
namespace LineageServer.Server.Model.Npc.Action
{

	using Element = org.w3c.dom.Element;
	using GameObject = LineageServer.Server.Model.GameObject;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using L1NpcHtml = LineageServer.Server.Model.Npc.L1NpcHtml;

	public class L1NpcListedAction : L1NpcXmlAction
	{
		private IList<INpcAction> _actions;

		public L1NpcListedAction(Element element) : base(element)
		{
			_actions = L1NpcXmlParser.listActions(element);
		}

		public override L1NpcHtml execute(string actionName, L1PcInstance pc, GameObject obj, sbyte[] args)
		{
			L1NpcHtml result = null;
			foreach (INpcAction action in _actions)
			{
				if (!action.acceptsRequest(actionName, pc, obj))
				{
					continue;
				}
				L1NpcHtml r = action.execute(actionName, pc, obj, args);
				if (r != null)
				{
					result = r;
				}
			}
			return result;
		}

		public override L1NpcHtml executeWithAmount(string actionName, L1PcInstance pc, GameObject obj, int amount)
		{
			L1NpcHtml result = null;
			foreach (INpcAction action in _actions)
			{
				if (!action.acceptsRequest(actionName, pc, obj))
				{
					continue;
				}
				L1NpcHtml r = action.executeWithAmount(actionName, pc, obj, amount);
				if (r != null)
				{
					result = r;
				}
			}
			return result;
		}
	}

}