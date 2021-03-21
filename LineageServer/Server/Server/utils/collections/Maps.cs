using System.Collections.Concurrent;
using System.Collections.Generic;
namespace LineageServer.Server.Server.utils.collections
{
	public class Maps
	{
		public static IDictionary<K, V> newMap<K, V>()
		{
			return new Dictionary<K, V>();
		}

		public static IDictionary<K, V> newMap<K, V>(IDictionary<K, V> from)
		{
			return new Dictionary<K, V>(from);
		}

		//public static IDictionary<K, V> newWeakMap<K, V>()
		//{
		//	return new WeakHashMap<K, V>();
		//}

		//public static IDictionary<K, V> newWeakMap<K, V>(IDictionary<K, V> from)
		//{
		//	return new WeakHashMap<K, V>(from);
		//}

		public static IDictionary<K, V> newConcurrentMap<K, V>()
		{
			return new ConcurrentDictionary<K, V>();
		}

		public static IDictionary<K, V> newConcurrentMap<K, V>(IDictionary<K, V> from)
		{
			return new ConcurrentDictionary<K, V>(from);
		}

		//public static IDictionary<K, V> newSerializableMap<K, V>()
		//{
		//	return new SerializableHashMap<K, V>();
		//}

		//public static IDictionary<K, V> newSerializableMap<K, V>(IDictionary<K, V> from)
		//{
		//	return new SerializableHashMap<K, V>(from);
		//}

//		public class SerializableHashMap<K, V> : FastMap<K, V> where K : object where V : object
//		{
//			internal const long serialVersionUID = 1L;

//			public SerializableHashMap() : base()
//			{
//			}

//			public SerializableHashMap(IDictionary<K, V> m) : base(m)
//			{
//			}
//		}

		public static ConcurrentDictionary<K, V> newConcurrentHashMap<K, V>()
		{
			return new ConcurrentDictionary<K, V>();
		}
	}

}