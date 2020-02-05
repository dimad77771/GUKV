using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data.Common;
using StackExchange.Profiling.Data;
using StackExchange.Profiling;
using System.Web.UI;
using System.Collections;
using System.Reflection;
using System.Data;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Text;

namespace MiniProfilerHelpers
{
   public class ProfiledSqlDataSource : Cache.CachingDataSource
    {
        protected override DbProviderFactory GetDbProviderFactory()
        {
            // Return a ProfiledDbProviderFactory from MiniProfiler
            // that wraps the "base" DbProviderFactory
            return new ProfiledDbProviderFactory(base.GetDbProviderFactory());
        }
    }
}