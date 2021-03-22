using LineageServer.Models;
using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Model.map;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Templates;
using System;
namespace LineageServer.Server.Server.Command.Executor
{
    class L1GfxId : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                StringTokenizer st = new StringTokenizer(arg);
                int gfxid = Convert.ToInt32(st.nextToken(), 10);
                int count = Convert.ToInt32(st.nextToken(), 10);
                for (int i = 0; i < count; i++)
                {
                    L1Npc l1npc = NpcTable.Instance.getTemplate(45001);
                    if (l1npc != null)
                    {
                        string s = l1npc.Impl;

                        //System.Reflection.ConstructorInfo<object> constructor = Type.GetType("l1j.server.server.model.Instance." + s + "Instance").GetConstructors()[0];
                        //object[] aobj = new object[] { l1npc };
                        //L1NpcInstance npc = (L1NpcInstance)constructor.Invoke(aobj);
                        L1NpcInstance npc = new L1NpcInstance(l1npc);
                        npc.Id = IdFactory.Instance.nextId();
                        npc.GfxId = gfxid + i;
                        npc.TempCharGfx = 0;
                        npc.NameId = "";
                        npc.Map = L1WorldMap.Instance.getMap(pc.MapId);
                        npc.X = pc.X + i * 2;
                        npc.Y = pc.Y + i * 2;
                        npc.HomeX = npc.X;
                        npc.HomeY = npc.Y;
                        npc.Heading = 4;

                        L1World.Instance.storeObject(npc);
                        L1World.Instance.addVisibleObject(npc);
                    }
                }
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage(cmdName + " 請輸入  動畫編號  動畫數量  人物ID。"));
            }
        }
    }

}