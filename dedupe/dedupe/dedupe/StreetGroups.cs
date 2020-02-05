using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Itg.Utility.Fuzzy;

namespace dedupe
{
    class StreetGroups
    {
        private int AdaptDistanse(int distance)
        {
            if (distance == int.MinValue)
            {
                return int.MaxValue;
            }
            else
            {
                return Math.Abs(distance);
            }
        }
        public StreetGroups()
        {
            Items = new List<StreetGroup>();
        }
        public StreetGroup FindByName(string name, int maxDistance = 0)
        {
            var result = new StreetGroup();
            foreach (var item in Items)
            {
                //TODO добавить нечетое сравнение
                if (item.GroupName == name ||
                    !LevenshteinDistanceReference.IsDistanceTooLarge(name, item.GroupName, maxDistance) &&
                    AdaptDistanse(LevenshteinDistanceReference.CalculateDistance(name, item.GroupName, maxDistance)) <= maxDistance)
                {
                    result = item;
                    break;
                }
            }
            return result;
        }
        public List<StreetGroup> Items { get; set; }
    }
}
