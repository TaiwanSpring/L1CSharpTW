using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.Model
{
    class L1UbPattern
    {
        private bool _isFrozen = false;

        private IDictionary<int, IList<L1UbSpawn>> _groups = MapFactory.NewMap<int, IList<L1UbSpawn>>();

        public virtual void addSpawn(int groupNumber, L1UbSpawn spawn)
        {
            if (_isFrozen)
            {
                return;
            }

            IList<L1UbSpawn> spawnList = _groups[groupNumber];
            if (spawnList == null)
            {
                spawnList = ListFactory.NewList<L1UbSpawn>();
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
                if (spawnList is List<L1UbSpawn> list)
                {
                    list.Sort();
                }
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