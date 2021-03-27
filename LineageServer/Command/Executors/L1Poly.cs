using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Command.Executors
{
    class L1Poly : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                StringTokenizer st = new StringTokenizer(arg);
                string name = st.nextToken();
                int polyid = int.Parse(st.nextToken());

                L1PcInstance tg = L1World.Instance.getPlayer(name);

                if (tg == null)
                {
                    pc.sendPackets(new S_ServerMessage(73, name)); // \f1%0はゲームをしていません。
                }
                else
                {
                    try
                    {
                        L1PolyMorph.doPoly(tg, polyid, 7200, L1PolyMorph.MORPH_BY_GM);
                    }
                    catch (Exception)
                    {
                        pc.sendPackets(new S_SystemMessage("請輸入 .poly 玩家名稱 變身代碼。"));
                    }
                }
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage(cmdName + " 請輸入  玩家名稱 變身代碼。"));
            }
        }
    }

}