using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selama.Classes.Utility
{
    public class Util
    {
        public static List<TResult> ConvertLists<TIn, TResult>(IEnumerable<TIn> src, Func<TIn, TResult> ctor)
        {
            List<TResult> dest = new List<TResult>();

            foreach (TIn item in src)
            {
                dest.Add(ctor(item));
            }

            return dest;
        }
    }
}