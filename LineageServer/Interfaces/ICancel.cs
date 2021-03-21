using System;

namespace LineageServer.Interfaces
{
    interface ICancel : IRunnable
    {
        event Action<ICancel> Cancel;
        bool IsCancelled { get; }
        void cancel();
    }
}
