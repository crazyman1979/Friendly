using System.Collections.Generic;
using System.Linq;

namespace CodeFriendly.Core
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> items) where T : class
        {
            return items.Where(i => i != null);
        }
    }
}