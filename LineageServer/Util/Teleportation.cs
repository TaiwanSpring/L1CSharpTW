using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.Map;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;
using System.Collections.Generic;
namespace LineageServer.Utils
{
    class Teleportation
    {
        private Teleportation()
        {
        }

        public static void actionTeleportation(L1PcInstance pc)
        {
            if (pc.Dead || pc.Teleport)
            {
                return;
            }

            int x = pc.TeleportX;
            int y = pc.TeleportY;
            short mapId = pc.TeleportMapId;

            int head = pc.TeleportHeading;

            L1Map map = Container.Instance.Resolve<IWorldMap>().getMap(mapId);

            if (!map.isInMap(x, y) && !pc.Gm)
            {
                x = pc.X;
                y = pc.Y;
                mapId = pc.MapId;
            }

            pc.Teleport = true;

            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final l1j.server.server.model.L1Clan clan = l1j.server.server.model.L1World.getInstance().getClan(pc.getClanname());
            L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
            if (clan != null)
            {
                if (clan.WarehouseUsingChar == pc.Id)
                { // 自キャラがクラン倉庫使用中
                    clan.WarehouseUsingChar = 0; // クラン倉庫のロックを解除
                }
            }

            Container.Instance.Resolve<IGameWorld>().moveVisibleObject(pc, mapId);
            pc.setLocation(x, y, mapId);
            pc.Heading = head;
            pc.sendPackets(new S_MapID(pc.MapId, pc.Map.Underwater));

            if (pc.ReserveGhost)
            { // ゴースト状態解除
                pc.endGhost();
            }
            if (pc.Ghost || pc.GmInvis)
            {
            }
            else if (pc.Invisble)
            {
                pc.broadcastPacketForFindInvis(new S_OtherCharPacks(pc, true), true);
            }
            else
            {
                pc.broadcastPacket(new S_OtherCharPacks(pc));
            }
            pc.sendPackets(new S_OwnCharPack(pc));

            pc.removeAllKnownObjects();
            pc.sendVisualEffectAtTeleport(); // クラウン、毒、水中等の視覚効果を表示
            pc.updateObject();
            // spr番号6310, 5641の変身中にテレポートするとテレポート後に移動できなくなる
            // 武器を着脱すると移動できるようになるため、S_CharVisualUpdateを送信する
            pc.sendPackets(new S_CharVisualUpdate(pc));

            pc.killSkillEffectTimer(L1SkillId.MEDITATION);
            pc.CallClanId = 0; // コールクランを唱えた後に移動すると召喚無効

            /*
			 * subjects ペットとサモンのテレポート先画面内へ居たプレイヤー。
			 * 各ペット毎にUpdateObjectを行う方がコード上ではスマートだが、
			 * ネットワーク負荷が大きくなる為、一旦Setへ格納して最後にまとめてUpdateObjectする。
			 */
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final java.util.HashSet<l1j.server.server.model.Instance.L1PcInstance> subjects = new java.util.HashSet<l1j.server.server.model.Instance.L1PcInstance>();
            HashSet<L1PcInstance> subjects = new HashSet<L1PcInstance>();
            subjects.Add(pc);

            if (!pc.Ghost)
            {
                if (pc.Map.TakePets)
                {
                    // ペットとサモンも一緒に移動させる。
                    foreach (L1NpcInstance petNpc in pc.PetList.Values)
                    {
                        // テレポート先の設定
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final l1j.server.server.model.L1Location loc = pc.getLocation().randomLocation(3, false);
                        L1Location loc = pc.Location.randomLocation(3, false);
                        int nx = loc.X;
                        int ny = loc.Y;
                        if ((pc.MapId == 5125) || (pc.MapId == 5131) || (pc.MapId == 5132) || (pc.MapId == 5133) || (pc.MapId == 5134))
                        { // ペットマッチ会場
                            nx = 32799 + RandomHelper.Next(5) - 3;
                            ny = 32864 + RandomHelper.Next(5) - 3;
                        }
                        teleport(petNpc, nx, ny, mapId, head);
                        if (petNpc is L1SummonInstance)
                        { // サモンモンスター
                          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                          //ORIGINAL LINE: final l1j.server.server.model.Instance.L1SummonInstance summon = (l1j.server.server.model.Instance.L1SummonInstance) petNpc;
                            L1SummonInstance summon = (L1SummonInstance)petNpc;
                            pc.sendPackets(new S_SummonPack(summon, pc));
                        }
                        else if (petNpc is L1PetInstance)
                        { // ペット
                          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                          //ORIGINAL LINE: final l1j.server.server.model.Instance.L1PetInstance pet = (l1j.server.server.model.Instance.L1PetInstance) petNpc;
                            L1PetInstance pet = (L1PetInstance)petNpc;
                            pc.sendPackets(new S_PetPack(pet, pc));
                        }

                        foreach (L1PcInstance visiblePc in Container.Instance.Resolve<IGameWorld>().getVisiblePlayer(petNpc))
                        {
                            // テレポート元と先に同じPCが居た場合、正しく更新されない為、一度removeする。
                            visiblePc.removeKnownObject(petNpc);
                            subjects.Add(visiblePc);
                        }
                    }

                    // マジックドールも一緒に移動させる。
                    foreach (L1DollInstance doll in pc.DollList.Values)
                    {
                        // テレポート先の設定
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final l1j.server.server.model.L1Location loc = pc.getLocation().randomLocation(3, false);
                        L1Location loc = pc.Location.randomLocation(3, false);
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final int nx = loc.getX();
                        int nx = loc.X;
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final int ny = loc.getY();
                        int ny = loc.Y;

                        teleport(doll, nx, ny, mapId, head);
                        pc.sendPackets(new S_DollPack(doll));

                        foreach (L1PcInstance visiblePc in Container.Instance.Resolve<IGameWorld>().getVisiblePlayer(doll))
                        {
                            // テレポート元と先に同じPCが居た場合、正しく更新されない為、一度removeする。
                            visiblePc.removeKnownObject(doll);
                            subjects.Add(visiblePc);
                        }
                    }
                }
                else
                {
                    foreach (L1DollInstance doll in pc.DollList.Values)
                    {
                        // テレポート先の設定
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final l1j.server.server.model.L1Location loc = pc.getLocation().randomLocation(3, false);
                        L1Location loc = pc.Location.randomLocation(3, false);
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final int nx = loc.getX();
                        int nx = loc.X;
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final int ny = loc.getY();
                        int ny = loc.Y;

                        teleport(doll, nx, ny, mapId, head);
                        pc.sendPackets(new S_DollPack(doll));

                        foreach (L1PcInstance visiblePc in Container.Instance.Resolve<IGameWorld>().getVisiblePlayer(doll))
                        {
                            // テレポート元と先に同じPCが居た場合、正しく更新されない為、一度removeする。
                            visiblePc.removeKnownObject(doll);
                            subjects.Add(visiblePc);
                        }
                    }
                }
            }

            foreach (L1PcInstance updatePc in subjects)
            {
                updatePc.updateObject();
            }

            pc.Teleport = false;

            if (pc.hasSkillEffect(L1SkillId.WIND_SHACKLE))
            {
                pc.sendPackets(new S_SkillIconWindShackle(pc.Id, pc.getSkillEffectTimeSec(L1SkillId.WIND_SHACKLE)));
            }

            // 副本編號與副本地圖不符
            if (pc.PortalNumber != -1 && (pc.MapId != (1005 + pc.PortalNumber)))
            {
                L1DragonSlayer.Instance.removePlayer(pc, pc.PortalNumber);
                pc.PortalNumber = -1;
            }
            // 離開旅館地圖，旅館鑰匙歸零
            if (pc.MapId <= 10000 && pc.InnKeyId != 0)
            {
                pc.InnKeyId = 0;
            }
        }

        private static void teleport(L1NpcInstance npc, int x, int y, short map, int head)
        {
            Container.Instance.Resolve<IGameWorld>().moveVisibleObject(npc, map);
            Container.Instance.Resolve<IWorldMap>().getMap(npc.MapId).setPassable(npc.X, npc.Y, true);
            npc.X = x;
            npc.Y = y;
            npc.MapId = map;
            npc.Heading = head;
            Container.Instance.Resolve<IWorldMap>().getMap(npc.MapId).setPassable(npc.X, npc.Y, false);
        }

    }

}