using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day11Tests
    { 
        [Theory]
        [MemberData(nameof(TestDataStateChange))]
        public void Part1StateChange(List<string> state, List<string> nextState)
        {
            var puzzleSolver = new Year2020Day11();
            Assert.Equal(ToCharArray(nextState), puzzleSolver.GetNextState(ToCharArray(state)));
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<string> seatingArrangement, int finalOccupiedSeats)
        {
            var puzzleSolver = new Year2020Day11();
            Assert.Equal(finalOccupiedSeats.ToString(), puzzleSolver.Part1(seatingArrangement));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> adapters, int adapterProduct)
        {
            var puzzleSolver = new Year2020Day11();
            Assert.Equal(adapterProduct.ToString(), puzzleSolver.Part2(adapters));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "L.LL.LL.LL",
                    "LLLLLLL.LL",
                    "L.L.L..L..",
                    "LLLL.LL.LL",
                    "L.LL.LL.LL",
                    "L.LLLLL.LL",
                    "..L.L.....",
                    "LLLLLLLLLL",
                    "L.LLLLLL.L",
                    "L.LLLLL.LL",
                },
                37
            };
        }

        public static IEnumerable<object[]> TestDataStateChange()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "L.LL.LL.LL",
                    "LLLLLLL.LL",
                    "L.L.L..L..",
                    "LLLL.LL.LL",
                    "L.LL.LL.LL",
                    "L.LLLLL.LL",
                    "..L.L.....",
                    "LLLLLLLLLL",
                    "L.LLLLLL.L",
                    "L.LLLLL.LL",
                },
                new List<string>()
                {
                    "#.##.##.##",
                    "#######.##",
                    "#.#.#..#..",
                    "####.##.##",
                    "#.##.##.##",
                    "#.#####.##",
                    "..#.#.....",
                    "##########",
                    "#.######.#",
                    "#.#####.##",
                }
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "#.##.##.##",
                    "#######.##",
                    "#.#.#..#..",
                    "####.##.##",
                    "#.##.##.##",
                    "#.#####.##",
                    "..#.#.....",
                    "##########",
                    "#.######.#",
                    "#.#####.##",
                },
                new List<string>()
                {
                    "#.LL.L#.##",
                    "#LLLLLL.L#",
                    "L.L.L..L..",
                    "#LLL.LL.L#",
                    "#.LL.LL.LL",
                    "#.LLLL#.##",
                    "..L.L.....",
                    "#LLLLLLLL#",
                    "#.LLLLLL.L",
                    "#.#LLLL.##",
                }
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "#.LL.L#.##",
                    "#LLLLLL.L#",
                    "L.L.L..L..",
                    "#LLL.LL.L#",
                    "#.LL.LL.LL",
                    "#.LLLL#.##",
                    "..L.L.....",
                    "#LLLLLLLL#",
                    "#.LLLLLL.L",
                    "#.#LLLL.##",
                },
                new List<string>()
                {
                    "#.##.L#.##",
                    "#L###LL.L#",
                    "L.#.#..#..",
                    "#L##.##.L#",
                    "#.##.LL.LL",
                    "#.###L#.##",
                    "..#.#.....",
                    "#L######L#",
                    "#.LL###L.L",
                    "#.#L###.##",
                },
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "#.##.L#.##",
                    "#L###LL.L#",
                    "L.#.#..#..",
                    "#L##.##.L#",
                    "#.##.LL.LL",
                    "#.###L#.##",
                    "..#.#.....",
                    "#L######L#",
                    "#.LL###L.L",
                    "#.#L###.##",
                },
                new List<string>()
                {
                    "#.#L.L#.##",
                    "#LLL#LL.L#",
                    "L.L.L..#..",
                    "#LLL.##.L#",
                    "#.LL.LL.LL",
                    "#.LL#L#.##",
                    "..L.L.....",
                    "#L#LLLL#L#",
                    "#.LLLLLL.L",
                    "#.#L#L#.##",
                },
            };

            yield return new object[]
            {
                new List<string>()
                {
                    "#.#L.L#.##",
                    "#LLL#LL.L#",
                    "L.L.L..#..",
                    "#LLL.##.L#",
                    "#.LL.LL.LL",
                    "#.LL#L#.##",
                    "..L.L.....",
                    "#L#LLLL#L#",
                    "#.LLLLLL.L",
                    "#.#L#L#.##",
                },
                new List<string>()
                {
                    "#.#L.L#.##",
                    "#LLL#LL.L#",
                    "L.#.L..#..",
                    "#L##.##.L#",
                    "#.#L.LL.LL",
                    "#.#L#L#.##",
                    "..L.L.....",
                    "#L#L##L#L#",
                    "#.LLLLLL.L",
                    "#.#L#L#.##",
                },
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "L.LL.LL.LL",
                    "LLLLLLL.LL",
                    "L.L.L..L..",
                    "LLLL.LL.LL",
                    "L.LL.LL.LL",
                    "L.LLLLL.LL",
                    "..L.L.....",
                    "LLLLLLLLLL",
                    "L.LLLLLL.L",
                    "L.LLLLL.LL",
                },
                26
            };
        }

        private List<char[]> ToCharArray(List<string> input)
        {
            var result = new List<char[]>();
            foreach (var line in input)
            {
                result.Add(line.ToCharArray());
            }

            return result;
        }
    }
}