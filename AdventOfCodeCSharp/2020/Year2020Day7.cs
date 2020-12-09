using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 7)]
    internal class Year2020Day7 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var rules = input
                .Select(r => RuleParser(r))
                .ToDictionary(r => (r.Variant, r.Color));

            var containers = GetContainingBags(("shiny", "gold"), rules);

            return $"{containers.Count}";
        }

        public string Part2(IList<string> input)
        {
            var rules = input
                .Select(r => RuleParser(r))
                .ToDictionary(r => (r.Variant, r.Color));

            // The shiny gold back doesn't count hence -1
            var count = GetNumberOfBagsRecursive(
                rules,
                ("shiny", "gold")) - 1;
            return $"{count}";
        }

        private long GetNumberOfBagsInside(
            (string variant, string color) bag,
            Dictionary<(string variant, string color), Rule> rules)
        {
            var stack = new Stack<(int count, (string variant, string color) bag)>();
            stack.Push((1, bag));
            var multiplier = 1;
            var sum = 0;

            while (stack.Count > 0)
            {
                var (currentCount, currentBag) = stack.Pop();
                multiplier /= currentCount;
                var rule = rules[currentBag];
                sum += multiplier * currentCount;
                multiplier *= currentCount;
                foreach (var nestedbag in rule.Contains)
                {
                    stack.Push(nestedbag);
                }
            }

            return sum;

        }

        private long GetNumberOfBagsRecursive(
            Dictionary<(string variant, string color), Rule> rules, 
            (string variant, string color) bag)
        {
            var bagRule = rules[bag];
            long sum = 1;
            foreach (var nestedBag in bagRule.Contains)
            {
                sum += nestedBag.number * GetNumberOfBagsRecursive(rules, nestedBag.Item2);
            }

            return sum;
        }

        private HashSet<Rule> GetContainingBags(
            (string variant, string color) bag,
            Dictionary<(string variant, string color), Rule> rules)
        {
            var queue = new Queue<(string variant, string color)>();
            var result = new HashSet<Rule>();

            queue.Enqueue(bag);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var rule in rules.Values)
                {
                    if (rule.Contains.Any(r => r.Item2 == current))
                    {
                        queue.Enqueue((rule.Variant, rule.Color));
                        result.Add(rule);
                    }
                }
            }

            return result;
        }

        private Rule RuleParser(string rule)
        {
            var matches = Regex.Match(rule, @"(?<variant>\w+) (?<color>\w+) bags contain( (\d+) (\w+) (\w+) bags?[\.,]?| no other bags\.)+");
            var r = new Rule()
            {
                Variant = matches.Groups["variant"].Value,
                Color = matches.Groups["color"].Value,
            };

            if (matches.Groups[1].Value == " no other bags.")
            {
                return r;
            }
            else
            {
                for (int i = 0; i < matches.Groups[1].Captures.Count; i++)
                {
                    var number = int.Parse(matches.Groups[2].Captures[i].Value);
                    var variant = matches.Groups[3].Captures[i].Value;
                    var color = matches.Groups[4].Captures[i].Value;
                    r.Contains.Add((number, (variant, color)));
                }
            }

            return r;
        }
    }

    internal class Rule
    {
        internal string Color { get; set; }
        internal string Variant { get; set; }
        internal List<(int number, (string variant, string color))> Contains { get; }
            = new List<(int, (string, string))>();
    }
}
