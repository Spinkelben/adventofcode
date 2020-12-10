using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day10Tests
    { 
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<string> adapters, int adapterPrpduct)
        {
            var puzzleSolver = new Year2020Day10();
            Assert.Equal(adapterPrpduct.ToString(), puzzleSolver.Part1(adapters));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> adapters, int adapterProduct)
        {
            var puzzleSolver = new Year2020Day10();
            Assert.Equal(adapterProduct.ToString(), puzzleSolver.Part2(adapters));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "16",
                    "10",
                    "15",
                    "5 ",
                    "1 ",
                    "11",
                    "7 ",
                    "19",
                    "6 ",
                    "12",
                    "4",
                },
                7 * 5
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "28",
                    "33",
                    "18",
                    "42",
                    "31",
                    "14",
                    "46",
                    "20",
                    "48",
                    "47",
                    "24",
                    "23",
                    "49",
                    "45",
                    "19",
                    "38",
                    "39",
                    "11",
                    "1 ",
                    "32",
                    "25",
                    "35",
                    "8 ",
                    "17",
                    "7 ",
                    "9 ",
                    "4 ",
                    "2 ",
                    "34",
                    "10",
                    "3 ",
                },
                22 * 10
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "16",
                    "10",
                    "15",
                    "5 ",
                    "1 ",
                    "11",
                    "7 ",
                    "19",
                    "6 ",
                    "12",
                    "4",
                },
                8
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "28",
                    "33",
                    "18",
                    "42",
                    "31",
                    "14",
                    "46",
                    "20",
                    "48",
                    "47",
                    "24",
                    "23",
                    "49",
                    "45",
                    "19",
                    "38",
                    "39",
                    "11",
                    "1 ",
                    "32",
                    "25",
                    "35",
                    "8 ",
                    "17",
                    "7 ",
                    "9 ",
                    "4 ",
                    "2 ",
                    "34",
                    "10",
                    "3 ",
                },
                19208
            };
        }
    }
}