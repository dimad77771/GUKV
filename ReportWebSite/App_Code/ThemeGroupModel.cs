using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

public class ThemeGroupModel : ThemeModelBase
{
    List<ThemeModel> _themes = new List<ThemeModel>();

    [XmlElement(ElementName = "Theme")]
    public List<ThemeModel> Themes
    {
        get { return _themes; }
    }
}
