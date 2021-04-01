using System.Collections.Generic;
using System.Linq;

namespace LineageServer.Server.Templates
{
    class L1MobGroup
    {
        public IList<L1NpcCount> Minions
        {
            get
            {
                return _minions.ToArray();
            }
        }

        private readonly IList<L1NpcCount> _minions;

        public int Id { get; }

        public int LeaderId { get; }

        public bool RemoveGroupIfLeaderDie { get; }

        public L1MobGroup(int id, int leaderId, IList<L1NpcCount> minions, bool isRemoveGroupIfLeaderDie)
        {
            Id = id;
            LeaderId = leaderId;
            _minions = minions; // 参照コピーの方が速いが、不変性が保証できない
            RemoveGroupIfLeaderDie = isRemoveGroupIfLeaderDie;
        }
    }
}