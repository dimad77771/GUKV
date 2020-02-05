using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// Wraps the "DefReportsConfig" custom section in the Web.Config file
/// </summary>
public class DefReportsConfig : ConfigurationSection
{
	public DefReportsConfig()
	{
	}

    public static DefReportsConfig GetConfig()
    {
        return ConfigurationManager.GetSection("customConfig/defaultReports") as DefReportsConfig;
    }

    [ConfigurationProperty("Folders")]
    public DefUserFolderCollection Folders
    {
        get
        {
            return this["Folders"] as DefUserFolderCollection;
        }
    }
}