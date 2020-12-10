using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 10)]
    internal class Year2020Day10 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var adapters = PrepareAdapterList(ConvertInput(input));

            var num1JoltDifference = 0;
            var num3JoltDifference = 0;

            for (int i = 0; i < adapters.Length - 1; i++)
            {
                var difference = adapters[i + 1] - adapters[i];
                if (difference == 1)
                {
                    num1JoltDifference++;
                }
                else if (difference == 3)
                {
                    num3JoltDifference++;
                }
                else
                {
                    throw new InvalidOperationException("Only 1 and 3 jolt differences expected");
                }
            }

            return $"{num1JoltDifference * num3JoltDifference}";
        }

        public string Part2(IList<string> input)
        {
            var adapters = PrepareAdapterList(ConvertInput(input));

            // Find sequences of 1 jolt differences
            // Find number of combinations for each sequence
            // Multiply together

            
            var segments = new List<int[]>();

            var segmentStart = 0;
            for (int i = 0; i < adapters.Length - 1; i++)
            {
                var difference = adapters[i + 1] - adapters[i];
                if (difference == 3)
                {
                    segments.Add(adapters[segmentStart..(i + 1)]);
                    segmentStart = i + 1;
                }
            }

            segments.Add(adapters[segmentStart..adapters.Length]);

            var combinations = segments
                .Select(s => NumberOfCombinations(s, new HashSet<string>()))
                .ToList();
            var totalCombinatios = combinations
                .Aggregate(1L, (acc, v) => acc * v);

            return $"{totalCombinatios}";
        }

        private long NumberOfCombinations(int[] segment, HashSet<string> countedSegments)
        {
            var hash = GetHash(segment);
            if (countedSegments.Contains(hash))
            {
                return 0L;
            }

            countedSegments.Add(hash);

            if (!IsValidSegment(segment))
            {
                return 0;
            }

            if (segment.Length < 3)
            {
                return 1;
            }
            else
            {
                var validSegments = 1L;
                for (int i = 1; i < segment.Length - 1; i++)
                {
                    var newSegment = new List<int>(segment);
                    newSegment.RemoveAt(i);
                    validSegments += NumberOfCombinations(newSegment.ToArray(), countedSegments);
                }

                return validSegments;
            }
        }

        private string GetHash(int[] segment)
        {
            return string.Join(':', segment);
        }

        private bool IsValidSegment(int[] segment)
        {
            for (int i = 0; i < segment.Length - 1; i++)
            {
                if (segment[i + 1] - segment[i] > 3)
                {
                    return false;
                }
            }

            return true;
        }

        private int[] PrepareAdapterList(List<int> adapters)
        {
            // Add outlet
            adapters.Add(0);

            // Add device
            var deviceRating = adapters.Max() + 3;
            adapters.Add(deviceRating);

            adapters.Sort();

            return adapters.ToArray();
        }

        private List<int> ConvertInput(IList<string> input)
        {
            return input
                .Where(i => i.Length > 0)
                .Select(i => int.Parse(i))
                .ToList();
        }
    }
}
