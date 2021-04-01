using System;

namespace LineageServer.Interfaces
{
    interface ITaskController
    {
        void Stop();
        void Start();
        void execute(Action action);
        void execute(Action action, int delay);
        void execute(ITimerTask task, int delay, int period);
        void execute(IRunnable task);
        void execute(IRunnable task, int delay);
    }
}
