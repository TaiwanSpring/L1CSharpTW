using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Command.Executors
{
    class L1Ress : ILineageCommand
    {
        private L1Ress()
        {

        }

        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                int objid = pc.Id;
                pc.sendPackets(new S_SkillSound(objid, 759));
                pc.broadcastPacket(new S_SkillSound(objid, 759));
                pc.CurrentHp = pc.getMaxHp();
                pc.CurrentMp = pc.getMaxMp();
                foreach (L1PcInstance tg in Container.Instance.Resolve<IGameWorld>().getVisiblePlayer(pc))
                {
                    if ((tg.CurrentHp == 0) && tg.Dead)
                    {
                        tg.sendPackets(new S_SystemMessage("GM給予了重生。"));
                        tg.broadcastPacket(new S_SkillSound(tg.Id, 3944));
                        tg.sendPackets(new S_SkillSound(tg.Id, 3944));
                        // 祝福された 復活スクロールと同じ効果
                        tg.TempID = objid;
                        tg.sendPackets(new S_Message_YN(322, "")); // また復活したいですか？（Y/N）
                    }
                    else
                    {
                        tg.sendPackets(new S_SystemMessage("GM給予了治療。"));
                        tg.broadcastPacket(new S_SkillSound(tg.Id, 832));
                        tg.sendPackets(new S_SkillSound(tg.Id, 832));
                        tg.CurrentHp = tg.getMaxHp();
                        tg.CurrentMp = tg.getMaxMp();
                    }
                }
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage(cmdName + " 指令錯誤"));
            }
        }
    }

}