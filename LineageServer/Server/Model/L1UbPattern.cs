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
namespace LineageServer.Server.Model
{

	using ListFactory = LineageServer.Utils.ListFactory;
	using MapFactory = LineageServer.Utils.MapFactory;

	public class L1UbPattern
	{
		private bool _isFrozen = false;

		private IDictionary<int, IList<L1UbSpawn>> _groups = MapFactory.newMap();

		public virtual void addSpawn(int groupNumber, L1UbSpawn spawn)
		{
			if (_isFrozen)
			{
				return;
			}

			IList<L1UbSpawn> spawnList = _groups[groupNumber];
			if (spawnList == null)
			{
				spawnList = ListFactory.newList();
				_groups[groupNumber] = spawnList;
			}

			spawnList.Add(spawn);
		}

		public virtual void freeze()
		{
			if (_isFrozen)
			{
				return;
			}

			// 格納されているグループのスポーンリストをID順にソート
			foreach (IList<L1UbSpawn> spawnList in _groups.Values)
			{
				spawnList.Sort();
			}

			_isFrozen = true;
		}

		public virtual bool Frozen
		{
			get
			{
				return _isFrozen;
			}
		}

		public virtual IList<L1UbSpawn> getSpawnList(int groupNumber)
		{
			if (!_isFrozen)
			{
				return null;
			}

			return _groups[groupNumber];
		}
	}

}