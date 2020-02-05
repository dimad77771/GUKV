using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ExtDataEntry.Services
{
    public class AuthService
    {
        public static bool IsUserAuthenticated
        {
            get
            {
                return HttpContext.Current != null
                    && HttpContext.Current.User != null
                    && HttpContext.Current.User.Identity != null
                    && HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }

        public static Guid CurrentUserID
        { 
            get 
            {
                if (IsUserAuthenticated)
                {
                    try
                    {
                        MembershipUser user = Membership.GetUser();
                        return user == null ? Guid.Empty : (Guid)user.ProviderUserKey;
                    }
                    catch
                    {
                    }
                }
                return Guid.Empty;
            }
        }
    }
}
