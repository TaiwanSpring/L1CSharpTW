using LineageServer.Interfaces;
using LineageServer.Server;
using System;
using System.Collections.Generic;

namespace LineageServer
{
    class Container : IContainerAdapter
    {
        static readonly Dictionary<Type, object> mapping = new Dictionary<Type, object>();
        public static IContainerAdapter Instance { get; } = new Container();

        public bool CanResolve<T>()
        {
            return mapping.ContainsKey(typeof(T));
        }

        public void RegisterInstance<T>(T instance)
        {
            Type type = typeof(T);
            if (!mapping.ContainsKey(type))
            {
                mapping.Add(type, instance);
            }
        }
        public T Resolve<T>()
        {
            Type type = typeof(T);
            if (mapping.ContainsKey(type))
            {
                return (T)mapping[type];
            }
            else
            {
                return default(T);
            }
        }
    }
}
