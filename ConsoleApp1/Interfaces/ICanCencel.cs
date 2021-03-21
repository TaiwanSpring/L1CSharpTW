using System;

namespace ConsoleApp1.Interfaces
{
    public interface ICanCencel
    {
        event Action<ICanCencel> OnCancel;

        void Cancel();
    }
}
