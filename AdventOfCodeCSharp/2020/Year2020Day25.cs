using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 25)]
    internal class Year2020Day25 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var cardPublicKey = int.Parse(input[0]);
            var doorPublicKey = int.Parse(input[1]);

            var cardLoopSize = (int)GetLoopSize(7, cardPublicKey);
            var encryptionKey = TransformNumber(doorPublicKey, cardLoopSize);
            return $"{encryptionKey}";
        }

        public string Part2(IList<string> input)
        {
            return string.Empty;
        }

        private long GetLoopSize(int subject, int target)
        {
            var value = 1L;
            var count = 0;
            while (value != target)
            {
                value = TranseformOnce(subject, value);
                count++;
            }

            return count;
        }

        private long TransformNumber(int subject, int loopSize)
        {
            var value = 1L;
            for (int i = 0; i < loopSize; i++)
            {
                value = TranseformOnce(subject, value);
            }

            return value;
        }

        private static long TranseformOnce(int subject, long value)
        {
            value *= subject;
            value %= 20201227;
            return value;
        }
    }
}
