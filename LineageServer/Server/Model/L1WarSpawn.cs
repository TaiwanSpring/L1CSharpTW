using System;
using System.Text;

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
namespace LineageServer.Server.Model
{

	using IdFactory = LineageServer.Server.IdFactory;
	using NpcTable = LineageServer.Server.DataTables.NpcTable;
	using L1NpcInstance = LineageServer.Server.Model.Instance.L1NpcInstance;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using S_NPCPack = LineageServer.Serverpackets.S_NPCPack;
	using L1Npc = LineageServer.Server.Templates.L1Npc;

	// Referenced classes of package l1j.server.server.model:
	// L1WarSpawn

	public class L1WarSpawn
	{
		private static L1WarSpawn _instance;

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: private java.lang.reflect.Constructor<?> _constructor;
		private System.Reflection.ConstructorInfo<object> _constructor;

		public L1WarSpawn()
		{
		}

		public static L1WarSpawn Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new L1WarSpawn();
				}
				return _instance;
			}
		}

		public virtual void SpawnTower(int castleId)
		{
			int npcId = 81111;
			if (castleId == L1CastleLocation.ADEN_CASTLE_ID)
			{
				npcId = 81189;
			}
			L1Npc l1npc = Container.Instance.Resolve<INpcController>().getTemplate(npcId); // ガーディアンタワー
			int[] loc = new int[3];
			loc = L1CastleLocation.getTowerLoc(castleId);
			SpawnWarObject(l1npc, loc[0], loc[1], (short)(loc[2]));
			if (castleId == L1CastleLocation.ADEN_CASTLE_ID)
			{
				spawnSubTower();
			}
		}

		private void spawnSubTower()
		{
			L1Npc l1npc;
			int[] loc = new int[3];
			for (int i = 1; i <= 4; i++)
			{
				l1npc = Container.Instance.Resolve<INpcController>().getTemplate(81189 + i); // サブタワー
				loc = L1CastleLocation.getSubTowerLoc(i);
				SpawnWarObject(l1npc, loc[0], loc[1], (short)(loc[2]));
			}
		}

		public virtual void SpawnCrown(int castleId)
		{
			L1Npc l1npc = Container.Instance.Resolve<INpcController>().getTemplate(81125); // クラウン
			int[] loc = new int[3];
			loc = L1CastleLocation.getTowerLoc(castleId);
			SpawnWarObject(l1npc, loc[0], loc[1], (short)(loc[2]));
		}

		public virtual void SpawnFlag(int castleId)
		{
			L1Npc l1npc = Container.Instance.Resolve<INpcController>().getTemplate(81122); // 旗
			int[] loc = new int[5];
			loc = L1CastleLocation.getWarArea(castleId);
			int x = 0;
			int y = 0;
			int locx1 = loc[0];
			int locx2 = loc[1];
			int locy1 = loc[2];
			int locy2 = loc[3];
			short mapid = (short) loc[4];

			for (x = locx1, y = locy1; x <= locx2; x += 8)
			{
				SpawnWarObject(l1npc, x, y, mapid);
			}
			for (x = locx2, y = locy1; y <= locy2; y += 8)
			{
				SpawnWarObject(l1npc, x, y, mapid);
			}
			for (x = locx2, y = locy2; x >= locx1; x -= 8)
			{
				SpawnWarObject(l1npc, x, y, mapid);
			}
			for (x = locx1, y = locy2; y >= locy1; y -= 8)
			{
				SpawnWarObject(l1npc, x, y, mapid);
			}
		}

		private void SpawnWarObject(L1Npc l1npc, int locx, int locy, short mapid)
		{
			try
			{
				if (l1npc != null)
				{
					string s = l1npc.Impl;
					_constructor = Type.GetType((new StringBuilder()).Append("l1j.server.server.model.Instance.").Append(s).Append("Instance").ToString()).GetConstructors()[0];
					object[] aobj = new object[] {l1npc};
					L1NpcInstance npc = (L1NpcInstance) _constructor.Invoke(aobj);
					npc.Id = Container.Instance.Resolve<IIdFactory>().nextId();
					npc.X = locx;
					npc.Y = locy;
					npc.HomeX = locx;
					npc.HomeY = locy;
					npc.Heading = 0;
					npc.Map = mapid;
					Container.Instance.Resolve<IGameWorld>().storeObject(npc);
					Container.Instance.Resolve<IGameWorld>().addVisibleObject(npc);

					foreach (L1PcInstance pc in Container.Instance.Resolve<IGameWorld>().AllPlayers)
					{
						npc.addKnownObject(pc);
						pc.addKnownObject(npc);
						pc.sendPackets(new S_NPCPack(npc));
						pc.broadcastPacket(new S_NPCPack(npc));
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}

}