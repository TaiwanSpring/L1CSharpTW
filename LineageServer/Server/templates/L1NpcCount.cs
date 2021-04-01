using System;

namespace LineageServer.Server.Templates
{
    class L1NpcCount
    {
        public int Id { get; }
        public int Count { get; }

        public bool Zero
        {
            get
            {
                return Id == 0 && Count == 0;
            }
        }
        public L1NpcCount(int id, int count)
        {
            Id = id;
            Count = count;
        }

        public static bool operator ==(L1NpcCount me, L1NpcCount other)
        {
            return me.Id == other.Id && me.Count == other.Count;
        }

        public static bool operator !=(L1NpcCount me, L1NpcCount other)
        {
            return !(me == other);
        }

        public override bool Equals(object obj)
        {
            return obj is L1NpcCount count &&
                   Id == count.Id &&
                   Count == count.Count;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Count);
        }
    }
}