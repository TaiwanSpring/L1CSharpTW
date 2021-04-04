using LineageServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace LineageServer
{
    class Container : IContainerAdapter
    {
        readonly Dictionary<Type, object> mapping = new Dictionary<Type, object>();
        public static IContainerAdapter Instance { get; } = new Container();

        public bool CanResolve<T>()
        {
            if (typeof(T) is IDbConnection)
            {
                return this.func != null;
            }
            return this.mapping.ContainsKey(typeof(T));
        }
        //不得已先這樣做
        Func<IDbConnection> func;
        public void SetDbConnection(Func<IDbConnection> func)
        { this.func = func; }

        public void Register<TTo, TFrom>()
        {

        }

        public void RegisterInstance<T>(T instance)
        {
            Type type = typeof(T);
            if (!this.mapping.ContainsKey(type))
            {
                this.mapping.Add(type, instance);
            }
        }
        public T Resolve<T>()
        {
            //if (typeof(T) == typeof(IDbConnection))
            //{
            //    return (T)this.func.Invoke();
            //}
            Type type = typeof(T);
            if (this.mapping.ContainsKey(type))
            {
                return (T)this.mapping[type];
            }
            else
            {
                return default(T);
            }
        }
    }
}
