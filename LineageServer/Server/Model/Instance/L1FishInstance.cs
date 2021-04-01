using LineageServer.Models;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
namespace LineageServer.Server.Model.Instance
{
    class L1FishInstance : L1NpcInstance
    {

        private const long serialVersionUID = 1L;
        private fishTimer _fishTimer;

        public L1FishInstance(L1Npc template) : base(template)
        {
            _fishTimer = new fishTimer(this, this);
            Container.Instance.Resolve<ITaskController>().execute(_fishTimer, 1000, (RandomHelper.Next(30, 30) * 1000));
        }

        public override void onPerceive(L1PcInstance perceivedFrom)
        {
            perceivedFrom.addKnownObject(this);
            perceivedFrom.sendPackets(new S_NPCPack(this));
        }

        private class fishTimer : TimerTask
        {
            private readonly L1FishInstance outerInstance;


            internal L1FishInstance _fish;

            public fishTimer(L1FishInstance outerInstance, L1FishInstance fish)
            {
                this.outerInstance = outerInstance;
                _fish = fish;
            }

            public override void run()
            {
                if (_fish != null)
                {
                    _fish.Heading = RandomHelper.Next(8); // 隨機面向
                    _fish.broadcastPacket(new S_ChangeHeading(_fish)); // 更新面向
                    _fish.broadcastPacket(new S_DoActionGFX(_fish.Id, 0)); // 動作
                }
                else
                {
                    cancel();
                }
            }
        }

    }

}