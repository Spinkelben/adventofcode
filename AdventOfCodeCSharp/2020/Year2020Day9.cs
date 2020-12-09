using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 9)]
    internal class Year2020Day9 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            return $"{FirstViolator(input, 25)}";
        }

        public string Part2(IList<string> input)
        {
            var weakNumber = FirstViolator(input, 25);
            return GetEncryptionWeakness(input, weakNumber).ToString();
        }

        internal long FirstViolator(IList<string> numbersStr, int lookBack)
        {
            var numbers = numbersStr
                .Where(s => s.Length > 0)
                .Select(s => long.Parse(s))
                .ToList();
            var window = new long[lookBack];
            var next = 0;

            for (int i = 0; i < lookBack; i++)
            {
                window[i] = numbers[i];
                next++;
            }

            for (int i = next; i < numbers.Count; i++)
            {
                if (!IsValid(numbers[i], window))
                {
                    return numbers[i];
                }

                window[i % window.Length] = numbers[i];
            }

            return -1;
        }

        internal long GetEncryptionWeakness(IList<string> numbersStr, long weakNumber)
        {
            var numbers = numbersStr
                .Where(s => s.Length > 0)
                .Select(s => long.Parse(s))
                .ToList();
            var range = GetWeakWindow(numbers, weakNumber);
            var window = numbers.GetRange(range.offset, range.length);
            return window.Min() + window.Max();
        }

        private (int offset, int length) GetWeakWindow(List<long> numbers, long weakNumber)
        {
            for (int i = 2; i <= numbers.Count; i++)
            {
                foreach (var range in GetContiguousWindows(i, numbers.Count))
                {
                    var window = numbers.GetRange(range.offset, range.length);
                    if (window.Sum() == weakNumber)
                    {
                        return range;
                    }
                } 
            }

            return (-1, -1);
        }

        private IEnumerable<(int offset, int length)> GetContiguousWindows(int size, int listLength)
        {
            for (int i = 0; i < listLength - (size - 1); i++)
            {
                yield return (i, size);
            }
        }

        private bool IsValid(long number, IList<long> window)
        {
            return GetCombinations(window).Any(p => p.Item1 + p.Item2 == number);
        }

        private IEnumerable<(long, long)> GetCombinations(IList<long> window)
        {
            for (int i = 0; i < window.Count - 1; i++)
            {
                for (int j = i + 1; j < window.Count; j++)
                {
                    var n1 = window[i];
                    var n2 = window[j];
                    if (n1 != n2)
                    {
                        yield return (n1, n2);
                    }
                }
            }
        }
    }
}
