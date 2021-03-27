using System.Collections.Concurrent;
using System.Collections.Generic;
namespace LineageServer.Utils
{
    public class MapFactory
    {
        public static IDictionary<K, V> NewMap<K, V>()
        {
            return new Dictionary<K, V>();
        }

        public static IDictionary<K, V> NewMap<K, V>(IDictionary<K, V> from)
        {
            return new Dictionary<K, V>(from);
        }

        public static IDictionary<K, V> NewConcurrentMap<K, V>()
        {
            return new ConcurrentDictionary<K, V>();
        }

        public static IDictionary<K, V> NewConcurrentMap<K, V>(IDictionary<K, V> from)
        {
            return new ConcurrentDictionary<K, V>(from);
        }
    }
}