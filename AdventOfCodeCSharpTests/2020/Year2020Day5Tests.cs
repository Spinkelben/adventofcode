using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day5Tests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(string boardingPass, int row, int column, int id)
        {
            var puzzleSolver = new Year2020Day5();
            (int r, int c, int i) = puzzleSolver.ParseBoardingPass(boardingPass);
            Assert.Equal(row, r);
            Assert.Equal(column, c);
            Assert.Equal(id, i);
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                "BFFFBBFRRR",
                70,
                7,
                567
            };

            yield return new object[]
            {
                "FFFBBBFRRR",
                14,
                7,
                119
            };

            yield return new object[]
            {
                "BBFFBBFRLL",
                102,
                4,
                820
            };
        }
    }
}