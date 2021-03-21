﻿using LineageServer.Model;
using LineageServer.Server.Server.datatables;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.utils.collections;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.Server.Command.Executor
{
    class L1ToSpawn : IL1CommandExecutor
    {
        static readonly IDictionary<int, int> _spawnId = Maps.newMap<int, int>();

        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                if (!_spawnId.ContainsKey(pc.Id))
                {
                    _spawnId[pc.Id] = 0;
                }
                int id = _spawnId[pc.Id];
                if (arg.Length == 0 || arg.Equals("+"))
                {
                    id++;
                }
                else if (arg.Equals("-"))
                {
                    id--;
                }
                else
                {
                    StringTokenizer st = new StringTokenizer(arg);
                    id = int.Parse(st.nextToken());
                }
                L1Spawn spawn = NpcSpawnTable.Instance.getTemplate(id);
                if (spawn == null)
                {
                    spawn = SpawnTable.Instance.getTemplate(id);
                }
                if (spawn != null)
                {
                    L1Teleport.teleport(pc, spawn.LocX, spawn.LocY, spawn.MapId, 5, false);
                    pc.sendPackets(new S_SystemMessage("spawnid(" + id + ")已傳送到"));
                }
                else
                {
                    pc.sendPackets(new S_SystemMessage("spawnid(" + id + ")找不到"));
                }
                _spawnId[pc.Id] = id;
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage(cmdName + " spawnid|+|-"));
            }
        }
    }

}