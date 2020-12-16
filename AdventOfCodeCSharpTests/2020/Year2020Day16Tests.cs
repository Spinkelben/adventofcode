using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day16Tests
    { 
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<string> startingNumbers, int finalNumber)
        {
            var puzzleSolver = new Year2020Day16();
            Assert.Equal(finalNumber.ToString(), puzzleSolver.Part1(startingNumbers));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> startingNumbers, int distance)
        {
            var puzzleSolver = new Year2020Day16();
            Assert.Equal(distance.ToString(), puzzleSolver.Part2(startingNumbers));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "class: 1-3 or 5-7   ",
                    "row: 6-11 or 33-44  ",
                    "seat: 13-40 or 45-50",
                    "                    ",
                    "your ticket:        ",
                    "7,1,14              ",
                    "                    ",
                    "nearby tickets:     ",
                    "7,3,47              ",
                    "40,4,50             ",
                    "55,2,20             ",
                    "38,6,12             ",
                },
                4 + 55 + 12,
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "class: 0-1 or 4-19 ",
                    "row: 0-5 or 8-19   ",
                    "seat: 0-13 or 16-19",
                    "                   ",
                    "your ticket:       ",
                    "11,12,13           ",
                    "                   ",
                    "nearby tickets:    ",
                    "3,9,18             ",
                    "15,1,5             ",
                    "5,14,9             ",
                },
                new Dictionary<string, int>() 
                {
                    ["row"] = 0,
                    ["class"] = 1,
                    ["seat"] = 2,
                },
            };
        }
    }
}