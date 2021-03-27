using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Server.Model.Instance
{
    class L1SignboardInstance : L1NpcInstance
    {
        public L1SignboardInstance(L1Npc template) : base(template)
        {
        }

        public override void onAction(L1PcInstance pc)
        {
        }

        public override void onPerceive(L1PcInstance perceivedFrom)
        {
            perceivedFrom.addKnownObject(this);
            perceivedFrom.sendPackets(new S_SignboardPack(this));
        }
    }

}