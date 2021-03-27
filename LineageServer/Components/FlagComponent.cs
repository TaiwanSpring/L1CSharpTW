using LineageServer.Enum;
using LineageServer.Interfaces;
using System.Threading;

namespace LineageServer.Components
{
    class FlagComponent : IHasFlag
    {
        readonly ReaderWriterLockSlim readerWriterLockSlim;
        private FlagEnum flags = FlagEnum.NoFlag;
        public bool Has(FlagEnum flag)
        {
            this.readerWriterLockSlim.EnterReadLock();
            bool result = (this.flags & flag) == flag;
            this.readerWriterLockSlim.ExitReadLock();
            return result;
        }

        public void Add(FlagEnum flag)
        {
            this.readerWriterLockSlim.EnterWriteLock();
            this.flags = this.flags | flag;
            this.readerWriterLockSlim.ExitWriteLock();
        }

        public void Remove(FlagEnum flag)
        {
            this.readerWriterLockSlim.EnterWriteLock();
            this.flags = this.flags & (~flag);
            this.readerWriterLockSlim.ExitWriteLock();
        }
    }
}
