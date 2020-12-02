using System;
using System.Collections.Generic;
using Xunit;
using AdventOfCodeCSharp.Year2020;
using System.Linq;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day1Tests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<int> expenseReport, int product)
        {
            var puzzleSolver = new Year2020Day1();
            Assert.Equal(product.ToString(), puzzleSolver.Part1(expenseReport.Select(l => l.ToString()).ToList()));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<int> expenseReport, int product)
        {
            var puzzleSolver = new Year2020Day1();
            Assert.Equal(product.ToString(), puzzleSolver.Part2(expenseReport.Select(l => l.ToString()).ToList()));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<int>()
                {
                    1721,
                    979 ,
                    366 ,
                    299 ,
                    675 ,
                    1456,
                },
                514579
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<int>()
                {
                    1721,
                    979 ,
                    366 ,
                    299 ,
                    675 ,
                    1456,
                },
                241861950
            };
        }
    }
}