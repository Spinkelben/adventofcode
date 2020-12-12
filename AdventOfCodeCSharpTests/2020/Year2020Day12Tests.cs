using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day12Tests
    { 
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<string> instructions, int distance)
        {
            var puzzleSolver = new Year2020Day12();
            Assert.Equal(distance.ToString(), puzzleSolver.Part1(instructions));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> instructions, int distance)
        {
            var puzzleSolver = new Year2020Day12();
            Assert.Equal(distance.ToString(), puzzleSolver.Part2(instructions));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "F10",
                    "N3 ",
                    "F7 ",
                    "R90",
                    "F11",
                },
                17 + 8
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "F10",
                    "N3 ",
                    "F7 ",
                    "R90",
                    "F11",
                },
                214 + 72
            };
        }
    }
}