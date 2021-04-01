using System;
using System.Collections.Generic;
using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Utils;

namespace LineageServer.Server.Model
{
    class L1MobGroupSpawn : IGameComponent, IMobGroupController
    {
        private IDictionary<int, L1MobGroup> mobGroupMapping;
        private IDictionary<int, L1MobGroup> GetMobGroupMapping()
        {
            IDataSource dataSource =
               Container.Instance.Resolve<IDataSourceFactory>()
               .Factory(Enum.DataSourceTypeEnum.Mobgroup);
            IDictionary<int, L1MobGroup> result = MapFactory.NewMap<int, L1MobGroup>();
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                int mobGroupId = dataSourceRow.getInt(Mobgroup.Column_id);
                bool isRemoveGroup = (dataSourceRow.getBoolean(Mobgroup.Column_remove_group_if_leader_die));
                int leaderId = dataSourceRow.getInt(Mobgroup.Column_leader_id);
                IList<L1NpcCount> minions = ListFactory.NewList<L1NpcCount>();
                for (int j = 1; j <= 7; j++)
                {
                    int id = dataSourceRow.getInt("minion" + j + "_id");
                    int count = dataSourceRow.getInt("minion" + j + "_count");
                    minions.Add(new L1NpcCount(id, count));
                }
                L1MobGroup mobGroup = new L1MobGroup(mobGroupId, leaderId, minions, isRemoveGroup);
                result[mobGroupId] = mobGroup;
            }

            return result;
        }
        public void doSpawn(L1NpcInstance leader, int groupId, bool isRespawnScreen, bool isInitSpawn)
        {
            if (this.mobGroupMapping.ContainsKey(groupId))
            {
                L1MobGroup mobGroup = this.mobGroupMapping[groupId];
                L1NpcInstance mob;
                L1MobGroupInfo mobGroupInfo = new L1MobGroupInfo();

                mobGroupInfo.RemoveGroup = mobGroup.RemoveGroupIfLeaderDie;
                mobGroupInfo.addMember(leader);

                foreach (L1NpcCount minion in mobGroup.Minions)
                {
                    if (minion.Zero)
                    {
                        continue;
                    }
                    for (int i = 0; i < minion.Count; i++)
                    {
                        mob = spawn(leader, minion.Id, isRespawnScreen, isInitSpawn);
                        if (mob != null)
                        {
                            mobGroupInfo.addMember(mob);
                        }
                    }
                }
            }
        }

        private L1NpcInstance spawn(L1NpcInstance leader, int npcId, bool isRespawnScreen, bool isInitSpawn)
        {
            L1NpcInstance mob = null;
            try
            {
                mob = Container.Instance.Resolve<INpcController>().newNpcInstance(npcId);

                mob.Id = Container.Instance.Resolve<IIdFactory>().nextId();

                mob.Heading = leader.Heading;
                mob.MapId = leader.MapId;
                mob.MovementDistance = leader.MovementDistance;
                mob.Rest = leader.Rest;

                mob.X = leader.X + RandomHelper.Next(5) - 2;
                mob.Y = leader.Y + RandomHelper.Next(5) - 2;
                // マップ外、障害物上、画面内沸き不可で画面内にPCがいる場合、リーダーと同じ座標
                if (!canSpawn(mob, isRespawnScreen))
                {
                    mob.X = leader.X;
                    mob.Y = leader.Y;
                }
                mob.HomeX = mob.X;
                mob.HomeY = mob.Y;

                if (mob is L1MonsterInstance)
                {
                    ((L1MonsterInstance)mob).initHideForMinion(leader);
                }

                mob.Spawn = leader.Spawn;
                mob.setreSpawn(leader.ReSpawn);
                mob.SpawnNumber = leader.SpawnNumber;

                if (mob is L1MonsterInstance)
                {
                    if (mob.MapId == 666)
                    {
                        ((L1MonsterInstance)mob).set_storeDroped(true);
                    }
                }

                Container.Instance.Resolve<IGameWorld>().storeObject(mob);
                Container.Instance.Resolve<IGameWorld>().addVisibleObject(mob);

                if (mob is L1MonsterInstance)
                {
                    if (!isInitSpawn && mob.HiddenStatus == 0)
                    {
                        mob.onNpcAI(); // モンスターのＡＩを開始
                    }
                }
                mob.turnOnOffLight();
                mob.startChat(L1NpcInstance.CHAT_TIMING_APPEARANCE); // チャット開始
            }
            catch (Exception e)
            {
                Logger.GenericLogger.Error(e);
            }
            return mob;
        }

        private bool canSpawn(L1NpcInstance mob, bool isRespawnScreen)
        {
            if (mob.Map.isInMap(mob.Location) && mob.Map.isPassable(mob.Location))
            {
                if (isRespawnScreen)
                {
                    return true;
                }
                if (Container.Instance.Resolve<IGameWorld>().getVisiblePlayer(mob).Count == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void Initialize()
        {
            this.mobGroupMapping = GetMobGroupMapping();
        }
    }

}