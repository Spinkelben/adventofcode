using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day23Tests
    { 
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<string> ringNumbers, int rounds, string result)
        {
            var puzzleSolver = new Year2020Day23();
            var ring = new Year2020Day23.Ring(ringNumbers[0]);
            Year2020Day23.DoGame(ring, ring.First, rounds);

            Assert.Equal(result, Year2020Day23.GetResultString(ring));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> foods, string canonicalListOfDangerousIngredients)
        {
            var puzzleSolver = new Year2020Day23();
            Assert.Equal(canonicalListOfDangerousIngredients.ToString(), puzzleSolver.Part2(foods));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "389125467",
                },
                10,
                "92658374",
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "389125467",
                },
                100,
                "67384529",
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "389125467",
                },
                "149245887792",
            };
        }
    }
}