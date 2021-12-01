using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode
{
    class DayTwo : IDaySolution
    {
        public string PartOne(string input)
        {
            var letterCounts = this.CountLetters(this.ParseInput(input));
            var twoLetterCount = letterCounts.Count(d => d.Any(kvp => kvp.Value == 2));
            var threeLetterCount = letterCounts.Count(d => d.Any(kvp => kvp.Value == 3));
            return $"{twoLetterCount * threeLetterCount}";
        }

        public string PartTwo(string input)
        {
            var words = ParseInput(input);
            foreach (var word in words)
            {
                foreach (var innerWord in words)
                {
                    var commonLetters = this.CommonLetters(innerWord, word);
                    if (commonLetters.Length == innerWord.Length - 1)
                    {
                        return commonLetters;
                    }
                }
            }
            return "";
        }

        private List<Dictionary<char, int>> CountLetters(IEnumerable<string> input)
        {
            var listCount = new List<Dictionary<char, int>>();
            foreach (var word in input)
            {
                var dict = new Dictionary<char, int>();
                foreach (var letter in word)
                {
                    if (dict.ContainsKey(letter) == false)
                    {
                        dict[letter] = 0;
                    }
                    dict[letter] += 1;
                }
                listCount.Add(dict);
            }
            return listCount;
        }

        private string CommonLetters(string word, string otherword)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == otherword[i])
                {
                    sb.Append(word[i]);
                }
            }
            return sb.ToString();
        }

        private IEnumerable<string> ParseInput(string input)
        {
            return input.Split('\n').Where(s => string.IsNullOrWhiteSpace(s) == false);
        }
    }
}
