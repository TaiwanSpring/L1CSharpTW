using LineageServer.Serverpackets;

namespace LineageServer.Server
{
    interface IPacketOutput
    {
        void SendPacket(ServerBasePacket packet);
    }
}