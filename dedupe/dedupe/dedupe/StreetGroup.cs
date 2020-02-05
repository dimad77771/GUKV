using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dedupe
{
    class StreetGroup
    {
        public StreetGroup()
        {
            StreetsInGroup = new List<Street>();
        }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<Street> StreetsInGroup { get; set; }
        public bool IsEmpty() {
            return !StreetsInGroup.Any();
        }
    }
}
