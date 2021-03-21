using ConsoleApp1.Interfaces;
using System;

namespace ConsoleApp1.Model
{
    abstract class WorkerBase : RunnableBase, ICanCencel
    {
        public event Action<ICanCencel> OnCancel;

        public virtual void Cancel()
        {
            if (OnCancel != null)
            {
                OnCancel.Invoke(this);
            }
        }
    }
}
