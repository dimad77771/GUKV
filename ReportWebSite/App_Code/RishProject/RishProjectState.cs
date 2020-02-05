using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[Flags]
public enum RishProjectState
{
    None = 0,
    Intermediate = 1,
    RequresCoverLetter = 2,
    Final = 4,
    Executed = 8,
}
