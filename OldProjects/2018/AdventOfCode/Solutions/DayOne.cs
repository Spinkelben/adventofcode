using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode
{
    internal class DayOne : IDaySolution
    {
        public string PartOne(string input)
        {
            return this.SanitizeInput(input)
                .Aggregate((seed, value) => { return seed + value; })
                .ToString();
        }

        public string PartTwo(string input)
        {
            var map = new HashSet<int>();
            var list = this.SanitizeInput(input);
            var frequency = 0;
            map.Add(frequency);
            while (true)
            {
                foreach (var number in list)
                {
                    frequency += number;
                    if (map.Contains(frequency))
                    {
                        return frequency.ToString();
                    }
                    map.Add(frequency);
                }
            }
        }

        private List<int> SanitizeInput(string input)
        {
            return input
                .Split('\n')
                .Where(s => int.TryParse(s, out int result))
                .Select(s => int.Parse(s))
                .ToList();
        }
    }
}
