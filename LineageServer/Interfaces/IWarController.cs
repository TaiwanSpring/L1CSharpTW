using LineageServer.Server.Model.Instance;

namespace LineageServer.Interfaces
{
    interface IWarController
    {
        bool isNowWar(int castle_id);
        void checkCastleWar(L1PcInstance player);
    }
}
