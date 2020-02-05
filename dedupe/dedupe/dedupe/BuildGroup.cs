using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dedupe
{
    class BuildGroup
    {
        public BuildGroup()
        {
            BuildsInGroup = new List<BuildingFull>();
        }
        public int GroupId { get; set; }
        public int? StreetId { get; set; }
        public string StreetName { get; set; }
        public string NormalizedBuild { get; set; }
        public string BuildName { get; set; }
        public DateTime? LastModifiedDate { get; set; } 
        
        public List<BuildingFull> BuildsInGroup { get; set; }
        public bool IsEmpty() {
            return !BuildsInGroup.Any();
        }
    }
}
