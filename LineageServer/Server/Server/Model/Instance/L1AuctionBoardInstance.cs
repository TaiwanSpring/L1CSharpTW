using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Templates;
using System;
namespace LineageServer.Server.Server.Model.Instance
{
	[Serializable]
	class L1AuctionBoardInstance : L1NpcInstance
	{
		public L1AuctionBoardInstance(L1Npc template) : base(template)
		{
		}

		public override void onAction(L1PcInstance pc)
		{
			pc.sendPackets(new S_AuctionBoard(this));
		}
	}
}