using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace LineageServer.Utils
{
    public static class ListFactory
    {
        public static IList<E> NewList<E>()
        {
            return new List<E>();
        }
        public static IList<E> NewList<E>(int n)
        {
            return new List<E>(n);
        }
        public static IList<E> NewList<E>(IEnumerable<E> from)
        {
            return new List<E>(from);
        }
        public static IList<E> NewConcurrentList<E>()
        {
            return new ConcurrentList<E>();
        }
        public static IList<E> NewConcurrentList<E>(IList<E> from)
        {
            ConcurrentList<E> result = new ConcurrentList<E>();
            foreach (var item in from)
            {
                result.Add(item);
            }
            return result;
        }
        public static List<E> NewArrayList<E, T1>(ICollection<T1> c) where T1 : E
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
    }

}