using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day13Tests
    { 
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<string> instructions, int distance)
        {
            var puzzleSolver = new Year2020Day13();
            Assert.Equal(distance.ToString(), puzzleSolver.Part1(instructions));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> instructions, int distance)
        {
            var puzzleSolver = new Year2020Day13();
            Assert.Equal(distance.ToString(), puzzleSolver.Part2(instructions));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "939",
                    "7,13,x,x,59,x,31,19",
                },
                59 * 5,
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "939",
                    "7,13,x,x,59,x,31,19",
                },
                1068781
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "939",
                    "67,7,59,61",
                },
                754018
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "939",
                    "67,x,7,59,61",
                },
                779210
            }; 
            
            yield return new object[]
             {
                new List<string>()
                {
                    "939",
                    "67,7,x,59,61",
                },
                1261476
             };

            yield return new object[]
             {
                new List<string>()
                {
                    "939",
                    "1789,37,47,1889",
                },
                1202161486
             };
        }
    }
}