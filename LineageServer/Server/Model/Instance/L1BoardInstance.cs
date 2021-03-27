using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
namespace LineageServer.Server.Model.Instance
{
    class L1BoardInstance : L1NpcInstance
    {
        public L1BoardInstance(L1Npc template) : base(template)
        {
        }

        public override void onAction(L1PcInstance player)
        {
            player.sendPackets(new S_Board(Id));
        }

        public override void onAction(L1PcInstance player, int number)
        {
            player.sendPackets(new S_Board(Id, number));
        }

        public virtual void onActionRead(L1PcInstance player, int number)
        {
            player.sendPackets(new S_BoardRead(number));
        }
    }

}