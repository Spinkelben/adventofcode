using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day8Tests
    { 
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<string> program, int accumulator)
        {
            var puzzleSolver = new Year2020Day8();
            Assert.Equal(accumulator.ToString(), puzzleSolver.Part1(program));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> program, int accumulator)
        {
            var puzzleSolver = new Year2020Day8();
            Assert.Equal(accumulator.ToString(), puzzleSolver.Part2(program));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "nop +0 ",
                    "acc +1 ",
                    "jmp +4 ",
                    "acc +3 ",
                    "jmp -3 ",
                    "acc -99",
                    "acc +1 ",
                    "jmp -4 ",
                    "acc +6 ",
                },
                5
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "nop +0 ",
                    "acc +1 ",
                    "jmp +4 ",
                    "acc +3 ",
                    "jmp -3 ",
                    "acc -99",
                    "acc +1 ",
                    "jmp -4 ",
                    "acc +6 ",
                },
                8
            };
        }
    }
}