using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 6)]
    internal class Year2020Day6 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var groups = GetGroups(input);
            return groups
                .Select(g => GetQuestionsWithPositiveResponses(g))
                .Sum(g => g.Count)
                .ToString();
        }

        public string Part2(IList<string> input)
        {
            var groups = GetGroups(input);
            return groups.Select(g => GetQuestionsEveryoneAnswered(g))
                .Sum(q => q.Count)
                .ToString();
        }

        internal HashSet<char> GetQuestionsWithPositiveResponses(IList<string> groupAnswers)
        {
            var answers = new HashSet<char>();
            foreach (var person in groupAnswers)
            {
                foreach (var answer in person.Where(a => !char.IsWhiteSpace(a)))
                {
                    answers.Add(answer);
                }
            }

            return answers;
        }

        private List<char> GetQuestionsEveryoneAnswered(List<string> groupsAnswers)
        {
            var allQuestions = GetQuestionsWithPositiveResponses(groupsAnswers);
            var result = new List<char>();
            foreach (var question in allQuestions)
            {
                if (groupsAnswers.All(a => a.Contains(question)))
                {
                    result.Add(question);
                }
            }

            return result;
        }

        private List<List<string>> GetGroups(IList<string> answers)
        {
            var result = new List<List<string>>();
            var currentGroup = new List<string>();
            foreach (var line in answers)
            {
                if (line.Length == 0)
                {
                    result.Add(currentGroup);
                    currentGroup = new List<string>();
                }
                else
                {
                    currentGroup.Add(line);
                }
            }

            if (currentGroup.Count > 0)
            {
                result.Add(currentGroup);
            }

            return result;
        }

    }
}
