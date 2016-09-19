using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Selama.Common.ExtensionMethods
{
    public static class IEnumerableExtensionMethods
    {
        public static List<TOut> ToListOfDifferentType<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, TOut> constructor)
        {
            List<TOut> result = new List<TOut>();
            foreach (TIn item in source)
            {
                result.Add(constructor(item));
            }
            return result;
        }

        public async static Task<List<TOut>> ToListOfDifferentType<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, Task<TOut>> constructor)
        {
            List<TOut> result = new List<TOut>();
            foreach (TIn item in source)
            {
                result.Add(await constructor(item));
            }
            return result;
        }
    }
}