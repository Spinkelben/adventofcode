using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day14Tests
    { 
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<string> instructions, int distance)
        {
            var puzzleSolver = new Year2020Day14();
            Assert.Equal(distance.ToString(), puzzleSolver.Part1(instructions));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> instructions, int distance)
        {
            var puzzleSolver = new Year2020Day14();
            Assert.Equal(distance.ToString(), puzzleSolver.Part2(instructions));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X",
                    "mem[8] = 11                                ",
                    "mem[7] = 101                               ",
                    "mem[8] = 0                                 ",
                },
                165,
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "mask = 000000000000000000000000000000X1001X",
                    "mem[42] = 100                              ",
                    "mask = 00000000000000000000000000000000X0XX",
                    "mem[26] = 1                                ",
                },
                208,
            };
        }
    }
}