using System;
using System.Collections.Generic;

/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Server.command.executor
{

	using NpcSpawnTable = LineageServer.Server.Server.datatables.NpcSpawnTable;
	using SpawnTable = LineageServer.Server.Server.datatables.SpawnTable;
	using L1Spawn = LineageServer.Server.Server.Model.L1Spawn;
	using L1Teleport = LineageServer.Server.Server.Model.L1Teleport;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	public class L1ToSpawn : L1CommandExecutor
	{
		private static readonly IDictionary<int, int> _spawnId = Maps.newMap();

		private L1ToSpawn()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1ToSpawn();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
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