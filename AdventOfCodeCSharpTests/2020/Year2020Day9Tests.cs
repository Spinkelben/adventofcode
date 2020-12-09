using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day9Tests
    { 
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<string> xmasNumbers, int invalidNumber)
        {
            var puzzleSolver = new Year2020Day9();
            Assert.Equal(invalidNumber, puzzleSolver.FirstViolator(xmasNumbers, 5));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> xmasNumbers, int encryptionWeakness)
        {
            var puzzleSolver = new Year2020Day9();
            var weakNumber = puzzleSolver.FirstViolator(xmasNumbers, 5);
            Assert.Equal(encryptionWeakness, puzzleSolver.GetEncryptionWeakness(xmasNumbers, weakNumber));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "35 ",
                    "20 ",
                    "15 ",
                    "25 ",
                    "47 ",
                    "40 ",
                    "62 ",
                    "55 ",
                    "65 ",
                    "95 ",
                    "102",
                    "117",
                    "150",
                    "182",
                    "127",
                    "219",
                    "299",
                    "277",
                    "309",
                    "576",
                },
                127
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "35 ",
                    "20 ",
                    "15 ",
                    "25 ",
                    "47 ",
                    "40 ",
                    "62 ",
                    "55 ",
                    "65 ",
                    "95 ",
                    "102",
                    "117",
                    "150",
                    "182",
                    "127",
                    "219",
                    "299",
                    "277",
                    "309",
                    "576",
                },
                62
            };
        }
    }
}