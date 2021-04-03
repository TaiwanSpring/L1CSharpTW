using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.Map;
using LineageServer.Server.Model.skill;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using System;
using System.Text;
namespace LineageServer.Server.Model
{
    class L1EffectSpawn
    {
        private static L1EffectSpawn _instance;

        private L1EffectSpawn()
        {
        }

        public static L1EffectSpawn Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new L1EffectSpawn();
                }
                return _instance;
            }
        }

        /// <summary>
        /// エフェクトオブジェクトを生成し設置する
        /// </summary>
        /// <param name="npcId">
        ///            エフェクトNPCのテンプレートID </param>
        /// <param name="time">
        ///            存在時間(ms) </param>
        /// <param name="locX">
        ///            設置する座標X </param>
        /// <param name="locY">
        ///            設置する座標Y </param>
        /// <param name="mapId">
        ///            設置するマップのID </param>
        /// <returns> 生成されたエフェクトオブジェクト </returns>
        public virtual L1EffectInstance spawnEffect(int npcId, int time, int locX, int locY, short mapId)
        {
            return spawnEffect(npcId, time, locX, locY, mapId, null, 0);
        }

        public virtual L1EffectInstance spawnEffect(int npcId, int time, int locX, int locY, short mapId, L1PcInstance user, int skiiId)
        {
            L1Npc template = Container.Instance.Resolve<INpcController>().getTemplate(npcId);

            if (template == null)
            {
                return null;
            }

            if (L1NpcInstance.Factory(template) is L1EffectInstance effect)
            {

                effect.Id = Container.Instance.Resolve<IIdFactory>().nextId();
                effect.GfxId = template.get_gfxid();
                effect.X = locX;
                effect.Y = locY;
                effect.HomeX = locX;
                effect.HomeY = locY;
                effect.Heading = 0;
                effect.MapId = mapId;
                effect.User = user;
                effect.SkillId = skiiId;
                Container.Instance.Resolve<IGameWorld>().storeObject(effect);
                Container.Instance.Resolve<IGameWorld>().addVisibleObject(effect);

                foreach (L1PcInstance pc in Container.Instance.Resolve<IGameWorld>().getRecognizePlayer(effect))
                {
                    effect.addKnownObject(pc);
                    pc.addKnownObject(effect);
                    pc.sendPackets(new S_NPCPack(effect));
                    pc.broadcastPacket(new S_NPCPack(effect));
                }
                L1NpcDeleteTimer timer = new L1NpcDeleteTimer(effect, time);
                timer.begin();
                return effect;
            }
            else
            {
                return null;
            }
        }

        public virtual void doSpawnFireWall(L1Character cha, int targetX, int targetY)
        {
            L1Npc firewall = Container.Instance.Resolve<INpcController>().getTemplate(81157); // ファイアーウォール
            int duration = SkillsTable.Instance.getTemplate(L1SkillId.FIRE_WALL).BuffDuration;

            if (firewall == null)
            {
                throw new System.NullReferenceException("FireWall data not found:npcid=81157");
            }

            L1Character @base = cha;
            for (int i = 0; i < 8; i++)
            {
                int a = @base.targetDirection(targetX, targetY);
                int x = @base.X;
                int y = @base.Y;
                if (a == 1)
                {
                    x++;
                    y--;
                }
                else if (a == 2)
                {
                    x++;
                }
                else if (a == 3)
                {
                    x++;
                    y++;
                }
                else if (a == 4)
                {
                    y++;
                }
                else if (a == 5)
                {
                    x--;
                    y++;
                }
                else if (a == 6)
                {
                    x--;
                }
                else if (a == 7)
                {
                    x--;
                    y--;
                }
                else if (a == 0)
                {
                    y--;
                }
                if (!@base.isAttackPosition(x, y, 1))
                {
                    x = @base.X;
                    y = @base.Y;
                }
                L1Map map = Container.Instance.Resolve<IWorldMap>().getMap(cha.MapId);
                if (!map.isArrowPassable(x, y, cha.Heading))
                {
                    break;
                }

                L1EffectInstance effect = spawnEffect(81157, duration * 1000, x, y, cha.MapId);
                if (effect == null)
                {
                    break;
                }
                foreach (GameObject objects in Container.Instance.Resolve<IGameWorld>().getVisibleObjects(effect, 0))
                {
                    if (objects is L1EffectInstance)
                    {
                        L1EffectInstance npc = (L1EffectInstance)objects;
                        if (npc.NpcTemplate.get_npcId() == 81157)
                        {
                            npc.deleteMe();
                        }
                    }
                }
                if ((targetX == x) && (targetY == y))
                {
                    break;
                }
                @base = effect;
            }

        }
    }

}