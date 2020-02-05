using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace PortalWebSite.Common
{
    public class PropertyCopier<T> where T : class, new()
    {
        public static T FromItem(object other) 
        {
            if (other == null)
                return null;

            T result = new T();

            foreach (PropertyInfo dst in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (dst.GetIndexParameters().Length > 0)
                    continue;
                
                PropertyInfo src = other.GetType().GetProperty(dst.Name, dst.PropertyType);
                if (src == null)
                    throw new InvalidOperationException("Property '" + typeof(T).Name + "." + dst.Name + "' has no prototype in " + other.GetType().Name);

                dst.SetValue(result, src.GetValue(other, new object[0]), new object[0]);
            }

            return result;
        }
    }
}
