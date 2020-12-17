using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day17Tests
    { 
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<string> startState, int finalActive)
        {
            var puzzleSolver = new Year2020Day17();
            Assert.Equal(finalActive.ToString(), puzzleSolver.Part1(startState));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> startState, int finalActive)
        {
            var puzzleSolver = new Year2020Day17();
            Assert.Equal(finalActive.ToString(), puzzleSolver.Part2(startState));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    ".#.",
                    "..#",
                    "###",
                },
                112,
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
             {
                new List<string>()
                {
                    ".#.",
                    "..#",
                    "###",
                },
                848,
             };
        }
    }
}