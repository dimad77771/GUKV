using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("Themes")]
public class ThemesModel
{
    static ThemesModel _current;
    static readonly object _currentLock = new object();

    public static ThemesModel Current
    {
        get
        {
            lock (_currentLock)
            {
                if (_current == null)
                {
                    using (Stream stream = File.OpenRead(HttpContext.Current.Server.MapPath("~/App_Data/Themes.xml")))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ThemesModel));
                        _current = (ThemesModel)serializer.Deserialize(stream);
                    }
                }
                return _current;
            }
        }
    }

    List<ThemeGroupModel> _groups = new List<ThemeGroupModel>();

    [XmlElement("ThemeGroup")]
    public List<ThemeGroupModel> Groups
    {
        get { return _groups; }
    }
}
