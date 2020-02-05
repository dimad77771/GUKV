using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// Wraps a configuration element in Web.Config file that describes a user-defined report
/// </summary>
public class DefUserReport : ConfigurationElement
{
	public DefUserReport()
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

    [ConfigurationProperty("layout", IsRequired = true)]
    public string Layout
    {
        get
        {
            return this["layout"] as string;
        }
    }

    [ConfigurationProperty("grid", IsRequired = true)]
    public int GridID
    {
        get
        {
            string gridIdStr = this["grid"] as string;

            return int.Parse(gridIdStr.Trim());
        }
    }
}