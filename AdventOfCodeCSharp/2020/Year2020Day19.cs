using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 19)]
    internal class Year2020Day19 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var (grammar, expressions) = ParseInput(input);
            var regexPattern = "^" + GetExpressionFromGrammar(grammar) + "$"; 
            var regex = new Regex(regexPattern);
            var numValid = expressions.Count(e => regex.IsMatch(e));

            return $"{numValid}";
        }

        public string Part2(IList<string> input)
        {
            // Assume only input we recieve rule 0 always is 0: 8 11
            // Given custom rules for 8 and 11 we solve the remaining sections as regex
            // and roll custom matching code for rule 8 and 11

            // 0: 8 11. That means apply rule 8 then apply rule 11
            // 8: 42 | 42 8. That means rule 42 one or more times (this is expressible in regular language)
            // 11: 42 31 | 42 11 31. That means "matching 42,31 pairs". Not solveable with pure regex

            var (grammar, expressions) = ParseInput(input);
            var pattern42 = GetExpressionFromGrammar(grammar, 42);
            var pattern31 = GetExpressionFromGrammar(grammar, 31);

            var numValid = expressions.Count(e => IsExpressionInLangueage(pattern31, pattern42, e));

            return $"{numValid}";
        }

        private bool IsExpressionInLangueage(string pattern31, string pattern42, string input)
        {
            Match match = null;
            string rule8exp = string.Empty;
            do
            {
                // Find all matches of rule 8
                rule8exp += $"({pattern42})";
                match = Regex.Match(input, "^" + rule8exp);
                if (match.Success && Rule11Match(pattern31, pattern42, input, rule8exp))
                {
                    return true;
                }
            } while (match?.Success ?? false);

            return false;

        }

        private bool Rule11Match(string exp31, string exp42, string input, string rule8Match)
        {
            // 11: 42 32 | 42 11 31
            var exp42MatchPatterns = RuleMatchPatterns(rule8Match, exp42, input);
            if (exp42MatchPatterns.Count == 0)
            {
                return false;
            }

            // Apply rule 31
            var i = 1;
            foreach (var pattern in exp42MatchPatterns)
            {
                var repeated = string.Join(
                    string.Empty, 
                    Enumerable.Repeat(exp31, i).Select(p => $"({p})"));
                if (Regex.IsMatch(input, "^" + $"({rule8Match})({pattern})({repeated})" + "$"))
                {
                    return true;
                }
                i++;
            }

            return false;
        }

        private List<string> RuleMatchPatterns(string prefixPattern, string pattern, string input)
        {
            Match match = null;
            var result = new List<string>();
            string repeatedPattern = string.Empty;
            do
            {
                repeatedPattern += $"({pattern})";
                match = Regex.Match(input, "^" + prefixPattern + repeatedPattern);
                result.Add(repeatedPattern);
            } while (match.Success);

            // Last element is from the first failed match
            result.RemoveAt(result.Count - 1);
            return result; 
        }

        private (IEnumerable<string> grammar, IEnumerable<string> expressions) ParseInput(IList<string> input)
        {
            // Remove last line if empty
            if (string.IsNullOrWhiteSpace(input[^1]))
            {
                input.RemoveAt(input.Count - 1);
            }

            var seperator = input
                .Zip(Enumerable.Range(0, input.Count))
                .First(e => string.IsNullOrWhiteSpace(e.First))
                .Second;

            var inputArr = input.Select(l => l.Trim()).ToArray();

            return (inputArr[..seperator], inputArr[(seperator + 1)..]);
        }

        private string GetExpressionFromGrammar(IEnumerable<string> grammar, int startingRule = 0)
        {
            var ruleDict = grammar.Select(l =>
            {
                var split = l.Split(':');
                var key = int.Parse(split[0]);
                var expression = split[1]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                return (key, expression);
            })
            .ToDictionary(kvp => kvp.key, kvp => kvp.expression);

            var expression = ruleDict[startingRule];

            while (expression.Any(s => int.TryParse(s, out int _)))
            {
                var nextExpression = new List<string>();
                foreach (var element in expression)
                {
                    if (int.TryParse(element, out int ruleNumber))
                    {
                        nextExpression.Add("(");
                        nextExpression.AddRange(ruleDict[ruleNumber]);
                        nextExpression.Add(")");
                    }
                    else
                    {
                        nextExpression.Add(element);
                    }
                }

                expression = nextExpression;
            }


            return expression
                .Select(e => e.Trim('"'))
                .Aggregate(new StringBuilder(), (sb, value) => sb.Append(value))
                .ToString();
        }
    }
}
