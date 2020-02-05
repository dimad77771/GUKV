using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Itg.Utility.Fuzzy
{
    public static class LevenshteinDistanceReference
    {
        /// <returns>
        /// int.MinValue (or any negative value) if the distance is greater than maxErrorCount
        /// </returns>
        public static int CalculateDistance(string text, string pattern, int maxErrorCount)
        {
            // If text lengths difference is greater than the maximum allowed
            // error, we can be pretty sure the actual distance will be no less
            // than the lengths difference.
            if (System.Math.Abs(text.Length - pattern.Length) > maxErrorCount)
                return int.MinValue;

            // Choose the shortest string for the row
            int rowLength, columnLength;
            string rowString, columnString;
            if (text.Length > pattern.Length)
            {
                rowLength = pattern.Length;
                columnLength = text.Length;
                rowString = pattern;
                columnString = text;
            }
            else
            {
                rowLength = text.Length;
                columnLength = pattern.Length;
                rowString = text;
                columnString = pattern;
            }

            int[] matrixColumn = new int[rowLength + 1];

            // Seed the initial values for the first column of the matrix
            for (int index = 0; index < matrixColumn.Length; index++)
                matrixColumn[index] = index;

            for (int columnIndex = 0; columnIndex < columnLength; columnIndex++)
            {
                char columnChar = columnString[columnIndex];

                int prevCellValue = matrixColumn[0];

                // Seed the initial value for the first row of the matrix
                matrixColumn[0] = matrixColumn[0] + 1;

                bool shouldProceed = false;

                for (int rowIndex = 0; rowIndex < rowLength; rowIndex++)
                {
                    char rowChar = rowString[rowIndex];

                    int weight = (columnChar == rowChar ? 0 : 1);

                    int nextCellValue = matrixColumn[rowIndex + 1];

                    matrixColumn[rowIndex + 1] = System.Math.Min(prevCellValue + weight,
                        System.Math.Min(matrixColumn[rowIndex] + 1, matrixColumn[rowIndex + 1] + 1));

                    if (matrixColumn[rowIndex + 1] <= maxErrorCount)
                        shouldProceed = true;

                    prevCellValue = nextCellValue;
                }

                if (!shouldProceed)
                    return int.MinValue;
            }

            // The final value may have exceeded the maximum error threshold
            if (matrixColumn[matrixColumn.Length - 1] <= maxErrorCount)
                return matrixColumn[matrixColumn.Length - 1];
            
            return int.MinValue;
        }

        public static bool IsDistanceTooLarge(string text, string pattern, int maxErrorCount)
        {
            return (System.Math.Abs(text.Length - pattern.Length) > maxErrorCount);
        }
    }
}
