namespace LineageServer.Interfaces
{
    interface IGameActionProvider
    {
        int getDefaultAttack(int gfxid);
        int getSpecialAttack(int gfxid);
        int getRangedAttack(int gfxid);
        int getStatus(int gfxid);
    }
}
