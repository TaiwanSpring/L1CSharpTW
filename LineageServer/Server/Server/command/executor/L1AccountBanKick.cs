﻿using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System;

namespace LineageServer.Server.Server.Command.Executor
{
    /// <summary>
    /// GM指令：踢掉且禁止帳號登入
    /// </summary>
    class L1AccountBanKick : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                L1PcInstance target = L1World.Instance.getPlayer(arg);

                if (target != null)
                {
                    // アカウントをBANする
                    Account.ban(target.AccountName);
                    pc.sendPackets(new S_SystemMessage(target.Name + "被您強制踢除遊戲並封鎖IP"));
                    target.sendPackets(new S_Disconnect());
                }
                else
                {
                    pc.sendPackets(new S_SystemMessage(arg + "不在線上。"));
                }
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入 " + cmdName + " 玩家名稱。"));
            }
        }
    }

}