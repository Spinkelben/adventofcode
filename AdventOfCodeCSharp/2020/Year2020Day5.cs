using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 5)]
    internal class Year2020Day5 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            return input
                .Where(i => i.Length > 0)
                .Max(p => ParseBoardingPass(p).id)
                .ToString();
        }

        public string Part2(IList<string> input)
        {
            var parsedBoardingPasses = input
                .Where(s => s.Length > 0)
                .Select(bp => ParseBoardingPass(bp))
                .ToList();

            var minId = parsedBoardingPasses.Min(p => p.id);
            var maxId = parsedBoardingPasses.Max(p => p.id);
            var boardingPassDict = parsedBoardingPasses.ToDictionary(p => p.id);

            for (int i = minId; i <= maxId; i++)
            {
                if (!boardingPassDict.ContainsKey(i) 
                    && boardingPassDict.ContainsKey(i - 1)
                    && boardingPassDict.ContainsKey(i + 1))
                {
                    return $"{i}";
                }
            }

            return "No noarding pass found";
        }

        internal (int row, int column, int id) ParseBoardingPass(string boardingPass)
        {
            var row = GetIndexFromBinaryPartition(0, 127, 'F', 'B', boardingPass[0..7]);
            var column = GetIndexFromBinaryPartition(0, 7, 'L', 'R', boardingPass[7..]);

            return (row, column, row * 8 + column);
        }

        private int GetIndexFromBinaryPartition(int low, int high, char lowSymbol, char highSymbol, string path)
        {
            for (int i = 0; i < path.Length; i++)
            {
                var range = high - low + 1;
                var c = path[i];
                if (c == lowSymbol)
                {   
                    high -= range / 2;
                }
                else if (c == highSymbol)
                {
                    low += range / 2;
                }
            }

            if (low != high)
            {
                throw new InvalidOperationException("Haven't found specific index");
            }

            return low;
        }
    }
}
