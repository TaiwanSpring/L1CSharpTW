using System.Collections.Generic;
using System.Linq;

namespace LineageServer.Server.Server.Utils.collections
{
    public static class Lists
    {
        public static IList<E> newList<E>()
        {
            return new List<E>();
        }

        public static IList<E> newList<E>(int n)
        {
            return new List<E>(n);
        }

        public static IList<E> newList<E>(ICollection<E> from)
        {
            return new List<E>(from);
        }

        public static IList<E> newList<E>(ISet<E> from)
        {
            return new List<E>(from);
        }

        public static IList<E> newConcurrentList<E>()
        {
            return new System.Collections.Concurrent.ConcurrentList<E>();
        }

        public static IList<E> newConcurrentList<E>(IList<E> from)
        {
            System.Collections.Concurrent.ConcurrentList<E> result = new System.Collections.Concurrent.ConcurrentList<E>();
            foreach (var item in from)
            {
                result.Add(item);
            }
            return result;
        }

        //public static IList<E> newSerializableList<E>()
        //{
        //	return new SerializableArrayList<E>();
        //}

        //public static IList<E> newSerializableList<E>(int n)
        //{
        //	return new SerializableArrayList<E>(n);
        //}

        public static List<E> newArrayList<E>()
        {
            return new List<E>();
        }

        public static List<E> newArrayList<E, T1>(ICollection<T1> c) where T1 : E
        {
            return new List<E>(c.Cast<E>());
        }
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = RandomHelper.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        //public class SerializableArrayList<E> : FastTable<E> where E : object
        //{
        //	internal const long serialVersionUID = 1L;

        //	public SerializableArrayList() : base()
        //	{
        //	}

        //	public SerializableArrayList(int capacity) : base(capacity)
        //	{
        //	}
        //}
    }

}