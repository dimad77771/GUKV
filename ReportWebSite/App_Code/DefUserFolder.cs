using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// Wraps a configuration element in Web.Config file that describes a user-defined folder
/// </summary>
public class DefUserFolder : ConfigurationElement
{
    public DefUserFolder()
	{
	}

    [ConfigurationProperty("name", IsRequired = true)]
    public string Name
    {
        get
        {
            return this["name"] as string;
        }
    }

    [ConfigurationProperty("Reports")]
    public DefUserReportCollection Reports
    {
        get
        {
            return this["Reports"] as DefUserReportCollection;
        }
    }
}