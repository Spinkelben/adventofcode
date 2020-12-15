using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day15Tests
    { 
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<string> startingNumbers, int finalNumber)
        {
            var puzzleSolver = new Year2020Day15();
            Assert.Equal(finalNumber.ToString(), puzzleSolver.Part1(startingNumbers));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> startingNumbers, int distance)
        {
            var puzzleSolver = new Year2020Day15();
            Assert.Equal(distance.ToString(), puzzleSolver.Part2(startingNumbers));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "0,3,6",
                },
                436,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "1,3,2",
                },
                1,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "2,1,3",
                },
                10,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "1,2,3",
                },
                27,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "2,3,1",
                },
                78,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "3,2,1",
                },
                438,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "3,1,2",
                },
                1836,
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "0,3,6",
                },
                175594,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "1,3,2",
                },
                2578,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "2,1,3",
                },
                3544142,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "1,2,3",
                },
                261214,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "2,3,1",
                },
                6895259,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "3,2,1",
                },
                18,
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "3,1,2",
                },
                362,
            };
        }
    }
}