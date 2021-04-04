using LineageServer.Interfaces;
using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_GameTime : ServerBasePacket
    {
        public S_GameTime(int time)
        {
            buildPacket(time);
        }

        public S_GameTime()
        {
            int time = Container.Instance.Resolve<IGameTimeClock>().CurrentTime().Seconds;
            buildPacket(time);
        }

        private void buildPacket(int time)
        {
            WriteC(Opcodes.S_OPCODE_GAMETIME);
            WriteD(time);
        }
    }
}