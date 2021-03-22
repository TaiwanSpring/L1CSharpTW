using LineageServer.Models;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System;
namespace LineageServer.Server.Server.Command.Executor
{
    /// <summary>
    /// GM指令：castgfxid
    /// </summary>
    class L1CastGfx : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                StringTokenizer stringtokenizer = new StringTokenizer(arg);
                int sprid = int.Parse(stringtokenizer.nextToken());

                pc.sendPackets(new S_SkillSound(pc.Id, sprid));
                pc.broadcastPacket(new S_SkillSound(pc.Id, sprid));
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入 " + cmdName + " castgfxid。"));
            }
        }
    }

}