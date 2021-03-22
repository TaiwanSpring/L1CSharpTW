using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Interfaces
{
    interface IContainerAdapter
    {
        void RegisterInstance<T>(T instance);
        bool CanResolve<T>();
        T Resolve<T>();
    }
}
