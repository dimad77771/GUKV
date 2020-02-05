using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeocodingService
{
    public static class Extensions
    {
        public static IEnumerable<T> Distinct<T, A>(this IEnumerable<T> items, Func<T, A> selector)
        {
            return items.GroupBy(selector).Select(x => x.First());
        }
    }
}
