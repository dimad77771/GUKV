using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeocodingService
{
    public enum StatusCode
    {
        Queued = 0,
        Success = 1,
        Ambiguous = 2,
        NoMatch = 3,
        Failure = 4,
    }
}
