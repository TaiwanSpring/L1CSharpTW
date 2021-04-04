using LineageServer.Server.Model.Instance;
using LineageServer.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LineageServer.Server.Model
{
    class L1HateList
    {
        /*
			 * ConcurrentHashMapを利用するより、 全てのメソッドを同期する方がメモリ使用量、速度共に優れていた。
			 * 但し、今後このクラスの利用方法が変わった場合、 例えば多くのスレッドから同時に読み出しがかかるようになった場合は、
			 * ConcurrentHashMapを利用した方が良いかもしれない。
			 */
        private readonly IDictionary<L1Character, int> _hateMap = MapFactory.NewMap<L1Character, int>();

        private L1HateList(IDictionary<L1Character, int> hateMap)
        {
            _hateMap = hateMap;
        }

        public L1HateList()
        {

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

                    foreach (KeyValuePair<L1Character, int> e in _hateMap)
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
                IList<L1Character> invalidChars = ListFactory.NewList<L1Character>();
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
                    foreach (KeyValuePair<L1Character, int> e in _hateMap)
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

                foreach (KeyValuePair<L1Character, int> e in _hateMap)
                {
                    L1PcInstance pc = null;
                    if (e.Key is L1PcInstance)
                    {
                        pc = (L1PcInstance)e.Key;
                    }
                    if (e.Key is L1NpcInstance)
                    {
                        L1Character cha = ((L1NpcInstance)e.Key).Master;
                        if (cha is L1PcInstance)
                        {
                            pc = (L1PcInstance)cha;
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

                foreach (KeyValuePair<L1Character, int> e in _hateMap)
                {
                    L1PcInstance pc = null;
                    if (e.Key is L1PcInstance)
                    {
                        pc = (L1PcInstance)e.Key;
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

        public virtual KeyValuePair<L1Character, int>[] entrySet()
        {
            lock (this)
            {
                return _hateMap.ToArray();
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