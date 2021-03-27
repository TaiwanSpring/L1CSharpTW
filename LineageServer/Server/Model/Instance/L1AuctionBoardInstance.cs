using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
namespace LineageServer.Server.Model.Instance
{
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