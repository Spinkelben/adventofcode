using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 15)]
    internal class Year2020Day15 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var startingNumbers = input[0]
                .Split(',')
                .Select(n => int.Parse(n));
            int lastSpoken = PlayMemoryGame(startingNumbers, 2020);

            return $"{lastSpoken}";
        }

        public string Part2(IList<string> input)
        {
            var startingNumbers = input[0]
                .Split(',')
                .Select(n => int.Parse(n));
            int lastSpoken = PlayMemoryGame(startingNumbers, 30000000);

            return $"{lastSpoken}";
        }

        private static int PlayMemoryGame(IEnumerable<int> startingNumbers, int lastTurn)
        {
            var memory = new Dictionary<int, (int lastSpoken, int before)>();
            var turn = 1;
            var lastSpoken = -1;
            foreach (var number in startingNumbers)
            {
                memory[number] = (turn, -1);
                lastSpoken = number;
                turn++;
            }

            for (; turn <= lastTurn; turn++)
            {
                int spokenNumber;

                if (memory[lastSpoken].before > 0)
                {
                    var (lastTime, before) = memory[lastSpoken];
                    spokenNumber = lastTime - before;
                }
                else
                {
                    spokenNumber = 0;
                }

                if (memory.ContainsKey(spokenNumber))
                {
                    UpdateMemory(memory, turn, spokenNumber);
                }
                else
                {
                    memory[spokenNumber] = (turn, -1);
                }
                lastSpoken = spokenNumber;

            }

            return lastSpoken;
        }

        private static void UpdateMemory(Dictionary<int, (int lastSpoken, int before)> memory, int turn, int spokenNumber)
        {
            var (last, _) = memory[spokenNumber];
            memory[spokenNumber] = (turn, last);
        }


    }
}
