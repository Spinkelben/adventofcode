using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day3Tests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<string> treePattern, int numberOfTreesHit)
        {
            var puzzleSolver = new Year2020Day3();
            Assert.Equal(numberOfTreesHit.ToString(), puzzleSolver.Part1(treePattern));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> treePattern, int numberOfTreesHit)
        {
            var puzzleSolver = new Year2020Day3();
            Assert.Equal(numberOfTreesHit.ToString(), puzzleSolver.Part2(treePattern));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "..##.......",
                    "#...#...#..",
                    ".#....#..#.",
                    "..#.#...#.#",
                    ".#...##..#.",
                    "..#.##.....",
                    ".#.#.#....#",
                    ".#........#",
                    "#.##...#...",
                    "#...##....#",
                    ".#..#...#.#",
                },
                7
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "..##.......",
                    "#...#...#..",
                    ".#....#..#.",
                    "..#.#...#.#",
                    ".#...##..#.",
                    "..#.##.....",
                    ".#.#.#....#",
                    ".#........#",
                    "#.##...#...",
                    "#...##....#",
                    ".#..#...#.#",
                },
                336
            };
        }
    }
}