using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day18Tests
    { 
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<string> expression, int value)
        {
            var puzzleSolver = new Year2020Day18();
            Assert.Equal(value.ToString(), puzzleSolver.Part1(expression));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> expression, int value)
        {
            var puzzleSolver = new Year2020Day18();
            Assert.Equal(value.ToString(), puzzleSolver.Part2(expression));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "1 + 2 * 3 + 4 * 5 + 6",
                },
                71,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "1 + (2 * 3) + (4 * (5 + 6))",
                },
                51,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "2 * 3 + (4 * 5)",
                },
                26,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "5 + (8 * 3 + 9 + 3 * 4 * 3)",
                },
                437,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))",
                },
                12240,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2",
                },
                13632,
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "1 + 2 * 3 + 4 * 5 + 6",
                },
                231,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "1 + (2 * 3) + (4 * (5 + 6))",
                },
                51,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "2 * 3 + (4 * 5)",
                },
                46,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "5 + (8 * 3 + 9 + 3 * 4 * 3)",
                },
                1445,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))",
                },
                669060,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2",
                },
                23340,
            };
        }
    }
}