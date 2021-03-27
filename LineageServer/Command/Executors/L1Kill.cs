using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Command.Executors
{
    class L1Kill : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                L1PcInstance target = L1World.Instance.getPlayer(arg);

                if (target != null)
                {
                    target.CurrentHp = 0;
                    target.death(null);
                }
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入 : " + cmdName + " 玩家名稱。"));
            }
        }
    }
}