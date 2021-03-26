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
namespace LineageServer.Server.Server.Model
{

	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using Lists = LineageServer.Server.Server.Utils.collections.Lists;
	using Maps = LineageServer.Server.Server.Utils.collections.Maps;

	public class L1HateList
	{
		private readonly IDictionary<L1Character, int> _hateMap;

		private L1HateList(IDictionary<L1Character, int> hateMap)
		{
			_hateMap = hateMap;
		}

		public L1HateList()
		{
			/*
			 * ConcurrentHashMapを利用するより、 全てのメソッドを同期する方がメモリ使用量、速度共に優れていた。
			 * 但し、今後このクラスの利用方法が変わった場合、 例えば多くのスレッドから同時に読み出しがかかるようになった場合は、
			 * ConcurrentHashMapを利用した方が良いかもしれない。
			 */
			_hateMap = Maps.newMap();
		}

		public virtual void add(L1Character cha, int hate)
		{
			lock (this)
			{
				if (cha == null)
				{
					return;
				}
				if (_hateMap.ContainsKey(cha))
				{
					_hateMap[cha] = _hateMap[cha] + hate;
				}
				else
				{
					_hateMap[cha] = hate;
				}
			}
		}

		public virtual int get(L1Character cha)
		{
			lock (this)
			{
				return _hateMap[cha];
			}
		}

		public virtual bool containsKey(L1Character cha)
		{
			lock (this)
			{
				return _hateMap.ContainsKey(cha);
			}
		}

		public virtual void remove(L1Character cha)
		{
			lock (this)
			{
				_hateMap.Remove(cha);
			}
		}

		public virtual void clear()
		{
			lock (this)
			{
				_hateMap.Clear();
			}
		}

		public virtual bool Empty
		{
			get
			{
				lock (this)
				{
					return _hateMap.Count == 0;
				}
			}
		}

		public virtual L1Character MaxHateCharacter
		{
			get
			{
				lock (this)
				{
					L1Character cha = null;
					int hate = int.MinValue;
            
					foreach (KeyValuePair<L1Character, int> e in _hateMap.SetOfKeyValuePairs())
					{
						if (hate < e.Value)
						{
							cha = e.Key;
							hate = e.Value;
						}
					}
					return cha;
				}
			}
		}

		public virtual void removeInvalidCharacter(L1NpcInstance npc)
		{
			lock (this)
			{
				IList<L1Character> invalidChars = Lists.newList();
				foreach (L1Character cha in _hateMap.Keys)
				{
					if ((cha == null) || cha.Dead || !npc.knownsObject(cha))
					{
						invalidChars.Add(cha);
					}
				}
        
				foreach (L1Character cha in invalidChars)
				{
					_hateMap.Remove(cha);
				}
			}
		}

		public virtual int TotalHate
		{
			get
			{
				lock (this)
				{
					int totalHate = 0;
					foreach (int hate in _hateMap.Values)
					{
						totalHate += hate;
					}
					return totalHate;
				}
			}
		}

		public virtual int TotalLawfulHate
		{
			get
			{
				lock (this)
				{
					int totalHate = 0;
					foreach (KeyValuePair<L1Character, int> e in _hateMap.SetOfKeyValuePairs())
					{
						if (e.Key is L1PcInstance)
						{
							totalHate += e.Value;
						}
					}
					return totalHate;
				}
			}
		}

		public virtual int getPartyHate(L1Party party)
		{
			lock (this)
			{
				int partyHate = 0;
        
				foreach (KeyValuePair<L1Character, int> e in _hateMap.SetOfKeyValuePairs())
				{
					L1PcInstance pc = null;
					if (e.Key is L1PcInstance)
					{
						pc = (L1PcInstance) e.Key;
					}
					if (e.Key is L1NpcInstance)
					{
						L1Character cha = ((L1NpcInstance) e.Key).Master;
						if (cha is L1PcInstance)
						{
							pc = (L1PcInstance) cha;
						}
					}
        
					if ((pc != null) && party.isMember(pc))
					{
						partyHate += e.Value;
					}
				}
				return partyHate;
			}
		}

		public virtual int getPartyLawfulHate(L1Party party)
		{
			lock (this)
			{
				int partyHate = 0;
        
				foreach (KeyValuePair<L1Character, int> e in _hateMap.SetOfKeyValuePairs())
				{
					L1PcInstance pc = null;
					if (e.Key is L1PcInstance)
					{
						pc = (L1PcInstance) e.Key;
					}
        
					if ((pc != null) && party.isMember(pc))
					{
						partyHate += e.Value;
					}
				}
				return partyHate;
			}
		}

		public virtual L1HateList copy()
		{
			lock (this)
			{
				return new L1HateList(new Dictionary<L1Character, int>(_hateMap));
			}
		}

		public virtual ISet<KeyValuePair<L1Character, int>> entrySet()
		{
			lock (this)
			{
				return _hateMap.SetOfKeyValuePairs();
			}
		}

		public virtual List<L1Character> toTargetArrayList()
		{
			lock (this)
			{
				return new List<L1Character>(_hateMap.Keys);
			}
		}

		public virtual List<int> toHateArrayList()
		{
			lock (this)
			{
				return new List<int>(_hateMap.Values);
			}
		}
	}

}