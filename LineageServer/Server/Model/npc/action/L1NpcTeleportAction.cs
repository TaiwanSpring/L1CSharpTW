using LineageServer.Extensions;
using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System.Xml;
using System;

namespace LineageServer.Server.Model.Npc.Action
{
	class L1NpcTeleportAction : L1NpcXmlAction
	{
		private readonly L1Location _loc;
		private readonly int _heading;
		private readonly int _price;
		private readonly bool _effect;

		public L1NpcTeleportAction(XmlElement xmlElement) : base(xmlElement)
		{
			int x = xmlElement.GetInt("X", -1);
			if (x == -1)
			{
				throw new NullReferenceException(nameof(x));
			}
			int y = xmlElement.GetInt("Y", -1);
			if (y == -1)
			{
				throw new NullReferenceException(nameof(y));
			}
			int mapId = xmlElement.GetInt("Map", -1);
			if (mapId == -1)
			{
				throw new NullReferenceException(nameof(mapId));
			}
			_loc = new L1Location(x, y, mapId);
			_heading = xmlElement.GetInt("Heading", 5);
			_price = xmlElement.GetInt("Price", 0);
			_effect = xmlElement.GetBool("Effect", true);
		}

		public override L1NpcHtml Execute(string actionName, L1PcInstance pc, GameObject obj, byte[] args)
		{
			if (!pc.Inventory.checkItem(L1ItemId.ADENA, _price))
			{
				pc.sendPackets(new S_ServerMessage(337, "$4")); // アデナが不足しています。
				return L1NpcHtml.HTML_CLOSE;
			}
			pc.Inventory.consumeItem(L1ItemId.ADENA, _price);
			L1Teleport.teleport(pc, _loc.X, _loc.Y, (short)_loc.MapId, _heading, _effect);
			return null;
		}

	}

}