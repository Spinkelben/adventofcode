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
            return "";
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
