using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Gameplay
{
    public static class EnumerableExtensions
    {
        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            
            var list = enumerable as IList<T> ?? enumerable.ToList(); 
            return list.Count == 0 ? default(T) : list[UnityEngine.Random.Range(0,list.Count)];
        }

        public static T[] RandomRange<T>(this IEnumerable<T> enumerable, int range)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            
            var list = enumerable as IList<T> ?? enumerable.ToList(); 
            var randList = list.OrderBy(_ => UnityEngine.Random.Range(0,list.Count())).Take(Mathf.Min(range, list.Count))
                .ToArray();
            return randList;
        }
    }
}