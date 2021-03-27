using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server;
using LineageServer.Server.DataSources;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
using System.Threading;
namespace LineageServer.Utils
{
    class L1SpawnUtil
    {
        private static ILogger _log = Logger.GetLogger(nameof(L1SpawnUtil));

        public static void spawn(L1PcInstance pc, int npcId, int randomRange, int timeMillisToDelete)
        {
            try
            {
                L1NpcInstance npc = NpcTable.Instance.newNpcInstance(npcId);
                npc.Id = IdFactory.Instance.nextId();
                npc.MapId = pc.MapId;
                if (randomRange == 0)
                {
                    npc.Location.set(pc.Location);
                    npc.Location.forward(pc.Heading);
                }
                else
                {
                    int tryCount = 0;
                    do
                    {
                        tryCount++;
                        npc.X = pc.X + RandomHelper.Next(randomRange) - RandomHelper.Next(randomRange);
                        npc.Y = pc.Y + RandomHelper.Next(randomRange) - RandomHelper.Next(randomRange);
                        if (npc.Map.isInMap(npc.Location) && npc.Map.isPassable(npc.Location))
                        {
                            break;
                        }
                        Thread.Sleep(1);
                    } while (tryCount < 50);

                    if (tryCount >= 50)
                    {
                        npc.Location.set(pc.Location);
                        npc.Location.forward(pc.Heading);
                    }
                }

                npc.HomeX = npc.X;
                npc.HomeY = npc.Y;
                npc.Heading = pc.Heading;
                // 紀錄龍之門扉編號
                if (npc.NpcId == 81273)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (!L1DragonSlayer.Instance.PortalNumber[i])
                        {
                            L1DragonSlayer.Instance.setPortalNumber(i, true);
                            // 重置副本
                            L1DragonSlayer.Instance.resetDragonSlayer(i);
                            npc.PortalNumber = i;
                            L1DragonSlayer.Instance.portalPack()[i] = npc;
                            break;
                        }
                    }
                }
                else if (npc.NpcId == 81274)
                {
                    for (int i = 6; i < 12; i++)
                    {
                        if (!L1DragonSlayer.Instance.PortalNumber[i])
                        {
                            L1DragonSlayer.Instance.setPortalNumber(i, true);
                            // 重置副本
                            L1DragonSlayer.Instance.resetDragonSlayer(i);
                            npc.PortalNumber = i;
                            L1DragonSlayer.Instance.portalPack()[i] = npc;
                            break;
                        }
                    }
                }
                L1World.Instance.storeObject(npc);
                L1World.Instance.addVisibleObject(npc);

                if (npc.TempCharGfx == 7548 || npc.TempCharGfx == 7550 || npc.TempCharGfx == 7552 || npc.TempCharGfx == 7554 || npc.TempCharGfx == 7585 || npc.TempCharGfx == 7591)
                {
                    npc.broadcastPacket(new S_NPCPack(npc));
                    npc.broadcastPacket(new S_DoActionGFX(npc.Id, ActionCodes.ACTION_AxeWalk));
                }
                else if (npc.TempCharGfx == 7539 || npc.TempCharGfx == 7557 || npc.TempCharGfx == 7558 || npc.TempCharGfx == 7864 || npc.TempCharGfx == 7869 || npc.TempCharGfx == 7870)
                {
                    foreach (L1PcInstance _pc in L1World.Instance.getVisiblePlayer(npc, 50))
                    {
                        if (npc.TempCharGfx == 7539)
                        {
                            _pc.sendPackets(new S_ServerMessage(1570));
                        }
                        else if (npc.TempCharGfx == 7864)
                        {
                            _pc.sendPackets(new S_ServerMessage(1657));
                        }
                        npc.onPerceive(_pc);
                        S_DoActionGFX gfx = new S_DoActionGFX(npc.Id, ActionCodes.ACTION_AxeWalk);
                        _pc.sendPackets(gfx);
                    }
                    npc.npcSleepTime(ActionCodes.ACTION_AxeWalk, L1NpcInstance.ATTACK_SPEED);
                }
                else if (npc.TempCharGfx == 145)
                { // 史巴托
                    npc.Status = 11;
                    npc.broadcastPacket(new S_NPCPack(npc));
                    npc.broadcastPacket(new S_DoActionGFX(npc.Id, ActionCodes.ACTION_Appear));
                    npc.Status = 0;
                    npc.broadcastPacket(new S_CharVisualUpdate(npc, npc.Status));
                }

                npc.turnOnOffLight();
                npc.startChat(L1NpcInstance.CHAT_TIMING_APPEARANCE); // チャット開始
                if (0 < timeMillisToDelete)
                {
                    L1NpcDeleteTimer timer = new L1NpcDeleteTimer(npc, timeMillisToDelete);
                    timer.begin();
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
        }
    }

}