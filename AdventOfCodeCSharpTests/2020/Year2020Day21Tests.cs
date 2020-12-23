using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day21Tests
    { 
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<string> foods, int numSafeIngredients)
        {
            var puzzleSolver = new Year2020Day21();
            Assert.Equal(numSafeIngredients.ToString(), puzzleSolver.Part1(foods));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> foods, string canonicalListOfDangerousIngredients)
        {
            var puzzleSolver = new Year2020Day21();
            Assert.Equal(canonicalListOfDangerousIngredients.ToString(), puzzleSolver.Part2(foods));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "mxmxvkd kfcds sqjhc nhms (contains dairy, fish)",
                    "trh fvjkl sbzzf mxmxvkd (contains dairy)       ",
                    "sqjhc fvjkl (contains soy)                     ",
                    "sqjhc mxmxvkd sbzzf (contains fish)            ",
                },
                5,
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "mxmxvkd kfcds sqjhc nhms (contains dairy, fish)",
                    "trh fvjkl sbzzf mxmxvkd (contains dairy)       ",
                    "sqjhc fvjkl (contains soy)                     ",
                    "sqjhc mxmxvkd sbzzf (contains fish)            ",
                },
                "mxmxvkd,sqjhc,fvjkl",
            };
        }
    }
}