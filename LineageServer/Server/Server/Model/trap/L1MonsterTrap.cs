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
namespace LineageServer.Server.Server.Model.trap
{

	using IdFactory = LineageServer.Server.Server.IdFactory;
	using NpcTable = LineageServer.Server.Server.DataSources.NpcTable;
	using L1Location = LineageServer.Server.Server.Model.L1Location;
	using L1Object = LineageServer.Server.Server.Model.L1Object;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1Map = LineageServer.Server.Server.Model.map.L1Map;
	using TrapStorage = LineageServer.Server.Server.storage.TrapStorage;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;
	using Point = LineageServer.Server.Server.Types.Point;
	using Lists = LineageServer.Server.Server.Utils.collections.Lists;

	public class L1MonsterTrap : L1Trap
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1MonsterTrap).FullName);

		private readonly int _npcId;

		private readonly int _count;

		private L1Npc _npcTemp = null; // パフォーマンスのためにキャッシュ

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: private java.lang.reflect.Constructor<?> _constructor = null;
		private System.Reflection.ConstructorInfo<object> _constructor = null; // パフォーマンスのためにキャッシュ

		public L1MonsterTrap(TrapStorage storage) : base(storage)
		{

			_npcId = storage.getInt("monsterNpcId");
			_count = storage.getInt("monsterCount");
		}

		private void addListIfPassable(IList<Point> list, L1Map map, Point pt)
		{
			if (map.isPassable(pt))
			{
				list.Add(pt);
			}
		}

		private IList<Point> getSpawnablePoints(L1Location loc, int d)
		{
			IList<Point> result = Lists.newList();
			L1Map m = loc.getMap();
			int x = loc.X;
			int y = loc.Y;
			// locを中心に、1辺dタイルの正方形を描くPointリストを作る
			for (int i = 0; i < d; i++)
			{
				addListIfPassable(result, m, new Point(d - i + x, i + y));
				addListIfPassable(result, m, new Point(-(d - i) + x, -i + y));
				addListIfPassable(result, m, new Point(-i + x, d - i + y));
				addListIfPassable(result, m, new Point(i + x, -(d - i) + y));
			}
			return result;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private java.lang.reflect.Constructor<?> getConstructor(l1j.server.server.templates.L1Npc npc) throws ClassNotFoundException
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
		private System.Reflection.ConstructorInfo<object> getConstructor(L1Npc npc)
		{
			return Type.GetType("l1j.server.server.model.Instance." + npc.Impl + "Instance").GetConstructors()[0];
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private l1j.server.server.model.Instance.L1NpcInstance createNpc() throws Exception
		private L1NpcInstance createNpc()
		{
			if (_npcTemp == null)
			{
				_npcTemp = NpcTable.Instance.getTemplate(_npcId);
			}
			if (_constructor == null)
			{
				_constructor = getConstructor(_npcTemp);
			}

			return (L1NpcInstance) _constructor.Invoke(new object[] {_npcTemp});
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private void spawn(l1j.server.server.model.L1Location loc) throws Exception
		private void spawn(L1Location loc)
		{
			L1NpcInstance npc = createNpc();
			npc.Id = IdFactory.Instance.nextId();
			npc.Location.set(loc);
			npc.HomeX = loc.X;
			npc.HomeY = loc.Y;
			L1World.Instance.storeObject(npc);
			L1World.Instance.addVisibleObject(npc);

			npc.onNpcAI();
			npc.turnOnOffLight();
			npc.startChat(L1NpcInstance.CHAT_TIMING_APPEARANCE); // チャット開始
		}

		public override void onTrod(L1PcInstance trodFrom, L1Object trapObj)
		{
			sendEffect(trapObj);

			IList<Point> points = getSpawnablePoints(trapObj.Location, 5);

			// 沸ける場所が無ければ終了
			if (points.Count == 0)
			{
				return;
			}

			try
			{
				int cnt = 0;
				while (true)
				{
					foreach (Point pt in points)
					{
						spawn(new L1Location(pt, trapObj.getMap()));
						cnt++;
						if (_count <= cnt)
						{
							return;
						}
					}
				}
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
		}
	}

}