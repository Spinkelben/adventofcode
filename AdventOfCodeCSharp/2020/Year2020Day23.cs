using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 23)]
    internal class Year2020Day23 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var ring = new Ring(input[0]);
            var current = ring.First;
            DoGame(ring, current, 100);
            return GetResultString(ring);
        }

        internal static string GetResultString(Ring ring)
        {
            var numbers = new List<string>();
            var first = ring.GetValue(1);
            numbers.Add(first.Value.ToString());

            var current = first.Next;
            while (current != first)
            {
                numbers.Add(current.Value.ToString());
                current = current.Next;
            }

            return string.Join(string.Empty, numbers)[1..];
        }

        internal static Ring.Cup DoGame(Ring ring, Ring.Cup current, int rounds)
        {
            for (int i = 0; i < rounds; i++)
            {
                current = ring.DoMove(current);
            }

            return current;
        }

        public string Part2(IList<string> input)
        {
            var ring = new Ring(input[0]);
            ring.AddExtraElements(1_000_000);
            DoGame(ring, ring.First, 10_000_000);
            var one = ring.GetValue(1);
            return $"{(long)one.Next.Value * (long)one.Next.Next.Value}";
        }

        internal class Ring
        {
            private readonly Cup ring;
            private int minValue = int.MaxValue;
            private int maxValue = int.MinValue;
            private readonly Dictionary<int, Cup> cupLabelLookup = new Dictionary<int, Cup>();

            internal Ring(string input)
            {
                Cup current = null;
                foreach (var number in input.Select(c => int.Parse(c.ToString())))
                {
                    if (number < minValue)
                    {
                        minValue = number;
                    }

                    if (number > maxValue)
                    {
                        maxValue = number;
                    }

                    var next = new Cup(number);
                    cupLabelLookup[number] = next;

                    if (current == null)
                    {
                        current = next;
                        ring = current;
                    }
                    else
                    {
                        current.Next = next;
                        current = next;
                    }
                };

                // Make a cycle
                current.Next = this.ring;
            }

            internal void AddExtraElements(int totalCount)
            {
                var current = this.ring;
                var count = 1;
                while (current.Next != this.ring)
                {
                    current = current.Next;
                    count++;
                }

                for (int i = maxValue + 1; count < totalCount; i++, count++)
                {
                    current.Next = new Cup(i);
                    current = current.Next;
                    cupLabelLookup[i] = current;
                }
                maxValue = cupLabelLookup.Keys.Max();
                if (cupLabelLookup.Count != totalCount)
                {
                    throw new Exception("I can't program");
                }

                current.Next = this.ring;
            }

            internal Cup First => this.ring;

            internal Cup DoMove(Cup current)
            {
                var cups = new List<Cup>();
                var next = current.Next;
                for (int i = 0; i < 3; i++)
                {
                    cups.Add(next);
                    next = next.Next;
                }

                // Remove the three cups from the list
                current.Next = next;

                var nextLabel = GetNextLabel(current.Value, cups);
                var destinaton = GetValue(nextLabel);

                var temp = destinaton.Next;
                destinaton.Next = cups[0];
                for (int i = 0; i < 2; i++)
                {
                    cups[i].Next = cups[i + 1];
                }
                cups[2].Next = temp;

                return current.Next;
            }

            private int GetNextLabel(int curretLabel, List<Cup> cups)
            {
                var nextLabel = curretLabel - 1;

                if (nextLabel < minValue)
                {
                    nextLabel = maxValue;
                }

                while (cups.Any(c => c.Value == nextLabel))
                {
                    nextLabel--;

                    if (nextLabel < minValue)
                    {
                        nextLabel = maxValue;
                    }
                }

                return nextLabel;
            }

            internal Cup GetValue(int label)
            {
                return cupLabelLookup[label];
            }

            internal class Cup
            {
                internal Cup(int value)
                {
                    Value = value;
                }

                internal int Value { get; }
                internal Cup Next { get; set; }
            }
        }
    }
}
