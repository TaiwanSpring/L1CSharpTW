using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Command.Executors
{
    /// <summary>
    /// GM指令：castgfxid
    /// </summary>
    class L1CastGfx : ILineageCommand
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