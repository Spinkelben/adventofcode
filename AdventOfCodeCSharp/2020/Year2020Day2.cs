using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 2)]
    internal class Year2020Day2 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            return $"{input.Count(s => IsValidPassword(s, 1))}";
        }

        public string Part2(IList<string> input)
        {
            return $"{input.Count(s => IsValidPassword(s, 2))}";
        }

        private bool IsValidPassword(string s, int interpretation)
        {
            if (s.Length == 0)
            {
                return false;
            }

            var (password, first, second, letter) = ParsePolicyString(s);
            return interpretation switch 
            {
                1 => GetPolicyVerifier(first, second, letter)(password), 
                2 => GetNewPolicyVerifier(first, second, letter)(password),
                _ => false,
            };
        }

        private Func<string, bool> GetNewPolicyVerifier(int first, int second, char letter)
        {
            return (string password) =>
            {
                return password[first - 1] == letter ^ password[second - 1] == letter;
            };
        }

        private Func<string, bool> GetPolicyVerifier(int minOccurancesOfLetter, int maxOccurancesOfLetter, char letter)
        {
            return (string password) =>
            {
                var numOccurancesOfLetter = password.Count(c => c == letter);
                return numOccurancesOfLetter >= minOccurancesOfLetter &&
                    numOccurancesOfLetter <= maxOccurancesOfLetter;
            };
        }

        private (string password, int first, int second, char letter) ParsePolicyString(string passwordAndPolicy)
        {
            var segments = passwordAndPolicy.Split(':');
            var policy = segments[0].Trim();
            var password = segments[1].Trim();
            segments = policy.Split(' ');
            var range = segments[0];
            var letter = segments[1][0];
            var rangeSegments = range.Split('-');
            var first = int.Parse(rangeSegments[0]);
            var second = int.Parse(rangeSegments[1]);

            return (password, first, second, letter);
        }
    }
}
