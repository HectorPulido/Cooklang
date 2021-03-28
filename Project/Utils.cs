using System;
using System.Collections.Generic;

namespace CookLang
{
    public static class Utils
    {
        public static void AddRange<T, K>(this Dictionary<T, K> target, Dictionary<T, K> source)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            foreach (var element in source)
            {
                if (target.ContainsKey(element.Key))
                {
                    target[element.Key] = element.Value;
                }
                else
                {
                    target.Add(element.Key, element.Value);
                }
            }
        }
    }
}