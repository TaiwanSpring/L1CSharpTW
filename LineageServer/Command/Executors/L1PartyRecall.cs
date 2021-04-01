using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Command.Executors
{
    class L1PartyRecall : ILineageCommand
    {

        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            L1PcInstance target = Container.Instance.Resolve<IGameWorld>().getPlayer(arg);

            if (target != null)
            {
                L1Party party = target.Party;
                if (party != null)
                {
                    int x = pc.X;
                    int y = pc.Y + 2;
                    short map = pc.MapId;
                    L1PcInstance[] players = party.Members;
                    foreach (L1PcInstance pc2 in players)
                    {
                        try
                        {
                            L1Teleport.teleport(pc2, x, y, map, 5, true);
                            pc2.sendPackets(new S_SystemMessage("您被傳喚到GM身邊。"));
                        }
                        catch (Exception e)
                        {
                            pc.sendPackets(new S_SystemMessage(e.Message));
                        }
                    }
                }
                else
                {
                    pc.sendPackets(new S_SystemMessage("請輸入要召喚的角色名稱。"));
                }
            }
            else
            {
                pc.sendPackets(new S_SystemMessage("不在線上。"));
            }
        }
    }

}