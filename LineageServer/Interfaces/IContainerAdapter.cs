using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LineageServer.Interfaces
{
    public interface IContainerAdapter
    {
        void SetDbConnection(Func<IDbConnection> func);
        void RegisterInstance<T>(T instance);
        void Register<TTo, TFrom>();
        bool CanResolve<T>();
        T Resolve<T>();
    }
}
