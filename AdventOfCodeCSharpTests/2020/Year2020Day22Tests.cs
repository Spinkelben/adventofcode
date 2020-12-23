using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day22Tests
    { 
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<string> foods, int numSafeIngredients)
        {
            var puzzleSolver = new Year2020Day22();
            Assert.Equal(numSafeIngredients.ToString(), puzzleSolver.Part1(foods));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> foods, string canonicalListOfDangerousIngredients)
        {
            var puzzleSolver = new Year2020Day22();
            Assert.Equal(canonicalListOfDangerousIngredients.ToString(), puzzleSolver.Part2(foods));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "Player 1:",
                    "9        ",
                    "2        ",
                    "6        ",
                    "3        ",
                    "1        ",
                    "         ",
                    "Player 2:",
                    "5        ",
                    "8        ",
                    "4        ",
                    "7        ",
                    "10       ",
                },
                306,
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "Player 1:",
                    "9        ",
                    "2        ",
                    "6        ",
                    "3        ",
                    "1        ",
                    "         ",
                    "Player 2:",
                    "5        ",
                    "8        ",
                    "4        ",
                    "7        ",
                    "10       ",
                },
                291,
            };
        }
    }
}