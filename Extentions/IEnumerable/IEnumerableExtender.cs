using System;
using System.Collections.Generic;
using System.Linq;

namespace CB.CSharp.Extentions
{
    public static class IEnumerableExtender
    {
        public static IEnumerable<TSource> TakeRandom<TSource>(this IEnumerable<TSource> repo, int count) where TSource : class =>
            repo
            .OrderBy(o => Guid.NewGuid())
            .Take(count);
    }
}