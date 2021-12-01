using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class DayTwelve : IDaySolution
    {
        

        public string PartOne(string input)
        {
            var (pots, rules) = ParseInput(input);

            return SolvePuzzle(pots, rules, 20L);
        }

        public string PartTwo(string input)
        {
            var (pots, rules) = ParseInput(input);
            return SolvePuzzle(pots, rules, 50_000_000_000);
            
        }

        public string SolvePuzzle(LinkedList<(char flower, int pos)> pots, Dictionary<string, char> rules, long targetIteration)
        {
            (int skipSteps, var history) = FindCircle(pots, rules);
            if (skipSteps > targetIteration)
            {
                var finalPots = history[(int)targetIteration];
                return finalPots.Where(p => p.flower == '#').Sum(p => p.pos).ToString();
            }
            else
            {
                var lastIdx = history.Count - 1;
                var stepLength = history[lastIdx].First.Value.pos - history[lastIdx -1].First.Value.pos;
                var iterationToGo = targetIteration - history.Count;
                var stepsAdded = iterationToGo * stepLength;

                return history[history.Count - 1].Where(p => p.flower == '#').Sum(p => p.pos + stepsAdded + 1).ToString();
            }

            
        }

        public (int numStepsTillStart, List<LinkedList<(char flower, int pos)>>) FindCircle(LinkedList<(char flower, int pos)> pots, Dictionary<string, char> rules)
        {
            var counter = 0;
            Dictionary<string, int> seenInput = new Dictionary<string, int>();
            List<LinkedList<(char flower, int pos)>> history = new List<LinkedList<(char flower, int pos)>>();

            while (true)
            {
                var key = CalcKey(pots, false);
                if (seenInput.ContainsKey(key))
                {
                    Console.WriteLine($"KEY: {key} SEEN ITERATION: {seenInput[key]} HISTORY:");
                    return (seenInput[key], history);
                }
                Console.WriteLine($"{counter}\t\t: {CalcKey(pots, false)}");
                seenInput[key] = counter;
                history.Add(pots);
                pots = DoIteration(pots, rules);
                counter++;
            }
        }

        private string CalcKey(LinkedList<(char flower, int pos)> pots, bool absolute)
        {
            if (absolute)
            {
                var lowestNegative = pots.Where(p => p.pos < 0).OrderByDescending(p => p.pos).FirstOrDefault();
                var lowestPositive = pots.Where(p => p.pos > 0).OrderBy(p => p.pos).FirstOrDefault();
                lowestNegative.pos = Math.Min(lowestNegative.pos, -1);
                lowestPositive.pos = Math.Max(lowestPositive.pos, 1);
                var center = pots.Count(p => p.pos == 0) == 1 ? pots.First(p => p.pos == 0).flower : '.';

                return string.Join("", pots.Where(p => p.pos < 0).Select(s => s.flower)) + new string('.', lowestNegative.pos - -1) +
                         $"({center})" +
                        new string('.', lowestPositive.pos) + string.Join("", pots.Where(p => p.pos >= 0).Select(s => s.flower));
            }
            return string.Join("", pots.Select(p => p.flower));
        }

        public LinkedList<(char flower, int pos)> DoIteration(LinkedList<(char flower, int pos)> pots, Dictionary<string, char> rules)
        {
            var paddedPots = EnsurePadding(pots);
            var neighbours = new LinkedListNode<(char flower, int pos)>[5];
            var newList = new LinkedList<(char flower, int pos)>();
            neighbours[0] = paddedPots.First;
            neighbours[1] = neighbours[0].Next;
            neighbours[2] = neighbours[1].Next;
            neighbours[3] = neighbours[2].Next;
            neighbours[4] = neighbours[3].Next;

            while(neighbours[4] != null)
            {
                var key = string.Join("", neighbours.Select(n => n.Value.flower));
                if (rules.ContainsKey(key))
                {
                    newList.AddLast((rules[key], neighbours[2].Value.pos));
                }
                else
                {
                    newList.AddLast(('.', neighbours[2].Value.pos));
                }
                neighbours[0] = neighbours[0].Next;
                neighbours[1] = neighbours[1].Next;
                neighbours[2] = neighbours[2].Next;
                neighbours[3] = neighbours[3].Next;
                neighbours[4] = neighbours[4].Next;
            }

            return newList;
        }

        public LinkedList<(char flower, int pos)> EnsurePadding(LinkedList<(char flower, int pos)> pots)
        {
            var node = pots.First;
            int i;
            for (i = 4; i > 0; i--)
            {
                if (node.Value.flower == '#')
                {
                    break;
                }
                node = node.Next;
            }
            for (int j = 0; j < i; j++)
            {
                pots.AddFirst(('.', pots.First.Value.pos - 1));
            }

            node = pots.Last;
            for (i = 4; i > 0; i--)
            {
                if (node.Value.flower == '#')
                {
                    break;
                }
                node = node.Previous;
            }
            for (int j = 0; j < i; j++)
            {
                pots.AddLast(('.', pots.Last.Value.pos + 1));
            }

            return pots;
        }

        public (LinkedList<(char flower, int pos)> pots, Dictionary<string, char> rules) ParseInput(string input)
        {
            var lines = input.Split('\n').Select(l => l.Trim()).Where(s => string.IsNullOrWhiteSpace(s) == false).ToList();
            var initalCondition = lines[0].Substring("initial state: ".Length);
            var rules = lines.Skip(1);

            var i = 0;
            var potRow = new LinkedList<(char flower, int pos)>(initalCondition.Select(c => (c, i++)));
            var ruleMap = rules.Select(r => r.Split(" => ").ToList()).ToDictionary(l => l[0], l => l[1][0]);

            return (potRow, ruleMap);

        }
    }
}
