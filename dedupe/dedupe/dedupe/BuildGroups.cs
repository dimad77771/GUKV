using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Itg.Utility.Fuzzy;

namespace dedupe
{
    class BuildGroups
    {
        private int AdaptDistanse(int distance)
        {
            if (distance == int.MinValue)
            {
                return int.MaxValue;
            }
            else {
                return Math.Abs(distance);
            }
        }
        public BuildGroups()
        {
            Items = new List<BuildGroup>();
        }
        public BuildGroup FindByName(int? streetId, string buildName, string normalizedAddress, int maxDistance = 0)
        {
            var result = new BuildGroup();
            foreach (var item in Items)
            {
                if (//полные совпадения по абсолютным значениям не включаем в группы
                    (maxDistance == 0 && item.BuildName != buildName && normalizedAddress == item.NormalizedBuild) ||

                    (item.StreetId == streetId && item.BuildName != buildName) &&
                    
                    (
                    !LevenshteinDistanceReference.IsDistanceTooLarge(normalizedAddress, item.NormalizedBuild, maxDistance) &&
                    AdaptDistanse(LevenshteinDistanceReference.CalculateDistance(buildName, item.BuildName, maxDistance)) <= maxDistance )
                    )
                {
                    result = item;
                    break;
                }
            }
            return result;                
        }
        public List<BuildGroup> Items { get; set; }
    }
}
