using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 1)]
    internal class Year2020Day1 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            List<int> items = ToInts(input);

            var magicPair = GetPairs(items)
                .Where(pair => pair.Item1 + pair.Item2 == 2020)
                .First();

            return $"{magicPair.Item1 * magicPair.Item2}";
        }

        private static List<int> ToInts(IList<string> input)
        {
            return input
                .Select(s => s.Trim())
                .Where(s => s.Length > 0)
                .Select(s => int.Parse(s))
                .ToList();
        }

        public string Part2(IList<string> input)
        {
            var items = ToInts(input);

            var magicTriplets = GetTriplets(items)
                .Where(triplet => triplet.Item1 + triplet.Item2 + triplet.Item3 == 2020)
                .First();

            return $"{magicTriplets.Item1 * magicTriplets.Item2 * magicTriplets.Item3}";
        }

        private IEnumerable<(int, int, int)> GetTriplets(List<int> items)
        {
            for (int i = 0; i < items.Count - 2; i++)
            {
                for (int j = i + 1; j < items.Count - 1; j++)
                {
                    for (int k = j + 1; k < items.Count; k++)
                    {
                        yield return (items[i], items[j], items[k]);
                    }
                }
            }
        }

        private IEnumerable<(int, int)> GetPairs(List<int> items)
        {
            for (int i = 0; i < items.Count - 1; i++)
            {
                for (int j = i + 1; j < items.Count; j++)
                {
                    yield return (items[i], items[j]);
                }
            }
        }
    }
}
