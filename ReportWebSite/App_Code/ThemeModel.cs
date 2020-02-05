using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

public class ThemeModel : ThemeModelBase
{
    string _spriteCssClass;

    [XmlAttribute]
    public string SpriteCssClass
    {
        get
        {
            if (_spriteCssClass == null)
                return "";
            return _spriteCssClass;
        }
        set { _spriteCssClass = value; }
    }
}
