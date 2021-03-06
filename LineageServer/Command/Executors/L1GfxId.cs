using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.Map;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using System;
using LineageServer.Server;

namespace LineageServer.Command.Executors
{
    class L1GfxId : ILineageCommand
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
                    L1Npc l1npc = Container.Instance.Resolve<INpcController>().getTemplate(45001);
                    if (l1npc != null)
                    {
                        string s = l1npc.Impl;

                        //System.Reflection.ConstructorInfo<object> constructor = Type.GetType("l1j.server.server.model.Instance." + s + "Instance").GetConstructors()[0];
                        //object[] aobj = new object[] { l1npc };
                        //L1NpcInstance npc = (L1NpcInstance)constructor.Invoke(aobj);
                        L1NpcInstance npc = new L1NpcInstance(l1npc);
                        npc.Id = Container.Instance.Resolve<IIdFactory>().nextId();
                        npc.GfxId = gfxid + i;
                        npc.TempCharGfx = 0;
                        npc.NameId = "";
                        npc.Map = Container.Instance.Resolve<IWorldMap>().getMap(pc.MapId);
                        npc.X = pc.X + i * 2;
                        npc.Y = pc.Y + i * 2;
                        npc.HomeX = npc.X;
                        npc.HomeY = npc.Y;
                        npc.Heading = 4;

                        Container.Instance.Resolve<IGameWorld>().storeObject(npc);
                        Container.Instance.Resolve<IGameWorld>().addVisibleObject(npc);
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