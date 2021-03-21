using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System;
namespace LineageServer.Server.Server.Command.Executor
{
    class L1Kill : IL1CommandExecutor
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