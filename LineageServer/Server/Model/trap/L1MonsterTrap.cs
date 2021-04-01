using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.Map;
using LineageServer.Server.Storage;
using LineageServer.Server.Templates;
using LineageServer.Server.Types;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.Model.trap
{
	class L1MonsterTrap : L1Trap
	{
		private readonly int _npcId;

		private readonly int _count;

		private L1Npc _npcTemp = null; // パフォーマンスのためにキャッシュ

		public L1MonsterTrap(ITrapStorage storage) : base(storage)
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
			IList<Point> result = ListFactory.NewList<Point>();
			L1Map m = loc.getMap();
			int x = loc.X;
			int y = loc.Y;
			// locを中心に、1辺dタイルの正方形を描くPointリストを作る
			for (int i = 0; i < d; i++)
			{
				addListIfPassable(result, m, new Point(d - i + x, i + y));
				addListIfPassable(result, m, new Point(-( d - i ) + x, -i + y));
				addListIfPassable(result, m, new Point(-i + x, d - i + y));
				addListIfPassable(result, m, new Point(i + x, -( d - i ) + y));
			}
			return result;
		}
		private L1NpcInstance createNpc()
		{
			if (_npcTemp == null)
			{
				_npcTemp = Container.Instance.Resolve<INpcController>().getTemplate(_npcId);
			}

			return L1NpcInstance.Factory(_npcTemp);
		}
		private void spawn(L1Location loc)
		{
			L1NpcInstance npc = createNpc();
			npc.Id = Container.Instance.Resolve<IIdFactory>().nextId();
			npc.Location.set(loc);
			npc.HomeX = loc.X;
			npc.HomeY = loc.Y;
			Container.Instance.Resolve<IGameWorld>().storeObject(npc);
			Container.Instance.Resolve<IGameWorld>().addVisibleObject(npc);

			npc.onNpcAI();
			npc.turnOnOffLight();
			npc.startChat(L1NpcInstance.CHAT_TIMING_APPEARANCE); // チャット開始
		}

		public override void onTrod(L1PcInstance trodFrom, GameObject trapObj)
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
						spawn(new L1Location(pt, trapObj.MapId));
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
				throw e;
			}
		}
	}

}