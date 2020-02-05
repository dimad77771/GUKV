using System;
using System.Collections.Generic;
using System.Text;

namespace Itg.Utility.Fuzzy
{
    public static class DefaultFuzzy
    {
        public static int GetMaxSimilarityDistance(string text, float threshold)
        {
            return (int)(text.Length * threshold);
        }

        public static int GetMaxSimilarityDistance(string text, float threshold, float ieeeError)
        {
            return (int)(text.Length * threshold + ieeeError);
        }

        public static bool IsSimilar(string text, string pattern, float threshold, float ieeeError)
        {
            return (CalculateDistance(text, pattern, GetMaxSimilarityDistance(text, threshold, ieeeError)) >= 0);
        }

        public static bool IsSimilar(string text, string pattern, float threshold)
        {
            return (CalculateDistance(text, pattern, GetMaxSimilarityDistance(text, threshold)) >= 0);
        }

        public static int CalculateDistance(string text, string pattern, int maxErrorCount)
        {
            return LevenshteinDistanceReference.CalculateDistance(text, pattern, maxErrorCount);
        }

        public static bool IsBeyondErrorThreshold(string text, string pattern, float threshold)
        {
            return IsDistanceTooLarge(text, pattern, GetMaxSimilarityDistance(text, threshold));
        }

        public static bool IsDistanceTooLarge(string text, string pattern, int maxErrorCount)
        {
            return LevenshteinDistanceReference.IsDistanceTooLarge(text, pattern, maxErrorCount);
        }

        public static double CalculateSimilarity(string text, string pattern)
        {
            if (text == pattern)
                return 1.0;
            return (text.Length - CalculateDistance(text, pattern, int.MaxValue)) / text.Length;
        }
    }
}
