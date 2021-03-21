using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server
{
    interface IPacketOutput
    {
        void SendPacket(ServerBasePacket packet);
    }
}