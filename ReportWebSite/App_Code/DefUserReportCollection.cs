using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// Wraps a collection of DefUserReport elements in the Web.Config file
/// </summary>
public class DefUserReportCollection : ConfigurationElementCollection
{
	public DefUserReportCollection()
	{
	}

    public DefUserReport this[int index]
    {
        get
        {
            return base.BaseGet(index) as DefUserReport;
        }

        set
        {
            if (base.BaseGet(index) != null)
            {
                base.BaseRemoveAt(index);
            }

            this.BaseAdd(index, value);
        }
    }

    // To get by key value instead of index
    public new DefUserReport this[string key]
    {
        get
        {
            return base.BaseGet(key) as DefUserReport;
        }
    }

    protected override ConfigurationElement CreateNewElement()
    {
        return new DefUserReport();
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
        return ((DefUserReport)element).Name;
    }
}