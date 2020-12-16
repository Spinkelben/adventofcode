using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 16)]
    internal class Year2020Day16 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var filteredInput = input
                .Select(s => s.Trim())
                .ToList();
            if (filteredInput[^1] == "")
            {
                filteredInput.RemoveAt(filteredInput.Count - 1);
            }

            var rules = ParseValidationRules(filteredInput).ToList();
            var tickets = ParseTickets(filteredInput);
            var invalidFields = new List<int>();
            foreach (var ticket in tickets)
            {
                foreach (var field in ticket)
                {
                    if (!ValidateField(field, rules))
                    {
                        invalidFields.Add(field);
                    }
                }
            }

            return $"{invalidFields.Sum()}";
        }

        private IEnumerable<List<int>> ParseTickets(IList<string> input)
        {
            var startOfTickets = input.IndexOf("nearby tickets:");
            for (int i = startOfTickets + 1; i < input.Count; i++)
            {
                yield return input[i].Split(',').Select(int.Parse).ToList();
            }
        }

        private IEnumerable<Rule> ParseValidationRules(IList<string> input)
        {
            var matcher = new Regex(@"(\w+): (\d+)-(\d+) or (\d+)-(\d+)");
            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    yield break;
                }
                var match = matcher.Match(line);
                var rule = new Rule()
                {
                    Name = match.Groups[1].Value,
                };
                rule.Ranges.Add((int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value)));
                rule.Ranges.Add((int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value)));
                yield return rule;
            }
        }

        private bool ValidateField(int value, IEnumerable<Rule> rules)
        {
            return rules.Any(r => IsInRange(r, value));
        }

        private bool IsInRange(Rule rule, int value)
        {
            return rule.Ranges.Any(r =>  value >= r.min && value <= r.max);
        }

        internal List<List<int>> GetTicketColumns(IEnumerable<List<int>> tickets)
        {
            var numColums = tickets.First().Count;
            var result = new List<List<int>>();
            for (int i = 0; i < numColums; i++)
            {
                result.Add(new List<int>());
            }

            foreach (var ticket in tickets)
            {
                for (int i = 0; i < ticket.Count; i++)
                {
                    result[i].Add(ticket[i]);
                }
            }

            return result;
        }

        internal Dictionary<Rule, int> GetColumnNameMapping(List<List<int>> ticketColumns, List<Rule> rules)
        {
            if (rules.Count != ticketColumns.Count)
            {
                throw new Exception("Number of rules does not match number of columns!");
            }
            var result = new Dictionary<Rule, int>();
            var remainingRules = new HashSet<Rule>(rules);
            var remainingColumns = new HashSet<(int columnNumber, List<int> value)>();
            for (int i = 0; i < ticketColumns.Count; i++)
            {
                remainingColumns.Add((i, ticketColumns[i]));
            }
            while (remainingRules.Count > 0)
            {
                Rule ruleToBeRemoved = null;
                (int, List<int>) columnToBeRemoved = (-1, null);
                foreach (var (columnNumber, values) in remainingColumns)
                {
                    var validRules = GetValidRulesForColumn(values, remainingRules);
                    if (validRules.Count() == 1)
                    {
                        ruleToBeRemoved = validRules.First();
                        columnToBeRemoved = (columnNumber, values);
                        result.Add(ruleToBeRemoved, columnNumber);
                        break;
                    }
                }

                remainingColumns.Remove(columnToBeRemoved);
                remainingRules.Remove(ruleToBeRemoved);
            }

            return result;
        }

        internal IEnumerable<Rule> GetValidRulesForColumn(List<int> values, IEnumerable<Rule> rules)
        {
            foreach (var rule in rules)
            {
                if (values.All(v => IsInRange(rule, v)))
                {
                    yield return rule;
                }
            }
        }

        public string Part2(IList<string> input)
        {
            return "";
        }

        internal class Rule
        {
            internal List<(int min, int max)> Ranges { get; set; } = new List<(int min, int max)>();
            internal string Name { get; set; }
        }
    }
}
