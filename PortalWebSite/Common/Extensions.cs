using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalWebSite.Common
{
    public static class Extensions
    {
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,
            TValue defaultValue = default(TValue))
        {
            TValue result;
            if (!dict.TryGetValue(key, out result))
                return defaultValue;
            return result;
        }
    }
}
