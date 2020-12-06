using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day6Tests
    {
        [Theory]
        [MemberData(nameof(SingleGroup))]
        public void SingleGroupParsing(List<string> answers, int numQuestions)
        {
            var puzzleSolver = new Year2020Day6();
            var result = puzzleSolver.GetQuestionsWithPositiveResponses(answers);
            Assert.Equal(numQuestions, result.Count);
        }

        public static IEnumerable<object[]> SingleGroup()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "abcx",
                    "abcy",
                    "abcz",
                },
                6
            };
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<string> answers, int sumOfAnswers)
        {
            var puzzleSolver = new Year2020Day6();
            Assert.Equal(sumOfAnswers.ToString(), puzzleSolver.Part1(answers));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> answers, int sumOfAnswers)
        {
            var puzzleSolver = new Year2020Day6();
            Assert.Equal(sumOfAnswers.ToString(), puzzleSolver.Part2(answers));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "abc",
                    "",
                    "a",
                    "b",
                    "c",
                    "",
                    "ab",
                    "ac",
                    "",
                    "a",
                    "a",
                    "a",
                    "a",
                    "",
                    "b",
                },
                11
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "abc",
                    "",
                    "a",
                    "b",
                    "c",
                    "",
                    "ab",
                    "ac",
                    "",
                    "a",
                    "a",
                    "a",
                    "a",
                    "",
                    "b",
                },
                6
            };
        }
    }
}