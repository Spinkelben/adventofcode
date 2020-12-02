using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day2Tests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<string> passwordsAndPolicies, int numValidPasswords)
        {
            var puzzleSolver = new Year2020Day2();
            Assert.Equal(numValidPasswords.ToString(), puzzleSolver.Part1(passwordsAndPolicies));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> passwordsAndPolicies, int numValidPasswords)
        {
            var puzzleSolver = new Year2020Day2();
            Assert.Equal(numValidPasswords.ToString(), puzzleSolver.Part2(passwordsAndPolicies));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "1-3 a: abcde    ",
                    "1-3 b: cdefg    ",
                    "2-9 c: ccccccccc",
                },
                2
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "1-3 a: abcde    ",
                    "1-3 b: cdefg    ",
                    "2-9 c: ccccccccc",
                },
                1
            };
        }
    }
}