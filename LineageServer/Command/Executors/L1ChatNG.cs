using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;
using System;

namespace LineageServer.Command.Executors
{
    /// <summary>
    /// GM指令：禁言
    /// </summary>
    class L1ChatNG : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                StringTokenizer st = new StringTokenizer(arg);
                string name = st.nextToken();
                int time = int.Parse(st.nextToken());

                L1PcInstance tg = L1World.Instance.getPlayer(name);

                if (tg != null)
                {
                    tg.setSkillEffect(L1SkillId.STATUS_CHAT_PROHIBITED, time * 60 * 1000);
                    tg.sendPackets(new S_SkillIconGFX(36, time * 60));
                    tg.sendPackets(new S_ServerMessage(286, time.ToString())); // \f3ゲームに適合しない行動であるため、今後%0分間チャットを禁じます。
                    pc.sendPackets(new S_ServerMessage(287, name)); // %0のチャットを禁じました。
                }
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入 " + cmdName + " 玩家名稱 時間(分)。"));
            }
        }
    }

}