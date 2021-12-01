using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode
{
    public class DayFive : IDaySolution
    {
        public string PartOne(string input)
        {
            var cleanInput = string.Join("", input.Where(c => char.IsLetter(c)));
            var result = React(input);
            return $"Polymer: {result}, Result: {result.Length}";
        }

        public string PartTwo(string input)
        {
            var cleanInput = string.Join("", input.Where(c => char.IsLetter(c)));
            var distinctChars = cleanInput.Select(c => char.ToLowerInvariant(c)).Distinct();
            string result= default;
            char element = default;
            foreach (var distinctChar in distinctChars)
            {
                var reduced = this.React(RemoveElement(cleanInput, distinctChar));
                if (reduced.Length < (result?.Length ?? int.MaxValue))
                {
                    result = reduced;
                    element = distinctChar;
                }
            }

            return $"Removed {element}. Polymer: {result}, Result: {result.Length}";
        }

        public string RemoveElement(string input, char element)
        {
            var sb = new StringBuilder();
            var elementLower = char.ToLowerInvariant(element);
            for (int i = 0; i < input.Length; i++)
            {
                if (char.ToLowerInvariant(input[i]) != elementLower)
                {
                    sb.Append(input[i]);
                }
            }
            return sb.ToString();
        }

        public string React(string input)
        {
            var current = "";
            var next = input;
            do
            {
                current = next;
                next = this.ReduceOnce(current);
            }
            while (next != current);
            return current;
        }

        public string ReduceOnce(string input)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                if (i == input.Length - 1)
                {
                    sb.Append(input[i]);
                    continue;
                }
                var element = input[i];
                var nextElement = input[i + 1];
                if (char.ToLower(element) == char.ToLower(nextElement))
                {
                    if (char.IsLower(element))
                    {
                        if (char.IsUpper(nextElement))
                        {
                            i++;
                            continue;
                        }
                    }
                    else
                    {
                        if (char.IsLower(nextElement))
                        {
                            i++;
                            continue;
                        }
                    }
                }
                sb.Append(element);
            }

            return sb.ToString();
        }
    }


}
