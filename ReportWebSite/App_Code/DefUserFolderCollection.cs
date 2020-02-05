using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// Wraps a collection of DefUserFolder elements in the Web.Config file
/// </summary>
public class DefUserFolderCollection : ConfigurationElementCollection
{
	public DefUserFolderCollection()
	{
	}

    public DefUserFolder this[int index]
    {
        get
        {
            return base.BaseGet(index) as DefUserFolder;
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
    public new DefUserFolder this[string key]
    {
        get
        {
            return base.BaseGet(key) as DefUserFolder;
        }
    }
    
    protected override ConfigurationElement CreateNewElement()
    {
        return new DefUserFolder();
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
        return ((DefUserFolder)element).Name;
    }
}