using LineageServer.Extensions;
using LineageServer.Server.Model.Instance;
using System.Xml;

namespace LineageServer.Server.Model.Npc.Action
{
	class L1NpcSetQuestAction : L1NpcXmlAction
	{
		private readonly int _id;
		private readonly int _step;

		public L1NpcSetQuestAction(XmlElement xmlElement) : base(xmlElement)
		{

			_id = L1NpcXmlParser.parseQuestId(xmlElement.GetString("Id"));
			_step = L1NpcXmlParser.parseQuestStep(xmlElement.GetString("Step"));

			if (_id == -1 || _step == -1)
			{
				throw new System.ArgumentException();
			}
		}

		public override L1NpcHtml Execute(string actionName, L1PcInstance pc, GameObject obj, byte[] args)
		{
			pc.Quest.set_step(_id, _step);
			return null;
		}

	}

}