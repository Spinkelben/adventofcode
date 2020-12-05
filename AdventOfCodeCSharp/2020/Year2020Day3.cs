using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 3)]
    internal class Year2020Day3 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            return NumberOfTreesHit(input.Where(s => s.Length > 0).ToList(), (right: 3, down: 1))
                .ToString();
        }

        private long NumberOfTreesHit(IList<string> input, (int right, int down) slope)
        {
            return GetCoordSequence(slope, input.Count)
                .Select(c => GetSymbolAt(input, c))
                .Count(c => c == '#');
        }

        public string Part2(IList<string> input)
        {
            var filtered = input
                .Where(i => i.Length > 0)
                .ToList();
            return (NumberOfTreesHit(filtered, (1, 1))
                * NumberOfTreesHit(filtered, (3, 1))
                * NumberOfTreesHit(filtered, (5, 1))
                * NumberOfTreesHit(filtered, (7, 1))
                * NumberOfTreesHit(filtered, (1, 2)))
                .ToString();
        }

        private IEnumerable<(int, int)> GetCoordSequence((int right, int down) slope, int height)
        {
            for ((int x, int y) pos = slope; pos.y < height; pos = (pos.x + slope.right, pos.y + slope.down ))
            {
                yield return pos;
            }
        }

        private char? GetSymbolAt(IList<string> pattern, (int, int) coords)
        {
            var (x, y) = coords;
            if (y >= pattern.Count)
            {
                return null;
            }

            var line = pattern[y];
            return line[x % line.Length];
        }
    }
}
